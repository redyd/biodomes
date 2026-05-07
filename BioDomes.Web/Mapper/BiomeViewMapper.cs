using BioDomes.Domains.Core;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Mapper;

public class BiomeViewMapper : IBiomeViewMapper
{
    public BiomeView<ComplexBiome> MapSingle(ComplexBiome b, bool isAdmin, string? userId,
        Func<string?, string> resolveImage)
    {
        var rule = new HumidityRule(b.Temperature, b.Humidity);

        return new BiomeView<ComplexBiome>
        {
            ImageUrl = resolveImage(b.ImageUrl),
            CanEdit = userId is not null && (isAdmin || b.CreatedBy == userId),
            IsPublic = true,
            MaxHumidity = rule.MaxHumidity,
            HumidityPercent = rule.HumidityPercent,
            Model = b
        };
    }

    public List<BiomeView<Biome>> MapAll(List<Biome> biomes, bool isAdmin, string? userId, Func<string?, string> resolveImage)
    {
        return biomes.Select(b => MapSingle(b, isAdmin, userId, resolveImage)).ToList();
    }

    public List<BiomeView<ComplexBiome>> MapAll(List<ComplexBiome> biomes, bool isAdmin, string? userId, Func<string?, string> resolveImage)
    {
        return biomes.Select(b => MapSingle(b, isAdmin, userId, resolveImage)).ToList();
    }

    public BiomeView<Biome> MapSingle(Biome biome, bool isAdmin, string? userId, Func<string?, string> resolveImage)
    {
        return new BiomeView<Biome>
        {
            ImageUrl = resolveImage(biome.ImageUrl),
            CanEdit = userId is not null && (isAdmin || biome.CreatedBy == userId),
            IsPublic = true,
            Model = biome
        };
    }
}