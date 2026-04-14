using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IEquipmentRepository
{
    Task<List<Equipment>> GetAllAsync(string? userId, bool isAdmin);
    Task<List<EquipmentResource>> GetResourcesAsync();
    Task<Result<Equipment>> CreateEquipmentAsync(CreateEquipmentRequest createRequest, string userId);
    Task<Result<Equipment>> FindAsync(int id, string userId, bool isAdmin);
    Task<Result<Equipment>> DeleteAsync(int id, string userId, bool isAdmin);
    Task UpdateAsync(UpdateEquipmentRequest equipment, string userId, bool isAdmin);
}