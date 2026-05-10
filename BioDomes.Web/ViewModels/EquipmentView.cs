namespace BioDomes.Web.ViewModels;

public class EquipmentView
{
    public required string Slug { get; init; }
    public required string Name { get; init; }
    public required string Image { get; init; }
    public required string Produced { get; init; }
    public required string Consumed { get; init; }
    public required bool CanEdit { get; init; }
}