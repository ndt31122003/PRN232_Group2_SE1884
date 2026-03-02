using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Disputes.Dtos;
using PRN232_EbayClone.Application.Disputes.Mappings;
using PRN232_EbayClone.Domain.Disputes.Errors;

namespace PRN232_EbayClone.Application.Disputes.Queries;

public sealed record GetDisputeByIdQuery(
    Guid DisputeId
) : IQuery<DisputeDto>;

public sealed class GetDisputeByIdQueryValidator : AbstractValidator<GetDisputeByIdQuery>
{
    public GetDisputeByIdQueryValidator()
    {
        RuleFor(x => x.DisputeId).NotEmpty().WithMessage("Dispute ID là bắt buộc");
    }
}

public sealed class GetDisputeByIdQueryHandler : IQueryHandler<GetDisputeByIdQuery, DisputeDto>
{
    private readonly IDisputeRepository _disputeRepository;

    public GetDisputeByIdQueryHandler(IDisputeRepository disputeRepository)
    {
        _disputeRepository = disputeRepository;
    }

    public async Task<Result<DisputeDto>> Handle(
        GetDisputeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var dispute = await _disputeRepository.GetByIdAsync(request.DisputeId, cancellationToken);
        if (dispute is null)
        {
            return DisputeErrors.NotFound;
        }

        var disputeDto = dispute.ToDto();
        return Result.Success(disputeDto);
    }
}


