using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Policies.Dtos;

namespace PRN232_EbayClone.Application.Policies.Queries;

public sealed record GetShippingPoliciesQuery(string StoreId) : IQuery<IEnumerable<ShippingPolicyDto>>;

public sealed class GetShippingPoliciesQueryHandler : IQueryHandler<GetShippingPoliciesQuery, IEnumerable<ShippingPolicyDto>>
{
    private readonly IShippingPolicyRepository _shippingPolicyRepository;
    private readonly IStoreRepository _storeRepository;

    public GetShippingPoliciesQueryHandler(
        IShippingPolicyRepository shippingPolicyRepository,
        IStoreRepository storeRepository)
    {
        _shippingPolicyRepository = shippingPolicyRepository;
        _storeRepository = storeRepository;
    }

    public async Task<Result<IEnumerable<ShippingPolicyDto>>> Handle(GetShippingPoliciesQuery request, CancellationToken cancellationToken)
    {
        var storeId = Domain.Stores.ValueObjects.StoreId.From(Guid.Parse(request.StoreId));
        
        var store = await _storeRepository.GetByIdAsync(storeId, cancellationToken);
        if (store is null)
            return Domain.Stores.Errors.StoreErrors.NotFound;

        var policies = await _shippingPolicyRepository.GetByStoreIdAsync(storeId, cancellationToken);
        
        var dtos = policies.Select(p => new ShippingPolicyDto(
            p.Id,
            p.Carrier,
            p.ServiceName,
            p.Cost.Amount,
            p.Cost.Currency,
            p.HandlingTimeDays,
            p.IsDefault));

        return dtos.ToList();
    }
}

