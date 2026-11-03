
using Microsoft.EntityFrameworkCore;
using ArtOfDesign.Domain.Entities.Catalogue;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtOfDesign.Infrastructure.Persistence.Configurations;

public class CouleurConfiguration : IEntityTypeConfiguration<Couleur>
{
    public void Configure(EntityTypeBuilder<Couleur> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nom)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.CodeCouleur)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.ImagePresentationUrl)
            .HasMaxLength(500);
    }
}