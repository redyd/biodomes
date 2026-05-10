namespace BioDomes.Domains.Requests;

public class UpdateEquipmentRequest
{
    public required string PreviousSlug { get; init; }
    public required string Name { get; init; }
    public required string ProducedResourceSlug { get; init; }
    public required string ConsumedResourceSlug { get; init; }
    public required string Image { get; set; }
    public bool? IsPublic { get; set; }
}