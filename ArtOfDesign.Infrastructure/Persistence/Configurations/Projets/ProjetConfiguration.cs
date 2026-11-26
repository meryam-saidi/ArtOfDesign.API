using ArtOfDesign.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ArtOfDesign.Infrastructure.Persistence.Configurations.Projets;

public class ProjetConfiguration : IEntityTypeConfiguration<Projet>
{
    public void Configure(EntityTypeBuilder<Projet> builder)
    {
        builder.ToTable("Projets");
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(d => d.Categorie)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(d => d.Nom)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(d => d.Description)
            .HasMaxLength(2000);

        builder.HasMany(d => d.Images)
            .WithOne()
            .HasForeignKey(i => i.ProjetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
