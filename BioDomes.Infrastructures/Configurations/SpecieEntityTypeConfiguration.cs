using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieEntityTypeConfiguration : IEntityTypeConfiguration<SpecieEntity>
{
    public void Configure(EntityTypeBuilder<SpecieEntity> builder)
    {
        builder.ToTable("species");
        
        builder.HasKey(specie => specie.Slug);
        
        builder.Property(specie => specie.Name)
            .IsRequired();
        
        builder.Property(specie => specie.Height)
            .IsRequired();

        builder.Property(specie => specie.Weight)
            .IsRequired();

        builder.Property(specie => specie.UploadedImageId);

        builder.Property(specie => specie.TypeLabel)
            .IsRequired();

        builder.Property(specie => specie.DietLabel)
            .IsRequired();

        builder.HasOne(specie => specie.ClassificationEntity)
            .WithMany(type => type.Species)
            .HasForeignKey(specie => specie.TypeLabel)
            .HasPrincipalKey(type => type.Label)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(specie => specie.DietEntity)
            .WithMany(diet => diet.Species)
            .HasForeignKey(specie => specie.DietLabel)
            .HasPrincipalKey(diet => diet.Label)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(specie => specie.Slug).IsUnique();

        SeedSpecies(builder);
    }

    private static void SeedSpecies(EntityTypeBuilder<SpecieEntity> builder)
    {
        builder.HasData(
            new
            {
                Slug = "lion",
                Name = "Lion",
                Height = 180,
                Weight = 150000,
                UploadedImageId = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Carnivore"
            },
            new
            {
                Slug = "boa",
                Name = "Boa",
                Height = 225,
                Weight = 13000,
                UploadedImageId = (string?)null,
                TypeLabel = "Reptile",
                DietLabel = "Carnivore"
            },
            new
            {
                Slug = "hetre",
                Name = "Hetre",
                Height = 600,
                Weight = 830000,
                UploadedImageId = (string?)null,
                TypeLabel = "Plante",
                DietLabel = "Photosynthèse"
            },
            new
            {
                Slug = "chene",
                Name = "Chene",
                Height = 2500,
                Weight = 2100000,
                UploadedImageId = (string?)null,
                TypeLabel = "Plante",
                DietLabel = "Photosynthèse"
            },
            new
            {
                Slug = "mouche",
                Name = "Mouche",
                Height = 1,
                Weight = 1,
                UploadedImageId = (string?)null,
                TypeLabel = "Insecte",
                DietLabel = "Coprophage"
            },
            new
            {
                Slug = "araignee",
                Name = "Araignée",
                Height = 4,
                Weight = 95,
                UploadedImageId = (string?)null,
                TypeLabel = "Arachnides",
                DietLabel = "Carnivore"
            },
            new
            {
                Slug = "hirondelle",
                Name = "Hirondelle",
                Height = 18,
                Weight = 45,
                UploadedImageId = (string?)null,
                TypeLabel = "Oiseau",
                DietLabel = "Carnivore"
            },
            new
            {
                Slug = "lievre",
                Name = "Lièvre",
                Height = 50,
                Weight = 4000,
                UploadedImageId = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Herbivore"
            },
            new
            {
                Slug = "paturin-des-pres",
                Name = "Pâturin des prés",
                Height = 23,
                Weight = 1,
                UploadedImageId = (string?)null,
                TypeLabel = "Plante",
                DietLabel = "Photosynthèse"
            },
            new
            {
                Slug = "souris",
                Name = "Souris",
                Height = 9,
                Weight = 25,
                UploadedImageId = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Omnivore"
            }
        );
    }
}
