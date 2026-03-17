using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Disputes.Commands;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Queries;
using PRN232_EbayClone.Domain.Disputes.Enums;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/disputes")]
public sealed class DisputesController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public Task<IActionResult> GetDisputes([FromQuery] DisputeFilterDto filter, CancellationToken cancellationToken)
    {
        var query = new GetDisputesQuery(filter);
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

    [HttpPost("{disputeId}/accept-refund")]
    public Task<IActionResult> AcceptRefund(
        Guid disputeId,
        [FromBody] AcceptRefundRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AcceptRefundCommand(disputeId, request.RefundAmount, request.Message);
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("{disputeId}/messages")]
    public Task<IActionResult> AddDisputeMessage(
        Guid disputeId,
        [FromBody] AddDisputeMessageRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddDisputeMessageCommand(disputeId, request.Message);
        return SendAsync(command, cancellationToken);
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
