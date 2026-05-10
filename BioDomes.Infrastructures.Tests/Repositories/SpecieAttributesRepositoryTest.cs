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

    private SpecieClassification CreateClassification(string slug)
    {
        return new SpecieClassification
        {
            Slug = slug,
            Label = slug,
            RequireFood = true
        };
    }

    private SpecieDiet CreateDiet(string slug)
    {
        return new SpecieDiet
        {
            Slug = slug,
            Label = slug
        };
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

    [Test]
    public async Task should_return_all_classifications()
    {
        var result = await _repository.GetClassifications();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count, Is.EqualTo(2));
        Assert.That(result.Value.Any(c => c.Label == "mammal"), Is.True);
    }

    [Test]
    public async Task should_return_all_diets()
    {
        var result = await _repository.GetDiets();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count, Is.EqualTo(2));
        Assert.That(result.Value.Any(d => d.Label == "herbivore"), Is.True);
    }

    [Test]
    public async Task should_return_empty_classifications_when_none_exist()
    {
        _context.SpecieClassifications.RemoveRange(_context.SpecieClassifications);
        await _context.SaveChangesAsync();

        var result = await _repository.GetClassifications();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task should_return_empty_diets_when_none_exist()
    {
        _context.SpecieDiets.RemoveRange(_context.SpecieDiets);
        await _context.SaveChangesAsync();

        var result = await _repository.GetDiets();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value, Is.Not.Null);
        Assert.That(result.Value!.Count, Is.EqualTo(0));
    }
}