namespace BioDomes.Domains.Requests;

public class CreateEquipmentRequest
{
    public required string Name { get; init; }
    public required string Image { get; init; }
    public required string ProducedResourceSlug { get; init; }
    public required string ConsumedResourceSlug { get; init; }
    public required string UserId { get; set; }
}