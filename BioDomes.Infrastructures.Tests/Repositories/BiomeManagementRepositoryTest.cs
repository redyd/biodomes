// using BioDomes.Domains.Core;
// using BioDomes.Domains.Repository;
// using BioDomes.Domains.Requests;
// using BioDomes.Infrastructures.Models;
// using BioDomes.Infrastructures.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Logging;
// using Moq;
//
// namespace BioDomes.Infrastructures.Tests.Repositories;
//
// [TestFixture]
// public class BiomeManagementRepositoryTests
// {
//     private BiodomeContext _context = null!;
//     private BiomeManagementRepository _repository = null!;
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
//         var logger = new Mock<ILogger<IBiomeManagementRepository>>();
//
//         _repository = new BiomeManagementRepository(_context, logger.Object);
//     }
//
//     [TearDown]
//     public void TearDown()
//     {
//         _context.Dispose();
//     }
//
//     private UserEntity CreateUser(string id = "user")
//     {
//         return new UserEntity
//         {
//             Id = id,
//             UserName = id,
//             Email = $"{id}@test.com",
//             Birthdate = DateOnly.FromDateTime(DateTime.UtcNow),
//             SearchOrganism = "test",
//             ImageUrl = null
//         };
//     }
//
//     private BiomeEntity CreateBiome(string userId = "user")
//     {
//         return new BiomeEntity
//         {
//             Id = 1,
//             Name = "Test biome",
//             Temperature = 25,
//             Humidity = 70,
//             IsPublic = true,
//             ImageUrl = null,
//             UserId = userId,
//             User = CreateUser(userId)
//         };
//     }
//
//     [Test]
//     public async Task should_return_not_found_when_biome_does_not_exist_on_add_specie()
//     {
//         var request = new SpecieToBiomeRequest
//         {
//             SpecieId = 1,
//             Count = 1
//         };
//
//         var result = await _repository.AddSpecieToBiome(request, 1, "user", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
//     }
//
//     [Test]
//     public async Task should_return_forbidden_when_user_is_not_owner_on_add_specie()
//     {
//         _context.Biomes.Add(CreateBiome("owner"));
//
//         await _context.SaveChangesAsync();
//
//         var request = new SpecieToBiomeRequest
//         {
//             SpecieId = 1,
//             Count = 1
//         };
//
//         var result = await _repository.AddSpecieToBiome(request, 1, "other-user", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
//     }
//
//     [Test]
//     public async Task should_add_species_to_biome()
//     {
//         _context.Biomes.Add(CreateBiome());
//
//         await _context.SaveChangesAsync();
//
//         var request = new SpecieToBiomeRequest
//         {
//             SpecieId = 5,
//             Count = 3
//         };
//
//         var result = await _repository.AddSpecieToBiome(request, 1, "user", false);
//
//         var inserted = await _context.BiomeSpecies
//             .Where(x => x.BiomeId == 1 && x.SpecieId == 5)
//             .ToListAsync();
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//         Assert.That(inserted.Count, Is.EqualTo(3));
//     }
//
//     [Test]
//     public async Task should_remove_species_from_biome()
//     {
//         _context.Biomes.Add(CreateBiome());
//
//         _context.BiomeSpecies.AddRange(
//             new BiomeSpecieEntity { BiomeId = 1, SpecieId = 2 },
//             new BiomeSpecieEntity { BiomeId = 1, SpecieId = 2 },
//             new BiomeSpecieEntity { BiomeId = 1, SpecieId = 2 }
//         );
//
//         await _context.SaveChangesAsync();
//
//         var request = new SpecieToBiomeRequest
//         {
//             SpecieId = 2,
//             Count = 2
//         };
//
//         var result = await _repository.RemoveSpecieToBiome(request, 1, "user", false);
//
//         var remaining = await _context.BiomeSpecies
//             .Where(x => x.BiomeId == 1 && x.SpecieId == 2)
//             .ToListAsync();
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//         Assert.That(remaining.Count, Is.EqualTo(1));
//     }
//
//     [Test]
//     public async Task should_add_equipments_to_biome()
//     {
//         _context.Biomes.Add(CreateBiome());
//
//         await _context.SaveChangesAsync();
//
//         var request = new EquipmentToBiomeRequest
//         {
//             EquipmentId = 10,
//             Count = 2
//         };
//
//         var result = await _repository.AddEquipmentToBiome(request, 1, "user", false);
//
//         var inserted = await _context.BiomeEquipments
//             .Where(x => x.BiomeId == 1 && x.EquipmentId == 10)
//             .ToListAsync();
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//         Assert.That(inserted.Count, Is.EqualTo(2));
//     }
//
//     [Test]
//     public async Task should_remove_equipments_from_biome()
//     {
//         _context.Biomes.Add(CreateBiome());
//
//         _context.BiomeEquipments.AddRange(
//             new BiomeEquipmentEntity { BiomeId = 1, EquipmentId = 4 },
//             new BiomeEquipmentEntity { BiomeId = 1, EquipmentId = 4 },
//             new BiomeEquipmentEntity { BiomeId = 1, EquipmentId = 4 }
//         );
//
//         await _context.SaveChangesAsync();
//
//         var request = new EquipmentToBiomeRequest
//         {
//             EquipmentId = 4,
//             Count = 2
//         };
//
//         var result = await _repository.RemoveEquipmentToBiome(request, 1, "user", false);
//
//         var remaining = await _context.BiomeEquipments
//             .Where(x => x.BiomeId == 1 && x.EquipmentId == 4)
//             .ToListAsync();
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//         Assert.That(remaining.Count, Is.EqualTo(1));
//     }
//
//     [Test]
//     public async Task should_return_forbidden_when_user_is_not_owner_on_remove_equipment()
//     {
//         _context.Biomes.Add(CreateBiome("owner"));
//
//         await _context.SaveChangesAsync();
//
//         var request = new EquipmentToBiomeRequest
//         {
//             EquipmentId = 1,
//             Count = 1
//         };
//
//         var result = await _repository.RemoveEquipmentToBiome(request, 1, "other-user", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
//     }
// }