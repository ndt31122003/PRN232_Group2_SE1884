using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Performance.Queries;

public sealed record GetPerformanceInventoryDashboardQuery(Guid SellerId)
    : IQuery<PerformanceInventoryDashboardDto>;

public sealed class GetPerformanceInventoryDashboardQueryHandler(
    IPerformanceRepository PerformanceRepository)
    : IQueryHandler<GetPerformanceInventoryDashboardQuery, PerformanceInventoryDashboardDto>
{
    public async Task<Result<PerformanceInventoryDashboardDto>> Handle(
        GetPerformanceInventoryDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var snapshot = await PerformanceRepository.GetInventoryDashboardAsync(request.SellerId, cancellationToken);

        var dto = new PerformanceInventoryDashboardDto(
            snapshot.TotalListings,
            snapshot.AvailableQuantity,
            snapshot.ReservedQuantity,
            snapshot.SoldQuantity,
            snapshot.LowStockListings,
            snapshot.OutOfStockListings,
            snapshot.CriticalListings
                .Select(item => new PerformanceInventoryHealthDto(
                    item.ListingId,
                    item.Title,
                    item.Sku,
                    item.AvailableQuantity,
                    item.ReservedQuantity,
                    item.SoldQuantity,
                    item.ThresholdQuantity,
                    item.LastUpdatedAt))
                .ToList());

        return Result.Success(dto);
    }
}