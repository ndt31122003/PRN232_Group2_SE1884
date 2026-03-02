using PRN232_EbayClone.Application.Research.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Research.Commands;

public sealed record SaveSourcingCategoryCommand(Guid UserId, string CategoryId) : ICommand;

public sealed class SaveSourcingCategoryCommandHandler(
    ISourcingInsightsRepository Repository) : ICommandHandler<SaveSourcingCategoryCommand>
{
    public async Task<Result> Handle(SaveSourcingCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CategoryId))
        {
            return Error.Failure("Research.Sourcing.InvalidCategory", "Category id is required.");
        }

        var categories = await Repository.GetAllCategoriesAsync(cancellationToken);
        if (categories.All(category => !string.Equals(category.Id, request.CategoryId, StringComparison.OrdinalIgnoreCase)))
        {
            return Error.Failure("Research.Sourcing.CategoryNotFound", "The requested category was not found.");
        }

        await Repository.SaveCategoryAsync(request.UserId, request.CategoryId, cancellationToken);
        return Result.Success();
    }
}

public sealed record RemoveSourcingCategoryCommand(Guid UserId, string CategoryId) : ICommand;

public sealed class RemoveSourcingCategoryCommandHandler(
    ISourcingInsightsRepository Repository) : ICommandHandler<RemoveSourcingCategoryCommand>
{
    public async Task<Result> Handle(RemoveSourcingCategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.CategoryId))
        {
            return Error.Failure("Research.Sourcing.InvalidCategory", "Category id is required.");
        }

        await Repository.RemoveCategoryAsync(request.UserId, request.CategoryId, cancellationToken);
        return Result.Success();
    }
}
