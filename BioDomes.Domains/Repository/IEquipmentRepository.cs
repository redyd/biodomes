using BioDomes.Domains.Core;

namespace BioDomes.Domains.Repository;

public interface IEquipmentRepository
{
    Task<List<Equipment>> GetAllAsync();
}