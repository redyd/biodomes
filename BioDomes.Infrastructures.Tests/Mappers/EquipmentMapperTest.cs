using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Tests.Mappers;

[TestFixture]
public class EquipmentMapperTests
{
    [Test]
    public void should_map_equipment_correctly()
    {
        var entity = CreateEquipmentEntity();

        var result = EquipmentMapper.MapEquipment(entity);

        Assert.That(result.Id, Is.EqualTo(entity.Id));
        Assert.That(result.Name, Is.EqualTo(entity.Name));
        Assert.That(result.CreatedBy, Is.EqualTo(entity.UserId));
        Assert.That(result.ImageId, Is.EqualTo(entity.Image));
        Assert.That(result.IsPublic, Is.EqualTo(entity.IsPublic));

        Assert.That(result.Consumed.Id, Is.EqualTo(entity.ConsumedResourceEntity.Id));
        Assert.That(result.Consumed.Label, Is.EqualTo(entity.ConsumedResourceEntity.Label));

        Assert.That(result.Produced.Id, Is.EqualTo(entity.ProducedResourceEntity.Id));
        Assert.That(result.Produced.Label, Is.EqualTo(entity.ProducedResourceEntity.Label));
    }

    private static EquipmentEntity CreateEquipmentEntity()
    {
        return new EquipmentEntity
        {
            Id = 1,
            Name = "Filter",
            Image = "filter.png",
            UserId = "user-1",
            IsPublic = true,
            ConsumedResourceEntity = new EquipmentResourceEntity
            {
                Id = 10,
                Label = "Water"
            },
            ProducedResourceEntity = new EquipmentResourceEntity
            {
                Id = 20,
                Label = "Oxygen"
            }
        };
    }
}