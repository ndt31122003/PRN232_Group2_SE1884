using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.File;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Commands;

public sealed record UploadDisputeEvidenceCommand(
    Guid DisputeId,
    IEnumerable<IFormFile> Files
) : ICommand<UploadDisputeEvidenceCommandResult>;

public sealed record UploadDisputeEvidenceCommandResult(
    IEnumerable<string> FileUrls
);

public sealed class UploadDisputeEvidenceCommandValidator : AbstractValidator<UploadDisputeEvidenceCommand>
{
    public UploadDisputeEvidenceCommandValidator()
    {
        RuleFor(x => x.DisputeId).NotEmpty().WithMessage("Dispute ID là bắt buộc");
        RuleFor(x => x.Files)
            .NotEmpty().WithMessage("Files không được để trống");
        RuleForEach(x => x.Files)
            .Must(file => file.Length > 0).WithMessage("File không được để trống")
            .Must(file => file.Length <= 10 * 1024 * 1024).WithMessage("File không được vượt quá 10MB");
    }
}

public sealed class UploadDisputeEvidenceCommandHandler : 
    ICommandHandler<UploadDisputeEvidenceCommand, UploadDisputeEvidenceCommandResult>
{
    private readonly IDisputeRepository _disputeRepository;
    private readonly IUserContext _userContext;
    private readonly IFileManager _fileManager;
    private readonly IUnitOfWork _unitOfWork;

    public UploadDisputeEvidenceCommandHandler(
        IDisputeRepository disputeRepository,
        IUserContext userContext,
        IFileManager fileManager,
        IUnitOfWork unitOfWork)
    {
        _disputeRepository = disputeRepository;
        _userContext = userContext;
        _fileManager = fileManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UploadDisputeEvidenceCommandResult>> Handle(
        UploadDisputeEvidenceCommand request,
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

        // Verify seller ownership
        if (dispute.SellerId.ToString() != _userContext.UserId)
        {
            return DisputeErrors.Unauthorized;
        }

        // Upload files
        var uploadResult = await _fileManager.UploadMultipleFilesAsync(request.Files);
        if (uploadResult.IsFailure)
        {
            return uploadResult.Error;
        }

        var fileUrls = uploadResult.Value.Select(f => f.Url).ToList();

        // Update dispute status to WaitingBuyer after providing evidence
        var statusResult = dispute.ProvideEvidence(_userContext.UserId);
        if (statusResult.IsFailure)
        {
            return statusResult.Error;
        }

        _disputeRepository.Update(dispute);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // TODO: Create DisputeEvidence records for each file
        // TODO: Raise DisputeEvidenceUploadedDomainEvent for notifications

        return Result.Success(new UploadDisputeEvidenceCommandResult(fileUrls));
    }
}


