using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.File;
using PRN232_EbayClone.Domain.FileMetadata.Entities;

namespace PRN232_EbayClone.Application.Files.Commands;

public sealed record UploadMultipleFilesCommand(
    Guid LinkedEntityId,
    IEnumerable<IFormFile> Files
) : ICommand<UploadMultipleFilesCommandResult>;

public sealed record UploadMultipleFilesCommandResult(
    IEnumerable<string> FileUploadResults
);

public sealed class UploadMultipleFilesCommandValidator : AbstractValidator<UploadMultipleFilesCommand>
{
    public UploadMultipleFilesCommandValidator()
    {
        RuleFor(x => x.LinkedEntityId)
            .NotEmpty()
            .WithMessage("LinkedEntityId không được để trống");
        RuleFor(x => x.Files)
            .NotEmpty()
            .WithMessage("Files không được để trống");
        RuleForEach(x => x.Files)
            .Must(file => file.Length > 0)
            .WithMessage("File không được để trống");
    }
}

public sealed class UploadMultipleFilesCommandHandler :
    ICommandHandler<UploadMultipleFilesCommand, UploadMultipleFilesCommandResult>
{
    private readonly IFileManager _fileManager;
    private readonly IFileMetadataRepository _fileMetadataRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadMultipleFilesCommandHandler(
        IFileManager fileManager,
        IFileMetadataRepository fileMetadataRepository,
        IUnitOfWork unitOfWork)
    {
        _fileManager = fileManager;
        _fileMetadataRepository = fileMetadataRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UploadMultipleFilesCommandResult>> Handle(
        UploadMultipleFilesCommand request,
        CancellationToken cancellationToken)
    {
        var fileMetadatasOrError = await _fileManager.UploadMultipleFilesAsync(request.Files);
        if (fileMetadatasOrError.IsFailure)
            return fileMetadatasOrError.Error;

        var fileMetadatas = fileMetadatasOrError.Value;

        return new UploadMultipleFilesCommandResult(fileMetadatas.Select(x => x.Url));
    }
}

