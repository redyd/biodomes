using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.Mappers;
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
}