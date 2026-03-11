using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.SellingPreferences.Dtos;
using PRN232_EbayClone.Application.SellingPreferences.Mappings;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.SellingPreferences.Queries.GetBlockedBuyerList;

public sealed record GetBlockedBuyerListQuery(Guid UserId) : IQuery<BuyerListDto>;

public sealed class GetBlockedBuyerListQueryValidator : AbstractValidator<GetBlockedBuyerListQuery>
{
    public GetBlockedBuyerListQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}

public sealed class GetBlockedBuyerListQueryHandler : IQueryHandler<GetBlockedBuyerListQuery, BuyerListDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ISellerPreferenceRepository _sellerPreferenceRepository;

    public GetBlockedBuyerListQueryHandler(
        IUserRepository userRepository,
        ISellerPreferenceRepository sellerPreferenceRepository)
    {
        _userRepository = userRepository;
        _sellerPreferenceRepository = sellerPreferenceRepository;
    }

    public async Task<Result<BuyerListDto>> Handle(GetBlockedBuyerListQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<BuyerListDto>(Error.Failure("SellerPreference.UserNotFound", "User not found."));
        }

        var preference = await _sellerPreferenceRepository.GetBySellerIdAsync(request.UserId, cancellationToken);
        return Result.Success(preference.ToBlockedBuyerListDto());
    }
}
