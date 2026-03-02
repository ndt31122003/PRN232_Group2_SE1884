using System.Collections.Generic;

namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentReportSectionDto(string Title, decimal Total, IReadOnlyList<PaymentReportLineDto> Lines);
