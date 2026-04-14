namespace BioDomes.Domains.Requests;

public class CreateEquipmentRequest
{
    public required string Name { get; set; }
    public required string Image { get; set; }
    public required int ProducedResourceId { get; set; }
    public required int ConsumedResourceId { get; set; }
    public required string UserId { get; set; }
}