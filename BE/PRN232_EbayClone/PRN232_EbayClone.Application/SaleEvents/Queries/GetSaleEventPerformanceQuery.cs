using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.SaleEvents.Dtos;

namespace PRN232_EbayClone.Application.SaleEvents.Queries;

public sealed record GetSaleEventPerformanceQuery(
    Guid SaleEventId,
    DateTime? StartDate,
    DateTime? EndDate
) : IQuery<SaleEventPerformanceDto>;

public sealed class GetSaleEventPerformanceQueryValidator : AbstractValidator<GetSaleEventPerformanceQuery>
{
    public GetSaleEventPerformanceQueryValidator()
    {
        RuleFor(x => x.SaleEventId)
            .NotEmpty()
            .WithMessage("Sale event ID is required");

        RuleFor(x => x)
            .Must(x => !x.StartDate.HasValue || !x.EndDate.HasValue || x.StartDate.Value < x.EndDate.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Start date must be before end date");
    }
}

public sealed class GetSaleEventPerformanceQueryHandler : IQueryHandler<GetSaleEventPerformanceQuery, SaleEventPerformanceDto>
{
    private readonly ISaleEventRepository _saleEventRepository;
    private readonly IUserContext _userContext;

    public GetSaleEventPerformanceQueryHandler(
        ISaleEventRepository saleEventRepository,
        IUserContext userContext)
    {
        _saleEventRepository = saleEventRepository;
        _userContext = userContext;
    }

    public async Task<Result<SaleEventPerformanceDto>> Handle(
        GetSaleEventPerformanceQuery request,
        CancellationToken cancellationToken)
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

        // Get performance metrics with optional date range filtering
        var metrics = await _saleEventRepository.GetPerformanceMetricsAsync(
            request.SaleEventId,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        if (metrics == null)
        {
            // Return empty metrics if none exist yet
            return Result.Success(new SaleEventPerformanceDto(
                request.SaleEventId,
                saleEvent.Name,
                0,
                0,
                0,
                0,
                0,
                0,
                DateTime.UtcNow,
                new List<TierPerformanceDto>()));
        }

        // Get tier-level performance
        var tierPerformance = await _saleEventRepository.GetTierPerformanceMetricsAsync(
            request.SaleEventId,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        var tierPerformanceDtos = tierPerformance
            .Select(tp => new TierPerformanceDto(
                tp.TierId,
                tp.TierLabel,
                tp.Priority,
                tp.OrderCount,
                tp.TotalDiscountAmount,
                tp.TotalSalesRevenue))
            .ToList();

        var dto = new SaleEventPerformanceDto(
            metrics.SaleEventId,
            saleEvent.Name,
            metrics.OrderCount,
            metrics.TotalDiscountAmount,
            metrics.TotalSalesRevenue,
            metrics.TotalItemsSold,
            metrics.AverageDiscountPerOrder,
            metrics.AverageOrderValue,
            metrics.LastUpdated,
            tierPerformanceDtos);

        return Result.Success(dto);
    }
}
