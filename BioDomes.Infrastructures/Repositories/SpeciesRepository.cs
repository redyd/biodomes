using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.Repositories;

public class SpeciesRepository(BiodomeContext context) : ISpeciesRepository
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

    public async Task<Specie?> FindBySlugAsync(string slug)
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
                UploadedImageId = toUpdate.ImageUrl
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

    public async Task<Specie> AddAsync(Specie toAdd)
    {
        if (await context.Species.AnyAsync(specie => specie.Name.ToLower() == toAdd.Name.ToLower()))
        {
            throw new InvalidOperationException($"Une espèce nommée '{toAdd.Name}' existe déjà.");
        }

        var entity = new SpecieEntity
        {
            Slug = toAdd.Slug,
            Name = toAdd.Name,
            TypeLabel = toAdd.Type,
            DietLabel = toAdd.Diet,
            Height = toAdd.Height,
            Weight = toAdd.Weight,
            UploadedImageId = toAdd.ImageUrl
        };

        await context.Species.AddAsync(entity);
        await context.SaveChangesAsync();

        return (await FindBySlugAsync(entity.Slug))!;
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
