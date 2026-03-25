using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Listings.Inventory.Dtos;
using PRN232_EbayClone.Domain.Listings.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Inventory.Queries.GetInventoryByListingId;

public sealed record GetInventoryByListingIdQuery(Guid ListingId) : IQuery<InventoryDto>;

public sealed class GetInventoryByListingIdQueryValidator : AbstractValidator<GetInventoryByListingIdQuery>
{
    public GetInventoryByListingIdQueryValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("Listing ID is required.");
    }
}

public sealed class GetInventoryByListingIdQueryHandler : IQueryHandler<GetInventoryByListingIdQuery, InventoryDto>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUserContext _userContext;

    public GetInventoryByListingIdQueryHandler(
        IInventoryRepository inventoryRepository,
        IUserContext userContext)
    {
        _inventoryRepository = inventoryRepository;
        _userContext = userContext;
    }

    public async Task<Result<InventoryDto>> Handle(GetInventoryByListingIdQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Unauthorized", "You are not authorized to access inventory.");
        }

        var inventory = await _inventoryRepository.GetByListingIdAsync(new ListingId(request.ListingId), cancellationToken);
        if (inventory is null)
        {
            return Error.Failure("Inventory.NotFound", "The inventory was not found.");
        }

        if (inventory.SellerId.Value != Guid.Parse(userId))
        {
            return ListingErrors.Unauthorized;
        }

        return inventory.ToDto();
    }
}