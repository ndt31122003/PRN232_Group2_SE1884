using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Listings.Dtos;

namespace PRN232_EbayClone.Application.Listings.Queries;

public record GetListingOffersQuery(Guid? ListingId) : IQuery<List<OfferDto>>;
