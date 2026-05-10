using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Models;
using BioDomes.Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BioDomes.Infrastructures.Tests.Repositories;

[TestFixture]
public class BiomeManagementRepositoryTests
{
    private BiodomeContext _context = null!;
    private BiomeManagementRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<BiodomeContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new BiodomeContext(options);

        var logger = new Mock<ILogger<IBiomeManagementRepository>>();

        _repository = new BiomeManagementRepository(_context, logger.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    [Test]
    public async Task should_return_not_found_when_biome_does_not_exist_on_add_specie()
    {
        var request = new SpecieToBiomeRequest
        {
            SpecieSlug = "wolf",
            Count = 1
        };

        var result = await _repository.AddSpecieToBiome(request, "unknown", "user", false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task should_return_forbidden_when_user_is_not_owner_on_add_specie()
    {
        var biome = CreateBiome("owner");
        var specie = CreateSpecie();

        _context.Biomes.Add(biome);
        _context.Species.Add(specie);

        await _context.SaveChangesAsync();

        var request = new SpecieToBiomeRequest
        {
            SpecieSlug = specie.Slug,
            Count = 1
        };

        var result = await _repository.AddSpecieToBiome(
            request,
            biome.Slug,
            "other-user",
            false
        );

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
    }

    [Test]
    public async Task should_add_species_to_biome()
    {
        var biome = CreateBiome();
        var specie = CreateSpecie();

        _context.Biomes.Add(biome);
        _context.Species.Add(specie);

        await _context.SaveChangesAsync();

        var request = new SpecieToBiomeRequest
        {
            SpecieSlug = specie.Slug,
            Count = 3
        };

        var result = await _repository.AddSpecieToBiome(
            request,
            biome.Slug,
            "user",
            false
        );

        var inserted = await _context.BiomeSpecies
            .Where(x => x.BiomeId == biome.Id && x.SpecieId == specie.Id)
            .ToListAsync();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(inserted.Count, Is.EqualTo(3));
    }

    [Test]
    public async Task should_remove_species_from_biome()
    {
        var biome = CreateBiome();
        var specie = CreateSpecie();

        _context.Biomes.Add(biome);
        _context.Species.Add(specie);

        await _context.SaveChangesAsync();

        _context.BiomeSpecies.AddRange(
            new BiomeSpecieEntity
            {
                BiomeId = biome.Id,
                SpecieId = specie.Id
            },
            new BiomeSpecieEntity
            {
                BiomeId = biome.Id,
                SpecieId = specie.Id
            },
            new BiomeSpecieEntity
            {
                BiomeId = biome.Id,
                SpecieId = specie.Id
            }
        );

        await _context.SaveChangesAsync();

        var request = new SpecieToBiomeRequest
        {
            SpecieSlug = specie.Slug,
            Count = 2
        };

        var result = await _repository.RemoveSpecieToBiome(
            request,
            biome.Slug,
            "user",
            false
        );

        var remaining = await _context.BiomeSpecies
            .Where(x => x.BiomeId == biome.Id && x.SpecieId == specie.Id)
            .ToListAsync();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(remaining.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task should_add_equipments_to_biome()
    {
        var biome = CreateBiome();
        var equipment = CreateEquipment();

        _context.Biomes.Add(biome);
        _context.Equipments.Add(equipment);

        await _context.SaveChangesAsync();

        var request = new EquipmentToBiomeRequest
        {
            EquipmentSlug = equipment.Slug,
            Count = 2
        };

        var result = await _repository.AddEquipmentToBiome(
            request,
            biome.Slug,
            "user",
            false
        );

        var inserted = await _context.BiomeEquipments
            .Where(x => x.BiomeId == biome.Id && x.EquipmentId == equipment.Id)
            .ToListAsync();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(inserted.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task should_remove_equipments_from_biome()
    {
        var biome = CreateBiome();
        var equipment = CreateEquipment();

        _context.Biomes.Add(biome);
        _context.Equipments.Add(equipment);

        await _context.SaveChangesAsync();

        _context.BiomeEquipments.AddRange(
            new BiomeEquipmentEntity
            {
                BiomeId = biome.Id,
                EquipmentId = equipment.Id
            },
            new BiomeEquipmentEntity
            {
                BiomeId = biome.Id,
                EquipmentId = equipment.Id
            },
            new BiomeEquipmentEntity
            {
                BiomeId = biome.Id,
                EquipmentId = equipment.Id
            }
        );

        await _context.SaveChangesAsync();

        var request = new EquipmentToBiomeRequest
        {
            EquipmentSlug = equipment.Slug,
            Count = 2
        };

        var result = await _repository.RemoveEquipmentToBiome(
            request,
            biome.Slug,
            "user",
            false
        );

        var remaining = await _context.BiomeEquipments
            .Where(x => x.BiomeId == biome.Id && x.EquipmentId == equipment.Id)
            .ToListAsync();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(remaining.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task should_return_forbidden_when_user_is_not_owner_on_remove_equipment()
    {
        var biome = CreateBiome("owner");
        var equipment = CreateEquipment();

        _context.Biomes.Add(biome);
        _context.Equipments.Add(equipment);

        await _context.SaveChangesAsync();

        var request = new EquipmentToBiomeRequest
        {
            EquipmentSlug = equipment.Slug,
            Count = 1
        };

        var result = await _repository.RemoveEquipmentToBiome(
            request,
            biome.Slug,
            "other-user",
            false
        );

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
    }

    private static UserEntity CreateUser(string id = "user")
    {
        return new UserEntity
        {
            Id = id,
            UserName = id,
            Email = $"{id}@test.com",
            Birthdate = DateOnly.FromDateTime(DateTime.UtcNow),
            SearchOrganism = "test",
            ImageUrl = null
        };
    }

    private static BiomeEntity CreateBiome(string userId = "user")
    {
        return new BiomeEntity
        {
            Id = 1,
            Slug = "forest",
            Name = "Forest",
            Temperature = 25,
            Humidity = 70,
            IsPublic = true,
            ImageUrl = null,
            UserId = userId,
            User = CreateUser(userId),
            BiomeSpecies = new List<BiomeSpecieEntity>(),
            BiomeEquipments = new List<BiomeEquipmentEntity>()
        };
    }

    private static SpecieEntity CreateSpecie()
    {
        var diet = new SpecieDiet
        {
            Id = 1,
            Slug = "carnivore",
            Label = "Carnivore",
            CanEatClassifications = new List<SpecieClassification>()
        };

        var classification = new SpecieClassification
        {
            Id = 1,
            Slug = "mammal",
            Label = "Mammal",
            RequireFood = true,
            EatenByDiets = new List<SpecieDiet>()
        };

        return new SpecieEntity
        {
            Id = 1,
            Slug = "wolf",
            Name = "Wolf",
            Height = 100,
            Weight = 50,
            IsPublic = true,
            UserId = "user",
            UploadedImageId = null,

            DietId = diet.Id,
            DietEntity = diet,

            ClassificationId = classification.Id,
            ClassificationEntity = classification,

            BiomeSpecies = new List<BiomeSpecieEntity>()
        };
    }

    private static EquipmentEntity CreateEquipment()
    {
        return new EquipmentEntity
        {
            Id = 1,
            Slug = "tree",
            Name = "Tree",
            Image = "tree.png",
            UserId = "user",
            IsPublic = true,

            ConsumedResourceEntity = new EquipmentResourceEntity
            {
                Slug = "water",
                Label = "Water"
            },

            ProducedResourceEntity = new EquipmentResourceEntity
            {
                Slug = "oxygen",
                Label = "Oxygen"
            }
        };
    }
}