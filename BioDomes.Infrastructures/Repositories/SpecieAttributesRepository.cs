using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class SpecieAttributesRepository(BiodomeContext context, ILogger<SpecieAttributesRepository> logger)
    : ISpecieAttributesRepository
{
    public async Task<ResultT<List<Classification>>> GetClassifications()
    {
        try
        {
            var results = await context.SpecieClassifications.ToListAsync();
            return ResultT<List<Classification>>.Ok(results.Select(SpecieMapper.MapClassification).ToList());
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<List<Classification>>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<List<Diet>>> GetDiets()
    {
        try
        {
            var results = await context.SpecieDiets.ToListAsync();
            return ResultT<List<Diet>>.Ok(results.Select(SpecieMapper.MapDiet).ToList());
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {ExMessage}", e.Message);
            return ResultT<List<Diet>>.Failure($"Database error: {e.Message}");
        }
    }
}