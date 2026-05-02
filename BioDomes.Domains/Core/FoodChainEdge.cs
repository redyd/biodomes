namespace BioDomes.Domains.Core;

public class FoodChainEdge
{
    public int PredatorId { get; init; }
    public int PreyId { get; init; }

    public FoodChainEdge(int predatorId, int preyId)
    {
        PredatorId = predatorId;
        PreyId = preyId;
    }
}