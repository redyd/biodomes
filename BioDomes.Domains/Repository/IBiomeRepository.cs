using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IBiomeRepository
{
    Task<List<Biome>> GetAllAsync(string? userId, bool isAdmin);
    Task<Result<string>> GetCreatorAsync(int biomeId);

    Task<Result<int>> CountAsync();

    Task<Result<Biome>> FindAsync(int id, string? userId, bool isAdmin);
    
    Task<Result<Biome>> CreateBiomeAsync(CreateBiomeRequest createRequest, string userId);

    Task<Result<Biome>> UpdateAsync(UpdateBiomeRequest toUpdate, string? userId, bool isAdmin);
    
    Task<Result<Biome>> DeleteAsync(int id, string userId, bool isAdmin);
}