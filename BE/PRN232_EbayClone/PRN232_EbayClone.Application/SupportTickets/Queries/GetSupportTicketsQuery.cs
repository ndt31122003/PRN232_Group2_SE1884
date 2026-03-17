using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.SupportTickets.Dtos;

namespace PRN232_EbayClone.Application.SupportTickets.Queries;

public sealed record GetSupportTicketsQuery(
    SupportTicketFilterDto Filter
) : IQuery<SupportTicketListResponse>;

public sealed class GetSupportTicketsQueryValidator : AbstractValidator<GetSupportTicketsQuery>
{
    public GetSupportTicketsQueryValidator()
    {
        RuleFor(x => x.Filter).NotNull().WithMessage("Filter không được để trống");
        RuleFor(x => x.Filter.PageNumber).GreaterThan(0).WithMessage("Page number phải lớn hơn 0");
        RuleFor(x => x.Filter.PageSize).InclusiveBetween(1, 200).WithMessage("Page size phải từ 1 đến 200");
    }
}

public sealed class GetSupportTicketsQueryHandler : IQueryHandler<GetSupportTicketsQuery, SupportTicketListResponse>
{
    private readonly ISupportTicketRepository _supportTicketRepository;

    public GetSupportTicketsQueryHandler(ISupportTicketRepository supportTicketRepository)
    {
        _supportTicketRepository = supportTicketRepository;
    }

    public async Task<Result<SupportTicketListResponse>> Handle(
        GetSupportTicketsQuery request,
        CancellationToken cancellationToken)
    {
        var (tickets, totalCount, openCount, pendingCount) = await _supportTicketRepository.GetSupportTicketsAsync(
            request.Filter,
            cancellationToken);

        var response = new SupportTicketListResponse
        {
            Tickets = tickets,
            TotalCount = totalCount,
            PageNumber = request.Filter.PageNumber,
            PageSize = request.Filter.PageSize,
            OpenCount = openCount,
            PendingCount = pendingCount
        };

        return Result.Success(response);
    }
}
