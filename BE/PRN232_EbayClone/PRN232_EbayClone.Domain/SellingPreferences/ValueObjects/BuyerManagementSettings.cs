using System;
using System.Collections.Generic;
using PRN232_EbayClone.Domain.SellingPreferences.Errors;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.SellingPreferences.ValueObjects;

public sealed class BuyerManagementSettings
{
    private const int MaxMaxItemsThreshold = 25;
    private const int MaxFeedbackScore = 1000;

    public bool BlockUnpaidItemStrikes { get; private set; }
    public int UnpaidItemStrikesCount { get; private set; }
    public int UnpaidItemStrikesPeriodInMonths { get; private set; }
    public bool BlockPrimaryAddressOutsideShippingLocation { get; private set; }
    public bool BlockMaxItemsInLastTenDays { get; private set; }
    public int? MaxItemsInLastTenDays { get; private set; }
    public bool ApplyFeedbackScoreThreshold { get; private set; }
    public int? FeedbackScoreThreshold { get; private set; }
    public bool UpdateBlockSettingsForActiveListings { get; private set; }
    public bool RequirePaymentMethodBeforeBid { get; private set; }
    public bool RequirePaymentMethodBeforeOffer { get; private set; }
    public bool PreventBlockedBuyersFromContacting { get; private set; }

    private BuyerManagementSettings() { }

    private BuyerManagementSettings(
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
        BlockUnpaidItemStrikes = blockUnpaidItemStrikes;
        UnpaidItemStrikesCount = unpaidItemStrikesCount;
        UnpaidItemStrikesPeriodInMonths = unpaidItemStrikesPeriodInMonths;
        BlockPrimaryAddressOutsideShippingLocation = blockPrimaryAddressOutsideShippingLocation;
        BlockMaxItemsInLastTenDays = blockMaxItemsInLastTenDays;
        MaxItemsInLastTenDays = maxItemsInLastTenDays;
        ApplyFeedbackScoreThreshold = applyFeedbackScoreThreshold;
        FeedbackScoreThreshold = feedbackScoreThreshold;
        UpdateBlockSettingsForActiveListings = updateBlockSettingsForActiveListings;
        RequirePaymentMethodBeforeBid = requirePaymentMethodBeforeBid;
        RequirePaymentMethodBeforeOffer = requirePaymentMethodBeforeOffer;
        PreventBlockedBuyersFromContacting = preventBlockedBuyersFromContacting;
    }

    public static BuyerManagementSettings CreateDefault() => new(
        blockUnpaidItemStrikes: false,
        unpaidItemStrikesCount: 2,
        unpaidItemStrikesPeriodInMonths: 1,
        blockPrimaryAddressOutsideShippingLocation: true,
        blockMaxItemsInLastTenDays: false,
        maxItemsInLastTenDays: null,
        applyFeedbackScoreThreshold: false,
        feedbackScoreThreshold: null,
        updateBlockSettingsForActiveListings: false,
        requirePaymentMethodBeforeBid: true,
        requirePaymentMethodBeforeOffer: true,
        preventBlockedBuyersFromContacting: true);

    public Result Update(
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
        if (blockUnpaidItemStrikes)
        {
            if (unpaidItemStrikesCount is < 1 or > 4)
            {
                return SellerPreferenceErrors.InvalidUnpaidItemStrikeCount;
            }

            if (unpaidItemStrikesPeriodInMonths is < 1 or > 12)
            {
                return SellerPreferenceErrors.InvalidUnpaidItemStrikePeriod;
            }
        }
        else
        {
            unpaidItemStrikesCount = 0;
            unpaidItemStrikesPeriodInMonths = 0;
        }

        if (blockMaxItemsInLastTenDays)
        {
            if (maxItemsInLastTenDays is null or < 1 or > MaxMaxItemsThreshold)
            {
                return SellerPreferenceErrors.InvalidMaxItemsInLastTenDays;
            }
        }
        else
        {
            maxItemsInLastTenDays = null;
        }

        if (applyFeedbackScoreThreshold)
        {
            if (feedbackScoreThreshold is null or < 0 or > MaxFeedbackScore)
            {
                return SellerPreferenceErrors.InvalidFeedbackScoreThreshold;
            }
        }
        else
        {
            feedbackScoreThreshold = null;
        }

        BlockUnpaidItemStrikes = blockUnpaidItemStrikes;
        UnpaidItemStrikesCount = unpaidItemStrikesCount;
        UnpaidItemStrikesPeriodInMonths = unpaidItemStrikesPeriodInMonths;
        BlockPrimaryAddressOutsideShippingLocation = blockPrimaryAddressOutsideShippingLocation;
        BlockMaxItemsInLastTenDays = blockMaxItemsInLastTenDays;
        MaxItemsInLastTenDays = maxItemsInLastTenDays;
        ApplyFeedbackScoreThreshold = applyFeedbackScoreThreshold;
        FeedbackScoreThreshold = feedbackScoreThreshold;
        UpdateBlockSettingsForActiveListings = updateBlockSettingsForActiveListings;
        RequirePaymentMethodBeforeBid = requirePaymentMethodBeforeBid;
        RequirePaymentMethodBeforeOffer = requirePaymentMethodBeforeOffer;
        PreventBlockedBuyersFromContacting = preventBlockedBuyersFromContacting;

        return Result.Success();
    }
}
