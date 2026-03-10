using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Orders.Queries;

public sealed record GetShippingLabelsQuery(
    Guid SellerId,
    DateTime? FromDateUtc,
    DateTime? ToDateUtc,
    int Limit) : IQuery<IReadOnlyList<ShippingLabelSummaryDto>>;

public sealed class GetShippingLabelsQueryHandler : IQueryHandler<GetShippingLabelsQuery, IReadOnlyList<ShippingLabelSummaryDto>>
{
    private const int MaxLimit = 200;
    private const int DefaultLimit = 50;
    private readonly IShippingLabelRepository _shippingLabelRepository;

    public GetShippingLabelsQueryHandler(IShippingLabelRepository shippingLabelRepository)
    {
        _shippingLabelRepository = shippingLabelRepository;
    }

    public async Task<Result<IReadOnlyList<ShippingLabelSummaryDto>>> Handle(GetShippingLabelsQuery request, CancellationToken cancellationToken)
    {
        var limit = request.Limit <= 0 ? DefaultLimit : Math.Min(request.Limit, MaxLimit);

        var fromUtc = request.FromDateUtc?.ToUniversalTime();
        var toUtc = request.ToDateUtc?.ToUniversalTime();

        var records = await _shippingLabelRepository.GetSummariesAsync(
            request.SellerId,
            fromUtc,
            toUtc,
            limit,
            cancellationToken);

        var dtos = records
            .Select(record => new ShippingLabelSummaryDto(
                record.LabelId,
                record.OrderId,
                record.OrderNumber,
                record.Carrier,
                record.ServiceName,
                record.TrackingNumber,
                record.PurchasedAt,
                new MoneyDto(record.CostAmount, record.CostCurrency),
                record.LabelUrl,
                record.LabelFileName,
                record.IsVoided,
                record.VoidedAt,
                record.VoidReason))
            .ToList();

        return Result.Success<IReadOnlyList<ShippingLabelSummaryDto>>(dtos);
    }
}
