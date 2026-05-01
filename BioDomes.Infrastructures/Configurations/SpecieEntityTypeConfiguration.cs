using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieEntityTypeConfiguration : IEntityTypeConfiguration<SpecieEntity>
{
    public void Configure(EntityTypeBuilder<SpecieEntity> builder)
    {
        builder.ToTable("species");

        builder.HasKey(specie => specie.Id);

        builder.Property(specie => specie.Slug)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(specie => specie.Slug)
            .IsUnique();

        builder.Property(specie => specie.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(specie => specie.Height)
            .IsRequired();

        builder.Property(specie => specie.Weight)
            .IsRequired();

        builder.Property(specie => specie.UploadedImageId);

        builder.HasOne(specie => specie.ClassificationEntity)
            .WithMany()
            .HasForeignKey(specie => specie.ClassificationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(specie => specie.DietEntity)
            .WithMany()
            .HasForeignKey(specie => specie.DietId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(specie => specie.IsPublic)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(specie => specie.Id)
            .IsUnique();

        SeedSpecies(builder);
    }

    private static void SeedSpecies(EntityTypeBuilder<SpecieEntity> builder)
    {
        builder.HasData(
            new SpecieEntity
            {
                Id = 1,
                Name = "Lion",
                Slug = "lion",
                Height = 180,
                Weight = 150000,
                UploadedImageId = "3d128e91-b947-4bf7-b988-dc8ffd1de92f.jpg",
                ClassificationId = 1, // Mammifère
                DietId = 1, // Carnivore
                IsPublic = false,
                UserId = "user-1"
            },
            new SpecieEntity
            {
                Id = 2,
                Name = "Boa",
                Slug = "boa",
                Height = 225,
                Weight = 13000,
                UploadedImageId = "47e0e461-066e-404f-8738-361d268f6219.jpg",
                ClassificationId = 4, // Reptile
                DietId = 1, // Carnivore
                IsPublic = true,
                UserId = "user-2"
            },
            new SpecieEntity
            {
                Id = 3,
                Name = "Hêtre",
                Slug = "hetre",
                Height = 600,
                Weight = 830000,
                UploadedImageId = "9f21e809-9eeb-4714-8cd0-14b78d14fb60.jpg",
                ClassificationId = 16, // Plante
                DietId = 7, // Photosynthèse
                IsPublic = false,
                UserId = "user-1"
            },
            new SpecieEntity
            {
                Id = 4,
                Name = "Chêne",
                Slug = "chene",
                Height = 2500,
                Weight = 2100000,
                UploadedImageId = null,
                ClassificationId = 16,
                DietId = 7,
                IsPublic = false,
                UserId = "user-2"
            },
            new SpecieEntity
            {
                Id = 5,
                Name = "Mouche",
                Slug = "mouche",
                Height = 1,
                Weight = 1,
                UploadedImageId = "2b101f74-b1f0-4c18-a183-0331f7a4788c.jpg",
                ClassificationId = 8, // Insecte
                DietId = 4, // Coprophages
                IsPublic = false,
                UserId = "user-1"
            },
            new SpecieEntity
            {
                Id = 6,
                Name = "Araignée",
                Slug = "araignee",
                Height = 4,
                Weight = 95,
                UploadedImageId = "623fbf44-7418-44d7-8098-6cc6c7122c27.jpg",
                ClassificationId = 7, // Arachnides
                DietId = 1, // Carnivore
                IsPublic = true,
                UserId = "user-1"
            },
            new SpecieEntity
            {
                Id = 7,
                Name = "Hirondelle",
                Slug = "hirondelle",
                Height = 18,
                Weight = 45,
                UploadedImageId = null,
                ClassificationId = 2, // Oiseau
                DietId = 1, // Carnivore
                IsPublic = true,
                UserId = "user-1"
            },
            new SpecieEntity
            {
                Id = 8,
                Name = "Lièvre",
                Slug = "lievre",
                Height = 50,
                Weight = 4000,
                UploadedImageId = "7f084f4d-de3c-4436-b359-6fdfd4b93ec8.jpg",
                ClassificationId = 1, // Mammifère
                DietId = 2, // Herbivore
                IsPublic = true,
                UserId = "user-2"
            },
            new SpecieEntity
            {
                Id = 9,
                Name = "Pâturin des prés",
                Slug = "paturin-des-pres",
                Height = 23,
                Weight = 1,
                UploadedImageId = null,
                ClassificationId = 16,
                DietId = 7,
                IsPublic = false,
                UserId = "user-1"
            },
            new SpecieEntity
            {
                Id = 10,
                Name = "Souris",
                Slug = "souris",
                Height = 9,
                Weight = 25,
                UploadedImageId = "7e6d0ba0-7be3-4f94-a929-e7c04d3e1d90.jpg",
                ClassificationId = 1,
                DietId = 3, // Omnivore
                IsPublic = true,
                UserId = "user-2"
            }
        );
    }
}