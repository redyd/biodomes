using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class EquipmentResourceEntityTypeConfiguration : IEntityTypeConfiguration<EquipmentResourceEntity>
{
    public void Configure(EntityTypeBuilder<EquipmentResourceEntity> builder)
    {
        builder.ToTable("equipment_resources");

        builder.HasKey(eq => eq.Id);

        builder.Property(eq => eq.Label)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasMany(eq => eq.Equipments)
            .WithOne();

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<EquipmentResourceEntity> builder)
    {
        builder.HasData(
            new { Id = 1, Label = "Lumière" },
            new { Id = 2, Label = "Eau" },
            new { Id = 3, Label = "Azote" },
            new { Id = 4, Label = "Hydrogène" },
            new { Id = 5, Label = "Oxygène" },
            new { Id = 6, Label = "CO2" },
            new { Id = 7, Label = "Phosphore" },
            new { Id = 8, Label = "Potassium" },
            new { Id = 9, Label = "Calcium" },
            new { Id = 10, Label = "Magnésium" }
        );
    }
}