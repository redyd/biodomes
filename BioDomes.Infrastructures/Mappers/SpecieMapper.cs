using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public static class SpecieMapper
{
    public static Specie MapSpecie(SpecieEntity specie)
        => new()
        {
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
        => new()
        {
            Id = classification.Id,
            Label = classification.Label,
        };

    public static Diet MapDiet(SpecieDiet diet)
        => new()
        {
            Id = diet.Id,
            Label = diet.Label,
        };
}