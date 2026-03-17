using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Mappings;
using PRN232_EbayClone.Domain.Disputes.Entities;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record AddDisputeMessageCommand(
    Guid DisputeId,
    string Message
) : ICommand<DisputeMessageDto>;

public sealed class AddDisputeMessageCommandValidator : AbstractValidator<AddDisputeMessageCommand>
{
    public AddDisputeMessageCommandValidator()
    {
        RuleFor(x => x.DisputeId)
            .NotEmpty()
            .WithMessage("Dispute ID là bắt buộc");
        
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Tin nhắn là bắt buộc")
            .MaximumLength(DisputeMessage.MaxMessageLength)
            .WithMessage($"Tin nhắn không được vượt quá {DisputeMessage.MaxMessageLength} ký tự");
    }
}

public sealed class AddDisputeMessageCommandHandler : ICommandHandler<AddDisputeMessageCommand, DisputeMessageDto>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IDisputeMessageRepository _disputeMessageRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddDisputeMessageCommandHandler(
        IDisputeRepository disputeRepository,
        IDisputeMessageRepository disputeMessageRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _disputeMessageRepository = disputeMessageRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DisputeMessageDto>> Handle(
        AddDisputeMessageCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_userContext.UserId))
        {
            return DisputeErrors.Unauthorized;
        }

        var dispute = await _disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);
        if (dispute is null)
        {
            return DisputeErrors.NotFound;
        }

        // Check if dispute is closed
        if (dispute.IsClosed)
        {
            return DisputeErrors.CannotUpdate;
        }

        // Determine sender role
        var userId = Guid.Parse(_userContext.UserId);
        SenderRole senderRole;
        
        if (dispute.BuyerId == userId)
        {
            senderRole = SenderRole.Buyer;
        }
        else if (dispute.SellerId == userId)
        {
            senderRole = SenderRole.Seller;
        }
        else
        {
            // Assume admin if not buyer or seller
            senderRole = SenderRole.Admin;
        }

        // Create message
        var messageResult = DisputeMessage.Create(
            request.DisputeId,
            userId,
            senderRole,
            request.Message,
            DateTimeOffset.UtcNow,
            _userContext.UserId);

        if (messageResult.IsFailure)
        {
            return messageResult.Error;
        }

        var message = messageResult.Value;
        _disputeMessageRepository.Add(message);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Map to DTO
        var messageDto = message.ToDto();
        return Result.Success(messageDto);
    }
}
