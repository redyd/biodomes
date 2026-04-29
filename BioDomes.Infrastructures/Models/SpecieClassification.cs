
namespace BioDomes.Infrastructures.Models;

public class SpecieClassification
{
    public int Id  { get; set; }
    public string Label { get; set; }
    
    // relation
    public List<SpecieDiet> EatenByDiets { get; set; } = new();
}