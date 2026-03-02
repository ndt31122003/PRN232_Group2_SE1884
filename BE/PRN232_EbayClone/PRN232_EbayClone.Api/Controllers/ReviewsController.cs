using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Reviews.Commands;
using PRN232_EbayClone.Application.Reviews.Dtos;
using PRN232_EbayClone.Application.Reviews.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/reviews")]
public sealed class ReviewsController(ISender sender) : ApiController(sender)
{
    [HttpGet]
    public Task<IActionResult> GetReviews([FromQuery] ReviewFilterDto filter, CancellationToken cancellationToken)
    {
        var query = new GetReviewsQuery(filter);
        return SendAsync(query, cancellationToken);
    }

    [HttpPost("{reviewId}/reply")]
    [Authorize]
    public Task<IActionResult> ReplyToReview(
        Guid reviewId,
        [FromBody] ReplyToReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReplyToReviewCommand(reviewId, request.Reply);
        return SendAsync(command, cancellationToken);
    }

    [HttpPost("{reviewId}/request-revision")]
    [Authorize]
    public Task<IActionResult> RequestReviewRevision(
        Guid reviewId,
        CancellationToken cancellationToken)
    {
        var command = new RequestReviewRevisionCommand(reviewId);
        return SendAsync(command, cancellationToken);
    }
}

public sealed record ReplyToReviewRequest(string Reply);
