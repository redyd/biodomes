using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IEquipmentRepository
{
    Task<List<Equipment>> GetAllAsync();
    Task<Dictionary<int, string>> GetResourcesAsync();
    Task CreateEquipmentAsync(CreateEquipmentRequest createRequest);
}