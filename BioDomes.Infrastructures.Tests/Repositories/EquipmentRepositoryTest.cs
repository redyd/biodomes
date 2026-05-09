using BioDomes.Domains.Core;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Models;
using BioDomes.Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace BioDomes.Infrastructures.Tests.Repositories;

[TestFixture]
public class EquipmentRepositoryTests
{
    private BiodomeContext _context = null!;
    private EquipmentRepository _repository = null!;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<BiodomeContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        _context = new BiodomeContext(options);

        var logger = new Mock<ILogger<EquipmentRepository>>();

        _repository = new EquipmentRepository(_context, logger.Object);

        SeedResources();
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    private void SeedResources()
    {
        _context.EquipmentResources.AddRange(
            new EquipmentResourceEntity { Id = 1, Label = "Water" },
            new EquipmentResourceEntity { Id = 2, Label = "Energy" }
        );

        _context.SaveChanges();
    }

    [Test]
    public async Task should_return_all_public_equipment()
    {
        _context.Equipments.Add(CreateEquipment(1, isPublic: true));

        await _context.SaveChangesAsync();

        var result = await _repository.GetAllAsync(null, false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value!.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task should_create_equipment()
    {
        var request = new CreateEquipmentRequest
        {
            Name = "Generator",
            Image = "img.png",
            ProducedResourceId = 1,
            ConsumedResourceId = 2,
            UserId = "user1"
        };

        var result = await _repository.CreateEquipmentAsync(request, "user1");

        var db = await _context.Equipments.ToListAsync();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(db.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task should_return_not_found_when_equipment_missing()
    {
        var result = await _repository.FindAsync(999, "user", false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.NotFound));
    }

    [Test]
    public async Task should_return_forbidden_for_private_equipment()
    {
        _context.Equipments.Add(CreateEquipment(1, isPublic: false, owner: "owner"));

        await _context.SaveChangesAsync();

        var result = await _repository.FindAsync(1, "other", false);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Forbidden));
    }

    [Test]
    public async Task should_delete_equipment()
    {
        _context.Equipments.Add(CreateEquipment(1, owner: "user1"));

        await _context.SaveChangesAsync();

        var result = await _repository.DeleteAsync(1, "user1", false);

        var db = await _context.Equipments.ToListAsync();

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(db.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task should_update_equipment()
    {
        _context.Equipments.Add(CreateEquipment(1));

        await _context.SaveChangesAsync();

        var request = new UpdateEquipmentRequest
        {
            Id = 1,
            Name = "Updated",
            Image = "new.png",
            ProducedResourceId = 1,
            ConsumedResourceId = 2,
            IsPublic = true
        };

        var result = await _repository.UpdateAsync(request, "user1", true);

        Assert.That(result.Status, Is.EqualTo(ResultStatus.Success));
        Assert.That(result.Value!.Name, Is.EqualTo("Updated"));
    }

    private EquipmentEntity CreateEquipment(int id, bool isPublic = true, string owner = "user1")
    {
        return new EquipmentEntity
        {
            Id = id,
            Name = "TestEquipment",
            Image = "img.png",
            IsPublic = isPublic,
            UserId = owner,
            ProducedResourceId = 1,
            ConsumedResourceId = 2
        };
    }
}