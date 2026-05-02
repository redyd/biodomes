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
        return result.Select(BiomeMapper.MapSimpleBiome).ToList();
    }

    public async Task<ResultT<int>> CountAsync()
    {
        try
        {
            var count = await context.Biomes.CountAsync();
            return ResultT<int>.Ok(count);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<int>.Failure("Database error");
        }
    }

    public async Task<ResultT<string>> GetCreatorAsync(int biomeId)
    {
        try
        {
            var biome = await context.Biomes.Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == biomeId);

            return biome?.User.UserName is null 
                ? ResultT<string>.NotFound("Biome not found") 
                : ResultT<string>.Ok(biome.User.UserName);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<string>.Failure("Database error");
        }
    }

    public async Task<ResultT<Biome>> FindAsync(int id, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Biomes
                .FirstOrDefaultAsync(b => b.Id == id);

            if (entity is null)
            {
                return ResultT<Biome>.NotFound($"Biome {id} not found");
            }

            if (entity.IsPublic || entity.UserId == userId || isAdmin)
            {
                return ResultT<Biome>.Ok(BiomeMapper.MapSimpleBiome(entity));
            }

            return ResultT<Biome>.Forbidden("Access denied.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<Biome>.Failure("Database error");
        }
    }
    
    public async Task<ResultT<Biome>> FindComplexAsync(int id, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Biomes
                .Include(b => b.BiomeSpecies)
                .ThenInclude(spe => spe.Specie)
                .ThenInclude(sp => sp.ClassificationEntity)
                
                .Include(b => b.BiomeSpecies)
                .ThenInclude(spe => spe.Specie)
                .ThenInclude(sp => sp.DietEntity)
                
                .Include(b => b.BiomeSpecies)
                .ThenInclude(spe => spe.Specie)
                .ThenInclude(sp => sp.ClassificationEntity)
                .ThenInclude(c => c.EatenByDiets)
                
                .Include(b => b.BiomeEquipments)
                .ThenInclude(ee => ee.Equipment)
                .ThenInclude(e => e.ProducedResourceEntity)
                
                .Include(b => b.BiomeEquipments)
                .ThenInclude(ee => ee.Equipment)
                .ThenInclude(e => e.ConsumedResourceEntity)
                
                .Include(b => b.BiomeEquipments)
                .FirstOrDefaultAsync(b => b.Id == id);
            
            if (entity is null)
            {
                return ResultT<Biome>.NotFound($"Biome {id} not found");
            }

            if (entity.IsPublic || entity.UserId == userId || isAdmin)
            {
                return ResultT<Biome>.Ok(BiomeMapper.MapComplexBiome(entity));
            }

            return ResultT<Biome>.Forbidden("Access denied.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<Biome>.Failure("Database error");
        }
    }

    public async Task<ResultT<Biome>> CreateBiomeAsync(CreateBiomeRequest createRequest, string userId)
    {
        try
        {
            var exist = await context.Biomes.AnyAsync(b => b.Name.ToLower() == createRequest.Name.ToLower());

            if (exist)
            {
                return ResultT<Biome>.Conflict("A biome with this name already exist.");
            }

            var entity = new BiomeEntity
            {
                Name = createRequest.Name,
                Temperature = createRequest.Temperature,
                Humidity = createRequest.Humidity,
                ImageUrl = createRequest.ImageUrl,
                UserId = userId,
                IsPublic = createRequest.IsPublic,
            };

            await context.Biomes.AddAsync(entity);
            await context.SaveChangesAsync();
            var created = await context.Biomes
                .FirstAsync(b => b.Id == entity.Id);

            return ResultT<Biome>.Ok(BiomeMapper.MapSimpleBiome(created));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex))
        {
            return ResultT<Biome>.Conflict("A biome with this name already exists.");
        }
        catch (DbUpdateException ex) when (SqlServerError.IsForeignKeyViolation(ex))
        {
            return ResultT<Biome>.Failure("A referenced resource does not exist.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<Biome>.Failure("Database error");
        }
    }

    public async Task<ResultT<Biome>> UpdateAsync(UpdateBiomeRequest toUpdate, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Biomes
                .FirstOrDefaultAsync(b => b.Id == toUpdate.Id);

            if (entity is null)
                return ResultT<Biome>.NotFound("Biome not found");


            if (!isAdmin && !entity.IsPublic && entity.UserId != userId)
            {
                return ResultT<Biome>.Forbidden("Access denied.");
            }

            entity.Name = toUpdate.Name;
            entity.Temperature = toUpdate.Temperature;
            entity.Humidity = toUpdate.Humidity;

            if (toUpdate.ImageUrl is not null)
                entity.ImageUrl = toUpdate.ImageUrl;

            if (isAdmin && toUpdate.IsPublic.HasValue)
                entity.IsPublic = toUpdate.IsPublic.Value;

            await context.SaveChangesAsync();
            return ResultT<Biome>.Ok(BiomeMapper.MapSimpleBiome(entity));
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<Biome>.Failure("Database error");
        }
    }

    public async Task<ResultT<Biome>> DeleteAsync(int id, string userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Biomes
                .FirstOrDefaultAsync(b => b.Id == id);

            if (entity is null)
                return ResultT<Biome>.NotFound($"Biome {id} not found.");

            if (!isAdmin && entity.UserId != userId)
                return ResultT<Biome>.Forbidden("Access denied.");

            var model = BiomeMapper.MapSimpleBiome(entity);
            context.Biomes.Remove(entity);
            await context.SaveChangesAsync();
            return ResultT<Biome>.Ok(model);
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<Biome>.Failure("Database error");
        }
    }

    public async Task<Result> AddSpecie(SpecieToBiomeRequest request, int biomeId, string? userId, bool isAdmin)
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
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result.Failure("Database error");
        }
    }

    public async Task<Result> RemoveSpecie(SpecieToBiomeRequest request, int biomeId, string? userId, bool isAdmin)
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

    public async Task<Result> AddEquipment(EquipmentToBiomeRequest request, int biomeId, string? userId, bool isAdmin)
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
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result.Failure("Database error");
        }
    }

    public async Task<Result> RemoveEquipment(EquipmentToBiomeRequest request, int biomeId, string? userId, bool isAdmin)
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