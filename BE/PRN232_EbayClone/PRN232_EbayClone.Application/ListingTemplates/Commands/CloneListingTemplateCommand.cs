using System;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.ListingTemplates;
using PRN232_EbayClone.Domain.ListingTemplates.Entities;
using PRN232_EbayClone.Domain.ListingTemplates.Errors;

namespace PRN232_EbayClone.Application.ListingTemplates.Commands;

public sealed record CloneListingTemplateCommand(Guid Id, string? NameOverride) : ICommand<CloneListingTemplateResult>;

public sealed record CloneListingTemplateResult(Guid Id);

public sealed class CloneListingTemplateCommandHandler : ICommandHandler<CloneListingTemplateCommand, CloneListingTemplateResult>
{
    private readonly IListingTemplateRepository _templateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public CloneListingTemplateCommandHandler(IListingTemplateRepository templateRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _templateRepository = templateRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result<CloneListingTemplateResult>> Handle(CloneListingTemplateCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingTemplateErrors.Unauthorized;
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

        var existingCount = await _templateRepository.CountByOwnerAsync(userId, cancellationToken);
        if (existingCount >= ListingTemplateConstants.MaxTemplatesPerSeller)
        {
            return ListingTemplateErrors.MaximumLimitReached(ListingTemplateConstants.MaxTemplatesPerSeller);
        }

        var newName = string.IsNullOrWhiteSpace(request.NameOverride)
            ? GenerateCloneName(template.Name)
            : request.NameOverride.Trim();

        if (await _templateRepository.NameExistsAsync(userId, newName, null, cancellationToken))
        {
            return ListingTemplateErrors.DuplicateName;
        }

        var payloadJson = template.PayloadJson;
        var resolvedFormatLabel = ListingTemplateFormatResolver.Resolve(payloadJson);

        var cloneResult = ListingTemplate.Create(
            newName,
            payloadJson,
            template.Description,
            resolvedFormatLabel,
            template.ThumbnailUrl);

        if (cloneResult.IsFailure)
        {
            return cloneResult.Error;
        }

        var clone = cloneResult.Value;
        clone.CreatedAt = DateTime.UtcNow;
        clone.CreatedBy = userId;

        _templateRepository.Add(clone);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new CloneListingTemplateResult(clone.Id));
    }

    private static string GenerateCloneName(string originalName)
    {
        return originalName.EndsWith(" copy", StringComparison.OrdinalIgnoreCase)
            ? $"{originalName} (1)"
            : $"{originalName} copy";
    }
}
