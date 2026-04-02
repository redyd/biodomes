namespace BioDomes.Infrastructures.Models;

public class SpecieDiet
{
    public string Label { get; set; } = null!;
    
    // relation
    public List<Specie> Species { get; set; } = [];
}
