# Implementation Plan: Sale Event System

## Overview

This implementation plan converts the Sale Event System design into actionable coding tasks. The system enables sellers to run time-limited promotional sales with tiered discount structures, supporting two highlight modes (DiscountAndSaleEvent and SaleEventOnly). The implementation follows a layered architecture: Domain → Application → Infrastructure → API → Frontend, with comprehensive testing throughout.

The system integrates with the existing discount infrastructure (IDiscount interface) and extends the current SaleEvent entity to support advanced features like multi-tier discounts, eligibility validation, checkout revalidation, and performance tracking.

## Tasks

- [x] 1. Set up domain layer foundation
  - Create enums (SaleEventMode, SaleEventStatus, SaleEventDiscountType)
  - Create value objects (SaleEventDiscountTierDefinition, SalePriceCalculationResult)
  - Create entity classes (SaleEventDiscountTier, SaleEventListing, SaleEventPerformanceMetrics, SaleEventPriceSnapshot)
  - Files: `BE/.../Domain/Discounts/Enums/`, `BE/.../Domain/Discounts/ValueObjects/`, `BE/.../Domain/Discounts/Entities/`
  - _Requirements: 1.2, 2.2, 2.5, 5.1, 6.1_

- [ ]* 1.1 Write property tests for domain foundation
  - **Property 1: Sale Event Creation with Valid Data**
  - **Property 2: Required Fields Validation**
  - **Property 3: Name Length Validation**
  - **Property 6: Default Status**
  - **Validates: Requirements 1.1, 1.2, 1.3, 1.6**

- [x] 2. Enhance SaleEvent aggregate root entity
  - [x] 2.1 Add new properties and collections
    - Add Mode, HighlightPercentage, OfferFreeShipping, BlockPriceIncreaseRevisions, IncludeSkippedItems, Status
    - Add collections: _discountTiers, _listings
    - Implement IDiscount interface
    - File: `BE/.../Domain/Discounts/Entities/SaleEvent.cs`
    - _Requirements: 1.1, 4.1, 4.3, 4.5, 5.1, 6.1_
  
  - [x] 2.2 Implement factory and update methods
    - Create() factory method with mode-specific validation
    - Update() with status-based restrictions
    - Status management: Activate(), Deactivate(), UpdateStatus()
    - File: `BE/.../Domain/Discounts/Entities/SaleEvent.cs`
    - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 6.1, 6.4, 6.6, 11.2_
  
  - [x] 2.3 Implement tier management methods
    - AddTier(), RemoveTier(), UpdateTierPriority()
    - Validate tier constraints (max 10 tiers, unique priorities, discount ranges)
    - File: `BE/.../Domain/Discounts/Entities/SaleEvent.cs`
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.6, 2.7, 2.8_
  
  - [x] 2.4 Implement listing assignment methods
    - AssignListingToTier(), RemoveListingAssignment(), ReassignListing(), BulkAssignListings()
    - Validate listing uniqueness per sale event
    - Support bulk operations up to 1000 listings
    - File: `BE/.../Domain/Discounts/Entities/SaleEvent.cs`
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 18.1, 18.5_
  
  - [x] 2.5 Implement price calculation and eligibility
    - CalculateSalePrice() for tier-based discount calculation
    - IsListingEligible() for eligibility validation
    - CalculateDiscount() for IDiscount interface
    - Handle both Percent and Amount discount types
    - Enforce non-negative pricing and 2 decimal place rounding
    - File: `BE/.../Domain/Discounts/Entities/SaleEvent.cs`
    - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 9.1, 9.2, 9.3_

- [ ]* 2.6 Write property tests for SaleEvent entity
  - **Property 4: Date Range Validity**
  - **Property 5: Optional Buyer Message Label**
  - **Property 7: Seller Association**
  - **Property 8: Tier Addition**
  - **Property 9: Tier Required Fields**
  - **Property 10: Listing Ownership Validation**
  - **Property 11: Listing Uniqueness Per Sale Event**
  - **Property 12: Tier Priority Positivity**
  - **Property 13: Tier Priority Uniqueness**
  - **Property 14: Discount Type Support**
  - **Property 15: Percentage Discount Range**
  - **Property 16: Fixed Amount Discount Positivity**
  - **Property 17: Maximum Tier Count**
  - **Property 18: Listing Assignment Removal**
  - **Property 19: Listing Reassignment**
  - **Validates: Requirements 1.4, 1.5, 1.7, 2.1-2.8, 3.2-3.6**

- [ ]* 2.7 Write property tests for sale options and modes
  - **Property 20: Free Shipping Option Configuration**
  - **Property 21: Free Shipping Application**
  - **Property 22: Price Increase Block Configuration**
  - **Property 23: Price Increase Prevention**
  - **Property 24: Price Decrease Permission**
  - **Property 25: Include Skipped Items Option**
  - **Property 26: Sale Options Persistence**
  - **Property 27: Highlight Mode Selection**
  - **Property 28: Discount Calculation in DiscountAndSaleEvent Mode**
  - **Property 29: No Discount Calculation in SaleEventOnly Mode**
  - **Property 30: Default Highlight Mode**
  - **Validates: Requirements 4.1-4.6, 5.1-5.6**

- [ ]* 2.8 Write property tests for lifecycle and calculations
  - **Property 31: Sale Event Activation**
  - **Property 32: Activation Validation for Tier Assignments**
  - **Property 33: Activation Date Range Validation**
  - **Property 34: Sale Event Deactivation**
  - **Property 35: Immediate Deactivation Effect**
  - **Property 36: Sale Event Reactivation**
  - **Property 37: Percentage Discount Calculation**
  - **Property 38: Fixed Amount Discount Calculation**
  - **Property 39: Non-Negative Sale Price**
  - **Property 40: Sale Price Rounding**
  - **Property 41: Price Change Recalculation**
  - **Validates: Requirements 6.1-6.6, 7.1-7.7**

- [x] 3. Checkpoint - Domain layer complete
  - Ensure all domain entities compile without errors
  - Verify entity relationships and constraints
  - Ask the user if questions arise

- [x] 4. Implement application services
  - [x] 4.1 Create SaleEventPriceCalculator service
    - CalculateSalePrice(): Find tier assignment, apply discount calculation, enforce rounding
    - CalculateDiscountAmount(): Handle percentage and fixed amount discounts
    - CalculateBulkSalePrices(): Batch calculation for multiple listings
    - Validate non-negative pricing
    - File: `BE/.../Application/SaleEvents/Services/SaleEventPriceCalculator.cs`
    - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5, 7.7_
  
  - [x] 4.2 Create SaleEventEligibilityValidator service
    - ValidateListingEligibility(): Check active, published, fixed price, seller ownership
    - ValidateSaleEventEligibility(): Check active status, date range
    - ValidateListingForTierAssignment(): Pre-assignment validation
    - GetEligibleListings(): Query eligible listings with filters
    - File: `BE/.../Application/SaleEvents/Services/SaleEventEligibilityValidator.cs`
    - _Requirements: 9.1, 9.2, 9.3, 9.4, 9.5, 9.6, 20.1, 20.2, 20.3, 20.4_
  
  - [x] 4.3 Create command and query DTOs
    - Commands: CreateSaleEventCommand, UpdateSaleEventCommand, ActivateSaleEventCommand, DeactivateSaleEventCommand, AssignListingsToTierCommand, DeleteSaleEventCommand
    - DTOs: SaleEventDto, SaleEventDiscountTierDto, SaleEventPerformanceDto, TierPerformanceDto, ListingDto
    - Folders: `BE/.../Application/SaleEvents/Commands/`, `BE/.../Application/SaleEvents/Dtos/`
    - _Requirements: 1.1, 6.1, 6.4, 11.1, 12.1-12.5_
  
  - [x] 4.4 Implement command handlers
    - CreateSaleEventCommandHandler with validation
    - UpdateSaleEventCommandHandler with status-based restrictions
    - ActivateSaleEventCommandHandler with eligibility checks and price snapshot creation
    - DeactivateSaleEventCommandHandler
    - AssignListingsToTierCommandHandler with bulk support
    - DeleteSaleEventCommandHandler with activation check
    - Folder: `BE/.../Application/SaleEvents/Commands/`
    - _Requirements: 1.1-1.7, 2.1-2.8, 3.1-3.6, 6.1-6.6, 11.2-11.6, 13.1, 18.1-18.5_
  
  - [x] 4.5 Implement query handlers
    - GetSaleEventByIdQueryHandler
    - GetSaleEventsBySellerQueryHandler with pagination
    - GetActiveSaleEventsForListingQueryHandler
    - GetPerformanceMetricsQueryHandler with date range filter and tier aggregation
    - GetEligibleListingsQueryHandler with search, category, price filters
    - Folder: `BE/.../Application/SaleEvents/Queries/`
    - _Requirements: 11.1, 12.1-12.7_
  
  - [x] 4.6 Update DiscountPriorityService
    - Add sale event to discount priority resolution
    - Implement priority rules: sale event vs order discount (best wins), sale event + coupon (both apply)
    - Handle multiple sale events (lowest price wins)
    - File: `BE/.../Application/OrderDiscounts/Services/DiscountPriorityService.cs`
    - _Requirements: 10.1, 10.2, 10.3, 10.4, 10.5_

- [ ]* 4.7 Write property tests for eligibility and priority
  - **Property 42: End Date Display Threshold**
  - **Property 43: Inactive Sale Event Filtering**
  - **Property 44: Active Status Validation**
  - **Property 45: Date Range Eligibility**
  - **Property 46: Tier Assignment Validation**
  - **Property 47: Eligible Listing Sale Pricing**
  - **Property 48: Ineligible Listing No Pricing**
  - **Property 49: Multiple Sale Events Comparison**
  - **Property 50: Best Sale Price Selection**
  - **Property 51: Single Sale Event Application**
  - **Property 52: Sale Event vs Order Discount Priority**
  - **Property 53: Sale Event and Coupon Stacking**
  - **Validates: Requirements 8.5, 8.6, 9.1-9.6, 10.1-10.5**

- [ ]* 4.8 Write unit tests for application services
  - Test SaleEventPriceCalculator with specific examples (10% off $50 = $45, 20% off $100 = $80)
  - Test SaleEventEligibilityValidator with edge cases (exactly 7 days to expiration)
  - Test command handlers with error conditions (invalid data, missing fields)
  - Test query handlers with pagination and filtering
  - Test DiscountPriorityService with multiple discount combinations
  - File: `BE/.../Tests/SaleEvents/UnitTests/ServiceTests.cs`

- [x] 5. Checkpoint - Application layer complete
  - Ensure all services compile without errors
  - Verify command and query handlers work correctly
  - Ask the user if questions arise

- [x] 6. Implement infrastructure layer
  - [x] 6.1 Create repository interface and implementation
    - ISaleEventRepository interface with all CRUD methods
    - SaleEventRepository with EF Core implementation
    - Include related entities (tiers, listings) with proper eager loading
    - Files: `BE/.../Application/Abstractions/Data/ISaleEventRepository.cs`, `BE/.../Infrastructure/Repositories/SaleEventRepository.cs`
    - _Requirements: 1.1, 3.1, 11.1_
  
  - [x] 6.2 Create EF Core configurations
    - SaleEventConfiguration with owned collections
    - SaleEventDiscountTierConfiguration
    - SaleEventListingConfiguration
    - SaleEventPerformanceMetricsConfiguration
    - SaleEventPriceSnapshotConfiguration
    - Configure relationships, constraints, and indexes
    - Folder: `BE/.../Infrastructure/Configurations/`
  
  - [x] 6.3 Create database migration
    - Add 6 new tables: SaleEvents, SaleEventDiscountTiers, SaleEventListings, SaleEventPerformanceMetrics, SaleEventPriceSnapshots, AppliedSaleEvents
    - Add indexes for performance (SellerId, Status, ListingId, SaleEventId)
    - Add unique constraints (tier priority per sale event, listing assignment per sale event)
    - Migration file: `BE/.../Infrastructure/Migrations/`
  
  - [x] 6.4 Update Listings table for price tracking
    - Add LastPriceChangeDate column to Listings table
    - Create migration
    - Update Listing entity to track price changes
    - _Requirements: 13.1, 13.2_
  
  - [x] 6.5 Update DiscountType enum
    - Add SaleEvent = 4 to DiscountType enum
    - File: `BE/.../Domain/Discounts/Enums/DiscountType.cs`

- [ ]* 6.6 Write integration tests for repository
  - Test CRUD operations with related entities
  - Test query performance with indexes
    - Test eager loading of tiers and listings
    - Test filtering by seller, status, and date range
    - File: `BE/.../Tests/SaleEvents/IntegrationTests/RepositoryTests.cs`

- [x] 7. Checkpoint - Infrastructure layer complete
  - Run database migration on development environment
  - Verify schema changes and indexes
  - Test repository operations
  - Ask the user if questions arise

- [x] 8. Implement API layer
  - [x] 8.1 Create SaleEventsController
    - POST /api/sale-events (create with mode, tiers, options)
    - PUT /api/sale-events/{id} (update)
    - POST /api/sale-events/{id}/activate
    - POST /api/sale-events/{id}/deactivate
    - DELETE /api/sale-events/{id}
    - GET /api/sale-events/{id}
    - GET /api/sale-events/seller/{sellerId} (with pagination)
    - File: `BE/.../Api/Controllers/SaleEventsController.cs`
    - _Requirements: 1.1-1.7, 6.1-6.6, 11.1-11.6_
  
  - [x] 8.2 Add tier management endpoints
    - POST /api/sale-events/{id}/tiers (add tier)
    - DELETE /api/sale-events/{id}/tiers/{tierId}
    - PUT /api/sale-events/{id}/tiers/{tierId}/priority
    - File: `BE/.../Api/Controllers/SaleEventsController.cs`
    - _Requirements: 2.1-2.8, 19.3, 19.4_
  
  - [x] 8.3 Add listing assignment endpoints
    - POST /api/sale-events/{id}/listings (assign listings to tier with bulk support)
    - DELETE /api/sale-events/{id}/listings/{listingId}
    - PUT /api/sale-events/{id}/listings/{listingId}/reassign
    - GET /api/sale-events/eligible-listings (with filters)
    - File: `BE/.../Api/Controllers/SaleEventsController.cs`
    - _Requirements: 3.1-3.6, 18.1-18.5, 20.1-20.4_
  
  - [x] 8.4 Add performance metrics endpoint
    - GET /api/sale-events/{id}/performance (with date range)
    - Return event-level and tier-level metrics
    - File: `BE/.../Api/Controllers/SaleEventsController.cs`
    - _Requirements: 12.1-12.7_

- [ ]* 8.5 Write integration tests for API
  - Test all controller endpoints with valid and invalid data
  - Test authentication and authorization
    - Test pagination and filtering
    - Test bulk operations (up to 1000 listings)
    - Test error responses and status codes
    - File: `BE/.../Tests/SaleEvents/IntegrationTests/SaleEventsControllerTests.cs`

- [x] 9. Checkpoint - API layer complete
  - Test all endpoints with Postman or similar tool
  - Verify request/response formats
  - Update API documentation with Swagger annotations
  - Ask the user if questions arise

- [x] 10. Implement frontend sale event management
  - [x] 10.1 Create sale event creation page
    - Form with name, description, buyer message label
    - Date range picker (start date, end date)
    - Highlight mode selection (DiscountAndSaleEvent vs SaleEventOnly)
    - Multi-tier configuration (add/remove tiers with priority, discount type, discount value, label)
    - Tier priority management with drag-and-drop reordering
    - Listing selection per tier with search, category filter, price filter
    - Sale options: free shipping, price increase block, include skipped items
    - Validation and error handling
    - File: `FE/src/pages/Marketing/SaleEvent/CreateSaleEvent.jsx`
    - _Requirements: 1.1-1.7, 2.1-2.8, 3.1-3.6, 4.1-4.6, 5.1-5.6_
  
  - [x] 10.2 Create sale event edit page
    - Load existing sale event data
    - Allow editing before start date only (except tier assignments)
    - Allow tier assignment editing at any time
    - Validation and error handling
    - File: `FE/src/pages/Marketing/SaleEvent/EditSaleEvent.jsx`
    - _Requirements: 11.2, 11.3_
  
  - [x] 10.3 Create sale event list/management page
    - Table view with filters (status, mode, date range)
    - Actions: View, Edit, Activate, Deactivate, Delete, Duplicate
    - Performance metrics display (order count, total discount, total revenue)
    - Status badges (Draft, Scheduled, Active, Ended, Cancelled)
    - File: `FE/src/pages/Marketing/SaleEvent/SaleEventList.jsx`
    - _Requirements: 11.1, 11.4, 11.5, 11.6, 12.1-12.5_
  
  - [x] 10.4 Create sale event performance analytics page
    - Performance metrics dashboard with charts
    - Date range filter
    - Tier-level performance breakdown
    - Metrics: order count, total discount, total revenue, items sold, average discount per order, average order value
    - Export functionality
    - File: `FE/src/pages/Marketing/SaleEvent/SaleEventAnalytics.jsx`
    - _Requirements: 12.1-12.7_

- [ ]* 10.5 Write unit tests for sale event management components
  - Test form validation (name, dates, tiers, options)
  - Test tier management UI (add, remove, reorder)
  - Test listing selection and filtering
  - Test mode switching (DiscountAndSaleEvent vs SaleEventOnly)
  - File: `FE/src/pages/Marketing/SaleEvent/__tests__/`

- [x] 11. Implement frontend buyer-facing features
  - [x] 11.1 Integrate sale event display on product pages
    - Fetch active sale events for listing
    - Display sale badge/banner
    - Show original price with strike-through and sale price (DiscountAndSaleEvent mode)
    - Show original price with strike-through only (SaleEventOnly mode)
    - Display buyer message label if configured
    - Show sale end date if expires within 7 days
    - Show free shipping badge if applicable
    - File: `FE/src/pages/Listing/ListingDetail.jsx`
    - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5, 14.4_
  
  - [x] 11.2 Integrate sale event display in shopping cart
    - Evaluate active sale events for cart items
    - Display applied sale event with name and discount amount
    - Show original price with strike-through and sale price
    - Handle multiple discount types (sale event, order discount, coupon)
    - Apply discount priority rules
    - Update cart total calculation
    - File: `FE/src/components/Cart/ShoppingCart.jsx`
    - _Requirements: 7.1-7.7, 10.1-10.5_
  
  - [x] 11.3 Implement checkout sale event revalidation
    - Revalidate sale events before order finalization
    - Handle expired/deactivated sale events
    - Handle removed listing assignments
    - Notify user of changes with clear messaging
    - Update order total if sale event removed
    - File: `FE/src/pages/Checkout/CheckoutPage.jsx`
    - _Requirements: 15.1, 15.2, 15.3, 15.4, 15.5_

- [ ]* 11.4 Write integration tests for buyer-facing features
  - Test sale badge display on product pages
  - Test strike-through pricing in both modes
  - Test cart sale event evaluation and discount priority
  - Test checkout revalidation scenarios (expired, deactivated, unassigned)
  - Files: `FE/src/pages/Listing/__tests__/`, `FE/src/components/Cart/__tests__/`, `FE/src/pages/Checkout/__tests__/`

- [x] 12. Checkpoint - Frontend complete
  - Test all UI flows end-to-end
  - Verify responsive design
  - Test error handling and user notifications
  - Ask the user if questions arise

- [x] 13. Implement advanced features
  - [x] 13.1 Implement price increase blocking
    - Create price snapshots on sale event activation
    - Validate price changes against snapshots
    - Reject price increases, allow price decreases
    - Remove restrictions on deactivation
    - Notify seller when price increase is blocked
    - _Requirements: 13.1, 13.2, 13.3, 13.4, 13.5_
  
  - [x] 13.2 Implement free shipping application
    - Mark listings as eligible for free shipping
    - Apply free shipping at checkout
    - Remove free shipping on deactivation
    - Display free shipping badge
    - _Requirements: 14.1, 14.2, 14.3, 14.4_
  
  - [x] 13.3 Implement return handling
    - Refund sale price (not original price)
    - Track applied sale event and tier
    - Update performance metrics for returns
    - Adjust total sales revenue and quantity sold
    - _Requirements: 16.1, 16.2, 16.3, 16.4_
  
  - [x] 13.4 Implement performance tracking
    - Record order applications with sale event details
    - Track order count, discount amount, sales revenue, items sold
    - Calculate average metrics
    - Support date range filtering
    - Aggregate by tier
    - _Requirements: 12.1-12.7_

- [ ]* 13.5 Write property tests for advanced features
  - **Property 54: Edit Before Start Date**
  - **Property 55: Tier Assignment Editing**
  - **Property 56: Delete Inactive Sale Events**
  - **Property 57: Sale Event Duplication**
  - **Property 58: Order Count Tracking**
  - **Property 59: Total Discount Amount Tracking**
  - **Property 60: Total Sales Revenue Tracking**
  - **Property 61: Total Items Sold Tracking**
  - **Property 62: Average Discount Per Order Calculation**
  - **Property 63: Price Snapshot Creation**
  - **Property 64: Price Restriction Removal**
  - **Property 65: Free Shipping Eligibility Marking**
  - **Property 66: Free Shipping at Checkout**
  - **Property 67: Free Shipping Removal**
  - **Property 68: Checkout Revalidation**
  - **Property 69: Expired Sale Event Removal at Checkout**
  - **Property 70: Deactivated Sale Event Removal at Checkout**
  - **Property 71: Unassigned Listing Removal at Checkout**
  - **Property 72: Revalidated Pricing Application**
  - **Property 73: Return Sale Price Refund**
  - **Property 74: Sale Event Tracking with Orders**
  - **Property 75: Return Metrics Adjustment**
  - **Validates: Requirements 11.2-11.6, 12.1-12.7, 13.1-13.5, 14.1-14.4, 15.1-15.5, 16.1-16.4**

- [ ]* 13.6 Write property tests for bulk operations and validation
  - **Property 76: Listing Assignment Conflict Detection**
  - **Property 77: Multiple Sale Event Assignment**
  - **Property 78: Bulk Listing Selection**
  - **Property 79: Bulk Assignment Validation**
  - **Property 80: Bulk Assignment Execution**
  - **Property 81: Bulk Assignment Result Reporting**
  - **Property 82: Bulk Assignment Limit**
  - **Property 83: Tier Priority Reordering**
  - **Property 84: Priority Uniqueness Maintenance**
  - **Property 85: Active Listing Validation**
  - **Property 86: Fixed Price Listing Validation**
  - **Property 87: Ineligible Listing Rejection**
  - **Validates: Requirements 17.1-17.3, 18.1-18.5, 19.3-19.4, 20.1-20.4**

- [x] 14. Final integration and deployment
  - [x] 14.1 Create data migration script
    - Migrate existing SaleEvent records to new schema
    - Set default values for new fields (Mode, Status, Options)
    - Create initial performance metrics records
    - File: `BE/PRN232_EbayClone/migration_script_sale_events.sql`
  
  - [x] 14.2 Update system documentation
    - Update DISCOUNT_SYSTEM_GUIDE.md with sale event overview and priority rules
    - Add API documentation with Swagger annotations
    - Create user guide for sale event management
    - Files: `BE/PRN232_EbayClone/DISCOUNT_SYSTEM_GUIDE.md`, `docs/SaleEventUserGuide.md`
  
  - [x] 14.3 Run database migration
    - Execute migration on development environment
    - Verify schema changes and indexes
    - Run data migration script
    - Test rollback procedure
  
  - [x] 14.4 Performance testing
    - Test sale price calculation performance (<50ms target)
    - Test bulk eligibility validation (1000/sec target)
    - Test checkout revalidation performance
    - Test performance metrics aggregation
    - Optimize queries if needed
  
  - [x] 14.5 Deploy to staging
    - Deploy backend changes
    - Deploy frontend changes
    - Run smoke tests
    - Verify sale event creation, activation, and application flows

- [x] 15. Final checkpoint - Ensure all tests pass
  - Ensure all tests pass, ask the user if questions arise

## Notes

- Tasks marked with `*` are optional and can be skipped for faster MVP
- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation
- Property tests validate universal correctness properties
- Unit tests validate specific examples and edge cases
- The implementation uses C# for backend and React for frontend
- The system integrates with existing discount infrastructure (IDiscount interface)
- Sale events support two highlight modes: DiscountAndSaleEvent (applies discounts) and SaleEventOnly (display only)
- The system enforces strict validation: max 10 tiers, unique priorities, listing uniqueness per sale event
- Bulk operations support up to 1000 listings at once
- Performance metrics track order count, discount amount, sales revenue, and items sold at both event and tier levels
