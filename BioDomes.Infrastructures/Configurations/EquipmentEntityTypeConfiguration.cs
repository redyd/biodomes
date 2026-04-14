using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class EquipmentEntityTypeConfiguration : IEntityTypeConfiguration<EquipmentEntity>
{
    public void Configure(EntityTypeBuilder<EquipmentEntity> builder)
    {
        builder.ToTable("equipments");
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(e => e.Image)
            .IsRequired();
        
        builder.Property(e => e.IsPublic)
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.HasOne(e => e.ProducedResourceEntity)
            .WithMany()
            .HasForeignKey(e => e.ProducedResourceId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.ConsumedResourceEntity)
            .WithMany()
            .HasForeignKey(e => e.ConsumedResourceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.User)
            .WithMany()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        SeedData(builder);
    }

    private void SeedData(EntityTypeBuilder<EquipmentEntity> builder)
    {
        builder.HasData(new List<EquipmentEntity>
        {
            new()
            {
                Id = 1,
                Name = "Générateur de lumière artificielle",
                Image = "ca0fbc00-841b-41bb-8329-b0c95931d24e.png",
                ProducedResourceId = 1, // Lumière
                ConsumedResourceId = 11, // Énergie
                IsPublic = false,
                UserId = "user-1"
            },
            new()
            {
                Id = 2,
                Name = "Condenseur atmosphérique",
                Image = "e7a73200-4110-492e-ad32-e10c6654995b.png",
                ProducedResourceId = 2, // Eau
                ConsumedResourceId = 5, // Oxygène
                IsPublic = true,
                UserId = "user-1"
            },
            new()
            {
                Id = 3,
                Name = "Extracteur de CO2",
                Image = "2fa0bd7e-1425-4359-a841-9b6a59b1d3f6.png",
                ProducedResourceId = 6, // CO2
                ConsumedResourceId = 5, // Oxygène
                IsPublic = false,
                UserId = "user-1"
            },
            new()
            {
                Id = 4,
                Name = "Module de photosynthèse",
                Image = "bff55fcc-19e0-4c59-9575-6689dedb8d05.png",
                ProducedResourceId = 5, // Oxygène
                ConsumedResourceId = 6, // CO2
                IsPublic = true,
                UserId = "user-1"
            },
            new()
            {
                Id = 5,
                Name = "Synthétiseur d'engrais",
                Image = "1056b6e4-fa44-4d5d-a309-13e7a7bd9999.png",
                ProducedResourceId = 7, // Phosphore
                ConsumedResourceId = 3, // Azote
                IsPublic = false,
                UserId = "user-2"
            },
            new()
            {
                Id = 6,
                Name = "Recycleur de biomasse",
                Image = "a430eda5-2206-4963-8f6e-c9731c0d6bef.png",
                ProducedResourceId = 3, // Azote
                ConsumedResourceId = 12, // Matière organique
                IsPublic = true,
                UserId = "user-2"
            },
            new()
            {
                Id = 7,
                Name = "Filtre minéral",
                Image = "861decc8-a7e7-4b8c-8f31-76b3c09342b9.png",
                ProducedResourceId = 9, // Calcium
                ConsumedResourceId = 2, // Eau
                IsPublic = false,
                UserId = "user-2"
            },
            new()
            {
                Id = 8,
                Name = "Extracteur d'électrolytes",
                Image = "4fba83ed-a55f-4e1b-80b5-6829a696b7b3.png",
                ProducedResourceId = 10, // Magnésium
                ConsumedResourceId = 2, // Eau
                IsPublic = true,
                UserId = "user-2"
            }
        });

    }
}