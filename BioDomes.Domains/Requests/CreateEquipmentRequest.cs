namespace BioDomes.Domains.Requests;

public class CreateEquipmentRequest
{
    public string Name { get; set; }
    public string Image { get; set; }
    public int ProducedResourceId { get; set; }
    public int ConsumedResourceId { get; set; }
}