using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.ListingTemplates;
using PRN232_EbayClone.Domain.ListingTemplates.Entities;
using PRN232_EbayClone.Domain.ListingTemplates.Errors;
using System.Text.Json;

namespace PRN232_EbayClone.Application.ListingTemplates.Commands;

public sealed record CreateListingTemplateCommand(
    string Name,
    string? Description,
    string? FormatLabel,
    string? ThumbnailUrl,
    JsonElement Payload
) : ICommand<CreateListingTemplateResult>;

public sealed record CreateListingTemplateResult(Guid Id);

public sealed class CreateListingTemplateCommandHandler : ICommandHandler<CreateListingTemplateCommand, CreateListingTemplateResult>
{
    private readonly IListingTemplateRepository _templateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CreateListingTemplateCommandHandler(IListingTemplateRepository templateRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _templateRepository = templateRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<CreateListingTemplateResult>> Handle(CreateListingTemplateCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingTemplateErrors.Unauthorized;
        }

        if (request.Payload.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            return Error.Failure("ListingTemplate.InvalidPayload", "Template payload must be provided.");
        }

        var ownedCount = await _templateRepository.CountByOwnerAsync(userId, cancellationToken);
        if (ownedCount >= ListingTemplateConstants.MaxTemplatesPerSeller)
        {
            return ListingTemplateErrors.MaximumLimitReached(ListingTemplateConstants.MaxTemplatesPerSeller);
        }

        if (await _templateRepository.NameExistsAsync(userId, request.Name, null, cancellationToken))
        {
            return ListingTemplateErrors.DuplicateName;
        }

        var payloadJson = request.Payload.GetRawText();
        var resolvedFormatLabel = ListingTemplateFormatResolver.Resolve(payloadJson);

        var createResult = ListingTemplate.Create(
            request.Name,
            payloadJson,
            request.Description,
            resolvedFormatLabel,
            request.ThumbnailUrl);

        if (createResult.IsFailure)
        {
            return createResult.Error;
        }

        var template = createResult.Value;
        template.CreatedAt = DateTime.UtcNow;
        template.CreatedBy = userId;

        _templateRepository.Add(template);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new CreateListingTemplateResult(template.Id));
    }
}
