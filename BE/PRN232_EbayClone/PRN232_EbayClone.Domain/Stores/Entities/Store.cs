using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Stores.Enums;
using PRN232_EbayClone.Domain.Stores.Errors;
using PRN232_EbayClone.Domain.Stores.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Stores.Entities;

public sealed class Store : AggregateRoot<StoreId>
{
    public UserId UserId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Slug { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? BannerUrl { get; private set; }
    public StoreType StoreType { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<StoreSubscription> _subscriptions = [];
    public IReadOnlyCollection<StoreSubscription> Subscriptions => _subscriptions.AsReadOnly();

    private Store(StoreId id) : base(id) { }

    public static Result<Store> Create(
        UserId userId,
        string name,
        StoreType storeType,
        string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return StoreErrors.NameRequired;

        if (name.Length > 255)
            return StoreErrors.NameTooLong;

        var slug = GenerateSlug(name);

        var store = new Store(StoreId.New())
        {
            UserId = userId,
            Name = name,
            Slug = slug,
            Description = description,
            StoreType = storeType,
            IsActive = true
        };

        return store;
    }

    public Result UpdateProfile(string name, string? description, string? logoUrl, string? bannerUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            return StoreErrors.NameRequired;

        if (name.Length > 255)
            return StoreErrors.NameTooLong;

        Name = name;
        Slug = GenerateSlug(name);
        Description = description;
        LogoUrl = logoUrl;
        BannerUrl = bannerUrl;

        return Result.Success();
    }

    public Result Activate()
    {
        if (IsActive)
            return StoreErrors.AlreadyActive;

        IsActive = true;
        return Result.Success();
    }

    public Result Deactivate()
    {
        if (!IsActive)
            return StoreErrors.AlreadyInactive;

        IsActive = false;
        return Result.Success();
    }

    public void AddSubscription(StoreSubscription subscription)
    {
        _subscriptions.Add(subscription);
    }

    private static string GenerateSlug(string name)
    {
        return name.Trim()
            .Replace(" ", "-")
            .Replace("_", "-")
            .ToLowerInvariant();
    }
}

