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

        builder.Property(sd => sd.Slug)
            .IsRequired();
        
        builder.HasIndex(sd => sd.Slug)
            .IsUnique();

        Seed(builder);
    }

    private void Seed(EntityTypeBuilder<SpecieDiet> builder)
    {
        builder.HasData(
            new SpecieDiet { Id = 1, Label = "Carnivore", Slug = "carnivore" },
            new SpecieDiet { Id = 2, Label = "Herbivore", Slug = "herbivore" },
            new SpecieDiet { Id = 3, Label = "Omnivore", Slug = "omnivore" },
            new SpecieDiet { Id = 4, Label = "Coprophages", Slug = "coprophages" },
            new SpecieDiet { Id = 5, Label = "Limivores", Slug = "limivores" },
            new SpecieDiet { Id = 6, Label = "Détritivores", Slug = "detritivores" },
            new SpecieDiet { Id = 7, Label = "Autotrophie (Photosynthèse)", Slug = "autotrophie-photosynthese" },
            new SpecieDiet { Id = 8, Label = "Hétérotrophie", Slug = "heterotrophie" }
        );
    }
}