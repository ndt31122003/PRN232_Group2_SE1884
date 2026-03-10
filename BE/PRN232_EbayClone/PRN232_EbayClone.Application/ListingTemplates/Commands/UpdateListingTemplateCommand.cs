using System;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.ListingTemplates.Errors;
using System.Text.Json;

namespace PRN232_EbayClone.Application.ListingTemplates.Commands;

public sealed record UpdateListingTemplateCommand(
    Guid Id,
    string Name,
    string? Description,
    string? FormatLabel,
    string? ThumbnailUrl,
    JsonElement Payload
) : ICommand;

public sealed class UpdateListingTemplateCommandHandler : ICommandHandler<UpdateListingTemplateCommand>
{
    private readonly IListingTemplateRepository _templateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public UpdateListingTemplateCommandHandler(IListingTemplateRepository templateRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _templateRepository = templateRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(UpdateListingTemplateCommand request, CancellationToken cancellationToken)
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

        var template = await _templateRepository.GetByIdForOwnerAsync(request.Id, userId, cancellationToken);
        if (template is null)
        {
            var existing = await _templateRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existing is null)
            {
                return ListingTemplateErrors.NotFound;
            }

            if (!string.Equals(existing.CreatedBy, userId, StringComparison.OrdinalIgnoreCase))
            {
                return ListingTemplateErrors.Unauthorized;
            }

            template = existing;
        }

        if (await _templateRepository.NameExistsAsync(userId, request.Name, request.Id, cancellationToken))
        {
            return ListingTemplateErrors.DuplicateName;
        }

        var payloadJson = request.Payload.GetRawText();
        var resolvedFormatLabel = ListingTemplateFormatResolver.Resolve(payloadJson);

        var updateResult = template.Update(
            request.Name,
            payloadJson,
            request.Description,
            resolvedFormatLabel,
            request.ThumbnailUrl);

        if (updateResult.IsFailure)
        {
            return updateResult.Error;
        }

        template.UpdatedBy = userId;

        _templateRepository.Update(template);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
