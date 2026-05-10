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

        builder.Property(eq => eq.Slug)
            .IsRequired();
        
        builder.HasIndex(eq => eq.Slug)
            .IsUnique();

        builder.Property(eq => eq.Label)
            .IsRequired()
            .HasMaxLength(50);

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<EquipmentResourceEntity> builder)
    {
        builder.HasData(new List<EquipmentResourceEntity>
        {
            new() { Id = 1, Label = "Lumière", Slug = "lumiere" },
            new() { Id = 2, Label = "Eau", Slug = "eau" },
            new() { Id = 3, Label = "Azote", Slug = "azote" },
            new() { Id = 4, Label = "Hydrogène", Slug = "hydrogene" },
            new() { Id = 5, Label = "Oxygène", Slug = "oxygene" },
            new() { Id = 6, Label = "CO2", Slug = "co2" },
            new() { Id = 7, Label = "Phosphore", Slug = "phosphore" },
            new() { Id = 8, Label = "Potassium", Slug = "potassium" },
            new() { Id = 9, Label = "Calcium", Slug = "calcium" },
            new() { Id = 10, Label = "Magnésium", Slug = "magnesium" },
            new() { Id = 11, Label = "Énergie", Slug = "energie" },
            new() { Id = 12, Label = "Matière organique", Slug = "matiere-organique" }
        });
    }
}