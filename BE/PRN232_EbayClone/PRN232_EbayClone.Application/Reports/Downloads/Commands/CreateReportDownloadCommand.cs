using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PRN232_EbayClone.Application.Abstractions.File;
using PRN232_EbayClone.Application.Reports.Downloads.Abstractions;
using PRN232_EbayClone.Application.Reports.Downloads.Dtos;
using PRN232_EbayClone.Application.Reports.Downloads.Mappings;
using PRN232_EbayClone.Domain.Reports.Entities;
using PRN232_EbayClone.Domain.Reports.Enums;
using PRN232_EbayClone.Domain.Reports.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Reports.Downloads.Commands;

public sealed record CreateReportDownloadCommand(
    Guid UserId,
    string Source,
    string Type,
    DateTime? RangeStartUtc,
    DateTime? RangeEndUtc,
    string? TimeZone
) : ICommand<ReportDownloadDto>;

public sealed class CreateReportDownloadCommandValidator : AbstractValidator<CreateReportDownloadCommand>
{
    public CreateReportDownloadCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Source)
            .NotEmpty().WithMessage("Source is required.")
            .MaximumLength(64);

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MaximumLength(128);

        RuleFor(x => x.RangeEndUtc)
            .GreaterThanOrEqualTo(x => x.RangeStartUtc)
            .When(x => x.RangeStartUtc.HasValue && x.RangeEndUtc.HasValue)
            .WithMessage("End date must be after or equal to start date.");

        RuleFor(x => x.TimeZone)
            .MaximumLength(64)
            .When(x => !string.IsNullOrWhiteSpace(x.TimeZone));
    }
}

public sealed class CreateReportDownloadCommandHandler : ICommandHandler<CreateReportDownloadCommand, ReportDownloadDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IReportDownloadRepository _reportDownloadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportDownloadGenerator _reportDownloadGenerator;
    private readonly IFileManager _fileManager;
    private readonly ILogger<CreateReportDownloadCommandHandler> _logger;

    public CreateReportDownloadCommandHandler(
        IUserRepository userRepository,
        IReportDownloadRepository reportDownloadRepository,
        IUnitOfWork unitOfWork,
        IReportDownloadGenerator reportDownloadGenerator,
        IFileManager fileManager,
        ILogger<CreateReportDownloadCommandHandler> logger)
    {
        _userRepository = userRepository;
        _reportDownloadRepository = reportDownloadRepository;
        _unitOfWork = unitOfWork;
        _reportDownloadGenerator = reportDownloadGenerator;
        _fileManager = fileManager;
        _logger = logger;
    }

    public async Task<Result<ReportDownloadDto>> Handle(CreateReportDownloadCommand request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<ReportDownloadDto>(Error.Failure("ReportDownload.UserNotFound", "User not found."));
        }

        ReportDateRange? dateRange = null;
        if (request.RangeStartUtc.HasValue || request.RangeEndUtc.HasValue || !string.IsNullOrWhiteSpace(request.TimeZone))
        {
            var dateRangeResult = ReportDateRange.Create(request.RangeStartUtc, request.RangeEndUtc, request.TimeZone);
            if (dateRangeResult.IsFailure)
            {
                return Result.Failure<ReportDownloadDto>(dateRangeResult.Error);
            }

            dateRange = dateRangeResult.Value;
        }

        var createResult = ReportDownload.Create(
            userId,
            request.Source,
            request.Type,
            ReportDownloadStatus.Pending,
            DateTime.UtcNow,
            referenceCode: null,
            dateRange: dateRange);

        if (createResult.IsFailure)
        {
            return Result.Failure<ReportDownloadDto>(createResult.Error);
        }

        var download = createResult.Value;

        var generationResult = await _reportDownloadGenerator.GenerateAsync(
            request.UserId,
            request.Source,
            request.Type,
            dateRange,
            cancellationToken);

        if (generationResult.IsFailure)
        {
            _logger.LogWarning(
                "Report generation failed for user {UserId}, source {Source}, type {Type}: {Code}",
                request.UserId,
                request.Source,
                request.Type,
                generationResult.Error.Code);

            return Result.Failure<ReportDownloadDto>(generationResult.Error);
        }

        var generatedFile = generationResult.Value;

        // Upload the generated report so the client can download it immediately.
        using var memoryStream = new MemoryStream(generatedFile.ContentBytes, writable: false);
        memoryStream.Position = 0;

        var formFile = new FormFile(memoryStream, 0, memoryStream.Length, "report", generatedFile.FileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = generatedFile.ContentType
        };

        var uploadResult = await _fileManager.UploadFileAsync(formFile);

        if (uploadResult.IsFailure)
        {
            _logger.LogWarning(
                "Report upload failed for user {UserId}, source {Source}, type {Type}: {Code}",
                request.UserId,
                request.Source,
                request.Type,
                uploadResult.Error.Code);

            return Result.Failure<ReportDownloadDto>(uploadResult.Error);
        }

        var markCompletedResult = download.MarkCompleted(uploadResult.Value, DateTime.UtcNow);

        if (markCompletedResult.IsFailure)
        {
            _logger.LogWarning(
                "Failed to mark report download {DownloadId} as completed: {Code}",
                download.Id,
                markCompletedResult.Error.Code);

            return Result.Failure<ReportDownloadDto>(markCompletedResult.Error);
        }

        _reportDownloadRepository.Add(download);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success<ReportDownloadDto>(download.ToDto());
    }
}
