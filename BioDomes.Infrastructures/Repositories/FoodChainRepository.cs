using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.Repositories;

public class FoodChainRepository(BiodomeContext context) : IFoodChainRepository
{
    public async Task<FoodChainGraph> GetFullGraphAsync()
    {
        var relations = await context.SpecieFoodChains
            .Include(r => r.Predator)
            .ThenInclude(s => s.DietEntity)

            .Include(r => r.Predator)
            .ThenInclude(s => s.ClassificationEntity)
            .ThenInclude(c => c.EatenByDiets)

            .Include(r => r.Prey)
            .ThenInclude(s => s.DietEntity)

            .Include(r => r.Prey)
            .ThenInclude(s => s.ClassificationEntity)
            .ThenInclude(c => c.EatenByDiets)

            .AsNoTracking()
            .ToListAsync();

 
        var speciesById = relations
            .SelectMany(r => new[] { r.Predator, r.Prey })
            .DistinctBy(s => s.Slug)
            .Select(s => new FoodChainNode(
                slug: s.Slug,
                name: s.Name,
                diet: SpecieMapper.MapDiet(s.DietEntity),
                classification: SpecieMapper.MapClassification(s.ClassificationEntity)
            ))
            .ToList();

 
        var edges = relations
            .Select(r => new FoodChainEdge(
                predatorId: r.PredatorId,
                preyId: r.PreyId
            ))
            .ToList();
 
        return new FoodChainGraph(speciesById, edges);
    }
}