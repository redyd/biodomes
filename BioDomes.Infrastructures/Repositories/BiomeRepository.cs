using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Errors;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class BiomeRepository(BiodomeContext context, ILogger<IBiomeRepository> logger) : IBiomeRepository
{
    private readonly IQueryable<BiomeEntity> _complexQuery = context.Biomes
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
        .Include(b => b.BiomeEquipments);
    
    public async Task<ResultT<List<ComplexBiome>>> GetAllComplexAsync(string? userId, bool isAdmin)
    {
        try
        {
            var query = _complexQuery
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(b =>
                    b.IsPublic
                    || (userId != null && b.UserId == userId));
            }

            var result = await query.ToListAsync();
            return ResultT<List<ComplexBiome>>.Ok(result.Select(BiomeMapper.MapComplexBiome).ToList());
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<List<ComplexBiome>>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<string>> GetCreatorAsync(string slug)
    {
        try
        {
            var biome = await context
                .Biomes
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Slug == slug);

            return biome?.User.UserName is null
                ? ResultT<string>.NotFound("Biome not found")
                : ResultT<string>.Ok(biome.User.UserName);
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<string>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<Biome>> FindAsync(string slug, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context
                .Biomes
                .FirstOrDefaultAsync(b => b.Slug == slug);

            if (entity is null)
            {
                return ResultT<Biome>.NotFound($"Biome {slug} not found");
            }

            if (entity.IsPublic || entity.UserId == userId || isAdmin)
            {
                return ResultT<Biome>.Ok(BiomeMapper.MapSimpleBiome(entity));
            }

            return ResultT<Biome>.Forbidden("Access denied.");
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<Biome>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<ComplexBiome>> FindComplexAsync(string slug, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await _complexQuery
                .FirstOrDefaultAsync(b => b.Slug == slug);

            if (entity is null)
            {
                return ResultT<ComplexBiome>.NotFound($"Biome {slug} not found");
            }

            if (entity.IsPublic || entity.UserId == userId || isAdmin)
            {
                return ResultT<ComplexBiome>.Ok(BiomeMapper.MapComplexBiome(entity));
            }

            return ResultT<ComplexBiome>.Forbidden("Access denied.");
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<ComplexBiome>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<Biome>> CreateBiomeAsync(CreateBiomeRequest createRequest, string userId)
    {
        try
        {
            var slug = createRequest.Name.ToKebabCase();
            var exist = await context.Biomes.AnyAsync(b => b.Slug == slug);

            if (exist)
            {
                return ResultT<Biome>.Conflict("A biome with this name already exist.");
            }

            var entity = new BiomeEntity
            {
                Slug = slug,
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
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<Biome>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<Biome>> UpdateAsync(UpdateBiomeRequest toUpdate, string? userId, bool isAdmin)
    {
        try
        {
            var slug = toUpdate.Name.ToKebabCase();
            var exist = await context
                .Biomes
                .Where(b => b.Slug != toUpdate.PreviousSlug)
                .AnyAsync(b => b.Slug == slug);

            if (exist)
            {
                return ResultT<Biome>.Conflict("A biome with this name already exist.");
            }

            var entity = await context
                .Biomes
                .FirstOrDefaultAsync(b => b.Slug == toUpdate.PreviousSlug);

            if (entity is null)
            {
                return ResultT<Biome>.NotFound("Biome not found");
            }

            if (!isAdmin && !entity.IsPublic && entity.UserId != userId)
            {
                return ResultT<Biome>.Forbidden("Access denied.");
            }

            entity.Slug = slug;
            entity.Name = toUpdate.Name;
            entity.Temperature = toUpdate.Temperature;
            entity.Humidity = toUpdate.Humidity;

            if (toUpdate.ImageUrl is not null)
            {
                entity.ImageUrl = toUpdate.ImageUrl;
            }

            if (isAdmin && toUpdate.IsPublic.HasValue)
            {
                entity.IsPublic = toUpdate.IsPublic.Value;
            }

            await context.SaveChangesAsync();
            return ResultT<Biome>.Ok(BiomeMapper.MapSimpleBiome(entity));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex))
        {
            return ResultT<Biome>.Conflict("A biome with this name already exists.");
        }
        catch (DbUpdateException ex) when (SqlServerError.IsForeignKeyViolation(ex))
        {
            return ResultT<Biome>.Failure("A referenced resource does not exist.");
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<Biome>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<Biome>> DeleteAsync(string slug, string userId, bool isAdmin)
    {
        try
        {
            var entity = await context
                .Biomes
                .FirstOrDefaultAsync(b => b.Slug == slug);

            if (entity is null)
            {
                return ResultT<Biome>.NotFound($"Biome {slug} not found.");
            }

            if (!isAdmin && entity.UserId != userId)
            {
                return ResultT<Biome>.Forbidden("Access denied.");
            }

            var model = BiomeMapper.MapSimpleBiome(entity);
            context.Biomes.Remove(entity);
            await context.SaveChangesAsync();
            
            return ResultT<Biome>.Ok(model);
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<Biome>.Failure($"Database error: {e.Message}");
        }
    }
}