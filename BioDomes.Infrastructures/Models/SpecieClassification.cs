
namespace BioDomes.Infrastructures.Models;

public class SpecieClassification
{
    public int Id  { get; set; }
    public string Slug { get; set; } = null!;
    public string Label { get; set; } = null!;
    public bool RequireFood { get; set; }
    
    // relation
    public List<SpecieDiet> EatenByDiets { get; set; } = new();
}