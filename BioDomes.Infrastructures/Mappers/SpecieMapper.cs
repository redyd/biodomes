using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public static class SpecieMapper
{
    public static Specie MapSpecie(SpecieEntity specie)
        => new()
        {
            CreatedBy = specie.UserId,
            Name =  specie.Name,
            Type = specie.TypeLabel,
            Diet =  specie.DietLabel,
            Height =  specie.Height,
            Weight = specie.Weight,
            ImageUrl = specie.UploadedImageId
        };
}
