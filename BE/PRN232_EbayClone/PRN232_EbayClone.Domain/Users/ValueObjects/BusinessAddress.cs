using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Users.ValueObjects;

public sealed record BusinessAddress
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string ZipCode { get; }
    public string Country { get; }

    private BusinessAddress(string street, string city, string state, string zipCode, string country)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
    }

    public static Result<BusinessAddress> Create(
        string street, string city, string state, string zipCode, string country)
    {
        if (string.IsNullOrWhiteSpace(street))
            return Error.Validation("BusinessAddress.StreetRequired", "Street is required.");

        if (string.IsNullOrWhiteSpace(city))
            return Error.Validation("BusinessAddress.CityRequired", "City is required.");

        if (string.IsNullOrWhiteSpace(state))
            return Error.Validation("BusinessAddress.StateRequired", "State is required.");

        if (string.IsNullOrWhiteSpace(zipCode))
            return Error.Validation("BusinessAddress.ZipCodeRequired", "Zip code is required.");

        if (string.IsNullOrWhiteSpace(country))
            return Error.Validation("BusinessAddress.CountryRequired", "Country is required.");

        return new BusinessAddress(street.Trim(), city.Trim(), state.Trim(), zipCode.Trim(), country.Trim());
    }
}
