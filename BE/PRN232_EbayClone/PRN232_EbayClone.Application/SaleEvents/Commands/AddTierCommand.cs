using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Discounts.Entities;
using PRN232_EbayClone.Domain.Discounts.Enums;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record AddTierCommand(
    Guid SaleEventId,
    SaleEventDiscountType DiscountType,
    decimal DiscountValue,
    int Priority,
    string? Label
) : ICommand<Guid>;

public sealed class AddTierCommandValidator : AbstractValidator<AddTierCommand>
{
    public AddTierCommandValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEmpty()
            .WithMessage("Sale event ID is required");

        RuleFor(x => x.Priority)
            .GreaterThan(0)
            .WithMessage("Tier priority must be greater than 0");

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0)
            .WithMessage("Discount value must be greater than 0");

        RuleFor(x => x.DiscountValue)
            .InclusiveBetween(0.01m, 90m)
            .When(x => x.DiscountType == SaleEventDiscountType.Percent)
            .WithMessage("Percentage discount must be between 0.01 and 90");

        RuleFor(x => x.Label)
            .MaximumLength(100)
            .When(x => x.Label != null)
            .WithMessage("Tier label cannot exceed 100 characters");
    }
}

public sealed class AddTierCommandHandler : ICommandHandler<AddTierCommand, Guid>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public AddTierCommandHandler(
        ISaleEventRepository saleEventRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddTierCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerGuid))
        {
            return new Error("SaleEvent.Unauthorized", "User is not authorized");
        }

        var saleEvent = await _saleEventRepository.GetByIdAsync(request.SaleEventId, cancellationToken);
        if (saleEvent == null)
        {
            return new Error("SaleEvent.NotFound", "Sale event not found");
        }

        // Verify ownership
        if (saleEvent.SellerId != sellerGuid)
        {
            return new Error("SaleEvent.Unauthorized", "User does not own this sale event");
        }

        try
        {
            var tier = SaleEventDiscountTier.Create(
                request.SaleEventId,
                request.DiscountType,
                request.DiscountValue,
                request.Priority,
                request.Label);

            saleEvent.AddTier(tier);

            await _saleEventRepository.UpdateAsync(saleEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(tier.Id);
        }
        catch (ArgumentException ex)
        {
            return new Error("SaleEvent.ValidationFailed", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return new Error("SaleEvent.OperationFailed", ex.Message);
        }
    }
}
