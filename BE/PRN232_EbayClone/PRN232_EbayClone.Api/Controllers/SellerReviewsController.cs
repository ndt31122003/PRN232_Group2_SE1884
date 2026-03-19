using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Reviews.Commands;
using PRN232_EbayClone.Application.Reviews.Queries;
using PRN232_EbayClone.Api.Services;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/seller/reviews")]
[Authorize]
public sealed class SellerReviewsController : ApiController
{
    private readonly ICurrentUser _currentUser;

    public SellerReviewsController(ISender sender, ICurrentUser currentUser) : base(sender)
    {
        _currentUser = currentUser;
    }

    /// <summary>
    /// Get reviews for the current seller with filtering and sorting
    /// </summary>
    /// <param name="startDate">Filter by start date</param>
    /// <param name="endDate">Filter by end date</param>
    /// <param name="rating">Filter by exact rating (1-5)</param>
    /// <param name="minRating">Filter by minimum rating</param>
    /// <param name="maxRating">Filter by maximum rating</param>
    /// <param name="isReplied">Filter by reply status (true = replied, false = not replied)</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20, max: 200)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of reviews with average rating</returns>
    [HttpGet]
    public Task<IActionResult> GetSellerReviews(
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] int? rating = null,
        [FromQuery] int? minRating = null,
        [FromQuery] int? maxRating = null,
        [FromQuery] bool? isReplied = null,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var sellerId = _currentUser.Id;
        var query = new GetSellerReviewsQuery(
            sellerId,
            startDate,
            endDate,
            rating,
            minRating,
            maxRating,
            isReplied,
            pageNumber,
            pageSize);

        return SendAsync(query, cancellationToken);
    }

    /// <summary>
    /// Get a specific review by ID (must belong to current seller)
    /// </summary>
    /// <param name="id">Review ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Review details</returns>
    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetSellerReviewById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var sellerId = _currentUser.Id;
        var query = new GetSellerReviewByIdQuery(id, sellerId);
        return SendAsync(query, cancellationToken);
    }

    /// <summary>
    /// Reply to a review (must belong to current seller and not already replied)
    /// </summary>
    /// <param name="id">Review ID</param>
    /// <param name="request">Reply request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success result</returns>
    [HttpPost("{id:guid}/reply")]
    public Task<IActionResult> ReplyToReview(
        Guid id,
        [FromBody] ReplyToSellerReviewRequest request,
        CancellationToken cancellationToken)
    {
        var sellerId = _currentUser.Id;
        var command = new ReplyToSellerReviewCommand(id, sellerId, request.Reply);
        return SendAsync(command, cancellationToken);
    }
}

public sealed record ReplyToSellerReviewRequest(string Reply);