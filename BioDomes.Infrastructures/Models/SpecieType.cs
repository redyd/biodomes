namespace BioDomes.Infrastructures.Models;

public class SpecieType
{
    public string Label { get; set; } = null!;
    
    // relation
    public List<Specie> Species { get; set; } = [];
}
