namespace PRN232_EbayClone.Application.Performance.Dtos;

public sealed record PerformanceSellerLevelDetailDto(
    string Region,
    string CurrentSellerLevel,
    string IfEvaluatedTodayLevel,
    decimal TransactionDefectRate,
    decimal LateShipmentRate,
    decimal TrackingUploadedOnTimeRate,
    decimal CasesClosedWithoutSellerResolutionRate, // ✅ CHANGED: from int to decimal rate
    int TransactionsLast12Months,
    decimal SalesLast12Months,
    string Currency,
    string NextEvaluationDate
);
