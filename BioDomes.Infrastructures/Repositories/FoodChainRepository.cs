using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class FoodChainRepository(BiodomeContext context, ILogger<FoodChainRepository> logger) : IFoodChainRepository
{
    public async Task<Result<FoodChainGraph>> GetFullGraphAsync(string? userId, bool isAdmin)
    {
        try
        {
            var allSpecies = await context.Species
                .Where(s => s.IsPublic || s.UserId == userId || isAdmin)
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
                    count: s.BiomeSpecies.Count
                ))
                .ToList();

            var visibleIds = allSpecies.Select(s => s.Id).ToHashSet();

            var edges = await context.SpecieFoodChains
                .Where(r => visibleIds.Contains(r.Predator.Id) && visibleIds.Contains(r.Prey.Id))
                .Select(r => new FoodChainEdge(r.Predator.Id, r.Prey.Id))
                .AsNoTracking()
                .ToListAsync();

            return Result<FoodChainGraph>.Ok(new FoodChainGraph(nodes, edges));
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<FoodChainGraph>.Failure("Database error");
        }
    }

    public async Task<Result<FoodChainGraph>> GetFullGraphForBiomeAsync(int biomeId, string? userId, bool isAdmin)
    {
        try
        {
            var biome = await context.Biomes
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == biomeId);

            if (biome is null)
                return Result<FoodChainGraph>.NotFound($"Biome {biomeId} not found");

            if (!biome.IsPublic && biome.UserId != userId && !isAdmin)
                return Result<FoodChainGraph>.Forbidden("Access denied.");

            var allSpecies = await context.Species
                .Where(s =>
                    s.BiomeSpecies.Any(b => b.BiomeId == biomeId) &&
                    (s.IsPublic || s.UserId == userId || isAdmin))
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
                    count: s.BiomeSpecies.Count(bs => bs.BiomeId == biomeId)
                ))
                .ToList();

            var visibleIds = allSpecies.Select(s => s.Id).ToHashSet();

            var edges = await context.SpecieFoodChains
                .Where(r =>
                    r.Predator.BiomeSpecies.Any(b => b.BiomeId == biomeId) &&
                    r.Prey.BiomeSpecies.Any(b => b.BiomeId == biomeId) &&
                    visibleIds.Contains(r.Predator.Id) &&
                    visibleIds.Contains(r.Prey.Id))
                .Select(r => new FoodChainEdge(r.Predator.Id, r.Prey.Id))
                .AsNoTracking()
                .ToListAsync();

            return Result<FoodChainGraph>.Ok(new FoodChainGraph(nodes, edges));
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<FoodChainGraph>.Failure("Database error");
        }
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