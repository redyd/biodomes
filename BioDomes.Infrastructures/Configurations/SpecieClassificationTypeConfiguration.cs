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

        builder.HasMany(sc => sc.EatenByDiets)
            .WithMany(sd => sd.CanEatClassifications)
            .UsingEntity<Dictionary<string, object>>(
                "classification_eaten_by_diet",
                j => j.HasOne<SpecieDiet>().WithMany().HasForeignKey("diet_id"),
                j => j.HasOne<SpecieClassification>().WithMany().HasForeignKey("classification_id"),
                j =>
                {
                    j.HasKey("classification_id", "diet_id");
                    SeedJoinTable(j);
                });

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

    private void SeedJoinTable(EntityTypeBuilder<Dictionary<string, object>> j)
    {
        j.HasData(
            // --- VERTÉBRÉS (Mangés par Carnivores: 1, Omnivores: 3) ---
            new { classification_id = 1, diet_id = 1 }, new { classification_id = 1, diet_id = 3 }, // Mammifères
            new { classification_id = 2, diet_id = 1 }, new { classification_id = 2, diet_id = 3 }, // Oiseaux
            new { classification_id = 3, diet_id = 1 }, new { classification_id = 3, diet_id = 3 }, // Poissons
            new { classification_id = 4, diet_id = 1 }, new { classification_id = 4, diet_id = 3 }, // Reptiles
            new { classification_id = 5, diet_id = 1 }, new { classification_id = 5, diet_id = 3 }, // Amphibiens

            // --- INVERTÉBRÉS (Mangés par Carnivores: 1, Omnivores: 3) ---
            new { classification_id = 6, diet_id = 1 }, new { classification_id = 6, diet_id = 3 }, // Crustacés
            new { classification_id = 7, diet_id = 1 }, new { classification_id = 7, diet_id = 3 }, // Arachnides
            new { classification_id = 8, diet_id = 1 }, new { classification_id = 8, diet_id = 3 }, // Insectes
            new { classification_id = 9, diet_id = 1 }, new { classification_id = 9, diet_id = 3 }, // Myriapodes
            new { classification_id = 10, diet_id = 1 }, new { classification_id = 10, diet_id = 3 }, // Mollusques
            new { classification_id = 11, diet_id = 1 }, new { classification_id = 11, diet_id = 3 }, // Echinodermes
            new { classification_id = 12, diet_id = 1 }, new { classification_id = 12, diet_id = 3 }, // Anélides
            new { classification_id = 13, diet_id = 1 }, new { classification_id = 13, diet_id = 3 }, // Cnidaires
            new { classification_id = 14, diet_id = 1 }, new { classification_id = 14, diet_id = 3 }, // Plathelminthes
            new { classification_id = 15, diet_id = 1 }, new { classification_id = 15, diet_id = 3 }, // Porifères

            // --- PRODUCTEURS PRIMAIRES (Mangés par Herbivores: 2, Omnivores: 3) ---
            new { classification_id = 16, diet_id = 2 }, new { classification_id = 16, diet_id = 3 }, // Plantes
            new { classification_id = 17, diet_id = 2 }, new { classification_id = 17, diet_id = 3 }, // Mousses
            new { classification_id = 18, diet_id = 2 }, new { classification_id = 18, diet_id = 3 }, // Algues

            // --- AUTRES / DÉCOMPOSEURS (Mangés par Détritivores: 6, Herbivores: 2, Omnivores: 3) ---
            new { classification_id = 19, diet_id = 6 }, new { classification_id = 19, diet_id = 2 }, // Lichen
            new { classification_id = 20, diet_id = 6 }, new { classification_id = 20, diet_id = 3 } // Champignon
        );
    }
}