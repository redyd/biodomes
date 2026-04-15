using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class RoleEntityTypeConfiguration: IEntityTypeConfiguration<IdentityRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRole<string>> builder)
    {
        builder.ToTable("IdentityRoles");
        builder.HasData(new IdentityRole<string>
        {
            Id = "admin-role-id",
            Name = "Admin",
            NormalizedName = "ADMIN"
        });
    }
}