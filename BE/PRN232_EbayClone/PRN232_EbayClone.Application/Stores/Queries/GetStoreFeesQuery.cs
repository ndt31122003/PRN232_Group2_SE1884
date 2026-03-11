using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Stores.Enums;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Application.Stores.Queries;

public sealed record GetStoreFeesQuery(string StoreId) : IQuery<StoreFeesDto>;

public sealed record StoreFeesDto(
    decimal MonthlyFee,
    string Currency,
    decimal FinalValueFeePercentage,
    int ListingLimit,
    StoreType PlanType
);

public sealed class GetStoreFeesQueryValidator : AbstractValidator<GetStoreFeesQuery>
{
    public GetStoreFeesQueryValidator()
    {
        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("Store ID là bắt buộc")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Store ID không hợp lệ");
    }
}

public sealed class GetStoreFeesQueryHandler : IQueryHandler<GetStoreFeesQuery, StoreFeesDto>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IUserContext _userContext;

    public GetStoreFeesQueryHandler(
        IStoreRepository storeRepository,
        IUserContext userContext)
    {
        _storeRepository = storeRepository;
        _userContext = userContext;
    }

    public async Task<Result<StoreFeesDto>> Handle(GetStoreFeesQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
            return StoreErrors.Unauthorized;

        var storeId = new StoreId(Guid.Parse(request.StoreId));
        var store = await _storeRepository.GetByIdAsync(storeId, cancellationToken);

        if (store is null)
            return StoreErrors.NotFound;

        // Check ownership
        if (store.UserId.Value.ToString() != userId)
            return StoreErrors.Unauthorized;

        var activeSubscription = store.Subscriptions.FirstOrDefault(s => s.IsActive);
        if (activeSubscription is null)
            return StoreErrors.NotFound;

        var feesDto = new StoreFeesDto(
            activeSubscription.MonthlyFee.Amount,
            activeSubscription.MonthlyFee.Currency,
            activeSubscription.FinalValueFeePercentage,
            activeSubscription.ListingLimit,
            activeSubscription.SubscriptionType);

        return feesDto;
    }
}

