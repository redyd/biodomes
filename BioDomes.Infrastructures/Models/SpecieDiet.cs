namespace BioDomes.Infrastructures.Models;

public class SpecieDiet
{
    public int Id  { get; set; }
    public string Slug { get; set; } = null!;
    public string Label { get; set; } = null!;

    public List<SpecieClassification> CanEatClassifications { get; set; } = new();
}