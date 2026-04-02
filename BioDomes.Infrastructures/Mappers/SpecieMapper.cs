using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public static class SpecieMapper
{
    public static Specie MapSpecie(SpecieEntity specie)
        => new(specie.Name, specie.ClassificationEntity.Label, specie.DietEntity.Label,
            specie.Height, specie.Weight, specie.UploadedImageId);
}