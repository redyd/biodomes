using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieEntityTypeConfiguration : IEntityTypeConfiguration<Specie>
{
    public void Configure(EntityTypeBuilder<Specie> builder)
    {
        builder.HasKey(specie => specie.Slug);
        
        builder.Property(specie => specie.Name)
            .IsRequired();
        
        builder.Property(specie => specie.Height)
            .IsRequired();

        builder.Property(specie => specie.ImageUrl);

        builder.Property(specie => specie.TypeLabel)
            .IsRequired();

        builder.Property(specie => specie.DietLabel)
            .IsRequired();

        builder.HasOne(specie => specie.Type)
            .WithMany(type => type.Species)
            .HasForeignKey(specie => specie.TypeLabel)
            .HasPrincipalKey(type => type.Label)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(specie => specie.Diet)
            .WithMany(diet => diet.Species)
            .HasForeignKey(specie => specie.DietLabel)
            .HasPrincipalKey(diet => diet.Label)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(specie => specie.Slug).IsUnique();

        SeedSpecies(builder);
    }

    private static void SeedSpecies(EntityTypeBuilder<Specie> builder)
    {
        builder.HasData(
            new
            {
                Slug = "lion",
                Name = "Lion",
                Height = 120,
                ImageUrl = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Carnivore"
            },
            new
            {
                Slug = "vache",
                Name = "Vache",
                Height = 150,
                ImageUrl = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Herbivore"
            },
            new
            {
                Slug = "aigle",
                Name = "Aigle",
                Height = 70,
                ImageUrl = (string?)null,
                TypeLabel = "Oiseau",
                DietLabel = "Carnivore"
            },
            new
            {
                Slug = "chene",
                Name = "Chêne",
                Height = 2000,
                ImageUrl = (string?)null,
                TypeLabel = "Plante",
                DietLabel = "Photosynthèse"
            },
            new
            {
                Slug = "renard",
                Name = "Renard",
                Height = 40,
                ImageUrl = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Omnivore"
            },
            new
            {
                Slug = "requin",
                Name = "Requin",
                Height = 500,
                ImageUrl = (string?)null,
                TypeLabel = "Poisson",
                DietLabel = "Carnivore"
            },
            new
            {
                Slug = "bambou",
                Name = "Bambou",
                Height = 1000,
                ImageUrl = (string?)null,
                TypeLabel = "Plante",
                DietLabel = "Photosynthèse"
            },
            new
            {
                Slug = "ours",
                Name = "Ours",
                Height = 180,
                ImageUrl = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Omnivore"
            },
            new
            {
                Slug = "poulet",
                Name = "Poulet",
                Height = 45,
                ImageUrl = (string?)null,
                TypeLabel = "Oiseau",
                DietLabel = "Omnivore"
            },
            new
            {
                Slug = "lapin",
                Name = "Lapin",
                Height = 35,
                ImageUrl = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Herbivore"
            }
        );
    }
}
