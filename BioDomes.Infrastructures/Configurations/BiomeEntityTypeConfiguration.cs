using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class BiomeEntityTypeConfiguration : IEntityTypeConfiguration<BiomeEntity>
{
    public void Configure(EntityTypeBuilder<BiomeEntity> builder)
    {
        builder.ToTable("biomes");
        
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(e => e.Humidity)
            .IsRequired();
        
        builder.Property(e => e.Temperature)
            .IsRequired();
        
        builder.HasMany(e => e.Species)
            .WithMany(e => e.Biomes)
            .UsingEntity(j =>
            {
                j.ToTable("biomes_species");
                j.HasOne(typeof(SpecieEntity))
                    .WithMany()
                    .OnDelete(DeleteBehavior.NoAction); 
            });
        
        builder
            .HasMany(b => b.Equipments)
            .WithMany(e => e.Biomes)
            .UsingEntity(j => j.ToTable("biome_equipments"));
    }
}