namespace BioDomes.Domains.Core;

public class FoodChainGraph(IReadOnlyList<FoodChainNode> nodes, IReadOnlyList<FoodChainEdge> edges)
{
    public IReadOnlyList<FoodChainNode> Nodes { get; } = nodes;
    public IReadOnlyList<FoodChainEdge> Edges { get; } = edges;

    /// <summary>
    /// Retourne le nombre de fois qu'un id d'une espèce se retrouve dans les liens (specieId - count)
    /// </summary>
    /// <returns></returns>
    public Dictionary<Specie, int> CountRelations()
    {
        var dict = new Dictionary<Specie, int>();

        foreach (var node in Nodes)
        {
            dict[node.Specie] = Edges
                .Count(edge => edge.PreySlug == node.Specie.Slug || edge.PredatorSlug == node.Specie.Slug);
        }

        return dict;
    }
    
    public IReadOnlyList<FoodChainNode> PreyOf(string predatorSlug)
    {
        var preyIds = Edges
            .Where(e => e.PredatorSlug == predatorSlug)
            .Select(e => e.PreySlug)
            .ToHashSet();

        return Nodes
            .Where(n => preyIds.Contains(n.Specie.Slug))
            .ToList();
    }

    public IReadOnlyList<FoodChainNode> AvailablePreyFor(string predatorSlug)
    {
        var target = Nodes.FirstOrDefault(n => n.Specie.Slug == predatorSlug);
        if (target == null) return new List<FoodChainNode>();

        var alreadyConnectedPreys = Edges
            .Where(e => e.PredatorSlug == predatorSlug)
            .Select(e => e.PreySlug)
            .ToHashSet();

        return Nodes
            .Where(n =>
                n.Specie.Slug != predatorSlug &&
                !alreadyConnectedPreys.Contains(n.Specie.Slug) &&
                n.Specie.Classification.EatenByDiets.Any(t => t.Slug == target.Specie.Diet.Slug)
            )
            .ToList();
    }
}