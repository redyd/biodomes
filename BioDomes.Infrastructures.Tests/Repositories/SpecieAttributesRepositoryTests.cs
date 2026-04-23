using BioDomes.Infrastructures.Repositories;
using BioDomes.Infrastructures.Tests.TestSupport;
using Microsoft.Extensions.Logging.Abstractions;

namespace BioDomes.Infrastructures.Tests.Repositories;

[TestFixture]
public class SpecieAttributesRepositoryTests
{
    [Test]
    public async Task GetClassifications_ReturnsMappedClassifications()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new SpecieAttributesRepository(context, NullLogger<SpecieAttributesRepository>.Instance);

        var result = await repository.GetClassifications();

        Assert.That(result.Select(c => c.Label), Does.Contain("Mammifère"));
    }

    [Test]
    public async Task GetDiets_ReturnsMappedDiets()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new SpecieAttributesRepository(context, NullLogger<SpecieAttributesRepository>.Instance);

        var result = await repository.GetDiets();

        Assert.That(result.Select(d => d.Label), Does.Contain("Carnivore"));
    }
}
