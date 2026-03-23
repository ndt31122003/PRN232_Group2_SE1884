using PRN232_EbayClone.Application.Abstractions.Messaging;

namespace PRN232_EbayClone.Application.VolumePricings.Commands;

public sealed record ActivateVolumePricingCommand(Guid PricingId) : ICommand;
