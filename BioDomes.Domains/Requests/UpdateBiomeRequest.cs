namespace BioDomes.Domains.Requests;

public class UpdateBiomeRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required double Temperature { get; set; }
    public required double Humidity { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsPublic { get; set; }
}