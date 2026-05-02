namespace BioDomes.Domains.Core;

public class Tracked<T>
{
    public required T Model { get; init; }
    public required Dictionary<int, DateTime> TrackedInsertionsById { get; init; }
}