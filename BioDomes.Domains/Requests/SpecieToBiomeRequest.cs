namespace BioDomes.Domains.Requests;

public class SpecieToBiomeRequest
{
    public required int SpecieId { get; init; }
    public required int Count { get; init; }
}