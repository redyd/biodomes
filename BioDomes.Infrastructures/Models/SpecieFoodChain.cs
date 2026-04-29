namespace BioDomes.Infrastructures.Models;

public class SpecieFoodChain
{
    public string PredatorId { get; set; }
    public SpecieEntity Predator { get; set; }

    public string PreyId { get; set; }
    public SpecieEntity Prey { get; set; }
}