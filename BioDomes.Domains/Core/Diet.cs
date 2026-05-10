using BioDomes.Domains.Extensions;

namespace BioDomes.Domains.Core;

public class Diet(string label)
{
    public string Slug => Label.ToKebabCase();
    public string Label { get; } = label;

    public static bool operator ==(Diet a, Diet b) => a.Equals(b);

    public static bool operator !=(Diet a, Diet b) => !(a == b);

    protected bool Equals(Diet other)
    {
        return Label == other.Label;
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
        return Label.GetHashCode();
    }
}