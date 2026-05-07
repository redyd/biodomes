using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IBiomeRepository
{
    Task<ResultT<List<Biome>>> GetAllAsync(string? userId, bool isAdmin);
    Task<ResultT<List<ComplexBiome>>> GetAllComplexAsync(string? userId, bool isAdmin);
    
    Task<ResultT<string>> GetCreatorAsync(int biomeId);
    
    Task<ResultT<Biome>> FindAsync(int id, string? userId, bool isAdmin);
    Task<ResultT<ComplexBiome>> FindComplexAsync(int id, string? userId, bool isAdmin);
    
    Task<ResultT<Biome>> CreateBiomeAsync(CreateBiomeRequest createRequest, string userId);
    Task<ResultT<Biome>> UpdateAsync(UpdateBiomeRequest toUpdate, string? userId, bool isAdmin);
    Task<ResultT<Biome>> DeleteAsync(int id, string userId, bool isAdmin);
}