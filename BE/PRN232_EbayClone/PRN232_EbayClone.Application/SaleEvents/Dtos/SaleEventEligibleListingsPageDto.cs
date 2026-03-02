namespace PRN232_EbayClone.Application.SaleEvents.Dtos;

public sealed record SaleEventEligibleListingsPageDto(
    IReadOnlyList<SaleEventEligibleListingDto> Items,
    int TotalCount,
    int PageNumber,
    int PageSize);
