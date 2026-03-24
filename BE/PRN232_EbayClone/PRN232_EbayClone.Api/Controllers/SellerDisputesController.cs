using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Services;
using PRN232_EbayClone.Application.Disputes.Commands;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/seller/disputes")]
public sealed class SellerDisputesController(ISender sender, ICurrentUser currentUser) : ApiController(sender)
{
    [HttpGet]
    public async Task<IActionResult> GetSellerDisputes(
        [FromQuery] SellerDisputeFilterDto filter,
        CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Unauthorized();
        }

        var query = new GetSellerDisputesQuery(sellerId, filter);
        return await SendAsync(query, cancellationToken);
    }

    [HttpGet("{disputeId}")]
    public async Task<IActionResult> GetSellerDisputeById(
        Guid disputeId,
        CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Unauthorized();
        }

        var query = new GetSellerDisputeByIdQuery(disputeId, sellerId);
        return await SendAsync(query, cancellationToken);
    }

    [HttpPost("{disputeId}/accept-refund")]
    public async Task<IActionResult> AcceptRefund(
        Guid disputeId,
        CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Unauthorized();
        }

        var command = new AcceptRefundCommand(disputeId, sellerId);
        return await SendAsync(command, cancellationToken);
    }

    [HttpPost("{disputeId}/provide-evidence")]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB limit
    public async Task<IActionResult> ProvideEvidence(
        Guid disputeId,
        [FromForm] List<IFormFile> files,
        CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Unauthorized();
        }

        if (files == null || files.Count == 0)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "File upload error",
                Detail = "Files không được để trống.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var formFileCollection = new FormFileCollection();
        formFileCollection.AddRange(files);

        var command = new ProvideEvidenceCommand(disputeId, sellerId, formFileCollection);
        return await SendAsync(command, cancellationToken);
    }

    [HttpPost("{disputeId}/escalate")]
    public async Task<IActionResult> EscalateDispute(
        Guid disputeId,
        CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Unauthorized();
        }

        var command = new EscalateSellerDisputeCommand(disputeId, sellerId);
        return await SendAsync(command, cancellationToken);
    }
}