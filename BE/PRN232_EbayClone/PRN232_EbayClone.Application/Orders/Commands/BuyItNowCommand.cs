using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.Enums;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Entities;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Orders.Commands;

public sealed record BuyItNowCommand(Guid ListingId, int Quantity, Guid BuyerId) : ICommand<Guid>;

public sealed class BuyItNowCommandHandler : ICommandHandler<BuyItNowCommand, Guid>
{
    private readonly IListingRepository _listingRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BuyItNowCommandHandler(
        IListingRepository listingRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _listingRepository = listingRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(BuyItNowCommand request, CancellationToken cancellationToken)
    {
        var listing = await _listingRepository.GetByIdAsync(request.ListingId, cancellationToken);
        if (listing is null)
        {
            return Result.Failure<Guid>(Error.Failure("Listing.NotFound", "Listing not found."));
        }

        if (listing.Status != ListingStatus.Active)
        {
            return Result.Failure<Guid>(Error.Failure("Listing.NotActive", "Cannot buy a listing that is not active."));
        }

        if (!Guid.TryParse(listing.CreatedBy, out var sellerId))
        {
            return Result.Failure<Guid>(Error.Failure("Listing.InvalidSeller", "Cannot determine seller of the listing."));
        }

        //if (sellerId == request.BuyerId)
        //{
        //    return Result.Failure<Guid>(Error.Failure("Listing.CannotBuyOwn", "You cannot buy your own listing."));
        //}

        decimal price;
        if (listing is FixedPriceListing fp)
        {
            if (fp.Type != ListingType.Single)
            {
                return Result.Failure<Guid>(Error.Failure("Listing.Unsupported", "Multi-variation Buy It Now is not supported in this mock."));
            }
            price = fp.Pricing.Price;
        }
        else if (listing is AuctionListing al)
        {
            if (al.Pricing.BuyItNowPrice is null || al.Pricing.BuyItNowPrice <= 0)
            {
                return Result.Failure<Guid>(Error.Failure("Listing.NoBuyItNow", "Auction does not have a Buy It Now price."));
            }
            price = al.Pricing.BuyItNowPrice.Value;
        }
        else
        {
            return Result.Failure<Guid>(Error.Failure("Listing.Unsupported", "Unsupported listing format."));
        }

        var awaitingShipmentStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.AwaitingShipment, cancellationToken);
        if (awaitingShipmentStatus is null)
        {
            return Result.Failure<Guid>(Error.Failure("Order.StatusNotFound", "Awaiting shipment status not found."));
        }

        // Generate order identifiers without going through EF Core
        // (EF Core has a known conflict with OwnsOne + HasData seeding for brand-new Order inserts)
        var orderId = Guid.NewGuid();
        var orderNumber = Order.GenerateOrderNumber();
        var orderItemId = Guid.NewGuid();

        var imageUrl = listing.Images.FirstOrDefault(i => i.IsPrimary)?.Url ?? listing.Images.FirstOrDefault()?.Url ?? string.Empty;

        await _orderRepository.InsertBuyItNowOrderAsync(
            orderId: orderId,
            orderNumber: orderNumber,
            buyerId: request.BuyerId,
            sellerId: sellerId,
            statusId: awaitingShipmentStatus.Id,
            amount: price,
            currency: "USD",
            orderItemId: orderItemId,
            listingId: listing.Id,
            categoryId: listing.CategoryId,
            imageUrl: imageUrl,
            title: listing.Title,
            sku: listing.Sku,
            quantity: request.Quantity,
            cancellationToken: cancellationToken);

        return Result.Success(orderId);
    }
}
