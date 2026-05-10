using BioDomes.Domains.Core;
using BioDomes.Domains.Extensions;
using BioDomes.Domains.Repository;
using BioDomes.Domains.Requests;
using BioDomes.Infrastructures.Errors;
using BioDomes.Infrastructures.Mappers;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class EquipmentRepository(BiodomeContext context, ILogger<EquipmentRepository> logger) : IEquipmentRepository
{
    public async Task<ResultT<List<Equipment>>> GetAllAsync(string? userId, bool isAdmin)
    {
        try
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
            return ResultT<List<Equipment>>.Ok(result.Select(EquipmentMapper.MapEquipment).ToList());
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<List<Equipment>>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<List<EquipmentResource>>> GetResourcesAsync()
    {
        try
        {
            var result = await context.EquipmentResources.ToListAsync();
            return ResultT<List<EquipmentResource>>.Ok(result.Select(r => new EquipmentResource
            {
                Slug = r.Slug,
                Label = r.Label
            }).ToList());
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<List<EquipmentResource>>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<Equipment>> CreateEquipmentAsync(CreateEquipmentRequest request, string userId)
    {
        try
        {
            var slug = request.Name.ToKebabCase();
            var exist = await context.Equipments.AnyAsync(e => e.Slug == slug);

            if (exist)
            {
                return ResultT<Equipment>.Conflict("An equipment with this name already exists.");
            }

            var produced = await context
                .EquipmentResources
                .FirstOrDefaultAsync(e => e.Slug == request.ProducedResourceSlug);

            var consumed = await context
                .EquipmentResources
                .FirstOrDefaultAsync(e => e.Slug == request.ConsumedResourceSlug);

            if (produced is null || consumed is null)
            {
                return ResultT<Equipment>.NotFound("Equipment not found.");
            }

            var entity = new EquipmentEntity
            {
                Slug = slug,
                Name = request.Name,
                Image = request.Image,
                ProducedResourceId = produced.Id,
                ConsumedResourceId = consumed.Id,
                UserId = userId
            };

            await context.Equipments.AddAsync(entity);
            await context.SaveChangesAsync();

            var created = await context.Equipments
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
                .FirstAsync(e => e.Id == entity.Id);

            return ResultT<Equipment>.Ok(EquipmentMapper.MapEquipment(created));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex))
        {
            return ResultT<Equipment>.Conflict("An equipment with this name already exists.");
        }
        catch (DbUpdateException ex) when (SqlServerError.IsForeignKeyViolation(ex))
        {
            return ResultT<Equipment>.Failure("A referenced resource does not exist.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogError("Database error: {ExMessage}", ex.Message);
            return ResultT<Equipment>.Failure("Database error");
        }
    }

    public async Task<ResultT<Equipment>> DeleteAsync(string slug, string userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Equipments
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
                .FirstOrDefaultAsync(e => e.Slug == slug);

            if (entity is null)
            {
                return ResultT<Equipment>.NotFound($"Equipment {slug} not found.");
            }

            if (!isAdmin && entity.UserId != userId)
            {
                return ResultT<Equipment>.Forbidden("Access denied.");
            }

            var model = EquipmentMapper.MapEquipment(entity);
            context.Equipments.Remove(entity);
            await context.SaveChangesAsync();
            return ResultT<Equipment>.Ok(model);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError("Database error: {ExMessage}", ex.Message);
            return ResultT<Equipment>.Failure("Database error");
        }
    }

    public async Task<ResultT<Equipment>> FindAsync(string slug, string userId, bool isAdmin)
    {
        try
        {
            var entity = await context.Equipments
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
                .FirstOrDefaultAsync(e => e.Slug == slug);

            if (entity is null)
            {
                return ResultT<Equipment>.NotFound($"Equipment {slug} not found.");
            }

            if (!isAdmin && !entity.IsPublic && entity.UserId != userId)
            {
                return ResultT<Equipment>.Forbidden("Access denied.");
            }

            return ResultT<Equipment>.Ok(EquipmentMapper.MapEquipment(entity));
        }
        catch (DbUpdateException ex)
        {
            logger.LogError("Database error: {ExMessage}", ex.Message);
            return ResultT<Equipment>.Failure("Database error");
        }
    }

    public async Task<ResultT<Equipment>> UpdateAsync(UpdateEquipmentRequest toUpdate, string? userId, bool isAdmin)
    {
        try
        {
            var slug = toUpdate.Name.ToKebabCase();
            var exist = await context
                .Equipments
                .Where(e => e.Slug != toUpdate.PreviousSlug)
                .AnyAsync(e => e.Slug == slug);

            if (exist)
            {
                return ResultT<Equipment>.Conflict("An equipment with this name already exists.");
            }

            var entity = await context.Equipments
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
                .FirstOrDefaultAsync(e => e.Slug == toUpdate.PreviousSlug);

            if (entity is null)
            {
                return ResultT<Equipment>.NotFound($"Equipment {toUpdate.PreviousSlug} not found.");
            }

            if (!isAdmin && entity.UserId != userId)
            {
                return ResultT<Equipment>.Forbidden("Access denied.");
            }
            
            var produced = await context
                .EquipmentResources
                .FirstOrDefaultAsync(e => e.Slug == toUpdate.ProducedResourceSlug);

            var consumed = await context
                .EquipmentResources
                .FirstOrDefaultAsync(e => e.Slug == toUpdate.ConsumedResourceSlug);

            if (produced is null || consumed is null)
            {
                return ResultT<Equipment>.NotFound("Equipment not found.");
            }

            entity.Slug = slug;
            entity.Name = toUpdate.Name;
            entity.Image = toUpdate.Image;
            entity.ProducedResourceId = produced.Id;
            entity.ConsumedResourceId = consumed.Id;

            if (isAdmin)
            {
                entity.IsPublic = toUpdate.IsPublic ?? entity.IsPublic;
            }

            await context.SaveChangesAsync();

            var updated = await context.Equipments
                .Include(e => e.ConsumedResourceEntity)
                .Include(e => e.ProducedResourceEntity)
                .FirstAsync(e => e.Id == entity.Id);

            return ResultT<Equipment>.Ok(EquipmentMapper.MapEquipment(updated));
        }
        catch (DbUpdateException ex) when (SqlServerError.IsUniqueConstraintViolation(ex))
        {
            return ResultT<Equipment>.Conflict("An equipment with this name already exists.");
        }
        catch (DbUpdateException ex) when (SqlServerError.IsForeignKeyViolation(ex))
        {
            return ResultT<Equipment>.Failure("A referenced resource does not exist.");
        }
        catch (DbUpdateException ex)
        {
            logger.LogWarning($"Database error: {ex.Message}");
            return ResultT<Equipment>.Failure("Database error");
        }
    }
}