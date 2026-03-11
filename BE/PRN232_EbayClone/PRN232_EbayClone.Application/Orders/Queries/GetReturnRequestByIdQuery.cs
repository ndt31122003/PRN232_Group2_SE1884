using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Orders.Mappings;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Queries;

public sealed record GetReturnRequestByIdQuery(
    Guid SellerId,
    Guid ReturnRequestId) : IQuery<ReturnSummaryDto>;

public sealed class GetReturnRequestByIdQueryHandler
    : IQueryHandler<GetReturnRequestByIdQuery, ReturnSummaryDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetReturnRequestByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<ReturnSummaryDto>> Handle(
        GetReturnRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var record = await _orderRepository.GetReturnRequestAsync(
            request.SellerId,
            request.ReturnRequestId,
            cancellationToken);

        if (record is null)
        {
            return Error.Failure("ReturnRequest.NotFound", "Return request not found.");
        }

        var dto = record.ToReturnSummaryDto();
        return Result.Success(dto);
    }
}
