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
            Consumed = entity.ConsumedResourceEntity.Label,
            Produced = entity.ProducedResourceEntity.Label,
            ImageId = entity.Image
        };
}