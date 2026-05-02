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
    }
}