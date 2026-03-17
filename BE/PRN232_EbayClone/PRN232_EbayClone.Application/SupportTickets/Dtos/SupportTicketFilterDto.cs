namespace PRN232_EbayClone.Application.SupportTickets.Dtos;

public sealed record SupportTicketFilterDto
{
    public Guid? SellerId { get; init; }
    public string? Status { get; init; }
    public string? Category { get; init; }
    public string? Priority { get; init; }
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SortBy { get; init; } = "CreatedAt";
    public string? SortOrder { get; init; } = "DESC";
}
