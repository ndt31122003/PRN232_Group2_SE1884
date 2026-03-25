using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Services;
using PRN232_EbayClone.Application.Disputes.Commands;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Queries;
using PRN232_EbayClone.Domain.Disputes.Enums;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/disputes")]
public sealed class DisputesController : ApiController
{
    private readonly ICurrentUser _currentUser;

    public DisputesController(ISender sender, ICurrentUser currentUser) : base(sender)
    {
        _currentUser = currentUser;
    }

    [HttpGet]
    public Task<IActionResult> GetDisputes([FromQuery] DisputeFilterDto filter, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId;
        Console.WriteLine($"[DisputesController] GetDisputes called by user: {currentUserId}");
        Console.WriteLine($"[DisputesController] Filter: Status={filter.Status}, PageNumber={filter.PageNumber}, PageSize={filter.PageSize}");
        
        var query = new GetDisputesQuery(filter, currentUserId!);
        return SendAsync(query, cancellationToken);
    }

    [HttpPost]
    public Task<IActionResult> CreateDispute(
        [FromBody] CreateDisputeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDisputeCommand(request.ListingId, request.Reason);
        return SendAsync(command, cancellationToken);
    }

    [HttpPut("{disputeId}/status")]
    public Task<IActionResult> UpdateDisputeStatus(
        Guid disputeId,
        [FromBody] UpdateDisputeStatusRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDisputeStatusCommand(disputeId, request.Status);
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("{disputeId}/resolve")]
    public Task<IActionResult> ResolveDispute(
        Guid disputeId,
        CancellationToken cancellationToken)
    {
        var command = new ResolveDisputeCommand(disputeId);
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("{disputeId}/close")]
    public Task<IActionResult> CloseDispute(
        Guid disputeId,
        CancellationToken cancellationToken)
    {
        var command = new CloseDisputeCommand(disputeId);
        return SendAsync(command, cancellationToken);
    }

    [HttpGet("{disputeId}")]
    public Task<IActionResult> GetDisputeById(
        Guid disputeId,
        CancellationToken cancellationToken)
    {
        var query = new GetDisputeByIdQuery(disputeId);
        return SendAsync(query, cancellationToken);
    }

    [HttpPost("{disputeId}/respond")]
    public Task<IActionResult> RespondToDispute(
        Guid disputeId,
        [FromBody] RespondToDisputeRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RespondToDisputeCommand(disputeId, request.Message);
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("{disputeId}/escalate")]
    public Task<IActionResult> EscalateDispute(
        Guid disputeId,
        CancellationToken cancellationToken)
    {
        var command = new EscalateDisputeCommand(disputeId);
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("{disputeId}/evidence")]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB limit
    public async Task<IActionResult> UploadEvidence(
        Guid disputeId,
        [FromForm] IFormFileCollection files,
        CancellationToken cancellationToken)
    {
        if (files == null || files.Count == 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "File upload error",
                Detail = "Files không được để trống.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var command = new UploadDisputeEvidenceCommand(disputeId, files);
        return await SendAsync(command, cancellationToken);
    }

    [HttpPost("{disputeId}/accept-evidence")]
    public async Task<IActionResult> BuyerAcceptEvidence(
        Guid disputeId,
        CancellationToken cancellationToken)
    {
        var buyerId = _currentUser.UserId;
        if (string.IsNullOrEmpty(buyerId))
        {
            return Unauthorized();
        }

        var command = new BuyerAcceptEvidenceCommand(disputeId, buyerId);
        return await SendAsync(command, cancellationToken);
    }

    [HttpPost("{disputeId}/request-refund")]
    public async Task<IActionResult> BuyerRequestRefund(
        Guid disputeId,
        [FromBody] BuyerRequestRefundRequest? request,
        CancellationToken cancellationToken)
    {
        var buyerId = _currentUser.UserId;
        if (string.IsNullOrEmpty(buyerId))
        {
            return Unauthorized();
        }

        var command = new BuyerRequestRefundCommand(disputeId, buyerId, request?.Message);
        return await SendAsync(command, cancellationToken);
    }
}

public sealed record CreateDisputeRequest(
    Guid ListingId,
    string Reason
);

public sealed record UpdateDisputeStatusRequest(
    string Status
);

public sealed record RespondToDisputeRequest(
    string Message
);

public sealed record BuyerRequestRefundRequest(
    string? Message
);
