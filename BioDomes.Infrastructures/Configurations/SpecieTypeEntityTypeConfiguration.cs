using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieTypeEntityTypeConfiguration : IEntityTypeConfiguration<SpecieClassificationEntity>
{
    public void Configure(EntityTypeBuilder<SpecieClassificationEntity> builder)
    {
        builder.ToTable("species_classification");
        
        builder.HasKey(type => type.Label);

        builder.Property(type => type.Label)
            .IsRequired();

        builder.HasData(
            new SpecieClassificationEntity { Label = "Mammifères" },
            new SpecieClassificationEntity { Label = "Oiseaux" },
            new SpecieClassificationEntity { Label = "Poissons" },
            new SpecieClassificationEntity { Label = "Reptiles" },
            new SpecieClassificationEntity { Label = "Amphibiens" },
            new SpecieClassificationEntity { Label = "Crustacés" },
            new SpecieClassificationEntity { Label = "Arachnides" },
            new SpecieClassificationEntity { Label = "Insectes" },
            new SpecieClassificationEntity { Label = "Myriapodes" },
            new SpecieClassificationEntity { Label = "Mollusques" },
            new SpecieClassificationEntity { Label = "Echinodermes" },
            new SpecieClassificationEntity { Label = "Anélides" },
            new SpecieClassificationEntity { Label = "Cnidaires" },
            new SpecieClassificationEntity { Label = "Plathelminthes" },
            new SpecieClassificationEntity { Label = "Porifères" },
            new SpecieClassificationEntity { Label = "Plantes" },
            new SpecieClassificationEntity { Label = "Mousses" },
            new SpecieClassificationEntity { Label = "Algues" },
            new SpecieClassificationEntity { Label = "Lichen" },
            new SpecieClassificationEntity { Label = "Champignons" },
            new SpecieClassificationEntity { Label = "Mammifère" },
            new SpecieClassificationEntity { Label = "Oiseau" },
            new SpecieClassificationEntity { Label = "Reptile" },
            new SpecieClassificationEntity { Label = "Plante" },
            new SpecieClassificationEntity { Label = "Insecte" }
        );
    }
}
