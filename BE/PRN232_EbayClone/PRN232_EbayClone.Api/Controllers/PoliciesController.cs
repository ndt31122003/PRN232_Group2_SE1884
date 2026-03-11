using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Application.Policies.Commands;
using PRN232_EbayClone.Application.Policies.Queries;
using PRN232_EbayClone.Domain.Policies.Enums;

namespace PRN232_EbayClone.Api.Controllers;

[Authorize]
[Route("api/stores/{storeId}/policies")]
public sealed class PoliciesController(ISender sender) : ApiController(sender)
{
    private readonly ISender _sender = sender;
    
    [HttpGet("shipping")]
    public Task<IActionResult> GetShippingPolicies(string storeId, CancellationToken cancellationToken)
    {
        return SendAsync(new GetShippingPoliciesQuery(storeId), cancellationToken);
    }

    [HttpPost("shipping")]
    public Task<IActionResult> CreateShippingPolicy(string storeId, [FromBody] CreateShippingPolicyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateShippingPolicyCommand(
            storeId,
            request.Carrier,
            request.ServiceName,
            request.CostAmount,
            request.Currency,
            request.HandlingTimeDays,
            request.IsDefault);
        
        return SendAsync(command, cancellationToken);
    }

    [HttpPut("shipping/{policyId}")]
    public Task<IActionResult> UpdateShippingPolicy(string storeId, Guid policyId, [FromBody] UpdateShippingPolicyRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateShippingPolicyCommand(
            storeId,
            policyId,
            request.Carrier,
            request.ServiceName,
            request.CostAmount,
            request.Currency,
            request.HandlingTimeDays,
            request.IsDefault);
        
        return SendAsync(command, cancellationToken);
    }

    [HttpDelete("shipping/{policyId}")]
    public Task<IActionResult> DeleteShippingPolicy(string storeId, Guid policyId, CancellationToken cancellationToken)
    {
        var command = new DeleteShippingPolicyCommand(storeId, policyId);
        return SendAsync(command, cancellationToken);
    }

    [HttpGet("return")]
    public async Task<IActionResult> GetReturnPolicy(string storeId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetReturnPolicyQuery(storeId), cancellationToken);
        
        if (result.IsFailure)
            return HandleFailure(result);
        
        return result.Value is null ? NoContent() : Ok(result.Value);
    }

    [HttpPost("return")]
    public Task<IActionResult> CreateReturnPolicy(string storeId, [FromBody] CreateReturnPolicyRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateReturnPolicyCommand(
            storeId,
            request.AcceptReturns,
            request.ReturnPeriodDays,
            request.RefundMethod,
            request.ReturnShippingPaidBy);
        
        return SendAsync(command, cancellationToken);
    }

    [HttpPut("return")]
    public Task<IActionResult> UpdateReturnPolicy(string storeId, [FromBody] UpdateReturnPolicyRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateReturnPolicyCommand(
            storeId,
            request.AcceptReturns,
            request.ReturnPeriodDays,
            request.RefundMethod,
            request.ReturnShippingPaidBy);
        
        return SendAsync(command, cancellationToken);
    }

    [HttpDelete("return")]
    public Task<IActionResult> DeleteReturnPolicy(string storeId, CancellationToken cancellationToken)
    {
        var command = new DeleteReturnPolicyCommand(storeId);
        return SendAsync(command, cancellationToken);
    }
}

public sealed record CreateShippingPolicyRequest(
    string Carrier,
    string ServiceName,
    decimal CostAmount,
    string Currency,
    int HandlingTimeDays,
    bool IsDefault
);

public sealed record UpdateShippingPolicyRequest(
    string Carrier,
    string ServiceName,
    decimal CostAmount,
    string Currency,
    int HandlingTimeDays,
    bool IsDefault
);

public sealed record CreateReturnPolicyRequest(
    bool AcceptReturns,
    ReturnPeriod? ReturnPeriodDays,
    RefundMethod? RefundMethod,
    ReturnShippingPaidBy? ReturnShippingPaidBy
);

public sealed record UpdateReturnPolicyRequest(
    bool AcceptReturns,
    ReturnPeriod? ReturnPeriodDays,
    RefundMethod? RefundMethod,
    ReturnShippingPaidBy? ReturnShippingPaidBy
);
