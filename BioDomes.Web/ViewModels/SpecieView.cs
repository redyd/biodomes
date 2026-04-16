namespace BioDomes.Web.ViewModels;

public class SpecieView
{
    public required string Slug { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Diet { get; set; }
    public required string Height { get; set; }
    public required string Weight { get; set; }
    public required string? ImageUrl { get; set; }
    public required bool CanEdit { get; set; }
}
