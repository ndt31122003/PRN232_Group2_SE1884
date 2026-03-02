using System;
using PRN232_EbayClone.Application.Common.Dtos;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public enum ReturnFilterStatus
{
    NeedsAttention,
    OpenReturnsReplacements,
    OpenReplacements,
    OpenReturns,
    InProgress,
    Shipped,
    Delivered,
    Closed
}

public enum ReturnSearchBy
{
    BuyerUsername,
    OrderNumber,
    ItemTitle,
    TrackingNumber
}

public enum ReturnSortBy
{
    DateRequested,
    Buyer,
    ReturnStatus,
    DueDate
}

public sealed record ReturnFilterDto(
    ReturnFilterStatus Status = ReturnFilterStatus.OpenReturnsReplacements,
    ResolutionPeriod Period = ResolutionPeriod.Last90Days,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    ReturnSearchBy SearchBy = ReturnSearchBy.BuyerUsername,
    string? Keyword = null,
    ReturnSortBy SortBy = ReturnSortBy.DateRequested,
    bool SortDescending = true,
    int PageNumber = 1,
    int PageSize = 50
) : PagingRequest(PageNumber, PageSize);
