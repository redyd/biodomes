using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IEquipmentRepository
{
    Task<List<Equipment>> GetAllAsync();
    Task<List<EquipmentResource>> GetResourcesAsync();
    Task CreateEquipmentAsync(CreateEquipmentRequest createRequest);
    Task<Equipment?> FindAsync(int id);
    Task<Equipment?> DeleteAsync(int id);
    Task<Equipment> UpdateAsync(UpdateEquipmentRequest equipment);
}