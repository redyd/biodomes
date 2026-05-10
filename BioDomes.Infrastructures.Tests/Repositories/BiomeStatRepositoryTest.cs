// using BioDomes.Domains.Repository;
// using BioDomes.Infrastructures.Models;
// using BioDomes.Infrastructures.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Moq;
//
// namespace BioDomes.Infrastructures.Tests.Repositories;
//
// [TestFixture]
// public class BiomeStatRepositoryTests
// {
//     private BiodomeContext _context = null!;
//     private BiomeStatRepository _repository = null!;
//
//     [SetUp]
//     public void Setup()
//     {
//         var options = new DbContextOptionsBuilder<BiodomeContext>()
//             .UseInMemoryDatabase(Guid.NewGuid().ToString())
//             .Options;
//
//         _context = new BiodomeContext(options);
//
//         var logger = new Mock<ILogger<IBiomeStatRepository>>();
//
//         _repository = new BiomeStatRepository(_context, logger.Object);
//     }
//
//     [TearDown]
//     public void TearDown()
//     {
//         _context.Dispose();
//     }
//     
//     [Test]
//     public async Task should_return_empty_list_when_no_biomes_exist()
//     {
//         var result = await _repository.GetStatsAsync("user", true);
//
//         Assert.That(result.Value, Is.Not.Null);
//         Assert.That(result.Value!.Count, Is.EqualTo(0));
//     }
//     
//     [Test]
//     public async Task should_compute_species_stats_correctly()
//     {
//         var user = new UserEntity
//         {
//             Id = "u1",
//             UserName = "user",
//             Birthdate = new DateOnly(2000, 1, 1),
//             SearchOrganism = "test"
//         };
//
//         var diet = new SpecieDiet
//         {
//             Id = 1,
//             Label = "Carnivore"
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
//             Id = 1,
//             Name = "Lion",
//             Slug = "lion",
//             UserId = "u1",
//             DietEntity = diet,
//             ClassificationEntity = classification
//         };
//
//         var biome = new BiomeEntity
//         {
//             Id = 1,
//             Name = "Savane",
//             UserId = "u1",
//             Temperature = 25,
//             Humidity = 40,
//             User = user,
//             BiomeSpecies =
//             {
//                 new BiomeSpecieEntity
//                 {
//                     Specie = specie,
//                     Inserted = DateTime.UtcNow
//                 }
//             }
//         };
//
//         _context.Biomes.Add(biome);
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.GetStatsAsync("u1", true);
//
//         Assert.That(result.Value, Is.Not.Null);
//         Assert.That(result.Value!.Count, Is.EqualTo(1));
//
//         var stats = result.Value.First();
//
//         Assert.That(stats.SpeciesCount, Is.EqualTo(1));
//         Assert.That(stats.Name, Is.EqualTo("Savane"));
//     }
//
//     [Test]
//     public async Task should_filter_biomes_for_non_admin()
//     {
//         _context.Biomes.Add(new BiomeEntity
//         {
//             Id = 1,
//             Name = "A",
//             UserId = "u1",
//             Temperature = 10,
//             Humidity = 10,
//             IsPublic = false,
//             User = new UserEntity
//             {
//                 Id = "u1",
//                 UserName = "user1",
//                 Birthdate = new DateOnly(2000, 1, 1),
//                 SearchOrganism = "test"
//             }
//         });
//
//         _context.Biomes.Add(new BiomeEntity
//         {
//             Id = 2,
//             Name = "B",
//             UserId = "u2",
//             Temperature = 10,
//             Humidity = 10,
//             IsPublic = false,
//             User = new UserEntity
//             {
//                 Id = "u2",
//                 UserName = "user2",
//                 Birthdate = new DateOnly(2000, 1, 1),
//                 SearchOrganism = "test"
//             }
//         });
//
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.GetStatsAsync("u1", false);
//
//         Assert.That(result.Value, Is.Not.Null);
//         Assert.That(result.Value!.Count, Is.EqualTo(1));
//         Assert.That(result.Value.First().Name, Is.EqualTo("A"));
//     }
//     
//     [Test]
//     public async Task should_return_history()
//     {
//         var specie = new SpecieEntity
//         {
//             Id = 1,
//             Name = "Wolf",
//             Slug = "wolf",
//             UserId = "u1",
//             DietEntity = new SpecieDiet { Id = 1, Label = "Carnivore" },
//             ClassificationEntity = new SpecieClassification
//             {
//                 Id = 1,
//                 Label = "Mammal",
//                 RequireFood = true
//             }
//         };
//
//         var biome = new BiomeEntity
//         {
//             Id = 1,
//             Name = "Forest",
//             UserId = "u1",
//             Temperature = 10,
//             Humidity = 60,
//             User = new UserEntity
//             {
//                 Id = "u1",
//                 UserName = "user",
//                 Birthdate = new DateOnly(2000, 1, 1),
//                 SearchOrganism = "test"
//             },
//             BiomeSpecies =
//             {
//                 new BiomeSpecieEntity
//                 {
//                     Specie = specie,
//                     Inserted = DateTime.UtcNow
//                 }
//             }
//         };
//
//         _context.Biomes.Add(biome);
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.GetHistoryAsync("u1", true, 1, 30);
//
//         Assert.That(result.Value, Is.Not.Null);
//         Assert.That(result.Value!.First().BiomeId, Is.EqualTo(1));
//     }
// }