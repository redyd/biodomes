namespace BioDomes.Domains.Core;

public class Diet
{
    public int Id { get; }
    public string Label { get; }
    
    public static bool operator ==(Diet? a, Diet? b)
    {
        if (a is null && b is null) return true;
        if (a is null || b is null) return false;
        return a.Id == b.Id && a.Label == b.Label;
    }

    public static bool operator !=(Diet? a, Diet? b)
    {
        return !(a == b);
    }

    public Diet(int id, string label)
    {
        Id = id;
        Label = label;
    }
    
    protected bool Equals(Diet other)
    {
        return Id == other.Id && Label == other.Label;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Diet)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Label);
    }

}