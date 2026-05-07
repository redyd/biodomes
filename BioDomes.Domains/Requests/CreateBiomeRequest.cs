namespace BioDomes.Domains.Requests;

public class CreateBiomeRequest
{
    public required string Name { get; set; }
    public required double Temperature { get; set; }
    public required double Humidity { get; set; }
    public required bool IsPublic  { get; set; }
    public string? ImageUrl { get; set; }
}