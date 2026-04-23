using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Tests.TestSupport;

internal static class TestData
{
    public static SpecieClassification Mammal() => new() { Id = 1, Label = "Mammal" };
    public static SpecieClassification Reptile() => new() { Id = 2, Label = "Reptile" };
    public static SpecieDiet Herbivore() => new() { Id = 1, Label = "Herbivore" };
    public static SpecieDiet Carnivore() => new() { Id = 2, Label = "Carnivore" };

    public static EquipmentResourceEntity Water() => new() { Id = 1, Label = "Water" };
    public static EquipmentResourceEntity Electricity() => new() { Id = 2, Label = "Electricity" };

    public static SpecieEntity CreateSpecie(
        string slug,
        string name,
        string userId,
        bool isPublic,
        int classificationId = 1,
        int dietId = 1,
        string? image = "image-id")
        => new()
        {
            Slug = slug,
            Name = name,
            UserId = userId,
            User = null!,
            IsPublic = isPublic,
            ClassificationId = classificationId,
            ClassificationEntity = null!,
            DietId = dietId,
            DietEntity = null!,
            Height = 10,
            Weight = 20,
            UploadedImageId = image
        };

    public static EquipmentEntity CreateEquipment(
        int id,
        string name,
        string userId,
        bool isPublic,
        int consumedResourceId = 1,
        int producedResourceId = 2,
        string image = "equipment-image")
        => new()
        {
            Id = id,
            Name = name,
            UserId = userId,
            User = null!,
            IsPublic = isPublic,
            ConsumedResourceId = consumedResourceId,
            ConsumedResourceEntity = null!,
            ProducedResourceId = producedResourceId,
            ProducedResourceEntity = null!,
            Image = image,
            Biomes = []
        };
}
