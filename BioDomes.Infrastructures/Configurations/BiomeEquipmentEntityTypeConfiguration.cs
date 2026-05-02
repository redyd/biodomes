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
    }
}