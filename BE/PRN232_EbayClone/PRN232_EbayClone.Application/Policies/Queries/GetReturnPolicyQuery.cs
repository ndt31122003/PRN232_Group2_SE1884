using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Policies.Dtos;

namespace PRN232_EbayClone.Application.Policies.Queries;

public sealed record GetReturnPolicyQuery(string StoreId) : IQuery<ReturnPolicyDto?>;

public sealed class GetReturnPolicyQueryHandler : IQueryHandler<GetReturnPolicyQuery, ReturnPolicyDto?>
{
    private readonly IReturnPolicyRepository _returnPolicyRepository;
    private readonly IStoreRepository _storeRepository;

    public GetReturnPolicyQueryHandler(
        IReturnPolicyRepository returnPolicyRepository,
        IStoreRepository storeRepository)
    {
        _returnPolicyRepository = returnPolicyRepository;
        _storeRepository = storeRepository;
    }

    public async Task<Result<ReturnPolicyDto?>> Handle(GetReturnPolicyQuery request, CancellationToken cancellationToken)
    {
        var storeId = Domain.Stores.ValueObjects.StoreId.From(Guid.Parse(request.StoreId));
        
        var store = await _storeRepository.GetByIdAsync(storeId, cancellationToken);
        if (store is null)
            return Domain.Stores.Errors.StoreErrors.NotFound;

        var policy = await _returnPolicyRepository.GetByStoreIdAsync(storeId, cancellationToken);
        
        if (policy is null)
            return (ReturnPolicyDto?)null;

        var dto = new ReturnPolicyDto(
            policy.Id,
            policy.AcceptReturns,
            policy.ReturnPeriodDays,
            policy.RefundMethod,
            policy.ReturnShippingPaidBy);

        return dto;
    }
}

