using Microsoft.EntityFrameworkCore;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Domain.Vouchers.Entities;

namespace PRN232_EbayClone.Infrastructure.Persistence.Repositories;

public sealed class VoucherRepository : Repository<Voucher, Guid>, IVoucherRepository
{
    public VoucherRepository(ApplicationDbContext context, IDbConnectionFactory connectionFactory)
        : base(context, connectionFactory)
    {
    }

    public override Task<Voucher?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return DbContext.Vouchers
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public Task<Voucher?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return DbContext.Vouchers
            .FirstOrDefaultAsync(v => v.Code == normalizedCode, cancellationToken);
    }

    public Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return DbContext.Vouchers
            .AsNoTracking()
            .AnyAsync(v => v.Code == normalizedCode, cancellationToken);
    }
}
