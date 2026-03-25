using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Discounts.Entities;

namespace PRN232_EbayClone.Application.VolumePricings.Commands;

public sealed record VolumePricingTierRequest(
    int MinQuantity,
    decimal DiscountValue,
    string DiscountUnit); // "percent" or "fixed"

public sealed record CreateVolumePricingCommand(
    Guid SellerId,
    Guid? ListingId,
    string Name,
    string? Description,
    DateTime StartDate,
    DateTime EndDate,
    List<VolumePricingTierRequest> Tiers) : ICommand<Guid>;
