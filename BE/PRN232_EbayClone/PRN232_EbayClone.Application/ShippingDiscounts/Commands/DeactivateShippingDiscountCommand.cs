using PRN232_EbayClone.Application.Abstractions.Messaging;

namespace PRN232_EbayClone.Application.ShippingDiscounts.Commands;

public sealed record DeactivateShippingDiscountCommand(Guid DiscountId) : ICommand;
