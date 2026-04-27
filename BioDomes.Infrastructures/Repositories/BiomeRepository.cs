using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class BiomeRepository(BiodomeContext context, ILogger<IBiomeRepository> logger) : IBiomeRepository
{
    public async Task<List<Biome>> GetAllAsync(string? userId, bool isAdmin)
    {
        var query = context.Biomes
            .AsQueryable();

        if (!isAdmin)
        {
            query = query.Where(b =>
                b.IsPublic
                || (userId != null && b.UserId == userId));
        }

        var result = await query.ToListAsync();
        return result.Select(BiomeMapper.MapBiome).ToList();
    }

    public async Task<Result<int>> CountAsync()
    {
        try
        {
            var count = await context.Biomes.CountAsync();
            return Result<int>.Ok(count);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<int>.Failure("Database error");
        }
    }

    public async Task<Result<Biome>> FindAsync(int id, string userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Biomes
                .FirstOrDefaultAsync(b => b.Id == id);

            if (entity is null)
                return Result<Biome>.Failure($"Biome {id} not found");

            if (!isAdmin && entity.UserId != userId)
                return Result<Biome>.Forbidden("Access denied.");

            return Result<Biome>.Ok(BiomeMapper.MapBiome(entity));
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Biome>.Failure("Database error");
        }
    }

    public async Task<Result<Biome>> CreateBiomeAsync(CreateBiomeRequest createRequest, string userId)
    {
        try
        {
            var exist = await context.Biomes.AnyAsync(b => b.Name.ToLower() == createRequest.Name.ToLower());

            if (exist)
            {
                return Result<Biome>.Conflict("A biome with this name already exist.");
            }

            var entity = new BiomeEntity
            {
                Name = createRequest.Name,
                Temperature = createRequest.Temperature,
                Humidity = createRequest.Humidity,
                ImageUrl = createRequest.ImageUrl,
                UserId = createRequest.UserId,
                IsPublic = createRequest.IsPublic,
            };

            await context.Biomes.AddAsync(entity);
            await context.SaveChangesAsync();
            var created = await context.Biomes
                .FirstAsync(b => b.Id == entity.Id);

            return Result<Biome>.Ok(BiomeMapper.MapBiome(created));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex))
        {
            return Result<Biome>.Conflict("A biome with this name already exists.");
        }
        catch (DbUpdateException ex) when (SqlServerError.IsForeignKeyViolation(ex))
        {
            return Result<Biome>.Failure("A referenced resource does not exist.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Biome>.Failure("Database error");
        }
    }

    public async Task<Result<Biome>> UpdateAsync(UpdateBiomeRequest toUpdate, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Biomes
                .FirstOrDefaultAsync(b => b.Id == toUpdate.Id);
            
            if (entity is null)
                return Result<Biome>.NotFound("Biome not found");
            
            
            if (!isAdmin && !entity.IsPublic && entity.UserId != userId)
            {
                return Result<Biome>.Forbidden("Access denied.");
            }

            entity.Name = toUpdate.Name;
            entity.Temperature = toUpdate.Temperature;
            entity.Humidity = toUpdate.Humidity;

            if (toUpdate.ImageUrl is not null)
                entity.ImageUrl = toUpdate.ImageUrl;

            if (isAdmin && toUpdate.IsPublic.HasValue)
                entity.IsPublic = toUpdate.IsPublic.Value;

            await context.SaveChangesAsync();
            return Result<Biome>.Ok(BiomeMapper.MapBiome(entity));
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Biome>.Failure("Database error");
        }
    }

    public async Task<Result<Biome>> DeleteAsync(int id, string userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Biomes
                .FirstOrDefaultAsync(b => b.Id == id);

            if (entity is null)
                return Result<Biome>.NotFound($"Biome {id} not found.");

            if (!isAdmin && entity.UserId != userId)
                return Result<Biome>.Forbidden("Access denied.");

            var model = BiomeMapper.MapBiome(entity);
            context.Biomes.Remove(entity);
            await context.SaveChangesAsync();
            return Result<Biome>.Ok(model);
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Biome>.Failure("Database error");
        }
    }
}