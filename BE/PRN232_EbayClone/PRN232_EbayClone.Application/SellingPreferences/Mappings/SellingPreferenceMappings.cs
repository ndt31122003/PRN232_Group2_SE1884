using System;
using System.Collections.Generic;
using System.Linq;
using PRN232_EbayClone.Application.SellingPreferences.Dtos;
using PRN232_EbayClone.Domain.SellingPreferences.Entities;
using PRN232_EbayClone.Domain.SellingPreferences.ValueObjects;

namespace PRN232_EbayClone.Application.SellingPreferences.Mappings;

public static class SellingPreferenceMappings
{
    public static SellerPreferenceDto ToDto(this SellerPreference? preference)
    {
        if (preference is null)
        {
            return SellerPreferenceDto.Default();
        }

        return new SellerPreferenceDto
        {
            MultiQuantity = new MultiQuantityPreferenceDto
            {
                ListingsStayActiveWhenOutOfStock = preference.ListingsStayActiveWhenOutOfStock,
                ShowExactQuantityAvailable = preference.ShowExactQuantityAvailable
            },
            Invoice = preference.InvoicePreference.ToDto(),
            BuyerManagement = preference.BuyerManagement.ToDto(),
            BlockedBuyerCount = preference.BlockedBuyers.Count,
            ExemptBuyerCount = preference.ExemptBuyers.Count,
            BuyersCanSeeVatNumber = preference.BuyersCanSeeVatNumber,
            VatNumber = preference.VatNumber
        };
    }

    public static InvoicePreferenceDto ToDto(this InvoicePreference? preference)
    {
        if (preference is null)
        {
            return InvoicePreferenceDto.Default();
        }

        return new InvoicePreferenceDto
        {
            Format = preference.Format,
            FormatName = preference.Format.ToString(),
            SendEmailCopy = preference.SendEmailCopy,
            ApplyCreditsAutomatically = preference.ApplyCreditsAutomatically
        };
    }

    public static BuyerManagementDto ToDto(this BuyerManagementSettings? settings)
    {
        if (settings is null)
        {
            return BuyerManagementDto.Default();
        }

        return new BuyerManagementDto
        {
            BlockSettings = new BuyerManagementBlockSettingsDto
            {
                BlockUnpaidItemStrikes = settings.BlockUnpaidItemStrikes,
                UnpaidItemStrikesCount = settings.UnpaidItemStrikesCount,
                UnpaidItemStrikesPeriodInMonths = settings.UnpaidItemStrikesPeriodInMonths,
                BlockPrimaryAddressOutsideShippingLocation = settings.BlockPrimaryAddressOutsideShippingLocation,
                BlockMaxItemsInLastTenDays = settings.BlockMaxItemsInLastTenDays,
                MaxItemsInLastTenDays = settings.MaxItemsInLastTenDays,
                ApplyFeedbackScoreThreshold = settings.ApplyFeedbackScoreThreshold,
                FeedbackScoreThreshold = settings.FeedbackScoreThreshold,
                UpdateBlockSettingsForActiveListings = settings.UpdateBlockSettingsForActiveListings
            },
            Rules = new BuyerManagementRulesDto
            {
                RequirePaymentMethodBeforeBid = settings.RequirePaymentMethodBeforeBid,
                RequirePaymentMethodBeforeOffer = settings.RequirePaymentMethodBeforeOffer,
                PreventBlockedBuyersFromContacting = settings.PreventBlockedBuyersFromContacting
            }
        };
    }

    public static BuyerListDto ToBlockedBuyerListDto(this SellerPreference? preference)
    {
        if (preference is null)
        {
            return BuyerListDto.EmptyList();
        }

        var items = preference.BlockedBuyers.Select(b => b.Identifier).ToList();
        return new BuyerListDto
        {
            Items = items,
            LastUpdatedAtUtc = preference.UpdatedAt ?? preference.CreatedAt
        };
    }

    public static BuyerListDto ToExemptBuyerListDto(this SellerPreference? preference)
    {
        if (preference is null)
        {
            return BuyerListDto.EmptyList();
        }

        var items = preference.ExemptBuyers.Select(b => b.Identifier).ToList();
        return new BuyerListDto
        {
            Items = items,
            LastUpdatedAtUtc = preference.UpdatedAt ?? preference.CreatedAt
        };
    }
}
