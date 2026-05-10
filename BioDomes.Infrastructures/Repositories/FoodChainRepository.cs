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
        .Include(s => s.BiomeSpecies)
        .ThenInclude(bs => bs.Biome)
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
                .Select(s => new FoodChainNode(specie: SpecieMapper.MapSpecie(s), count: s.BiomeSpecies.Count
                ))
                .ToList();

            var visibleIds = allSpecies.Select(s => s.Id).ToHashSet();

            var edges = await context.SpecieFoodChains
                .Include(spc => spc.Predator)
                .Include(spc => spc.Prey)
                .Where(r => visibleIds.Contains(r.Predator.Id) && visibleIds.Contains(r.Prey.Id))
                .Select(r => new FoodChainEdge(r.Predator.Slug, r.Prey.Slug))
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

    public async Task<ResultT<FoodChainGraph>> GetFullGraphForBiomeAsync(string biomeSlug, string? userId, bool isAdmin)
    {
        try
        {
            var biome = await context.Biomes
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Slug == biomeSlug);

            if (biome is null)
            {
                return ResultT<FoodChainGraph>.NotFound($"Biome {biomeSlug} not found");
            }

            if (!biome.IsPublic && biome.UserId != userId && !isAdmin)
            {
                return ResultT<FoodChainGraph>.Forbidden("Access denied.");
            }

            var allSpecies = await _query
                .Where(s =>
                    s.BiomeSpecies.Any(b => b.Biome.Slug == biomeSlug) &&
                    (s.IsPublic || s.UserId == userId || isAdmin))
                .AsNoTracking()
                .ToListAsync();

            var nodes = allSpecies
                .Select(s => new FoodChainNode(SpecieMapper.MapSpecie(s),
                    s.BiomeSpecies.Count(bs => bs.Biome.Slug == biomeSlug)
                ))
                .ToList();

            var visibleIds = allSpecies.Select(s => s.Id).ToHashSet();

            var edges = await context.SpecieFoodChains
                .Include(sfc => sfc.Predator)
                .Include(sfc => sfc.Prey)
                .Where(r =>
                    r.Predator.BiomeSpecies.Any(b => b.Biome.Slug == biomeSlug) &&
                    r.Prey.BiomeSpecies.Any(b => b.Biome.Slug == biomeSlug) &&
                    visibleIds.Contains(r.Predator.Id) &&
                    visibleIds.Contains(r.Prey.Id))
                .Select(r => new FoodChainEdge(r.Predator.Slug, r.Prey.Slug))
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