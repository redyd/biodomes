using BioDomes.Domains.Core;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Mapper;

public interface IEquipmentViewMapper
{
    EquipmentView MapSingle(Equipment equipment, bool isAdmin, string? userId, Func<string?, string> resolveImage);
    List<EquipmentView> MapAll(List<Equipment> equipment, bool isAdmin, string? userId, Func<string?, string> resolveImage);
    
}