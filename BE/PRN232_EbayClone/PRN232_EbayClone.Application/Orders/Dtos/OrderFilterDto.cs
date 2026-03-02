using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Domain.Orders.Enums;

namespace PRN232_EbayClone.Application.Orders.Dtos;
public enum OrderPeriod
{
    Today,
    Yesterday,
    ThisWeek,
    LastWeek,
    ThisMonth,
    LastMonth,
    ThisYear,
    LastYear,
    Last7Days,
    Last30Days,
    Last90Days,
    Custom
}
public enum OrderSearchBy
{
    BuyerUsername,
    BuyerName,
    OrderNumber,
    SalesRecordNumber,
    ItemTitle,
    ItemId,
    CustomLabel
}
public enum OrderSortBy
{
    DatePaid,
    Buyer,
    CustomLabel,
    Total
}
public sealed record OrderFilterDto(
    string? Status = null,
    OrderPeriod Period = OrderPeriod.Last90Days,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    OrderSearchBy? SearchBy = null,
    string? Keyword = null,
    OrderSortBy SortBy = OrderSortBy.DatePaid,
    bool SortDescending = true
) : PagingRequest;