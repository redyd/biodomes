using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IEquipmentRepository
{
    Task<ResultT<List<Equipment>>> GetAllAsync(string? userId, bool isAdmin);
    Task<ResultT<List<EquipmentResource>>> GetResourcesAsync();
    Task<ResultT<Equipment>> CreateEquipmentAsync(CreateEquipmentRequest createRequest, string userId);
    Task<ResultT<Equipment>> FindAsync(string slug, string userId, bool isAdmin);
    Task<ResultT<Equipment>> DeleteAsync(string slug, string userId, bool isAdmin);
    Task<ResultT<Equipment>> UpdateAsync(UpdateEquipmentRequest toUpdate, string userId, bool isAdmin);
}