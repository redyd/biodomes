using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;

namespace BioDomes.Domains.Repository;

public interface IBiomeManagementRepository
{
    Task<Result> AddSpecieToBiome(SpecieToBiomeRequest request, string biomeSlug, string? userId, bool isAdmin);
    Task<Result> RemoveSpecieToBiome(SpecieToBiomeRequest request, string biomeSlug, string? userId, bool isAdmin);
    
    Task<Result> AddEquipmentToBiome(EquipmentToBiomeRequest request, string biomeSlug, string? userId, bool isAdmin);
    Task<Result> RemoveEquipmentToBiome(EquipmentToBiomeRequest request, string biomeSlug, string? userId, bool isAdmin);
}