using System;
using System.Collections.Generic;
using PRN232_EbayClone.Domain.Listings.Enums;

namespace PRN232_EbayClone.Application.Research.Dtos;

public sealed record ProductResearchRangeDto(DateOnly From, DateOnly To, string Label);

public sealed record ProductResearchTrendPointDto(DateOnly Date, decimal AveragePrice);

public sealed record ProductResearchSummaryMetricDto(string Key, string Label, string Value);

public sealed record ProductResearchMoneyDto(decimal Amount, string Currency);

public sealed record ProductResearchListingDto(
    Guid ListingId,
    string Title,
    ProductResearchMoneyDto Price,
    ProductResearchMoneyDto? Shipping,
    int? TotalSold,
    ProductResearchMoneyDto? TotalSales,
    int? Watchers,
    bool? Promoted,
    string? PricingType,
    int? Bids,
    DateTime? LastSoldAt,
    DateTime? StartDate,
    string? ImageUrl);

public sealed record ProductResearchPaginationDto(int Page, int PageSize, int TotalCount);

public sealed record ProductResearchActiveListingsPage(
    IReadOnlyList<ProductResearchActiveListingRecord> Items,
    int TotalCount,
    decimal? AveragePrice,
    decimal? MinPrice,
    decimal? MaxPrice,
    string Currency);

public sealed record ProductResearchPanelDto(
    IReadOnlyList<ProductResearchSummaryMetricDto> Summary,
    IReadOnlyList<ProductResearchTrendPointDto> Trend,
    IReadOnlyList<ProductResearchListingDto> Listings,
    ProductResearchPaginationDto Pagination);

public sealed record ProductResearchResponseDto(
    ProductResearchRangeDto Range,
    ProductResearchPanelDto Sold,
    ProductResearchPanelDto Active);

public sealed record ProductResearchOrderItemRecord(
    Guid OrderId,
    Guid ListingId,
    string Title,
    string Sku,
    int Quantity,
    decimal UnitPriceAmount,
    string UnitPriceCurrency,
    decimal TotalPriceAmount,
    DateTime OrderedAtUtc,
    DateTime? PaidAtUtc,
    decimal ShippingAmount,
    string ShippingCurrency,
    Guid BuyerId,
    string? ImageUrl,
    Guid CategoryId);

public sealed record ProductResearchActiveListingRecord(
    Guid ListingId,
    string Title,
    string Sku,
    ListingFormat Format,
    decimal PriceAmount,
    string PriceCurrency,
    DateTime? StartDate,
    DateTime CreatedAt,
    string? ImageUrl,
    bool IsMultiVariation,
    int AvailableQuantity,
    Guid CategoryId);
