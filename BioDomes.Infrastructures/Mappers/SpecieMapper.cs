using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public static class SpecieMapper
{
    public static Specie MapSpecie(SpecieEntity specie)
        => new()
        {
            Id =  specie.Id,
            CreatedBy = specie.UserId,
            Name = specie.Name,
            Classification = MapClassification(specie.ClassificationEntity),
            Diet = MapDiet(specie.DietEntity),
            Height = specie.Height,
            Weight = specie.Weight,
            ImageUrl = specie.UploadedImageId,
            IsPublic = specie.IsPublic
        };

    public static Classification MapClassification(SpecieClassification classification)
        => new(classification.Id, classification.Label, classification.EatenByDiets.Select(MapDiet).ToList());

    public static Diet MapDiet(SpecieDiet diet) => new(diet.Id, diet.Label);
}