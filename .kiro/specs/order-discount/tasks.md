# Tasks Document: Order Discount System

## Task Status Legend
- `todo`: Not started | `in-progress`: Working | `done`: Completed | `blocked`: Waiting on dependencies

---

## Phase 1: Domain Layer (Backend)

### 1.1 Create OrderDiscountThresholdType Enum
**Status**: `todo` | **Priority**: High | **Effort**: 1h | **Dependencies**: None
- Create enum with SpendBased=1, QuantityBased=2
- File: `BE/.../Domain/Discounts/Enums/OrderDiscountThresholdType.cs`

### 1.2 Create OrderDiscountTier Entity
**Status**: `todo` | **Priority**: High | **Effort**: 2h | **Dependencies**: None
- Properties: OrderDiscountId, ThresholdValue, DiscountValue, TierOrder
- Factory method with validation (threshold > 0, discount > 0)
- File: `BE/.../Domain/Discounts/Entities/OrderDiscountTier.cs`

### 1.3 Create OrderDiscountItemRule Entity
**Status**: `todo` | **Priority**: High | **Effort**: 1h | **Dependencies**: None
- Properties: OrderDiscountId, ListingId, IsExclusion
- Factory method with validation
- File: `BE/.../Domain/Discounts/Entities/OrderDiscountItemRule.cs`

### 1.4 Create OrderDiscountCategoryRule Entity
**Status**: `todo` | **Priority**: High | **Effort**: 1h | **Dependencies**: None
- Properties: OrderDiscountId, CategoryId, IsExclusion
- Factory method with validation
- File: `BE/.../Domain/Discounts/Entities/OrderDiscountCategoryRule.cs`

### 1.5 Create OrderDiscountPerformanceMetrics Entity
**Status**: `todo` | **Priority**: Medium | **Effort**: 2h | **Dependencies**: None
- Properties: OrderCount, TotalDiscountAmount, TotalSalesRevenue, TotalItemsSold
- Methods: RecordOrder(), AdjustForReturn()
- File: `BE/.../Domain/Discounts/Entities/OrderDiscountPerformanceMetrics.cs`

### 1.6 Create DiscountCalculationResult Value Object
**Status**: `todo` | **Priority**: High | **Effort**: 1h | **Dependencies**: None
- Record: DiscountAmount, AppliedTier, EligibleItemIds, ExcludedItemIds, IneligibilityReason
- File: `BE/.../Domain/Discounts/ValueObjects/DiscountCalculationResult.cs`

### 1.7 Create OrderItem Value Object
**Status**: `todo` | **Priority**: High | **Effort**: 1h | **Dependencies**: None
- Record: ListingId, CategoryId, Price, Quantity, LastPriceChange
- File: `BE/.../Domain/Discounts/ValueObjects/OrderItem.cs`

### 1.8 Enhance OrderDiscount Entity
**Status**: `todo` | **Priority**: High | **Effort**: 4h | **Dependencies**: 1.1-1.7
- Add ThresholdType, ThresholdAmount, ThresholdQuantity, ApplyToAllItems
- Add collections: _tiers, _itemRules, _categoryRules
- Implement CreateSpendBased() and CreateQuantityBased() factory methods
- Implement AddTier(), RemoveTier(), AddItemRule(), AddCategoryRule()
- Implement IsItemEligible() and enhanced CalculateDiscount()
- Update existing file: `BE/.../Domain/Discounts/Entities/OrderDiscount.cs`

---

## Phase 2: Application Layer (Backend)

### 2.1 Create Commands
**Status**: `todo` | **Priority**: High | **Effort**: 3h | **Dependencies**: 1.8

- CreateSpendBasedDiscountCommand, CreateQuantityBasedDiscountCommand
- UpdateOrderDiscountCommand, AddDiscountTierCommand
- ConfigureItemRulesCommand, ConfigureCategoryRulesCommand
- Folder: `BE/.../Application/OrderDiscounts/Commands/`

### 2.2 Create Command Handlers
**Status**: `todo` | **Priority**: High | **Effort**: 5h | **Dependencies**: 2.1
- Handlers for all commands with validation
- Use repository pattern for persistence
- Folder: `BE/.../Application/OrderDiscounts/Commands/`

### 2.3 Create Queries
**Status**: `todo` | **Priority**: High | **Effort**: 2h | **Dependencies**: 1.8
- GetOrderDiscountByIdQuery, GetOrderDiscountsBySellerQuery
- GetActiveDiscountsForListingQuery, GetPerformanceMetricsQuery
- Folder: `BE/.../Application/OrderDiscounts/Queries/`

### 2.4 Create Query Handlers
**Status**: `todo` | **Priority**: High | **Effort**: 3h | **Dependencies**: 2.3
- Handlers for all queries
- Map entities to DTOs
- Folder: `BE/.../Application/OrderDiscounts/Queries/`

### 2.5 Create DTOs
**Status**: `todo` | **Priority**: High | **Effort**: 2h | **Dependencies**: None
- OrderDiscountDto, OrderDiscountTierDto, OrderDiscountPerformanceDto
- Folder: `BE/.../Application/OrderDiscounts/Dtos/`

### 2.6 Implement DiscountEngine Service
**Status**: `todo` | **Priority**: High | **Effort**: 6h | **Dependencies**: 1.8
- EvaluateOrderDiscount(): Check eligibility, calculate discount, apply tiers
- SelectBestDiscount(): Compare multiple discounts, return highest
- ValidateDiscountAtCheckout(): Revalidate before order finalization
- Enforce 14-day price change waiting period
- File: `BE/.../Application/OrderDiscounts/Services/DiscountEngine.cs`

### 2.7 Implement DiscountPriorityService
**Status**: `todo` | **Priority**: Medium | **Effort**: 3h | **Dependencies**: 2.6
- Resolve conflicts between order discount, coupon, sale event
- Apply priority rules (best discount wins)
- File: `BE/.../Application/OrderDiscounts/Services/DiscountPriorityService.cs`

---

## Phase 3: Infrastructure Layer (Backend)

### 3.1 Create IOrderDiscountRepository Interface
**Status**: `todo` | **Priority**: High | **Effort**: 1h | **Dependencies**: 1.8
- Methods: GetByIdAsync, GetBySellerIdAsync, GetActiveDiscountsAsync, HasBeenAppliedToOrdersAsync
- File: `BE/.../Application/Abstractions/Data/IOrderDiscountRepository.cs`

### 3.2 Implement OrderDiscountRepository
**Status**: `todo` | **Priority**: High | **Effort**: 4h | **Dependencies**: 3.1
- EF Core implementation with Include() for related entities
- Optimized queries with proper indexing
- File: `BE/.../Infrastructure/Repositories/OrderDiscountRepository.cs`

### 3.3 Create EF Core Configurations
**Status**: `todo` | **Priority**: High | **Effort**: 3h | **Dependencies**: 1.8
- OrderDiscountConfiguration, OrderDiscountTierConfiguration
- OrderDiscountItemRuleConfiguration, OrderDiscountCategoryRuleConfiguration
- OrderDiscountPerformanceMetricsConfiguration
- Folder: `BE/.../Infrastructure/Configurations/`

### 3.4 Create Database Migration
**Status**: `todo` | **Priority**: High | **Effort**: 2h | **Dependencies**: 3.3
- Add 5 new tables: OrderDiscounts, OrderDiscountTiers, OrderDiscountItemRules, OrderDiscountCategoryRules, OrderDiscountPerformanceMetrics, AppliedOrderDiscounts
- Add indexes for performance
- Migration file: `BE/.../Infrastructure/Migrations/`

### 3.5 Update Listings Table for Price Change Tracking
**Status**: `todo` | **Priority**: Medium | **Effort**: 2h | **Dependencies**: None
- Add LastPriceChangeDate column to Listings table
- Create migration
- Update Listing entity

---

## Phase 4: API Layer (Backend)

### 4.1 Create OrderDiscountsController
**Status**: `todo` | **Priority**: High | **Effort**: 4h | **Dependencies**: 2.2, 2.4
- POST /api/order-discounts/spend-based
- POST /api/order-discounts/quantity-based
- PUT /api/order-discounts/{id}
- POST /api/order-discounts/{id}/tiers
- POST /api/order-discounts/{id}/item-rules
- POST /api/order-discounts/{id}/category-rules
- POST /api/order-discounts/{id}/activate
- POST /api/order-discounts/{id}/deactivate
- DELETE /api/order-discounts/{id}
- GET /api/order-discounts/{id}
- GET /api/order-discounts/seller/{sellerId}
- GET /api/order-discounts/{id}/performance
- File: `BE/.../Api/Controllers/OrderDiscountsController.cs`

---

## Phase 5: Frontend (React)

### 5.1 Create Order Discount Creation Page
**Status**: `todo` | **Priority**: High | **Effort**: 8h | **Dependencies**: 4.1

- Form with discount type selection (spend-based vs quantity-based)
- Threshold configuration (amount or quantity)
- Discount value and unit (percent/fixed)
- Multi-tier support (add/remove tiers with validation)
- Item/category selection with inclusion/exclusion
- Date range picker
- File: `FE/src/pages/Marketing/OrderDiscount/CreateOrderDiscount.jsx`

### 5.2 Create Order Discount Edit Page
**Status**: `todo` | **Priority**: High | **Effort**: 6h | **Dependencies**: 5.1
- Load existing discount data
- Allow editing before start date only
- Validation and error handling
- File: `FE/src/pages/Marketing/OrderDiscount/EditOrderDiscount.jsx`

### 5.3 Create Order Discount List/Management Page
**Status**: `todo` | **Priority**: High | **Effort**: 5h | **Dependencies**: 4.1
- Table view with filters (status, type, date range)
- Actions: View, Edit, Activate, Deactivate, Delete
- Performance metrics display
- File: `FE/src/pages/Marketing/OrderDiscount/OrderDiscountList.jsx`

### 5.4 Integrate Discount Display in Shopping Cart
**Status**: `todo` | **Priority**: High | **Effort**: 4h | **Dependencies**: 2.6
- Evaluate active discounts for cart items
- Display applied discount with name and amount
- Show threshold progress ("Spend $X more to save Y")
- Show next tier information if applicable
- Update cart total calculation
- File: `FE/src/components/Cart/ShoppingCart.jsx`

### 5.5 Integrate Discount Display on Product Pages
**Status**: `todo` | **Priority**: Medium | **Effort**: 3h | **Dependencies**: 2.6
- Fetch active discounts for listing
- Display discount badge/banner
- Format: "Save X% when you spend $Y" or "Save X% when you buy Y+ items"
- Show highest tier for multi-tier discounts
- Show expiration warning if within 7 days
- File: `FE/src/pages/Listing/ListingDetail.jsx`

### 5.6 Implement Checkout Discount Revalidation
**Status**: `todo` | **Priority**: High | **Effort**: 3h | **Dependencies**: 2.6
- Revalidate discount before order finalization
- Handle expired/deactivated discounts
- Notify user of changes
- File: `FE/src/pages/Checkout/CheckoutPage.jsx`

### 5.7 Update Promotions Overview Page
**Status**: `todo` | **Priority**: Medium | **Effort**: 2h | **Dependencies**: 5.3
- Integrate order discount data into existing table
- Update filters to include order discount type
- File: `FE/src/pages/Marketing/PromotionsOverview.jsx`

---

## Phase 6: Testing (Backend)

### 6.1 Setup FsCheck for Property-Based Testing
**Status**: `todo` | **Priority**: High | **Effort**: 2h | **Dependencies**: None
- Install FsCheck NuGet package
- Create test project structure
- Configure test runners

### 6.2 Create Property Test Generators
**Status**: `todo` | **Priority**: High | **Effort**: 4h | **Dependencies**: 6.1
- ValidDiscountConfig generator
- OrderItem generator
- TierDefinitions generator
- File: `BE/.../Tests/OrderDiscounts/Generators/OrderDiscountGenerators.cs`

### 6.3 Implement Property Tests (Properties 1-20)
**Status**: `todo` | **Priority**: High | **Effort**: 8h | **Dependencies**: 6.2
- Test validation properties (1-7)
- Test tier properties (8-12)
- Test eligibility properties (13-17)
- Test application properties (18-20)
- File: `BE/.../Tests/OrderDiscounts/PropertyTests/OrderDiscountPropertyTests.cs`

### 6.4 Implement Property Tests (Properties 21-40)
**Status**: `todo` | **Priority**: High | **Effort**: 8h | **Dependencies**: 6.2
- Test calculation properties (21-29)
- Test priority properties (30-33)
- Test lifecycle properties (34-37)
- Test tracking properties (38-41)
- File: `BE/.../Tests/OrderDiscounts/PropertyTests/OrderDiscountPropertyTests2.cs`

### 6.5 Implement Property Tests (Properties 41-59)
**Status**: `todo` | **Priority**: High | **Effort**: 8h | **Dependencies**: 6.2
- Test price change properties (42-43)
- Test display properties (44-48)
- Test checkout properties (49-52)
- Test return properties (53-59)
- File: `BE/.../Tests/OrderDiscounts/PropertyTests/OrderDiscountPropertyTests3.cs`

### 6.6 Create Unit Tests for Domain Entities
**Status**: `todo` | **Priority**: High | **Effort**: 6h | **Dependencies**: 1.8
- Test OrderDiscount factory methods
- Test tier management
- Test item/category rule management
- Test eligibility evaluation
- Test discount calculation
- File: `BE/.../Tests/OrderDiscounts/UnitTests/OrderDiscountTests.cs`

### 6.7 Create Unit Tests for Application Services
**Status**: `todo` | **Priority**: High | **Effort**: 6h | **Dependencies**: 2.6, 2.7
- Test DiscountEngine service
- Test DiscountPriorityService
- Test command handlers
- Test query handlers
- File: `BE/.../Tests/OrderDiscounts/UnitTests/ServiceTests.cs`

### 6.8 Create Integration Tests for API
**Status**: `todo` | **Priority**: Medium | **Effort**: 6h | **Dependencies**: 4.1
- Test all controller endpoints
- Test validation
- Test error handling
- File: `BE/.../Tests/OrderDiscounts/IntegrationTests/OrderDiscountsControllerTests.cs`

---

## Phase 7: Testing (Frontend)

### 7.1 Create Unit Tests for Order Discount Components
**Status**: `todo` | **Priority**: Medium | **Effort**: 4h | **Dependencies**: 5.1-5.3
- Test form validation
- Test tier management UI
- Test item/category selection
- File: `FE/src/pages/Marketing/OrderDiscount/__tests__/`

### 7.2 Create Integration Tests for Cart Discount Display
**Status**: `todo` | **Priority**: Medium | **Effort**: 3h | **Dependencies**: 5.4
- Test discount evaluation
- Test threshold progress display
- Test tier information display
- File: `FE/src/components/Cart/__tests__/ShoppingCart.test.jsx`

---

## Phase 8: Documentation & Deployment

### 8.1 Update API Documentation
**Status**: `todo` | **Priority**: Medium | **Effort**: 2h | **Dependencies**: 4.1
- Document all endpoints with Swagger annotations
- Add request/response examples
- Document error codes

### 8.2 Create User Guide
**Status**: `todo` | **Priority**: Low | **Effort**: 3h | **Dependencies**: 5.1-5.3
- How to create order discounts
- How to configure multi-tier discounts
- How to manage item/category rules
- Best practices
- File: `docs/OrderDiscountUserGuide.md`

### 8.3 Run Database Migration
**Status**: `todo` | **Priority**: High | **Effort**: 1h | **Dependencies**: 3.4
- Execute migration on development environment
- Verify schema changes
- Test rollback procedure

### 8.4 Performance Testing
**Status**: `todo` | **Priority**: Medium | **Effort**: 4h | **Dependencies**: All backend tasks
- Test discount evaluation performance (<50ms target)
- Test bulk evaluation (1000/sec target)
- Optimize queries if needed

### 8.5 Deploy to Staging
**Status**: `todo` | **Priority**: High | **Effort**: 2h | **Dependencies**: 8.3, 8.4
- Deploy backend changes
- Deploy frontend changes
- Run smoke tests

---

## Summary

**Total Tasks**: 48  
**Estimated Total Effort**: ~150 hours

**Critical Path**:
1. Domain entities (1.1-1.8) → 2. Application layer (2.1-2.7) → 3. Infrastructure (3.1-3.4) → 4. API (4.1) → 5. Frontend (5.1-5.6) → 6. Testing → 7. Deployment

**Parallel Work Opportunities**:
- Frontend can start after API is complete
- Property test generators can be created while domain layer is being built
- Documentation can be written alongside implementation
