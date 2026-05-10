namespace BioDomes.Domains.Requests;

public class SpecieToBiomeRequest
{
    public required string SpecieSlug { get; init; }
    public required int Count { get; init; }
}