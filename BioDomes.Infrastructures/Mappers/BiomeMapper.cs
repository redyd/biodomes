using BioDomes.Domains.Core;
using BioDomes.Infrastructures.Models;

namespace BioDomes.Infrastructures.Mappers;

public class BiomeMapper
{
    public static Biome MapSimpleBiome(BiomeEntity entity)
        => new()
        {
            CreatedBy = entity.UserId,
            Id = entity.Id,
            Name = entity.Name,
            Temperature = entity.Temperature,
            Humidity = entity.Humidity,
            ImageUrl = entity.ImageUrl,
            IsPublic = entity.IsPublic,
            Equipments = [],
            Species = []
        };

    public static Biome MapComplexBiome(BiomeEntity entity)
    {
        return new Biome
        {
            Id = entity.Id,
            Name = entity.Name,
            Temperature = entity.Temperature,
            Humidity = entity.Humidity,
            ImageUrl = entity.ImageUrl,
            IsPublic = entity.IsPublic,
            CreatedBy = entity.UserId,
            Equipments = entity
                .BiomeEquipments
                .GroupBy(be => be.EquipmentId)
                .Select(be => new Tracked<Equipment>
                {
                    Model = EquipmentMapper.MapEquipment(be.First().Equipment),
                    TrackedInsertionsById = be.ToDictionary(all => all.Id, all => all.Inserted)
                }).ToList(),
            Species = entity
                .BiomeSpecies
                .GroupBy(bs => bs.SpecieId)
                .Select(bs => new Tracked<Specie>
                {
                    Model = SpecieMapper.MapSpecie(bs.First().Specie),
                    TrackedInsertionsById = bs.ToDictionary(all => all.Id, all => all.Inserted)
                }).ToList(),
        };
    }
}