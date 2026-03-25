using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Common.Dtos;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Mappings;

namespace PRN232_EbayClone.Application.Disputes.Queries;

public sealed record GetDisputesQuery(
    DisputeFilterDto Filter,
    string CurrentUserId
) : IQuery<PagingResult<DisputeDto>>;

public sealed class GetDisputesQueryValidator : AbstractValidator<GetDisputesQuery>
{
    public GetDisputesQueryValidator()
    {
        RuleFor(x => x.Filter).NotNull().WithMessage("Filter không được để trống");
        RuleFor(x => x.Filter.PageNumber).GreaterThan(0).WithMessage("Page number phải lớn hơn 0");
        RuleFor(x => x.Filter.PageSize).InclusiveBetween(1, 200).WithMessage("Page size phải từ 1 đến 200");
    }
}

public sealed class GetDisputesQueryHandler : IQueryHandler<GetDisputesQuery, PagingResult<DisputeDto>>
{
    private readonly IDisputeRepository _disputeRepository;

    public GetDisputesQueryHandler(IDisputeRepository disputeRepository)
    {
        _disputeRepository = disputeRepository;
    }

    public async Task<Result<PagingResult<DisputeDto>>> Handle(
        GetDisputesQuery request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"[GetDisputesQuery] CurrentUserId: {request.CurrentUserId}, Filter: Status={request.Filter.Status}, PageNumber={request.Filter.PageNumber}");
        
        var (disputes, totalCount) = await _disputeRepository.GetDisputesAsync(
            request.Filter,
            request.CurrentUserId,
            cancellationToken);

        Console.WriteLine($"[GetDisputesQuery] Found {disputes.Count} disputes out of {totalCount} total");

        var disputeDtos = disputes.Select(d => d.ToDto()).ToList();

        var pagingResult = new PagingResult<DisputeDto>(
            disputeDtos,
            totalCount,
            request.Filter.PageNumber,
            request.Filter.PageSize);

        return Result.Success(pagingResult);
    }
}
