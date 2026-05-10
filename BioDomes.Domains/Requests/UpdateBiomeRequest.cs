namespace BioDomes.Domains.Requests;

public class UpdateBiomeRequest
{
    public required string PreviousSlug { get; init; }
    public required string Name { get; init; }
    public required double Temperature { get; init; }
    public required double Humidity { get; init; }
    public string? ImageUrl { get; init; }
    public bool? IsPublic { get; init; }
}