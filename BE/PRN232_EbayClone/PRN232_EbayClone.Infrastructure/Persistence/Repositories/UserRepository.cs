using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class UserRepository :
    Repository<User, UserId>,
    IUserRepository
{
    public UserRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory) 
        : base(context, connectionFactory)
    {
    }

    public Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return DbContext.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public override Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        return DbContext.Users
            .Include(u => u.Roles)
            .Include(u => u.ActiveListings)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public Task<User?> GetByIdAsNoTrackingAsync(UserId userId, CancellationToken cancellationToken)
    {
        return DbContext.Users
            .AsNoTracking()
            .Include(u => u.Roles)
            .Include(u => u.ActiveListings)
            .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return DbContext.Users
            .Include(u => u.Roles)
            .SingleOrDefaultAsync(u => u.Username == username, cancellationToken);
    }

    // Add this method
    public async Task<IReadOnlyList<User>> GetAllSellersAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Users
            .AsNoTracking()
            .Where(u => !u.IsDeleted) // Active users only
            .ToListAsync(cancellationToken);
        
        // ⚠️ If you want ONLY sellers (not buyers), filter by role:
        // .Where(u => !u.IsDeleted && u.Roles.Any(r => r.Name == "Seller"))
    }
}
