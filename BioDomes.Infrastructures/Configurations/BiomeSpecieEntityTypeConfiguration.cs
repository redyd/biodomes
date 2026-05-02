using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class BiomeSpecieEntityTypeConfiguration : IEntityTypeConfiguration<BiomeSpecieEntity>
{
    public void Configure(EntityTypeBuilder<BiomeSpecieEntity> builder)
    {
        builder.ToTable("biomes_species");

        builder.HasKey(bs => bs.Id);

        builder.HasOne(bs => bs.Biome)
            .WithMany(b => b.BiomeSpecies)
            .HasForeignKey(bs => bs.BiomeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bs => bs.Specie)
            .WithMany(s => s.BiomeSpecies)
            .HasForeignKey(bs => bs.SpecieId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(bs => bs.Inserted)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnAdd();

        Seed(builder);
    }

    private static void Seed(EntityTypeBuilder<BiomeSpecieEntity> builder)
    {
        var seed = new (int SpecieId, int Count)[]
        {
            (2,  1),  // Boa
            (3,  9),  // Hêtre
            (5,  5),  // Mouche
            (7,  4),  // Hirondelle
            (10, 8),  // Souris
        };

        builder.HasData(
            seed.SelectMany((s, si) =>
                Enumerable.Range(0, s.Count).Select((_, i) =>
                    new BiomeSpecieEntity
                    {
                        Id = si * 10 + i + 1, // ID unique : 1-10, 11-19, etc.
                        SpecieId = s.SpecieId,
                        BiomeId = 1,
                    }
                )
            ).ToArray()
        );
    }
}