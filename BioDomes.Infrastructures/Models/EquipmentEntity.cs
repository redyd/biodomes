namespace BioDomes.Infrastructures.Models;

public class EquipmentEntity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required bool IsPublic { get; set; }

    // relation
    public int ProducedResourceId { get; set; }
    public EquipmentResourceEntity ProducedResourceEntity { get; set; } = null!;

    public int ConsumedResourceId { get; set; }
    public EquipmentResourceEntity ConsumedResourceEntity { get; set; } = null!;
}