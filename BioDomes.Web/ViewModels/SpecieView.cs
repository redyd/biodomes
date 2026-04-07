namespace BioDomes.Web.ViewModels;

public class SpecieView
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Diet { get; set; }
    public required int Height { get; set; }
    public required int Weight { get; set; }
    public required string? ImageUrl { get; set; }
}
