namespace BioDomes.Domains.Core;

public class Equipment
{
    public required string Slug { get; init; }
    public required string CreatedBy { get; init; }
    public required string Name { get; init; }
    public required string ImageId { get; init; }
    public required bool IsPublic { get; init; }
    
    public required EquipmentResource Produced { get; init; }
    public required EquipmentResource Consumed { get; init; }
}