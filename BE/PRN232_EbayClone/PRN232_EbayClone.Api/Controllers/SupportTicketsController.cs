using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.SupportTickets.Commands;
using PRN232_EbayClone.Application.SupportTickets.Dtos;
using PRN232_EbayClone.Application.SupportTickets.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/support-tickets")]
public sealed class SupportTicketsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public Task<IActionResult> GetSupportTickets(
        [FromQuery] SupportTicketFilterDto filter,
        CancellationToken cancellationToken)
    {
        var query = new GetSupportTicketsQuery(filter);
        return SendAsync(query, cancellationToken);
    }

    [HttpGet("{ticketId}")]
    public Task<IActionResult> GetSupportTicketById(
        Guid ticketId,
        CancellationToken cancellationToken)
    {
        var query = new GetSupportTicketByIdQuery(ticketId);
        return SendAsync(query, cancellationToken);
    }

    [HttpPost]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50MB limit for attachments
    public async Task<IActionResult> CreateSupportTicket(
        [FromForm] CreateSupportTicketRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateSupportTicketCommand(
            request.Category,
            request.Subject,
            request.Message,
            request.Priority,
            request.Attachments);

        return await SendAsync(command, cancellationToken);
    }

    [HttpPost("{ticketId}/responses")]
    public Task<IActionResult> AddTicketResponse(
        Guid ticketId,
        [FromBody] AddTicketResponseRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddTicketResponseCommand(ticketId, request.Message);
        return SendAsync(command, cancellationToken);
    }
}
