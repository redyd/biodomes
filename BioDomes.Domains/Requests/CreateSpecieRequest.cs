namespace BioDomes.Domains.Requests;

public class CreateSpecieRequest
{
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required string Diet { get; set; }
    public required int Height { get; set; }
    public required int Weight { get; set; }
    public required string? Image { get; set; }
}