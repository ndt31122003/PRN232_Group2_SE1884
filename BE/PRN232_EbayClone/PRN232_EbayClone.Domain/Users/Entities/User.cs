using PRN232_EbayClone.Domain.Listings.Entities;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Roles.Entities;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.Errors;
using PRN232_EbayClone.Domain.Users.Events;
using PRN232_EbayClone.Domain.Users.Services;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using System.Net.Mail;

namespace PRN232_EbayClone.Domain.Users.Entities;

public sealed class User : AggregateRoot<UserId>
{
    private readonly List<Role> _roles = [];
    public string Username { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public IReadOnlyList<Role> Roles => _roles.AsReadOnly();
    public bool IsEmailVerified { get; private set; } = false;
    public bool IsPaymentVerified { get; private set; } = false;

    public string? PhoneNumber { get; private set; }
    public bool IsPhoneVerified { get; private set; } = false;
    public string? BusinessName { get; private set; }
    public BusinessAddress? BusinessAddress { get; private set; }
    public bool IsBusinessVerified { get; private set; } = false;

    public bool IsSellerVerified => IsEmailVerified && IsPhoneVerified && IsBusinessVerified;

    //Seller
    public SellerPerformanceLevel PerformanceLevel { get; private set; } = SellerPerformanceLevel.BelowStandard;
    public SellingLimitPolicy LimitPolicy => SellingLimitPolicy.For(PerformanceLevel);

    private readonly HashSet<ListingId> _activeListings = [];
    public IReadOnlyCollection<ListingId> ActiveListings => _activeListings;
    private decimal _activeTotalValue;

    private User(UserId id) : base(id) { }

    public static async Task<Result<User>> CreateAsync(
        IEmailUniquenessChecker emailUniquenessChecker,
        string fullName,
        string email,
        string passwordHash,
        List<Role> roles)
    {
        var emailOrError = Email.Create(email);
        if (emailOrError.IsFailure)
            return emailOrError.Error;

        if (!await emailUniquenessChecker.IsUniqueEmail(emailOrError.Value))
            return UserErrors.DuplicateEmail(emailOrError.Value);

        var user = new User(UserId.New())
        {
            Username = emailOrError.Value,
            FullName = fullName,
            Email = emailOrError.Value,
            PasswordHash = passwordHash
        };

        foreach (var role in roles)
            user.AddRole(role);

        return user;
    }
    public static async Task<Result<User>> RegisterAsync(
        IEmailUniquenessChecker emailUniquenessChecker,
        string fullName,
        string email,
        string passwordHash)
    {
        var userOrError = await CreateAsync(emailUniquenessChecker, fullName, email, passwordHash, []);
        if (userOrError.IsFailure)
            return userOrError.Error;

        var user = userOrError.Value;

        user.RaiseDomainEvent(new UserRegisteredDomainEvent(user.Id, user.FullName, user.Email));
        return user;
    }

    public void AddRole(Role role)
    {
        if (!_roles.Contains(role))
            _roles.Add(role);
    }

    public void RemoveRole(Role role) => _roles.Remove(role);
    public void VerifyEmail() => IsEmailVerified = true;
    public void VerifyPayment() => IsPaymentVerified = true;
    public void UpdatePassword(string newHashedPassword) => PasswordHash = newHashedPassword;

    public void SetPhoneNumber(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
        IsPhoneVerified = false;
    }

    public void VerifyPhone() => IsPhoneVerified = true;

    public void SetBusinessInfo(string businessName, BusinessAddress address)
    {
        BusinessName = businessName;
        BusinessAddress = address;
        IsBusinessVerified = true;
    }
    public bool CanPostNewListing(Listing listing)
        => LimitPolicy.CanList(_activeListings.Count, _activeTotalValue + listing.GetEstimatedValue());

    public Result AddListing(Listing listing)
    {
        if (!IsSellerVerified)
            return UserErrors.SellerNotVerified;

        if (!CanPostNewListing(listing))
            return Error.Failure(
                "Seller.AddListing",
                $"Seller has reached limit for {PerformanceLevel.Name} level.");

        _activeListings.Add(new ListingId(listing.Id));
        _activeTotalValue += listing.GetEstimatedValue();
        return Result.Success();
    }   

    /// <summary>
    /// Update seller's performance level after monthly evaluation.
    /// Also updates the selling limit policy based on new level.
    /// </summary>
    /// <param name="newLevel">New performance level (TopRated, AboveStandard, or BelowStandard)</param>
    public void UpdatePerformanceLevel(SellerPerformanceLevel newLevel)
    {
        PerformanceLevel = newLevel;
    }
}
