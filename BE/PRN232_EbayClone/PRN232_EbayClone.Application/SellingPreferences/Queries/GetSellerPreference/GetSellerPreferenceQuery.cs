using System;
using System.Threading;
using System.Threading.Tasks;
using PRN232_EbayClone.Application.SellingPreferences.Dtos;
using PRN232_EbayClone.Application.SellingPreferences.Mappings;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.SellingPreferences.Queries.GetSellerPreference;

public sealed record GetSellerPreferenceQuery(Guid UserId) : IQuery<SellerPreferenceDto>;

public sealed class GetSellerPreferenceQueryValidator : AbstractValidator<GetSellerPreferenceQuery>
{
    public GetSellerPreferenceQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}

public sealed class GetSellerPreferenceQueryHandler : IQueryHandler<GetSellerPreferenceQuery, SellerPreferenceDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ISellerPreferenceRepository _sellerPreferenceRepository;

    public GetSellerPreferenceQueryHandler(
        IUserRepository userRepository,
        ISellerPreferenceRepository sellerPreferenceRepository)
    {
        _userRepository = userRepository;
        _sellerPreferenceRepository = sellerPreferenceRepository;
    }

    public async Task<Result<SellerPreferenceDto>> Handle(GetSellerPreferenceQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<SellerPreferenceDto>(Error.Failure("SellerPreference.UserNotFound", "User not found."));
        }

        var preference = await _sellerPreferenceRepository.GetBySellerIdAsync(request.UserId, cancellationToken);
        return Result.Success(preference.ToDto());
    }
}
