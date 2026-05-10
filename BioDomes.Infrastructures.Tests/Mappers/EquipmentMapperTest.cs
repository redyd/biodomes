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

        Assert.That(result.Slug, Is.EqualTo(entity.Slug));
        Assert.That(result.Name, Is.EqualTo(entity.Name));
        Assert.That(result.CreatedBy, Is.EqualTo(entity.UserId));
        Assert.That(result.ImageId, Is.EqualTo(entity.Image));
        Assert.That(result.IsPublic, Is.EqualTo(entity.IsPublic));

        Assert.That(result.Consumed.Slug, Is.EqualTo(entity.ConsumedResourceEntity.Slug));
        Assert.That(result.Consumed.Label, Is.EqualTo(entity.ConsumedResourceEntity.Label));

        Assert.That(result.Produced.Slug, Is.EqualTo(entity.ProducedResourceEntity.Slug));
        Assert.That(result.Produced.Label, Is.EqualTo(entity.ProducedResourceEntity.Label));
    }

    private static EquipmentEntity CreateEquipmentEntity()
    {
        return new EquipmentEntity
        {
            Slug = "filter",
            Name = "Filter",
            Image = "filter.png",
            UserId = "user-1",
            IsPublic = true,

            ConsumedResourceEntity = new EquipmentResourceEntity
            {
                Slug = "water",
                Label = "Water"
            },

            ProducedResourceEntity = new EquipmentResourceEntity
            {
                Slug = "oxygen",
                Label = "Oxygen"
            }
        };
    }
}