namespace BioDomes.Domains.Core;

public class Classification
{
    public int Id { get; }
    public string Label { get; }
    public List<Diet> EatenByDiets { get; }

    public Classification(int id, string label, List<Diet> eatenByDiets)
    {
        Id = id;
        Label = label;
        EatenByDiets = eatenByDiets;
    }
}