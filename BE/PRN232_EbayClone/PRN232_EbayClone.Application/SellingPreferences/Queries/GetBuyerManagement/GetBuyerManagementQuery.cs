using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.SellingPreferences.Dtos;
using PRN232_EbayClone.Application.SellingPreferences.Mappings;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.SellingPreferences.Queries.GetBuyerManagement;

public sealed record GetBuyerManagementQuery(Guid UserId) : IQuery<BuyerManagementDto>;

public sealed class GetBuyerManagementQueryValidator : AbstractValidator<GetBuyerManagementQuery>
{
    public GetBuyerManagementQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}

public sealed class GetBuyerManagementQueryHandler : IQueryHandler<GetBuyerManagementQuery, BuyerManagementDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ISellerPreferenceRepository _sellerPreferenceRepository;

    public GetBuyerManagementQueryHandler(
        IUserRepository userRepository,
        ISellerPreferenceRepository sellerPreferenceRepository)
    {
        _userRepository = userRepository;
        _sellerPreferenceRepository = sellerPreferenceRepository;
    }

    public async Task<Result<BuyerManagementDto>> Handle(GetBuyerManagementQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<BuyerManagementDto>(Error.Failure("SellerPreference.UserNotFound", "User not found."));
        }

    var preference = await _sellerPreferenceRepository.GetBySellerIdAsync(request.UserId, cancellationToken);
    var buyerManagement = preference?.BuyerManagement;
    var dto = buyerManagement.ToDto();
    return Result.Success(dto);
    }
}
