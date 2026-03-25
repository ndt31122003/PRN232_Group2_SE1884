using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Abstractions.Messaging;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Mappings;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.Disputes.Queries;

public sealed record GetBuyerDisputesQuery(
    string BuyerId,
    DisputeFilterDto Filter
) : IQuery<PagingResult<DisputeDto>>;

public sealed class GetBuyerDisputesQueryValidator : AbstractValidator<GetBuyerDisputesQuery>
{
    public GetBuyerDisputesQueryValidator()
    {
        RuleFor(x => x.BuyerId).NotEmpty().WithMessage("Buyer ID không được để trống");
        RuleFor(x => x.Filter).NotNull().WithMessage("Filter không được để trống");
        RuleFor(x => x.Filter.PageNumber).GreaterThan(0).WithMessage("Page number phải lớn hơn 0");
        RuleFor(x => x.Filter.PageSize).InclusiveBetween(1, 200).WithMessage("Page size phải từ 1 đến 200");
    }
}

internal sealed class GetBuyerDisputesQueryHandler : IQueryHandler<GetBuyerDisputesQuery, PagingResult<DisputeDto>>
{
    private readonly IDisputeRepository _disputeRepository;

    public GetBuyerDisputesQueryHandler(IDisputeRepository disputeRepository)
    {
        _disputeRepository = disputeRepository;
    }

    public async Task<Result<PagingResult<DisputeDto>>> Handle(
        GetBuyerDisputesQuery request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"[GetBuyerDisputesQuery] BuyerId: {request.BuyerId}, Filter: Status={request.Filter.Status}, PageNumber={request.Filter.PageNumber}");
        
        // Create filter with RaisedById to get only disputes raised by this buyer
        var filter = new DisputeFilterDto
        {
            RaisedById = request.BuyerId,
            Status = request.Filter.Status,
            ListingId = request.Filter.ListingId,
            FromDate = request.Filter.FromDate,
            ToDate = request.Filter.ToDate,
            PageNumber = request.Filter.PageNumber,
            PageSize = request.Filter.PageSize
        };

        var (disputes, totalCount) = await _disputeRepository.GetDisputesByBuyerIdAsync(
            request.BuyerId,
            filter,
            cancellationToken);

        Console.WriteLine($"[GetBuyerDisputesQuery] Found {disputes.Count} disputes out of {totalCount} total");

        var disputeDtos = disputes.Select(d => d.ToDto()).ToList();

        var pagingResult = new PagingResult<DisputeDto>(
            disputeDtos,
            totalCount,
            request.Filter.PageNumber,
            request.Filter.PageSize);

        return Result.Success(pagingResult);
    }
}
