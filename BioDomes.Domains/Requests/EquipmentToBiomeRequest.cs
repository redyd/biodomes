namespace BioDomes.Domains.Requests;

public class EquipmentToBiomeRequest
{
    public required string EquipmentSlug { get; init; }
    public required int Count { get; init; }
}