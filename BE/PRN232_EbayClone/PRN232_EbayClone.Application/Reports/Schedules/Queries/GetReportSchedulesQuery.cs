using System.Linq;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Reports.Schedules.Dtos;
using PRN232_EbayClone.Application.Reports.Schedules.Mappings;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Reports.Schedules.Queries;

public sealed record GetReportSchedulesQuery(
    Guid UserId,
    ReportScheduleFilterDto Filter) : IQuery<PagingResult<ReportScheduleDto>>;

public sealed class GetReportSchedulesQueryValidator : AbstractValidator<GetReportSchedulesQuery>
{
    public GetReportSchedulesQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Filter)
            .NotNull().WithMessage("Filter is required.");

        When(x => x.Filter is not null, () =>
        {
            RuleFor(x => x.Filter!.PageNumber)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than zero.");

            RuleFor(x => x.Filter!.PageSize)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than zero.");
        });
    }
}

public sealed class GetReportSchedulesQueryHandler : IQueryHandler<GetReportSchedulesQuery, PagingResult<ReportScheduleDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IReportScheduleRepository _reportScheduleRepository;

    public GetReportSchedulesQueryHandler(
        IUserRepository userRepository,
        IReportScheduleRepository reportScheduleRepository)
    {
        _userRepository = userRepository;
        _reportScheduleRepository = reportScheduleRepository;
    }

    public async Task<Result<PagingResult<ReportScheduleDto>>> Handle(GetReportSchedulesQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<PagingResult<ReportScheduleDto>>(Error.Failure("ReportSchedule.UserNotFound", "User not found."));
        }

        var filter = request.Filter ?? new ReportScheduleFilterDto();
        var sanitizedPageNumber = filter.GetSanitizedPageNumber();
        var sanitizedPageSize = filter.GetSanitizedPageSize();

        var sanitizedFilter = new ReportScheduleFilterDto
        {
            Source = filter.Source,
            Type = filter.Type,
            Frequency = filter.Frequency,
            OnlyActive = filter.OnlyActive,
            PageNumber = sanitizedPageNumber,
            PageSize = sanitizedPageSize
        };

        var (schedules, totalCount) = await _reportScheduleRepository.GetSchedulesAsync(
            request.UserId,
            sanitizedFilter,
            cancellationToken);

        var items = schedules.Select(schedule => schedule.ToDto()).ToList();

        var result = new PagingResult<ReportScheduleDto>(items, totalCount, sanitizedPageNumber, sanitizedPageSize);

        return Result.Success<PagingResult<ReportScheduleDto>>(result);
    }
}
