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

public sealed record GetShippingServicesQuery() : IQuery<IReadOnlyList<ShippingServiceDto>>;

public sealed class GetShippingServicesQueryHandler : IQueryHandler<GetShippingServicesQuery, IReadOnlyList<ShippingServiceDto>>
{
    private readonly IShippingServiceRepository _shippingServiceRepository;

    public GetShippingServicesQueryHandler(IShippingServiceRepository shippingServiceRepository)
    {
        _shippingServiceRepository = shippingServiceRepository;
    }

    public async Task<Result<IReadOnlyList<ShippingServiceDto>>> Handle(GetShippingServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await _shippingServiceRepository.GetAllAsync(cancellationToken);

        if (services.Count == 0)
        {
            return Result.Success<IReadOnlyList<ShippingServiceDto>>(Array.Empty<ShippingServiceDto>());
        }

        var serviceDtos = services
            .Select(service => new ShippingServiceDto(
                service.Id,
                service.Carrier,
                service.Slug,
                service.ServiceCode,
                service.ServiceName,
                new MoneyDto(service.BaseCost.Amount, service.BaseCost.Currency),
                service.MinEstimatedDeliveryDays,
                service.MaxEstimatedDeliveryDays,
                service.PrinterRequired,
                service.SupportsQrCode,
                service.CoverageDescription,
                service.SavingsDescription,
                service.Notes,
                service.DeliveryWindowLabel))
            .ToList();

        return Result.Success<IReadOnlyList<ShippingServiceDto>>(serviceDtos);
    }
}
