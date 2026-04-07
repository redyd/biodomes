using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Repositories;

public class SpeciesRepository(BiodomeContext context) : ISpeciesRepository
{
    public List<Specie> FindAll()
    {
        return context.Species
            .Select(SpecieMapper.MapSpecie)
            .ToList();
    }

    public Specie? FindBySlug(string slug)
    {
        var search = context.Species
            .FirstOrDefault(specie => specie.Slug == slug);

        return search == null ? null : SpecieMapper.MapSpecie(search);
    }

    public Specie? Update(string previousSlug, Specie toUpdate)
    {
        if (context.Species.Any(specie =>
                specie.Slug != previousSlug &&
                specie.Name.ToLower() == toUpdate.Name.ToLower()))
        {
            throw new InvalidOperationException($"Une espèce nommée '{toUpdate.Name}' existe déjà.");
        }

        var entity = context.Species.FirstOrDefault(specie => specie.Slug == previousSlug);
        if (entity == null)
        {
            return null;
        }

        if (entity.Slug != toUpdate.Slug)
        {
            var replacement = new SpecieEntity
            {
                Slug = toUpdate.Slug,
                Name = toUpdate.Name,
                TypeLabel = toUpdate.Type.ToString(),
                DietLabel = toUpdate.Diet,
                Height = toUpdate.Height,
                Weight = toUpdate.Weight,
                UploadedImageId = toUpdate.ImageUrl
            };

            context.Species.Remove(entity);
            context.Species.Add(replacement);
            context.SaveChanges();

            return FindBySlug(replacement.Slug);
        }

        entity.Name = toUpdate.Name;
        entity.TypeLabel = toUpdate.Type.ToString();
        entity.DietLabel = toUpdate.Diet;
        entity.Height = toUpdate.Height;
        entity.Weight = toUpdate.Weight;
        entity.UploadedImageId = toUpdate.ImageUrl;

        context.SaveChanges();

        return FindBySlug(entity.Slug);
    }

    public Specie Add(Specie toAdd)
    {
        if (context.Species.Any(specie => specie.Name.ToLower() == toAdd.Name.ToLower()))
        {
            throw new InvalidOperationException($"Une espèce nommée '{toAdd.Name}' existe déjà.");
        }

        var entity = new SpecieEntity
        {
            Slug = toAdd.Slug,
            Name = toAdd.Name,
            TypeLabel = toAdd.Type.ToString(),
            DietLabel = toAdd.Diet,
            Height = toAdd.Height,
            Weight = toAdd.Weight,
            UploadedImageId = toAdd.ImageUrl
        };

        context.Species.Add(entity);
        context.SaveChanges();

        return FindBySlug(entity.Slug)!;
    }

    public Specie? Delete(string slug)
    {
        var entity = context.Species.FirstOrDefault(specie => specie.Slug == slug);

        if (entity == null)
            return null;

        var deleted = SpecieMapper.MapSpecie(entity);

        context.Species.Remove(entity);
        context.SaveChanges();

        return deleted;
    }
}
