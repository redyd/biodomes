using BioDomes.Domains.Extensions;

namespace BioDomes.Domains.Requests;

public class UpdateSpecieRequest
{
    public required string PreviousSlug { get; init; }
    public string Slug => Name.ToKebabCase();
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Diet { get; init; }
    public required int Height { get; init; }
    public required int Weight { get; init; }
    public string? ImageUrl { get; set; }
    public bool? IsPublic { get; set; }
}