# Requirements Document

## Introduction

The Order Discount feature enables sellers to create automatic discounts that apply to orders without requiring buyers to enter coupon codes. This system supports two primary discount types: spend-based discounts (threshold amount) and quantity-based discounts (threshold quantity). The feature aligns with eBay's order discount implementation, providing sellers with flexible promotional tools to increase sales while offering buyers transparent, automatic savings during checkout.

## Glossary

- **Order_Discount_System**: The subsystem responsible for managing, validating, and applying order-level discounts
- **Discount_Engine**: The component that calculates discount amounts based on order criteria
- **Seller**: A user with active seller status who can create and manage order discounts
- **Buyer**: A user who purchases items and receives automatic discount application
- **Shopping_Cart**: The collection of items a buyer intends to purchase
- **Order_Subtotal**: The sum of all item prices before shipping, taxes, and discounts
- **Threshold_Amount**: The minimum order subtotal required to trigger a spend-based discount
- **Threshold_Quantity**: The minimum number of items required to trigger a quantity-based discount
- **Discount_Tier**: A level within a multi-tier discount structure with specific threshold and discount value
- **Eligible_Item**: An item that qualifies for discount application based on inclusion/exclusion rules
- **Discount_Priority**: The ranking system that determines which discount applies when multiple discounts are eligible
- **Fixed_Price_Item**: An item listed with Buy It Now pricing (not auction-style)

## Requirements

### Requirement 1: Create Spend-Based Order Discount

**User Story:** As a seller, I want to create spend-based order discounts, so that I can incentivize buyers to increase their order value.

#### Acceptance Criteria

1. THE Order_Discount_System SHALL allow Seller to create a discount with threshold amount
2. WHEN creating a spend-based discount, THE Order_Discount_System SHALL require discount name, threshold amount, discount value, and discount unit (percentage or fixed amount)
3. WHEN discount unit is percentage, THE Order_Discount_System SHALL validate that discount value is between 0.01 and 100
4. WHEN discount unit is fixed amount, THE Order_Discount_System SHALL validate that discount value is greater than 0
5. THE Order_Discount_System SHALL allow Seller to set optional maximum discount amount
6. THE Order_Discount_System SHALL allow Seller to set start date and end date for the discount
7. WHEN end date is provided, THE Order_Discount_System SHALL validate that end date is after start date
8. THE Order_Discount_System SHALL store the discount with status active by default

### Requirement 2: Create Quantity-Based Order Discount

**User Story:** As a seller, I want to create quantity-based order discounts, so that I can encourage buyers to purchase multiple items.

#### Acceptance Criteria

1. THE Order_Discount_System SHALL allow Seller to create a discount with threshold quantity
2. WHEN creating a quantity-based discount, THE Order_Discount_System SHALL require discount name, threshold quantity, discount value, and discount unit
3. THE Order_Discount_System SHALL validate that threshold quantity is a positive integer greater than 0
4. WHEN discount unit is percentage, THE Order_Discount_System SHALL validate that discount value is between 0.01 and 100
5. WHEN discount unit is fixed amount, THE Order_Discount_System SHALL validate that discount value is greater than 0
6. THE Order_Discount_System SHALL allow Seller to set optional maximum discount amount
7. THE Order_Discount_System SHALL allow Seller to set start date and end date for the discount

### Requirement 3: Configure Multi-Tier Discounts

**User Story:** As a seller, I want to create multi-tier discounts, so that I can offer increasing savings for higher spending or quantity thresholds.

#### Acceptance Criteria

1. THE Order_Discount_System SHALL allow Seller to define multiple discount tiers for a single discount
2. WHEN adding a discount tier, THE Order_Discount_System SHALL require threshold value and discount value
3. THE Order_Discount_System SHALL validate that each tier has a higher threshold than the previous tier
4. THE Order_Discount_System SHALL validate that each tier has a discount value greater than or equal to the previous tier
5. THE Order_Discount_System SHALL support up to 10 tiers per discount
6. WHEN calculating discount, THE Discount_Engine SHALL apply the highest tier for which the order qualifies

### Requirement 4: Select Eligible Items and Categories

**User Story:** As a seller, I want to specify which items or categories qualify for the discount, so that I can target specific products for promotion.

#### Acceptance Criteria

1. THE Order_Discount_System SHALL allow Seller to apply discount to all items by default
2. THE Order_Discount_System SHALL allow Seller to select specific items for discount eligibility
3. THE Order_Discount_System SHALL allow Seller to select specific categories for discount eligibility
4. WHEN both items and categories are selected, THE Order_Discount_System SHALL include items that match either condition
5. THE Order_Discount_System SHALL validate that at least one item or category is eligible when not applying to all items
6. WHEN calculating eligibility, THE Discount_Engine SHALL only count eligible items toward threshold requirements

### Requirement 5: Exclude Specific Items from Discount

**User Story:** As a seller, I want to exclude specific items from discounts, so that I can protect margins on certain products while still offering promotions.

#### Acceptance Criteria

1. THE Order_Discount_System SHALL allow Seller to exclude specific items from discount application
2. THE Order_Discount_System SHALL allow Seller to exclude specific categories from discount application
3. WHEN an item is both included and excluded, THE Order_Discount_System SHALL exclude the item
4. WHEN calculating discount, THE Discount_Engine SHALL not count excluded items toward threshold requirements
5. WHEN calculating discount amount, THE Discount_Engine SHALL not apply discount to excluded items

### Requirement 6: Automatic Discount Application in Cart

**User Story:** As a buyer, I want to see order discounts automatically applied in my shopping cart, so that I know my savings before checkout.

#### Acceptance Criteria

1. WHEN Buyer views Shopping_Cart, THE Discount_Engine SHALL evaluate all active order discounts from the seller
2. WHEN order meets discount threshold, THE Discount_Engine SHALL automatically apply the discount
3. THE Shopping_Cart SHALL display the discount name and discount amount as a separate line item
4. THE Shopping_Cart SHALL display the order subtotal before discount and final total after discount
5. WHEN order does not meet threshold, THE Shopping_Cart SHALL display a message indicating how much more is needed to qualify
6. WHEN multiple tiers exist, THE Shopping_Cart SHALL display the next tier threshold and additional savings available

### Requirement 7: Validate Discount Eligibility

**User Story:** As a system, I want to validate discount eligibility in real-time, so that only qualified orders receive discounts.

#### Acceptance Criteria

1. THE Discount_Engine SHALL validate that discount is active
2. THE Discount_Engine SHALL validate that current date is between start date and end date
3. THE Discount_Engine SHALL validate that order contains only fixed price items
4. WHEN order contains eligible items, THE Discount_Engine SHALL calculate Order_Subtotal using only eligible item prices
5. WHEN threshold is amount-based, THE Discount_Engine SHALL compare Order_Subtotal to threshold amount
6. WHEN threshold is quantity-based, THE Discount_Engine SHALL count only eligible items toward threshold quantity
7. IF discount has maximum discount amount, THEN THE Discount_Engine SHALL cap calculated discount at maximum amount

### Requirement 8: Calculate Discount Amount

**User Story:** As a system, I want to accurately calculate discount amounts, so that buyers receive correct savings and sellers maintain expected margins.

#### Acceptance Criteria

1. WHEN discount unit is percentage, THE Discount_Engine SHALL calculate discount as Order_Subtotal multiplied by discount value divided by 100
2. WHEN discount unit is fixed amount, THE Discount_Engine SHALL use discount value as the discount amount
3. WHEN calculated discount exceeds maximum discount amount, THE Discount_Engine SHALL apply maximum discount amount
4. WHEN calculated discount exceeds Order_Subtotal, THE Discount_Engine SHALL cap discount at Order_Subtotal
5. THE Discount_Engine SHALL round discount amount to 2 decimal places
6. THE Discount_Engine SHALL ensure discount amount is never negative
7. THE Discount_Engine SHALL apply discount before tax calculation

### Requirement 9: Handle Multiple Discount Priority

**User Story:** As a system, I want to apply discount priority rules when multiple discounts are eligible, so that the most beneficial discount is applied consistently.

#### Acceptance Criteria

1. WHEN multiple order discounts are eligible, THE Discount_Engine SHALL compare discount amounts for each eligible discount
2. THE Discount_Engine SHALL apply the discount that provides the highest discount amount to the buyer
3. THE Discount_Engine SHALL apply only one order discount per order
4. WHEN order discount and coupon are both applicable, THE Discount_Engine SHALL allow both to apply
5. WHEN order discount and sale event are both applicable, THE Discount_Engine SHALL apply only the discount with higher value
6. THE Shopping_Cart SHALL display which discount was applied and why other discounts were not applied

### Requirement 10: Manage Discount Lifecycle

**User Story:** As a seller, I want to manage my order discounts throughout their lifecycle, so that I can activate, deactivate, edit, and delete promotions as needed.

#### Acceptance Criteria

1. THE Order_Discount_System SHALL allow Seller to view all created order discounts
2. THE Order_Discount_System SHALL allow Seller to edit discount details before start date
3. THE Order_Discount_System SHALL allow Seller to deactivate an active discount
4. WHEN discount is deactivated, THE Order_Discount_System SHALL immediately stop applying the discount to new orders
5. THE Order_Discount_System SHALL allow Seller to reactivate a deactivated discount
6. THE Order_Discount_System SHALL allow Seller to delete a discount that has not been applied to any orders
7. THE Order_Discount_System SHALL prevent deletion of discounts that have been applied to orders

### Requirement 11: Track Discount Performance

**User Story:** As a seller, I want to view analytics for my order discounts, so that I can measure their effectiveness and ROI.

#### Acceptance Criteria

1. THE Order_Discount_System SHALL track the number of orders that applied each discount
2. THE Order_Discount_System SHALL track the total discount amount given for each discount
3. THE Order_Discount_System SHALL track the total sales revenue from orders using each discount
4. THE Order_Discount_System SHALL track the quantity of items sold through each discount
5. THE Order_Discount_System SHALL calculate average order value for orders using each discount
6. THE Order_Discount_System SHALL allow Seller to view performance metrics for a specific date range
7. THE Order_Discount_System SHALL display performance metrics grouped by discount

### Requirement 12: Enforce Price Change Waiting Period

**User Story:** As a system, I want to enforce a waiting period after price changes before including items in new discounts, so that discount integrity is maintained.

#### Acceptance Criteria

1. THE Order_Discount_System SHALL track the last price change date for each item
2. WHEN Seller creates a new discount, THE Order_Discount_System SHALL exclude items with price changes within the past 14 days
3. WHEN item price is changed, THE Order_Discount_System SHALL automatically exclude the item from new discounts for 14 days
4. THE Order_Discount_System SHALL allow items in existing active discounts to remain eligible despite price changes
5. THE Order_Discount_System SHALL notify Seller when items are excluded due to recent price changes

### Requirement 13: Display Discount Information to Buyers

**User Story:** As a buyer, I want to see available order discounts on product pages and search results, so that I am aware of potential savings before adding items to cart.

#### Acceptance Criteria

1. WHEN Buyer views a product page for an eligible item, THE Order_Discount_System SHALL display active order discount information
2. THE Order_Discount_System SHALL display discount description in format "Save [amount/percentage] when you spend [threshold]" for spend-based discounts
3. THE Order_Discount_System SHALL display discount description in format "Save [amount/percentage] when you buy [threshold]+ items" for quantity-based discounts
4. WHEN multiple tiers exist, THE Order_Discount_System SHALL display the highest tier savings
5. THE Order_Discount_System SHALL display discount end date if discount expires within 7 days
6. THE Order_Discount_System SHALL not display expired or inactive discounts

### Requirement 14: Validate Discount at Checkout

**User Story:** As a system, I want to revalidate discount eligibility at checkout, so that discounts are only applied to valid orders.

#### Acceptance Criteria

1. WHEN Buyer proceeds to checkout, THE Discount_Engine SHALL revalidate all applied discounts
2. IF discount has expired between cart view and checkout, THEN THE Discount_Engine SHALL remove the discount and notify Buyer
3. IF discount has been deactivated between cart view and checkout, THEN THE Discount_Engine SHALL remove the discount and notify Buyer
4. IF order no longer meets threshold requirements, THEN THE Discount_Engine SHALL remove the discount and notify Buyer
5. IF item eligibility has changed, THEN THE Discount_Engine SHALL recalculate discount amount
6. THE Discount_Engine SHALL apply the revalidated discount to the final order total

### Requirement 15: Handle Discount with Partial Returns

**User Story:** As a system, I want to adjust discount amounts when orders are partially returned, so that discount is proportional to items kept.

#### Acceptance Criteria

1. WHEN Buyer returns items from a discounted order, THE Order_Discount_System SHALL recalculate discount eligibility based on remaining items
2. IF remaining items no longer meet threshold requirements, THEN THE Order_Discount_System SHALL remove the discount from the order
3. IF remaining items still meet threshold but at a lower tier, THEN THE Order_Discount_System SHALL adjust discount to the appropriate tier
4. THE Order_Discount_System SHALL calculate refund amount including the adjusted discount difference
5. THE Order_Discount_System SHALL update discount performance metrics to reflect the return
