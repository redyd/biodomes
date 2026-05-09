namespace BioDomes.Domains.Core;

public class FoodChainNode
{
    public int SpecieId { get; }
    public string Name { get; }
    public int Count { get; }
    public Diet Diet { get; }
    public Classification Classification { get; }
 
    public FoodChainNode(int specieId, string name, Diet diet, Classification classification, int count)
    {
        SpecieId = specieId;
        Name = name;
        Diet = diet;    
        Classification = classification;
        Count = count;
    }
}