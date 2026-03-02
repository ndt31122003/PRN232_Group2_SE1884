using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Abstractions;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Payments.Dtos;
using PRN232_EbayClone.Application.Payments.Queries;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/payments")]
public sealed class PaymentsController : ApiController
{
    private readonly IUserContext _userContext;

    public PaymentsController(ISender sender, IUserContext userContext)
        : base(sender)
    {
        _userContext = userContext;
    }

    [HttpGet("transactions")]
    public Task<IActionResult> GetTransactions([FromQuery] PaymentTransactionsFilterDto filter, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerId))
        {
            return Task.FromResult(BuildUnauthorizedResult());
        }

        var query = new GetPaymentTransactionsQuery(sellerId, filter);
        return SendAsync(query, cancellationToken);
    }

    [HttpGet("reports")]
    public Task<IActionResult> GetReport([FromQuery] PaymentReportFilterDto filter, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerId))
        {
            return Task.FromResult(BuildUnauthorizedResult());
        }

        var query = new GetPaymentReportQuery(sellerId, filter);
        return SendAsync(query, cancellationToken);
    }

    [HttpGet("summary")]
    public Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerId))
        {
            return Task.FromResult(BuildUnauthorizedResult());
        }

        var query = new GetPaymentSummaryQuery(sellerId);
        return SendAsync(query, cancellationToken);
    }

    [HttpGet("payouts")]
    public Task<IActionResult> GetPayouts([FromQuery] PaymentPayoutsFilterDto filter, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerId))
        {
            return Task.FromResult(BuildUnauthorizedResult());
        }

        var query = new GetPaymentPayoutsQuery(sellerId, filter);
        return SendAsync(query, cancellationToken);
    }

    [HttpGet("payouts/{payoutId}")]
    public Task<IActionResult> GetPayoutDetail(string payoutId, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var sellerId))
        {
            return Task.FromResult(BuildUnauthorizedResult());
        }

        var query = new GetPaymentPayoutDetailQuery(sellerId, payoutId);
        return SendAsync(query, cancellationToken);
    }

    private IActionResult BuildUnauthorizedResult()
        => Unauthorized(new ProblemDetails
        {
            Title = "Authentication error",
            Detail = "User is not authenticated.",
            Status = StatusCodes.Status401Unauthorized
        });
}
