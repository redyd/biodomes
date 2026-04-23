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
    
    public async Task<Result<int>> CountAsync()
    {
        try
        {
            var count = await context.Equipments.CountAsync();
            return Result<int>.Ok(count);
        }
        catch (Exception ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return Result<int>.Failure("Database error");
        }
    }

    public async Task<Result<Equipment>> CreateEquipmentAsync(CreateEquipmentRequest request, string userId)
    {
        try
        {
            var exist = await context.Equipments.AnyAsync(e => e.Name.ToLower() == request.Name.ToLower());

            if (exist)
            {
                return Result<Equipment>.Conflict("An equipment with this name already exists.");
            }
            
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
            return Result<Equipment>.Failure("Database error");
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
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
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

    public async Task<Result<Equipment>> UpdateAsync(UpdateEquipmentRequest toUpdate, string? userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Equipments
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
                .FirstOrDefaultAsync(e => e.Id == toUpdate.Id);

            if (entity is null)
                return Result<Equipment>.NotFound($"Equipment {toUpdate.Id} not found.");

            if (!isAdmin && entity.UserId != userId)
                return Result<Equipment>.Forbidden("Access denied.");

            entity.Name = toUpdate.Name;
            entity.Image = toUpdate.Image;
            entity.ProducedResourceId = toUpdate.ProducedResourceId;
            entity.ConsumedResourceId = toUpdate.ConsumedResourceId;

            if (isAdmin)
            {
                entity.IsPublic = toUpdate.IsPublic ?? entity.IsPublic;
            }

            await context.SaveChangesAsync();

            var updated = await context.Equipments
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
                .FirstAsync(e => e.Id == entity.Id);

            return Result<Equipment>.Ok(EquipmentMapper.MapEquipment(updated));
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
            return Result<Equipment>.Failure("Database error");
        }
    }
}
