using PRN232_EbayClone.Application.Files.Commands;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/files")]
public sealed class FilesController(ISender sender) : ApiController(sender)
{
    [HttpPost("upload")]
    public Task<IActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        => SendAsync(new UploadFileCommand(file), cancellationToken);

    [HttpPost("upload-multiple")]
    public Task<IActionResult> UploadMultipleFiles([FromForm] UploadMultipleFilesCommand command, CancellationToken cancellationToken)
        => SendAsync(command, cancellationToken);
}
