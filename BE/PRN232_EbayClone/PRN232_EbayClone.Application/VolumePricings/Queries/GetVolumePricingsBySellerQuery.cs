using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.VolumePricings.Dtos;

namespace PRN232_EbayClone.Application.VolumePricings.Queries;

public sealed record GetVolumePricingsBySellerQuery(Guid SellerId) : IQuery<List<VolumePricingDto>>;
