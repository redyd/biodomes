namespace BioDomes.Domains.Requests;

public class CreateEquipmentRequest
{
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
    public int ProducedResourceId { get; set; }
    public int ConsumedResourceId { get; set; }
    public int UserId { get; set; }
}