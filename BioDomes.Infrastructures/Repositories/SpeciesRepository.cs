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
    public async Task<List<Specie>> FindAllAsync()
    {
        var entities = await context.Species.ToListAsync();
        return entities.Select(SpecieMapper.MapSpecie).ToList();
    }

    public async Task<int> CountAsync()
    {
        return await context.Species.CountAsync();
    }

    public async Task<Result<Specie>> FindBySlugAsync(string slug)
    {
        var entity = await context.Species
            .FirstOrDefaultAsync(specie => specie.Slug == slug);

        return entity == null ? null : SpecieMapper.MapSpecie(entity);
    }

    public async Task<Specie?> UpdateAsync(string previousSlug, Specie toUpdate)
    {
        if (await context.Species.AnyAsync(specie =>
                specie.Slug != previousSlug &&
                specie.Name.ToLower() == toUpdate.Name.ToLower()))
        {
            throw new InvalidOperationException($"Une espèce nommée '{toUpdate.Name}' existe déjà.");
        }

        var entity = await context.Species.FirstOrDefaultAsync(specie => specie.Slug == previousSlug);
        if (entity == null)
            return null;

        if (entity.Slug != toUpdate.Slug)
        {
            var replacement = new SpecieEntity
            {
                Slug = toUpdate.Slug,
                Name = toUpdate.Name,
                TypeLabel = toUpdate.Type,
                DietLabel = toUpdate.Diet,
                Height = toUpdate.Height,
                Weight = toUpdate.Weight,
                UploadedImageId = toUpdate.ImageUrl,
                IsPublic = false
            };

            context.Species.Remove(entity);
            await context.Species.AddAsync(replacement);
            await context.SaveChangesAsync();

            return await FindBySlugAsync(replacement.Slug);
        }

        entity.Name = toUpdate.Name;
        entity.TypeLabel = toUpdate.Type;
        entity.DietLabel = toUpdate.Diet;
        entity.Height = toUpdate.Height;
        entity.Weight = toUpdate.Weight;
        entity.UploadedImageId = toUpdate.ImageUrl;

        await context.SaveChangesAsync();

        return await FindBySlugAsync(entity.Slug);
    }

    public async Task<Result<Specie>> AddAsync(CreateSpecieRequest toAdd)
    {
        try
        {
            var slug = toAdd.Name.ToKebabCase();

            var exists = await context.Species
                .AnyAsync(s => s.Slug == slug);

            if (exists)
            {
                return Result<Specie>.Conflict("Specie already exists");
            }
            
            var entity = new SpecieEntity
            {
                Slug = toAdd.Name.ToKebabCase(),
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

            var specie = await context.Species.FirstAsync(specie => specie.Slug == entity.Slug);

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

    public async Task<Specie?> DeleteAsync(string slug)
    {
        var entity = await context.Species.FirstOrDefaultAsync(specie => specie.Slug == slug);

        if (entity == null) return null;

        var deleted = SpecieMapper.MapSpecie(entity);

        context.Species.Remove(entity);
        await context.SaveChangesAsync();

        return deleted;
    }
}