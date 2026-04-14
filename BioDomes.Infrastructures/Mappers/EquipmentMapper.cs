using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public static class EquipmentMapper
{
    public static Equipment MapEquipment(EquipmentEntity entity)
        => new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Consumed = MapResource(entity.ConsumedResourceEntity),
            Produced = MapResource(entity.ProducedResourceEntity),
            ImageId = entity.Image
        };

    private static EquipmentResource MapResource(EquipmentResourceEntity entity)
        => new()
        {
            Id = entity.Id,
            Label = entity.Label,
        };
}