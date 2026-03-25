using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Research.Dtos;
using PRN232_EbayClone.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IOrderRepository : IRepository<Order, Guid>
{
    Task<(List<Order> Orders, int TotalCount)> GetOrdersByUserIdAsync(
        Guid userId,
        OrderFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetOrdersForSellerAsync(
        Guid sellerId,
        DateTime fromUtc,
        DateTime toUtc,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetByIdsAsync(
        IEnumerable<Guid> orderIds,
        CancellationToken cancellationToken = default);

    Task<List<OrderStatus>> GetAllOrderStatusesAsync(CancellationToken cancellationToken = default);
    Task<OrderStatus?> GetStatusByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ProductResearchOrderItemRecord>> GetProductResearchOrderItemsAsync(
        Guid? sellerId,
        DateTime fromUtc,
        DateTime toUtc,
        string? keyword,
        Guid? categoryId,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<CancellationSummaryRecord> Records, int TotalCount)> GetCancellationRequestsAsync(
        Guid sellerId,
        CancellationFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<CancellationSummaryRecord?> GetCancellationRequestAsync(
        Guid sellerId,
        Guid cancellationRequestId,
        CancellationToken cancellationToken = default);

    Task<Order?> GetOrderByCancellationRequestIdAsync(
        Guid sellerId,
        Guid cancellationRequestId,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ReturnRequestSummaryRecord> Records, int TotalCount)> GetReturnRequestsAsync(
        Guid sellerId,
        ReturnFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ReturnRequestSummaryRecord?> GetReturnRequestAsync(
        Guid sellerId,
        Guid returnRequestId,
        CancellationToken cancellationToken = default);

    Task<Order?> GetOrderByReturnRequestIdAsync(
        Guid sellerId,
        Guid returnRequestId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Order>> GetAwaitingShipmentOrdersAsync(
        CancellationToken cancellationToken = default);

    /// <summary>Inserts an order directly via Dapper to bypass EF Core owned-entity tracking issues.</summary>
    Task InsertBuyItNowOrderAsync(
        Guid orderId, string orderNumber, Guid buyerId, Guid sellerId,
        Guid statusId, decimal amount, string currency,
        Guid orderItemId, Guid listingId, Guid? categoryId, string imageUrl,
        string title, string? sku, int quantity,
        CancellationToken cancellationToken = default);
}
