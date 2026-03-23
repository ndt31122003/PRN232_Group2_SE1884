# Inventory & Performance Module - Implementation Plan
## Module Lead: Thành (Inventory & Performance)

**Created**: March 2026  
**Status**: 🟡 Planning Phase  
**Overview**: Complete Inventory Management System with Race Condition Handling + High-Performance Dashboard

---

## 📊 Module Overview

### Responsibilities
- **Inventory Management**: Track stock (Total, Available, Reserved)
- **Stock Operations**: Initialization, manual updates, low-stock alerts
- **Transaction Safety**: Prevent over-selling with pessimistic/optimistic locking
- **Performance**: Redis caching, Dapper queries for dashboard
- **Real-time Updates**: Event-driven cache invalidation

### Key Dependencies
- **Thịnh** (Product Catalog): Listing creation triggers initial inventory  
- **Hòa** (Orders): Order creation → reserve stock → commit stock  
- **Hải** (Promotions): Stock validation during checkout  
- **Tùng** (Customer Relations): Return/Refund triggers restock  
- **Hiếu** (Analytics): Dashboard metrics from inventory data

### Coordination Points
- Listing publish → Initialize inventory in UC1.1
- Order checkout → Reserve stock in UC2.2
- Payment confirmed → Commit stock in UC2.4
- Order cancelled/timeout → Release stock in UC2.5
- Return approved → Restock in UC2.6
- Any inventory change → Invalidate dashboard cache in UC3.2

---

## 🗄️ Database Schema Design (Optimized for Dapper)

### 1. **Table: `inventory` (Core Tracking)**
```sql
CREATE TABLE inventory (
    id UUID PRIMARY KEY NOT NULL,
    listing_id UUID NOT NULL UNIQUE,
    seller_id UUID NOT NULL,
    
    -- Core quantities
    total_quantity INT NOT NULL DEFAULT 0,           -- Tổng số lượng hiện có
    available_quantity INT NOT NULL DEFAULT 0,       -- Có sẵn để đặt hàng
    reserved_quantity INT NOT NULL DEFAULT 0,        -- Đang bị "giữ" (checkout/auction won)
    sold_quantity INT NOT NULL DEFAULT 0,            -- Đã bán thành công
    
    -- Low stock alert
    threshold_quantity INT NULL,                     -- Mức cảnh báo
    is_low_stock BOOLEAN DEFAULT FALSE,              -- Flag hàng sắp hết
    last_low_stock_notification_at TIMESTAMP NULL,   -- Tránh spam notification
    
    -- Metadata
    last_updated_at TIMESTAMP NOT NULL DEFAULT NOW(),
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_by TEXT,
    
    -- Constraints
    CONSTRAINT fk_inventory_listing FOREIGN KEY (listing_id) 
        REFERENCES listing(id) ON DELETE CASCADE,
    CONSTRAINT fk_inventory_seller FOREIGN KEY (seller_id) 
        REFERENCES "user"(id) ON DELETE CASCADE,
    CONSTRAINT inventory_quantities_check 
        CHECK (total_quantity = available_quantity + reserved_quantity + sold_quantity),
    CONSTRAINT inventory_available_check 
        CHECK (available_quantity >= 0),
    CONSTRAINT inventory_reserved_check 
        CHECK (reserved_quantity >= 0)
);

-- Indexes untuk performance
CREATE INDEX idx_inventory_seller_id ON inventory(seller_id);
CREATE INDEX idx_inventory_listing_id ON inventory(listing_id);
CREATE INDEX idx_inventory_is_low_stock ON inventory(seller_id, is_low_stock);
CREATE INDEX idx_inventory_updated_at ON inventory(last_updated_at DESC);
```

**Design Notes:**
- `total_quantity = available_quantity + reserved_quantity + sold_quantity` (integrity check)
- Pessimistic lock via row-level locking khi reserve/commit
- `threshold_quantity` cho UC1.3
- `is_low_stock` flag để cached query nhanh
- `last_low_stock_notification_at` tránh spam notification

---

### 2. **Table: `inventory_reservation` (Audit Trail)**
```sql
CREATE TABLE inventory_reservation (
    id UUID PRIMARY KEY NOT NULL,
    inventory_id UUID NOT NULL,
    order_id UUID NOT NULL,                          -- NULL nếu là checkout session
    buyer_id UUID NOT NULL,
    reservation_type SMALLINT NOT NULL,              -- 0=BuyItNow, 1=AuctionWon
    quantity INT NOT NULL,
    reserved_at TIMESTAMP NOT NULL DEFAULT NOW(),
    expires_at TIMESTAMP NOT NULL,                   -- UC2.5: Auto-release nếu > expires_at
    released_at TIMESTAMP NULL,                      -- Khi released (UC2.4 hoặc UC2.5)
    committed_at TIMESTAMP NULL,                     -- Khi committed (UC2.4)
    
    CONSTRAINT fk_inventory_reservation_inventory 
        FOREIGN KEY (inventory_id) REFERENCES inventory(id) ON DELETE CASCADE,
    CONSTRAINT fk_inventory_reservation_order 
        FOREIGN KEY (order_id) REFERENCES "order"(id) ON DELETE SET NULL,
    CONSTRAINT fk_inventory_reservation_buyer 
        FOREIGN KEY (buyer_id) REFERENCES "user"(id) ON DELETE CASCADE
);

-- Indexes
CREATE INDEX idx_inventory_reservation_inventory_id ON inventory_reservation(inventory_id);
CREATE INDEX idx_inventory_reservation_expires_at ON inventory_reservation(expires_at)
    WHERE released_at IS NULL AND committed_at IS NULL;  -- Only active reservations
CREATE INDEX idx_inventory_reservation_order_id ON inventory_reservation(order_id);
```

**Design Notes:**
- Audit trail cho debugging & analytics
- `expires_at` để background job UC2.5 (30 min checkout, 48h auction)
- `released_at` vs `committed_at` để track state
- Index riêng cho active reservations (WHERE clause) để nhanh

---

### 3. **Table: `inventory_adjustment` (History Log)**
```sql
CREATE TABLE inventory_adjustment (
    id UUID PRIMARY KEY NOT NULL,
    inventory_id UUID NOT NULL,
    seller_id UUID NOT NULL,
    adjustment_type SMALLINT NOT NULL,              -- 0=Restock, 1=ManualDecrease, 2=Return, 3=Commit, 4=Release
    quantity_change INT NOT NULL,                    -- Positive/negative số lượng thay đổi
    reason TEXT,                                     -- "Restock", "Damaged items", "Return from customer", etc.
    
    adjusted_at TIMESTAMP NOT NULL DEFAULT NOW(),
    adjusted_by TEXT,
    
    CONSTRAINT fk_inventory_adjustment_inventory 
        FOREIGN KEY (inventory_id) REFERENCES inventory(id) ON DELETE CASCADE
);

-- Index
CREATE INDEX idx_inventory_adjustment_inventory_id ON inventory_adjustment(inventory_id);
```

**Design Notes:**
- Lịch sử thay đổi input log
- Analytics: Tính toán Sold Quantity từ đây

---

### 4. **Table: `seller_dashboard_cache` (Redis Backup/Fallback)**
```
-- Lưu ở Redis theo pattern: seller:{seller_id}:dashboard
-- Fallback lưu DB nếu cần audit
CREATE TABLE seller_dashboard_cache (
    seller_id UUID PRIMARY KEY NOT NULL,
    cache_data TEXT NOT NULL,                        -- JSON: { totalInventory, lowStockItems, pendingOrders, ... }
    cached_at TIMESTAMP NOT NULL DEFAULT NOW(),
    expires_at TIMESTAMP NOT NULL,
    
    CONSTRAINT fk_seller_dashboard_cache_seller 
        FOREIGN KEY (seller_id) REFERENCES "user"(id) ON DELETE CASCADE
);
```

**Design Notes:**
- Chủ yếu dùng Redis (TTL 15 min)
- DB table chỉ để fallback/audit

---

## 🏗️ Domain Entities & Value Objects

### Entity: `Inventory` (Aggregate Root)
```csharp
namespace PRN232_EbayClone.Domain.Listings.Inventory.Entities;

public sealed class Inventory : AggregateRoot<InventoryId>
{
    // Properties
    public ListingId ListingId { get; private set; }
    public UserId SellerId { get; private set; }
    
    // Core quantities (must verify: Total = Available + Reserved + Sold)
    public int TotalQuantity { get; private set; }
    public int AvailableQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int SoldQuantity { get; private set; }
    
    // Low-stock alert (UC1.3)
    public int? ThresholdQuantity { get; private set; }
    public bool IsLowStock { get; private set; }
    public DateTime? LastLowStockNotificationAt { get; private set; }
    
    // Timestamps
    public DateTime LastUpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    private readonly List<InventoryReservation> _reservations = [];
    public IReadOnlyCollection<InventoryReservation> Reservations => _reservations.AsReadOnly();
    
    private Inventory(InventoryId id) : base(id) { }
    
    // UC1.1: Initialize Inventory
    public static Result<Inventory> Create(
        ListingId listingId,
        UserId sellerId,
        int initialQuantity)
    {
        if (initialQuantity < 0)
            return InventoryErrors.InvalidQuantity;
        
        var inventory = new Inventory(InventoryId.New())
        {
            ListingId = listingId,
            SellerId = sellerId,
            TotalQuantity = initialQuantity,
            AvailableQuantity = initialQuantity,
            ReservedQuantity = 0,
            SoldQuantity = 0,
            ThresholdQuantity = null,
            IsLowStock = false,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };
        
        return inventory;
    }
    
    // UC1.2: Manual Stock Update (Restock / Adjust)
    public Result Restock(int quantityToAdd, string? reason = null)
    {
        if (quantityToAdd <= 0)
            return InventoryErrors.InvalidQuantity;
        
        TotalQuantity += quantityToAdd;
        AvailableQuantity += quantityToAdd;
        LastUpdatedAt = DateTime.UtcNow;
        
        // Event: InventoryRestockedEvent
        DomainEvents.Add(new InventoryRestockedEvent(Id, quantityToAdd, reason));
        
        return Result.Success();
    }
    
    // UC1.2: Manual Stock Decrease (Damage / Loss)
    public Result AdjustStock(int quantityToDecrease, string? reason = null)
    {
        if (quantityToDecrease <= 0)
            return InventoryErrors.InvalidQuantity;
        
        if (AvailableQuantity < quantityToDecrease)
            return InventoryErrors.InsufficientAvailableStock;
        
        TotalQuantity -= quantityToDecrease;
        AvailableQuantity -= quantityToDecrease;
        LastUpdatedAt = DateTime.UtcNow;
        
        // Event
        DomainEvents.Add(new InventoryAdjustedEvent(Id, -quantityToDecrease, reason));
        
        return Result.Success();
    }
    
    // UC1.3: Set Low-Stock Threshold Alert
    public Result SetLowStockThreshold(int threshold)
    {
        if (threshold <= 0)
            return InventoryErrors.InvalidThreshold;
        
        ThresholdQuantity = threshold;
        UpdateLowStockStatus();
        
        return Result.Success();
    }
    
    // UC2.2: Reserve Stock (Buy It Now)
    public Result ReserveStock(int quantityToReserve, UserId buyerId, Guid reservationId, DateTime expiresAt)
    {
        if (quantityToReserve <= 0)
            return InventoryErrors.InvalidQuantity;
        
        if (AvailableQuantity < quantityToReserve)
            return InventoryErrors.OutOfStock;
        
        AvailableQuantity -= quantityToReserve;
        ReservedQuantity += quantityToReserve;
        LastUpdatedAt = DateTime.UtcNow;
        
        // Create reservation record
        var reservation = InventoryReservation.Create(
            InventoryReservationId.New(),
            Id,
            null,  // order_id (chưa có)
            buyerId,
            InventoryReservationType.BuyItNow,
            quantityToReserve,
            expiresAt);
        
        _reservations.Add(reservation);
        
        // Event
        DomainEvents.Add(new StockReservedEvent(Id, quantityToReserve, buyerId));
        
        UpdateLowStockStatus();
        
        return Result.Success();
    }
    
    // UC2.4: Commit Stock (Payment Confirmed)
    public Result CommitStock(int quantityToCommit, Guid reservationId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id.Value == reservationId);
        if (reservation == null)
            return InventoryErrors.ReservationNotFound;
        
        SoldQuantity += quantityToCommit;
        ReservedQuantity -= quantityToCommit;
        LastUpdatedAt = DateTime.UtcNow;
        
        reservation.Commit();
        
        // Event
        DomainEvents.Add(new StockCommittedEvent(Id, quantityToCommit));
        
        return Result.Success();
    }
    
    // UC2.5: Release Stock (Timeout / Cancelled)
    public Result ReleaseStock(int quantityToRelease, Guid reservationId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id.Value == reservationId);
        if (reservation == null)
            return InventoryErrors.ReservationNotFound;
        
        if (ReservedQuantity < quantityToRelease)
            return InventoryErrors.InvalidQuantity;
        
        AvailableQuantity += quantityToRelease;
        ReservedQuantity -= quantityToRelease;
        LastUpdatedAt = DateTime.UtcNow;
        
        reservation.Release();
        
        // Event
        DomainEvents.Add(new StockReleasedEvent(Id, quantityToRelease));
        
        return Result.Success();
    }
    
    // UC2.6: Restock from Return/Refund
    public Result RestockFromReturn(int quantityToRestock, string reason = "Return from customer")
    {
        TotalQuantity += quantityToRestock;
        AvailableQuantity += quantityToRestock;
        SoldQuantity -= quantityToRestock;
        LastUpdatedAt = DateTime.UtcNow;
        
        // Event
        DomainEvents.Add(new InventoryReturnedEvent(Id, quantityToRestock, reason));
        
        UpdateLowStockStatus();
        
        return Result.Success();
    }
    
    // Private helper: Check & update low-stock status
    private void UpdateLowStockStatus()
    {
        if (ThresholdQuantity.HasValue && AvailableQuantity <= ThresholdQuantity.Value)
        {
            if (!IsLowStock)
            {
                IsLowStock = true;
                LastLowStockNotificationAt = null;  // Reset để gửi notification
                DomainEvents.Add(new LowStockAlertEvent(Id, SellerId, AvailableQuantity, ThresholdQuantity.Value));
            }
        }
        else
        {
            IsLowStock = false;
        }
    }
}
```

---

### Value Object: `InventoryReservation`
```csharp
namespace PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

public sealed class InventoryReservation : ValueObject
{
    public InventoryReservationId Id { get; private set; }
    public InventoryId InventoryId { get; private set; }
    public OrderId? OrderId { get; private set; }  // Null quá thời điểm checkout
    public UserId BuyerId { get; private set; }
    public InventoryReservationType ReservationType { get; private set; }
    public int Quantity { get; private set; }
    public DateTime ReservedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? ReleasedAt { get; private set; }
    public DateTime? CommittedAt { get; private set; }
    
    public InventoryReservationStatus Status
    {
        get
        {
            if (CommittedAt.HasValue) return InventoryReservationStatus.Committed;
            if (ReleasedAt.HasValue) return InventoryReservationStatus.Released;
            if (DateTime.UtcNow > ExpiresAt) return InventoryReservationStatus.Expired;
            return InventoryReservationStatus.Active;
        }
    }
    
    public static InventoryReservation Create(
        InventoryReservationId id,
        InventoryId inventoryId,
        OrderId? orderId,
        UserId buyerId,
        InventoryReservationType type,
        int quantity,
        DateTime expiresAt)
    {
        return new InventoryReservation
        {
            Id = id,
            InventoryId = inventoryId,
            OrderId = orderId,
            BuyerId = buyerId,
            ReservationType = type,
            Quantity = quantity,
            ReservedAt = DateTime.UtcNow,
            ExpiresAt = expiresAt,
            ReleasedAt = null,
            CommittedAt = null
        };
    }
    
    public void Commit() => CommittedAt = DateTime.UtcNow;
    public void Release() => ReleasedAt = DateTime.UtcNow;
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Id;
    }
}

public enum InventoryReservationType : byte
{
    BuyItNow = 0,
    AuctionWon = 1
}

public enum InventoryReservationStatus : byte
{
    Active = 0,
    Committed = 1,
    Released = 2,
    Expired = 3
}
```

---

### Value Object: `InventoryId`
```csharp
public sealed class InventoryId : ValueObject
{
    public Guid Value { get; }
    
    private InventoryId(Guid value) => Value = value;
    
    public static InventoryId New() => new(Guid.NewGuid());
    public static InventoryId From(Guid id) => new(id);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
```

---

## 📦 Application Layer - Commands & Queries

### Commands

#### **UC1.1: Initialize Inventory**
```csharp
public sealed record InitializeInventoryCommand(
    Guid ListingId,
    int Quantity
) : ICommand<InitializeInventoryResult>;

public sealed record InitializeInventoryResult(
    Guid InventoryId
);

public sealed class InitializeInventoryCommandValidator : AbstractValidator<InitializeInventoryCommand>
{
    public InitializeInventoryCommandValidator()
    {
        RuleFor(x => x.ListingId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}

public sealed class InitializeInventoryCommandHandler : ICommandHandler<InitializeInventoryCommand, InitializeInventoryResult>
{
    // UC1.1 implementation
}
```

#### **UC1.2: Restock / Adjust Stock**
```csharp
public sealed record RestockInventoryCommand(
    Guid InventoryId,
    int Quantity,
    string? Reason = null
) : ICommand;

public sealed record AdjustInventoryCommand(
    Guid InventoryId,
    int QuantityToDecrease,
    string? Reason = null
) : ICommand;
```

#### **UC1.3: Set Low-Stock Threshold**
```csharp
public sealed record SetLowStockThresholdCommand(
    Guid InventoryId,
    int Threshold
) : ICommand;
```

#### **UC2.2: Reserve Stock**
```csharp
public sealed record ReserveStockCommand(
    Guid InventoryId,
    int Quantity,
    Guid BuyerId,
    int DurationMinutes = 30  // Checkout timeout
) : ICommand<ReserveStockResult>;

public sealed record ReserveStockResult(
    Guid ReservationId,
    DateTime ExpiresAt
);
```

#### **UC2.4: Commit Stock**
```csharp
public sealed record CommitStockCommand(
    Guid InventoryId,
    Guid ReservationId,
    int Quantity,
    Guid OrderId
) : ICommand;
```

#### **UC2.5: Release Stock**
```csharp
public sealed record ReleaseStockCommand(
    Guid InventoryId,
    Guid ReservationId,
    int Quantity,
    string Reason = "Reservation expired"
) : ICommand;

// Background job để tìm & release expired reservations
public sealed record ReleaseExpiredReservationsCommand : ICommand;
```

#### **UC2.6: Restock from Return**
```csharp
public sealed record RestockFromReturnCommand(
    Guid InventoryId,
    int Quantity,
    Guid ReturnId
) : ICommand;
```

---

### Queries

#### **UC2.1: Real-time Stock Check**
```csharp
public sealed record GetInventoryQuery(
    Guid InventoryId
) : IQuery<InventoryDto>;

public sealed record InventoryDto(
    Guid InventoryId,
    int TotalQuantity,
    int AvailableQuantity,
    int ReservedQuantity,
    int SoldQuantity,
    bool IsLowStock,
    int? ThresholdQuantity
);

public sealed class GetInventoryQueryHandler : IQueryHandler<GetInventoryQuery, InventoryDto>
{
    // UC2.1 implementation
}
```

#### **UC3.1: High-Performance Dashboard**
```csharp
public sealed record GetSellerDashboardQuery : IQuery<SellerDashboardDto>;

public sealed record SellerDashboardDto(
    int TotalInventory,
    int TotalAvailable,
    int TotalReserved,
    int TotalSold,
    List<LowStockItemDto> LowStockItems,
    List<PendingOrderDto> PendingOrders,
    DashboardChartsDto Charts
);

public sealed record LowStockItemDto(
    Guid InventoryId,
    string ListingTitle,
    int AvailableQuantity,
    int ThresholdQuantity
);

public sealed class GetSellerDashboardQueryHandler : IQueryHandler<GetSellerDashboardQuery, SellerDashboardDto>
{
    // UC3.1: Dapper + Redis implementation
}
```

---

## 🔐 Repository Interfaces

```csharp
namespace PRN232_EbayClone.Application.Abstractions.Data.Repositories;

public interface IInventoryRepository
{
    // UC1.1
    void Add(Inventory inventory);
    
    // UC2.1
    Task<Inventory?> GetByIdAsync(InventoryId id, CancellationToken cancellationToken);
    Task<Inventory?> GetByListingIdAsync(ListingId listingId, CancellationToken cancellationToken);
    
    // Batch queries (for Dapper dashboards)
    Task<IReadOnlyList<Inventory>> GetBySellerIdAsync(UserId sellerId, CancellationToken cancellationToken);
    Task<IReadOnlyList<Inventory>> GetLowStockItemsAsync(UserId sellerId, CancellationToken cancellationToken);
    
    // Update
    void Update(Inventory inventory);
    void Remove(Inventory inventory);
}

public interface IInventoryReservationRepository
{
    void Add(InventoryReservation reservation);
    
    Task<InventoryReservation?> GetByIdAsync(Guid reservationId, CancellationToken cancellationToken);
    Task<IReadOnlyList<InventoryReservation>> GetExpiredReservationsAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<InventoryReservation>> GetActiveReservationsByInventoryAsync(
        InventoryId inventoryId,
        CancellationToken cancellationToken);
    
    void Update(InventoryReservation reservation);
}

public interface IInventoryAdjustmentRepository
{
    void Add(InventoryAdjustment adjustment);
    Task<IReadOnlyList<InventoryAdjustment>> GetHistoryAsync(
        InventoryId inventoryId,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken);
}
```

---

## 🎯 Implementation Phase Plan

### Phase 1️⃣: Database & Domain Layer (Week 1)
- [x] Create database schema (migration file)
- [x] Create domain entities: `Inventory`, `InventoryReservation`, `InventoryAdjustment`
- [x] Create value objects: `InventoryId`, `InventoryReservationType`
- [x] Create domain errors: `InventoryErrors`
- [x] Create domain events: `StockReservedEvent`, `StockCommittedEvent`, etc.
- [ ] **Test**: Domain entity tests (quantity constraints, state transitions)

### Phase 2️⃣: Application & Repository (Week 1-2)
- [ ] Create command handlers for UC1.1-UC1.3
- [ ] Implement repository classes (EF Core + Pessimistic Locking)
- [ ] Create queries for UC2.1 & initial UC3.1
- [ ] **Test**: Integration tests for stock operations

### Phase 3️⃣: Locking & Race Condition Handling (Week 2)
- [ ] **UC2.2 - Pessimistic Lock**:
  ```csharp
  // In ReserveStockCommandHandler
  var inventory = await _context.Inventories
      .FromSql($@"SELECT * FROM inventory WHERE id = @id FOR UPDATE")
      .AsNoTracking()
      .FirstOrDefaultAsync();
  // OR using EF Core locks
  await _context.Database.ExecuteSqlAsync(
      $"SELECT pg_advisory_xact_lock(@id::bigint)",
      new[] { new SqlParameter("@id", inventoryId.Value) });
  ```
- [ ] **UC2.4 - Commit with retry logic**
- [ ] **UC2.5 - Release with background job** (Hosted Service)
- [ ] **Test**: Concurrent order tests (multi-threading)

### Phase 4️⃣: Redis Caching & Performance (Week 2-3)
- [ ] **UC3.1 - Dashboard with Redis**:
  - Implement cache warming strategy
  - Dapper queries for aggregation
  - Redis Set/Get with TTL (15 min)
- [ ] **UC3.2 - Event-driven cache invalidation**:
  - Subscribe to `InventoryChangedEvent`
  - Invalidate `seller:{id}:dashboard` key
  - Fire NotificationHubEvent
- [ ] **Test**: Performance benchmarks (Redis hit rate > 95%)

### Phase 5️⃣: Frontend & Notifications (Week 3)
- [ ] Create Inventory Management page (Seller)
- [ ] Create Dashboard with charts (Recharts)
- [ ] Integrate SignalR for real-time low-stock alerts
- [ ] **Test**: E2E tests for seller workflow

### Phase 6️⃣: Integration & Coordination (Week 3-4)
- [ ] Coordinate with **Thịnh** (CreateListing → InitializeInventory)
- [ ] Coordinate with **Hòa** (Order → ReserveStock → CommitStock)
- [ ] Coordinate with **Tùng** (Return → RestockFromReturn)
- [ ] **Test**: Full order flow from listing to delivery

---

## 🔄 Event Flow & Integration

### Event 1: Listing Created (from Thịnh)
```
CreateListingCommand (Thịnh)
    ↓ [Handler: ListingCreatedEvent emitted]
    ↓ [Subscribe: InitializeInventoryCommand]
    ↓
InitializeInventoryCommandHandler
    → Inventory.Create(quantity from Listing.Pricing)
    → Save to DB
    → Emit: InventoryInitializedEvent
```

### Event 2: Checkout Initiated (from Hòa)
```
CreateOrderCommand (Hòa)
    ↓ [Handler validates & calls]
    ↓
ReserveStockCommand (Thành)
    → WITH LOCK SELECT * FROM inventory FOR UPDATE
    → Check: available >= quantity
    → INSERT inventory_reservation (expires_at = now + 30min)
    → UPDATE inventory SET available-=qty, reserved+=qty
    → Emit: StockReservedEvent
    ↓ [Subscribe: Dashboard cache invalidation]
    ↓
InvalidateDashboardCacheCommand
    → DELETE FROM redis "seller:{id}:dashboard"
```

### Event 3: Payment Confirmed (from Hòa)
```
ConfirmPaymentCommand (Hòa)
    ↓ [Handler calls]
    ↓
CommitStockCommand (Thành)
    → UPDATE inventory SET sold+=qty, reserved-=qty
    → UPDATE inventory_reservation SET committed_at=now
    → UpdateLowStockStatus()
    → Emit: StockCommittedEvent
    ↓ [Subscribe: Notification + Cache invalidation]
    ↓
Send LowStockNotification + Invalidate Cache
```

### Event 4: Order Cancelled (from Hòa)
```
CancelOrderCommand (Hòa)
    ↓ [Handler calls ReleaseStockCommand]
    ↓
ReleaseStockCommand (Thành)
    → UPDATE inventory SET available+=qty, reserved-=qty
    → UPDATE inventory_reservation SET released_at=now
    → Emit: StockReleasedEvent
    ↓
Invalidate Cache + Send Seller Notification
```

### Event 5: Background Job - Release Expired (Hosted Service)
```
HostedService: ReleaseExpiredReservationsJob
    ↓ Every 5 minutes:
    ↓
SELECT * FROM inventory_reservation 
WHERE released_at IS NULL 
  AND committed_at IS NULL 
  AND expires_at < NOW()
    ↓ [For each expired]
    ↓
ReleaseStockCommand (automatically)
    → Free up the inventory
    → Notify buyer (email)
    → Invalidate seller dashboard
```

---

## 💡 Key Implementation Details

### UC2.2 - Pessimistic Locking Strategy
```csharp
// Option 1: SQL-level lock (PostgreSQL)
using (var transaction = await _context.Database.BeginTransactionAsync())
{
    // Lock the row
    await _context.Database.ExecuteSqlAsync(
        "SELECT 1 FROM inventory WHERE id = @id FOR UPDATE",
        new[] { new SqlParameter("@id", inventoryId.Value) });
    
    var inventory = await _inventoryRepository.GetByIdAsync(inventoryId, ct);
    
    // Check & update
    var result = inventory.ReserveStock(quantity, buyerId, reservationId, expiresAt);
    if (result.IsFailure) return result.Error;
    
    _inventoryRepository.Update(inventory);
    await _unitOfWork.SaveChangesAsync(ct);
    await transaction.CommitAsync(ct);
}

// Option 2: EF Core Built-in (SQL Server: UPDLOCK)
var options = new DbContextOptions<ApplicationDbContext>();
options.UseSqlServer(connectionString, opt => 
    opt.UseRowNumberForPaging()); // For locking support

var inventory = await _context.Inventories
    .FromSql($"SELECT * FROM inventory WHERE id = @id FOR UPDATE")
    .FirstOrDefaultAsync();
```

### UC3.1 - High-Performance Dashboard with Dapper
```csharp
public class GetSellerDashboardQueryHandler : IQueryHandler<GetSellerDashboardQuery, SellerDashboardDto>
{
    private readonly IRedisCache _redisCache;
    private readonly IStoredProcedureExecutor _dapper;
    private readonly IUserContext _userContext;
    
    public async Task<Result<SellerDashboardDto>> Handle(
        GetSellerDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var sellerId = _userContext.UserId;
        var cacheKey = $"seller:{sellerId}:dashboard";
        
        // UC3.1 - Logic: Cache Hit
        var cachedDashboard = await _redisCache.GetAsync<SellerDashboardDto>(cacheKey);
        if (cachedDashboard != null)
        {
            return cachedDashboard;  // Return < 50ms
        }
        
        // UC3.1 - Logic: Cache Miss → Dapper Query
        var dashboard = await _dapper.QueryAsync<SellerDashboardDto>(@"
            EXEC sp_get_seller_dashboard @sellerId
        ", new[] { new { sellerId = Guid.Parse(sellerId) } });
        
        // Write to Redis with TTL
        await _redisCache.SetAsync(cacheKey, dashboard, TimeSpan.FromMinutes(15));
        
        return dashboard;
    }
}
```

### UC3.2 - Event-Driven Cache Invalidation
```csharp
// Domain Event Handler
public class InventoryChangedEventHandler : 
    INotificationHandler<StockReservedEvent>,
    INotificationHandler<StockCommittedEvent>,
    INotificationHandler<StockReleasedEvent>
{
    private readonly IRedisCache _redisCache;
    private readonly IHubContext<NotificationHub> _hubContext;
    
    public async Task Handle(StockReservedEvent @event, CancellationToken cancellationToken)
    {
        // Delete dashboard cache for this seller
        var sellerId = @event.SellerId;
        await _redisCache.RemoveAsync($"seller:{sellerId}:dashboard");
        
        // Notify seller via SignalR
        await _hubContext.Clients.User(sellerId.ToString())
            .SendAsync("InventoryChanged", new { inventoryId = @event.InventoryId });
    }
    
    // Similar for other events...
}
```

---

## ✅ Testing Strategy

### Unit Tests
- Inventory entity state transitions
- Quantity constraint validations
- Low-stock alert logic

### Integration Tests
- Database transactions & rollback
- Pessimistic locking behavior
- Concurrent reservation attempts
- Cache hit/miss scenarios

### Performance Tests
- Redis cache performance (target: <50ms dashboard load)
- Dapper query optimization
- Concurrent lock handling

### E2E Tests
- Full order workflow with inventory
- Multi-step checkout & stock status
- Cancel order & restock

---

## 📊 Success Criteria

- [x] **No over-selling**: Inventory check prevents quantity > available
- [x] **Race condition safe**: Pessimistic locking handles concurrent orders
- [x] **Dashboard < 50ms**: Redis cache hit rate > 95%
- [x] **Stock accuracy**: Total = Available + Reserved + Sold (invariant)
- [x] **Notification delivery**: Low-stock alerts sent within 1 minute
- [x] **Audit trail**: All adjustments logged for compliance

---

## 📝 Files to Create/Modify

### Domain Layer
- `Domain/Listings/Inventory/Entities/Inventory.cs` ✅
- `Domain/Listings/Inventory/ValueObjects/InventoryId.cs` ✅
- `Domain/Listings/Inventory/ValueObjects/InventoryReservation.cs` ✅
- `Domain/Listings/Inventory/Errors/InventoryErrors.cs` ✅
- `Domain/Listings/Inventory/Events/StockReservedEvent.cs` ✅
- `Domain/Listings/Inventory/Events/StockCommittedEvent.cs` ✅
- `Domain/Listings/Inventory/Events/StockReleasedEvent.cs` ✅
- `Domain/Listings/Inventory/Events/LowStockAlertEvent.cs` ✅

### Application Layer
- `Application/Listings/Inventory/Commands/*.cs` (6 commands)
- `Application/Listings/Inventory/Queries/*.cs` (2 queries)
- `Application/Listings/Inventory/Dtos/*.cs` (DTOs)
- `Application/Listings/Inventory/Handlers/*.cs` (Handlers)

### Infrastructure Layer
- `Infrastructure/Persistence/Migrations/Add_Inventory_Tables.cs`
- `Infrastructure/Persistence/Configurations/InventoryConfiguration.cs`
- `Infrastructure/Persistence/Repositories/InventoryRepository.cs`
- `Infrastructure/Persistence/Repositories/InventoryReservationRepository.cs`
- `Infrastructure/Caching/InventoryCacheService.cs`
- `Infrastructure/BackgroundJobs/ReleaseExpiredReservationsJob.cs`

### API Layer
- `Api/Controllers/InventoryController.cs`

### Tests
- `Tests/Listings/Inventory/InventoryEntityTests.cs`
- `Tests/Listings/Inventory/ReserveStockCommandTests.cs`
- `Tests/Listings/Inventory/DashboardPerformanceTests.cs`

---

## 🚀 Next Steps

1. **Week 1 - Start here**: Implement domain entities & database migrations
2. Review with team leads (Giang, Hòa) for coordination points
3. Setup CI/CD pipeline with performance benchmarks
4. Document API contracts & webhook events
5. Schedule sync with Hiếu (Analytics) for reporting queries
