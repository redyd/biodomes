using BioDomes.Infrastructures.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.Property(u => u.Birthdate).IsRequired();
        builder.Property(u => u.SearchOrganism).IsRequired();
        builder.Property(u => u.ImageUrl).IsRequired(false);

        SeedUser(builder);
    }

    private static void SeedUser(EntityTypeBuilder<UserEntity> builder)
    {
        var admin = new UserEntity
        {
            Id = "user-1",
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@biodomes.com",
            NormalizedEmail = "ADMIN@BIODOMES.COM",
            EmailConfirmed = true,
            Birthdate = new DateOnly(1990, 1, 1),
            SearchOrganism = "Saint Paul",
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var user = new UserEntity
        {
            Id = "user-2",
            UserName = "john",
            NormalizedUserName = "JOHN",
            Email = "john@biodomes.com",
            NormalizedEmail = "JOHN@BIODOMES.COM",
            EmailConfirmed = true,
            Birthdate = new DateOnly(1995, 6, 15),
            SearchOrganism = "Marie Thérèse",
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        builder.HasData(admin, user);
    }
}