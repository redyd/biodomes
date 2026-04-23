using BioDomes.Infrastructures.Configurations;
using BioDomes.Infrastructures.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures;

public class BiodomeContext(DbContextOptions<BiodomeContext> options) : IdentityDbContext<UserEntity, IdentityRole, string>(options)
{
    public DbSet<SpecieEntity> Species { get; set; }
    public DbSet<EquipmentEntity> Equipments { get; set; }
    public DbSet<EquipmentResourceEntity> EquipmentResources { get; set; }
    public DbSet<SpecieClassification> SpecieClassifications { get; set; }
    public DbSet<SpecieDiet> SpecieDiets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.HasDefaultSchema("dbo");

        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
        builder.ApplyConfiguration(new RoleEntityTypeConfiguration());

        builder.ApplyConfiguration(new EquipmentResourceEntityTypeConfiguration());
        builder.ApplyConfiguration(new BiomeEntityTypeConfiguration());
        builder.ApplyConfiguration(new EquipmentEntityTypeConfiguration());
        builder.ApplyConfiguration(new SpecieClassificationTypeConfiguration());
        builder.ApplyConfiguration(new SpecieDietTypeConfiguration());
        builder.ApplyConfiguration(new SpecieEntityTypeConfiguration());
    }
}
