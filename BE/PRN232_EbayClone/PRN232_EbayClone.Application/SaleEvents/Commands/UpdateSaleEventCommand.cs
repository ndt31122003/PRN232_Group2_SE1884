using PRN232_EbayClone.Domain.SaleEvents.Enums;

namespace PRN232_EbayClone.Application.SaleEvents.Commands;

public sealed record UpdateSaleEventCommand(
    Guid SaleEventId,
    string Name,
    string? Description,
    SaleEventMode Mode,
    DateTime StartDate,
    DateTime EndDate,
    bool OfferFreeShipping,
    bool IncludeSkippedItems,
    bool BlockPriceIncreaseRevisions,
    decimal? HighlightPercentage,
    IReadOnlyList<CreateSaleEventTierRequest>? Tiers
) : ICommand<Guid>;

public sealed class UpdateSaleEventCommandValidator : AbstractValidator<UpdateSaleEventCommand>
{
    public UpdateSaleEventCommandValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEqual(Guid.Empty);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(90);

        RuleFor(x => x.StartDate)
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date.");

        RuleFor(x => x.Mode)
            .IsInEnum();

        When(x => x.Mode == SaleEventMode.DiscountAndSaleEvent, () =>
        {
            RuleFor(x => x.Tiers)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .Must(t => t!.Count <= 10)
                .WithMessage("A sale event can include at most 10 discount tiers.");

            When(x => x.Tiers is not null, () =>
            {
                RuleForEach(x => x.Tiers!)
                    .SetValidator(new SaleEventTierRequestValidator());
            });
        });

        When(x => x.Mode == SaleEventMode.SaleEventOnly, () =>
        {
            RuleFor(x => x.HighlightPercentage)
                .NotNull()
                .GreaterThan(0)
                .LessThanOrEqualTo(90);
        });
    }
}
