namespace BioDomes.Domains.Requests;

public class UpdateSpecieRequest
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int Height { get; init; }
    public required int Weight { get; init; }
    public string? ImageUrl { get; set; }
    public bool? IsPublic { get; set; }

    public required int ClassificationId { get; init; }
    public required int DietId { get; init; }
}