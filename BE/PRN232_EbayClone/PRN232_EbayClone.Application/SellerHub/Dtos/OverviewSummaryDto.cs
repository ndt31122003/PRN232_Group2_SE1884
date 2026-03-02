namespace PRN232_EbayClone.Application.SellerHub.Dtos;

public sealed record OverviewSummaryDto(
    OverviewHeaderDto Header,
    OverviewStatusDto Status,
    OverviewSectionDto Listings,
    OverviewSectionDto Orders,
    OverviewSalesDto Sales
);

public sealed record OverviewHeaderDto(
    Guid SellerId,
    string SellerName,
    string SellerUsername,
    string SellerEmail,
    int ListingViewsLast90Days,
    decimal SalesLast90Days,
    string SalesCurrency,
    int OrdersLast90Days
);

public sealed record OverviewStatusDto(
    string Message,
    string Level,
    int OutstandingTasks
);

public sealed record OverviewSectionDto(
    string Title,
    IReadOnlyList<OverviewSectionItemDto> Items
);

public sealed record OverviewSectionItemDto(
    string Key,
    string Label,
    int? Count,
    string? NavigationPath
);

public sealed record OverviewSalesDto(
    IReadOnlyList<OverviewSalesPointDto> Chart,
    IReadOnlyList<OverviewSalesSummaryRowDto> Summary,
    string Currency
);

public sealed record OverviewSalesPointDto(
    DateOnly Date,
    decimal Total
);

public sealed record OverviewSalesSummaryRowDto(
    string Key,
    string Label,
    decimal Total
);
