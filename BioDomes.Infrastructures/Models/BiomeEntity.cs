namespace BioDomes.Infrastructures.Models;

public class BiomeEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string? ImageUrl { get; set; }
    
    public bool IsPublic { get; set; }

    // User
    public string UserId { get; set; }
    public UserEntity User { get; set; }
    
    // relations
    public List<SpecieEntity> Species { get; set; } = new();
    public List<EquipmentEntity> Equipments { get; set; } = new();
}