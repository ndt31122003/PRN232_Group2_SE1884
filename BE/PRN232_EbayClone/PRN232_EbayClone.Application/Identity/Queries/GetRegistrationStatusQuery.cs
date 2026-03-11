using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Identity.Queries;

public sealed record GetRegistrationStatusQuery : IQuery<RegistrationStatusDto>;

public sealed record RegistrationStatusDto(
    bool IsEmailVerified,
    bool IsPhoneVerified,
    bool IsBusinessVerified,
    string? PhoneNumber,
    string? BusinessName,
    BusinessAddressDto? BusinessAddress
);

public sealed record BusinessAddressDto(
    string Street,
    string City,
    string State,
    string ZipCode,
    string Country
);

public sealed class GetRegistrationStatusQueryHandler : IQueryHandler<GetRegistrationStatusQuery, RegistrationStatusDto>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;

    public GetRegistrationStatusQueryHandler(
        IUserContext userContext,
        IUserRepository userRepository)
    {
        _userContext = userContext;
        _userRepository = userRepository;
    }

    public async Task<Result<RegistrationStatusDto>> Handle(GetRegistrationStatusQuery request, CancellationToken cancellationToken)
    {
        var userId = new UserId(Guid.Parse(_userContext.UserId!));
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return UserErrors.NotFound;

        BusinessAddressDto? addressDto = null;
        if (user.BusinessAddress is not null)
        {
            addressDto = new BusinessAddressDto(
                user.BusinessAddress.Street,
                user.BusinessAddress.City,
                user.BusinessAddress.State,
                user.BusinessAddress.ZipCode,
                user.BusinessAddress.Country);
        }

        return new RegistrationStatusDto(
            user.IsEmailVerified,
            user.IsPhoneVerified,
            user.IsBusinessVerified,
            user.PhoneNumber,
            user.BusinessName,
            addressDto);
    }
}
