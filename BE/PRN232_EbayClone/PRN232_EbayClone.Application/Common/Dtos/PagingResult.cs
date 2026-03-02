namespace PRN232_EbayClone.Application.Common.Dtos;

public record PagingResult<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
);
