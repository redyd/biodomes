using BioDomes.Domains.Core;
using BioDomes.Domains.Repository;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BioDomes.Infrastructures.Repositories;

public class BiomeStatRepository(BiodomeContext context, ILogger<IBiomeStatRepository> logger) : IBiomeStatRepository
{
    private readonly IQueryable<BiomeEntity> _query = context.Biomes
        .Include(b => b.BiomeSpecies)
        .ThenInclude(bs => bs.Specie)
        .ThenInclude(s => s.DietEntity)
        .Include(b => b.BiomeSpecies)
        .ThenInclude(bs => bs.Specie)
        .ThenInclude(s => s.ClassificationEntity)
        .AsQueryable();

    public async Task<ResultT<List<BiomeStatsDto>>> GetStatsAsync(string? userId, bool isAdmin)
    {
        try
        {
            var query = _query;
            
            if (!isAdmin)
            {
                query = query.Where(b => userId != null && b.UserId == userId);
            }

            var biomes = await query.ToListAsync();

            return ResultT<List<BiomeStatsDto>>.Ok(biomes.Select(b => new BiomeStatsDto
            {
                Id = b.Id,
                Name = b.Name,
                SpeciesCount = b.BiomeSpecies.Count,
                EquipmentCount = b.BiomeEquipments.Count,
                Temperature = b.Temperature,
                Humidity = b.Humidity,

                SpeciesByDiet = b.BiomeSpecies
                    .GroupBy(bs => bs.Specie.DietEntity.Label)
                    .ToDictionary(g => g.Key, g => g.Count()),

                SpeciesByClassification = b.BiomeSpecies
                    .GroupBy(bs => bs.Specie.ClassificationEntity.Label)
                    .ToDictionary(g => g.Key, g => g.Count()),

                SpeciesByName = b.BiomeSpecies
                    .GroupBy(bs => bs.Specie.Name)
                    .ToDictionary(g => g.Key, g => g.Count()),
            }).ToList());
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {EMessage}", e.Message);
            return ResultT<List<BiomeStatsDto>>.Failure($"Database error: {e.Message}");
        }
    }

    public async Task<ResultT<List<BiomeHistory>>> GetHistoryAsync(
        string? userId, bool isAdmin, int? biomeId, int days)
    {
        try
        {
            var since = days == 0 ? DateTime.MinValue : DateTime.UtcNow.AddDays(-days);

            var query = _query;

            if (!isAdmin)
                query = query.Where(b => userId != null && b.UserId == userId);

            if (biomeId.HasValue)
                query = query.Where(b => b.Id == biomeId.Value);

            var biomes = await query.ToListAsync();

            return ResultT<List<BiomeHistory>>.Ok(biomes.Select(b =>
            {
                var speciesInRange = b.BiomeSpecies
                    .Where(bs => bs.Inserted >= since)
                    .OrderBy(bs => bs.Inserted)
                    .ToList();

                var speciesOverTime = speciesInRange
                    .GroupBy(bs => bs.Inserted.ToString("yyyy-MM-dd"))
                    .Select(g => new HistoryPoint
                    {
                        Date = g.Key,
                        Label = "Espèces",
                        Count = g.Count()
                    })
                    .OrderBy(p => p.Date)
                    .ToList();

                var cumul = 0;
                foreach (var point in speciesOverTime)
                {
                    cumul += point.Count;
                    point.Count = cumul;
                }

                var byDiet = speciesInRange
                    .GroupBy(bs => new
                    {
                        Date = bs.Inserted.ToString("yyyy-MM-dd"),
                        Diet = bs.Specie.DietEntity.Label
                    })
                    .Select(g => new HistoryPoint
                    {
                        Date = g.Key.Date,
                        Label = g.Key.Diet,
                        Count = g.Count()
                    })
                    .OrderBy(p => p.Date)
                    .ToList();

                var byClassification = speciesInRange
                    .GroupBy(bs => new
                    {
                        Date = bs.Inserted.ToString("yyyy-MM-dd"),
                        Class = bs.Specie.ClassificationEntity.Label
                    })
                    .Select(g => new HistoryPoint
                    {
                        Date = g.Key.Date,
                        Label = g.Key.Class,
                        Count = g.Count()
                    })
                    .OrderBy(p => p.Date)
                    .ToList();

                return new BiomeHistory
                {
                    BiomeId = b.Id,
                    BiomeName = b.Name,
                    SpeciesOverTime = speciesOverTime,
                    ByDietOverTime = byDiet,
                    ByClassificationOverTime = byClassification
                };
            }).ToList());
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Database error: {EMessage}", e.Message);
            return ResultT<List<BiomeHistory>>.Failure($"Database error: {e.Message}");
        }
    }
}