# Requirements Document

## Introduction

The Sale Event feature enables sellers to run time-limited promotional sales on selected items with tiered discount structures. This system supports two highlight modes: discount tiers (markdown tiers with multiple priority levels) and highlight only (strike-through pricing without tiered discounts). The feature aligns with eBay's sale event implementation, providing sellers with flexible promotional tools to drive sales during specific time periods while offering buyers transparent savings on participating listings.

## Glossary

- **Sale_Event_System**: The subsystem responsible for managing, validating, and applying sale event discounts
- **Sale_Event**: A time-limited promotional campaign with discount tiers and listing assignments
- **Discount_Tier**: A priority level within a sale event with specific discount type and value
- **Tier_Priority**: The ranking of discount tiers where 1 is the highest priority
- **Seller**: A user with active seller status who can create and manage sale events
- **Buyer**: A user who views listings and receives sale pricing
- **Listing**: An item listed for sale that can be assigned to a sale event tier
- **Highlight_Mode**: The display mode for sale events (discount tiers or highlight only)
- **Discount_Tiers_Mode**: A highlight mode that applies tiered markdown discounts to listings
- **Highlight_Only_Mode**: A highlight mode that shows strike-through pricing without applying tiered discounts
- **Free_Shipping_Option**: A sale option that offers free shipping on all items in the sale
- **Price_Increase_Block**: A sale option that prevents price increases while the sale is active
- **Buyer_Message_Label**: An optional custom message displayed to buyers during the sale
- **Sale_Price**: The discounted price shown to buyers when a sale event is active

## Requirements

### Requirement 1: Create Sale Event

**User Story:** As a seller, I want to create sale events, so that I can run time-limited promotions on selected items.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL allow Seller to create a sale event with name, start date, and end date
2. WHEN creating a sale event, THE Sale_Event_System SHALL require sale event name, start date, and end date
3. THE Sale_Event_System SHALL validate that sale event name is not empty and does not exceed 200 characters
4. WHEN end date is provided, THE Sale_Event_System SHALL validate that end date is after start date
5. THE Sale_Event_System SHALL allow Seller to set optional buyer message label with maximum 100 characters
6. THE Sale_Event_System SHALL store the sale event with status inactive by default
7. THE Sale_Event_System SHALL associate the sale event with the Seller who created it

### Requirement 2: Configure Discount Tiers

**User Story:** As a seller, I want to create multiple discount tiers with different priorities, so that I can offer varying discount levels for different items.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL allow Seller to add discount tiers to a sale event
2. WHEN adding a discount tier, THE Sale_Event_System SHALL require tier priority, discount type, and discount value
3. THE Sale_Event_System SHALL validate that tier priority is a positive integer
4. THE Sale_Event_System SHALL validate that each tier has a unique priority within the sale event
5. THE Sale_Event_System SHALL support discount types of percentage off and fixed amount off
6. WHEN discount type is percentage off, THE Sale_Event_System SHALL validate that discount value is between 0.01 and 100
7. WHEN discount type is fixed amount off, THE Sale_Event_System SHALL validate that discount value is greater than 0
8. THE Sale_Event_System SHALL support up to 10 tiers per sale event

### Requirement 3: Assign Listings to Tiers

**User Story:** As a seller, I want to assign specific listings to each discount tier, so that I can control which items receive which discount levels.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL allow Seller to assign listings to a specific discount tier
2. THE Sale_Event_System SHALL validate that assigned listings belong to the Seller
3. THE Sale_Event_System SHALL validate that a listing can only be assigned to one tier per sale event
4. WHEN a listing is assigned to a tier, THE Sale_Event_System SHALL prevent the listing from being assigned to another tier in the same sale event
5. THE Sale_Event_System SHALL allow Seller to remove listing assignments from tiers
6. THE Sale_Event_System SHALL allow Seller to reassign a listing to a different tier within the same sale event

### Requirement 4: Configure Sale Options

**User Story:** As a seller, I want to configure sale options, so that I can offer additional benefits and protect pricing integrity during the sale.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL allow Seller to enable free shipping option for the sale event
2. WHEN free shipping option is enabled, THE Sale_Event_System SHALL apply free shipping to all listings in the sale event
3. THE Sale_Event_System SHALL allow Seller to enable price increase block option
4. WHEN price increase block is enabled, THE Sale_Event_System SHALL prevent price increases on listings assigned to the sale event while the sale is active
5. THE Sale_Event_System SHALL allow Seller to include listings previously skipped in past sales
6. THE Sale_Event_System SHALL store sale option configurations with the sale event

### Requirement 5: Select Highlight Mode

**User Story:** As a seller, I want to choose between discount tiers mode and highlight only mode, so that I can control how sale pricing is displayed to buyers.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL allow Seller to select highlight mode when creating a sale event
2. THE Sale_Event_System SHALL support Discount_Tiers_Mode as a highlight mode option
3. THE Sale_Event_System SHALL support Highlight_Only_Mode as a highlight mode option
4. WHEN Discount_Tiers_Mode is selected, THE Sale_Event_System SHALL apply tiered markdown discounts to assigned listings
5. WHEN Highlight_Only_Mode is selected, THE Sale_Event_System SHALL display strike-through pricing without applying tiered discounts
6. THE Sale_Event_System SHALL default to Discount_Tiers_Mode if no highlight mode is specified

### Requirement 6: Activate and Deactivate Sale Events

**User Story:** As a seller, I want to activate and deactivate sale events, so that I can control when promotions are live.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL allow Seller to activate a sale event
2. WHEN a sale event is activated, THE Sale_Event_System SHALL validate that at least one tier has assigned listings
3. WHEN a sale event is activated, THE Sale_Event_System SHALL validate that current date is between start date and end date
4. THE Sale_Event_System SHALL allow Seller to deactivate an active sale event
5. WHEN a sale event is deactivated, THE Sale_Event_System SHALL immediately stop applying sale pricing to assigned listings
6. THE Sale_Event_System SHALL allow Seller to reactivate a deactivated sale event

### Requirement 7: Calculate Sale Price

**User Story:** As a system, I want to calculate sale prices based on tier assignments, so that buyers see correct discounted prices.

#### Acceptance Criteria

1. WHEN a listing is assigned to a tier with percentage off, THE Sale_Event_System SHALL calculate sale price as original price multiplied by (1 minus discount value divided by 100)
2. WHEN a listing is assigned to a tier with fixed amount off, THE Sale_Event_System SHALL calculate sale price as original price minus discount value
3. THE Sale_Event_System SHALL validate that calculated sale price is not negative
4. WHEN calculated sale price is negative, THE Sale_Event_System SHALL set sale price to zero
5. THE Sale_Event_System SHALL round sale price to 2 decimal places
6. WHEN highlight mode is Highlight_Only_Mode, THE Sale_Event_System SHALL not apply discount calculations
7. THE Sale_Event_System SHALL recalculate sale price when listing original price changes

### Requirement 8: Display Sale Information to Buyers

**User Story:** As a buyer, I want to see sale event information on listings, so that I am aware of promotional pricing.

#### Acceptance Criteria

1. WHEN Buyer views a listing in an active sale event, THE Sale_Event_System SHALL display sale pricing information
2. WHEN highlight mode is Discount_Tiers_Mode, THE Sale_Event_System SHALL display original price with strike-through and sale price
3. WHEN highlight mode is Highlight_Only_Mode, THE Sale_Event_System SHALL display original price with strike-through only
4. THE Sale_Event_System SHALL display buyer message label if configured by Seller
5. THE Sale_Event_System SHALL display sale end date if sale expires within 7 days
6. THE Sale_Event_System SHALL not display sale information for inactive or expired sale events

### Requirement 9: Validate Sale Event Eligibility

**User Story:** As a system, I want to validate sale event eligibility in real-time, so that only qualified listings receive sale pricing.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL validate that sale event is active
2. THE Sale_Event_System SHALL validate that current date is between start date and end date
3. THE Sale_Event_System SHALL validate that listing is assigned to a tier in the sale event
4. THE Sale_Event_System SHALL validate that listing belongs to the Seller who created the sale event
5. WHEN all validations pass, THE Sale_Event_System SHALL apply sale pricing to the listing
6. WHEN any validation fails, THE Sale_Event_System SHALL not apply sale pricing to the listing

### Requirement 10: Handle Multiple Sale Event Priority

**User Story:** As a system, I want to apply priority rules when a listing is in multiple sale events, so that the most beneficial sale is applied consistently.

#### Acceptance Criteria

1. WHEN a listing is assigned to multiple active sale events, THE Sale_Event_System SHALL compare sale prices for each eligible sale event
2. THE Sale_Event_System SHALL apply the sale event that provides the lowest sale price to the buyer
3. THE Sale_Event_System SHALL apply only one sale event per listing
4. WHEN sale event and order discount are both applicable, THE Sale_Event_System SHALL apply only the discount with higher value
5. WHEN sale event and coupon are both applicable, THE Sale_Event_System SHALL allow both to apply

### Requirement 11: Manage Sale Event Lifecycle

**User Story:** As a seller, I want to manage my sale events throughout their lifecycle, so that I can edit, activate, deactivate, and delete promotions as needed.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL allow Seller to view all created sale events
2. THE Sale_Event_System SHALL allow Seller to edit sale event details before start date
3. THE Sale_Event_System SHALL allow Seller to edit tier assignments at any time
4. THE Sale_Event_System SHALL allow Seller to delete a sale event that has not been activated
5. THE Sale_Event_System SHALL prevent deletion of sale events that have been activated
6. THE Sale_Event_System SHALL allow Seller to duplicate an existing sale event with new dates

### Requirement 12: Track Sale Event Performance

**User Story:** As a seller, I want to view analytics for my sale events, so that I can measure their effectiveness and ROI.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL track the number of orders containing items from each sale event
2. THE Sale_Event_System SHALL track the total discount amount given for each sale event
3. THE Sale_Event_System SHALL track the total sales revenue from items in each sale event
4. THE Sale_Event_System SHALL track the quantity of items sold through each sale event
5. THE Sale_Event_System SHALL calculate average discount per order for each sale event
6. THE Sale_Event_System SHALL allow Seller to view performance metrics for a specific date range
7. THE Sale_Event_System SHALL display performance metrics grouped by sale event and by tier

### Requirement 13: Enforce Price Increase Block

**User Story:** As a system, I want to enforce price increase blocking when enabled, so that sale integrity is maintained.

#### Acceptance Criteria

1. WHEN price increase block is enabled for a sale event, THE Sale_Event_System SHALL track original prices of assigned listings at activation time
2. WHEN Seller attempts to increase price on a listing in an active sale event with price increase block, THE Sale_Event_System SHALL reject the price change
3. THE Sale_Event_System SHALL allow price decreases on listings in active sale events with price increase block
4. WHEN sale event ends or is deactivated, THE Sale_Event_System SHALL remove price increase restrictions
5. THE Sale_Event_System SHALL notify Seller when price increase is blocked

### Requirement 14: Apply Free Shipping Option

**User Story:** As a system, I want to apply free shipping when enabled, so that buyers receive shipping benefits during sales.

#### Acceptance Criteria

1. WHEN free shipping option is enabled for a sale event, THE Sale_Event_System SHALL mark all assigned listings as eligible for free shipping
2. THE Sale_Event_System SHALL apply free shipping at checkout for items from sale events with free shipping enabled
3. WHEN sale event ends or is deactivated, THE Sale_Event_System SHALL remove free shipping from previously assigned listings
4. THE Sale_Event_System SHALL display free shipping badge on listings in sale events with free shipping enabled

### Requirement 15: Validate Sale Event at Checkout

**User Story:** As a system, I want to revalidate sale event eligibility at checkout, so that sale pricing is only applied to valid orders.

#### Acceptance Criteria

1. WHEN Buyer proceeds to checkout, THE Sale_Event_System SHALL revalidate all applied sale pricing
2. IF sale event has expired between cart view and checkout, THEN THE Sale_Event_System SHALL remove sale pricing and notify Buyer
3. IF sale event has been deactivated between cart view and checkout, THEN THE Sale_Event_System SHALL remove sale pricing and notify Buyer
4. IF listing has been removed from tier assignment, THEN THE Sale_Event_System SHALL remove sale pricing and notify Buyer
5. THE Sale_Event_System SHALL apply the revalidated sale pricing to the final order total

### Requirement 16: Handle Sale Event with Returns

**User Story:** As a system, I want to handle returns for items purchased during sale events, so that refunds reflect the sale price paid.

#### Acceptance Criteria

1. WHEN Buyer returns an item purchased during a sale event, THE Sale_Event_System SHALL refund the sale price paid
2. THE Sale_Event_System SHALL track which sale event and tier were applied to the original order
3. THE Sale_Event_System SHALL update sale event performance metrics to reflect the return
4. THE Sale_Event_System SHALL adjust total sales revenue and quantity sold for the affected sale event

### Requirement 17: Prevent Conflicting Tier Assignments

**User Story:** As a system, I want to prevent conflicting tier assignments, so that listings have clear discount application.

#### Acceptance Criteria

1. WHEN a listing is assigned to a tier in a sale event, THE Sale_Event_System SHALL check for existing assignments in the same sale event
2. IF listing is already assigned to a tier in the same sale event, THEN THE Sale_Event_System SHALL reject the new assignment
3. THE Sale_Event_System SHALL allow listing to be assigned to tiers in different sale events
4. THE Sale_Event_System SHALL provide clear error messages when tier assignment conflicts occur

### Requirement 18: Support Bulk Listing Assignment

**User Story:** As a seller, I want to assign multiple listings to a tier at once, so that I can efficiently configure large sales.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL allow Seller to select multiple listings for tier assignment
2. THE Sale_Event_System SHALL validate all selected listings before assignment
3. WHEN bulk assignment is performed, THE Sale_Event_System SHALL assign all valid listings to the specified tier
4. THE Sale_Event_System SHALL report which listings were successfully assigned and which failed validation
5. THE Sale_Event_System SHALL support bulk assignment of up to 1000 listings at once

### Requirement 19: Display Tier Priority to Seller

**User Story:** As a seller, I want to see tier priorities clearly, so that I understand the discount hierarchy.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL display tiers ordered by priority (1 = highest)
2. THE Sale_Event_System SHALL show tier priority, discount type, discount value, and assigned listing count for each tier
3. THE Sale_Event_System SHALL allow Seller to reorder tier priorities
4. WHEN tier priority is changed, THE Sale_Event_System SHALL update all affected tier priorities to maintain uniqueness

### Requirement 20: Validate Listing Eligibility for Sale Events

**User Story:** As a system, I want to validate listing eligibility for sale events, so that only appropriate items are included.

#### Acceptance Criteria

1. THE Sale_Event_System SHALL validate that listing is active and published
2. THE Sale_Event_System SHALL validate that listing has fixed price (not auction-style)
3. THE Sale_Event_System SHALL validate that listing belongs to the Seller creating the sale event
4. WHEN listing does not meet eligibility criteria, THE Sale_Event_System SHALL reject tier assignment
5. THE Sale_Event_System SHALL provide clear error messages for ineligible listings
