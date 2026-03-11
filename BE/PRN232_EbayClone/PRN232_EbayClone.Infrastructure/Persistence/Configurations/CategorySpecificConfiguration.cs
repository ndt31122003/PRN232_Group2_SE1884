using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class CategorySpecificConfiguration : IEntityTypeConfiguration<CategorySpecific>
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.General);

    public void Configure(EntityTypeBuilder<CategorySpecific> builder)
    {
        builder
            .ToTable("category_specific");

        builder
            .HasKey(cs => cs.Id);

        builder
            .Property(cs => cs.Name)
            .IsRequired();

        builder
            .Property(cs => cs.IsRequired)
            .HasColumnName("is_required");

        builder
            .Property(cs => cs.AllowMultiple)
            .HasColumnName("allow_multiple");

        var comparer = new ValueComparer<HashSet<string>>(
            (left, right) => CompareValues(left, right),
            values => ComputeHash(values),
            values => Clone(values));

        builder
            .Property<HashSet<string>>("_values")
            .HasColumnName("values")
            .HasColumnType("jsonb")
            .HasConversion(
                values => JsonSerializer.Serialize(values ?? new HashSet<string>(StringComparer.Ordinal), SerializerOptions),
                json => string.IsNullOrWhiteSpace(json)
                    ? new HashSet<string>(StringComparer.Ordinal)
                    : JsonSerializer.Deserialize<HashSet<string>>(json, SerializerOptions)
                        ?? new HashSet<string>(StringComparer.Ordinal))
            .Metadata.SetValueComparer(comparer);

        builder.HasData(CategorySeedData.CategorySpecifics);
    }

    private static bool CompareValues(HashSet<string>? left, HashSet<string>? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.SetEquals(right);
    }

    private static int ComputeHash(HashSet<string>? values)
    {
        if (values is null || values.Count == 0) return 0;
        var aggregate = 0;
        foreach (var value in values)
        {
            aggregate = HashCode.Combine(aggregate, StringComparer.Ordinal.GetHashCode(value));
        }
        return aggregate;
    }

    private static HashSet<string> Clone(HashSet<string>? values)
    {
        return values is null
            ? new HashSet<string>(StringComparer.Ordinal)
            : values.ToHashSet(StringComparer.Ordinal);
    }
}
