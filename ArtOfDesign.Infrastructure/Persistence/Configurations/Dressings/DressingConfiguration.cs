
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Cuisines;
using ArtOfDesign.Domain.Entities.Dressings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtOfDesign.Infrastructure.Persistence.Configurations.Dressings;

public class DressingConfiguration : IEntityTypeConfiguration<Dressing>
{
    public void Configure(EntityTypeBuilder<Dressing> builder)
    {
        builder.ToTable("Dressings");
        builder.HasKey(d => d.Id);
        
        builder.Property(d => d.Id)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(d => d.Categorie)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(d => d.Forme)
            .HasMaxLength(20)
            .IsRequired(); 
        builder.Property(d => d.TypeOuverture)
            .HasMaxLength(20)
            .IsRequired();
        builder.Property(d => d.Nom)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(d => d.PrixInitial)
            .HasColumnType("decimal(10,2)");
        builder.Property(d => d.Reduction)
            .IsRequired();
        builder.Property(d => d.Description)
            .HasMaxLength(2000);

        builder.HasOne(d => d.Dimensions)
            .WithOne()
            .HasForeignKey<DimensionsDressing>(dm => dm.DressingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(d => d.CaracteristiquesTechniques)
            .WithOne()
            .HasForeignKey<CaracteristiquesTechniquesDressing>(ct => ct.DressingId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(d => d.Images)
            .WithOne()
            .HasForeignKey(i => i.DressingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}