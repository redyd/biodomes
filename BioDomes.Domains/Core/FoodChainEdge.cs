namespace BioDomes.Domains.Core;

public class FoodChainEdge
{
    public string PredatorId { get; init; }
    public string PreyId { get; init; }

    public FoodChainEdge(string predatorId, string preyId)
    {
        PredatorId = predatorId;
        PreyId = preyId;
    }
}