using PRN232_EbayClone.Application.Abstractions.Messaging;

namespace PRN232_EbayClone.Application.VolumePricings.Commands;

public sealed record DeactivateVolumePricingCommand(Guid PricingId) : ICommand;
