using System.Linq;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Reports.Downloads.Dtos;
using PRN232_EbayClone.Application.Reports.Downloads.Mappings;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Reports.Downloads.Queries;

public sealed record GetReportDownloadsQuery(
    Guid UserId,
    ReportDownloadFilterDto Filter) : IQuery<PagingResult<ReportDownloadDto>>;

public sealed class GetReportDownloadsQueryValidator : AbstractValidator<GetReportDownloadsQuery>
{
    public GetReportDownloadsQueryValidator()
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

public sealed class GetReportDownloadsQueryHandler : IQueryHandler<GetReportDownloadsQuery, PagingResult<ReportDownloadDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IReportDownloadRepository _reportDownloadRepository;

    public GetReportDownloadsQueryHandler(
        IUserRepository userRepository,
        IReportDownloadRepository reportDownloadRepository)
    {
        _userRepository = userRepository;
        _reportDownloadRepository = reportDownloadRepository;
    }

    public async Task<Result<PagingResult<ReportDownloadDto>>> Handle(GetReportDownloadsQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<PagingResult<ReportDownloadDto>>(Error.Failure("ReportDownload.UserNotFound", "User not found."));
        }

        var filter = request.Filter ?? new ReportDownloadFilterDto();
        var sanitizedPageNumber = filter.GetSanitizedPageNumber();
        var sanitizedPageSize = filter.GetSanitizedPageSize();

        var sanitizedFilter = new ReportDownloadFilterDto
        {
            Source = filter.Source,
            Status = filter.Status,
            FromUtc = filter.FromUtc,
            ToUtc = filter.ToUtc,
            PageNumber = sanitizedPageNumber,
            PageSize = sanitizedPageSize
        };

        var (downloads, totalCount) = await _reportDownloadRepository.GetDownloadsAsync(
            request.UserId,
            sanitizedFilter,
            cancellationToken);

        var items = downloads.Select(download => download.ToDto()).ToList();

        var result = new PagingResult<ReportDownloadDto>(items, totalCount, sanitizedPageNumber, sanitizedPageSize);

        return Result.Success<PagingResult<ReportDownloadDto>>(result);
    }
}
