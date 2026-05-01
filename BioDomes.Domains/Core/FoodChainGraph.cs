namespace BioDomes.Domains.Core;

public class FoodChainGraph
{
    public IReadOnlyList<FoodChainNode> Nodes { get; }
    public IReadOnlyList<FoodChainEdge> Edges { get; }
 
    public FoodChainGraph(IReadOnlyList<FoodChainNode> nodes, IReadOnlyList<FoodChainEdge> edges)
    {
        Nodes = nodes;
        Edges = edges;
    }

    public IReadOnlyList<FoodChainNode> PreyOf(int predatorId)
    {
        var preyIds = Edges
            .Where(e => e.PredatorId == predatorId)
            .Select(e => e.PreyId)
            .ToHashSet();

        return Nodes
            .Where(n => preyIds.Contains(n.SpecieId))
            .ToList();
    }

    public IReadOnlyList<FoodChainNode> AvailablePreyFor(int predatorId)
    {
        var target = Nodes.FirstOrDefault(n => n.SpecieId == predatorId);
        if (target == null) return new List<FoodChainNode>();

        var alreadyConnectedPreys = Edges
            .Where(e => e.PredatorId == predatorId)
            .Select(e => e.PreyId)
            .ToHashSet();

        return Nodes
            .Where(n =>
                n.SpecieId != predatorId &&
                !alreadyConnectedPreys.Contains(n.SpecieId) &&
                n.Classification.EatenByDiets.Any(t => t.Id == target.Diet.Id)
            )
            .ToList();
    }
}