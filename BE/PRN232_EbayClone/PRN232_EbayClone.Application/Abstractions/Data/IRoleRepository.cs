using PRN232_EbayClone.Domain.Roles.Entities;
using PRN232_EbayClone.Domain.Roles.ValueObjects;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IRoleRepository : IRepository<Role, RoleId>
{
    Task<Role?> GetDefaultRoleAsync(CancellationToken cancellationToken);
}
