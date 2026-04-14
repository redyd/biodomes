using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IEquipmentRepository
{
    Task<List<Equipment>> GetAllAsync(int? userId, bool isAdmin);
    Task<List<EquipmentResource>> GetResourcesAsync();
    Task CreateEquipmentAsync(CreateEquipmentRequest createRequest, int userId);
    Task<Equipment?> FindAsync(int id, int userId);
    Task<Equipment?> DeleteAsync(int id, int userId);
    Task UpdateAsync(UpdateEquipmentRequest equipment, int userId);
}