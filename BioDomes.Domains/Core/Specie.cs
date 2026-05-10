using BioDomes.Domains.Extensions;

namespace BioDomes.Domains.Core;

public class Specie
{
    public string Slug => Name.ToKebabCase();
    public required string Name { get; set; }
    public required int Height { get; set; }
    public required int Weight { get; set; }
    public required string? ImageUrl { get; set; }
    public required bool IsPublic { get; init; }
    public required string CreatedBy { get; set; }

    public required Diet Diet { get; set; }
    public required Classification Classification { get; set; }
}