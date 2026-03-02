using System;
using PRN232_EbayClone.Application.Common.Dtos;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public enum ResolutionPeriod
{
    Last30Days,
    Last90Days,
    Last180Days,
    ThisYear,
    Custom
}

public enum CancellationFilterStatus
{
    Open,
    Requests,
    InProgress,
    Declined,
    Cancelled
}

public enum CancellationSearchBy
{
    BuyerUsername,
    CancelId
}

public enum CancellationSortBy
{
    DateRequested,

}

public sealed record CancellationFilterDto(
    CancellationFilterStatus Status = CancellationFilterStatus.Open,
    ResolutionPeriod Period = ResolutionPeriod.Last90Days,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    CancellationSearchBy? SearchBy = CancellationSearchBy.BuyerUsername,
    string? Keyword = null,
    CancellationSortBy? SortBy = CancellationSortBy.DateRequested,
    bool SortDescending = true,
    int PageNumber = 1,
    int PageSize = 50
) : PagingRequest(PageNumber, PageSize);
