using BioDomes.Infrastructures.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BioDomes.Infrastructures.Configurations;

public class EquipmentEntityTypeConfiguration : IEntityTypeConfiguration<EquipmentEntity>
{
    public void Configure(EntityTypeBuilder<EquipmentEntity> builder)
    {
        builder.ToTable("equipments");
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(e => e.Image)
            .IsRequired();
        
        builder.HasOne(e => e.ProducedResourceEntity)
            .WithMany()
            .HasForeignKey(e => e.ProducedResourceId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(e => e.ConsumedResourceEntity)
            .WithMany()
            .HasForeignKey(e => e.ConsumedResourceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}