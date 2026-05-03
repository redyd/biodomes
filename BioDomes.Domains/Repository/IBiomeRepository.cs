using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IBiomeRepository
{
    Task<List<Biome>> GetAllAsync(string? userId, bool isAdmin);
    Task<ResultT<string>> GetCreatorAsync(int biomeId);

    Task<ResultT<int>> CountAsync();

    Task<ResultT<Biome>> FindAsync(int id, string? userId, bool isAdmin);
    Task<ResultT<ComplexBiome>> FindComplexAsync(int id, string? userId, bool isAdmin);

    Task<ResultT<Biome>> CreateBiomeAsync(CreateBiomeRequest createRequest, string userId);

    Task<ResultT<Biome>> UpdateAsync(UpdateBiomeRequest toUpdate, string? userId, bool isAdmin);

    Task<ResultT<Biome>> DeleteAsync(int id, string userId, bool isAdmin);

    Task<Result> AddSpecie(SpecieToBiomeRequest request, int biomeId, string? userId, bool isAdmin);
    Task<Result> RemoveSpecie(SpecieToBiomeRequest request, int biomeId, string? userId, bool isAdmin);
    
    Task<Result> AddEquipment(EquipmentToBiomeRequest request, int biomeId, string? userId, bool isAdmin);
    Task<Result> RemoveEquipment(EquipmentToBiomeRequest request, int biomeId, string? userId, bool isAdmin);
}