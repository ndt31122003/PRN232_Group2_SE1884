using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Listings.Inventory.Dtos;
using PRN232_EbayClone.Domain.Listings.Inventory.Enums;
using PRN232_EbayClone.Domain.Listings.Inventory.Errors;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Listings.Inventory.Commands.ReserveStock;

public sealed record ReserveStockCommand(
    Guid ListingId,
    int Quantity,
    DateTime? ExpiresAt = null,
    InventoryReservationType ReservationType = InventoryReservationType.BuyItNow,
    Guid? OrderId = null) : ICommand<ReserveStockResult>;

public sealed record ReserveStockResult(Guid ReservationId, InventoryDto Inventory);

public sealed class ReserveStockCommandValidator : AbstractValidator<ReserveStockCommand>
{
    public ReserveStockCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("Listing ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

        RuleFor(x => x.ReservationType)
            .IsInEnum().WithMessage("Reservation type is invalid.");

        RuleFor(x => x.ExpiresAt)
            .Must(x => !x.HasValue || x.Value > DateTime.UtcNow)
            .WithMessage("Expiration time must be in the future.");
    }
}

public sealed class ReserveStockCommandHandler : ICommandHandler<ReserveStockCommand, ReserveStockResult>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public ReserveStockCommandHandler(
        IInventoryRepository inventoryRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ReserveStockResult>> Handle(ReserveStockCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Error.Failure("Unauthorized", "You are not authorized to reserve stock.");
        }

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var inventory = await _inventoryRepository.GetByListingIdForUpdateAsync(new ListingId(request.ListingId), cancellationToken);
        if (inventory is null)
        {
            return InventoryErrors.NotFound;
        }

        var buyerId = new UserId(Guid.Parse(userId));
        var expiresAt = request.ExpiresAt ?? DateTime.UtcNow.AddMinutes(30);

        var reserveResult = inventory.ReserveStock(request.Quantity, buyerId, expiresAt, request.ReservationType, request.OrderId);
        if (reserveResult.IsFailure)
        {
            return reserveResult.Error;
        }

        _inventoryRepository.Update(inventory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        var reservation = inventory.Reservations
            .Where(x => x.BuyerId == buyerId && x.IsActive && x.Quantity == request.Quantity && x.OrderId == request.OrderId)
            .OrderByDescending(x => x.ReservedAt)
            .First();

        return new ReserveStockResult(reservation.Id.Value, inventory.ToDto());
    }
}