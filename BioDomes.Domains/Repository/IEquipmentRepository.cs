using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IEquipmentRepository
{
    Task<List<Equipment>> GetAllAsync(string? userId, bool isAdmin);
    Task<List<EquipmentResource>> GetResourcesAsync();
    Task<ResultT<int>> CountAsync();
    Task<ResultT<Equipment>> CreateEquipmentAsync(CreateEquipmentRequest createRequest, string userId);
    Task<ResultT<Equipment>> FindAsync(int id, string userId, bool isAdmin);
    Task<ResultT<Equipment>> DeleteAsync(int id, string userId, bool isAdmin);
    Task<ResultT<Equipment>> UpdateAsync(UpdateEquipmentRequest toUpdate, string userId, bool isAdmin);
}