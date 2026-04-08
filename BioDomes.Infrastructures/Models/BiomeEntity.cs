namespace BioDomes.Infrastructures.Models;

public class BiomeEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    
    // relations
    public List<SpecieEntity> Species { get; set; } = new();
    public List<EquipmentEntity> Equipments { get; set; } = new();
}