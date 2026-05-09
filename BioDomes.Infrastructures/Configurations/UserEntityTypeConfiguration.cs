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
            ImageUrl = "1637ca51-6139-4ec4-aa5a-3e824212b0e3.jpg",
            Email = "admin@biodomes.com",
            NormalizedEmail = "ADMIN@BIODOMES.COM",
            EmailConfirmed = true,
            Birthdate = new DateOnly(1990, 1, 1),
            SearchOrganism = "Saint Paul",
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        var user1 = new UserEntity
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

        var user2 = new UserEntity
        {
            Id = "user-3",
            UserName = "bob",
            NormalizedUserName = "BOB",
            Email = "bob@biodomes.com",
            NormalizedEmail = "BOB@BIODOMES.COM",
            EmailConfirmed = true,
            Birthdate = new DateOnly(1995, 6, 15),
            SearchOrganism = "Marie Thérèse",
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        builder.HasData(admin, user1, user2);
    }
}