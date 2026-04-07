using BioDomes.Infrastructures.Configurations;
using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures;

public class BiodomeContext(DbContextOptions<BiodomeContext> options) : DbContext(options)
{
    public DbSet<SpecieEntity> Species { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("dbo");

        builder.ApplyConfiguration(new SpecieEntityTypeConfiguration());
        builder.ApplyConfiguration(new EquipmentResourceEntityTypeConfiguration());
        builder.ApplyConfiguration(new EquipmentEntityTypeConfiguration());
    }
}
