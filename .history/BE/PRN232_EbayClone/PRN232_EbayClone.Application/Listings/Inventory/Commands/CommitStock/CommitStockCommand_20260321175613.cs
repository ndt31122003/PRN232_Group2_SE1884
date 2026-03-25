using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Listings.Inventory.Dtos;
using PRN232_EbayClone.Domain.Listings.Inventory.Errors;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Inventory.Commands.CommitStock;

public sealed record CommitStockCommand(Guid ReservationId) : ICommand<InventoryDto>;

public sealed class CommitStockCommandValidator : AbstractValidator<CommitStockCommand>
{
    public CommitStockCommandValidator()
    {
        RuleFor(x => x.ReservationId)
            .NotEmpty().WithMessage("Reservation ID is required.");
    }
}

public sealed class CommitStockCommandHandler : ICommandHandler<CommitStockCommand, InventoryDto>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CommitStockCommandHandler(
        IInventoryRepository inventoryRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<InventoryDto>> Handle(CommitStockCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Unauthorized", "You are not authorized to commit stock.");
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
            return Error.Failure("Unauthorized", "You are not authorized to commit this reservation.");
        }

        var commitResult = inventory.CommitStock(reservation.Quantity, reservation.Id);
        if (commitResult.IsFailure)
        {
            return commitResult.Error;
        }

        _inventoryRepository.Update(inventory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return inventory.ToDto();
    }
}