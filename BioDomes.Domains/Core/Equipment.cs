namespace BioDomes.Domains.Core;

public class Equipment
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string ImageId { get; set; }
    
    public required string Produced { get; set; }
    public required string Consumed { get; set; }
    public required bool IsPublic { get; set; }
}