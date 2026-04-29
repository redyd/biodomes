using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieFoodChainTypeConfiguration : IEntityTypeConfiguration<SpecieFoodChain>
{
    public void Configure(EntityTypeBuilder<SpecieFoodChain> builder)
    {
        builder.ToTable("specie_food_chains");

        builder.HasKey(r => new { r.PredatorId, r.PreyId });

        builder
            .HasOne(r => r.Predator)
            .WithMany(s => s.Preys)
            .HasForeignKey(r => r.PredatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(r => r.Prey)
            .WithMany(s => s.Predators)
            .HasForeignKey(r => r.PreyId)
            .OnDelete(DeleteBehavior.Restrict);

        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<SpecieFoodChain> builder)
    {
        builder.HasData(
            // 🌱 Plantes → herbivores
            new SpecieFoodChain { PredatorId = "lievre", PreyId = "paturin-des-pres" },
            new SpecieFoodChain { PredatorId = "lievre", PreyId = "hetre" },

            // 🐭 Omnivore (souris)
            new SpecieFoodChain { PredatorId = "souris", PreyId = "paturin-des-pres" },
            new SpecieFoodChain { PredatorId = "souris", PreyId = "mouche" },

            // 🕷️ Petit prédateur
            new SpecieFoodChain { PredatorId = "araignee", PreyId = "mouche" },

            // 🐦 Prédateur intermédiaire
            new SpecieFoodChain { PredatorId = "hirondelle", PreyId = "araignee" },

            // 🐍 Boa (multi-proies)
            new SpecieFoodChain { PredatorId = "boa", PreyId = "souris" },
            new SpecieFoodChain { PredatorId = "boa", PreyId = "hirondelle" },

            // 🦁 Lion (top predator)
            new SpecieFoodChain { PredatorId = "lion", PreyId = "lievre" },
            new SpecieFoodChain { PredatorId = "lion", PreyId = "souris" },

            // 🌿 Interaction alternative
            new SpecieFoodChain { PredatorId = "souris", PreyId = "chene" },

            // 🔁 Cycle volontaire (test algo)
            new SpecieFoodChain { PredatorId = "hirondelle", PreyId = "mouche" },

            // 🔀 Ramification
            new SpecieFoodChain { PredatorId = "araignee", PreyId = "souris" }
        );
    }
}