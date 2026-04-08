namespace BioDomes.Infrastructures.Models;

public class EquipmentEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public bool IsPublic { get; set; }

    // relation
    public int ProducedResourceId { get; set; }
    public EquipmentResourceEntity ProducedResourceEntity { get; set; } = null!;

    public int ConsumedResourceId { get; set; }
    public EquipmentResourceEntity ConsumedResourceEntity { get; set; } = null!;
    
    public List<BiomeEntity> Biomes { get; set; }
}