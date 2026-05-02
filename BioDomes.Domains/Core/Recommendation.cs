namespace BioDomes.Domains.Core;

public class Recommendation<T>
{
    public required RecommendationType Type { get; init; }
    public required string ActionId { get; init; }
    public required string Label { get; init; }
    public required string Message { get; init; } 
    public required T Involved { get; init; } 
}