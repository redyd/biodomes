using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Tests.Mappers;

[TestFixture]
public class BiomeMapperTests
{
    [Test]
    public void should_map_simple_biome()
    {
        var entity = CreateBiomeEntity();

        var result = BiomeMapper.MapSimpleBiome(entity);

        Assert.That(result.Slug, Is.EqualTo(entity.Slug));
        Assert.That(result.Name, Is.EqualTo(entity.Name));
        Assert.That(result.CreatedBy, Is.EqualTo(entity.UserId));
        Assert.That(result.IsPublic, Is.EqualTo(entity.IsPublic));
        Assert.That(result.Temperature, Is.EqualTo(entity.Temperature));
        Assert.That(result.Humidity, Is.EqualTo(entity.Humidity));
    }

    [Test]
    public void should_map_complex_biome_with_species_and_equipments()
    {
        var entity = CreateBiomeEntity(withRelations: true);

        var result = BiomeMapper.MapComplexBiome(entity);

        Assert.That(result.Slug, Is.EqualTo(entity.Slug));
        Assert.That(result.Name, Is.EqualTo(entity.Name));

        Assert.That(result.Species.Count, Is.EqualTo(1));
        Assert.That(result.Equipments.Count, Is.EqualTo(1));

        Assert.That(result.Species[0].Model.Name, Is.EqualTo("Deer"));
        Assert.That(result.Equipments[0].Model.Name, Is.EqualTo("Tree"));
    }

    [Test]
    public void should_group_species_by_specie_id()
    {
        var entity = CreateBiomeEntity(withRelations: true);

        entity.BiomeSpecies.Add(new BiomeSpecieEntity
        {
            Id = 2,
            BiomeId = 1,
            SpecieId = 10,
            Specie = entity.BiomeSpecies.First().Specie,
            Inserted = DateTime.UtcNow
        });

        var result = BiomeMapper.MapComplexBiome(entity);

        Assert.That(result.Species.Count, Is.EqualTo(1));

        Assert.That(
            result.Species[0].TrackedInsertionsById.Count,
            Is.EqualTo(2)
        );
    }

    [Test]
    public void should_group_equipments_by_equipment_id()
    {
        var entity = CreateBiomeEntity(withRelations: true);

        entity.BiomeEquipments.Add(new BiomeEquipmentEntity
        {
            Id = 2,
            BiomeId = 1,
            EquipmentId = 20,
            Equipment = entity.BiomeEquipments.First().Equipment,
            Inserted = DateTime.UtcNow
        });

        var result = BiomeMapper.MapComplexBiome(entity);

        Assert.That(result.Equipments.Count, Is.EqualTo(1));

        Assert.That(
            result.Equipments[0].TrackedInsertionsById.Count,
            Is.EqualTo(2)
        );
    }

    private static BiomeEntity CreateBiomeEntity(bool withRelations = false)
    {
        var biome = new BiomeEntity
        {
            Slug = "forest",
            Name = "Forest",
            Temperature = 20,
            Humidity = 70,
            ImageUrl = "img.png",
            IsPublic = true,
            UserId = "user-1",
            BiomeSpecies = new List<BiomeSpecieEntity>(),
            BiomeEquipments = new List<BiomeEquipmentEntity>()
        };

        if (!withRelations)
            return biome;

        var diet = new SpecieDiet
        {
            Id = 1,
            Label = "Herbivore"
        };

        var classification = new SpecieClassification
        {
            Id = 1,
            Label = "Mammal",
            RequireFood = true,
            EatenByDiets = new List<SpecieDiet>()
        };

        var specie = new SpecieEntity
        {
            Id = 10,
            Name = "Deer",
            Slug = "deer",
            Height = 100,
            Weight = 80,
            UploadedImageId = "deer.png",
            IsPublic = true,
            UserId = "user-1",
            DietEntity = diet,
            ClassificationEntity = classification,
            BiomeSpecies = new List<BiomeSpecieEntity>()
        };

        var equipment = new EquipmentEntity
        {
            Id = 20,
            Name = "Tree",
            Image = "tree.png",
            IsPublic = true,
            UserId = "user-1",
            ConsumedResourceEntity = new EquipmentResourceEntity
            {
                Id = 1,
                Label = "Water"
            },
            ProducedResourceEntity = new EquipmentResourceEntity
            {
                Id = 2,
                Label = "Oxygen"
            }
        };

        var biomeSpecie = new BiomeSpecieEntity
        {
            Id = 1,
            BiomeId = 1,
            SpecieId = specie.Id,
            Specie = specie,
            Inserted = DateTime.UtcNow
        };

        var biomeEquipment = new BiomeEquipmentEntity
        {
            Id = 1,
            BiomeId = 1,
            EquipmentId = equipment.Id,
            Equipment = equipment,
            Inserted = DateTime.UtcNow
        };

        biome.BiomeSpecies.Add(biomeSpecie);
        biome.BiomeEquipments.Add(biomeEquipment);

        specie.BiomeSpecies.Add(biomeSpecie);

        return biome;
    }
}