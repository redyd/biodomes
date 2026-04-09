using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("users");
        
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .IsRequired();
        
        builder.Property(u => u.Email)
            .IsRequired();
        
        builder.Property(u => u.Password)
            .IsRequired();
        
        builder.Property(u => u.Birthdate)
            .IsRequired();
        
        builder.Property(u => u.SearchOrganism)
            .IsRequired();
        
        builder.Property(u => u.ImageUrl)
            .IsRequired();
        
        SeedUser(builder);
    }

    private static void SeedUser(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasData(
            new
            {
                Id = 1,
                Username = "admin",
                Password = "admin",
                Email = "admin@gmail.com",
                Birthdate = DateOnly.FromDateTime(DateTime.Now.AddYears(-20)),
                SearchOrganism = "adminOrganism",
                ImageUrl = ""
            });
    }
}