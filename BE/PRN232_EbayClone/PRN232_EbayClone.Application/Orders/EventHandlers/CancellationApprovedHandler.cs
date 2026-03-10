using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.Authentication;
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
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _unitOfWork;
        public CancellationApprovedHandler(IOrderRepository orderRepository, IUserContext userContext, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
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

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
