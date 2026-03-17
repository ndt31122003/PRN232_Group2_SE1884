using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.SupportTickets.Dtos;
using PRN232_EbayClone.Domain.SupportTickets.Entities;
using PRN232_EbayClone.Domain.SupportTickets.Enums;
using PRN232_EbayClone.Domain.SupportTickets.Errors;

namespace PRN232_EbayClone.Application.SupportTickets.Commands;

public sealed record AddTicketResponseCommand(
    Guid TicketId,
    string Message
) : ICommand<SupportTicketResponseDto>;

public sealed class AddTicketResponseCommandValidator : AbstractValidator<AddTicketResponseCommand>
{
    public AddTicketResponseCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty()
            .WithMessage("Ticket ID là bắt buộc");
        
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Nội dung phản hồi là bắt buộc")
            .MaximumLength(SupportTicket.MaxMessageLength)
            .WithMessage($"Nội dung không được vượt quá {SupportTicket.MaxMessageLength} ký tự");
    }
}

public sealed class AddTicketResponseCommandHandler : 
    ICommandHandler<AddTicketResponseCommand, SupportTicketResponseDto>
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly ISupportTicketResponseRepository _responseRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddTicketResponseCommandHandler(
        ISupportTicketRepository supportTicketRepository,
        ISupportTicketResponseRepository responseRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _supportTicketRepository = supportTicketRepository;
        _responseRepository = responseRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SupportTicketResponseDto>> Handle(
        AddTicketResponseCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_userContext.UserId))
        {
            return Error.Unauthorized("User is not authenticated");
        }

        var ticket = await _supportTicketRepository.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket is null)
        {
            return SupportTicketErrors.NotFound;
        }

        // Check if ticket is closed
        if (ticket.IsClosed)
        {
            return SupportTicketErrors.Closed;
        }

        // Verify seller ownership
        var userId = Guid.Parse(_userContext.UserId);
        if (ticket.SellerId != userId)
        {
            return Error.Unauthorized("You are not authorized to respond to this ticket");
        }

        // Create response
        var responseResult = SupportTicketResponse.Create(
            request.TicketId,
            userId,
            ResponderRole.Seller,
            request.Message,
            false, // Not an internal note
            DateTimeOffset.UtcNow,
            _userContext.UserId);

        if (responseResult.IsFailure)
        {
            return responseResult.Error;
        }

        var response = responseResult.Value;
        _responseRepository.Add(response);

        // Raise domain event for response added
        ticket.RaiseResponseAddedEvent(
            response.Id,
            response.ResponderId,
            response.ResponderRole.ToString(),
            response.RespondedAt);

        // If ticket status is WAITING_SELLER, update to PENDING
        if (ticket.Status == SupportTicketStatus.WaitingSeller)
        {
            var statusResult = ticket.UpdateStatus(SupportTicketStatus.Pending, _userContext.UserId);
            if (statusResult.IsFailure)
            {
                return statusResult.Error;
            }
            
            _supportTicketRepository.Update(ticket);
        }
        else
        {
            // Update ticket to trigger domain event persistence
            _supportTicketRepository.Update(ticket);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Notify assigned admin (if any)

        var responseDto = new SupportTicketResponseDto
        {
            Id = response.Id,
            ResponderId = response.ResponderId,
            ResponderName = string.Empty, // Would need to load from User
            ResponderRole = response.ResponderRole.ToString(),
            Message = response.Message,
            RespondedAt = response.RespondedAt.DateTime
        };

        return Result.Success(responseDto);
    }
}
