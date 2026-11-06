using ArtOfDesign.Domain.Entities.Dressings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArtOfDesign.Infrastructure.Persistence.Configurations.Dressings;

public class CaracteristiquesTechniquesDressingConfiguration :
    IEntityTypeConfiguration<CaracteristiquesTechniquesDressing>
{
    public void Configure(EntityTypeBuilder<CaracteristiquesTechniquesDressing> builder)
    {
        builder.ToTable("CaracteristiquesTechniquesDressings");
        builder.HasKey(ct => ct.Id);
        builder.Property(ct => ct.CompositionType)
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
