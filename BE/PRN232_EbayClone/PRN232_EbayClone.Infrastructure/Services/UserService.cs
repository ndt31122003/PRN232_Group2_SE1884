using PRN232_EbayClone.Domain.Shared.ValueObjects;
using PRN232_EbayClone.Domain.Users.Services;
using PRN232_EbayClone.Domain.Users.ValueObjects;
using PRN232_EbayClone.Infrastructure.Persistence;

namespace PRN232_EbayClone.Infrastructure.Services;

public sealed class UserService :
    IEmailUniquenessChecker
{
    private readonly ApplicationDbContext _context;
    public UserService(ApplicationDbContext context) => _context = context;
    public async Task<bool> IsUniqueEmail(Email email)
    {
        return !await _context.Users
            .AnyAsync(u => u.Email == email);
    }
}
