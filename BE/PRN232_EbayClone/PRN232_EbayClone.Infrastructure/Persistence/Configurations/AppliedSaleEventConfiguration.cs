using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

internal sealed class AppliedSaleEventConfiguration : IEntityTypeConfiguration<AppliedSaleEvent>
{
    public void Configure(EntityTypeBuilder<AppliedSaleEvent> builder)
    {
        builder.ToTable("applied_sale_events");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.OrderId)
            .IsRequired();

        builder.Property(a => a.SaleEventId)
            .IsRequired();

        builder.Property(a => a.DiscountTierId);

        builder.Property(a => a.DiscountAmount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(a => a.AppliedAt)
            .IsRequired();

        builder.HasIndex(a => a.OrderId);
        builder.HasIndex(a => a.SaleEventId);
    }
}
