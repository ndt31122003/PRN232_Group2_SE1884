/*
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Abstractions;
using PRN232_EbayClone.Application.BuyerFeedback.Commands;
using PRN232_EbayClone.Application.BuyerFeedback.Queries;
using PRN232_EbayClone.Domain.BuyerFeedback.Enums;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
public class BuyerFeedbackController : ApiController
{
    /// <summary>
    /// Create feedback for a buyer after transaction
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateBuyerFeedback([FromBody] CreateBuyerFeedbackRequest request)
    {
        var command = new CreateBuyerFeedbackCommand(
            request.BuyerId,
            request.OrderId,
            request.ListingId,
            request.FeedbackType,
            request.Reason,
            request.Comment
        );

        var result = await Sender.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Blacklist a buyer
    /// </summary>
    [HttpPost("blacklist/{buyerId}")]
    public async Task<IActionResult> BlacklistBuyer(string buyerId, [FromBody] BlacklistBuyerRequest request)
    {
        var command = new BlacklistBuyerCommand(buyerId, request.Reason);
        var result = await Sender.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Remove buyer from blacklist
    /// </summary>
    [HttpDelete("blacklist/{buyerId}")]
    public async Task<IActionResult> UnblacklistBuyer(string buyerId)
    {
        var command = new UnblacklistBuyerCommand(buyerId);
        var result = await Sender.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}

public record CreateBuyerFeedbackRequest(
    string BuyerId,
    Guid? OrderId,
    Guid? ListingId,
    FeedbackType FeedbackType,
    FeedbackReason? Reason,
    string? Comment
);

public record BlacklistBuyerRequest(string? Reason);
*/