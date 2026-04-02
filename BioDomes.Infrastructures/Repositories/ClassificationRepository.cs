using BioDomes.Domains.Repository;

namespace BioDomes.Infrastructures.Repositories;

public class ClassificationRepository(BiodomeContext context) : IClassificationRepository
{
    public List<string> FetchAll()
    {
        return context.SpeciesClassification
            .Select(classification => classification.Label)
            .ToList();
    }
}