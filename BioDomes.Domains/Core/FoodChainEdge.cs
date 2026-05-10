namespace BioDomes.Domains.Core;

public class FoodChainEdge(string predatorSlug, string preySlug)
{
    public string PredatorSlug { get; } = predatorSlug;
    public string PreySlug { get; } = preySlug;
}