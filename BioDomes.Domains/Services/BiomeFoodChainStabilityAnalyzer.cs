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
    public static BiomeStats Analyze(FoodChainGraph graph, Dictionary<int, Specie> allSpecies, int biomeId)
    {
        var starved = new List<FoodChainNode>();
        var underfed = new List<FoodChainNode>();

        var predatorIdsWithPrey = graph.Edges
            .Select(e => e.PredatorId)
            .ToHashSet();

        var predatorsUnderfed = ComputeUnderfedPredators(graph, allSpecies);

        foreach (var node in graph.Nodes)
        {
            if (!node.Classification.RequireFood) continue;

            if (!predatorIdsWithPrey.Contains(node.SpecieId))
                starved.Add(node);
            else if (predatorsUnderfed.Contains(node.SpecieId))
                underfed.Add(node);
        }

        return new BiomeStats
        {
            BiomeId = biomeId,
            TotalsSpecies = allSpecies.Count,
            AllSpeciesFed = starved.Count == 0 && underfed.Count == 0,
            StarvedSpecies = starved,
            UnderfedSpecies = underfed,
            ProblemCount = starved
                .Select(s => s.SpecieId)
                .Concat(underfed.Select(s => s.SpecieId))
                .Distinct()
                .Count(),
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
    /// Simule l'ajout d'un candidat dans le graphe et retourne si le biome reste stable.
    /// </summary>
    public static bool IsStableAfterAdding(
        int biomeId,
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

        var previouslyUnderfedIds = Analyze(currentGraph, allSpecies, biomeId)
            .UnderfedSpecies
            .Select(n => n.SpecieId)
            .ToHashSet();

        var newStats = Analyze(simulatedGraph, allSpecies, biomeId);
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