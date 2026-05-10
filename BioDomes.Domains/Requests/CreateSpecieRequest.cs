namespace BioDomes.Domains.Requests;

public class CreateSpecieRequest
{
    public required string Name { get; init; }
    public required int Height { get; init; }
    public required int Weight { get; init; }
    public required string? Image { get; init; }

    public required string ClassificationSlug { get; init; }
    public required string DietSlug { get; init; }
}