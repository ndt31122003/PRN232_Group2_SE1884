using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Identity.Enums;
using PRN232_EbayClone.Domain.Shared.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class OtpRepository :
    Repository<Otp, Guid>,
    IOtpRepository
{
    public OtpRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory) 
        : base(context, connectionFactory)
    {
    }

    public Task<Otp?> GetByEmailAndCodeAndTypeAsync(Email email, string code, OtpType type, CancellationToken cancellationToken)
    {
        return DbContext.Otps
            .FirstOrDefaultAsync(otp =>
                otp.Email == email &&
                otp.Code == code &&
                otp.Type == type,
                cancellationToken);
    }

    public Task<Otp?> GetByEmailAndTypeAsync(Email email, OtpType type, CancellationToken cancellationToken)
    {
        return DbContext.Otps
            .Where(otp => otp.Email == email && otp.Type == type)
            .OrderByDescending(otp => otp.ExpiresOnUtc)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public override Task<Otp?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
