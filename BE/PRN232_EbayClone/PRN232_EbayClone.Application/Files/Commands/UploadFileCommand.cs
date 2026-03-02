using Microsoft.AspNetCore.Http;
using PRN232_EbayClone.Application.Abstractions.File;

namespace PRN232_EbayClone.Application.Files.Commands;

public sealed record UploadFileCommand(
    IFormFile File
) : ICommand<string>;

public sealed class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(x => x.File)
            .NotNull().WithMessage("File must be provided.")
            .Must(file => file.Length > 0).WithMessage("File must not be empty.");
    }
}

public sealed class UploadFileCommandHandler : ICommandHandler<UploadFileCommand, string>
{
    private readonly IFileManager _fileManager;

    public UploadFileCommandHandler(IFileManager fileManager)
    {
        _fileManager = fileManager;
    }

    public async Task<Result<string>> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var filePathOrError = await _fileManager.UploadFileAsync(request.File);
        if(filePathOrError.IsFailure)
            return filePathOrError.Error;

        return filePathOrError.Value;
    }
}


