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
    /// <param name="isAdmin"></param>
    /// <param name="biomeId"></param>
    /// <param name="biomeGraph"></param>
    /// <param name="userId"></param>
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

        // basic case
        if (biomeGraph.Nodes.Count == 0)
        {
            return HandleEmptyBiome(globalGraph, allSpecies, results, biomeId);
        }

        // complex case
        var relations = biomeGraph.CountRelations();

        // handling the case where a specie does not have something to eat
        var withoutPreys = biomeGraph.NoPreySpecies();
        foreach (var withoutPrey in withoutPreys)
        {
            var specie = allSpecies[withoutPrey.SpecieId];

            // find every species than it can eat
            var potential = allSpecies
                .Values
                .Select(sp => sp.Classification.EatenByDiets.Any(diet => diet == specie.Diet))
                .ToList();

            if (potential.Count == 0) continue;

            // calculate how much the specie would need to
            var totalWeight = specie.Weight * relations[specie.Id];

            // to select which species will be added, we create a 
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
                Type = isExcellent ? RecommendationType.Excellent : RecommendationType.Ok
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