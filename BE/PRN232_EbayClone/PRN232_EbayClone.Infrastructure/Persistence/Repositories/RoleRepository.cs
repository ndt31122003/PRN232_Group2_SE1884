using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Roles.Entities;
using PRN232_EbayClone.Domain.Roles.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class RoleRepository :
    Repository<Role, RoleId>,
    IRoleRepository
{
    public RoleRepository(
        ApplicationDbContext context,
        IDbConnectionFactory connectionFactory) 
        : base(context, connectionFactory)
    {
    }

    public override Task<Role?> GetByIdAsync(RoleId id, CancellationToken cancellationToken)
    {
        return DbContext.Roles
            .Include(r => r.Permissions)
            .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public Task<Role?> GetDefaultRoleAsync(CancellationToken cancellationToken)
    {
        return DbContext.Roles.FirstOrDefaultAsync(cancellationToken);
    }
}
