using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class SpeciesRepository(BiodomeContext context, ILogger<SpeciesRepository> logger) : ISpeciesRepository
{
    public async Task<Result<List<Specie>>> FindAllAsync(string? userId, bool isAdmin)
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
            return Result<List<Specie>>.Ok(result.Select(SpecieMapper.MapSpecie).ToList());
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<List<Specie>>.Failure("Database error");
        }
    }

    public async Task<Result<int>> CountAsync()
    {
        try
        {
            var count = await context.Species.CountAsync();
            return Result<int>.Ok(count);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<int>.Failure("Database error");
        }
    }

    public async Task<Result<Specie>> FindBySlugAsync(string slug, string? userId, bool isAdmin)
    {
        try
        {
            var result = await context
                .Species
                .Include(e => e.DietEntity)
                .Include(e => e.ClassificationEntity)
                .FirstOrDefaultAsync(e => e.Slug == slug);

            if (result is null)
                return Result<Specie>.NotFound("Specie not found");

            if (!isAdmin && !result.IsPublic && result.UserId != userId)
                return Result<Specie>.Forbidden("User not authorized to access this specie");

            return Result<Specie>.Ok(SpecieMapper.MapSpecie(result));
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Specie>.Failure("Database error");
        }
    }

    public async Task<Result<Specie>> UpdateAsync(UpdateSpecieRequest toUpdate, string? userId, bool isAdmin)
    {
        try
        {
            var allName = await context.Species
                .Where(s => s.Id != toUpdate.Id)
                .Select(s => s.Name)
                .ToListAsync();
            
            var updateKebab = toUpdate.Name.ToKebabCase();
            if (allName.Any(n => n.ToKebabCase() == updateKebab))
            {
                return Result<Specie>.Conflict($"Une espèce nommée '{toUpdate.Name}' existe déjà.");
            }

            var entity = await context.Species.FirstOrDefaultAsync(s => s.Id == toUpdate.Id);

            if (entity is null)
                return Result<Specie>.NotFound($"Specie '{toUpdate.Name}' not found.");

            if (!isAdmin && entity.UserId != userId)
                return Result<Specie>.Forbidden("Access denied.");

            var visibility = entity.IsPublic;
            if (isAdmin && toUpdate.IsPublic is not null)
            {
                visibility = (bool)toUpdate.IsPublic;
            }

            entity.Name = toUpdate.Name;
            entity.Slug = toUpdate.Name.ToKebabCase();
            entity.ClassificationId = toUpdate.ClassificationId;
            entity.DietId = toUpdate.DietId;
            entity.Height = toUpdate.Height;
            entity.Weight = toUpdate.Weight;
            entity.IsPublic = visibility;

            if (toUpdate.ImageUrl is not null)
            {
                entity.UploadedImageId = toUpdate.ImageUrl;
            }

            await context.SaveChangesAsync();

            var specie = await context.Species
                .Include(s => s.DietEntity)
                .Include(s => s.ClassificationEntity)
                .ThenInclude(ce => ce.EatenByDiets)
                .FirstAsync(s => s.Id == toUpdate.Id);
            
            return Result<Specie>.Ok(SpecieMapper.MapSpecie(specie));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex) ||
                                           SqlServerError.IsPrimaryKeyViolation(ex))
        {
            return Result<Specie>.Conflict("A specie with this name already exists.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Specie>.Failure("Database error");
        }
    }

    public async Task<Result<Specie>> AddAsync(CreateSpecieRequest toAdd, string? userId)
    {
        try
        {
            if (userId is null)
            {
                return Result<Specie>.Forbidden("You should be authenticate to add a specie");
            }
            
            var allName = await context.Species.Select(s => s.Name).ToListAsync();
            var updateKebab = toAdd.Name.ToKebabCase();
            if (allName.Any(n => n.ToKebabCase() == updateKebab))
            {
                return Result<Specie>.Conflict($"Une espèce nommée '{toAdd.Name}' existe déjà.");
            }
            
            var entity = new SpecieEntity
            {
                UserId = userId,
                Name = toAdd.Name,
                Slug = toAdd.Name.ToKebabCase(),
                ClassificationId = toAdd.ClassificationId,
                DietId = toAdd.DietId,
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
            return Result<Specie>.Ok(SpecieMapper.MapSpecie(specie));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex) ||
                                           SqlServerError.IsPrimaryKeyViolation(ex))
        {
            return Result<Specie>.Conflict("A specie with this name already exists.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Specie>.Failure("Database error");
        }
    }

    public async Task<Result<Specie>> DeleteAsync(int specieId, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Species
                .Include(s => s.DietEntity)
                .Include(s => s.ClassificationEntity)
                .FirstOrDefaultAsync(s => s.Id == specieId);

            if (entity is null)
                return Result<Specie>.NotFound($"Specie '{specieId}' not found.");

            if (!isAdmin && entity.UserId != userId)
                return Result<Specie>.Forbidden("Access denied.");

            var deleted = SpecieMapper.MapSpecie(entity);
            context.Species.Remove(entity);
            await context.SaveChangesAsync();

            return Result<Specie>.Ok(deleted);
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Specie>.Failure("Database error");
        }
    }
}
