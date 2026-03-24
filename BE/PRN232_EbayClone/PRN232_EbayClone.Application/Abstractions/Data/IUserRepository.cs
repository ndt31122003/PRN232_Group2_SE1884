using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IUserRepository : IRepository<User, UserId>
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);

    /// <summary>
    /// Get all sellers for monthly performance evaluation.
    /// </summary>
    Task<IReadOnlyList<User>> GetAllSellersAsync(CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsNoTrackingAsync(UserId userId, CancellationToken cancellationToken);
}
