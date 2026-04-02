using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieDietEntityTypeConfiguration : IEntityTypeConfiguration<SpecieDietEntity>
{
    public void Configure(EntityTypeBuilder<SpecieDietEntity> builder)
    {
        builder.ToTable("diets");
        
        builder.HasKey(diet => diet.Label);

        builder.Property(diet => diet.Label)
            .IsRequired();

        builder.HasData(
            new SpecieDietEntity { Label = "Carnivore" },
            new SpecieDietEntity { Label = "Herbivore" },
            new SpecieDietEntity { Label = "Omnivore" },
            new SpecieDietEntity { Label = "Coprophages" },
            new SpecieDietEntity { Label = "Coprophage" },
            new SpecieDietEntity { Label = "Limivores" },
            new SpecieDietEntity { Label = "Détritivores" },
            new SpecieDietEntity { Label = "Photosynthèse" },
            new SpecieDietEntity { Label = "Hétérotrophie" }
        );
    }
}
