using BioDomes.Domains.Core;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Mapper;

public interface IBiomeViewMapper
{
    BiomeView<ComplexBiome> MapSingle(ComplexBiome b, bool isAdmin, string? userId,
        Func<string?, string> resolveImage);

    BiomeView<Biome> MapSingle(Biome biome, bool isAdmin, string? userId, Func<string?, string> resolveImage);
    List<BiomeView<Biome>> MapAll(List<Biome> biomes, bool isAdmin, string? userId, Func<string?, string> resolveImage);
    List<BiomeView<ComplexBiome>> MapAll(List<ComplexBiome> biomes, bool isAdmin, string? userId, Func<string?, string> resolveImage);
}