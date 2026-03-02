using PRN232_EbayClone.Domain.Policies.Enums;

namespace PRN232_EbayClone.Application.Policies.Dtos;

public sealed record ReturnPolicyDto(
    Guid Id,
    bool AcceptReturns,
    ReturnPeriod? ReturnPeriodDays,
    RefundMethod? RefundMethod,
    ReturnShippingPaidBy? ReturnShippingPaidBy
);

