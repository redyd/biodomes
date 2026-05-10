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
    public async Task<Result> AddSpecieToBiome(SpecieToBiomeRequest request, string biomeSlug, string? userId,
        bool isAdmin)
    {
        try
        {
            var biome = await context.Biomes.FirstOrDefaultAsync(b => b.Slug == biomeSlug);
            var specie = await context.Species.FirstOrDefaultAsync(s => s.Slug == request.SpecieSlug);
            
            if (biome is null || specie is null)
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
                    BiomeId = biome.Id,
                    SpecieId = specie.Id
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

    public async Task<Result> RemoveSpecieToBiome(SpecieToBiomeRequest request, string biomeSlug, string? userId,
        bool isAdmin)
    {
        var biome = await context.Biomes.FirstOrDefaultAsync(b => b.Slug == biomeSlug);

        if (biome is null)
        {
            return Result.NotFound("Biome is not found");
        }

        if (biome.UserId != userId && !isAdmin)
        {
            return Result.Forbidden("Access denied.");
        }

        var existing = await context.BiomeSpecies
            .Include(bs => bs.Specie)
            .Include(bs => bs.Biome)
            .Where(bs => bs.Biome.Slug == biomeSlug && bs.Specie.Slug == request.SpecieSlug)
            .ToListAsync();

        var toRemove = existing.Take(request.Count).ToList();

        context.BiomeSpecies.RemoveRange(toRemove);
        await context.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> AddEquipmentToBiome(EquipmentToBiomeRequest request, string biomeSlug, string? userId,
        bool isAdmin)
    {
        try
        {
            var biome = await context.Biomes.FirstOrDefaultAsync(b => b.Slug == biomeSlug);
            var equipment = await context.Equipments.FirstOrDefaultAsync(e => e.Slug == request.EquipmentSlug);

            if (biome is null || equipment is null)
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
                    BiomeId = biome.Id,
                    EquipmentId = equipment.Id
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

    public async Task<Result> RemoveEquipmentToBiome(EquipmentToBiomeRequest request, string biomeSlug, string? userId,
        bool isAdmin)
    {
        var biome = await context.Biomes.FirstOrDefaultAsync(b => b.Slug == biomeSlug);

        if (biome is null)
        {
            return Result.NotFound("Biome is not found");
        }

        if (biome.UserId != userId && !isAdmin)
        {
            return Result.Forbidden("Access denied.");
        }

        var existing = await context.BiomeEquipments
            .Include(be => be.Biome)
            .Include(be => be.Equipment)
            .Where(bs => bs.Biome.Slug == biomeSlug && bs.Equipment.Slug == request.EquipmentSlug)
            .ToListAsync();

        var toRemove = existing.Take(request.Count).ToList();

        context.BiomeEquipments.RemoveRange(toRemove);
        await context.SaveChangesAsync();

        return Result.Ok();
    }
}