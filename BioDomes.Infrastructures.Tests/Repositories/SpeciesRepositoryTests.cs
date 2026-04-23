using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Repositories;
using BioDomes.Infrastructures.Tests.TestSupport;
using Microsoft.Extensions.Logging.Abstractions;

namespace BioDomes.Infrastructures.Tests.Repositories;

[TestFixture]
public class SpeciesRepositoryTests
{
    [Test]
    public async Task FindAllAsync_ForNonAdmin_ReturnsPublicAndOwnedSpeciesOnly()
    {
        await using var context = TestDbContextFactory.Create();
        context.Species.AddRange(
            TestData.CreateSpecie("public-specie", "Public Specie", "user-1", true),
            TestData.CreateSpecie("owned-private", "Owned Private", "user-1", false),
            TestData.CreateSpecie("other-private", "Other Private", "user-2", false));
        await context.SaveChangesAsync();

        var repository = new SpeciesRepository(context, NullLogger<SpeciesRepository>.Instance);

        var result = await repository.FindAllAsync("user-1", isAdmin: false);
        var names = result.Value?.Select(s => s.Name).ToList() ?? [];

        Assert.Multiple(() =>
        {
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(names, Does.Contain("Public Specie"));
            Assert.That(names, Does.Contain("Owned Private"));
            Assert.That(names, Does.Not.Contain("Other Private"));
        });
    }

    [Test]
    public async Task FindBySlugAsync_ForPrivateSpecieOwnedByAnotherUser_ReturnsForbidden()
    {
        await using var context = TestDbContextFactory.Create();
        context.Species.Add(TestData.CreateSpecie("secret-wolf", "Secret Wolf", "user-1", false));
        await context.SaveChangesAsync();

        var repository = new SpeciesRepository(context, NullLogger<SpeciesRepository>.Instance);

        var result = await repository.FindBySlugAsync("secret-wolf", "user-2", isAdmin: false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
    }

    [Test]
    public async Task AddAsync_WithoutAuthenticatedUser_ReturnsForbidden()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new SpeciesRepository(context, NullLogger<SpeciesRepository>.Instance);

        var result = await repository.AddAsync(new CreateSpecieRequest
        {
            Name = "Red Panda",
            Height = 50,
            Weight = 6,
            Image = "red-panda.png",
            ClassificationId = 1,
            DietId = 2
        }, null);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
    }

    [Test]
    public async Task UpdateAsync_WhenNonAdminUpdatesSpecie_ShouldPreserveVisibility()
    {
        await using var context = TestDbContextFactory.Create();
        context.Species.Add(TestData.CreateSpecie("red-panda", "Red Panda", "user-1", true, dietId: 2));
        await context.SaveChangesAsync();

        var repository = new SpeciesRepository(context, NullLogger<SpeciesRepository>.Instance);

        var result = await repository.UpdateAsync(new UpdateSpecieRequest
        {
            PreviousSlug = "red-panda",
            Name = "Updated Panda",
            Height = 55,
            Weight = 7,
            ImageUrl = null,
            IsPublic = null,
            ClassificationId = 1,
            DietId = 2
        }, "user-1", isAdmin: false);

        Assert.Multiple(() =>
        {
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Value!.IsPublic, Is.True,
                "A non-admin update should not silently unpublish an existing public specie.");
        });
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnDeletedSpecie()
    {
        await using var context = TestDbContextFactory.Create();
        context.Species.Add(TestData.CreateSpecie("red-panda-delete", "Red Panda", "user-1", true, dietId: 2));
        await context.SaveChangesAsync();

        var repository = new SpeciesRepository(context, NullLogger<SpeciesRepository>.Instance);

        var result = await repository.DeleteAsync("red-panda-delete", "user-1", isAdmin: false);

        Assert.Multiple(() =>
        {
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Value!.Name, Is.EqualTo("Red Panda"));
        });
    }
}
