namespace BioDomes.Infrastructures.Models;

public class SpecieFoodChain
{
    public int PredatorId { get; set; }
    public SpecieEntity Predator { get; set; } = null!;

    public int PreyId { get; set; }
    public SpecieEntity Prey { get; set; } = null!;
}