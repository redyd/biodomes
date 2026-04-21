namespace BioDomes.Domains.Requests;

public class UpdateEquipmentRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProducedResourceId { get; set; }
    public int ConsumedResourceId { get; set; }
    public string Image { get; set; }
    public bool? IsPublic { get; set; }
}