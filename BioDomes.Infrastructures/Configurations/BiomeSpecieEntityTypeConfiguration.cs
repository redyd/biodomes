using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class BiomeSpecieEntityTypeConfiguration : IEntityTypeConfiguration<BiomeSpecieEntity>
{
    public void Configure(EntityTypeBuilder<BiomeSpecieEntity> builder)
    {
        builder.ToTable("biomes_species");
        
        builder.HasKey(bs => new { bs.BiomeId, bs.SpecieId });

        builder.HasOne(bs => bs.Biome)
            .WithMany(b => b.BiomeSpecies)
            .HasForeignKey(bs => bs.BiomeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bs => bs.Specie)
            .WithMany(s => s.BiomeSpecies)
            .HasForeignKey(bs => bs.SpecieId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Property(bs => bs.Count)
            .IsRequired()
            .HasDefaultValue(1);
    }
}