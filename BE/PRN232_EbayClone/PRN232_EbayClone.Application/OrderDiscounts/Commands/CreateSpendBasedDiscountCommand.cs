using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Application.OrderDiscounts.Commands;

public sealed record CreateSpendBasedDiscountCommand(
    Guid SellerId,
    string Name,
    string? Description,
    decimal ThresholdAmount,
    decimal DiscountValue,
    DiscountUnit DiscountUnit,
    decimal? MaxDiscount,
    DateTime StartDate,
    DateTime EndDate,
    List<TierDefinition>? Tiers,
    List<Guid>? IncludedItemIds,
    List<Guid>? ExcludedItemIds,
    List<Guid>? IncludedCategoryIds,
    List<Guid>? ExcludedCategoryIds
) : ICommand<Guid>;

public sealed record TierDefinition(
    decimal ThresholdValue,
    decimal DiscountValue);
