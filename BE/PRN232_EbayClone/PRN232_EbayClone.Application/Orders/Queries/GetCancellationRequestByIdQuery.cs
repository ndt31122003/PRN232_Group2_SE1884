using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Orders.Mappings;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Queries;

public sealed record GetCancellationRequestByIdQuery(
    Guid SellerId,
    Guid CancellationRequestId) : IQuery<CancellationSummaryDto>;

public sealed class GetCancellationRequestByIdQueryHandler
    : IQueryHandler<GetCancellationRequestByIdQuery, CancellationSummaryDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetCancellationRequestByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<CancellationSummaryDto>> Handle(
        GetCancellationRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        var record = await _orderRepository.GetCancellationRequestAsync(
            request.SellerId,
            request.CancellationRequestId,
            cancellationToken);

        if (record is null)
        {
            return Error.Failure("CancellationRequest.NotFound", "Cancellation request not found.");
        }

        var dto = record.ToDto();
        return Result.Success(dto);
    }
}
