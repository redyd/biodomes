using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Core;

public class SpecieRecommendation(
    IFoodChainRepository foodChainRepo,
    ISpeciesRepository speciesRepo,
    IBiomeRepository biomeRepo)
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
    /// <param name="biomeId"></param>
    /// <returns></returns>
    public async Task<List<Recommendation<Specie>>> RecommendationToAddForBiome(int biomeId,
        FoodChainGraph biomeGraph)
    {
        var results = new List<Recommendation<Specie>>();
        var globalGraph = await foodChainRepo.GetFullGraphAsync();
        var allSpeciesResult =
            await speciesRepo.FindAllAsDictAsync(null, false); // get all public and available species

        if (!allSpeciesResult.IsSuccess || allSpeciesResult.Value is null)
        {
            return [];
        }

        var allSpecies = allSpeciesResult.Value!;
        // basic case

        if (biomeGraph.Nodes.Count == 0)
        {
            return HandleEmptyBiome(globalGraph, allSpecies, results, biomeId);
        }


        return results;
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
            var fun = async () => await biomeRepo.AddSpecie(new AddSpecieToBiomeRequest()
            {
                SpecieId = id,
                Count = 1
            }, biomeId);
            
            var isExcellent = count >= 3;

            var rec = new Recommendation<Specie>
            {
                Involved = [specie],
                Message = isExcellent 
                    ? $"Ajout majeur : Ajoutez {specie.Name}" 
                    : $"Ajout bénéfique : Ajoutez {specie.Name}",
                Resolve = fun,
                Type = isExcellent ? RecommendationType.Excellent : RecommendationType.Ok
            };

            results.Add(rec);
        }

        return results;
    }
}