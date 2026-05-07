using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IBiomeManagementRepository
{
    Task<Result> AddSpecieToBiome(SpecieToBiomeRequest request, int biomeId, string? userId, bool isAdmin);
    Task<Result> RemoveSpecieToBiome(SpecieToBiomeRequest request, int biomeId, string? userId, bool isAdmin);
    
    Task<Result> AddEquipmentToBiome(EquipmentToBiomeRequest request, int biomeId, string? userId, bool isAdmin);
    Task<Result> RemoveEquipmentToBiome(EquipmentToBiomeRequest request, int biomeId, string? userId, bool isAdmin);
}