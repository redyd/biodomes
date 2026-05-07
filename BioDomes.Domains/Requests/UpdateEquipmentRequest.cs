namespace BioDomes.Domains.Requests;

public class UpdateEquipmentRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int ProducedResourceId { get; set; }
    public required int ConsumedResourceId { get; set; }
    public required string Image { get; set; }
    public bool? IsPublic { get; set; }
}