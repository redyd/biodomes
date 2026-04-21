using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Mappers;
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
                .FirstOrDefaultAsync();

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

    public async Task<Result<Biome>> CreateEquipmentAsync(CreateBiomeRequest createRequest, string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Biome>> UpdateAsync(UpdateBiomeRequest toUpdate, string? userId, bool isAdmin)
    {
        throw new NotImplementedException();
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