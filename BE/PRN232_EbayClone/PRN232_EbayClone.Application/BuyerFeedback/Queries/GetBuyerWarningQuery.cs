/*
using MediatR;
using PRN232_EbayClone.Domain.BuyerFeedback.Enums;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Application.BuyerFeedback.Queries;

public record BuyerWarningDto(
    string BuyerId,
    string BuyerName,
    bool IsBlacklisted,
    int TotalFeedbacks,
    int PositiveFeedbacks,
    int NeutralFeedbacks,
    int NegativeFeedbacks,
    double NegativePercentage,
    int BomHangCount30Days,
    int ThanhToanChamCount30Days,
    bool ShouldShowWarning,
    string WarningMessage,
    List<RecentNegativeFeedbackDto> RecentNegativeFeedbacks
);

public record RecentNegativeFeedbackDto(
    string SellerName,
    FeedbackReason? Reason,
    string? Comment,
    DateTime CreatedAt
);

public record GetBuyerWarningQuery(string BuyerId) : IRequest<Result<BuyerWarningDto>>;

public class GetBuyerWarningQueryHandler : IRequestHandler<GetBuyerWarningQuery, Result<BuyerWarningDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public GetBuyerWarningQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<BuyerWarningDto>> Handle(GetBuyerWarningQuery request, CancellationToken cancellationToken)
    {
        // Implementation will be added later
        return Result.Failure<BuyerWarningDto>(Error.NotFound("NotImplemented", "Not implemented yet"));
    }
}
*/