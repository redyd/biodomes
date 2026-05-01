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
            .Include(s => s.BiomeSpecies)
            .AsNoTracking()
            .ToListAsync();

        var nodes = allSpecies
            .Select(s => new FoodChainNode(
                specieId: s.Id,
                name: s.Name,
                diet: SpecieMapper.MapDiet(s.DietEntity),
                classification: SpecieMapper.MapClassification(s.ClassificationEntity),
                count: s.BiomeSpecies.Sum(bs => bs.Count)
            ))
            .ToList();

        var edges = await context.SpecieFoodChains
            .Select(r => new FoodChainEdge(
                r.Predator.Id,
                r.Prey.Id
            ))
            .AsNoTracking()
            .ToListAsync();

        return new FoodChainGraph(nodes, edges);
    }

    public async Task<Result<FoodChainGraph>> GetFullGraphForBiomeAsync(int biomeId)
    {
        var allSpecies = await context.Species
            .Where(s => s.BiomeSpecies.Any(b => b.BiomeId == biomeId))
            .Include(s => s.DietEntity)
            .Include(s => s.ClassificationEntity)
            .ThenInclude(s => s.EatenByDiets)
            .Include(s => s.BiomeSpecies)
            .AsNoTracking()
            .ToListAsync();

        var nodes = allSpecies
            .Select(s => new FoodChainNode(
                specieId: s.Id,
                name: s.Name,
                diet: SpecieMapper.MapDiet(s.DietEntity),
                classification: SpecieMapper.MapClassification(s.ClassificationEntity),
                count: s.BiomeSpecies.SingleOrDefault(bs => bs.BiomeId == biomeId)?.Count ?? 0
            ))
            .ToList();

        var edges = await context.SpecieFoodChains
            .Where(r =>
                r.Predator.BiomeSpecies.Any(b => b.BiomeId == biomeId) &&
                r.Prey.BiomeSpecies.Any(b => b.BiomeId == biomeId))
            .Select(r => new FoodChainEdge(r.Predator.Id, r.Prey.Id))
            .AsNoTracking()
            .ToListAsync();

        return Result<FoodChainGraph>.Ok(new FoodChainGraph(nodes, edges));
    }

    public async Task AddRelation(int predatorId, int preyId)
    {
        var predator = await context.Species
            .FirstOrDefaultAsync(s => s.Id == predatorId);
        
        var prey = await context.Species
            .FirstOrDefaultAsync(s => s.Id == preyId);

        if (predator == null || prey == null)
        {
            throw new Exception("Prédateur ou proie introuvable");
        }

        var relation = new SpecieFoodChain
        {
            PredatorId = predator.Id,
            PreyId = prey.Id
        };

        context.SpecieFoodChains.Add(relation);
        await context.SaveChangesAsync();
    }

    public async Task DeleteRelation(int predatorId, int preyId)
    {
        var relation = await context.SpecieFoodChains
            .FirstOrDefaultAsync(r => r.Predator.Id == predatorId && r.Prey.Id == preyId);

        if (relation != null)
        {
            context.SpecieFoodChains.Remove(relation);
            await context.SaveChangesAsync();
        }
    }
}