namespace BioDomes.Domains.Core;

public class Diet
{
    public int Id { get; }
    public string Label { get; }
    
    public static bool operator ==(Diet a, Diet b)
    {
        return a.Id == b.Id && a.Label == b.Label;
    }

    public static bool operator !=(Diet a, Diet b)
    {
        return !(a == b);
    }

    public Diet(int id, string label)
    {
        Id = id;
        Label = label;
    }
}