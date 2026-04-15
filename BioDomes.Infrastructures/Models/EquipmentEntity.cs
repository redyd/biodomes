namespace BioDomes.Infrastructures.Models;

public class EquipmentEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
    public bool IsPublic { get; set; }

    // user
    public string UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    // relation
    public int ProducedResourceId { get; set; }
    public EquipmentResourceEntity ProducedResourceEntity { get; set; } = null!;

    public int ConsumedResourceId { get; set; }
    public EquipmentResourceEntity ConsumedResourceEntity { get; set; } = null!;
    
    public List<BiomeEntity> Biomes { get; set; } = null!;
}