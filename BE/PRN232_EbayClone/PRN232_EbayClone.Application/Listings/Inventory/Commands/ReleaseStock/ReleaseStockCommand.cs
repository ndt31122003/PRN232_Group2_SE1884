using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Listings.Inventory.Dtos;
using PRN232_EbayClone.Domain.Listings.Inventory.Errors;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Inventory.Commands.ReleaseStock;

public sealed record ReleaseStockCommand(Guid ReservationId) : ICommand<InventoryDto>;

public sealed class ReleaseStockCommandValidator : AbstractValidator<ReleaseStockCommand>
{
    public ReleaseStockCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty().WithMessage("Reservation ID is required.");
    }
}

public sealed class ReleaseStockCommandHandler : ICommandHandler<ReleaseStockCommand, InventoryDto>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public ReleaseStockCommandHandler(
        IInventoryRepository inventoryRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<InventoryDto>> Handle(ReleaseStockCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Unauthorized", "You are not authorized to release stock.");
        }

        var inventory = await _inventoryRepository.GetByReservationIdAsync(new InventoryReservationId(request.ReservationId), cancellationToken);
        if (inventory is null)
        {
            return InventoryErrors.NotFound;
        }

        var actorId = Guid.Parse(userId);
        var reservation = inventory.Reservations.FirstOrDefault(x => x.Id == new InventoryReservationId(request.ReservationId));
        if (reservation is null)
        {
            return InventoryErrors.ReservationNotFound;
        }

        if (reservation.BuyerId.Value != actorId && inventory.SellerId.Value != actorId)
        {
            return Error.Failure("Unauthorized", "You are not authorized to release this reservation.");
        }

        var releaseResult = inventory.ReleaseStock(reservation.Quantity, reservation.Id);
        if (releaseResult.IsFailure)
        {
            return releaseResult.Error;
        }

        _inventoryRepository.Update(inventory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return inventory.ToDto();
    }
}