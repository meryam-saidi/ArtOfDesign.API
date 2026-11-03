using ArtOfDesign.Domain.Entities.Cuisines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtOfDesign.Infrastructure.Persistence.Configurations;

public class CuisineCouleurConfiguration : IEntityTypeConfiguration<CuisineCouleur>
{
    public void Configure(EntityTypeBuilder<CuisineCouleur> builder)
    {
        builder.HasKey(cc => new { cc.CuisineId, cc.CouleurId });

        builder.HasOne(cc => cc.Cuisine)
            .WithMany(c => c.CuisineCouleurs)
            .HasForeignKey(cc => cc.CuisineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cc => cc.Couleur)
            .WithMany()
            .HasForeignKey(cc => cc.CouleurId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}