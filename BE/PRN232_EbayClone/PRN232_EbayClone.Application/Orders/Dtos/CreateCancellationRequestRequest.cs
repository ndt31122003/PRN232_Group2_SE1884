using PRN232_EbayClone.Domain.Orders.Enums;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record CreateCancellationRequestRequest(
    CancellationReason Reason,
    string? SellerNote);
