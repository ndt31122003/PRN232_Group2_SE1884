using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IRefreshTokenRepository : IRepository<RefreshToken, Guid>
{
    Task DeleteByUserIdAsync(UserId userId, CancellationToken cancellationToken);

    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken);
}
