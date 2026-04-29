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
        var allSpecies = await context.Species
            .Include(s => s.DietEntity)
            .Include(s => s.ClassificationEntity)
            .ThenInclude(c => c.EatenByDiets)
            .AsNoTracking()
            .ToListAsync();

        var nodes = allSpecies
            .Select(s => new FoodChainNode(
                slug: s.Slug,
                name: s.Name,
                diet: SpecieMapper.MapDiet(s.DietEntity),
                classification: SpecieMapper.MapClassification(s.ClassificationEntity)
            ))
            .ToList();

        var edges = await context.SpecieFoodChains
            .Select(r => new FoodChainEdge(
                r.Predator.Slug,
                r.Prey.Slug
            ))
            .AsNoTracking()
            .ToListAsync();

        return new FoodChainGraph(nodes, edges);
    }

    public async Task AddRelation(string predatorSlug, string preySlug)
    {
        var predator = await context.Species
            .FirstOrDefaultAsync(s => s.Slug == predatorSlug);
        
        var prey = await context.Species
            .FirstOrDefaultAsync(s => s.Slug == preySlug);

        if (predator == null || prey == null)
        {
            throw new Exception("Prédateur ou proie introuvable");
        }

        var relation = new SpecieFoodChain
        {
            PredatorId = predator.Slug,
            PreyId = prey.Slug
        };

        context.SpecieFoodChains.Add(relation);
        await context.SaveChangesAsync();
    }

    public async Task DeleteRelation(string predatorSlug, string preySlug)
    {
        var relation = await context.SpecieFoodChains
            .FirstOrDefaultAsync(r => r.Predator.Slug == predatorSlug && r.Prey.Slug == preySlug);

        if (relation != null)
        {
            context.SpecieFoodChains.Remove(relation);
            await context.SaveChangesAsync();
        }
    }
}