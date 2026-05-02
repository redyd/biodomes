namespace BioDomes.Infrastructures.Models;

public class BiomeEquipmentEntity
{
    public int Id { get; set; }
    public DateTime Inserted { get; set; }
    
    public int BiomeId { get; set; }
    public BiomeEntity Biome { get; set; }
    
    public int EquipmentId { get; set; }
    public EquipmentEntity Equipment { get; set; }
}