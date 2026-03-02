namespace PRN232_EbayClone.Application.Common.Dtos;

public record PagingRequest(
    int PageNumber = 1,
    int PageSize = 10
);
