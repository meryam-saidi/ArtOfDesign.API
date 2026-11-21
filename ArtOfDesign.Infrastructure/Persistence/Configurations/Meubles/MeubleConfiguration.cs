using ArtOfDesign.Domain.Entities;
using ArtOfDesign.Domain.Entities.Meubles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ArtOfDesign.Infrastructure.Persistence.Configurations.Meubles;

public class MeubleConfiguration : IEntityTypeConfiguration<Meuble>
{
    public void Configure(EntityTypeBuilder<Meuble> builder)
    {
        builder.ToTable("Meubles");
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
        builder.Property(d => d.Nom)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(d => d.PrixInitial)
            .HasColumnType("decimal(10,3)");
        builder.Property(d => d.Reduction);
        builder.Property(d => d.Description)
            .HasMaxLength(2000);

        builder.HasOne(d => d.Dimensions)
            .WithOne()
            .HasForeignKey<DimensionsMeuble>(dm => dm.MeubleId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(d => d.CaracteristiquesTechniques)
            .WithOne()
            .HasForeignKey<CaracteristiquesTechniquesMeuble>(ct => ct.MeubleId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(d => d.Images)
            .WithOne()
            .HasForeignKey(i => i.MeubleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
