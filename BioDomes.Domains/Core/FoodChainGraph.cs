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

    public IReadOnlyList<FoodChainNode> PreyOf(string predatorSlug)
    {
        var preyIds = Edges
            .Where(e => e.PredatorId == predatorSlug)
            .Select(e => e.PreyId)
            .ToHashSet();

        return Nodes
            .Where(n => preyIds.Contains(n.Slug))
            .ToList();
    }

    public IReadOnlyList<FoodChainNode> AvailablePreyFor(string predatorSlug)
    {
        var target = Nodes.FirstOrDefault(n => n.Slug == predatorSlug);
        if (target == null) return new List<FoodChainNode>();

        var alreadyConnectedPreys = Edges
            .Where(e => e.PredatorId == predatorSlug)
            .Select(e => e.PreyId)
            .ToHashSet();

        return Nodes
            .Where(n =>
                n.Slug != predatorSlug &&
                !alreadyConnectedPreys.Contains(n.Slug) &&
                n.Classification.EatenByDiets.Any(t => t.Id == target.Diet.Id)
            )
            .ToList();
    }
}