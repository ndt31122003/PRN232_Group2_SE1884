using System;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.ListingTemplates.Entities;
using PRN232_EbayClone.Domain.ListingTemplates.Errors;

namespace PRN232_EbayClone.Application.ListingTemplates.Commands;

public sealed record DeleteListingTemplateCommand(Guid Id) : ICommand;

public sealed class DeleteListingTemplateCommandHandler : ICommandHandler<DeleteListingTemplateCommand>
{
    private readonly IListingTemplateRepository _templateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;

    public DeleteListingTemplateCommandHandler(IListingTemplateRepository templateRepository, IUnitOfWork unitOfWork, IUserContext userContext)
    {
        _templateRepository = templateRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(DeleteListingTemplateCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        if (string.IsNullOrWhiteSpace(userId))
        {
            return ListingTemplateErrors.Unauthorized;
        }

        ListingTemplate? template = await _templateRepository.GetByIdForOwnerAsync(request.Id, userId, cancellationToken);
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

        _templateRepository.Remove(template);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
