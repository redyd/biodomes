namespace BioDomes.Domains.Core;

public class Species
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Diet { get; set; }
    public int Height { get; set; }
    public string? ImageUrl { get; set; }

    public Species(string name, string type, string diet, int height, string? imageUrl = null)
    {
        Name = name;
        Type = type;
        Diet = diet;
        Height = height;
        ImageUrl = imageUrl;
    }
}