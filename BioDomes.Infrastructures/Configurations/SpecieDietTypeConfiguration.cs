using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieDietTypeConfiguration : IEntityTypeConfiguration<SpecieDiet>
{
    public void Configure(EntityTypeBuilder<SpecieDiet> builder)
    {
        builder.ToTable("specie_diets");

        builder.HasKey(sd => sd.Id);
        builder.Property(sd => sd.Label)
            .IsRequired()
            .HasMaxLength(50);

        Seed(builder);
    }

    private void Seed(EntityTypeBuilder<SpecieDiet> builder)
    {
        builder.HasData(
            new SpecieDiet { Id = 1, Label = "Carnivore" },
            new SpecieDiet { Id = 2, Label = "Herbivore" },
            new SpecieDiet { Id = 3, Label = "Omnivore" },
            new SpecieDiet { Id = 4, Label = "Coprophages" },
            new SpecieDiet { Id = 5, Label = "Limivores" },
            new SpecieDiet { Id = 6, Label = "Détritivores" },
            new SpecieDiet { Id = 7, Label = "Autotrophie (Photosynthèse)" },
            new SpecieDiet { Id = 8, Label = "Hétérotrophie" }
        );
    }
}