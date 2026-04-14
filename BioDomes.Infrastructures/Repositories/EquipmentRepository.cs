using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class EquipmentRepository(BiodomeContext context, ILogger<EquipmentRepository> logger) : IEquipmentRepository
{
    public async Task<List<Equipment>> GetAllAsync(string? userId, bool isAdmin)
    {
        var query = context.Equipments
            .Include(e => e.ConsumedResourceEntity)
            .Include(e => e.ProducedResourceEntity)
            .AsQueryable();

        if (!isAdmin)
        {
            query = query.Where(e =>
                e.IsPublic
                || (userId != null && e.UserId == userId));
        }

        var result = await query.ToListAsync();
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

    public async Task<Result<Equipment>> CreateEquipmentAsync(
        CreateEquipmentRequest request, string userId)
    {
        try
        {
            var entity = new EquipmentEntity
            {
                Name = request.Name,
                Image = request.Image,
                ProducedResourceId = request.ProducedResourceId,
                ConsumedResourceId = request.ConsumedResourceId,
                UserId = userId
            };

            await context.Equipments.AddAsync(entity);
            await context.SaveChangesAsync();
            
            var created = await context.Equipments
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
                .FirstAsync(e => e.Id == entity.Id);

            return Result<Equipment>.Ok(EquipmentMapper.MapEquipment(created));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex))
        {
            return Result<Equipment>.Conflict("An equipment with this name already exists.");
        }
        catch (DbUpdateException ex) when (SqlServerError.IsForeignKeyViolation(ex))
        {
            return Result<Equipment>.Failure("A referenced resource does not exist.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Equipment>.Failure("Database error.");
        }
    }

    public async Task<Result<Equipment>> FindAsync(int id, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Equipments
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity is null)
            {
                return Result<Equipment>.NotFound($"Equipment {id} not found.");
            }

            if (!isAdmin && !entity.IsPublic && entity.UserId != userId)
            {
                return Result<Equipment>.Forbidden("Access denied.");
            }

            return Result<Equipment>.Ok(EquipmentMapper.MapEquipment(entity));
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Equipment>.Failure("Database error");
        }
    }

    public async Task<Result<Equipment>> DeleteAsync(int id, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Equipments
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity is null)
                return Result<Equipment>.NotFound($"Equipment {id} not found.");

            if (!isAdmin && entity.UserId != userId)
                return Result<Equipment>.Forbidden("Access denied.");

            var model = EquipmentMapper.MapEquipment(entity);
            context.Equipments.Remove(entity);
            await context.SaveChangesAsync();
            return Result<Equipment>.Ok(model);
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<Equipment>.Failure("Database error");
        }
    }

    public async Task UpdateAsync(UpdateEquipmentRequest equipment, string? userId, bool isAdmin)
    {
        var entity = await context.Equipments
            .Include(e => e.ConsumedResourceEntity)
            .Include(e => e.ProducedResourceEntity)
            .FirstOrDefaultAsync(e =>
                e.Id == equipment.Id
                && (isAdmin || (userId != null && e.UserId == userId)));

        if (entity is null)
        {
            throw new KeyNotFoundException($"Equipment with Id {equipment.Id} not found or access denied.");
        }

        entity.Name = equipment.Name;
        entity.Image = equipment.Image;
        entity.ProducedResourceId = equipment.ProducedResourceId;
        entity.ConsumedResourceId = equipment.ConsumedResourceId;
        await context.SaveChangesAsync();
    }
}