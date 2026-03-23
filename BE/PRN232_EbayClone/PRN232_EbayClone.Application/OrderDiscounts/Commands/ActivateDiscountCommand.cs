using PRN232_EbayClone.Application.Abstractions.Messaging;

namespace PRN232_EbayClone.Application.OrderDiscounts.Commands;

public sealed record ActivateDiscountCommand(Guid DiscountId) : ICommand;
