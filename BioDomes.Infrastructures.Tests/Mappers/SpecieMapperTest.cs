using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Tests.Mappers;

[TestFixture]
public class SpecieMapperTests
{
    [Test]
    public void should_map_specie_correctly()
    {
        var entity = CreateSpecieEntity();

        var result = SpecieMapper.MapSpecie(entity);

        Assert.That(result.Id, Is.EqualTo(entity.Id));
        Assert.That(result.Name, Is.EqualTo(entity.Name));
        Assert.That(result.CreatedBy, Is.EqualTo(entity.UserId));
        Assert.That(result.Height, Is.EqualTo(entity.Height));
        Assert.That(result.Weight, Is.EqualTo(entity.Weight));
        Assert.That(result.ImageUrl, Is.EqualTo(entity.UploadedImageId));
        Assert.That(result.IsPublic, Is.EqualTo(entity.IsPublic));

        Assert.That(result.Classification.Id, Is.EqualTo(entity.ClassificationEntity.Id));
        Assert.That(result.Diet.Id, Is.EqualTo(entity.DietEntity.Id));
    }

    [Test]
    public void should_map_classification_with_diets()
    {
        var classification = CreateClassificationEntity();

        var result = SpecieMapper.MapClassification(classification);

        Assert.That(result.Id, Is.EqualTo(classification.Id));
        Assert.That(result.Label, Is.EqualTo(classification.Label));
        Assert.That(result.RequireFood, Is.EqualTo(classification.RequireFood));
    }

    [Test]
    public void should_map_diet_correctly()
    {
        var diet = new SpecieDiet
        {
            Id = 1,
            Label = "Carnivore"
        };

        var result = SpecieMapper.MapDiet(diet);

        Assert.That(result.Id, Is.EqualTo(diet.Id));
        Assert.That(result.Label, Is.EqualTo(diet.Label));
    }

    private static SpecieEntity CreateSpecieEntity()
    {
        return new SpecieEntity
        {
            Id = 1,
            Name = "Wolf",
            UserId = "user-1",
            Height = 120,
            Weight = 40,
            UploadedImageId = "wolf.png",
            IsPublic = true,
            DietEntity = new SpecieDiet
            {
                Id = 10,
                Label = "Carnivore"
            },
            ClassificationEntity = CreateClassificationEntity()
        };
    }

    private static SpecieClassification CreateClassificationEntity()
    {
        return new SpecieClassification
        {
            Id = 20,
            Label = "Mammal",
            RequireFood = true,
            EatenByDiets = new List<SpecieDiet>
            {
                new SpecieDiet
                {
                    Id = 11,
                    Label = "Herbivore"
                }
            }
        };
    }
}