namespace BioDomes.Domains.Requests;

public class AddSpecieToBiomeRequest
{
    public required int SpecieId { get; init; }
    public required int Count { get; init; }
}