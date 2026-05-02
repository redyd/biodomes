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
    /// Algorithme de recommendation d'espèces à ajouter.
    /// Dans le cas le plus basique où le biome est vide, on va rechercher dans la chaine alimentaire établie les
    /// espèces avec le plus de lien et les favoriser par rapport aux autres (3 liens ou plus -> Good, le reste Ok)
    /// Sinon, on va classer les espèces comme ceci:
    /// - En premier, on va récupérer les espèces qui n'ont pas de quoi se nourrir, et leur trouver des espèces à manger
    /// (attention à ne pas créer des boucles, par ex. A -> B et B -> A)
    /// - Ensuite, ce seront les types d'espèces qui ne sont pas encore dans le biome (par exemple "Mammifère",
    /// et qui auraient de quoi manger
    /// </summary>
    /// <returns></returns>
    public async Task<List<Recommendation<Specie>>> RecommendationToAddForBiome(string? userId, bool isAdmin,
        int biomeId, FoodChainGraph biomeGraph)
    {
        var results = new List<Recommendation<Specie>>();
        var globalGraphResult = await foodChainRepo.GetFullGraphAsync(userId, isAdmin);
        var allSpeciesResult =
            await speciesRepo.FindAllAsDictAsync(userId, isAdmin); // get all public and available species

        if (!allSpeciesResult.IsSuccess
            || allSpeciesResult.Value is null
            || !globalGraphResult.IsSuccess
            || globalGraphResult.Value is null)
        {
            return [];
        }
        var globalGraph = globalGraphResult.Value!;

        var allSpecies = allSpeciesResult.Value!;
        var speciesIdAlreadyInBiomes = biomeGraph.Nodes.Select(n => n.SpecieId).ToHashSet();

        // basic case
        if (biomeGraph.Nodes.Count == 0)
        {
            return HandleEmptyBiome(globalGraph, allSpecies, results, biomeId);
        }

        // handling the case where a specie does not have something to eat
        var withoutPreys = biomeGraph.NoPreySpecies();
        
        foreach (var withoutPrey in withoutPreys)
        {
            var specie = allSpecies[withoutPrey.SpecieId];
            
            // find every species than it can eat, except those already in biome
            var candidates = allSpecies
                .Values
                .Where(sp => !speciesIdAlreadyInBiomes.Contains(sp.Id))
                .Where(sp => specie.Diet.CanEat(sp.Classification))
                .Where(sp => sp.Id != specie.Id)
                .ToList();
            
            // exclude boucle
            var predatorIsPreyOf = biomeGraph.Edges
                .Where(e => e.PreyId == withoutPrey.SpecieId)
                .Select(e => e.PredatorId)
                .ToHashSet();
            candidates = candidates
                .Where(sp => !predatorIsPreyOf.Contains(sp.Id))
                .ToList();
            
            if (candidates.Count == 0) continue;

            // make a set of the species id in the biodome
            var biomeSpecieIds = biomeGraph.Nodes.Select(n => n.SpecieId).ToHashSet();

            // score each candidate
            foreach (var candidate in candidates)
            {
                var candidateHasFoodInBiome = biomeGraph.Edges
                    .Any(e => e.PredatorId == candidate.Id && biomeSpecieIds.Contains(e.PreyId));

                var relationsAdded = globalGraph.Edges
                    .Count(e => e.PredatorId == candidate.Id && biomeSpecieIds.Contains(e.PreyId)
                                || e.PreyId == candidate.Id && biomeSpecieIds.Contains(e.PredatorId));

                var type = relationsAdded >= 3
                    ? RecommendationType.Excellent
                    : candidateHasFoodInBiome
                        ? RecommendationType.Good
                        : RecommendationType.Ok;
                
                results.Add(new Recommendation<Specie>
                {
                    ActionId = $"add-biome-specie specie:{candidate.Id} count:1 biome:{biomeId}",
                    Involved = candidate,
                    Label = $"Proie pour {specie.Name}",
                    Message = $"Ajoutez '{candidate.Name}' pour nourrir {specie.Name}",
                    Type = type
                });
            }
        }

        return results;
    }

    public async Task PerformAction(string actionId)
    {
        var (action, args) = ParseActionId(actionId);

        if (action == "add-biome-specie")
        {
            await biomeRepo.AddSpecie(new AddSpecieToBiomeRequest
            {
                SpecieId = args["specie"],
                Count = args["count"]
            }, args["biome"]);
        }
    }

    private List<Recommendation<Specie>> HandleEmptyBiome(
        FoodChainGraph globalGraph,
        Dictionary<int, Specie> allSpecies,
        List<Recommendation<Specie>> results,
        int biomeId)
    {
        foreach (var (id, count) in globalGraph.CountRelations())
        {
            var specie = allSpecies[id];
            var isExcellent = count >= 3;

            var rec = new Recommendation<Specie>
            {
                ActionId = $"add-biome-specie specie:{id} count:1 biome:{biomeId}",
                Involved = specie,
                Label = $"Ajouter un {specie.Classification.Label}",
                Message = isExcellent
                    ? $"Ajoutez '{specie.Name}' pour bien démarrer"
                    : $"Ajoutez '{specie.Name}' pour commencer",
                Type = isExcellent ? RecommendationType.Excellent : RecommendationType.Good
            };

            results.Add(rec);
        }

        return results;
    }
    
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