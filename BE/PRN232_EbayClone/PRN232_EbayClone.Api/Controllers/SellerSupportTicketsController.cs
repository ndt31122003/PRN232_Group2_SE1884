using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Services;
using PRN232_EbayClone.Application.SupportTickets.Commands;
using PRN232_EbayClone.Application.SupportTickets.Dtos;
using PRN232_EbayClone.Application.SupportTickets.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/seller/tickets")]
public sealed class SellerSupportTicketsController(ISender sender, ICurrentUser currentUser) : ApiController(sender)
{
    [HttpPost]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB limit
    public async Task<IActionResult> CreateSupportTicket(
        [FromForm] CreateSupportTicketRequest request,
        CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Unauthorized();
        }

        var command = new CreateSupportTicketCommand(
            sellerId,
            request.Category,
            request.Subject,
            request.Message,
            request.Attachments);

        var result = await SendAsync(command, cancellationToken);
        
        if (result is OkObjectResult okResult && okResult.Value is { })
        {
            return CreatedAtAction(
                nameof(GetSupportTicketById),
                new { ticketId = okResult.Value },
                new { TicketId = okResult.Value });
        }

        return result;
    }

    [HttpGet]
    public async Task<IActionResult> GetSupportTickets(
        [FromQuery] SupportTicketFilterDto filter,
        CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Unauthorized();
        }

        var query = new GetSellerSupportTicketsQuery(sellerId, filter);
        return await SendAsync(query, cancellationToken);
    }

    [HttpGet("{ticketId}")]
    public async Task<IActionResult> GetSupportTicketById(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Unauthorized();
        }

        var query = new GetSupportTicketByIdQuery(ticketId, sellerId);
        return await SendAsync(query, cancellationToken);
    }

    [HttpPost("{ticketId}/close")]
    public async Task<IActionResult> CloseSupportTicket(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var sellerId = currentUser.UserId;
        if (string.IsNullOrEmpty(sellerId))
        {
            return Unauthorized();
        }

        var command = new CloseSupportTicketCommand(ticketId, sellerId);
        return await SendAsync(command, cancellationToken);
    }
}

public sealed record CreateSupportTicketRequest(
    string Category,
    string Subject,
    string Message,
    IFormFileCollection? Attachments = null
);