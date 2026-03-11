using PRN232_EbayClone.Application.Reports.Schedules.Dtos;
using PRN232_EbayClone.Application.Reports.Schedules.Mappings;
using PRN232_EbayClone.Domain.Reports.Entities;
using PRN232_EbayClone.Domain.Reports.Enums;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Reports.Schedules.Commands;

public sealed record CreateReportScheduleCommand(
    Guid UserId,
    string Source,
    string Type,
    ReportScheduleFrequency Frequency,
    DateTime? EndDateUtc,
    string? DeliveryEmail
) : ICommand<ReportScheduleDto>;

public sealed class CreateReportScheduleCommandValidator : AbstractValidator<CreateReportScheduleCommand>
{
    public CreateReportScheduleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Source)
            .NotEmpty().WithMessage("Source is required.")
            .MaximumLength(64);

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MaximumLength(128);

        RuleFor(x => x.DeliveryEmail)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.DeliveryEmail))
            .WithMessage("Delivery email is invalid.")
            .MaximumLength(256);

        RuleFor(x => x.EndDateUtc)
            .Must(endDate => !endDate.HasValue || endDate.Value.Date >= DateTime.UtcNow.Date)
            .WithMessage("End date cannot be in the past.");
    }
}

public sealed class CreateReportScheduleCommandHandler : ICommandHandler<CreateReportScheduleCommand, ReportScheduleDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IReportScheduleRepository _reportScheduleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReportScheduleCommandHandler(
        IUserRepository userRepository,
        IReportScheduleRepository reportScheduleRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _reportScheduleRepository = reportScheduleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ReportScheduleDto>> Handle(CreateReportScheduleCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<ReportScheduleDto>(Error.Failure("ReportSchedule.UserNotFound", "User not found."));
        }

        var alreadyExists = await _reportScheduleRepository.ExistsAsync(
            request.UserId,
            request.Source,
            request.Type,
            cancellationToken);

        if (alreadyExists)
        {
            return Result.Failure<ReportScheduleDto>(Error.Failure("ReportSchedule.Duplicate", "An active schedule already exists for this source and type."));
        }

        var createResult = ReportSchedule.Create(
            userId,
            request.Source,
            request.Type,
            request.Frequency,
            DateTime.UtcNow,
            request.EndDateUtc,
            request.DeliveryEmail);

        if (createResult.IsFailure)
        {
            return Result.Failure<ReportScheduleDto>(createResult.Error);
        }

        var schedule = createResult.Value;
        _reportScheduleRepository.Add(schedule);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success<ReportScheduleDto>(schedule.ToDto());
    }
}
