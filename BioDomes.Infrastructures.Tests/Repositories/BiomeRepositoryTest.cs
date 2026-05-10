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
// public class BiomeRepositoryTests
// {
//     private BiodomeContext _context = null!;
//     private BiomeRepository _repository = null!;
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
//         var logger = new Mock<ILogger<IBiomeRepository>>();
//
//         _repository = new BiomeRepository(_context, logger.Object);
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
//             SearchOrganism = "test"
//         };
//     }
//
//     private BiomeEntity CreateBiome(
//         int id = 1,
//         string userId = "user",
//         bool isPublic = true,
//         string name = "Biome")
//     {
//         return new BiomeEntity
//         {
//             Id = id,
//             Name = name,
//             Temperature = 20,
//             Humidity = 50,
//             IsPublic = isPublic,
//             ImageUrl = null,
//             UserId = userId,
//             User = CreateUser(userId)
//         };
//     }
//
//     [Test]
//     public async Task should_return_all_public_biomes_for_non_admin()
//     {
//         _context.Biomes.AddRange(
//             CreateBiome(1, "user1", true, "Public"),
//             CreateBiome(2, "user2", false, "Private")
//         );
//
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.GetAllAsync(null, false);
//
//         Assert.That(result.Value, Has.Count.EqualTo(1));
//         Assert.That(result.Value![0].Name, Is.EqualTo("Public"));
//     }
//
//     [Test]
//     public async Task should_return_user_private_biomes_for_owner()
//     {
//         _context.Biomes.AddRange(
//             CreateBiome(1, "user", false, "Mine"),
//             CreateBiome(2, "other", false, "Other")
//         );
//
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.GetAllAsync("user", false);
//
//         Assert.That(result.Value, Has.Count.EqualTo(1));
//         Assert.That(result.Value![0].Name, Is.EqualTo("Mine"));
//     }
//
//     [Test]
//     public async Task should_return_all_biomes_for_admin()
//     {
//         _context.Biomes.AddRange(
//             CreateBiome(1, "user1", true),
//             CreateBiome(2, "user2", false)
//         );
//
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.GetAllAsync(null, true);
//
//         Assert.That(result.Value, Has.Count.EqualTo(2));
//     }
//
//     [Test]
//     public async Task should_return_not_found_when_biome_does_not_exist()
//     {
//         var result = await _repository.FindAsync(999, null, false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
//     }
//
//     [Test]
//     public async Task should_return_forbidden_when_user_cannot_access_biome()
//     {
//         _context.Biomes.Add(CreateBiome(1, "owner", false));
//
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.FindAsync(1, "other-user", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
//     }
//
//     [Test]
//     public async Task should_return_biome_when_user_has_access()
//     {
//         _context.Biomes.Add(CreateBiome(1, "user", false));
//
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.FindAsync(1, "user", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//         Assert.That(result.Value!.Id, Is.EqualTo(1));
//     }
//
//     [Test]
//     public async Task should_create_biome()
//     {
//         var request = new CreateBiomeRequest
//         {
//             Name = "New biome",
//             Temperature = 30,
//             Humidity = 60,
//             ImageUrl = null,
//             IsPublic = true
//         };
//
//         var result = await _repository.CreateBiomeAsync(request, "user");
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//
//         var biome = await _context.Biomes.FirstOrDefaultAsync();
//
//         Assert.That(biome, Is.Not.Null);
//         Assert.That(biome!.Name, Is.EqualTo("New biome"));
//     }
//
//     [Test]
//     public async Task should_return_conflict_when_creating_duplicate_biome()
//     {
//         _context.Biomes.Add(CreateBiome(name: "Existing"));
//
//         await _context.SaveChangesAsync();
//
//         var request = new CreateBiomeRequest
//         {
//             Name = "Existing",
//             Temperature = 20,
//             Humidity = 40,
//             ImageUrl = null,
//             IsPublic = true
//         };
//
//         var result = await _repository.CreateBiomeAsync(request, "user");
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Conflict));
//     }
//
//     [Test]
//     public async Task should_update_biome()
//     {
//         _context.Biomes.Add(CreateBiome());
//
//         await _context.SaveChangesAsync();
//
//         var request = new UpdateBiomeRequest
//         {
//             Id = 1,
//             Name = "Updated",
//             Temperature = 99,
//             Humidity = 12,
//             ImageUrl = "image.png",
//             IsPublic = false
//         };
//
//         var result = await _repository.UpdateAsync(request, "user", true);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//
//         var biome = await _context.Biomes.FirstAsync();
//
//         Assert.That(biome.Name, Is.EqualTo("Updated"));
//         Assert.That(biome.Temperature, Is.EqualTo(99));
//         Assert.That(biome.Humidity, Is.EqualTo(12));
//         Assert.That(biome.IsPublic, Is.False);
//     }
//
//     [Test]
//     public async Task should_return_not_found_when_updating_unknown_biome()
//     {
//         var request = new UpdateBiomeRequest
//         {
//             Id = 999,
//             Name = "Unknown",
//             Temperature = 10,
//             Humidity = 10
//         };
//
//         var result = await _repository.UpdateAsync(request, "user", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
//     }
//
//     [Test]
//     public async Task should_delete_biome()
//     {
//         _context.Biomes.Add(CreateBiome());
//
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.DeleteAsync(1, "user", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//         Assert.That(await _context.Biomes.CountAsync(), Is.EqualTo(0));
//     }
//
//     [Test]
//     public async Task should_return_forbidden_when_deleting_other_user_biome()
//     {
//         _context.Biomes.Add(CreateBiome(1, "owner"));
//
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.DeleteAsync(1, "other-user", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
//     }
//
//     [Test]
//     public async Task should_return_creator_username()
//     {
//         _context.Biomes.Add(CreateBiome());
//
//         await _context.SaveChangesAsync();
//
//         var result = await _repository.GetCreatorAsync(1);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//         Assert.That(result.Value, Is.EqualTo("user"));
//     }
//
//     [Test]
//     public async Task should_return_not_found_when_creator_biome_does_not_exist()
//     {
//         var result = await _repository.GetCreatorAsync(999);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
//     }
// }