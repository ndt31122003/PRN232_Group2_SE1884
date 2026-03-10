using PRN232_EbayClone.Application.Reviews.Dtos;
using PRN232_EbayClone.Domain.Reviews.Entities;

namespace PRN232_EbayClone.Application.Abstractions.Data;

public interface IReviewRepository : IRepository<Review, Guid>
{
    Task<(IReadOnlyList<Review> Reviews, int TotalCount)> GetReviewsAsync(
        ReviewFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Review>> GetReviewsByListingIdAsync(
        Guid listingId,
        CancellationToken cancellationToken = default);
}

