using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Listings.Errors;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record RemoveListingCommand(
    Guid ListingId
) : ICommand;

public sealed class RemoveListingCommandHandler : ICommandHandler<RemoveListingCommand>
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public RemoveListingCommandHandler(
        IUnitOfWork unitOfWork,
        IListingRepository listingRepository,
        IUserContext userContext)
    {
        _unitOfWork = unitOfWork;
        _listingRepository = listingRepository;
        _userContext = userContext;
    }

    public async Task<Result> Handle(RemoveListingCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingErrors.Unauthorized;
        }

        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null) 
            return ListingErrors.NotFound;

        if (!string.Equals(listing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
        {
            return ListingErrors.Unauthorized;
        }

        _listingRepository.Remove(listing);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}