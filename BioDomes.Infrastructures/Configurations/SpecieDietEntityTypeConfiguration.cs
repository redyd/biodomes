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
            new SpecieDiet { Label = "Photosynthèse" }
        );
    }
}
