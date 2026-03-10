using System;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Orders.Mappings;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Queries;

public sealed record GetReturnRequestsQuery(
    Guid SellerId,
    ReturnFilterDto Filter) : IQuery<PagingResult<ReturnSummaryDto>>;

public sealed class GetReturnRequestsQueryHandler
    : IQueryHandler<GetReturnRequestsQuery, PagingResult<ReturnSummaryDto>>
{
    private const int MaxPageSize = 200;
    private readonly IOrderRepository _orderRepository;

    public GetReturnRequestsQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<PagingResult<ReturnSummaryDto>>> Handle(
        GetReturnRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = request.Filter ?? new ReturnFilterDto();
        var pageNumber = Math.Max(filter.PageNumber, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, MaxPageSize);
        filter = filter with { PageNumber = pageNumber, PageSize = pageSize };

        var page = await _orderRepository.GetReturnRequestsAsync(
            request.SellerId,
            filter,
            cancellationToken);

        var dto = page.ToDto(pageNumber, pageSize);
        return Result.Success(dto);
    }
}
