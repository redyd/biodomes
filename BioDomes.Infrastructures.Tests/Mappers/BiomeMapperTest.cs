// using BioDomes.Infrastructures.Mappers;
// using BioDomes.Infrastructures.Models;
//
// namespace BioDomes.Infrastructures.Tests.Mappers;
//
// [TestFixture]
// public class BiomeMapperTests
// {
//     [Test]
//     public void should_map_simple_biome()
//     {
//         var entity = CreateBiomeEntity();
//
//         var result = BiomeMapper.MapSimpleBiome(entity);
//
//         Assert.That(result.Id, Is.EqualTo(entity.Id));
//         Assert.That(result.Name, Is.EqualTo(entity.Name));
//         Assert.That(result.CreatedBy, Is.EqualTo(entity.UserId));
//         Assert.That(result.IsPublic, Is.EqualTo(entity.IsPublic));
//     }
//
//     [Test]
//     public void should_map_complex_biome_with_species_and_equipments()
//     {
//         var entity = CreateBiomeEntity(withRelations: true);
//
//         var result = BiomeMapper.MapComplexBiome(entity);
//
//         Assert.That(result.Id, Is.EqualTo(entity.Id));
//         Assert.That(result.Species.Count, Is.GreaterThan(0));
//         Assert.That(result.Equipments.Count, Is.GreaterThan(0));
//     }
//
//     private static BiomeEntity CreateBiomeEntity(bool withRelations = false)
//     {
//         var biome = new BiomeEntity
//         {
//             Id = 1,
//             Name = "Forest",
//             Temperature = 20,
//             Humidity = 70,
//             ImageUrl = "img.png",
//             IsPublic = true,
//             UserId = "user-1",
//             BiomeSpecies = new List<BiomeSpecieEntity>(),
//             BiomeEquipments = new List<BiomeEquipmentEntity>()
//         };
//
//         if (!withRelations)
//             return biome;
//
//         var diet = new SpecieDiet
//         {
//             Id = 1,
//             Label = "Herbivore"
//         };
//
//         var classification = new SpecieClassification
//         {
//             Id = 1,
//             Label = "Mammal",
//             RequireFood = true
//         };
//
//         var specie = new SpecieEntity
//         {
//             Id = 10,
//             Name = "Deer",
//             DietEntity = diet,
//             ClassificationEntity = classification,
//             BiomeSpecies = new List<BiomeSpecieEntity>()
//         };
//
//         var equipment = new EquipmentEntity
//         {
//             Id = 20,
//             Name = "Tree",
//             Image = "tree.png",
//             ConsumedResourceEntity = new EquipmentResourceEntity
//             {
//                 Id = 1,
//                 Label = "Water"
//             },
//             ProducedResourceEntity = new EquipmentResourceEntity
//             {
//                 Id = 2,
//                 Label = "Oxygen"
//             }
//         };
//
//         var biomeSpecie = new BiomeSpecieEntity
//         {
//             Id = 1,
//             BiomeId = biome.Id,
//             SpecieId = specie.Id,
//             Specie = specie,
//             Inserted = DateTime.UtcNow
//         };
//
//         var biomeEquipment = new BiomeEquipmentEntity
//         {
//             Id = 1,
//             BiomeId = biome.Id,
//             EquipmentId = equipment.Id,
//             Equipment = equipment,
//             Inserted = DateTime.UtcNow
//         };
//
//         biome.BiomeSpecies.Add(biomeSpecie);
//         biome.BiomeEquipments.Add(biomeEquipment);
//
//         specie.BiomeSpecies.Add(biomeSpecie);
//
//         return biome;
//     }
// }