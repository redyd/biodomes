using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;

namespace BioDomes.Infrastructures.Configurations;

public class BiodomeContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Specie> Species { get; set; }
    public DbSet<SpecieType> SpeciesTypes { get; set; }
    public DbSet<SpecieDiet> SpeciesDiets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new SpecieEntityTypeConfiguration());
        builder.ApplyConfiguration(new SpecieTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new SpecieDietEntityTypeConfiguration());
    }
}
