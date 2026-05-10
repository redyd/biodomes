namespace BioDomes.Domains.Core;

public class Classification(string slug, string label, List<Diet> eatenByDiets, bool requireFood)
{
    public string Slug { get; } = slug;
    public string Label { get; } = label;
    public List<Diet> EatenByDiets { get; } = eatenByDiets;
    public bool RequireFood { get; } = requireFood;
    
    public static bool operator==(Classification left, Classification right) => left.Equals(right);
    public static bool operator !=(Classification left, Classification right) => !(left == right);

    protected bool Equals(Classification other)
    {
        return Label == other.Label;
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
        return Label.GetHashCode();
    }
}