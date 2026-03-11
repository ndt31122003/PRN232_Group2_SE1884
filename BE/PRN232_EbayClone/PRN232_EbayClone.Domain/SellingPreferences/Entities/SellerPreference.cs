using System;
using System.Collections.Generic;
using PRN232_EbayClone.Domain.SellingPreferences.Enums;
using PRN232_EbayClone.Domain.SellingPreferences.Errors;
using PRN232_EbayClone.Domain.SellingPreferences.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.SellingPreferences.Entities;

public sealed class SellerPreference : AggregateRoot<Guid>
{
    public const int MaxBlockedBuyerCount = 5000;
    public const int MaxExemptBuyerCount = 1000;
    public const int MaxBuyerIdentifierLength = 200;

    private readonly List<BlockedBuyer> _blockedBuyers = [];
    private readonly List<ExemptBuyer> _exemptBuyers = [];

    private SellerPreference(Guid id) : base(id) { }

    public UserId SellerId { get; private set; }
    public bool ListingsStayActiveWhenOutOfStock { get; private set; }
    public bool ShowExactQuantityAvailable { get; private set; }
    public bool BuyersCanSeeVatNumber { get; private set; }
    public string? VatNumber { get; private set; }

    public BuyerManagementSettings BuyerManagement { get; private set; } = BuyerManagementSettings.CreateDefault();
    public InvoicePreference InvoicePreference { get; private set; } = InvoicePreference.CreateDefault();

    public IReadOnlyCollection<BlockedBuyer> BlockedBuyers => _blockedBuyers.AsReadOnly();
    public IReadOnlyCollection<ExemptBuyer> ExemptBuyers => _exemptBuyers.AsReadOnly();

    public static SellerPreference CreateDefault(UserId sellerId)
    {
        return new SellerPreference(Guid.NewGuid())
        {
            SellerId = sellerId,
            ListingsStayActiveWhenOutOfStock = false,
            ShowExactQuantityAvailable = true,
            BuyersCanSeeVatNumber = false,
            VatNumber = null,
            BuyerManagement = BuyerManagementSettings.CreateDefault(),
            InvoicePreference = InvoicePreference.CreateDefault()
        };
    }

    public Result UpdateMultiQuantitySettings(bool listingsStayActiveWhenOutOfStock, bool showExactQuantityAvailable)
    {
        ListingsStayActiveWhenOutOfStock = listingsStayActiveWhenOutOfStock;
        ShowExactQuantityAvailable = showExactQuantityAvailable;
        return Result.Success();
    }

    public Result UpdateInvoicePreferences(InvoiceFormat format, bool sendEmailCopy, bool applyCreditsAutomatically)
    {
        EnsureInvoicePreferenceInitialized();
        return InvoicePreference.Update(format, sendEmailCopy, applyCreditsAutomatically);
    }

    public Result UpdateBuyerManagement(
        bool blockUnpaidItemStrikes,
        int unpaidItemStrikesCount,
        int unpaidItemStrikesPeriodInMonths,
        bool blockPrimaryAddressOutsideShippingLocation,
        bool blockMaxItemsInLastTenDays,
        int? maxItemsInLastTenDays,
        bool applyFeedbackScoreThreshold,
        int? feedbackScoreThreshold,
        bool updateBlockSettingsForActiveListings,
        bool requirePaymentMethodBeforeBid,
        bool requirePaymentMethodBeforeOffer,
        bool preventBlockedBuyersFromContacting)
    {
        EnsureBuyerManagementInitialized();
        return BuyerManagement.Update(
            blockUnpaidItemStrikes,
            unpaidItemStrikesCount,
            unpaidItemStrikesPeriodInMonths,
            blockPrimaryAddressOutsideShippingLocation,
            blockMaxItemsInLastTenDays,
            maxItemsInLastTenDays,
            applyFeedbackScoreThreshold,
            feedbackScoreThreshold,
            updateBlockSettingsForActiveListings,
            requirePaymentMethodBeforeBid,
            requirePaymentMethodBeforeOffer,
            preventBlockedBuyersFromContacting);
    }

    public Result ReplaceBlockedBuyers(IEnumerable<string> identifiers)
    {
        EnsureBuyerManagementInitialized();
        var normalized = NormalizeIdentifiers(identifiers, MaxBlockedBuyerCount, SellerPreferenceErrors.BlockedBuyerIdentifierRequired, SellerPreferenceErrors.BlockedBuyerIdentifierTooLong, SellerPreferenceErrors.BlockedBuyerLimitExceeded);
        if (normalized.IsFailure)
        {
            return normalized.Error;
        }

        var values = normalized.Value;
        if (values.HasDuplicates)
        {
            return SellerPreferenceErrors.DuplicateBlockedBuyer;
        }

        _blockedBuyers.Clear();
        foreach (var value in values.Items)
        {
            _blockedBuyers.Add(BlockedBuyer.Create(value));
        }

        return Result.Success();
    }

    public Result ReplaceExemptBuyers(IEnumerable<string> identifiers)
    {
        var normalized = NormalizeIdentifiers(identifiers, MaxExemptBuyerCount, SellerPreferenceErrors.ExemptBuyerIdentifierRequired, SellerPreferenceErrors.ExemptBuyerIdentifierTooLong, SellerPreferenceErrors.ExemptBuyerLimitExceeded);
        if (normalized.IsFailure)
        {
            return normalized.Error;
        }

        var values = normalized.Value;
        if (values.HasDuplicates)
        {
            return SellerPreferenceErrors.DuplicateExemptBuyer;
        }

        _exemptBuyers.Clear();
        foreach (var value in values.Items)
        {
            _exemptBuyers.Add(ExemptBuyer.Create(value));
        }

        return Result.Success();
    }

    public void UpdateVatSettings(bool buyersCanSeeVatNumber, string? vatNumber)
    {
        BuyersCanSeeVatNumber = buyersCanSeeVatNumber;
        VatNumber = string.IsNullOrWhiteSpace(vatNumber) ? null : vatNumber.Trim();
    }

    private void EnsureBuyerManagementInitialized()
    {
        BuyerManagement ??= BuyerManagementSettings.CreateDefault();
    }

    private void EnsureInvoicePreferenceInitialized()
    {
        InvoicePreference ??= InvoicePreference.CreateDefault();
    }

    private static Result<(IReadOnlyCollection<string> Items, bool HasDuplicates)> NormalizeIdentifiers(
        IEnumerable<string> identifiers,
        int maxCount,
        Error emptyError,
        Error lengthError,
        Error limitError)
    {
        if (identifiers is null)
        {
            return Result.Success<(IReadOnlyCollection<string>, bool)>((Array.Empty<string>(), false));
        }

        var comparer = StringComparer.OrdinalIgnoreCase;
        var seen = new HashSet<string>(comparer);
        var normalizedList = new List<string>();
        var hasDuplicate = false;

        foreach (var raw in identifiers)
        {
            if (string.IsNullOrWhiteSpace(raw))
            {
                return emptyError;
            }

            var trimmed = raw.Trim();
            if (trimmed.Length > MaxBuyerIdentifierLength)
            {
                return lengthError;
            }

            if (!seen.Add(trimmed))
            {
                hasDuplicate = true;
            }

            normalizedList.Add(trimmed);
            if (normalizedList.Count > maxCount)
            {
                return limitError;
            }
        }

        return Result.Success<(IReadOnlyCollection<string>, bool)>((normalizedList, hasDuplicate));
    }
}
