namespace BioDomes.Domains.Core;

public class FoodChainNode(Specie specie, int count)
{
    public Specie Specie { get; } = specie;
    public int Count { get; } = count;
}