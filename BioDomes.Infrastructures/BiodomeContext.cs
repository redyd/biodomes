using BioDomes.Infrastructures.Configurations;
using BioDomes.Infrastructures.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures;

public class BiodomeContext(DbContextOptions<BiodomeContext> options) : IdentityDbContext<UserEntity, IdentityRole<int>, int>(options)
{
    public DbSet<SpecieEntity> Species { get; set; }
    public DbSet<EquipmentEntity> Equipments { get; set; }
    public DbSet<EquipmentResourceEntity> EquipmentResources { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.HasDefaultSchema("dbo");

        builder.ApplyConfiguration(new SpecieEntityTypeConfiguration());
        builder.ApplyConfiguration(new EquipmentResourceEntityTypeConfiguration());
        builder.ApplyConfiguration(new EquipmentEntityTypeConfiguration());
        builder.ApplyConfiguration(new BiomeEntityTypeConfiguration());
    }
}
