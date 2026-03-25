namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record CompleteReturnRefundRequest(decimal? RefundAmount, string? SellerNote);