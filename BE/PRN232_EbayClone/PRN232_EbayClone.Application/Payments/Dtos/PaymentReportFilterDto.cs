namespace PRN232_EbayClone.Application.Payments.Dtos;

public sealed record PaymentReportFilterDto
{
    public string Period { get; init; } = "thisMonth";
    public string Compared { get; init; } = "none";
}
