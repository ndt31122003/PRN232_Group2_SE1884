using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PRN232_EbayClone.Domain.SellingPreferences.Enums;

namespace PRN232_EbayClone.Application.SellingPreferences.Dtos;

public sealed class SellerPreferenceDto
{
    public MultiQuantityPreferenceDto MultiQuantity { get; init; } = MultiQuantityPreferenceDto.Default();
    public InvoicePreferenceDto Invoice { get; init; } = InvoicePreferenceDto.Default();
    public BuyerManagementDto BuyerManagement { get; init; } = BuyerManagementDto.Default();
    public int BlockedBuyerCount { get; init; }
    public int ExemptBuyerCount { get; init; }
    public bool BuyersCanSeeVatNumber { get; init; }
    public string? VatNumber { get; init; }

    public static SellerPreferenceDto Default() => new()
    {
        MultiQuantity = MultiQuantityPreferenceDto.Default(),
        Invoice = InvoicePreferenceDto.Default(),
        BuyerManagement = BuyerManagementDto.Default(),
        BlockedBuyerCount = 0,
        ExemptBuyerCount = 0,
        BuyersCanSeeVatNumber = false,
        VatNumber = null
    };
}

public sealed class MultiQuantityPreferenceDto
{
    public bool ListingsStayActiveWhenOutOfStock { get; init; }
    public bool ShowExactQuantityAvailable { get; init; }

    public static MultiQuantityPreferenceDto Default() => new()
    {
        ListingsStayActiveWhenOutOfStock = false,
        ShowExactQuantityAvailable = true
    };
}

public sealed class InvoicePreferenceDto
{
    public InvoiceFormat Format { get; init; }
    public string FormatName { get; init; } = InvoiceFormat.Detailed.ToString();
    public bool SendEmailCopy { get; init; }
    public bool ApplyCreditsAutomatically { get; init; }

    public static InvoicePreferenceDto Default() => new()
    {
        Format = InvoiceFormat.Detailed,
        FormatName = InvoiceFormat.Detailed.ToString(),
        SendEmailCopy = true,
        ApplyCreditsAutomatically = true
    };
}

public sealed class BuyerManagementDto
{
    public BuyerManagementBlockSettingsDto BlockSettings { get; init; } = BuyerManagementBlockSettingsDto.Default();
    public BuyerManagementRulesDto Rules { get; init; } = BuyerManagementRulesDto.Default();

    public static BuyerManagementDto Default() => new()
    {
        BlockSettings = BuyerManagementBlockSettingsDto.Default(),
        Rules = BuyerManagementRulesDto.Default()
    };
}

public sealed class BuyerManagementBlockSettingsDto
{
    public bool BlockUnpaidItemStrikes { get; init; }
    public int UnpaidItemStrikesCount { get; init; }
    public int UnpaidItemStrikesPeriodInMonths { get; init; }
    public bool BlockPrimaryAddressOutsideShippingLocation { get; init; }
    public bool BlockMaxItemsInLastTenDays { get; init; }
    public int? MaxItemsInLastTenDays { get; init; }
    public bool ApplyFeedbackScoreThreshold { get; init; }
    public int? FeedbackScoreThreshold { get; init; }
    public bool UpdateBlockSettingsForActiveListings { get; init; }

    public static BuyerManagementBlockSettingsDto Default() => new()
    {
        BlockUnpaidItemStrikes = false,
        UnpaidItemStrikesCount = 2,
        UnpaidItemStrikesPeriodInMonths = 1,
        BlockPrimaryAddressOutsideShippingLocation = true,
        BlockMaxItemsInLastTenDays = false,
        MaxItemsInLastTenDays = null,
        ApplyFeedbackScoreThreshold = false,
        FeedbackScoreThreshold = null,
        UpdateBlockSettingsForActiveListings = false
    };
}

public sealed class BuyerManagementRulesDto
{
    public bool RequirePaymentMethodBeforeBid { get; init; }
    public bool RequirePaymentMethodBeforeOffer { get; init; }
    public bool PreventBlockedBuyersFromContacting { get; init; }

    public static BuyerManagementRulesDto Default() => new()
    {
        RequirePaymentMethodBeforeBid = true,
        RequirePaymentMethodBeforeOffer = true,
        PreventBlockedBuyersFromContacting = true
    };
}

public sealed class BuyerListDto
{
    private static readonly IReadOnlyList<string> Empty = new ReadOnlyCollection<string>(Array.Empty<string>());

    public IReadOnlyList<string> Items { get; init; } = Empty;
    public DateTime? LastUpdatedAtUtc { get; init; }

    public static BuyerListDto EmptyList() => new()
    {
        Items = Empty,
        LastUpdatedAtUtc = null
    };
}
