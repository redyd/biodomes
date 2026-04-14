using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IEquipmentRepository
{
    Task<List<Equipment>> GetAllAsync(string? userId, bool isAdmin);
    Task<List<EquipmentResource>> GetResourcesAsync();
    Task CreateEquipmentAsync(CreateEquipmentRequest createRequest, string userId);
    Task<Equipment?> FindAsync(int id, string userId);
    Task<Equipment?> DeleteAsync(int id, string userId);
    Task UpdateAsync(UpdateEquipmentRequest equipment, string userId);
}