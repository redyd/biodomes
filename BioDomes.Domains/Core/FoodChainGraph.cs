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
    
    /// <summary>
    /// Récupère les espèces sans proies
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<FoodChainNode> NoPreySpecies()
    {
        var predatorIds = Edges
            .Select(e => e.PredatorId)
            .ToHashSet();

        return Nodes
            .Where(n => !predatorIds.Contains(n.SpecieId))
            .ToList();
    }

    /// <summary>
    /// Retourne le nombre de fois qu'un id d'une espèce se retrouve dans les liens (specieId - count)
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, int> CountRelations()
    {
        var dict = new Dictionary<int, int>();

        foreach (var node in Nodes)
        {
            dict[node.SpecieId] = Edges.Count(edge => edge.PreyId == node.SpecieId || edge.PredatorId == node.SpecieId);
        }

        return dict;
    }

    /// <summary>
    /// Détermine le nombre de proies qu'une espèce a
    /// </summary>
    /// <returns></returns>
    public Dictionary<int, int> CountPreyRelations()
    {
        var dict = new Dictionary<int, int>();

        foreach (var node in Nodes)
        {
            dict[node.SpecieId] = Edges.Count(edge => edge.PredatorId == node.SpecieId);
        }

        return dict;
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