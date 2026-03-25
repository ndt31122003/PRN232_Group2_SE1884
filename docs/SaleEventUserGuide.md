# Sale Event User Guide

## Table of Contents
1. [Introduction](#introduction)
2. [Getting Started](#getting-started)
3. [Creating a Sale Event](#creating-a-sale-event)
4. [Managing Sale Events](#managing-sale-events)
5. [Performance Analytics](#performance-analytics)
6. [Best Practices](#best-practices)
7. [Troubleshooting](#troubleshooting)

## Introduction

Sale Events allow sellers to run time-limited promotional sales on selected items with tiered discount structures. This feature provides flexible promotional tools to drive sales during specific time periods while offering buyers transparent savings.

### Key Features
- **Multi-tier discounts**: Create up to 10 discount tiers with different priority levels
- **Two highlight modes**: Choose between applying discounts or display-only mode
- **Sale options**: Offer free shipping, block price increases, include previously skipped items
- **Bulk operations**: Assign up to 1000 listings at once
- **Performance tracking**: Monitor order count, revenue, and discount metrics
- **Automatic revalidation**: System validates sale eligibility at checkout

## Getting Started

### Accessing Sale Events
1. Navigate to the Marketing section in the main menu
2. Click on "Promotions" or go directly to `/marketing/sale-events`
3. You'll see a list of all your sale events with their current status

### Sale Event Statuses
- **Draft**: Sale event created but not yet activated
- **Scheduled**: Sale event scheduled to start in the future
- **Active**: Sale event currently running
- **Ended**: Sale event has passed its end date
- **Cancelled**: Sale event was manually deactivated

## Creating a Sale Event

### Step 1: Basic Information
1. Click "Create Sale Event" button
2. Fill in the required fields:
   - **Name**: Give your sale a descriptive name (max 200 characters)
   - **Description**: Optional detailed description
   - **Start Date**: When the sale begins
   - **End Date**: When the sale ends (must be after start date)
   - **Buyer Message Label**: Optional custom message shown to buyers (max 100 characters)

### Step 2: Choose Highlight Mode

#### Discount and Sale Event Mode (Recommended)
- Applies tiered markdown discounts to assigned listings
- Buyers see original price with strike-through and discounted sale price
- Best for: Traditional sales with actual price reductions

#### Sale Event Only Mode
- Displays strike-through pricing without applying discounts
- Buyers see original price with strike-through only
- Best for: Highlighting items without changing prices

### Step 3: Configure Sale Options

#### Free Shipping
- Enable to offer free shipping on all items in the sale
- Free shipping badge will be displayed on product pages
- Applied automatically at checkout

#### Block Price Increases
- Prevents price increases on assigned listings while sale is active
- Price decreases are still allowed
- Protects sale integrity and buyer trust

#### Include Skipped Items
- Include listings that were previously skipped in past sales
- Useful for giving items a second chance in promotions

### Step 4: Create Discount Tiers

1. Click "Add Tier" to create a new discount tier
2. For each tier, configure:
   - **Priority**: Ranking from 1 (highest) to 10 (lowest)
   - **Discount Type**: Choose Percentage Off or Fixed Amount Off
   - **Discount Value**: 
     - Percentage: 0.01% to 100%
     - Fixed Amount: Any positive value
   - **Label**: Optional label for the tier (e.g., "Premium Discount")

**Important Rules**:
- Maximum 10 tiers per sale event
- Each tier must have a unique priority
- Priority 1 is the highest priority tier

### Step 5: Assign Listings to Tiers

#### Individual Assignment
1. Select a tier
2. Click "Assign Listings"
3. Search and filter listings:
   - Search by title or SKU
   - Filter by category
   - Filter by price range
4. Select listings and click "Assign"

#### Bulk Assignment
1. Select a tier
2. Click "Bulk Assign"
3. Select multiple listings (up to 1000 at once)
4. Click "Assign All"
5. Review the assignment report for any failures

**Eligibility Requirements**:
- Listing must be active and published
- Listing must have fixed price (not auction-style)
- Listing must belong to you (the seller)
- Listing can only be in one tier per sale event

### Step 6: Review and Activate

1. Review all sale event details
2. Verify tier configurations and listing assignments
3. Click "Activate" to start the sale
4. System will validate:
   - At least one tier has assigned listings
   - Current date is between start and end dates
   - All configurations are valid

## Managing Sale Events

### Editing Sale Events

#### Before Start Date
- You can edit all sale event details
- Change name, description, dates, mode, options
- Modify tier configurations

#### After Start Date
- Basic details are locked (name, dates, mode)
- You can still edit tier assignments
- Add or remove listings from tiers at any time

### Activating and Deactivating

#### Manual Activation
1. Open the sale event
2. Click "Activate"
3. System validates eligibility
4. Sale becomes active immediately

#### Manual Deactivation
1. Open an active sale event
2. Click "Deactivate"
3. Sale pricing stops immediately
4. You can reactivate later if within date range

#### Automatic Activation/Deactivation
- Sale automatically activates at start date (if validated)
- Sale automatically ends at end date
- System updates status in real-time

### Deleting Sale Events
- You can only delete sale events that have never been activated
- Once activated, sale events cannot be deleted (for audit purposes)
- Deactivate instead if you need to stop a sale

### Duplicating Sale Events
1. Open an existing sale event
2. Click "Duplicate"
3. System creates a copy with new dates
4. All tier configurations and options are copied
5. Listing assignments are NOT copied (assign manually)

## Performance Analytics

### Accessing Analytics
1. Navigate to sale event list
2. Click on a sale event
3. Go to "Analytics" tab

### Key Metrics

#### Event-Level Metrics
- **Order Count**: Total number of orders containing items from this sale
- **Total Discount Amount**: Sum of all discounts given
- **Total Sales Revenue**: Total revenue from sale items
- **Total Items Sold**: Quantity of items sold through the sale
- **Average Discount Per Order**: Average discount amount per order
- **Average Order Value**: Average revenue per order

#### Tier-Level Metrics
- View performance breakdown by each discount tier
- Compare which tiers are most effective
- Identify best-performing discount levels

### Date Range Filtering
- Filter metrics by custom date range
- Compare performance across different time periods
- Export data for external analysis

### Export Functionality
- Export performance data to CSV
- Include event-level and tier-level metrics
- Use for reporting and analysis

## Best Practices

### Planning Your Sale

1. **Set Clear Goals**
   - Define what you want to achieve (revenue, inventory clearance, customer acquisition)
   - Set realistic discount levels based on margins

2. **Choose the Right Duration**
   - Short sales (1-3 days): Create urgency
   - Medium sales (1 week): Balance urgency and reach
   - Long sales (2+ weeks): Maximize participation

3. **Structure Tiers Strategically**
   - Tier 1 (Highest): Best-selling or premium items
   - Tier 2-3: Mid-range items
   - Lower tiers: Clearance or slow-moving inventory

### Optimizing Performance

1. **Use Buyer Message Labels**
   - Create urgency: "Limited Time Only!"
   - Highlight value: "Save up to 50%"
   - Build trust: "Lowest Prices Guaranteed"

2. **Enable Free Shipping**
   - Increases conversion rates
   - Reduces cart abandonment
   - Competitive advantage

3. **Block Price Increases**
   - Maintains buyer trust
   - Prevents accidental price changes
   - Protects sale integrity

4. **Monitor Analytics Daily**
   - Track performance in real-time
   - Adjust strategy if needed
   - Identify top-performing tiers

### Listing Assignment Strategy

1. **Start with High-Value Items**
   - Assign best-sellers to highest priority tiers
   - Use lower tiers for clearance items

2. **Use Bulk Assignment**
   - Save time with bulk operations
   - Assign similar items together
   - Review assignment report for errors

3. **Review Eligibility**
   - Ensure all listings are active
   - Verify fixed-price format
   - Check for conflicts with other sales

## Troubleshooting

### Common Issues

#### "Cannot activate sale event"
**Possible causes**:
- No listings assigned to any tier
- Current date is outside start/end date range
- Invalid tier configurations

**Solution**:
- Verify at least one tier has listings
- Check date range is valid
- Review tier configurations for errors

#### "Listing assignment failed"
**Possible causes**:
- Listing is not active or published
- Listing is auction-style (not fixed price)
- Listing already assigned to another tier in this sale
- Listing belongs to another seller

**Solution**:
- Check listing status and format
- Remove from other tiers if needed
- Verify listing ownership

#### "Price increase blocked"
**Cause**: Sale event has "Block Price Increases" enabled

**Solution**:
- This is expected behavior during active sales
- Wait until sale ends to increase prices
- Or deactivate the sale event

#### "Sale pricing not showing at checkout"
**Possible causes**:
- Sale event expired between cart and checkout
- Sale event was deactivated
- Listing was removed from tier assignment

**Solution**:
- System automatically revalidates at checkout
- Buyer will be notified of any changes
- Sale pricing is removed if no longer eligible

### Getting Help

If you encounter issues not covered in this guide:
1. Check the API documentation at `/swagger`
2. Review system logs for error messages
3. Contact support with:
   - Sale event ID
   - Error message or screenshot
   - Steps to reproduce the issue

## API Reference

For developers integrating with the Sale Event API, see:
- Swagger documentation: `/swagger`
- API testing guide: `BE/PRN232_EbayClone/SALE_EVENTS_API_TESTING.md`
- Discount system guide: `BE/PRN232_EbayClone/DISCOUNT_SYSTEM_GUIDE.md`

## Appendix

### Sale Event Lifecycle Diagram

```
Draft → Scheduled → Active → Ended
  ↓         ↓         ↓
Cancelled ← ← ← ← ← ←
```

### Discount Priority Rules

When multiple discounts apply:
1. Sale Event vs Order Discount: Best value wins
2. Sale Event + Coupon: Both apply (stack)
3. Multiple Sale Events: Lowest price wins

### Maximum Limits

- **Tiers per sale**: 10
- **Listings per bulk assignment**: 1000
- **Sale event name**: 200 characters
- **Buyer message label**: 100 characters
- **Percentage discount**: 0.01% - 100%
- **Fixed amount discount**: > $0

---

**Last Updated**: January 2024  
**Version**: 1.0
