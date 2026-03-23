using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232_EbayClone.Api.Abstractions;
using PRN232_EbayClone.Application.SaleEvents.Commands;
using PRN232_EbayClone.Application.SaleEvents.Queries;
using PRN232_EbayClone.Domain.Discounts.Enums;
using PRN232_EbayClone.Domain.Discounts.ValueObjects;

namespace PRN232_EbayClone.Api.Controllers;

/// <summary>
/// Manages sale events for sellers to run time-limited promotional sales with tiered discount structures
/// </summary>
[Route("api/sale-events")]
[Authorize]
public class SaleEventsController : ApiController
{
    public SaleEventsController(ISender sender) : base(sender)
    {
    }

    // 8.1 - Basic CRUD endpoints
    
    /// <summary>
    /// Creates a new sale event with discount tiers and listing assignments
    /// </summary>
    /// <param name="request">Sale event creation details including name, dates, mode, tiers, and options</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The ID of the created sale event</returns>
    /// <response code="200">Sale event created successfully</response>
    /// <response code="400">Invalid request data or validation failure</response>
    /// <response code="401">Unauthorized - authentication required</response>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateSaleEvent([FromBody] CreateSaleEventRequest request, CancellationToken cancellationToken)
    {
        var tiers = request.Tiers?.Select(t => new CreateSaleEventTierRequest(
            t.DiscountType,
            t.DiscountValue,
            t.Priority,
            t.Label,
            t.ListingIds
        )).ToList();

        var command = new CreateSaleEventCommand(
            request.Name,
            request.Description,
            request.BuyerMessageLabel,
            request.Mode,
            request.StartDate,
            request.EndDate,
            request.OfferFreeShipping,
            request.BlockPriceIncreaseRevisions,
            request.IncludeSkippedItems,
            request.HighlightPercentage,
            tiers
        );

        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Updates an existing sale event (allowed before start date only, except tier assignments)
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="request">Updated sale event details</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result</returns>
    /// <response code="200">Sale event updated successfully</response>
    /// <response code="400">Invalid request or sale event already started</response>
    /// <response code="404">Sale event not found</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSaleEvent(Guid id, [FromBody] UpdateSaleEventRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateSaleEventCommand(
            id,
            request.Name,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.BuyerMessageLabel,
            request.OfferFreeShipping,
            request.BlockPriceIncreaseRevisions,
            request.IncludeSkippedItems
        );

        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Activates a sale event, making it live and applying sale pricing to assigned listings
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result</returns>
    /// <response code="200">Sale event activated successfully</response>
    /// <response code="400">Validation failure (no listings assigned or invalid date range)</response>
    /// <response code="404">Sale event not found</response>
    [HttpPost("{id:guid}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ActivateSaleEvent(Guid id, CancellationToken cancellationToken)
    {
        var command = new ActivateSaleEventCommand(id);
        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Deactivates an active sale event, immediately stopping sale pricing application
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result</returns>
    /// <response code="200">Sale event deactivated successfully</response>
    /// <response code="404">Sale event not found</response>
    [HttpPost("{id:guid}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeactivateSaleEvent(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivateSaleEventCommand(id);
        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Deletes a sale event (only allowed if not yet activated)
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result</returns>
    /// <response code="200">Sale event deleted successfully</response>
    /// <response code="400">Cannot delete activated sale event</response>
    /// <response code="404">Sale event not found</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSaleEvent(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteSaleEventCommand(id);
        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale event by ID with all tiers and listing assignments
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Sale event details</returns>
    /// <response code="200">Sale event found</response>
    /// <response code="404">Sale event not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSaleEventById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetSaleEventByIdQuery(id);
        return await SendAsync(query, cancellationToken);
    }

    /// <summary>
    /// Retrieves all sale events for a specific seller with pagination
    /// </summary>
    /// <param name="sellerId">Seller user ID</param>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 10)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of sale events</returns>
    /// <response code="200">Sale events retrieved successfully</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMySaleEvents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetSaleEventsBySellerQuery(page, pageSize);
        return await SendAsync(query, cancellationToken);
    }

    [HttpGet("seller/{sellerId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSaleEventsBySeller(
        Guid sellerId, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var query = new GetSaleEventsBySellerQuery(page, pageSize);
        return await SendAsync(query, cancellationToken);
    }

    /// <summary>
    /// Retrieves all active sale events for a specific listing
    /// </summary>
    /// <param name="listingId">Listing ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of active sale events for the listing</returns>
    /// <response code="200">Active sale events retrieved successfully</response>
    [HttpGet("listing/{listingId:guid}/active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActiveSaleEventsForListing(Guid listingId, CancellationToken cancellationToken)
    {
        var query = new GetActiveSaleEventsForListingQuery(listingId);
        return await SendAsync(query, cancellationToken);
    }

    // 8.2 - Tier management endpoints
    
    /// <summary>
    /// Adds a new discount tier to a sale event
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="request">Tier details including discount type, value, and priority</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result</returns>
    /// <response code="200">Tier added successfully</response>
    /// <response code="400">Invalid tier configuration or maximum tier count exceeded</response>
    /// <response code="404">Sale event not found</response>
    [HttpPost("{id:guid}/tiers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddTier(Guid id, [FromBody] AddTierRequest request, CancellationToken cancellationToken)
    {
        var command = new AddTierCommand(
            id,
            request.DiscountType,
            request.DiscountValue,
            request.Priority,
            request.Label
        );

        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Removes a discount tier from a sale event
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="tierId">Tier ID to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result</returns>
    /// <response code="200">Tier removed successfully</response>
    /// <response code="404">Sale event or tier not found</response>
    [HttpDelete("{id:guid}/tiers/{tierId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveTier(Guid id, Guid tierId, CancellationToken cancellationToken)
    {
        var command = new RemoveTierCommand(id, tierId);
        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Updates the priority of a discount tier
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="tierId">Tier ID</param>
    /// <param name="request">New priority value</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result</returns>
    /// <response code="200">Tier priority updated successfully</response>
    /// <response code="400">Invalid priority or duplicate priority</response>
    /// <response code="404">Sale event or tier not found</response>
    [HttpPut("{id:guid}/tiers/{tierId:guid}/priority")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTierPriority(
        Guid id, 
        Guid tierId, 
        [FromBody] UpdateTierPriorityRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTierPriorityCommand(id, tierId, request.NewPriority);
        return await SendAsync(command, cancellationToken);
    }

    // 8.3 - Listing assignment endpoints
    
    /// <summary>
    /// Assigns multiple listings to a discount tier (supports bulk assignment up to 1000 listings)
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="request">Tier ID and list of listing IDs to assign</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result with assignment details</returns>
    /// <response code="200">Listings assigned successfully</response>
    /// <response code="400">Invalid listings or assignment conflicts</response>
    /// <response code="404">Sale event or tier not found</response>
    [HttpPost("{id:guid}/listings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignListingsToTier(Guid id, [FromBody] AssignListingsRequest request, CancellationToken cancellationToken)
    {
        var command = new AssignListingsToTierCommand(id, request.TierId, request.ListingIds);
        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Removes a listing assignment from a sale event
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="listingId">Listing ID to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result</returns>
    /// <response code="200">Listing assignment removed successfully</response>
    /// <response code="404">Sale event or listing assignment not found</response>
    [HttpDelete("{id:guid}/listings/{listingId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveListingAssignment(Guid id, Guid listingId, CancellationToken cancellationToken)
    {
        var command = new RemoveListingAssignmentCommand(id, listingId);
        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Reassigns a listing to a different tier within the same sale event
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="listingId">Listing ID to reassign</param>
    /// <param name="request">New tier ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success or error result</returns>
    /// <response code="200">Listing reassigned successfully</response>
    /// <response code="404">Sale event, listing, or tier not found</response>
    [HttpPut("{id:guid}/listings/{listingId:guid}/reassign")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReassignListing(
        Guid id, 
        Guid listingId, 
        [FromBody] ReassignListingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReassignListingCommand(id, listingId, request.NewTierId);
        return await SendAsync(command, cancellationToken);
    }

    /// <summary>
    /// Retrieves eligible listings for sale event assignment with filtering and pagination
    /// </summary>
    /// <param name="searchTerm">Search term for listing title</param>
    /// <param name="categoryId">Filter by category ID</param>
    /// <param name="minPrice">Minimum price filter</param>
    /// <param name="maxPrice">Maximum price filter</param>
    /// <param name="minDaysOnSite">Minimum days on site filter</param>
    /// <param name="excludeAlreadyAssigned">Exclude listings already assigned to sale events</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Items per page (default: 20)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of eligible listings</returns>
    /// <response code="200">Eligible listings retrieved successfully</response>
    [HttpGet("eligible-listings")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEligibleListings(
        [FromQuery] string? searchTerm,
        [FromQuery] Guid? categoryId,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int? minDaysOnSite,
        [FromQuery] bool excludeAlreadyAssigned = false,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var query = new GetSaleEventEligibleListingsQuery(
            searchTerm,
            categoryId,
            minPrice,
            maxPrice,
            minDaysOnSite,
            excludeAlreadyAssigned,
            pageNumber,
            pageSize
        );

        return await SendAsync(query, cancellationToken);
    }

    // 8.4 - Performance metrics endpoint
    
    /// <summary>
    /// Retrieves performance metrics for a sale event with optional date range filtering
    /// </summary>
    /// <param name="id">Sale event ID</param>
    /// <param name="startDate">Start date for metrics filtering (optional)</param>
    /// <param name="endDate">End date for metrics filtering (optional)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Performance metrics including order count, discount amount, revenue, and tier-level breakdown</returns>
    /// <response code="200">Performance metrics retrieved successfully</response>
    /// <response code="404">Sale event not found</response>
    [HttpGet("{id:guid}/performance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPerformanceMetrics(
        Guid id,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        CancellationToken cancellationToken)
    {
        var query = new GetSaleEventPerformanceQuery(id, startDate, endDate);
        return await SendAsync(query, cancellationToken);
    }
}

// Request DTOs

/// <summary>
/// Request model for creating a new sale event
/// </summary>
public record CreateSaleEventRequest(
    /// <summary>Sale event name (max 200 characters)</summary>
    string Name,
    /// <summary>Seller user ID</summary>
    Guid SellerId,
    /// <summary>Sale start date and time</summary>
    DateTime StartDate,
    /// <summary>Sale end date and time</summary>
    DateTime EndDate,
    /// <summary>Highlight mode (1=DiscountAndSaleEvent, 2=SaleEventOnly)</summary>
    SaleEventMode Mode,
    /// <summary>Highlight percentage for SaleEventOnly mode (optional)</summary>
    decimal? HighlightPercentage,
    /// <summary>Enable free shipping for all items in sale</summary>
    bool OfferFreeShipping,
    /// <summary>Block price increases while sale is active</summary>
    bool BlockPriceIncreaseRevisions,
    /// <summary>Include listings previously skipped in past sales</summary>
    bool IncludeSkippedItems,
    /// <summary>Sale description (optional)</summary>
    string? Description,
    /// <summary>Custom message label shown to buyers (max 100 characters, optional)</summary>
    string? BuyerMessageLabel,
    /// <summary>List of discount tiers with listing assignments (optional)</summary>
    List<TierDefinitionRequest>? Tiers
);

/// <summary>
/// Discount tier definition for sale event creation
/// </summary>
public record TierDefinitionRequest(
    /// <summary>Discount type (1=Percent, 2=Amount)</summary>
    SaleEventDiscountType DiscountType,
    /// <summary>Discount value (0.01-100 for percent, >0 for amount)</summary>
    decimal DiscountValue,
    /// <summary>Tier priority (1 = highest, must be unique)</summary>
    int Priority,
    /// <summary>Tier label (optional)</summary>
    string Label,
    /// <summary>List of listing IDs to assign to this tier</summary>
    List<Guid> ListingIds
);

/// <summary>
/// Request model for updating an existing sale event
/// </summary>
public record UpdateSaleEventRequest(
    /// <summary>Updated sale event name (optional)</summary>
    string? Name,
    /// <summary>Updated description (optional)</summary>
    string? Description,
    /// <summary>Updated start date (optional)</summary>
    DateTime? StartDate,
    /// <summary>Updated end date (optional)</summary>
    DateTime? EndDate,
    /// <summary>Updated buyer message label (optional)</summary>
    string? BuyerMessageLabel,
    /// <summary>Updated free shipping option (optional)</summary>
    bool? OfferFreeShipping,
    /// <summary>Updated price increase block option (optional)</summary>
    bool? BlockPriceIncreaseRevisions,
    /// <summary>Updated include skipped items option (optional)</summary>
    bool? IncludeSkippedItems
);

/// <summary>
/// Request model for adding a discount tier to a sale event
/// </summary>
public record AddTierRequest(
    /// <summary>Discount type (1=Percent, 2=Amount)</summary>
    SaleEventDiscountType DiscountType,
    /// <summary>Discount value (0.01-100 for percent, >0 for amount)</summary>
    decimal DiscountValue,
    /// <summary>Tier priority (1 = highest, must be unique)</summary>
    int Priority,
    /// <summary>Tier label (optional)</summary>
    string? Label
);

/// <summary>
/// Request model for updating tier priority
/// </summary>
public record UpdateTierPriorityRequest(
    /// <summary>New priority value (must be unique within sale event)</summary>
    int NewPriority
);

/// <summary>
/// Request model for assigning listings to a tier
/// </summary>
public record AssignListingsRequest(
    /// <summary>Target tier ID</summary>
    Guid TierId,
    /// <summary>List of listing IDs to assign (max 1000)</summary>
    List<Guid> ListingIds
);

/// <summary>
/// Request model for reassigning a listing to a different tier
/// </summary>
public record ReassignListingRequest(
    /// <summary>New tier ID</summary>
    Guid NewTierId
);
