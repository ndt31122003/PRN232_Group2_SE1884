using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Domain.Users.Services;

public interface IEmailUniquenessChecker
{
    Task<bool> IsUniqueEmail(Email email);
}
