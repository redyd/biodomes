using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieDietEntityTypeConfiguration : IEntityTypeConfiguration<SpecieDiet>
{
    public void Configure(EntityTypeBuilder<SpecieDiet> builder)
    {
        builder.HasKey(diet => diet.Label);

        builder.Property(diet => diet.Label)
            .IsRequired();

        builder.HasData(
            new SpecieDiet { Label = "Carnivore" },
            new SpecieDiet { Label = "Herbivore" },
            new SpecieDiet { Label = "Omnivore" },
            new SpecieDiet { Label = "Coprophages" },
            new SpecieDiet { Label = "Coprophage" },
            new SpecieDiet { Label = "Limivores" },
            new SpecieDiet { Label = "Détritivores" },
            new SpecieDiet { Label = "Photosynthèse" },
            new SpecieDiet { Label = "Hétérotrophie" }
        );
    }
}
