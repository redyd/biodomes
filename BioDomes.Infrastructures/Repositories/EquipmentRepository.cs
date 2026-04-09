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

    public async Task<Dictionary<int, string>> GetResourcesAsync()
    {
        var result = await context.EquipmentResources.ToListAsync();
        var dict = new Dictionary<int, string>();
        result.ForEach(e => dict[e.Id] = e.Label);
        return dict;
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
}