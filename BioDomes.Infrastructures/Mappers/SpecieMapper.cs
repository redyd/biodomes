using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public static class SpecieMapper
{
    public static Specie MapSpecie(SpecieEntity specie)
        => new()
        {
            Slug = specie.Slug,
            Name = specie.Name,
            CreatedBy = specie.UserId,
            Classification = MapClassification(specie.ClassificationEntity),
            Diet = MapDiet(specie.DietEntity),
            Height = specie.Height,
            Weight = specie.Weight,
            ImageUrl = specie.UploadedImageId,
            IsPublic = specie.IsPublic
        };

    public static Classification MapClassification(SpecieClassification classification)
        => new(classification.Slug, classification.Label, classification.EatenByDiets.Select(MapDiet).ToList(),
            classification.RequireFood);

    public static Diet MapDiet(SpecieDiet diet) => new(diet.Slug, diet.Label);
}