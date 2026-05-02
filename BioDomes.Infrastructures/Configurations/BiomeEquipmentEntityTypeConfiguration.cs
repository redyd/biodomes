using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class BiomeEquipmentEntityTypeConfiguration : IEntityTypeConfiguration<BiomeEquipmentEntity>
{
    public void Configure(EntityTypeBuilder<BiomeEquipmentEntity> builder)
    {
        builder.ToTable("biomes_equipments");
        
        builder.HasKey(be => be.Id);
        
        builder.HasOne(bs => bs.Biome)
            .WithMany(b => b.BiomeEquipments)
            .HasForeignKey(bs => bs.BiomeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bs => bs.Equipment)
            .WithMany(s => s.BiomesEquipment)
            .HasForeignKey(bs => bs.EquipmentId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Property(be => be.Inserted)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnAdd();
        
        Seed(builder);
    }

    private static void Seed(EntityTypeBuilder<BiomeEquipmentEntity> builder)
    {
        var seed = new (int EquipmentId, int Count)[]
        {
            (3, 2), // Extracteur de CO2
            (1, 3), // Générateur de lumière artificielle
            (6, 2), // Recycleur de biomasse
        };

        int startId = 1;
        builder.HasData(
            seed.SelectMany((s, si) =>
                Enumerable.Range(0, s.Count).Select((_, i) =>
                    new BiomeEquipmentEntity
                    {
                        Id = startId + seed.Take(si).Sum(x => x.Count) + i,
                        EquipmentId = s.EquipmentId,
                        BiomeId = 1,
                    }
                )
            ).ToArray()
        );
    }
}