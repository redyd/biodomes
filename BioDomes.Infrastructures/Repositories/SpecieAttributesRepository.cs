using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class SpecieAttributesRepository(BiodomeContext context, ILogger<SpecieAttributesRepository> logger)
    : ISpecieAttributesRepository
{
    public async Task<List<Classification>> GetClassifications()
    {
        var results = await context.SpecieClassifications.ToListAsync();
        return results.Select(SpecieMapper.MapClassification).ToList();
    }

    public async Task<List<Diet>> GetDiets()
    {
        var results = await context.SpecieDiets.ToListAsync();
        return results.Select(SpecieMapper.MapDiet).ToList();
    }
}