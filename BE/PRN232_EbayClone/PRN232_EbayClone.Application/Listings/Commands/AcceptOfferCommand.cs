using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Orders.Enums;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Listings.Commands;

public sealed record AcceptOfferCommand(
    Guid ListingId,
    decimal OfferAmount,
    Guid BuyerId) : ICommand<Guid>;

public sealed class AcceptOfferCommandHandler : ICommandHandler<AcceptOfferCommand, Guid>
{
    private readonly IListingRepository _listingRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptOfferCommandHandler(
        IListingRepository listingRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _listingRepository = listingRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AcceptOfferCommand request, CancellationToken cancellationToken)
    {
        // 1. Get Listing
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
        {
            return Result.Failure<Guid>(Error.Failure("Listing.NotFound", "Listing not found."));
        }

        // 2. Parse SellerId from CreatedBy
        if (!Guid.TryParse(listing.CreatedBy, out var sellerId))
        {
            // Fallback for demo if CreatedBy is not a GUID (e.g. "Alice")
            // In a real system, this should always be a GUID string.
            // Using a demo seller ID as fallback if needed.
            sellerId = Guid.Parse("70000000-0000-0000-0000-000000000001"); 
        }

        // 3. Get Status
        var awaitingShipmentStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.AwaitingShipment, cancellationToken);
        if (awaitingShipmentStatus is null)
        {
            return Result.Failure<Guid>(Error.Failure("OrderStatus.NotFound", "AwaitingShipment status not found."));
        }

        // 4. Create Order
        var orderResult = Order.CreateDraft(
            new UserId(request.BuyerId),
            sellerId,
            FulfillmentType.SellerShips,
            "USD",
            awaitingShipmentStatus
        );

        if (orderResult.IsFailure)
        {
            return Result.Failure<Guid>(orderResult.Error);
        }

        var order = orderResult.Value;

        // 5. Create OrderItem
        var imageUrl = listing.Images.FirstOrDefault(i => i.IsPrimary)?.Url 
                       ?? listing.Images.FirstOrDefault()?.Url 
                       ?? "";
        
        var moneyResult = Money.Create(request.OfferAmount, "USD");
        if (moneyResult.IsFailure) return Result.Failure<Guid>(moneyResult.Error);

        var orderItem = OrderItem.Create(
            listing.Id,
            null, // VariationId
            listing.CategoryId,
            imageUrl,
            listing.Title,
            listing.Sku,
            1, // Quantity
            moneyResult.Value
        );

        var addItemResult = order.AddItem(orderItem);
        if (addItemResult.IsFailure)
        {
            return Result.Failure<Guid>(addItemResult.Error);
        }

        // 6. Save Order
        _orderRepository.Add(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order.Id);
    }
}
