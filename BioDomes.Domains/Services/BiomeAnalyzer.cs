using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Services;

public class BiomeAnalyzer(
    IFoodChainRepository foodChainRepo,
    ISpeciesRepository speciesRepo,
    IBiomeRepository biomeRepo,
    IBiomeManagementRepository managementRepo) : IBiomeAnalyzer
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
    public async Task<List<Recommendation<Specie>>> RecommendationToAddForBiome(string biomeSlug, string? userId,
        bool isAdmin, FoodChainGraph biomeGraph)
    {
        var globalGraphResult = await foodChainRepo.GetFullGraphAsync(userId, isAdmin);
        var allSpeciesResult = await speciesRepo.FindAllAsDictAsync(userId, isAdmin);
        var biomeResult = await biomeRepo.FindComplexAsync(biomeSlug, userId, isAdmin);

        if (allSpeciesResult.Failed
            || globalGraphResult.Failed
            || biomeResult.Failed)
        {
            return [];
        }

        var globalGraph = globalGraphResult.Value;
        var allSpecies = allSpeciesResult.Value;
        var candidates = allSpecies!.Values
            .ToList();

        return biomeGraph.Nodes.Count == 0
            ? HandleEmptyBiome(globalGraph!, candidates, biomeSlug)
            : HandlePopulatedBiome(biomeGraph, globalGraph!, allSpecies, candidates, biomeResult.Value!);
    }

    public async Task<BiomeAnalysis?> AnalyzeBiome(string biomeSlug, string? userId, bool isAdmin)
    {
        var graphResult = await foodChainRepo.GetFullGraphForBiomeAsync(biomeSlug, userId, isAdmin);
        var allSpeciesResult = await speciesRepo.FindAllAsDictAsync(userId, isAdmin);
        var biomeResult = await biomeRepo.FindComplexAsync(biomeSlug, userId, isAdmin);

        if (graphResult.Failed
            || allSpeciesResult.Failed
            || biomeResult.Failed)
        {
            return null;
        }

        var graph = graphResult.Value!;
        var stats = BiomeFoodChainStabilityAnalyzer.Analyze(biomeResult.Value!, graph, allSpeciesResult.Value!);

        return new BiomeAnalysis { Graph = graph, Stats = stats };
    }

    public async Task<Dictionary<ComplexBiome, BiomeStats>> AnalyzeAllBiomes(string? userId, bool isAdmin)
    {
        var biomesResult = await biomeRepo.GetAllComplexAsync(userId, isAdmin);
        var allSpeciesResult = await speciesRepo.FindAllAsDictAsync(userId, isAdmin);

        if (biomesResult.Failed || biomesResult.Value is null
                                || allSpeciesResult.Failed || allSpeciesResult.Value is null)
        {
            return [];
        }

        var allSpecies = allSpeciesResult.Value;
        var result = new Dictionary<ComplexBiome, BiomeStats>();

        foreach (var biome in biomesResult.Value)
        {
            var graphResult = await foodChainRepo.GetFullGraphForBiomeAsync(biome.Slug, userId, isAdmin);

            var stats = graphResult.Failed || graphResult.Value is null
                ? new BiomeStats { Biome = biome, TotalsSpecies = 0, AllSpeciesFed = true }
                : BiomeFoodChainStabilityAnalyzer.Analyze(biome, graphResult.Value, allSpecies);

            result[biome] = stats;
        }

        return result;
    }

    public async Task<GlobalStatistic> GlobalAnalyze(string? userId, bool isAdmin)
    {
        var allResult = await biomeRepo.GetAllComplexAsync(userId, isAdmin);
        if (allResult.Failed) return new GlobalStatistic { All = [] };

        var ana = new GlobalStatistic
        {
            All = allResult.Value!
        };

        return ana;
    }

    public async Task<Result> PerformAction(string actionId, string? userId, bool isAdmin)
    {
        var (action, args) = ParseActionId(actionId);

        if (action == "add-biome-specie")
        {
            return await managementRepo.AddSpecieToBiome(new SpecieToBiomeRequest
            {
                SpecieSlug = args["specie"],
                Count = int.Parse(args["count"])
            }, args["biome"], userId, isAdmin);
        }

        return Result.NotFound("Your request could not be resolved");
    }

    private List<Recommendation<Specie>> HandleEmptyBiome(FoodChainGraph globalGraph, List<Specie> candidates,
        string biomeSlug)
    {
        var relationCounts = globalGraph.CountRelations();

        return candidates.Select(sp =>
        {
            var count = relationCounts.GetValueOrDefault(sp, 0);
            var isExcellent = count >= 3;

            return new Recommendation<Specie>
            {
                ActionId = BuildActionId(sp.Slug, biomeSlug, 1),
                Involved = sp,
                Label = $"Ajouter un {sp.Classification.Label}",
                Message = isExcellent
                    ? $"Ajoutez '{sp.Name}' pour bien démarrer"
                    : $"Ajoutez '{sp.Name}' pour commencer",
                Type = isExcellent ? RecommendationType.Excellent : RecommendationType.Good
            };
        }).ToList();
    }

    private List<Recommendation<Specie>> HandlePopulatedBiome(FoodChainGraph biomeGraph,
        FoodChainGraph globalGraph,
        Dictionary<string, Specie> allSpecies,
        List<Specie> candidates,
        ComplexBiome biome)
    {
        var currentStats = BiomeFoodChainStabilityAnalyzer.Analyze(biome, biomeGraph, allSpecies);
        var starvedIds = currentStats.StarvedSpecies.Select(n => n.Specie).ToHashSet();
        var underfedIds = currentStats.UnderfedSpecies.Select(n => n.Specie).ToHashSet();
        var needsFoodIds = starvedIds.Union(underfedIds).ToHashSet();

        var results = new List<Recommendation<Specie>>();

        foreach (var candidate in candidates)
        {
            // what the candidate eats in the biome
            var candidatePreyIdsInBiome = globalGraph.Edges
                .Where(e => e.PredatorSlug == candidate.Slug)
                .Select(e => e.PreySlug)
                .Where(id => biomeGraph.Nodes.Any(n => n.Specie.Slug == id))
                .ToHashSet();

            // what eats the candidate in the biome
            var candidatePredatorIdsInBiome = globalGraph.Edges
                .Where(e => e.PreySlug == candidate.Slug)
                .Select(e => e.PredatorSlug)
                .Where(slug => biomeGraph.Nodes.Any(n => n.Specie.Slug == slug))
                .ToHashSet();

            var hasFood = candidatePreyIdsInBiome.Count > 0;
            var hasRole = candidatePredatorIdsInBiome.Count > 0; // something eats it

            // candidate is prey of a starved/underfed species
            var relievesHunger = biomeGraph.Nodes
                .Where(n => needsFoodIds.Contains(n.Specie))
                .Any(n => globalGraph.Edges.Any(e =>
                    e.PredatorSlug == n.Specie.Slug && e.PreySlug == candidate.Slug));

            var introducesLoop = BiomeFoodChainStabilityAnalyzer.IntroducesLoop(biomeGraph, candidate, globalGraph);
            var stableAfterAdd =
                BiomeFoodChainStabilityAnalyzer.IsStableAfterAdding(biome, biomeGraph, candidate, globalGraph,
                    allSpecies);

            RecommendationType type;
            string message;

            if (relievesHunger && stableAfterAdd && !introducesLoop)
            {
                type = RecommendationType.Excellent;
                var hungryNames = biomeGraph.Nodes
                    .Where(n => needsFoodIds.Contains(n.Specie)
                                && globalGraph.Edges.Any(e =>
                                    e.PredatorSlug == n.Specie.Slug && e.PreySlug == candidate.Slug))
                    .Select(n => n.Specie.Name);
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
                ActionId = BuildActionId(candidate.Slug, biome.Slug, 1),
                Involved = candidate,
                Label = $"Ajouter '{candidate.Name}'",
                Message = message,
                Type = type
            });
        }

        return results;
    }

    private static string BuildActionId(string specieSlug, string biomeSlug, int count)
        => $"add-biome-specie specie:{specieSlug} count:{count} biome:{biomeSlug}";

    private static (string action, Dictionary<string, string> args) ParseActionId(string actionId)
    {
        var parts = actionId.Split(' ');
        var action = parts[0];
        var args = parts[1..]
            .Select(p => p.Split(':'))
            .Where(p => p.Length == 2)
            .ToDictionary(p => p[0], p => p[1]);

        return (action, args);
    }
}