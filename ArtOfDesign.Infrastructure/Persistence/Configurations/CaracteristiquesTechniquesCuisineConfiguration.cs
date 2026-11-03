
using ArtOfDesign.Domain.Entities.Cuisines;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
            .HasColumnType("nvarchar(max)")
            .Metadata.SetValueComparer(
                new ValueComparer<List<string>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(
                        0,
                        (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()
                )
            );

        builder.Property(c => c.Caisson).HasMaxLength(500);
        builder.Property(c => c.Facade).HasMaxLength(500);
    }
}
