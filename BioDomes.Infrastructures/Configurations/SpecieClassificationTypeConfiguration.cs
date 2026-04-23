using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieClassificationTypeConfiguration : IEntityTypeConfiguration<SpecieClassification>
{
    public void Configure(EntityTypeBuilder<SpecieClassification> builder)
    {
        builder.ToTable("specie_classifications");

        builder.HasKey(sc => sc.Id);
        builder.Property(sc => sc.Label)
            .IsRequired()
            .HasMaxLength(50);

        Seed(builder);
    }

    private void Seed(EntityTypeBuilder<SpecieClassification> builder)
    {
        builder.HasData(
            new SpecieClassification { Id = 1, Label = "Mammifère" },
            new SpecieClassification { Id = 2, Label = "Oiseau" },
            new SpecieClassification { Id = 3, Label = "Poisson" },
            new SpecieClassification { Id = 4, Label = "Reptile" },
            new SpecieClassification { Id = 5, Label = "Amphibien" },
            new SpecieClassification { Id = 6, Label = "Crustacé" },
            new SpecieClassification { Id = 7, Label = "Arachnides" },
            new SpecieClassification { Id = 8, Label = "Insecte" },
            new SpecieClassification { Id = 9, Label = "Myriapode" },
            new SpecieClassification { Id = 10, Label = "Mollusque" },
            new SpecieClassification { Id = 11, Label = "Echinoderme" },
            new SpecieClassification { Id = 12, Label = "Anélide" },
            new SpecieClassification { Id = 13, Label = "Cnidaire" },
            new SpecieClassification { Id = 14, Label = "Plathelminthe" },
            new SpecieClassification { Id = 15, Label = "Porifère" },
            new SpecieClassification { Id = 16, Label = "Plante" },
            new SpecieClassification { Id = 17, Label = "Mousse" },
            new SpecieClassification { Id = 18, Label = "Algue" },
            new SpecieClassification { Id = 19, Label = "Lichen" },
            new SpecieClassification { Id = 20, Label = "Champignon" }
        );
    }
}