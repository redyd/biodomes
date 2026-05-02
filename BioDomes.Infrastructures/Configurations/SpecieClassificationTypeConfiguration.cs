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

        builder.Property(sc => sc.RequireFood)
            .IsRequired();

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
            // --- VERTÉBRÉS (RequireFood = true) ---
            new SpecieClassification { Id = 1, Label = "Mammifère", RequireFood = true },
            new SpecieClassification { Id = 2, Label = "Oiseau", RequireFood = true },
            new SpecieClassification { Id = 3, Label = "Poisson", RequireFood = true },
            new SpecieClassification { Id = 4, Label = "Reptile", RequireFood = true },
            new SpecieClassification { Id = 5, Label = "Amphibien", RequireFood = true },

            // --- INVERTÉBRÉS (RequireFood = true) ---
            new SpecieClassification { Id = 6, Label = "Crustacé", RequireFood = true },
            new SpecieClassification { Id = 7, Label = "Arachnides", RequireFood = true },
            new SpecieClassification { Id = 8, Label = "Insecte", RequireFood = true },
            new SpecieClassification { Id = 9, Label = "Myriapode", RequireFood = true },
            new SpecieClassification { Id = 10, Label = "Mollusque", RequireFood = true },
            new SpecieClassification { Id = 11, Label = "Echinoderme", RequireFood = true },
            new SpecieClassification { Id = 12, Label = "Anélide", RequireFood = true },
            new SpecieClassification { Id = 13, Label = "Cnidaire", RequireFood = true },
            new SpecieClassification { Id = 14, Label = "Plathelminthe", RequireFood = true },
            new SpecieClassification { Id = 15, Label = "Porifère", RequireFood = true },

            // --- PRODUCTEURS PRIMAIRES (RequireFood = false — autotrophes) ---
            new SpecieClassification { Id = 16, Label = "Plante", RequireFood = false },
            new SpecieClassification { Id = 17, Label = "Mousse", RequireFood = false },
            new SpecieClassification { Id = 18, Label = "Algue", RequireFood = false },

            // --- DÉCOMPOSEURS (RequireFood = false — se nourrissent de matière morte non-vivante) ---
            new SpecieClassification { Id = 19, Label = "Lichen", RequireFood = false },
            new SpecieClassification { Id = 20, Label = "Champignon", RequireFood = false }
        );
    }

    private void SeedJoinTable(EntityTypeBuilder<Dictionary<string, object>> j)
    {
        j.HasData(
            // --- VERTÉBRÉS ---
            // Mammifères : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 1, diet_id = 1 },
            new { classification_id = 1, diet_id = 3 },
            new { classification_id = 1, diet_id = 8 },

            // Oiseaux : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 2, diet_id = 1 },
            new { classification_id = 2, diet_id = 3 },
            new { classification_id = 2, diet_id = 8 },

            // Poissons : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 3, diet_id = 1 },
            new { classification_id = 3, diet_id = 3 },
            new { classification_id = 3, diet_id = 8 },

            // Reptiles : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 4, diet_id = 1 },
            new { classification_id = 4, diet_id = 3 },
            new { classification_id = 4, diet_id = 8 },

            // Amphibiens : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 5, diet_id = 1 },
            new { classification_id = 5, diet_id = 3 },
            new { classification_id = 5, diet_id = 8 },

            // --- INVERTÉBRÉS ---
            // Crustacés : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 6, diet_id = 1 },
            new { classification_id = 6, diet_id = 3 },
            new { classification_id = 6, diet_id = 8 },

            // Arachnides : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 7, diet_id = 1 },
            new { classification_id = 7, diet_id = 3 },
            new { classification_id = 7, diet_id = 8 },

            // Insectes : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 8, diet_id = 1 },
            new { classification_id = 8, diet_id = 3 },
            new { classification_id = 8, diet_id = 8 },

            // Myriapodes : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 9, diet_id = 1 },
            new { classification_id = 9, diet_id = 3 },
            new { classification_id = 9, diet_id = 8 },

            // Mollusques : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8),
            //              Coprophages (4), Limivores (5)
            new { classification_id = 10, diet_id = 1 },
            new { classification_id = 10, diet_id = 3 },
            new { classification_id = 10, diet_id = 8 },
            new { classification_id = 10, diet_id = 4 },
            new { classification_id = 10, diet_id = 5 },

            // Echinodermes : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 11, diet_id = 1 },
            new { classification_id = 11, diet_id = 3 },
            new { classification_id = 11, diet_id = 8 },

            // Anélides : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8),
            //            Limivores (5), Coprophages (4)
            new { classification_id = 12, diet_id = 1 },
            new { classification_id = 12, diet_id = 3 },
            new { classification_id = 12, diet_id = 8 },
            new { classification_id = 12, diet_id = 5 },
            new { classification_id = 12, diet_id = 4 },

            // Cnidaires : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 13, diet_id = 1 },
            new { classification_id = 13, diet_id = 3 },
            new { classification_id = 13, diet_id = 8 },

            // Plathelminthes : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 14, diet_id = 1 },
            new { classification_id = 14, diet_id = 3 },
            new { classification_id = 14, diet_id = 8 },

            // Porifères : mangés par Carnivores (1), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 15, diet_id = 1 },
            new { classification_id = 15, diet_id = 3 },
            new { classification_id = 15, diet_id = 8 },

            // --- PRODUCTEURS PRIMAIRES ---
            // Plantes : mangées par Herbivores (2), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 16, diet_id = 2 },
            new { classification_id = 16, diet_id = 3 },
            new { classification_id = 16, diet_id = 8 },

            // Mousses : mangées par Herbivores (2), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 17, diet_id = 2 },
            new { classification_id = 17, diet_id = 3 },
            new { classification_id = 17, diet_id = 8 },

            // Algues : mangées par Herbivores (2), Omnivores (3), Hétérotrophes (8)
            new { classification_id = 18, diet_id = 2 },
            new { classification_id = 18, diet_id = 3 },
            new { classification_id = 18, diet_id = 8 },

            // --- DÉCOMPOSEURS ---
            // Lichen : mangé par Détritivores (6), Herbivores (2), Coprophages (4)
            new { classification_id = 19, diet_id = 6 },
            new { classification_id = 19, diet_id = 2 },
            new { classification_id = 19, diet_id = 4 },

            // Champignon : mangé par Détritivores (6), Omnivores (3), Coprophages (4)
            new { classification_id = 20, diet_id = 6 },
            new { classification_id = 20, diet_id = 3 },
            new { classification_id = 20, diet_id = 4 }
        );
    }
}