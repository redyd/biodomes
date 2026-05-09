using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;
using BioDomes.Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BioDomes.Infrastructures.Tests.Repositories;

[TestFixture]
public class FoodChainRepositoryTests
{
    private BiodomeContext _context = null!;
    private FoodChainRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<BiodomeContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new BiodomeContext(options);

        var logger = new Mock<ILogger<FoodChainRepository>>();
        _repository = new FoodChainRepository(_context, logger.Object);

        Seed();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    private void Seed()
    {
        var (wolf, rabbit) = CreateSpeciesPair("user-1");

        _context.Species.AddRange(wolf, rabbit);

        _context.Biomes.Add(CreateBiome("user-1"));

        _context.SaveChanges();
    }

    private BiomeEntity CreateBiome(string userId)
    {
        return new BiomeEntity
        {
            Name = "Biome test",
            UserId = userId,
            IsPublic = true,
            Temperature = 20,
            Humidity = 50
        };
    }

    private SpecieEntity CreateSpecie(string name, string slug, string userId)
    {
        return new SpecieEntity
        {
            Name = name,
            Slug = slug,
            UserId = userId,
            IsPublic = true,
            DietEntity = new SpecieDiet
            {
                Label = "herbivore"
            },
            ClassificationEntity = new SpecieClassification
            {
                Label = "mammal",
                RequireFood = true
            }
        };
    }

    private (SpecieEntity predator, SpecieEntity prey) CreateSpeciesPair(string userId)
    {
        return (
            CreateSpecie("Wolf", "wolf", userId),
            CreateSpecie("Rabbit", "rabbit", userId)
        );
    }

    [Test]
    public async Task should_return_full_graph()
    {
        var result = await _repository.GetFullGraphAsync(null, true);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value.Nodes.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task should_return_graph_for_user()
    {
        var result = await _repository.GetFullGraphAsync("user-1", false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
    }

    [Test]
    public async Task should_return_not_found_biome()
    {
        var result = await _repository.GetFullGraphForBiomeAsync(999, null, false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task should_return_graph_for_biome()
    {
        var biome = CreateBiome("user-1");
        _context.Biomes.Add(biome);
        _context.SaveChanges();

        var result = await _repository.GetFullGraphForBiomeAsync(biome.Id, "user-1", false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
    }

    [Test]
    public async Task should_add_relation()
    {
        var (predator, prey) = CreateSpeciesPair("user-1");

        _context.Species.AddRange(predator, prey);
        _context.SaveChanges();

        var result = await _repository.AddRelation(predator.Id, prey.Id);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));

        var exists = _context.SpecieFoodChains.Any(r =>
            r.PredatorId == predator.Id &&
            r.PreyId == prey.Id);

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task should_delete_relation()
    {
        var (predator, prey) = CreateSpeciesPair("user-1");

        _context.Species.AddRange(predator, prey);
        _context.SaveChanges();

        _context.SpecieFoodChains.Add(new SpecieFoodChain
        {
            PredatorId = predator.Id,
            PreyId = prey.Id
        });

        _context.SaveChanges();

        var result = await _repository.DeleteRelation(predator.Id, prey.Id);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
    }
}