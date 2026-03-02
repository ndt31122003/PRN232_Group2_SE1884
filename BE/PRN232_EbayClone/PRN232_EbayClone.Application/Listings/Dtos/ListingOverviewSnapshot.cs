using PRN232_EbayClone.Domain.Listings.Enums;

namespace PRN232_EbayClone.Application.Listings.Dtos;

public sealed record ListingOverviewSnapshot(
    IReadOnlyDictionary<ListingStatus, int> StatusCounts,
    int AuctionsEndingToday,
    int BuyItNowRenewingToday,
    int WithReserveMet,
    int WithQuestions,
    int WithOpenOffers,
    int UnsoldNotRelisted
);
