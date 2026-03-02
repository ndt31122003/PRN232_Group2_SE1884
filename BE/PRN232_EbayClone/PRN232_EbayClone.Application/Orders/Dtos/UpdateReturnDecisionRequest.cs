using System;

namespace PRN232_EbayClone.Application.Orders.Dtos;

public sealed record UpdateReturnDecisionRequest(
    DateTime? BuyerReturnDueAtUtc,
    string? SellerNote);
