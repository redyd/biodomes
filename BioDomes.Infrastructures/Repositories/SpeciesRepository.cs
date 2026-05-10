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

public class SpeciesRepository(BiodomeContext context, ILogger<SpeciesRepository> logger) : ISpeciesRepository
{
    public async Task<ResultT<List<Specie>>> FindAllAsync(string? userId, bool isAdmin)
    {
        try
        {
            var query = context
                .Species
                .Include(e => e.DietEntity)
                .Include(e => e.ClassificationEntity)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(e =>
                    e.IsPublic
                    || (userId != null && e.UserId == userId));
            }

            var result = await query.ToListAsync();
            return ResultT<List<Specie>>.Ok(result.Select(SpecieMapper.MapSpecie).ToList());
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {EMessage}", e.Message);
            return ResultT<List<Specie>>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<Dictionary<string, Specie>>> FindAllAsDictAsync(string? userId, bool isAdmin)
    {
        try
        {
            var query = context
                .Species
                .Include(e => e.DietEntity)
                .Include(e => e.ClassificationEntity)
                .ThenInclude(e => e.EatenByDiets)
                .AsQueryable();

            if (!isAdmin)
            {
                query = query.Where(e =>
                    e.IsPublic
                    || (userId != null && e.UserId == userId));
            }

            var result = await query.ToDictionaryAsync(s => s.Slug, SpecieMapper.MapSpecie);
            return ResultT<Dictionary<string, Specie>>.Ok(result);
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {EMessage}", e.Message);
            return ResultT<Dictionary<string, Specie>>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<Specie>> FindBySlugAsync(string slug, string? userId, bool isAdmin)
    {
        try
        {
            var result = await context
                .Species
                .Include(e => e.DietEntity)
                .Include(e => e.ClassificationEntity)
                .FirstOrDefaultAsync(e => e.Slug == slug);

            if (result is null)
                return ResultT<Specie>.NotFound("Specie not found");

            if (!isAdmin && !result.IsPublic && result.UserId != userId)
                return ResultT<Specie>.Forbidden("User not authorized to access this specie");

            return ResultT<Specie>.Ok(SpecieMapper.MapSpecie(result));
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {EMessage}", e.Message);
            return ResultT<Specie>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<Specie>> UpdateAsync(UpdateSpecieRequest toUpdate, string? userId, bool isAdmin)
    {
        try
        {
            var allSlugs = await context.Species
                .Where(s => s.Slug != toUpdate.PreviousSlug)
                .Select(s => s.Slug)
                .ToListAsync();

            var slug = toUpdate.Name.ToKebabCase();
            if (allSlugs.Any(n => n == slug))
            {
                return ResultT<Specie>.Conflict($"Une espèce nommée '{toUpdate.Name}' existe déjà.");
            }

            var entity = await context.Species.FirstOrDefaultAsync(s => s.Slug == toUpdate.PreviousSlug);

            if (entity is null)
            {
                return ResultT<Specie>.NotFound($"Specie '{toUpdate.Name}' not found.");
            }

            if (!isAdmin && entity.UserId != userId)
            {
                return ResultT<Specie>.Forbidden("Access denied.");
            }

            var visibility = entity.IsPublic;
            if (isAdmin && toUpdate.IsPublic is not null)
            {
                visibility = (bool)toUpdate.IsPublic;
            }

            var classification = await context
                .SpecieClassifications
                .FirstOrDefaultAsync(sp => sp.Slug == toUpdate.ClassificationSlug);
            
            var diet = await context
                .SpecieDiets
                .FirstOrDefaultAsync(s => s.Slug == toUpdate.DietSlug);

            if (classification is null || diet is null)
            {
                return ResultT<Specie>.NotFound("Attributes not found");
            }

            entity.Slug = slug;
            entity.Name = toUpdate.Name;
            entity.Slug = toUpdate.Name.ToKebabCase();
            entity.Height = toUpdate.Height;
            entity.Weight = toUpdate.Weight;
            entity.IsPublic = visibility;
            
            entity.ClassificationId = classification.Id;
            entity.DietId = diet.Id;

            if (!string.IsNullOrEmpty(toUpdate.ImageUrl))
            {
                entity.UploadedImageId = toUpdate.ImageUrl;
            }

            await context.SaveChangesAsync();

            var specie = await context.Species
                .Include(s => s.DietEntity)
                .Include(s => s.ClassificationEntity)
                .ThenInclude(ce => ce.EatenByDiets)
                .FirstAsync(s => s.Slug == slug);

            return ResultT<Specie>.Ok(SpecieMapper.MapSpecie(specie));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex) ||
                                           SqlServerError.IsPrimaryKeyViolation(ex))
        {
            return ResultT<Specie>.Conflict("A specie with this name already exists.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<Specie>.Failure("Database error");
        }
    }

    public async Task<ResultT<Specie>> AddAsync(CreateSpecieRequest toAdd, string? userId)
    {
        try
        {
            if (userId is null)
            {
                return ResultT<Specie>.Forbidden("You should be authenticate to add a specie");
            }

            var allName = await context.Species.Select(s => s.Name).ToListAsync();
            var updateKebab = toAdd.Name.ToKebabCase();
            if (allName.Any(n => n.ToKebabCase() == updateKebab))
            {
                return ResultT<Specie>.Conflict($"Une espèce nommée '{toAdd.Name}' existe déjà.");
            }
            
            var classification = await context
                .SpecieClassifications
                .FirstOrDefaultAsync(sp => sp.Slug == toAdd.ClassificationSlug);
            
            var diet = await context
                .SpecieDiets
                .FirstOrDefaultAsync(s => s.Slug == toAdd.DietSlug);

            if (classification is null || diet is null)
            {
                return ResultT<Specie>.NotFound("Attributes not found");
            }

            var entity = new SpecieEntity
            {
                UserId = userId,
                Name = toAdd.Name,
                Slug = toAdd.Name.ToKebabCase(),
                ClassificationId = classification.Id,
                DietId = diet.Id,
                Height = toAdd.Height,
                Weight = toAdd.Weight,
                UploadedImageId = toAdd.Image,
                IsPublic = false
            };

            await context.Species.AddAsync(entity);
            await context.SaveChangesAsync();

            var specie = await context
                .Species
                .Include(s => s.DietEntity)
                .Include(s => s.ClassificationEntity)
                .FirstAsync(s => s.Id == entity.Id);
            return ResultT<Specie>.Ok(SpecieMapper.MapSpecie(specie));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex) ||
                                           SqlServerError.IsPrimaryKeyViolation(ex))
        {
            return ResultT<Specie>.Conflict("A specie with this name already exists.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<Specie>.Failure("Database error");
        }
    }

    public async Task<ResultT<Specie>> DeleteAsync(string specieSlug, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Species
                .Include(s => s.DietEntity)
                .Include(s => s.ClassificationEntity)
                .FirstOrDefaultAsync(s => s.Slug == specieSlug);

            if (entity is null)
                return ResultT<Specie>.NotFound($"Specie '{specieSlug}' not found.");

            if (!isAdmin && entity.UserId != userId)
                return ResultT<Specie>.Forbidden("Access denied.");

            var deleted = SpecieMapper.MapSpecie(entity);
            context.Species.Remove(entity);
            await context.SaveChangesAsync();

            return ResultT<Specie>.Ok(deleted);
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<Specie>.Failure("Database error");
        }
    }
}