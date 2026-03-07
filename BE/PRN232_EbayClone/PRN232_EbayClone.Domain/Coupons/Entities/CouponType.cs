using PRN232_EbayClone.Domain.Coupons.Errors;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Coupons.Entities;

public sealed class CouponType : AggregateRoot<Guid>
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    private CouponType(Guid id, string name, string? description) : base(id)
    {
        Name = name;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public CouponType(Guid id, string name, string? description, bool isActive) : base(id)
    {
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<CouponType> Create(string name, string? description)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CouponTypeErrors.EmptyName;
        }

        var couponType = new CouponType(
            Guid.NewGuid(),
            name.Trim(),
            description?.Trim());

        return couponType;
    }

    public Result Update(string name, string? description, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return CouponTypeErrors.EmptyName;
        }

        Name = name.Trim();
        Description = description?.Trim();
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }
}
