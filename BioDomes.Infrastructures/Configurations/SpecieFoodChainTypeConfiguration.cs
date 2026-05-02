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
            new SpecieFoodChain { PredatorId = 6, PreyId = 4 },  // lievre -> hetre
            new SpecieFoodChain { PredatorId = 6, PreyId = 9 },  // lievre -> paturin-des-pres

            new SpecieFoodChain { PredatorId = 10, PreyId = 9 }, // souris -> paturin-des-pres
            new SpecieFoodChain { PredatorId = 10, PreyId = 8 }, // souris -> mouche

            new SpecieFoodChain { PredatorId = 1, PreyId = 8 },  // araignee -> mouche

            new SpecieFoodChain { PredatorId = 5, PreyId = 1 },  // hirondelle -> araignee

            new SpecieFoodChain { PredatorId = 2, PreyId = 10 }, // boa -> souris
            new SpecieFoodChain { PredatorId = 2, PreyId = 5 },  // boa -> hirondelle

            new SpecieFoodChain { PredatorId = 7, PreyId = 6 },  // lion -> lievre
            new SpecieFoodChain { PredatorId = 7, PreyId = 10 }, // lion -> souris

            new SpecieFoodChain { PredatorId = 10, PreyId = 3 }, // souris -> chene

            new SpecieFoodChain { PredatorId = 5, PreyId = 8 },  // hirondelle -> mouche

            new SpecieFoodChain { PredatorId = 1, PreyId = 10 }  // araignee -> souris
        );
    }
}