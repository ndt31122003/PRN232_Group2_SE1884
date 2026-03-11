using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public class RefreshTokenRepository :
    Repository<RefreshToken, Guid>,
    IRefreshTokenRepository
{
    public RefreshTokenRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory) 
        : base(context, connectionFactory)
    {
    }

    public Task DeleteByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        return DbContext.RefreshTokens
            .Where(rt => rt.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public override Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        return DbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }
}
