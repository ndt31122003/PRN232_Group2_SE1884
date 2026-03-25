using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Orders.Constants;
using PRN232_EbayClone.Domain.Orders.Events;
using PRN232_EbayClone.Domain.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Orders.EventHandlers
{
    public class CancellationApprovedHandler : INotificationHandler<CancellationApprovedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;
        public CancellationApprovedHandler(
            IOrderRepository orderRepository,
            IInventoryRepository inventoryRepository,
            IUserContext userContext,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _inventoryRepository = inventoryRepository;
            _userContext = userContext;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(CancellationApprovedEvent notification, CancellationToken cancellationToken)
        {
            var targetStatus = await _orderRepository.GetStatusByCodeAsync(OrderStatusCodes.Cancelled, cancellationToken);
            if (targetStatus is null)
            {
                throw new InvalidOperationException("CancellationApprovedHandler: target status 'Cancelled' was not found.");
            }

            var order = await _orderRepository.GetByIdAsync(notification.OrderId, cancellationToken);
            if (order is null)
            {
                throw new InvalidOperationException("CancellationApprovedHandler: order was not found.");
            }

            var result = order.ChangeStatus(targetStatus, OrderRoles.System);
            if (result.IsFailure)
            {
                throw new InvalidOperationException("Failed to change order status to Cancelled: " + result.Error);
            }

            foreach (var item in order.Items)
            {
                var inventory = await _inventoryRepository.GetByListingIdAsync(new ListingId(item.ListingId), cancellationToken);
                if (inventory is null)
                {
                    continue;
                }

                var reservation = inventory.Reservations
                    .Where(r => r.IsActive && r.Quantity == item.Quantity)
                    .OrderByDescending(r => r.OrderId == notification.OrderId)
                    .ThenByDescending(r => r.ReservedAt)
                    .FirstOrDefault(r => r.OrderId == notification.OrderId || (r.OrderId is null && r.BuyerId == order.BuyerId));

                if (reservation is null)
                {
                    continue;
                }

                var releaseResult = inventory.ReleaseStock(reservation.Quantity, reservation.Id);
                if (releaseResult.IsFailure)
                {
                    throw new InvalidOperationException("Failed to release inventory reservation for cancelled order: " + releaseResult.Error);
                }

                _inventoryRepository.Update(inventory);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
