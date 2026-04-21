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
            var query = context.Species.AsQueryable();

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
            var result = await context.Species.FirstOrDefaultAsync(e => e.Slug == slug);

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
            if (await context.Species.AnyAsync(s =>
                    s.Slug != toUpdate.PreviousSlug &&
                    s.Name.ToLower() == toUpdate.Name.ToLower()))
            {
                return Result<Specie>.Conflict($"Une espèce nommée '{toUpdate.Name}' existe déjà.");
            }

            var entity = await context.Species.FirstOrDefaultAsync(s => s.Slug == toUpdate.PreviousSlug);

            if (entity is null)
                return Result<Specie>.NotFound($"Specie '{toUpdate.PreviousSlug}' not found.");

            if (!isAdmin && entity.UserId != userId)
                return Result<Specie>.Forbidden("Access denied.");

            var visibility = false;
            if (isAdmin && toUpdate.IsPublic is not null)
            {
                visibility = (bool)toUpdate.IsPublic;
            }

            // if slugs are different
            if (entity.Slug != toUpdate.Slug)
            {
                var replacement = new SpecieEntity
                {
                    UserId = entity.UserId,
                    Slug = toUpdate.Slug,
                    Name = toUpdate.Name,
                    TypeLabel = toUpdate.Type,
                    DietLabel = toUpdate.Diet,
                    Height = toUpdate.Height,
                    Weight = toUpdate.Weight,
                    UploadedImageId = toUpdate.ImageUrl ?? entity.UploadedImageId,
                    IsPublic = visibility
                };

                context.Species.Remove(entity);
                await context.Species.AddAsync(replacement);
                await context.SaveChangesAsync();

                return await FindBySlugAsync(replacement.Slug, userId, isAdmin);
            }

            entity.Name = toUpdate.Name;
            entity.TypeLabel = toUpdate.Type;
            entity.DietLabel = toUpdate.Diet;
            entity.Height = toUpdate.Height;
            entity.Weight = toUpdate.Weight;
            entity.IsPublic = visibility;

            if (toUpdate.ImageUrl is not null)
            {
                entity.UploadedImageId = toUpdate.ImageUrl;
            }

            await context.SaveChangesAsync();

            return await FindBySlugAsync(entity.Slug, userId, isAdmin);
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
            var slug = toAdd.Name.ToKebabCase();

            var exists = await context.Species.AnyAsync(s => s.Slug == slug);

            if (exists)
            {
                return Result<Specie>.Conflict("Specie already exists");
            }

            if (userId is null)
            {
                return Result<Specie>.Forbidden("You should be authenticate to add a specie");
            }

            var entity = new SpecieEntity
            {
                UserId = userId,
                Slug = slug,
                Name = toAdd.Name,
                TypeLabel = toAdd.Type,
                DietLabel = toAdd.Diet,
                Height = toAdd.Height,
                Weight = toAdd.Weight,
                UploadedImageId = toAdd.Image,
                IsPublic = false
            };

            await context.Species.AddAsync(entity);
            await context.SaveChangesAsync();

            var specie = await context.Species.FirstAsync(s => s.Slug == entity.Slug);
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

    public async Task<Result<Specie>> DeleteAsync(string slug, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Species.FirstOrDefaultAsync(s => s.Slug == slug);

            if (entity is null)
                return Result<Specie>.NotFound($"Specie '{slug}' not found.");

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