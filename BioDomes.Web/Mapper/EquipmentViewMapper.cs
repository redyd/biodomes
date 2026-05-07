using BioDomes.Domains.Core;
using BioDomes.Web.ViewModels;

namespace BioDomes.Web.Mapper;

public class EquipmentViewMapper : IEquipmentViewMapper
{
    public EquipmentView MapSingle(Equipment equipment, bool isAdmin, string? userId, Func<string?, string> resolveImage)
    {
        return new EquipmentView
        {
            Id = equipment.Id,
            Name = equipment.Name,
            Consumed = equipment.Consumed.Label,
            Image = resolveImage(equipment.ImageId),
            Produced = equipment.Produced.Label,
            CanEdit = equipment.CreatedBy == userId || isAdmin
        };
    }

    public List<EquipmentView> MapAll(List<Equipment> equipment, bool isAdmin, string? userId, Func<string?, string> resolveImage)
    {
        return equipment.Select(e => MapSingle(e, isAdmin, userId, resolveImage)).ToList();
    }
}