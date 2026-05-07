using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.Errors;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class FoodChainRepository(BiodomeContext context, ILogger<FoodChainRepository> logger) : IFoodChainRepository
{
    private readonly IQueryable<SpecieEntity> _query = context.Species
        .Include(s => s.DietEntity)
        .Include(s => s.ClassificationEntity)
        .ThenInclude(c => c.EatenByDiets)
        .Include(s => s.BiomeSpecies);

    public async Task<ResultT<FoodChainGraph>> GetFullGraphAsync(string? userId, bool isAdmin)
    {
        try
        {
            var allSpecies = await _query
                .Where(s => s.IsPublic || s.UserId == userId || isAdmin)
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

            return ResultT<FoodChainGraph>.Ok(new FoodChainGraph(nodes, edges));
        }
        catch (DbUpdateException ex)
        {
            logger.LogError("Database error: {ExMessage}", ex.Message);
            return ResultT<FoodChainGraph>.Failure("Database error");
        }
    }

    public async Task<ResultT<FoodChainGraph>> GetFullGraphForBiomeAsync(int biomeId, string? userId, bool isAdmin)
    {
        try
        {
            var biome = await context.Biomes
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == biomeId);

            if (biome is null)
                return ResultT<FoodChainGraph>.NotFound($"Biome {biomeId} not found");

            if (!biome.IsPublic && biome.UserId != userId && !isAdmin)
                return ResultT<FoodChainGraph>.Forbidden("Access denied.");

            var allSpecies = await _query
                .Where(s =>
                    s.BiomeSpecies.Any(b => b.BiomeId == biomeId) &&
                    (s.IsPublic || s.UserId == userId || isAdmin))
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

            return ResultT<FoodChainGraph>.Ok(new FoodChainGraph(nodes, edges));
        }
        catch (DbUpdateException ex)
        {
            logger.LogError("Database error: {ExMessage}", ex.Message);
            return ResultT<FoodChainGraph>.Failure("Database error");
        }
    }

    public async Task<Result> AddRelation(int predatorId, int preyId)
    {
        try
        {
            var predator = await context.Species
                .FirstOrDefaultAsync(s => s.Id == predatorId);

            var prey = await context.Species
                .FirstOrDefaultAsync(s => s.Id == preyId);

            if (predator == null || prey == null)
            {
                return Result.NotFound("Prey or predator not found");
            }

            var relation = new SpecieFoodChain
            {
                PredatorId = predator.Id,
                PreyId = prey.Id
            };

            context.SpecieFoodChains.Add(relation);
            await context.SaveChangesAsync();
            
            return Result.Ok();
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex))
        {
            return Result.Conflict("A similar node already exists.");
        }
        catch (DbUpdateException ex) when (SqlServerError.IsForeignKeyViolation(ex))
        {
            return Result.Failure("A referenced resource does not exist.");
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return Result.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<Result> DeleteRelation(int predatorId, int preyId)
    {
        try
        {
            var relation = await context.SpecieFoodChains
                .FirstOrDefaultAsync(r => r.Predator.Id == predatorId && r.Prey.Id == preyId);

            if (relation is null)
            {
                return Result.NotFound("Prey or predator not found");
            }
            
            context.SpecieFoodChains.Remove(relation);
            await context.SaveChangesAsync();
            
            return Result.Ok();
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return Result.Failure($"Database error: {e.Message}");
        }
    }
}