using ArtOfDesign.Domain.Entities.Authentification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtOfDesign.Infrastructure.Persistence.Configurations.Authentification;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admins");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.HasIndex(a => a.Email)
            .IsUnique();

        builder.Property(a => a.UserName)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(a => a.PasswordHash)
            .IsRequired();
    }
}
