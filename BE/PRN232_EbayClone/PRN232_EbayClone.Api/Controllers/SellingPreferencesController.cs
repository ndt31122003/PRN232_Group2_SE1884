using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.SellingPreferences.Commands.ReplaceBlockedBuyers;
using PRN232_EbayClone.Application.SellingPreferences.Commands.ReplaceExemptBuyers;
using PRN232_EbayClone.Application.SellingPreferences.Commands.UpdateBuyerManagement;
using PRN232_EbayClone.Application.SellingPreferences.Commands.UpdateSellerPreferences;
using PRN232_EbayClone.Application.SellingPreferences.Dtos;
using PRN232_EbayClone.Application.SellingPreferences.Queries.GetBlockedBuyerList;
using PRN232_EbayClone.Application.SellingPreferences.Queries.GetBuyerManagement;
using PRN232_EbayClone.Application.SellingPreferences.Queries.GetExemptBuyerList;
using PRN232_EbayClone.Application.SellingPreferences.Queries.GetSellerPreference;
using PRN232_EbayClone.Domain.SellingPreferences.Enums;

namespace PRN232_EbayClone.Api.Controllers;

[Route("api/selling-preferences")]
[Authorize]
public sealed class SellingPreferencesController : ApiController
{
    private readonly IUserContext _userContext;

    public SellingPreferencesController(ISender sender, IUserContext userContext) : base(sender)
    {
        _userContext = userContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetSellerPreferenceQuery(userId);
        return await SendAsync(query, cancellationToken);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UpdateSellerPreferencesRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var command = new UpdateSellerPreferencesCommand(
            userId,
            request.ListingsStayActiveWhenOutOfStock,
            request.ShowExactQuantityAvailable,
            request.Invoice is null
                ? null
                : new InvoicePreferencePayload
                {
                    Format = request.Invoice.Format,
                    SendEmailCopy = request.Invoice.SendEmailCopy,
                    ApplyCreditsAutomatically = request.Invoice.ApplyCreditsAutomatically
                },
            request.BuyersCanSeeVatNumber,
            request.VatNumber);

        return await SendAsync(command, cancellationToken);
    }

    [HttpGet("buyer-management")]
    public async Task<IActionResult> GetBuyerManagementAsync(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetBuyerManagementQuery(userId);
        return await SendAsync(query, cancellationToken);
    }

    [HttpPut("buyer-management")]
    public async Task<IActionResult> UpdateBuyerManagementAsync([FromBody] UpdateBuyerManagementRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var command = new UpdateBuyerManagementCommand(
            userId,
            request.BlockUnpaidItemStrikes,
            request.UnpaidItemStrikesCount,
            request.UnpaidItemStrikesPeriodInMonths,
            request.BlockPrimaryAddressOutsideShippingLocation,
            request.BlockMaxItemsInLastTenDays,
            request.MaxItemsInLastTenDays,
            request.ApplyFeedbackScoreThreshold,
            request.FeedbackScoreThreshold,
            request.UpdateBlockSettingsForActiveListings,
            request.RequirePaymentMethodBeforeBid,
            request.RequirePaymentMethodBeforeOffer,
            request.PreventBlockedBuyersFromContacting);

        return await SendAsync(command, cancellationToken);
    }

    [HttpGet("blocked-buyers")]
    public async Task<IActionResult> GetBlockedBuyersAsync(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetBlockedBuyerListQuery(userId);
        return await SendAsync(query, cancellationToken);
    }

    [HttpPut("blocked-buyers")]
    public async Task<IActionResult> ReplaceBlockedBuyersAsync([FromBody] ReplaceBuyerListRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var command = new ReplaceBlockedBuyersCommand(userId, request.Items?.ToList());
        return await SendAsync(command, cancellationToken);
    }

    [HttpGet("exempt-buyers")]
    public async Task<IActionResult> GetExemptBuyersAsync(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var query = new GetExemptBuyerListQuery(userId);
        return await SendAsync(query, cancellationToken);
    }

    [HttpPut("exempt-buyers")]
    public async Task<IActionResult> ReplaceExemptBuyersAsync([FromBody] ReplaceBuyerListRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(_userContext.UserId, out var userId))
        {
            return Unauthorized();
        }

        var command = new ReplaceExemptBuyersCommand(userId, request.Items?.ToList());
        return await SendAsync(command, cancellationToken);
    }

    public sealed record UpdateSellerPreferencesRequest
    {
        public bool? ListingsStayActiveWhenOutOfStock { get; init; }
        public bool? ShowExactQuantityAvailable { get; init; }
        public InvoicePreferenceRequest? Invoice { get; init; }
        public bool? BuyersCanSeeVatNumber { get; init; }
        public string? VatNumber { get; init; }
    }

    public sealed record InvoicePreferenceRequest
    {
        public InvoiceFormat? Format { get; init; }
        public bool? SendEmailCopy { get; init; }
        public bool? ApplyCreditsAutomatically { get; init; }
    }

    public sealed record UpdateBuyerManagementRequest
    {
        public bool BlockUnpaidItemStrikes { get; init; }
        public int UnpaidItemStrikesCount { get; init; }
        public int UnpaidItemStrikesPeriodInMonths { get; init; }
        public bool BlockPrimaryAddressOutsideShippingLocation { get; init; }
        public bool BlockMaxItemsInLastTenDays { get; init; }
        public int? MaxItemsInLastTenDays { get; init; }
        public bool ApplyFeedbackScoreThreshold { get; init; }
        public int? FeedbackScoreThreshold { get; init; }
        public bool UpdateBlockSettingsForActiveListings { get; init; }
        public bool RequirePaymentMethodBeforeBid { get; init; }
        public bool RequirePaymentMethodBeforeOffer { get; init; }
        public bool PreventBlockedBuyersFromContacting { get; init; }
    }

    public sealed record ReplaceBuyerListRequest
    {
        public IReadOnlyCollection<string>? Items { get; init; }
    }
}
