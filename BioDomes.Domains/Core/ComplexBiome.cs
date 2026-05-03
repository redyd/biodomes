namespace BioDomes.Domains.Core;

public class ComplexBiome : Biome
{
    public required List<Tracked<Specie>> Species { get; init; }
    public required List<Tracked<Equipment>> Equipments { get; init; }
}