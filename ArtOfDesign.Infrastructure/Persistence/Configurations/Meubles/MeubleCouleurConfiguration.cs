using ArtOfDesign.Domain.Entities.Meubles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ArtOfDesign.Infrastructure.Persistence.Configurations.Meubles;

public class MeubleCouleurConfiguration : IEntityTypeConfiguration<MeubleCouleur>
{
    public void Configure(EntityTypeBuilder<MeubleCouleur> builder)
    {
        builder.HasKey(cc => new { cc.MeubleId, cc.CouleurId });

        builder.HasOne(cc => cc.Meuble)
            .WithMany(c => c.MeubleCouleurs)
            .HasForeignKey(cc => cc.MeubleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cc => cc.Couleur)
            .WithMany()
            .HasForeignKey(cc => cc.CouleurId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
