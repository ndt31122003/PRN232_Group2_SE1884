using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Stores.Enums;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Stores.Queries;

public sealed record GetUserStoresQuery() : IQuery<List<StoreSummaryDto>>;

public sealed record StoreSummaryDto(
    Guid StoreId,
    string Name,
    string Slug,
    string? LogoUrl,
    string? BannerUrl,
    StoreType StoreType,
    bool IsActive,
    DateTime CreatedAt
);

public sealed class GetUserStoresQueryHandler : IQueryHandler<GetUserStoresQuery, List<StoreSummaryDto>>
{
    private readonly IStoreRepository _storeRepository;
    private readonly IUserContext _userContext;

    public GetUserStoresQueryHandler(
        IStoreRepository storeRepository,
        IUserContext userContext)
    {
        _storeRepository = storeRepository;
        _userContext = userContext;
    }

    public async Task<Result<List<StoreSummaryDto>>> Handle(GetUserStoresQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
            return StoreErrors.Unauthorized;

        var user = await _storeRepository.GetByUserIdAsync(new UserId(Guid.Parse(userId)), cancellationToken);

        var stores = user.Select(store => new StoreSummaryDto(
            store.Id.Value,
            store.Name,
            store.Slug,
            store.LogoUrl,
            store.BannerUrl,
            store.StoreType,
            store.IsActive,
            store.CreatedAt)).ToList();

        return stores;
    }
}

