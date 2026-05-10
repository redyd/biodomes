// using BioDomes.Domains.Core;
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
// public class SpeciesRepositoryTests
// {
//     private BiodomeContext _context = null!;
//     private SpeciesRepository _repository = null!;
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
//         var logger = new Mock<ILogger<SpeciesRepository>>();
//         _repository = new SpeciesRepository(_context, logger.Object);
//
//         Seed();
//     }
//
//     [TearDown]
//     public void TearDown()
//     {
//         _context.Dispose();
//     }
//
//     private void Seed()
//     {
//         var diet = CreateDiet();
//         var classification = CreateClassification();
//
//         var species = CreateSpecie("Wolf", "wolf", "user-1", diet, classification, true);
//         var species2 = CreateSpecie("Rabbit", "rabbit", "user-1", diet, classification, true);
//
//         _context.SpecieDiets.Add(diet);
//         _context.SpecieClassifications.Add(classification);
//         _context.Species.AddRange(species, species2);
//
//         _context.SaveChanges();
//     }
//
//     private SpecieDiet CreateDiet()
//     {
//         return new SpecieDiet
//         {
//             Label = "carnivore"
//         };
//     }
//
//     private SpecieClassification CreateClassification()
//     {
//         return new SpecieClassification
//         {
//             Label = "mammal",
//             RequireFood = true
//         };
//     }
//
//     private SpecieEntity CreateSpecie(
//         string name,
//         string slug,
//         string userId,
//         SpecieDiet diet,
//         SpecieClassification classification,
//         bool isPublic)
//     {
//         return new SpecieEntity
//         {
//             Name = name,
//             Slug = slug,
//             UserId = userId,
//             DietEntity = diet,
//             ClassificationEntity = classification,
//             DietId = 1,
//             ClassificationId = 1,
//             Height = 10,
//             Weight = 20,
//             IsPublic = isPublic
//         };
//     }
//
//     [Test]
//     public async Task should_return_all_species()
//     {
//         var result = await _repository.FindAllAsync(null, true);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//         Assert.That(result.Value.Count, Is.GreaterThan(0));
//     }
//
//     [Test]
//     public async Task should_filter_species_for_user()
//     {
//         var result = await _repository.FindAllAsync("user-1", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//         Assert.That(result.Value.Count, Is.GreaterThan(0));
//     }
//
//     [Test]
//     public async Task should_find_by_slug()
//     {
//         var result = await _repository.FindBySlugAsync("wolf", "user-1", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//     }
//
//     [Test]
//     public async Task should_return_not_found_when_slug_missing()
//     {
//         var result = await _repository.FindBySlugAsync("unknown", "user-1", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
//     }
//
//     [Test]
//     public async Task should_add_specie()
//     {
//         var request = new CreateSpecieRequest
//         {
//             Name = "Fox",
//             ClassificationId = 1,
//             DietId = 1,
//             Height = 5,
//             Weight = 8,
//             Image = "img.png"
//         };
//
//         var result = await _repository.AddAsync(request, "user-1");
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//
//         var exists = _context.Species.Any(s => s.Name == "Fox");
//         Assert.That(exists, Is.True);
//     }
//
//     [Test]
//     public async Task should_return_forbidden_when_no_user_on_add()
//     {
//         var request = new CreateSpecieRequest
//         {
//             Name = "Fox",
//             ClassificationId = 1,
//             DietId = 1,
//             Height = 5,
//             Weight = 8,
//             Image = "img.png"
//         };
//
//         var result = await _repository.AddAsync(request, null);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
//     }
//
//     [Test]
//     public async Task should_delete_specie()
//     {
//         var specie = _context.Species.First();
//
//         var result = await _repository.DeleteAsync(specie.Id, "user-1", false);
//
//         Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
//     }
// }