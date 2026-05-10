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
    public static BiomeStats Analyze(ComplexBiome biome, FoodChainGraph graph, Dictionary<string, Specie> allSpecies)
    {
        var starved = new List<FoodChainNode>();
        var underfed = new List<FoodChainNode>();

        var predatorIdsWithPrey = graph.Edges
            .Select(e => e.PredatorSlug)
            .ToHashSet();

        var predatorsUnderfed = ComputeUnderfedPredators(graph, allSpecies);

        foreach (var node in graph.Nodes)
        {
            if (!node.Specie.Classification.RequireFood) continue;

            if (!predatorIdsWithPrey.Contains(node.Specie.Slug))
                starved.Add(node);
            else if (predatorsUnderfed.Contains(node.Specie.Slug))
                underfed.Add(node);
        }

        return new BiomeStats
        {
            Biome = biome,
            TotalsSpecies = allSpecies.Count,
            AllSpeciesFed = starved.Count == 0 && underfed.Count == 0,
            StarvedSpecies = starved,
            UnderfedSpecies = underfed,
            ProblemCount = starved
                .Select(s => s.Specie)
                .Concat(underfed.Select(s => s.Specie))
                .Distinct()
                .Count(),
        };
    }

    /// <summary>
    /// Retourne les ids des prédateurs dont la biomasse combinée de toutes leurs proies
    /// dans le biome est insuffisante pour couvrir leur propre demande (count * weight).
    /// </summary>
    private static HashSet<string> ComputeUnderfedPredators(FoodChainGraph graph, Dictionary<string, Specie> allSpecies)
    {
        var underfedPredators = new HashSet<string>();

        var edgesByPredator = graph.Edges
            .GroupBy(e => e.PredatorSlug)
            .ToDictionary(g => g.Key, g => g.Select(e => e.PreySlug).ToList());

        foreach (var (predatorSlug, preySlugs) in edgesByPredator)
        {
            if (!allSpecies.TryGetValue(predatorSlug, out var predatorSpecie)) continue;

            var predatorNode = graph.Nodes.FirstOrDefault(n => n.Specie.Slug == predatorSlug);
            if (predatorNode is null) continue;

            var demand = (double)(predatorNode.Count * predatorSpecie.Weight);

            // compute biomass of all preys in biome
            var totalPreyBiomass = preySlugs
                .Select(preySlug => (
                    node: graph.Nodes.FirstOrDefault(n => n.Specie.Slug == preySlug),
                    specie: allSpecies.GetValueOrDefault(preySlug)
                ))
                .Where(p => p.node is not null && p.specie is not null)
                .Sum(p => (double)(p.node!.Count * p.specie!.Weight));

            if (totalPreyBiomass < demand)
            {
                underfedPredators.Add(predatorSlug);
            }
        }

        return underfedPredators;
    }

    /// <summary>
    /// Simule l'ajout d'un candidat dans le graphe et retourne si le biome reste stable.
    /// </summary>
    public static bool IsStableAfterAdding(ComplexBiome biome,
        FoodChainGraph currentGraph,
        Specie candidate,
        FoodChainGraph globalGraph,
        Dictionary<string, Specie> allSpecies)
    {
        var candidatePreyIds = globalGraph.Edges
            .Where(e => e.PredatorSlug == candidate.Slug)
            .Select(e => e.PreySlug)
            .Where(preySlug => currentGraph.Nodes.Any(n => n.Specie.Slug == preySlug))
            .ToList();

        var simulatedEdges = currentGraph.Edges
            .Concat(candidatePreyIds.Select(preySlug => new FoodChainEdge(candidate.Slug, preySlug)))
            .ToList();

        var candidateNode = new FoodChainNode(specie: candidate, count: 1);
        var simulatedNodes = currentGraph.Nodes
            .Where(n => n.Specie.Slug != candidate.Slug)
            .Concat([candidateNode])
            .ToList();
        var simulatedGraph = new FoodChainGraph(simulatedNodes, simulatedEdges);

        var previouslyUnderfedIds = Analyze(biome, currentGraph, allSpecies)
            .UnderfedSpecies
            .Select(n => n.Specie)
            .ToHashSet();

        var newStats = Analyze(biome, simulatedGraph, allSpecies);
        var newUnderfed = newStats.UnderfedSpecies
            .Any(n => !previouslyUnderfedIds.Contains(n.Specie));

        return !newUnderfed;
    }

    /// <summary>
    /// Vérifie si l'ajout d'un candidat introduirait une boucle (A->B et B->A).
    /// </summary>
    public static bool IntroducesLoop(FoodChainGraph currentGraph, Specie candidate, FoodChainGraph globalGraph)
    {
        var candidatePreyIds = globalGraph.Edges
            .Where(e => e.PredatorSlug == candidate.Slug)
            .Select(e => e.PreySlug)
            .ToHashSet();

        var predatorsOfCandidate = currentGraph.Edges
            .Where(e => e.PreySlug == candidate.Slug)
            .Select(e => e.PredatorSlug)
            .ToHashSet();

        return predatorsOfCandidate.Any(candidatePreyIds.Contains);
    }
}