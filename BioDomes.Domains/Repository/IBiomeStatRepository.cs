using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IBiomeStatRepository
{
    Task<ResultT<List<BiomeStatsDto>>> GetStatsAsync(string? userId, bool isAdmin);
    Task<ResultT<List<BiomeHistory>>> GetHistoryAsync(string? biomeSlug, string? userId, bool isAdmin, int days);
}