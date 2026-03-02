using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IOtpRepository : IRepository<Otp, Guid>
{
    Task<Otp?> GetByEmailAndCodeAndTypeAsync(Email email, string code, OtpType type, CancellationToken cancellationToken);
    Task<Otp?> GetByEmailAndTypeAsync(Email email, OtpType type, CancellationToken cancellationToken);
}
