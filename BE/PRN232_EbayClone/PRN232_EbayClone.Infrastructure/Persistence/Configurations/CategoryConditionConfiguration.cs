using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Categories.Entities;
using PRN232_EbayClone.Domain.Conditions.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class CategoryConditionConfiguration : IEntityTypeConfiguration<CategoryCondition>
{
    public void Configure(EntityTypeBuilder<CategoryCondition> builder)
    {
        builder
            .ToTable("category_condition");

        builder
            .HasKey(cc => new { cc.CategoryId, cc.ConditionId });

        builder
            .HasOne(cc => cc.Condition)
            .WithMany()
            .HasForeignKey(cc => cc.ConditionId);

        builder.HasData(CategorySeedData.CategoryConditions);
    }
}
