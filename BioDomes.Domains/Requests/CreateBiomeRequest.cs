namespace BioDomes.Domains.Requests;

public class CreateBiomeRequest
{
    public string Name { get; set; }
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public string? ImageUrl { get; set; }
    public required string UserId { get; set; }
}