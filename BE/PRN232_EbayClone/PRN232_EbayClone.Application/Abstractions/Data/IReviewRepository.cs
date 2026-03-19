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

    Task<(IReadOnlyList<Review> Reviews, int TotalCount, decimal AverageRating)> GetSellerReviewsAsync(
        Guid sellerId,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int? rating = null,
        int? minRating = null,
        int? maxRating = null,
        bool? isReplied = null,
        int pageNumber = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default);

    Task<Review?> GetSellerReviewByIdAsync(
        Guid reviewId,
        Guid sellerId,
        CancellationToken cancellationToken = default);
}

