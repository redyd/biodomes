using BioDomes.Infrastructures.Models;
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

    }

    private static void SeedUser(EntityTypeBuilder<UserEntity> builder)
    {
        
    }
}