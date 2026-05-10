using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public static class EquipmentMapper
{
    public static Equipment MapEquipment(EquipmentEntity entity)
        => new()
        {
            CreatedBy = entity.UserId,
            Slug = entity.Slug,
            Name = entity.Name,
            Consumed = MapResource(entity.ConsumedResourceEntity),
            Produced = MapResource(entity.ProducedResourceEntity),
            ImageId = entity.Image,
            IsPublic = entity.IsPublic,
        };

    private static EquipmentResource MapResource(EquipmentResourceEntity entity)
        => new()
        {
            Slug = entity.Slug,
            Label = entity.Label,
        };
}