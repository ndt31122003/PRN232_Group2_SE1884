using System;

namespace PRN232_EbayClone.Application.Performance.Records;

// ✅ GIỮ NGUYÊN - Record này dùng count, DTO mới dùng rate
public sealed record PerformanceSellerLevelRecord(
    string Region,
    string CurrentLevel,
    string EvaluatedTodayLevel,
    decimal TransactionDefectRate,
    decimal LateShipmentRate,
    decimal TrackingUploadedOnTimeRate,
    int CasesClosedWithoutSellerResolution, // ✅ GIỮ int count (convert sang rate ở handler)
    int TransactionsLast12Months,
    decimal SalesLast12Months,
    string Currency,
    DateOnly NextEvaluationDate
);
