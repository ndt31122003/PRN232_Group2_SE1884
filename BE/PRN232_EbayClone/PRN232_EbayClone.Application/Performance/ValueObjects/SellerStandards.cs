// ? CÓ TH? T?O FILE M?I TRONG PERFORMANCE FOLDER
namespace PRN232_EbayClone.Application.Performance.ValueObjects;

public sealed record SellerStandards(
    string Region,
    decimal TopRatedDefectRate,
    decimal TopRatedLateShipmentRate,
    decimal TopRatedTrackingRate,
    decimal TopRatedCasesClosedRate,
    int MinTransactions,
    decimal MinSales)
{
    public static SellerStandards GetDefault() => new("US", 0.5m, 2m, 95m, 0.3m, 100, 1000m);
    
    // ? Có th? detect region t? currency ho?c data khác
    public static SellerStandards ForCurrency(string currency) => currency switch
    {
        "USD" => new("US", 0.5m, 2m, 95m, 0.3m, 100, 1000m),
        "GBP" => new("UK", 0.5m, 2.5m, 95m, 0.3m, 100, 1000m),
        "EUR" => new("EU", 0.5m, 3m, 93m, 0.4m, 100, 1000m),
        _ => GetDefault()
    };
}