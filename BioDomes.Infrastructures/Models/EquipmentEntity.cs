namespace BioDomes.Infrastructures.Models;

public class EquipmentEntity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Image { get; set; }
    
    // relation
    public required int ProducedResourceId { get; set; }
    public required EquipmentResourceEntity ProducedResourceEntity { get; set; }
    
    public required int ConsumedResourceId { get; set; }
    public required EquipmentResourceEntity ConsumedResourceEntity { get; set; }
}