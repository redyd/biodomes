namespace BioDomes.Infrastructures.Models;

public class SpecieDiet
{
    public int Id  { get; set; }
    public string Label { get; set; } = null!;
    
    public List<SpecieClassification> CanEatClassifications { get; set; } = new();
}