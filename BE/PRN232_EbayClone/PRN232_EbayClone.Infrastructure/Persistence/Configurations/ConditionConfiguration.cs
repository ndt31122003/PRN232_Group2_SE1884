using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Conditions.Entities;
using PRN232_EbayClone.Infrastructure.Persistence.Seeds;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public sealed class ConditionConfiguration : IEntityTypeConfiguration<Condition>
{
    public void Configure(EntityTypeBuilder<Condition> builder)
    {
        builder
            .ToTable("condition");

        builder
            .HasKey(c => c.Id);

        builder.HasData(ConditionSeedData.Conditions);
    }
}
