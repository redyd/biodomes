namespace BioDomes.Domains.Core;

public class Classification
{
    public int Id { get; }
    public string Label { get; }
    public List<Diet> EatenByDiets { get; }
    public bool RequireFood { get; }

    public Classification(int id, string label, List<Diet> eatenByDiets, bool requireFood)
    {
        Id = id;
        Label = label;
        EatenByDiets = eatenByDiets;
        RequireFood = requireFood;
    }
    
    protected bool Equals(Classification other)
    {
        return Id == other.Id && Label == other.Label && EatenByDiets.Equals(other.EatenByDiets) && RequireFood == other.RequireFood;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Classification)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Label, EatenByDiets, RequireFood);
    }
    
    public static bool operator ==(Classification a, Classification b)
    {
        return a.Id == b.Id && a.Label == b.Label;
    }

    public static bool operator !=(Classification a, Classification b)
    {
        return !(a == b);
    }
}