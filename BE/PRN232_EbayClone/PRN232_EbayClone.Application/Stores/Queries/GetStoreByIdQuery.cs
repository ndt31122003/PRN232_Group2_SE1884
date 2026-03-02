using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Stores.Enums;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;

namespace PRN232_EbayClone.Application.Stores.Queries;

public sealed record GetStoreByIdQuery(string StoreId) : IQuery<StoreDetailsDto>;

public sealed record StoreDetailsDto(
    Guid StoreId,
    string Name,
    string Slug,
    string? Description,
    string? LogoUrl,
    string? BannerUrl,
    StoreType StoreType,
    bool IsActive,
    SubscriptionDetailsDto ActiveSubscription
);

public sealed record SubscriptionDetailsDto(
    Guid SubscriptionId,
    StoreType SubscriptionType,
    decimal MonthlyFee,
    string Currency,
    decimal FinalValueFeePercentage,
    int ListingLimit,
    DateTime StartDate,
    DateTime? EndDate,
    SubscriptionStatus Status
);

public sealed class GetStoreByIdQueryValidator : AbstractValidator<GetStoreByIdQuery>
{
    public GetStoreByIdQueryValidator()
    {
        RuleFor(x => x.StoreId)
            .NotEmpty().WithMessage("Store ID là bắt buộc")
            .Must(id => Guid.TryParse(id, out _)).WithMessage("Store ID không hợp lệ");
    }
}

public sealed class GetStoreByIdQueryHandler : IQueryHandler<GetStoreByIdQuery, StoreDetailsDto>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IUserContext _userContext;

    public GetStoreByIdQueryHandler(
        IStoreRepository storeRepository,
        IUserContext userContext)
    {
        _storeRepository = storeRepository;
        _userContext = userContext;
    }

    public async Task<Result<StoreDetailsDto>> Handle(GetStoreByIdQuery request, CancellationToken cancellationToken)
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

        var subscriptionDto = new SubscriptionDetailsDto(
            activeSubscription.Id,
            activeSubscription.SubscriptionType,
            activeSubscription.MonthlyFee.Amount,
            activeSubscription.MonthlyFee.Currency,
            activeSubscription.FinalValueFeePercentage,
            activeSubscription.ListingLimit,
            activeSubscription.StartDate,
            activeSubscription.EndDate,
            activeSubscription.Status);

        var storeDto = new StoreDetailsDto(
            store.Id.Value,
            store.Name,
            store.Slug,
            store.Description,
            store.LogoUrl,
            store.BannerUrl,
            store.StoreType,
            store.IsActive,
            subscriptionDto);

        return storeDto;
    }
}

