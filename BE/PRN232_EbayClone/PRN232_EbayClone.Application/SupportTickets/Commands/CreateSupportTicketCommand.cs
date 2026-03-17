using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.File;
using PRN232_EbayClone.Application.SupportTickets.Dtos;
using PRN232_EbayClone.Domain.SupportTickets.Entities;
using PRN232_EbayClone.Domain.SupportTickets.Enums;

namespace PRN232_EbayClone.Application.SupportTickets.Commands;

public sealed record CreateSupportTicketCommand(
    string Category,
    string Subject,
    string Message,
    string? Priority,
    IFormFileCollection? Attachments
) : ICommand<CreateSupportTicketResponse>;

public sealed class CreateSupportTicketCommandValidator : AbstractValidator<CreateSupportTicketCommand>
{
    public CreateSupportTicketCommandValidator()
    {
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage("Danh mục là bắt buộc")
            .Must(BeValidCategory)
            .WithMessage("Danh mục không hợp lệ");
        
        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("Tiêu đề là bắt buộc")
            .MaximumLength(SupportTicket.MaxSubjectLength)
            .WithMessage($"Tiêu đề không được vượt quá {SupportTicket.MaxSubjectLength} ký tự");
        
        RuleFor(x => x.Message)
            .NotEmpty()
            .WithMessage("Nội dung là bắt buộc")
            .MaximumLength(SupportTicket.MaxMessageLength)
            .WithMessage($"Nội dung không được vượt quá {SupportTicket.MaxMessageLength} ký tự");
        
        RuleFor(x => x.Priority)
            .Must(BeValidPriority)
            .When(x => !string.IsNullOrWhiteSpace(x.Priority))
            .WithMessage("Mức độ ưu tiên không hợp lệ");
        
        RuleFor(x => x.Attachments)
            .Must(attachments => attachments == null || attachments.Count <= 5)
            .WithMessage("Tối đa 5 tệp đính kèm");
        
        RuleForEach(x => x.Attachments)
            .Must(file => file.Length <= 10 * 1024 * 1024)
            .When(x => x.Attachments != null)
            .WithMessage("Mỗi tệp không được vượt quá 10MB");
    }

    private bool BeValidCategory(string category)
    {
        return Enum.TryParse<SupportTicketCategory>(category, true, out _);
    }

    private bool BeValidPriority(string? priority)
    {
        if (string.IsNullOrWhiteSpace(priority))
            return true;
        
        return Enum.TryParse<SupportTicketPriority>(priority, true, out _);
    }
}

public sealed class CreateSupportTicketCommandHandler : 
    ICommandHandler<CreateSupportTicketCommand, CreateSupportTicketResponse>
{
    private readonly ISupportTicketRepository _supportTicketRepository;
    private readonly IUserContext _userContext;
    private readonly IFileManager _fileManager;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSupportTicketCommandHandler(
        ISupportTicketRepository supportTicketRepository,
        IUserContext userContext,
        IFileManager fileManager,
        IUnitOfWork unitOfWork)
    {
        _supportTicketRepository = supportTicketRepository;
        _userContext = userContext;
        _fileManager = fileManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateSupportTicketResponse>> Handle(
        CreateSupportTicketCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_userContext.UserId))
        {
            return Error.Unauthorized("User is not authenticated");
        }

        var sellerId = Guid.Parse(_userContext.UserId);
        
        // Parse category and priority
        var category = Enum.Parse<SupportTicketCategory>(request.Category, true);
        var priority = string.IsNullOrWhiteSpace(request.Priority)
            ? SupportTicketPriority.Normal
            : Enum.Parse<SupportTicketPriority>(request.Priority, true);

        // Generate unique ticket number
        var ticketNumber = await GenerateTicketNumberAsync(cancellationToken);

        // Create ticket
        var ticketResult = SupportTicket.Create(
            ticketNumber,
            sellerId,
            category,
            request.Subject,
            request.Message,
            priority,
            DateTimeOffset.UtcNow,
            _userContext.UserId);

        if (ticketResult.IsFailure)
        {
            return ticketResult.Error;
        }

        var ticket = ticketResult.Value;
        _supportTicketRepository.Add(ticket);

        // Upload attachments if provided
        if (request.Attachments != null && request.Attachments.Count > 0)
        {
            var uploadResult = await _fileManager.UploadMultipleFilesAsync(request.Attachments);
            if (uploadResult.IsFailure)
            {
                return uploadResult.Error;
            }

            // TODO: Create SupportTicketAttachment records
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Send confirmation email to seller
        // TODO: Notify admin dashboard (real-time notification)
        // Domain event SupportTicketCreatedDomainEvent is raised in SupportTicket.Create method

        var response = new CreateSupportTicketResponse
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Status = ticket.Status.ToString(),
            CreatedAt = ticket.CreatedAt.DateTime
        };

        return Result.Success(response);
    }

    private async Task<string> GenerateTicketNumberAsync(CancellationToken cancellationToken)
    {
        var year = DateTime.UtcNow.Year;
        var lastTicketNumber = await _supportTicketRepository.GetLastTicketNumberForYearAsync(year, cancellationToken);
        
        int nextNumber = 1;
        if (!string.IsNullOrEmpty(lastTicketNumber))
        {
            // Extract number from format "TKT-YYYY-NNNNN"
            var parts = lastTicketNumber.Split('-');
            if (parts.Length == 3 && int.TryParse(parts[2], out var lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"TKT-{year}-{nextNumber:D5}";
    }
}
