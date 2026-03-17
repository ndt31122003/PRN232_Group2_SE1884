using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.SupportTickets.Dtos;
using PRN232_EbayClone.Domain.SupportTickets.Errors;

namespace PRN232_EbayClone.Application.SupportTickets.Queries;

public sealed record GetSupportTicketByIdQuery(
    Guid TicketId
) : IQuery<SupportTicketDetailDto>;

public sealed class GetSupportTicketByIdQueryValidator : AbstractValidator<GetSupportTicketByIdQuery>
{
    public GetSupportTicketByIdQueryValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty().WithMessage("Ticket ID là bắt buộc");
    }
}

public sealed class GetSupportTicketByIdQueryHandler : IQueryHandler<GetSupportTicketByIdQuery, SupportTicketDetailDto>
{
    private readonly ISupportTicketRepository _supportTicketRepository;

    public GetSupportTicketByIdQueryHandler(ISupportTicketRepository supportTicketRepository)
    {
        _supportTicketRepository = supportTicketRepository;
    }

    public async Task<Result<SupportTicketDetailDto>> Handle(
        GetSupportTicketByIdQuery request,
        CancellationToken cancellationToken)
    {
        var ticketDetail = await _supportTicketRepository.GetTicketDetailByIdAsync(
            request.TicketId,
            cancellationToken);

        if (ticketDetail is null)
        {
            return SupportTicketErrors.NotFound;
        }

        return Result.Success(ticketDetail);
    }
}
