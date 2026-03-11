using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder
            .ToTable("category");

        builder
            .HasKey(c => c.Id);

        // Parent - Children self-reference
        builder
            .HasMany(c => c.Children)
            .WithOne()
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Navigation(c => c.Children)
            .UsePropertyAccessMode(PropertyAccessMode.Field);


        // CategorySpecific
        builder
            .HasMany(c => c.CategorySpecifics)
            .WithOne()
            .HasForeignKey("category_id");

        // CategoryCondition
        builder
            .HasMany(c => c.CategoryConditions)
            .WithOne()
            .HasForeignKey(cc => cc.CategoryId);

        // Soft delete filter
        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasData(CategorySeedData.Categories);
    }
}
