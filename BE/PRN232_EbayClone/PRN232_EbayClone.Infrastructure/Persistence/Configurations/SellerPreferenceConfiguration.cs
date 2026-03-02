using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.SellingPreferences.Entities;
using PRN232_EbayClone.Domain.SellingPreferences.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class SellerPreferenceConfiguration : IEntityTypeConfiguration<SellerPreference>
{
    public void Configure(EntityTypeBuilder<SellerPreference> builder)
    {
        builder.ToTable("seller_preference");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.SellerId)
            .HasColumnName("seller_id")
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();

        builder.HasIndex(x => x.SellerId)
            .IsUnique()
            .HasDatabaseName("ux_seller_preference_seller");

        builder.Property(x => x.ListingsStayActiveWhenOutOfStock)
            .HasColumnName("listings_stay_active_when_out_of_stock")
            .HasDefaultValue(false);

        builder.Property(x => x.ShowExactQuantityAvailable)
            .HasColumnName("show_exact_quantity_available")
            .HasDefaultValue(true);

        builder.Property(x => x.BuyersCanSeeVatNumber)
            .HasColumnName("buyers_can_see_vat_number")
            .HasDefaultValue(false);

        builder.Property(x => x.VatNumber)
            .HasColumnName("vat_number")
            .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
            .IsRequired(false);

        builder.OwnsOne(x => x.InvoicePreference, invoice =>
        {
            invoice.Property(p => p.Format)
                .HasColumnName("invoice_format")
                .IsRequired();

            invoice.Property(p => p.SendEmailCopy)
                .HasColumnName("invoice_send_email_copy")
                .HasDefaultValue(true);

            invoice.Property(p => p.ApplyCreditsAutomatically)
                .HasColumnName("invoice_apply_credits_automatically")
                .HasDefaultValue(true);
        });

        builder.OwnsOne(x => x.BuyerManagement, buyer =>
        {
            buyer.Property(p => p.BlockUnpaidItemStrikes)
                .HasColumnName("block_unpaid_item_strikes")
                .HasDefaultValue(false);

            buyer.Property(p => p.UnpaidItemStrikesCount)
                .HasColumnName("unpaid_item_strikes_count")
                .HasDefaultValue(0);

            buyer.Property(p => p.UnpaidItemStrikesPeriodInMonths)
                .HasColumnName("unpaid_item_strikes_period_months")
                .HasDefaultValue(0);

            buyer.Property(p => p.BlockPrimaryAddressOutsideShippingLocation)
                .HasColumnName("block_primary_address_outside_shipping_location")
                .HasDefaultValue(true);

            buyer.Property(p => p.BlockMaxItemsInLastTenDays)
                .HasColumnName("block_max_items_last_ten_days")
                .HasDefaultValue(false);

            buyer.Property(p => p.MaxItemsInLastTenDays)
                .HasColumnName("max_items_last_ten_days")
                .IsRequired(false);

            buyer.Property(p => p.ApplyFeedbackScoreThreshold)
                .HasColumnName("apply_feedback_score_threshold")
                .HasDefaultValue(false);

            buyer.Property(p => p.FeedbackScoreThreshold)
                .HasColumnName("feedback_score_threshold")
                .IsRequired(false);

            buyer.Property(p => p.UpdateBlockSettingsForActiveListings)
                .HasColumnName("update_block_settings_active_listings")
                .HasDefaultValue(false);

            buyer.Property(p => p.RequirePaymentMethodBeforeBid)
                .HasColumnName("require_payment_method_before_bid")
                .HasDefaultValue(true);

            buyer.Property(p => p.RequirePaymentMethodBeforeOffer)
                .HasColumnName("require_payment_method_before_offer")
                .HasDefaultValue(true);

            buyer.Property(p => p.PreventBlockedBuyersFromContacting)
                .HasColumnName("prevent_blocked_buyers_contacting")
                .HasDefaultValue(true);
        });

        builder.OwnsMany(x => x.BlockedBuyers, blocked =>
        {
            blocked.ToTable("seller_blocked_buyer");
            blocked.WithOwner().HasForeignKey("seller_preference_id");
            blocked.HasKey("Id");

            blocked.Property(b => b.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            blocked.Property(b => b.Identifier)
                .HasColumnName("identifier")
                .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
                .IsRequired();

            blocked.Property(b => b.NormalizedIdentifier)
                .HasColumnName("normalized_identifier")
                .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
                .IsRequired();

            blocked.Property(b => b.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .HasDefaultValueSql("now() at time zone 'utc'");

            // create a unique index on (NormalizedIdentifier, seller_preference_id)
            // use string overload so we can reference the shadow FK column created by WithOwner().HasForeignKey("seller_preference_id")
            blocked.HasIndex(new[] { nameof(BlockedBuyer.NormalizedIdentifier), "seller_preference_id" })
                .IsUnique()
                .HasDatabaseName("ux_seller_blocked_buyer_identifier");
        });

        builder.OwnsMany(x => x.ExemptBuyers, exempt =>
        {
            exempt.ToTable("seller_exempt_buyer");
            exempt.WithOwner().HasForeignKey("seller_preference_id");
            exempt.HasKey("Id");

            exempt.Property(b => b.Id)
                .HasColumnName("id")
                .ValueGeneratedNever();

            exempt.Property(b => b.Identifier)
                .HasColumnName("identifier")
                .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
                .IsRequired();

            exempt.Property(b => b.NormalizedIdentifier)
                .HasColumnName("normalized_identifier")
                .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
                .IsRequired();

            exempt.Property(b => b.CreatedAtUtc)
                .HasColumnName("created_at_utc")
                .HasDefaultValueSql("now() at time zone 'utc'");

            // create a unique index on (NormalizedIdentifier, seller_preference_id)
            // use string overload to reference the shadow FK column
            exempt.HasIndex(new[] { nameof(ExemptBuyer.NormalizedIdentifier), "seller_preference_id" })
                .IsUnique()
                .HasDatabaseName("ux_seller_exempt_buyer_identifier");
        });

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}
