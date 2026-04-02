using BioDomes.Domains.Extensions;

namespace BioDomes.Domains.Core;

public class Specie
{
    public string Slug => Name.ToKebabCase();
    public string Name { get; set; }
    public string Type { get; set; }
    public string Diet { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string? ImageUrl { get; set; }

    public Specie(string name, string type, string diet, int height, int weight, string? imageUrl = null)
    {
        Name = name;
        Type = type;
        Diet = diet;
        Height = height;
        Weight = weight;
        ImageUrl = imageUrl;
    }
}