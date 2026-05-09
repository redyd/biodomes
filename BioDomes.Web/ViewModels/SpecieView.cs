using BioDomes.Domains.Core;

namespace BioDomes.Web.ViewModels;

public class SpecieView
{
    public required string FormatedHeight { get; init; }
    public required string FormatedWeight { get; init; }
    public required string ImageUrl { get; init; }
    public required bool CanEdit { get; init; }
    public required Specie Model { get; init; }
}