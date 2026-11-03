
using ArtOfDesign.Domain.Entities.Cuisines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace ArtOfDesign.Infrastructure.Persistence.Configurations;

public class CaracteristiquesTechniquesCuisineConfiguration
    : IEntityTypeConfiguration<CaracteristiquesTechniquesCuisine>
{
    public void Configure(
        EntityTypeBuilder<CaracteristiquesTechniquesCuisine> builder)
    {
        builder.ToTable("CaracteristiquesTechniquesCuisines");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompositionType)
            .HasConversion(
                value => JsonSerializer.Serialize(value, (JsonSerializerOptions?)null),
                value => JsonSerializer.Deserialize<List<string>>(
                    value, (JsonSerializerOptions?)null) ?? new List<string>())
            .HasColumnType("nvarchar(max)");

        builder.Property(c => c.Caisson).HasMaxLength(500);
        builder.Property(c => c.Facade).HasMaxLength(500);
    }
}
