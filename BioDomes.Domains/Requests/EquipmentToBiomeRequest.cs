namespace BioDomes.Domains.Requests;

public class EquipmentToBiomeRequest
{
    public required int EquipmentId { get; init; }
    public required int Count { get; init; }
}