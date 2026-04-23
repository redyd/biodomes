using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Repositories;
using BioDomes.Infrastructures.Tests.TestSupport;
using Microsoft.Extensions.Logging.Abstractions;

namespace BioDomes.Infrastructures.Tests.Repositories;

[TestFixture]
public class EquipmentRepositoryTests
{
    [Test]
    public async Task GetAllAsync_ForNonAdmin_ReturnsPublicAndOwnedEquipmentsOnly()
    {
        await using var context = TestDbContextFactory.Create();
        context.Equipments.AddRange(
            TestData.CreateEquipment(101, "Public Pump", "user-1", true),
            TestData.CreateEquipment(102, "Owned Filter", "user-1", false),
            TestData.CreateEquipment(103, "Other Secret Pump", "user-2", false));
        await context.SaveChangesAsync();

        var repository = new EquipmentRepository(context, NullLogger<EquipmentRepository>.Instance);

        var result = await repository.GetAllAsync("user-1", isAdmin: false);

        Assert.Multiple(() =>
        {
            Assert.That(result.Select(e => e.Name), Does.Contain("Public Pump"));
            Assert.That(result.Select(e => e.Name), Does.Contain("Owned Filter"));
            Assert.That(result.Select(e => e.Name), Does.Not.Contain("Other Secret Pump"));
        });
    }

    [Test]
    public async Task FindAsync_ForPrivateEquipmentOwnedByAnotherUser_ReturnsForbidden()
    {
        await using var context = TestDbContextFactory.Create();
        context.Equipments.Add(TestData.CreateEquipment(104, "Secret Pump", "user-1", false));
        await context.SaveChangesAsync();

        var repository = new EquipmentRepository(context, NullLogger<EquipmentRepository>.Instance);

        var result = await repository.FindAsync(104, "user-2", isAdmin: false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
    }

    [Test]
    public async Task CreateEquipmentAsync_CreatesEquipmentForGivenUser()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new EquipmentRepository(context, NullLogger<EquipmentRepository>.Instance);

        var result = await repository.CreateEquipmentAsync(new CreateEquipmentRequest
        {
            Name = "Solar Pump",
            Image = "pump.png",
            ProducedResourceId = 2,
            ConsumedResourceId = 1,
            UserId = "ignored-by-repository"
        }, "user-1");

        Assert.Multiple(() =>
        {
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Value!.CreatedBy, Is.EqualTo("user-1"));
            Assert.That(result.Value.Name, Is.EqualTo("Solar Pump"));
        });
    }

    [Test]
    public async Task DeleteAsync_ShouldReturnDeletedEquipment()
    {
        await using var context = TestDbContextFactory.Create();
        context.Equipments.Add(TestData.CreateEquipment(105, "Solar Pump", "user-1", true));
        await context.SaveChangesAsync();

        var repository = new EquipmentRepository(context, NullLogger<EquipmentRepository>.Instance);

        var result = await repository.DeleteAsync(105, "user-1", isAdmin: false);

        Assert.Multiple(() =>
        {
            Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
            Assert.That(result.Value!.Name, Is.EqualTo("Solar Pump"));
        });
    }
}
