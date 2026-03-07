using PRN232_EbayClone.Domain.Vouchers.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IVoucherRepository : IRepository<Voucher, Guid>
{
    Task<Voucher?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<bool> CodeExistsAsync(string code, CancellationToken cancellationToken = default);
}
