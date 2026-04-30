namespace BioDomes.Domains.Core;

public class FoodChainNode
{
    public string Slug { get; }
    public string Name { get; }
    public int Count { get; }
    public Diet Diet { get; }
    public Classification Classification { get; }
 
    public FoodChainNode(string slug, string name, Diet diet, Classification classification, int count)
    {
        Slug = slug;
        Name = name;
        Diet = diet;
        Classification = classification;
        Count = count;
    }
}