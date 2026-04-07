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
            .HasMaxLength(75)
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

        builder.Property(specie => specie.IsPublic)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(specie => specie.Slug)
            .IsUnique();
        
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
                DietLabel = "Carnivore",
                IsPublic = false
            },
            new
            {
                Slug = "boa",
                Name = "Boa",
                Height = 225,
                Weight = 13000,
                UploadedImageId = (string?)null,
                TypeLabel = "Reptile",
                DietLabel = "Carnivore",
                IsPublic = false
            },
            new
            {
                Slug = "hetre",
                Name = "Hetre",
                Height = 600,
                Weight = 830000,
                UploadedImageId = (string?)null,
                TypeLabel = "Plante",
                DietLabel = "Photosynthèse",
                IsPublic = false
            },
            new
            {
                Slug = "chene",
                Name = "Chene",
                Height = 2500,
                Weight = 2100000,
                UploadedImageId = (string?)null,
                TypeLabel = "Plante",
                DietLabel = "Photosynthèse",
                IsPublic = false
            },
            new
            {
                Slug = "mouche",
                Name = "Mouche",
                Height = 1,
                Weight = 1,
                UploadedImageId = (string?)null,
                TypeLabel = "Insecte",
                DietLabel = "Coprophage",
                IsPublic = false
            },
            new
            {
                Slug = "araignee",
                Name = "Araignée",
                Height = 4,
                Weight = 95,
                UploadedImageId = (string?)null,
                TypeLabel = "Arachnides",
                DietLabel = "Carnivore",
                IsPublic = false
            },
            new
            {
                Slug = "hirondelle",
                Name = "Hirondelle",
                Height = 18,
                Weight = 45,
                UploadedImageId = (string?)null,
                TypeLabel = "Oiseau",
                DietLabel = "Carnivore",
                IsPublic = false
            },
            new
            {
                Slug = "lievre",
                Name = "Lièvre",
                Height = 50,
                Weight = 4000,
                UploadedImageId = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Herbivore",
                IsPublic = false
            },
            new
            {
                Slug = "paturin-des-pres",
                Name = "Pâturin des prés",
                Height = 23,
                Weight = 1,
                UploadedImageId = (string?)null,
                TypeLabel = "Plante",
                DietLabel = "Photosynthèse",
                IsPublic = false
            },
            new
            {
                Slug = "souris",
                Name = "Souris",
                Height = 9,
                Weight = 25,
                UploadedImageId = (string?)null,
                TypeLabel = "Mammifère",
                DietLabel = "Omnivore",
                IsPublic = false
            }
        );
    }
}
