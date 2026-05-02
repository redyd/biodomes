using BioDomes.Domains.Core;

namespace BioDomes.Domains.Services;

/// <summary>
/// Calcule la stabilité alimentaire d'un graphe biome.
/// Règle : chaque proie doit couvrir la demande complète de chaque prédateur qui la mange.
/// Si A->B et C->B, B doit satisfaire countA*weightA ET countC*weightC indépendamment.
/// Une proie est stable si sa biomasse >= somme de toutes les demandes qui la ciblent.
/// </summary>
public static class BiomeFoodChainStabilityAnalyzer
{
    /// <summary>
    /// Calcule les stats de stabilité du biome.
    /// </summary>
    public static BiomeStats Analyze(FoodChainGraph graph, Dictionary<int, Specie> allSpecies)
    {
        var starved = new List<FoodChainNode>();
        var underfed = new List<FoodChainNode>();

        var predatorIdsWithPrey = graph.Edges
            .Select(e => e.PredatorId)
            .ToHashSet();

        foreach (var node in graph.Nodes)
        {
            if (!node.Classification.RequireFood) continue;
            if (!predatorIdsWithPrey.Contains(node.SpecieId))
                starved.Add(node);
        }

        var underfedIds = new HashSet<int>();

        // predators ids where preys mass is not enough
        var predatorsUnderfed = ComputeUnderfedPredators(graph, allSpecies);

        foreach (var node in TopologicalSort(graph))
        {
            if (!node.Classification.RequireFood) continue;

            // case 1
            var directlyUnderfed = predatorsUnderfed.Contains(node.SpecieId);

            // case 2
            var preyIsCompromised = graph.Edges
                .Where(e => e.PredatorId == node.SpecieId)
                .Select(e => e.PreyId)
                .Any(underfedIds.Contains);

            if (!directlyUnderfed && !preyIsCompromised) continue;

            underfedIds.Add(node.SpecieId);
            underfed.Add(node);
        }

        return new BiomeStats
        {
            AllSpeciesFed = starved.Count == 0 && underfed.Count == 0,
            StarvedSpecies = starved,
            UnderfedSpecies = underfed,
            ProblemCount = starved
                .Select(s => s.SpecieId)
                .Concat(underfed.Select(s => s.SpecieId))
                .Distinct()
                .Count()
        };
    }

    /// <summary>
    /// Retourne les ids des prédateurs dont la biomasse combinée de toutes leurs proies
    /// dans le biome est insuffisante pour couvrir leur propre demande (count * weight).
    /// </summary>
    private static HashSet<int> ComputeUnderfedPredators(
        FoodChainGraph graph,
        Dictionary<int, Specie> allSpecies)
    {
        var underfedPredators = new HashSet<int>();

        var edgesByPredator = graph.Edges
            .GroupBy(e => e.PredatorId)
            .ToDictionary(g => g.Key, g => g.Select(e => e.PreyId).ToList());

        foreach (var (predatorId, preyIds) in edgesByPredator)
        {
            if (!allSpecies.TryGetValue(predatorId, out var predatorSpecie)) continue;

            var predatorNode = graph.Nodes.FirstOrDefault(n => n.SpecieId == predatorId);
            if (predatorNode is null) continue;

            var demand = (double)(predatorNode.Count * predatorSpecie.Weight);

            // compute biomass of all preys in biome
            var totalPreyBiomass = preyIds
                .Select(preyId => (
                    node: graph.Nodes.FirstOrDefault(n => n.SpecieId == preyId),
                    specie: allSpecies.GetValueOrDefault(preyId)
                ))
                .Where(p => p.node is not null && p.specie is not null)
                .Sum(p => (double)(p.node!.Count * p.specie!.Weight));

            if (totalPreyBiomass < demand)
                underfedPredators.Add(predatorId);
        }

        return underfedPredators;
    }

    /// <summary>
    /// Tri topologique de Kahn - retourne les nœuds du graphe
    /// des proies vers les prédateurs (sources = proies pures, sans prédateur).
    /// Les cycles éventuels sont ignorés et les nœuds restants sont ajoutés en fin.
    /// </summary>
    private static List<FoodChainNode> TopologicalSort(FoodChainGraph graph)
    {
        // GroupBy au lieu de ToDictionary → survit aux SpecieId dupliqués
        var nodeById = graph.Nodes
            .GroupBy(n => n.SpecieId)
            .ToDictionary(g => g.Key, g => g.First());

        var inDegree = nodeById.Keys.ToDictionary(id => id, _ => 0);

        foreach (var edge in graph.Edges)
        {
            if (inDegree.ContainsKey(edge.PredatorId))
                inDegree[edge.PredatorId]++;
        }

        var queue = new Queue<FoodChainNode>(
            nodeById.Values.Where(n => inDegree[n.SpecieId] == 0));
        var sorted = new List<FoodChainNode>();

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            sorted.Add(node);

            foreach (var edge in graph.Edges.Where(e => e.PreyId == node.SpecieId))
            {
                if (!inDegree.ContainsKey(edge.PredatorId)) continue;
                inDegree[edge.PredatorId]--;
                if (inDegree[edge.PredatorId] == 0)
                    queue.Enqueue(nodeById[edge.PredatorId]);
            }
        }

        var visited = sorted.Select(n => n.SpecieId).ToHashSet();
        sorted.AddRange(nodeById.Values.Where(n => !visited.Contains(n.SpecieId)));

        return sorted;
    }

    /// <summary>
    /// Simule l'ajout d'un candidat dans le graphe et retourne si le biome reste stable.
    /// </summary>
    public static bool IsStableAfterAdding(
        FoodChainGraph currentGraph,
        Specie candidate,
        FoodChainGraph globalGraph,
        Dictionary<int, Specie> allSpecies)
    {
        var candidatePreyIds = globalGraph.Edges
            .Where(e => e.PredatorId == candidate.Id)
            .Select(e => e.PreyId)
            .Where(preyId => currentGraph.Nodes.Any(n => n.SpecieId == preyId))
            .ToList();

        var simulatedEdges = currentGraph.Edges
            .Concat(candidatePreyIds.Select(preyId => new FoodChainEdge(candidate.Id, preyId)))
            .ToList();

        var candidateNode = new FoodChainNode(
            candidate.Id,
            candidate.Name,
            candidate.Diet,
            candidate.Classification,
            count: 1
        );
        var simulatedNodes = currentGraph.Nodes
            .Where(n => n.SpecieId != candidate.Id)
            .Concat([candidateNode])
            .ToList();
        var simulatedGraph = new FoodChainGraph(simulatedNodes, simulatedEdges);

        var previouslyUnderfedIds = Analyze(currentGraph, allSpecies)
            .UnderfedSpecies
            .Select(n => n.SpecieId)
            .ToHashSet();

        var newStats = Analyze(simulatedGraph, allSpecies);
        var newUnderfed = newStats.UnderfedSpecies
            .Any(n => !previouslyUnderfedIds.Contains(n.SpecieId));

        return !newUnderfed;
    }

    /// <summary>
    /// Vérifie si l'ajout d'un candidat introduirait une boucle (A->B et B->A).
    /// </summary>
    public static bool IntroducesLoop(FoodChainGraph currentGraph, Specie candidate, FoodChainGraph globalGraph)
    {
        var candidatePreyIds = globalGraph.Edges
            .Where(e => e.PredatorId == candidate.Id)
            .Select(e => e.PreyId)
            .ToHashSet();

        var predatorsOfCandidate = currentGraph.Edges
            .Where(e => e.PreyId == candidate.Id)
            .Select(e => e.PredatorId)
            .ToHashSet();

        return predatorsOfCandidate.Any(candidatePreyIds.Contains);
    }
}