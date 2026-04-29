namespace BioDomes.Domains.Core;

public class Diet
{
    public int Id { get; }
    public string Label { get; }
    
    public Diet(int id, string label)
    {
        Id = id;
        Label = label;
    }
}