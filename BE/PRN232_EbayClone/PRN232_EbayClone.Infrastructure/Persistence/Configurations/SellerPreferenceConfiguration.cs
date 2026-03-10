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
            
            .HasConversion(id => id.Value, value => new UserId(value))
            .IsRequired();

        builder.HasIndex(x => x.SellerId)
            .IsUnique()
            .HasDatabaseName("ux_seller_preference_seller");

        builder.Property(x => x.ListingsStayActiveWhenOutOfStock)
            
            .HasDefaultValue(false);

        builder.Property(x => x.ShowExactQuantityAvailable)
            
            .HasDefaultValue(true);

        builder.Property(x => x.BuyersCanSeeVatNumber)
            
            .HasDefaultValue(false);

        builder.Property(x => x.VatNumber)
            
            .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
            .IsRequired(false);

        builder.OwnsOne(x => x.InvoicePreference, invoice =>
        {
            invoice.Property(p => p.Format)
                
                .IsRequired();

            invoice.Property(p => p.SendEmailCopy)
                
                .HasDefaultValue(true);

            invoice.Property(p => p.ApplyCreditsAutomatically)
                
                .HasDefaultValue(true);
        });

        builder.OwnsOne(x => x.BuyerManagement, buyer =>
        {
            buyer.Property(p => p.BlockUnpaidItemStrikes)
                
                .HasDefaultValue(false);

            buyer.Property(p => p.UnpaidItemStrikesCount)
                
                .HasDefaultValue(0);

            buyer.Property(p => p.UnpaidItemStrikesPeriodInMonths)
                
                .HasDefaultValue(0);

            buyer.Property(p => p.BlockPrimaryAddressOutsideShippingLocation)
                
                .HasDefaultValue(true);

            buyer.Property(p => p.BlockMaxItemsInLastTenDays)
                
                .HasDefaultValue(false);

            buyer.Property(p => p.MaxItemsInLastTenDays)
                
                .IsRequired(false);

            buyer.Property(p => p.ApplyFeedbackScoreThreshold)
                
                .HasDefaultValue(false);

            buyer.Property(p => p.FeedbackScoreThreshold)
                
                .IsRequired(false);

            buyer.Property(p => p.UpdateBlockSettingsForActiveListings)
                
                .HasDefaultValue(false);

            buyer.Property(p => p.RequirePaymentMethodBeforeBid)
                
                .HasDefaultValue(true);

            buyer.Property(p => p.RequirePaymentMethodBeforeOffer)
                
                .HasDefaultValue(true);

            buyer.Property(p => p.PreventBlockedBuyersFromContacting)
                
                .HasDefaultValue(true);
        });

        builder.OwnsMany(x => x.BlockedBuyers, blocked =>
        {
            blocked.ToTable("seller_blocked_buyer");
            blocked.WithOwner().HasForeignKey("seller_preference_id");
            blocked.HasKey("Id");

            blocked.Property(b => b.Id)
                
                .ValueGeneratedNever();

            blocked.Property(b => b.Identifier)
                
                .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
                .IsRequired();

            blocked.Property(b => b.NormalizedIdentifier)
                
                .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
                .IsRequired();

            blocked.Property(b => b.CreatedAtUtc)
                
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
                
                .ValueGeneratedNever();

            exempt.Property(b => b.Identifier)
                
                .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
                .IsRequired();

            exempt.Property(b => b.NormalizedIdentifier)
                
                .HasMaxLength(SellerPreference.MaxBuyerIdentifierLength)
                .IsRequired();

            exempt.Property(b => b.CreatedAtUtc)
                
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
