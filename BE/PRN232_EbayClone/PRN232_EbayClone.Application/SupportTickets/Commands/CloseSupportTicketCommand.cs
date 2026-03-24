using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.SupportTickets.Enums;

namespace PRN232_EbayClone.Application.SupportTickets.Commands;

public sealed record CloseSupportTicketCommand(
    Guid TicketId,
    string SellerId
) : ICommand;

internal sealed class CloseSupportTicketCommandHandler(
    ISupportTicketRepository supportTicketRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CloseSupportTicketCommand>
{
    public async Task<Result> Handle(CloseSupportTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await supportTicketRepository.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket == null)
        {
            return Error.NotFound("SupportTicket.NotFound", "Không tìm thấy support ticket");
        }

        if (ticket.SellerId != request.SellerId)
        {
            return Error.Unauthorized("Bạn không có quyền truy cập ticket này");
        }

        var updateResult = ticket.UpdateStatus(TicketStatus.Closed.ToString());
        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        supportTicketRepository.Update(ticket);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}