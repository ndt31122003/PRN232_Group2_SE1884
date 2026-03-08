using System;
using System.Collections.Generic;

namespace PRN232_EbayClone.Infrastructure.Persistence.Scaffolded;

public partial class Store
{
    public Guid Id { get; set; }

    public string? ThemeColor { get; private set; }
    public string? LayoutConfig { get; private set; }
    public string? ContactEmail { get; private set; }
    public string? ContactPhone { get; private set; }
    public string? SocialLinks { get; private set; }

    private readonly List<StoreSubscription> _subscriptions = [];
    public IReadOnlyCollection<StoreSubscription> Subscriptions => _subscriptions.AsReadOnly();

    public string? Storename { get; set; }

    public string? Description { get; set; }

    public string? Bannerimageurl { get; set; }

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

    public Result UpdateProfile(
        string name,
        string? description,
        string? logoUrl,
        string? bannerUrl,
        string? themeColor = null,
        string? contactEmail = null,
        string? contactPhone = null,
        string? socialLinks = null)
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
        ThemeColor = themeColor;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        SocialLinks = socialLinks;

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
