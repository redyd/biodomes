using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IBiomeRepository
{
    Task<ResultT<List<ComplexBiome>>> GetAllComplexAsync(string? userId, bool isAdmin);
    
    Task<ResultT<string>> GetCreatorAsync(string slug);
    
    Task<ResultT<Biome>> FindAsync(string slug, string? userId, bool isAdmin);
    Task<ResultT<ComplexBiome>> FindComplexAsync(string slug, string? userId, bool isAdmin);
    
    Task<ResultT<Biome>> CreateBiomeAsync(CreateBiomeRequest createRequest, string userId);
    Task<ResultT<Biome>> UpdateAsync(UpdateBiomeRequest toUpdate, string? userId, bool isAdmin);
    Task<ResultT<Biome>> DeleteAsync(string slug, string userId, bool isAdmin);
}