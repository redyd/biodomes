using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public static class SpecieMapper
{
    public static Specie MapSpecie(SpecieEntity specie)
        => new(specie.Name, specie.TypeLabel, specie.DietLabel,
            specie.Height, specie.Weight, specie.UploadedImageId);
}
