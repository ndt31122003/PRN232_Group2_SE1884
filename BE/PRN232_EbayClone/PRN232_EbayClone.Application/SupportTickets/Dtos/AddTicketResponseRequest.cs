namespace PRN232_EbayClone.Application.SupportTickets.Dtos;

public sealed record AddTicketResponseRequest
{
    public string Message { get; init; } = string.Empty;
}
