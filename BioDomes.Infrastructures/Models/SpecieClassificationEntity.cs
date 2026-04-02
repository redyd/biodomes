namespace BioDomes.Infrastructures.Models;

public class SpecieClassificationEntity
{
    public string Label { get; set; } = null!;
    
    // relation
    public List<SpecieEntity> Species { get; set; } = [];
}
