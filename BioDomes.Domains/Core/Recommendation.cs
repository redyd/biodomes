namespace BioDomes.Domains.Core;

public class Recommendation<T>
{
    public required RecommendationType Type { get; init; }
    public required Func<Task> Resolve { get; init; }
    public required string Message { get; init; } 
    public required List<T> Involved { get; init; } 
}