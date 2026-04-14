using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.Repositories;

public class EquipmentRepository(BiodomeContext context) : IEquipmentRepository
{
    public async Task<List<Equipment>> GetAllAsync()
    {
        var result = await context.Equipments
            .Include(e => e.ConsumedResourceEntity)
            .Include(e => e.ProducedResourceEntity)
            .ToListAsync();

        return result.Select(EquipmentMapper.MapEquipment).ToList();
    }

    public async Task<List<EquipmentResource>> GetResourcesAsync()
    {
        var result = await context.EquipmentResources.ToListAsync();
        return result.Select(r => new EquipmentResource
        {
            Id = r.Id,
            Label = r.Label
        }).ToList();
    }

    public async Task CreateEquipmentAsync(CreateEquipmentRequest createRequest)
    {
        var entity = new EquipmentEntity
        {
            Name = createRequest.Name,
            Image = createRequest.Image,
            ProducedResourceId = createRequest.ProducedResourceId,
            ConsumedResourceId = createRequest.ConsumedResourceId,
        };
        
        await context.Equipments.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task<Equipment?> FindAsync(int id)
    {
        var maybe = await context.Equipments
            .Include(e => e.ConsumedResourceEntity)
            .Include(e => e.ProducedResourceEntity)
            .FirstOrDefaultAsync(e => e.Id == id);
        return maybe is null ? null : EquipmentMapper.MapEquipment(maybe);
    }

    public async Task<Equipment?> DeleteAsync(int id)
    {
        var entity = await context.Equipments.FirstOrDefaultAsync(e => e.Id == id);
        if (entity is null) return null;
        var model = EquipmentMapper.MapEquipment(entity);
        context.Equipments.Remove(entity);
        await context.SaveChangesAsync();
        return model;
    }

    public async Task UpdateAsync(UpdateEquipmentRequest equipment)
    {
        var entity = await context.Equipments
            .Include(e => e.ConsumedResourceEntity)
            .Include(e => e.ProducedResourceEntity)
            .FirstOrDefaultAsync(e => e.Id == equipment.Id);

        if (entity is null)
        {
            throw new KeyNotFoundException($"Equipment with Id {equipment.Id} not found.");
        }

        entity.Name = equipment.Name;
        entity.Image = equipment.Image;

        entity.ProducedResourceId = equipment.ProducedResourceId;
        entity.ConsumedResourceId = equipment.ConsumedResourceId;

        await context.SaveChangesAsync();
    }

}