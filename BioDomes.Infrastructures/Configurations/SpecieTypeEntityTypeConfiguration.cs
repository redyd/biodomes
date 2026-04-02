using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieTypeEntityTypeConfiguration : IEntityTypeConfiguration<SpecieType>
{
    public void Configure(EntityTypeBuilder<SpecieType> builder)
    {
        builder.HasKey(type => type.Label);

        builder.Property(type => type.Label)
            .IsRequired();

        builder.HasData(
            new SpecieType { Label = "Mammifères" },
            new SpecieType { Label = "Oiseaux" },
            new SpecieType { Label = "Poissons" },
            new SpecieType { Label = "Reptiles" },
            new SpecieType { Label = "Amphibiens" },
            new SpecieType { Label = "Crustacés" },
            new SpecieType { Label = "Arachnides" },
            new SpecieType { Label = "Insectes" },
            new SpecieType { Label = "Myriapodes" },
            new SpecieType { Label = "Mollusques" },
            new SpecieType { Label = "Echinodermes" },
            new SpecieType { Label = "Anélides" },
            new SpecieType { Label = "Cnidaires" },
            new SpecieType { Label = "Plathelminthes" },
            new SpecieType { Label = "Porifères" },
            new SpecieType { Label = "Plantes" },
            new SpecieType { Label = "Mousses" },
            new SpecieType { Label = "Algues" },
            new SpecieType { Label = "Lichen" },
            new SpecieType { Label = "Champignons" },
            new SpecieType { Label = "Mammifère" },
            new SpecieType { Label = "Oiseau" },
            new SpecieType { Label = "Reptile" },
            new SpecieType { Label = "Plante" },
            new SpecieType { Label = "Insecte" }
        );
    }
}
