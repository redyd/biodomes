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
    
    public async Task<ResultT<ComplexBiome>> FindComplexAsync(int id, string? userId, bool isAdmin)
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
                return ResultT<ComplexBiome>.NotFound($"Biome {id} not found");
            }

            if (entity.IsPublic || entity.UserId == userId || isAdmin)
            {
                return ResultT<ComplexBiome>.Ok(BiomeMapper.MapComplexBiome(entity));
            }

            return ResultT<ComplexBiome>.Forbidden("Access denied.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<ComplexBiome>.Failure("Database error");
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

    public async Task<List<BiomeStatsDto>> GetStatsAsync(string? userId, bool isAdmin)
    {
        var query = context.Biomes
            .Include(b => b.BiomeSpecies)
            .ThenInclude(bs => bs.Specie)
            .ThenInclude(s => s.DietEntity)
            .Include(b => b.BiomeSpecies)
            .ThenInclude(bs => bs.Specie)
            .ThenInclude(s => s.ClassificationEntity)
            .Include(b => b.BiomeEquipments)
            .AsQueryable();

        if (!isAdmin)
        {
            query = query.Where(b => userId != null && b.UserId == userId);
        }

        var biomes = await query.ToListAsync();

        return biomes.Select(b => new BiomeStatsDto
        {
            Id = b.Id,
            Name = b.Name,
            SpeciesCount = b.BiomeSpecies.Count,
            EquipmentCount = b.BiomeEquipments.Count,
            Temperature = b.Temperature,
            Humidity = b.Humidity,

            SpeciesByDiet = b.BiomeSpecies
                .GroupBy(bs => bs.Specie.DietEntity.Label)
                .ToDictionary(g => g.Key, g => g.Count()),

            SpeciesByClassification = b.BiomeSpecies
                .GroupBy(bs => bs.Specie.ClassificationEntity.Label)
                .ToDictionary(g => g.Key, g => g.Count()),
            
            SpeciesByName = b.BiomeSpecies
            .GroupBy(bs => bs.Specie.Name)
            .ToDictionary(g => g.Key, g => g.Count()),
        }).ToList();
    }

    public async Task<List<BiomeHistoryDto>> GetHistoryAsync(
    string? userId, bool isAdmin, int? biomeId, int days)
{
    var since = days == 0 ? DateTime.MinValue : DateTime.UtcNow.AddDays(-days);

    var query = context.Biomes
        .Include(b => b.BiomeSpecies)
            .ThenInclude(bs => bs.Specie)
                .ThenInclude(s => s.DietEntity)
        .Include(b => b.BiomeSpecies)
            .ThenInclude(bs => bs.Specie)
                .ThenInclude(s => s.ClassificationEntity)
        .AsQueryable();

    if (!isAdmin)
        query = query.Where(b => userId != null && b.UserId == userId);

    if (biomeId.HasValue)
        query = query.Where(b => b.Id == biomeId.Value);

    var biomes = await query.ToListAsync();

    return biomes.Select(b =>
    {
        var speciesInRange = b.BiomeSpecies
            .Where(bs => bs.Inserted >= since)
            .OrderBy(bs => bs.Inserted)
            .ToList();

        var speciesOverTime = speciesInRange
            .GroupBy(bs => bs.Inserted.ToString("yyyy-MM-dd"))
            .Select(g => new HistoryPoint
            {
                Date = g.Key,
                Label = "Espèces",
                Count = g.Count()
            })
            .OrderBy(p => p.Date)
            .ToList();

        var cumul = 0;
        foreach (var point in speciesOverTime)
        {
            cumul += point.Count;
            point.Count = cumul;
        }

        var byDiet = speciesInRange
            .GroupBy(bs => new
            {
                Date = bs.Inserted.ToString("yyyy-MM-dd"),
                Diet = bs.Specie.DietEntity.Label
            })
            .Select(g => new HistoryPoint
            {
                Date = g.Key.Date,
                Label = g.Key.Diet,
                Count = g.Count()
            })
            .OrderBy(p => p.Date)
            .ToList();

        var byClassification = speciesInRange
            .GroupBy(bs => new
            {
                Date = bs.Inserted.ToString("yyyy-MM-dd"),
                Class = bs.Specie.ClassificationEntity.Label
            })
            .Select(g => new HistoryPoint
            {
                Date = g.Key.Date,
                Label = g.Key.Class,
                Count = g.Count()
            })
            .OrderBy(p => p.Date)
            .ToList();

        return new BiomeHistoryDto
        {
            BiomeId = b.Id,
            BiomeName = b.Name,
            SpeciesOverTime = speciesOverTime,
            ByDietOverTime = byDiet,
            ByClassificationOverTime = byClassification
        };
    }).ToList();
}
}