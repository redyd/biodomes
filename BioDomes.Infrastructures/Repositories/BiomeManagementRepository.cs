using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Errors;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class BiomeManagementRepository(BiodomeContext context, ILogger<IBiomeManagementRepository> logger)
    : IBiomeManagementRepository
{
    public async Task<Result> AddSpecieToBiome(SpecieToBiomeRequest request, int biomeId, string? userId, bool isAdmin)
    {
        try
        {
            var biome = await context.Biomes.FirstOrDefaultAsync(b => b.Id == biomeId);

            if (biome is null)
            {
                return Result.NotFound("Biome is not found");
            }

            if (biome.UserId != userId && !isAdmin)
            {
                return Result.Forbidden("Access denied.");
            }

            var allToAdd = new List<BiomeSpecieEntity>();
            for (var c = 0; c < request.Count; c++)
            {
                allToAdd.Add(new BiomeSpecieEntity
                {
                    BiomeId = biomeId,
                    SpecieId = request.SpecieId,
                });
            }

            await context.BiomeSpecies.AddRangeAsync(allToAdd);
            await context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex))
        {
            return Result.Conflict("A biome already contains this relation.");
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

    public async Task<Result> RemoveSpecieToBiome(SpecieToBiomeRequest request, int biomeId, string? userId, bool isAdmin)
    {
        var biome = await context.Biomes.FirstOrDefaultAsync(b => b.Id == biomeId);

        if (biome is null)
        {
            return Result.NotFound("Biome is not found");
        }

        if (biome.UserId != userId && !isAdmin)
        {
            return Result.Forbidden("Access denied.");
        }

        var existing = await context.BiomeSpecies
            .Where(bs => bs.BiomeId == biomeId && bs.SpecieId == request.SpecieId)
            .ToListAsync();

        var toRemove = existing.Take(request.Count).ToList();

        context.BiomeSpecies.RemoveRange(toRemove);
        await context.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> AddEquipmentToBiome(EquipmentToBiomeRequest request, int biomeId, string? userId, bool isAdmin)
    {
        try
        {
            var biome = await context.Biomes.FirstOrDefaultAsync(b => b.Id == biomeId);

            if (biome is null)
            {
                return Result.NotFound("Biome is not found");
            }

            if (biome.UserId != userId && !isAdmin)
            {
                return Result.Forbidden("Access denied.");
            }

            var allToAdd = new List<BiomeEquipmentEntity>();
            for (var c = 0; c < request.Count; c++)
            {
                allToAdd.Add(new BiomeEquipmentEntity
                {
                    BiomeId = biomeId,
                    EquipmentId = request.EquipmentId,
                });
            }

            await context.BiomeEquipments.AddRangeAsync(allToAdd);
            await context.SaveChangesAsync();

            return Result.Ok();
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex))
        {
            return Result.Conflict("A biome already contains this relation.");
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

    public async Task<Result> RemoveEquipmentToBiome(EquipmentToBiomeRequest request, int biomeId, string? userId, bool isAdmin)
    {
        var biome = await context.Biomes.FirstOrDefaultAsync(b => b.Id == biomeId);

        if (biome is null)
        {
            return Result.NotFound("Biome is not found");
        }

        if (biome.UserId != userId && !isAdmin)
        {
            return Result.Forbidden("Access denied.");
        }

        var existing = await context.BiomeEquipments
            .Where(bs => bs.BiomeId == biomeId && bs.EquipmentId == request.EquipmentId)
            .ToListAsync();

        var toRemove = existing.Take(request.Count).ToList();

        context.BiomeEquipments.RemoveRange(toRemove);
        await context.SaveChangesAsync();

        return Result.Ok();
    }
}