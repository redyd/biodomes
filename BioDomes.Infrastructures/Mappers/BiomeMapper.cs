using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public class BiomeMapper
{
    public static Biome MapBiome(BiomeEntity entity)
        => new()
        {
             CreatedBy = entity.UserId,
             Id = entity.Id,
             Name = entity.Name,
             Temperature = entity.Temperature,
             Humidity = entity.Humidity,
             ImageUrl = entity.ImageUrl,
             IsPublic = entity.IsPublic,
        };
}