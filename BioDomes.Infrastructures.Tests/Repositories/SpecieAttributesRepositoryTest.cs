using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;
using BioDomes.Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BioDomes.Infrastructures.Tests.Repositories;

[TestFixture]
public class SpecieAttributesRepositoryTests
{
    private BiodomeContext _context = null!;
    private SpecieAttributesRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<BiodomeContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new BiodomeContext(options);

        var logger = new Mock<ILogger<SpecieAttributesRepository>>();
        _repository = new SpecieAttributesRepository(_context, logger.Object);

        Seed();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private void Seed()
    {
        _context.SpecieClassifications.AddRange(
            CreateClassification("mammal"),
            CreateClassification("bird")
        );

        _context.SpecieDiets.AddRange(
            CreateDiet("herbivore"),
            CreateDiet("carnivore")
        );

        _context.SaveChanges();
    }

    private SpecieClassification CreateClassification(string label)
    {
        return new SpecieClassification
        {
            Label = label,
            RequireFood = true
        };
    }

    private SpecieDiet CreateDiet(string label)
    {
        return new SpecieDiet
        {
            Label = label
        };
    }

    [Test]
    public async Task should_return_all_classifications()
    {
        var result = await _repository.GetClassifications();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value, Is.Not.Empty);
        Assert.That(result.Value.Any(c => c.Label == "mammal"), Is.True);
    }

    [Test]
    public async Task should_return_all_diets()
    {
        var result = await _repository.GetDiets();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value, Is.Not.Empty);
        Assert.That(result.Value.Any(d => d.Label == "herbivore"), Is.True);
    }

    [Test]
    public async Task should_return_empty_classifications_when_none_exist()
    {
        _context.SpecieClassifications.RemoveRange(_context.SpecieClassifications);
        _context.SaveChanges();

        var result = await _repository.GetClassifications();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value, Is.Empty);
    }

    [Test]
    public async Task should_return_empty_diets_when_none_exist()
    {
        _context.SpecieDiets.RemoveRange(_context.SpecieDiets);
        _context.SaveChanges();

        var result = await _repository.GetDiets();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value, Is.Empty);
    }
}