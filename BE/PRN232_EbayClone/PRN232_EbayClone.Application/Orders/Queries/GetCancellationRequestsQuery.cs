using System;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Orders.Mappings;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Queries;

public sealed record GetCancellationRequestsQuery(
    Guid SellerId,
    CancellationFilterDto Filter) : IQuery<PagingResult<CancellationSummaryDto>>;

public sealed class GetCancellationRequestsQueryHandler
    : IQueryHandler<GetCancellationRequestsQuery, PagingResult<CancellationSummaryDto>>
{
    private const int MaxPageSize = 200;
    private readonly IOrderRepository _orderRepository;

    public GetCancellationRequestsQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<PagingResult<CancellationSummaryDto>>> Handle(
        GetCancellationRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var filter = request.Filter ?? new CancellationFilterDto();
        var pageNumber = Math.Max(filter.PageNumber, 1);
        var pageSize = Math.Clamp(filter.PageSize, 1, MaxPageSize);
        filter = filter with { PageNumber = pageNumber, PageSize = pageSize };

        var page = await _orderRepository.GetCancellationRequestsAsync(
            request.SellerId,
            filter,
            cancellationToken);

        var dto = page.ToDto(pageNumber, pageSize);
        return Result.Success(dto);
    }
}
