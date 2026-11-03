
using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Cuisines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtOfDesign.Infrastructure.Persistence.Configurations;

public class CuisineConfiguration : IEntityTypeConfiguration<Cuisine>
{
    public void Configure(EntityTypeBuilder<Cuisine> builder)
    {
        builder.ToTable("Cuisines");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Categorie)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Forme)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.Nom)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.PrixInitial)
            .HasColumnType("decimal(10,2)");

        builder.Property(c => c.Reduction)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(2000);

        builder.Property(c => c.Nb)
            .HasMaxLength(500);

        builder.HasOne(c => c.Dimensions)
            .WithOne()
            .HasForeignKey<DimensionsCuisine>(d => d.CuisineId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(c => c.CaracteristiquesTechniques)
            .WithOne()
            .HasForeignKey<CaracteristiquesTechniquesCuisine>(ct => ct.CuisineId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(c => c.Images)
            .WithOne()
            .HasForeignKey(i => i.CuisineId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
