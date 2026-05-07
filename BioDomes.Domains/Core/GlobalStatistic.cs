namespace BioDomes.Domains.Core;

public class GlobalStatistic
{
    public required List<ComplexBiome> All { get; init; }
    public int BiomesCount => All.Count;

    public List<ComplexBiome> GetBalancedBiome(int count)
    {
        return All
            .Where(cb => cb.State == BiomeState.Optimal)
            .OrderByDescending(cb => cb.SpeciesVariety)
            .Take(count)
            .ToList();
    }
}