using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Services;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Orders.Commands;
using PRN232_EbayClone.Application.Orders.Dtos;
using PRN232_EbayClone.Application.Orders.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/orders")]
public class OrdersController : ApiController
{
    private readonly IUserContext _userContext;
    public OrdersController(ISender sender, IUserContext userContext) : base(sender)
    {
        _userContext = userContext;
    }
    [HttpGet]
    public async Task<IActionResult> GetOrderById([FromQuery] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery(id);
        var result = await SendAsync(query, cancellationToken);
        return result;
    }
    [HttpGet("all-orders")]
    public async Task<IActionResult> GetAllOrders([FromQuery] OrderFilterDto filter, CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var userId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }
        var query = new GetOrdersQuery(userId, filter);
        var result = await SendAsync(query, cancellationToken);
        return result;
    }

    [HttpGet("cancellations")]
    public async Task<IActionResult> GetCancellationRequests(
        [FromQuery] CancellationFilterDto? filter,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var query = new GetCancellationRequestsQuery(sellerId, filter ?? new CancellationFilterDto());
        var result = await SendAsync(query, cancellationToken);
        return result;
    }

    [HttpGet("cancellations/{cancellationRequestId:guid}")]
    public async Task<IActionResult> GetCancellationRequestById(
        Guid cancellationRequestId,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var query = new GetCancellationRequestByIdQuery(sellerId, cancellationRequestId);
        var result = await SendAsync(query, cancellationToken);
        return result;
    }

    [HttpGet("returns")]
    public async Task<IActionResult> GetReturnRequests(
        [FromQuery] ReturnFilterDto? filter,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var query = new GetReturnRequestsQuery(sellerId, filter ?? new ReturnFilterDto());
        var result = await SendAsync(query, cancellationToken);
        return result;
    }

    [HttpGet("returns/{returnRequestId:guid}")]
    public async Task<IActionResult> GetReturnRequestById(
        Guid returnRequestId,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var query = new GetReturnRequestByIdQuery(sellerId, returnRequestId);
        var result = await SendAsync(query, cancellationToken);
        return result;
    }

    [HttpGet("returns/{returnRequestId:guid}/order")]
    public async Task<IActionResult> GetOrderByReturnRequest(
        Guid returnRequestId,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var query = new GetOrderByReturnRequestIdQuery(sellerId, returnRequestId);
        var result = await SendAsync(query, cancellationToken);
        return result;
    }

    [HttpGet("cancellations/{cancellationRequestId:guid}/order")]
    public async Task<IActionResult> GetOrderByCancellationRequest(
        Guid cancellationRequestId,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var query = new GetOrderByCancellationRequestIdQuery(sellerId, cancellationRequestId);
        var result = await SendAsync(query, cancellationToken);
        return result;
    }

    [HttpGet("all-statuses")]
    public async Task<IActionResult> GetAllStatuses(CancellationToken cancellationToken)
    {
        var query = new GetOrderStatusesQuery();
        var result = await SendAsync(query, cancellationToken);
        return result;

    }

    [HttpGet("shipping-labels")]
    public async Task<IActionResult> GetShippingLabels([FromQuery] ShippingLabelFilterDto filter, CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var limit = filter?.Limit ?? 50;
        var query = new GetShippingLabelsQuery(
            sellerId,
            filter?.FromDate,
            filter?.ToDate,
            limit);

        var result = await SendAsync(query, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/shipping-label/preview")]
    public async Task<IActionResult> PreviewShippingLabel(
        Guid orderId,
        [FromBody] PrintShippingLabelRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new PreviewShippingLabelCommand(
            orderId,
            sellerId,
            request.Carrier,
            request.ServiceCode,
            request.ServiceName,
            request.PackageType,
            request.WeightOz,
            request.LengthIn,
            request.WidthIn,
            request.HeightIn,
            request.ShipDate,
            request.LabelFormat,
            request.LabelPaperSize,
            request.PageWidthIn,
            request.PageHeightIn);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/shipping-label")]
    public async Task<IActionResult> PrintShippingLabel(
        Guid orderId,
        [FromBody] PrintShippingLabelRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new PrintShippingLabelCommand(
            orderId,
            sellerId,
            request.Carrier,
            request.ServiceCode,
            request.ServiceName,
            request.PackageType,
            request.WeightOz,
            request.LengthIn,
            request.WidthIn,
            request.HeightIn,
            request.ShipDate,
            request.PaymentMethod,
            request.LabelFormat,
            request.LabelPaperSize,
            request.PageWidthIn,
            request.PageHeightIn);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("bulk/shipping-labels")]
    public async Task<IActionResult> BulkPrintShippingLabels(
        [FromBody] BulkPrintShippingLabelsRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new BulkPrintShippingLabelsCommand(
            sellerId,
            request.OrderIds ?? Array.Empty<Guid>(),
            request.Carrier,
            request.ServiceCode,
            request.ServiceName ?? string.Empty,
            request.PackageType,
            request.WeightOz,
            request.LengthIn,
            request.WidthIn,
            request.HeightIn,
            request.ShipDate,
            request.PaymentMethod,
            request.LabelFormat,
            request.LabelPaperSize,
            request.PageWidthIn,
            request.PageHeightIn);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/cancellations")]
    public async Task<IActionResult> CreateCancellationRequest(
        Guid orderId,
        [FromBody] CreateCancellationRequestRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new CreateCancellationRequestCommand(
            orderId,
            sellerId,
            request.Reason,
            request.SellerNote);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("cancellations/{cancellationRequestId:guid}/approve")]
    public async Task<IActionResult> ApproveCancellationRequest(
        Guid cancellationRequestId,
        [FromBody] UpdateCancellationDecisionRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new ApproveCancellationRequestCommand(
            cancellationRequestId,
            sellerId,
            request.SellerNote);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("cancellations/{cancellationRequestId:guid}/reject")]
    public async Task<IActionResult> RejectCancellationRequest(
        Guid cancellationRequestId,
        [FromBody] UpdateCancellationDecisionRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new RejectCancellationRequestCommand(
            cancellationRequestId,
            sellerId,
            request.SellerNote);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("returns/{returnRequestId:guid}/approve")]
    public async Task<IActionResult> ApproveReturnRequest(
        Guid returnRequestId,
        [FromBody] UpdateReturnDecisionRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new ApproveReturnRequestCommand(
            returnRequestId,
            sellerId,
            request.BuyerReturnDueAtUtc,
            request.SellerNote);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("returns/{returnRequestId:guid}/reject")]
    public async Task<IActionResult> RejectReturnRequest(
        Guid returnRequestId,
        [FromBody] RejectReturnRequestRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new RejectReturnRequestCommand(
            returnRequestId,
            sellerId,
            request.SellerNote);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/confirm")]
    public async Task<IActionResult> ConfirmOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new ConfirmOrderCommand(orderId, sellerId);
        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/delivery-status")]
    public async Task<IActionResult> UpdateDeliveryStatus(
        Guid orderId,
        [FromBody] UpdateOrderDeliveryStatusRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new UpdateOrderDeliveryStatusCommand(
            orderId,
            sellerId,
            request.Outcome,
            request.Note);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/shipping-labels/{labelId:guid}/void")]
    public async Task<IActionResult> VoidShippingLabel(
        Guid orderId,
        Guid labelId,
        [FromBody] VoidShippingLabelRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new VoidShippingLabelCommand(orderId, labelId, sellerId, request?.Reason);
        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/feedback")]
    public async Task<IActionResult> LeaveBuyerFeedback(
        Guid orderId,
        [FromBody] LeaveBuyerFeedbackRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new LeaveBuyerFeedbackCommand(
            orderId,
            sellerId,
            request.UseStoredFeedback,
            request.Comment,
            request.StoredFeedbackKey);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("bulk/feedback")]
    public async Task<IActionResult> BulkLeaveBuyerFeedback(
        [FromBody] BulkLeaveBuyerFeedbackRequest request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new BulkLeaveBuyerFeedbackCommand(
            sellerId,
            request.OrderIds ?? Array.Empty<Guid>(),
            request.UseStoredFeedback,
            request.Comment,
            request.StoredFeedbackKey);

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/shipments")]
    public async Task<IActionResult> UpsertItemShipments(
        Guid orderId,
        [FromBody] UpsertOrderItemShipmentsRequest? request,
        CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var shipments = request?.Shipments ?? Array.Empty<OrderItemShipmentDraft>();
        var cleared = request?.ClearedOrderItemIds ?? Array.Empty<Guid>();

        var command = new UpsertOrderItemShipmentsCommand(
            orderId,
            sellerId,
            shipments.ToList(),
            cleared.ToList());

        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/mark-as-shipped")]
    public async Task<IActionResult> MarkAsShipped(Guid orderId, CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new MarkOrderAsShippedCommand(orderId, sellerId);
        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("{orderId:guid}/archive")]
    public async Task<IActionResult> ArchiveOrder(Guid orderId, CancellationToken cancellationToken)
    {
        var unauthorizedResult = TryGetAuthenticatedSeller(out var sellerId);
        if (unauthorizedResult is not null)
        {
            return unauthorizedResult;
        }

        var command = new ArchiveOrderCommand(orderId, sellerId);
        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    [HttpPost("buy-it-now")]
    public async Task<IActionResult> BuyItNow([FromBody] BuyItNowRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var buyerId))
        {
            return Unauthorized(new ProblemDetails
            {
                Title = "Lỗi xác thực",
                Detail = "Người dùng chưa được xác thực",
                Status = StatusCodes.Status401Unauthorized
            });
        }

        var command = new BuyItNowCommand(request.ListingId, request.Quantity, buyerId);
        var result = await SendAsync(command, cancellationToken);
        return result;
    }

    private IActionResult? TryGetAuthenticatedSeller(out Guid sellerId)
    {
        sellerId = Guid.Empty;

        if (Guid.TryParse(_userContext.UserId, out sellerId))
        {
            return null;
        }

        return Unauthorized(new ProblemDetails
        {
            Title = "Lỗi xác thực",
            Detail = "Người dùng chưa được xác thực",
            Status = StatusCodes.Status401Unauthorized
        });
    }

}

public sealed record BuyItNowRequest(Guid ListingId, int Quantity = 1);
