using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class SpecieTypeEntityTypeConfiguration : IEntityTypeConfiguration<SpecieType>
{
    public void Configure(EntityTypeBuilder<SpecieType> builder)
    {
        builder.HasKey(type => type.Label);

        builder.Property(type => type.Label)
            .IsRequired();

        builder.HasData(
            new SpecieType { Label = "Mammifère" },
            new SpecieType { Label = "Oiseau" },
            new SpecieType { Label = "Poisson" },
            new SpecieType { Label = "Plante" }
        );
    }
}
