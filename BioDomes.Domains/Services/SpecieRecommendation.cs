using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Services;

public class SpecieRecommendation(
    IFoodChainRepository foodChainRepo,
    ISpeciesRepository speciesRepo,
    IBiomeRepository biomeRepo) : ISpecieRecommendation
{
    /// <summary>
    /// Retourne toutes les espèces disponibles classées par pertinence pour le biome.
    ///
    /// Biome vide :
    ///   Excellent  → 3+ relations dans le graphe global
    ///   Good       → reste
    ///
    /// Biome peuplé :
    ///   Excellent  → nourrit une espèce affamée/sous-alimentée ET stabilité conservée
    ///   Good       → pas de boucle, ne prive personne, stabilité conservée
    ///   Ok         → a de quoi manger ou peut être mangée, mais déséquilibre mineur
    ///   Bad        → rien à manger ET rien ne la mange dans le biome
    /// </summary>
    public async Task<List<Recommendation<Specie>>> RecommendationToAddForBiome(
        string? userId, bool isAdmin, int biomeId, FoodChainGraph biomeGraph)
    {
        var globalGraphResult = await foodChainRepo.GetFullGraphAsync(userId, isAdmin);
        var allSpeciesResult = await speciesRepo.FindAllAsDictAsync(userId, isAdmin);

        if (allSpeciesResult.Failed
            || allSpeciesResult.Value is null
            || globalGraphResult.Failed
            || globalGraphResult.Value is null)
        {
            return [];
        }

        var globalGraph = globalGraphResult.Value;
        var allSpecies = allSpeciesResult.Value;
        var candidates = allSpecies.Values
            .ToList();

        return biomeGraph.Nodes.Count == 0
            ? HandleEmptyBiome(globalGraph, candidates, biomeId)
            : HandlePopulatedBiome(biomeGraph, globalGraph, allSpecies, candidates, biomeId);
    }

    public async Task<BiomeAnalysis> AnalyzeBiome(string? userId, bool isAdmin, int biomeId)
    {
        var graphResult = await foodChainRepo.GetFullGraphForBiomeAsync(biomeId, userId, isAdmin);
        var allSpeciesResult = await speciesRepo.FindAllAsDictAsync(userId, isAdmin);

        if (graphResult.Failed
            || graphResult.Value is null
            || allSpeciesResult.Failed
            || allSpeciesResult.Value is null)
        {
            return new BiomeAnalysis
            {
                Graph = new FoodChainGraph([], []),
                Stats = new BiomeStats { AllSpeciesFed = true }
            };
        }

        var graph = graphResult.Value;
        var stats = BiomeFoodChainStabilityAnalyzer.Analyze(graph, allSpeciesResult.Value);

        return new BiomeAnalysis { Graph = graph, Stats = stats };
    }

    public async Task<Result> PerformAction(string actionId, string? userId, bool isAdmin)
    {
        var (action, args) = ParseActionId(actionId);

        if (action == "add-biome-specie")
        {
            return await biomeRepo.AddSpecie(new SpecieToBiomeRequest
            {
                SpecieId = args["specie"],
                Count = args["count"]
            }, args["biome"], userId, isAdmin);
        }

        return Result.NotFound("Your request could not be resolved");
    }

    private List<Recommendation<Specie>> HandleEmptyBiome(FoodChainGraph globalGraph, List<Specie> candidates,
        int biomeId)
    {
        var relationCounts = globalGraph.CountRelations();

        return candidates.Select(sp =>
        {
            var count = relationCounts.GetValueOrDefault(sp.Id, 0);
            var isExcellent = count >= 3;

            return new Recommendation<Specie>
            {
                ActionId = BuildActionId(sp.Id, 1, biomeId),
                Involved = sp,
                Label = $"Ajouter un {sp.Classification.Label}",
                Message = isExcellent
                    ? $"Ajoutez '{sp.Name}' pour bien démarrer"
                    : $"Ajoutez '{sp.Name}' pour commencer",
                Type = isExcellent ? RecommendationType.Excellent : RecommendationType.Good
            };
        }).ToList();
    }

    private List<Recommendation<Specie>> HandlePopulatedBiome(
        FoodChainGraph biomeGraph,
        FoodChainGraph globalGraph,
        Dictionary<int, Specie> allSpecies,
        List<Specie> candidates,
        int biomeId)
    {
        var currentStats = BiomeFoodChainStabilityAnalyzer.Analyze(biomeGraph, allSpecies);
        var starvedIds = currentStats.StarvedSpecies.Select(n => n.SpecieId).ToHashSet();
        var underfedIds = currentStats.UnderfedSpecies.Select(n => n.SpecieId).ToHashSet();
        var needsFoodIds = starvedIds.Union(underfedIds).ToHashSet();

        var results = new List<Recommendation<Specie>>();

        foreach (var candidate in candidates)
        {
            // what the candidate eats in the biome
            var candidatePreyIdsInBiome = globalGraph.Edges
                .Where(e => e.PredatorId == candidate.Id)
                .Select(e => e.PreyId)
                .Where(id => biomeGraph.Nodes.Any(n => n.SpecieId == id))
                .ToHashSet();

            // what eats the candidate in the biome
            var candidatePredatorIdsInBiome = globalGraph.Edges
                .Where(e => e.PreyId == candidate.Id)
                .Select(e => e.PredatorId)
                .Where(id => biomeGraph.Nodes.Any(n => n.SpecieId == id))
                .ToHashSet();

            var hasFood = candidatePreyIdsInBiome.Count > 0;
            var hasRole = candidatePredatorIdsInBiome.Count > 0; // something eats it

            // candidate is prey of a starved/underfed species
            var relievesHunger = biomeGraph.Nodes
                .Where(n => needsFoodIds.Contains(n.SpecieId))
                .Any(n => globalGraph.Edges.Any(e =>
                    e.PredatorId == n.SpecieId && e.PreyId == candidate.Id));

            var introducesLoop = BiomeFoodChainStabilityAnalyzer.IntroducesLoop(biomeGraph, candidate, globalGraph);
            var stableAfterAdd =
                BiomeFoodChainStabilityAnalyzer.IsStableAfterAdding(biomeGraph, candidate, globalGraph, allSpecies);

            RecommendationType type;
            string message;

            if (relievesHunger && stableAfterAdd && !introducesLoop)
            {
                type = RecommendationType.Excellent;
                var hungryNames = biomeGraph.Nodes
                    .Where(n => needsFoodIds.Contains(n.SpecieId)
                                && globalGraph.Edges.Any(e => e.PredatorId == n.SpecieId && e.PreyId == candidate.Id))
                    .Select(n => n.Name);
                message = $"Nourrit : {string.Join(", ", hungryNames)}";
            }
            else if (hasFood && !introducesLoop && stableAfterAdd)
            {
                type = RecommendationType.Good;
                message = "N'impacte pas l'équilibre du biome";
            }
            else if (hasFood || hasRole)
            {
                type = RecommendationType.Ok;
                message = hasFood
                    ? "A de quoi manger mais peut déséquilibrer le biome"
                    : "Peut être mangée mais n'a rien à manger";
            }
            else
            {
                type = RecommendationType.Bad;
                message = "Aucune relation alimentaire avec le biome actuel";
            }

            results.Add(new Recommendation<Specie>
            {
                ActionId = BuildActionId(candidate.Id, 1, biomeId),
                Involved = candidate,
                Label = $"Ajouter '{candidate.Name}'",
                Message = message,
                Type = type
            });
        }

        return results;
    }

    private static string BuildActionId(int specieId, int count, int biomeId)
        => $"add-biome-specie specie:{specieId} count:{count} biome:{biomeId}";

    private static (string action, Dictionary<string, int> args) ParseActionId(string actionId)
    {
        var parts = actionId.Split(' ');
        var action = parts[0];
        var args = parts[1..]
            .Select(p => p.Split(':'))
            .Where(p => p.Length == 2 && int.TryParse(p[1], out _))
            .ToDictionary(p => p[0], p => int.Parse(p[1]));

        return (action, args);
    }
}