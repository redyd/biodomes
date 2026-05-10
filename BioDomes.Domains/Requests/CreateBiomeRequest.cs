namespace BioDomes.Domains.Requests;

public class CreateBiomeRequest
{
    public required string Name { get; init; }
    public required double Temperature { get; init; }
    public required double Humidity { get; init; }
    public required bool IsPublic  { get; init; }
    public string? ImageUrl { get; init; }
}