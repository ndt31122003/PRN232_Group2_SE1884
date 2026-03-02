using PRN232_EbayClone.Domain.Policies.Enums;
using PRN232_EbayClone.Domain.Policies.Errors;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Domain.Policies.Entities;

public sealed class ReturnPolicy : AggregateRoot<Guid>
{
    public StoreId StoreId { get; private set; }
    public bool AcceptReturns { get; private set; }
    public ReturnPeriod? ReturnPeriodDays { get; private set; }
    public RefundMethod? RefundMethod { get; private set; }
    public ReturnShippingPaidBy? ReturnShippingPaidBy { get; private set; }

    private ReturnPolicy(Guid id) : base(id) { }

    public static Result<ReturnPolicy> Create(
        StoreId storeId,
        bool acceptReturns,
        ReturnPeriod? returnPeriodDays = null,
        RefundMethod? refundMethod = null,
        ReturnShippingPaidBy? returnShippingPaidBy = null)
    {
        if (!acceptReturns)
        {
            return new ReturnPolicy(Guid.NewGuid())
            {
                StoreId = storeId,
                AcceptReturns = false
            };
        }

        if (returnPeriodDays == null)
            return PolicyErrors.ReturnPeriodRequired;

        if (refundMethod == null)
            return PolicyErrors.RefundMethodRequired;

        if (returnShippingPaidBy == null)
            return PolicyErrors.ReturnShippingPaidByRequired;

        return new ReturnPolicy(Guid.NewGuid())
        {
            StoreId = storeId,
            AcceptReturns = true,
            ReturnPeriodDays = returnPeriodDays.Value,
            RefundMethod = refundMethod.Value,
            ReturnShippingPaidBy = returnShippingPaidBy.Value
        };
    }

    public Result Update(
        bool acceptReturns,
        ReturnPeriod? returnPeriodDays = null,
        RefundMethod? refundMethod = null,
        ReturnShippingPaidBy? returnShippingPaidBy = null)
    {
        if (!acceptReturns)
        {
            AcceptReturns = false;
            ReturnPeriodDays = null;
            RefundMethod = null;
            ReturnShippingPaidBy = null;
            return Result.Success();
        }

        if (returnPeriodDays == null)
            return PolicyErrors.ReturnPeriodRequired;

        if (refundMethod == null)
            return PolicyErrors.RefundMethodRequired;

        if (returnShippingPaidBy == null)
            return PolicyErrors.ReturnShippingPaidByRequired;

        AcceptReturns = true;
        ReturnPeriodDays = returnPeriodDays.Value;
        RefundMethod = refundMethod.Value;
        ReturnShippingPaidBy = returnShippingPaidBy.Value;

        return Result.Success();
    }
}

