using PRN232_EbayClone.Application.Abstractions.Messaging;

namespace PRN232_EbayClone.Application.OrderDiscounts.Commands;

public sealed record DeactivateDiscountCommand(Guid DiscountId) : ICommand;
