namespace BioDomes.Domains.Core;

public class Equipment
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string ImageId { get; set; }
    
    public required EquipmentResource Produced { get; set; }
    public required EquipmentResource Consumed { get; set; }
}