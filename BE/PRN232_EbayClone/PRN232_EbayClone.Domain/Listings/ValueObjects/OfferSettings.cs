using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Listings.ValueObjects;

public sealed record OfferSettings(
    bool AllowOffers,
    decimal? MinimumOffer,
    decimal? AutoAcceptOffer
)
{
    public static Result<OfferSettings> Create(
        bool allowOffers,
        decimal? minimumOffer,
        decimal? autoAcceptOffer)
    {
        if (!allowOffers)
            return Result.Success(new OfferSettings(false, null, null));

        if (autoAcceptOffer.HasValue && minimumOffer.HasValue &&
            autoAcceptOffer.Value < minimumOffer.Value)
        {
            return Error.Failure(
                "OfferSettings.InvalidAutoAccept",
                "Auto-accept offer must be greater than or equal to the minimum offer.");
        }

        return Result.Success(new OfferSettings(allowOffers, minimumOffer, autoAcceptOffer));
    }
}

