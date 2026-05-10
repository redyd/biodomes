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
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private void Seed()
    {
        var biome = new BiomeEntity
        {
            Id = 1,
            Slug = "forest",
            Name = "Biome test",
            UserId = "user-1",
            IsPublic = true,
            Temperature = 20,
            Humidity = 50
        };

        var wolf = new SpecieEntity
        {
            Id = 1,
            Name = "Wolf",
            Slug = "wolf",
            UserId = "user-1",
            IsPublic = true,
            DietEntity = new SpecieDiet { Id = 1, Slug = "carnivore", Label = "carnivore" },
            ClassificationEntity = new SpecieClassification
            {
                Id = 1,
                Slug = "mammal",
                Label = "mammal",
                RequireFood = true
            }
        };

        var rabbit = new SpecieEntity
        {
            Id = 2,
            Name = "Rabbit",
            Slug = "rabbit",
            UserId = "user-1",
            IsPublic = true,
            DietEntity = new SpecieDiet { Id = 2, Slug = "herbivore", Label = "herbivore" },
            ClassificationEntity = new SpecieClassification
            {
                Id = 2,
                Slug = "mammal",
                Label = "mammal",
                RequireFood = true
            }
        };

        _context.Biomes.Add(biome);
        _context.Species.AddRange(wolf, rabbit);

        _context.BiomeSpecies.AddRange(
            new BiomeSpecieEntity { BiomeId = 1, SpecieId = 1, Inserted = DateTime.UtcNow },
            new BiomeSpecieEntity { BiomeId = 1, SpecieId = 2, Inserted = DateTime.UtcNow }
        );

        _context.SaveChanges();
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
        var result = await _repository.GetFullGraphForBiomeAsync("unknown", null, false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task should_return_graph_for_biome()
    {
        var result = await _repository.GetFullGraphForBiomeAsync("forest", "user-1", false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value!.Nodes.Count, Is.GreaterThan(0));
    }

    [Test]
    public async Task should_add_relation()
    {
        var wolf = await _context.Species.FirstAsync(s => s.Slug == "wolf");
        var rabbit = await _context.Species.FirstAsync(s => s.Slug == "rabbit");

        var result = await _repository.AddRelation(wolf.Slug, rabbit.Slug);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));

        var exists = await _context.SpecieFoodChains.AnyAsync(r =>
            r.PredatorId == wolf.Id &&
            r.PreyId == rabbit.Id);

        Assert.That(exists, Is.True);
    }

    [Test]
    public async Task should_delete_relation()
    {
        var wolf = await _context.Species.FirstAsync(s => s.Slug == "wolf");
        var rabbit = await _context.Species.FirstAsync(s => s.Slug == "rabbit");

        _context.SpecieFoodChains.Add(new SpecieFoodChain
        {
            PredatorId = wolf.Id,
            PreyId = rabbit.Id
        });

        await _context.SaveChangesAsync();

        var result = await _repository.DeleteRelation(wolf.Slug, rabbit.Slug);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
    }
}