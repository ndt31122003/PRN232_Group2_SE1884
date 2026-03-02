using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Performance.Abstractions;
using PRN232_EbayClone.Application.Performance.Dtos;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Performance.Queries;

public sealed record GetPerformanceSellerLevelDetailsQuery(Guid SellerId)
    : IQuery<PerformanceSellerLevelDetailDto>;

public sealed class GetPerformanceSellerLevelDetailsQueryHandler(
    IPerformanceRepository PerformanceRepository
) : IQueryHandler<GetPerformanceSellerLevelDetailsQuery, PerformanceSellerLevelDetailDto>
{
    public async Task<Result<PerformanceSellerLevelDetailDto>> Handle(
        GetPerformanceSellerLevelDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var record = await PerformanceRepository.GetSellerLevelAsync(
            request.SellerId,
            DateTime.UtcNow,
            cancellationToken);

        // ✅ Calculate CasesClosedRate
        var casesClosedRate = record.TransactionsLast12Months > 0
            ? Math.Round(
                record.CasesClosedWithoutSellerResolution / (decimal)record.TransactionsLast12Months * 100m,
                2,
                MidpointRounding.AwayFromZero)
            : 0m;

        var dto = new PerformanceSellerLevelDetailDto(
            record.Region,
            record.CurrentLevel,
            record.EvaluatedTodayLevel,
            record.TransactionDefectRate,
            record.LateShipmentRate,
            record.TrackingUploadedOnTimeRate,
            casesClosedRate, // ✅ Use rate instead of count
            record.TransactionsLast12Months,
            record.SalesLast12Months,
            record.Currency,
            record.NextEvaluationDate.ToString("MMM d, yyyy", CultureInfo.InvariantCulture)
        );

        return Result.Success(dto);
    }
}
