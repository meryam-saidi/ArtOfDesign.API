using ArtOfDesign.Domain.Entities.Dressings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtOfDesign.Infrastructure.Persistence.Configurations.Dressings;

public class DressingCouleurConfiguration : IEntityTypeConfiguration<DressingCouleur>
{
    public void Configure(EntityTypeBuilder<DressingCouleur> builder)
    {
        builder.HasKey(cc => new { cc.DressingId, cc.CouleurId });

        builder.HasOne(cc => cc.Dressing)
            .WithMany(c => c.DressingCouleurs)
            .HasForeignKey(cc => cc.DressingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cc => cc.Couleur)
            .WithMany()
            .HasForeignKey(cc => cc.CouleurId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
