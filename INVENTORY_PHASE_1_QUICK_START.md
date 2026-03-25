# Inventory Module - Phase 1 Quick Start Guide
## Thành (Inventory & Performance) - Week 1 Execution

**Duration**: Week 1 (5 days)  
**Objective**: Database schema + Domain entities ready  
**Success Criteria**: All domain entities compile, migrations created, basic tests pass

---

## 📋 Daily Breakdown

### Day 1️⃣: Database & Value Objects
**Tasks**: Create DB schema, create Ids and supporting value objects

#### Task 1.1: Create EF Core Migration File
**File**: `BE/PRN232_EbayClone/PRN232_EbayClone.Infrastructure/Persistence/Migrations/[DateStamp]_AddInventoryManagement.cs`

```csharp
using Microsoft.EntityFrameworkCore.Migrations;

namespace PRN232_EbayClone.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddInventoryManagement : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Table 1: inventory
        migrationBuilder.CreateTable(
            name: "inventory",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                listing_id = table.Column<Guid>(type: "uuid", nullable: false),
                seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                
                total_quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                available_quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                reserved_quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                sold_quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                
                threshold_quantity = table.Column<int>(type: "integer", nullable: true),
                is_low_stock = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                last_low_stock_notification_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                
                last_updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                updated_by = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inventory", x => x.id);
                table.ForeignKey(
                    name: "fk_inventory_listing_id",
                    column: x => x.listing_id,
                    principalTable: "listing",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_inventory_seller_id",
                    column: x => x.seller_id,
                    principalTable: "\"user\"",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.UniqueConstraint(
                    name: "uk_inventory_listing_id",
                    column: x => x.listing_id);
            });

        // Table 2: inventory_reservation
        migrationBuilder.CreateTable(
            name: "inventory_reservation",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                order_id = table.Column<Guid>(type: "uuid", nullable: true),
                buyer_id = table.Column<Guid>(type: "uuid", nullable: false),
                
                reservation_type = table.Column<int>(type: "integer", nullable: false),
                quantity = table.Column<int>(type: "integer", nullable: false),
                
                reserved_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                released_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                committed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inventory_reservation", x => x.id);
                table.ForeignKey(
                    name: "fk_inventory_reservation_inventory_id",
                    column: x => x.inventory_id,
                    principalTable: "inventory",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "fk_inventory_reservation_order_id",
                    column: x => x.order_id,
                    principalTable: "\"order\"",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
                table.ForeignKey(
                    name: "fk_inventory_reservation_buyer_id",
                    column: x => x.buyer_id,
                    principalTable: "\"user\"",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        // Table 3: inventory_adjustment
        migrationBuilder.CreateTable(
            name: "inventory_adjustment",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                seller_id = table.Column<Guid>(type: "uuid", nullable: false),
                
                adjustment_type = table.Column<int>(type: "integer", nullable: false),
                quantity_change = table.Column<int>(type: "integer", nullable: false),
                reason = table.Column<string>(type: "text", nullable: true),
                
                adjusted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                adjusted_by = table.Column<string>(type: "text", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_inventory_adjustment", x => x.id);
                table.ForeignKey(
                    name: "fk_inventory_adjustment_inventory_id",
                    column: x => x.inventory_id,
                    principalTable: "inventory",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        // Create indexes
        migrationBuilder.CreateIndex(
            name: "idx_inventory_seller_id",
            table: "inventory",
            column: "seller_id");

        migrationBuilder.CreateIndex(
            name: "idx_inventory_is_low_stock",
            table: "inventory",
            columns: new[] { "seller_id", "is_low_stock" });

        migrationBuilder.CreateIndex(
            name: "idx_inventory_updated_at",
            table: "inventory",
            column: "last_updated_at",
            descending: true);

        migrationBuilder.CreateIndex(
            name: "idx_inventory_reservation_expires_at",
            table: "inventory_reservation",
            column: "expires_at");

        migrationBuilder.CreateIndex(
            name: "idx_inventory_adjustment_inventory_id",
            table: "inventory_adjustment",
            column: "inventory_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "inventory_adjustment");
        migrationBuilder.DropTable(name: "inventory_reservation");
        migrationBuilder.DropTable(name: "inventory");
    }
}
```

**Command to create migration file**:
```bash
cd BE
dotnet ef migrations add AddInventoryManagement \
  -p PRN232_EbayClone.Infrastructure \
  -s PRN232_EbayClone.Api
```

---

#### Task 1.2: Create InventoryId Value Object
**File**: `BE/PRN232_EbayClone/PRN232_EbayClone.Domain/Listings/Inventory/ValueObjects/InventoryId.cs`

```csharp
using PRN232_EbayClone.Domain.Shared.Abstractions;

namespace PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

public sealed class InventoryId : ValueObject
{
    public Guid Value { get; }
    
    private InventoryId(Guid value) => Value = value;
    
    public static InventoryId New() => new(Guid.NewGuid());
    public static InventoryId From(Guid id) => new(id);
    public static InventoryId From(string id) => new(Guid.Parse(id));
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value.ToString();
}
```

---

#### Task 1.3: Create Enums
**File**: `BE/PRN232_EbayClone/PRN232_EbayClone.Domain/Listings/Inventory/Enums/InventoryReservationType.cs`

```csharp
namespace PRN232_EbayClone.Domain.Listings.Inventory.Enums;

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

public enum InventoryAdjustmentType : byte
{
    Restock = 0,
    ManualDecrease = 1,
    Return = 2,
    Commit = 3,
    Release = 4
}
```

---

#### Task 1.4: Create Domain Errors
**File**: `BE/PRN232_EbayClone/PRN232_EbayClone.Domain/Listings/Inventory/Errors/InventoryErrors.cs`

```csharp
using PRN232_EbayClone.Domain.Shared.Results;

namespace PRN232_EbayClone.Domain.Listings.Inventory.Errors;

public static class InventoryErrors
{
    public static Error OutOfStock => Error.Failure(
        "Inventory.OutOfStock",
        "The requested quantity is not available in stock.");
    
    public static Error InvalidQuantity => Error.Failure(
        "Inventory.InvalidQuantity",
        "The quantity must be greater than zero.");
    
    public static Error InsufficientAvailableStock => Error.Failure(
        "Inventory.InsufficientAvailableStock",
        "There are not enough available items to perform this operation.");
    
    public static Error InvalidThreshold => Error.Failure(
        "Inventory.InvalidThreshold",
        "The threshold must be greater than zero.");
    
    public static Error NotFound => Error.NotFound(
        "Inventory.NotFound",
        "The inventory was not found.");
    
    public static Error ReservationNotFound => Error.NotFound(
        "Inventory.ReservationNotFound",
        "The reservation was not found.");
    
    public static Error ReservationExpired => Error.Failure(
        "Inventory.ReservationExpired",
        "The reservation has expired.");
    
    public static Error InvalidStateTransition => Error.Failure(
        "Inventory.InvalidStateTransition",
        "The operation is not valid in the current reservation state.");
}
```

---

### Day 2️⃣: Domain Entities

#### Task 2.1: Create InventoryReservation Value Object
**File**: `BE/PRN232_EbayClone/PRN232_EbayClone.Domain/Listings/Inventory/ValueObjects/InventoryReservation.cs`

```csharp
using PRN232_EbayClone.Domain.Listings.Inventory.Enums;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;

public sealed class InventoryReservation : ValueObject
{
    public InventoryReservationId Id { get; private set; }
    public InventoryId InventoryId { get; private set; }
    public Guid? OrderId { get; private set; }
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
    
    public bool IsActive => Status == InventoryReservationStatus.Active;
    
    private InventoryReservation() { }
    
    public static InventoryReservation Create(
        InventoryReservationId id,
        InventoryId inventoryId,
        Guid? orderId,
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

public sealed class InventoryReservationId : ValueObject
{
    public Guid Value { get; }
    
    private InventoryReservationId(Guid value) => Value = value;
    
    public static InventoryReservationId New() => new(Guid.NewGuid());
    public static InventoryReservationId From(Guid id) => new(id);
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
```

---

#### Task 2.2: Create Inventory Aggregate Root
**File**: `BE/PRN232_EbayClone/PRN232_EbayClone.Domain/Listings/Inventory/Entities/Inventory.cs`

**⚠️ LONG FILE** - Create in separate sections:

**Part A: Properties & Create Method**
```csharp
using PRN232_EbayClone.Domain.Listings.Inventory.Enums;
using PRN232_EbayClone.Domain.Listings.Inventory.Errors;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Shared.Results;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Listings.Inventory.Entities;

public sealed class Inventory : AggregateRoot<InventoryId>
{
    // Properties
    public ListingId ListingId { get; private set; }
    public UserId SellerId { get; private set; }
    
    public int TotalQuantity { get; private set; }
    public int AvailableQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }
    public int SoldQuantity { get; private set; }
    
    public int? ThresholdQuantity { get; private set; }
    public bool IsLowStock { get; private set; }
    public DateTime? LastLowStockNotificationAt { get; private set; }
    
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
}
```

**Part B: Stock Operations**
```csharp
    // UC1.2: Restock
    public Result Restock(int quantityToAdd, string? reason = null)
    {
        if (quantityToAdd <= 0)
            return InventoryErrors.InvalidQuantity;
        
        TotalQuantity += quantityToAdd;
        AvailableQuantity += quantityToAdd;
        LastUpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }
    
    // UC1.2: Adjust Stock (Manual Decrease)
    public Result AdjustStock(int quantityToDecrease, string? reason = null)
    {
        if (quantityToDecrease <= 0)
            return InventoryErrors.InvalidQuantity;
        
        if (AvailableQuantity < quantityToDecrease)
            return InventoryErrors.InsufficientAvailableStock;
        
        TotalQuantity -= quantityToDecrease;
        AvailableQuantity -= quantityToDecrease;
        LastUpdatedAt = DateTime.UtcNow;
        
        return Result.Success();
    }
    
    // UC1.3: Set Low-Stock Threshold
    public Result SetLowStockThreshold(int threshold)
    {
        if (threshold <= 0)
            return InventoryErrors.InvalidThreshold;
        
        ThresholdQuantity = threshold;
        UpdateLowStockStatus();
        
        return Result.Success();
    }
    
    private void UpdateLowStockStatus()
    {
        if (ThresholdQuantity.HasValue && AvailableQuantity <= ThresholdQuantity.Value)
        {
            IsLowStock = true;
            if (!LastLowStockNotificationAt.HasValue)
            {
                LastLowStockNotificationAt = DateTime.UtcNow;
            }
        }
        else
        {
            IsLowStock = false;
        }
    }
```

**Part C: Reserve & Commit**
```csharp
    // UC2.2: Reserve Stock
    public Result ReserveStock(
        int quantityToReserve,
        UserId buyerId,
        DateTime expiresAt,
        InventoryReservationType reservationType = InventoryReservationType.BuyItNow)
    {
        if (quantityToReserve <= 0)
            return InventoryErrors.InvalidQuantity;
        
        if (AvailableQuantity < quantityToReserve)
            return InventoryErrors.OutOfStock;
        
        AvailableQuantity -= quantityToReserve;
        ReservedQuantity += quantityToReserve;
        LastUpdatedAt = DateTime.UtcNow;
        
        var reservation = InventoryReservation.Create(
            InventoryReservationId.New(),
            Id,
            null,
            buyerId,
            reservationType,
            quantityToReserve,
            expiresAt);
        
        _reservations.Add(reservation);
        
        UpdateLowStockStatus();
        
        return Result.Success();
    }
    
    // UC2.4: Commit Stock
    public Result CommitStock(int quantityToCommit, InventoryReservationId reservationId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
        if (reservation == null)
            return InventoryErrors.ReservationNotFound;
        
        if (!reservation.IsActive)
            return InventoryErrors.InvalidStateTransition;
        
        SoldQuantity += quantityToCommit;
        ReservedQuantity -= quantityToCommit;
        LastUpdatedAt = DateTime.UtcNow;
        
        reservation.Commit();
        
        return Result.Success();
    }
    
    // UC2.5: Release Stock
    public Result ReleaseStock(int quantityToRelease, InventoryReservationId reservationId)
    {
        var reservation = _reservations.FirstOrDefault(r => r.Id == reservationId);
        if (reservation == null)
            return InventoryErrors.ReservationNotFound;
        
        if (ReservedQuantity < quantityToRelease)
            return InventoryErrors.InvalidQuantity;
        
        AvailableQuantity += quantityToRelease;
        ReservedQuantity -= quantityToRelease;
        LastUpdatedAt = DateTime.UtcNow;
        
        reservation.Release();
        
        UpdateLowStockStatus();
        
        return Result.Success();
    }
    
    // UC2.6: Restock from Return
    public Result RestockFromReturn(int quantityToRestock)
    {
        if (quantityToRestock <= 0)
            return InventoryErrors.InvalidQuantity;
        
        if (SoldQuantity < quantityToRestock)
            return InventoryErrors.InvalidQuantity;
        
        TotalQuantity += quantityToRestock;
        AvailableQuantity += quantityToRestock;
        SoldQuantity -= quantityToRestock;
        LastUpdatedAt = DateTime.UtcNow;
        
        UpdateLowStockStatus();
        
        return Result.Success();
    }
}
```

---

### Day 3️⃣: Creating Other Entities & Tests

#### Task 3.1: Create InventoryAdjustment Entity
**File**: `BE/PRN232_EbayClone/PRN232_EbayClone.Domain/Listings/Inventory/Entities/InventoryAdjustment.cs`

```csharp
using PRN232_EbayClone.Domain.Listings.Inventory.Enums;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Shared.Abstractions;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Domain.Listings.Inventory.Entities;

public sealed class InventoryAdjustment : Entity<Guid>
{
    public InventoryId InventoryId { get; private set; }
    public UserId SellerId { get; private set; }
    public InventoryAdjustmentType AdjustmentType { get; private set; }
    public int QuantityChange { get; private set; }  // Positive/negative
    public string? Reason { get; private set; }
    public DateTime AdjustedAt { get; private set; }
    public string? AdjustedBy { get; private set; }
    
    private InventoryAdjustment() { }
    
    public static InventoryAdjustment Create(
        InventoryId inventoryId,
        UserId sellerId,
        InventoryAdjustmentType type,
        int quantityChange,
        string? reason = null,
        string? adjustedBy = null)
    {
        return new InventoryAdjustment
        {
            Id = Guid.NewGuid(),
            InventoryId = inventoryId,
            SellerId = sellerId,
            AdjustmentType = type,
            QuantityChange = quantityChange,
            Reason = reason,
            AdjustedAt = DateTime.UtcNow,
            AdjustedBy = adjustedBy
        };
    }
}
```

---

#### Task 3.2: Create Unit Tests
**File**: `BE/PRN232_EbayClone.Tests/Listings/Inventory/InventoryEntityTests.cs`

```csharp
using FluentAssertions;
using PRN232_EbayClone.Domain.Listings.Inventory.Entities;
using PRN232_EbayClone.Domain.Listings.Inventory.Enums;
using PRN232_EbayClone.Domain.Listings.Inventory.Errors;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Tests.Listings.Inventory;

public class InventoryEntityTests
{
    private static readonly ListingId ListingId = ListingId.New();
    private static readonly UserId SellerId = UserId.New();
    
    [Fact]
    public void Create_WithValidQuantity_ShouldSucceed()
    {
        // Arrange
        var quantity = 100;
        
        // Act
        var result = Inventory.Create(ListingId, SellerId, quantity);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalQuantity.Should().Be(100);
        result.Value.AvailableQuantity.Should().Be(100);
        result.Value.ReservedQuantity.Should().Be(0);
        result.Value.SoldQuantity.Should().Be(0);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Create_WithNegativeQuantity_ShouldFail(int quantity)
    {
        // Act
        var result = Inventory.Create(ListingId, SellerId, quantity);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(InventoryErrors.InvalidQuantity);
    }
    
    [Fact]
    public void ReserveStock_WithSufficientQuantity_ShouldSucceed()
    {
        // Arrange
        var inventory = Inventory.Create(ListingId, SellerId, 100).Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(30);
        var buyerId = UserId.New();
        
        // Act
        var result = inventory.ReserveStock(50, buyerId, expiresAt);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        inventory.AvailableQuantity.Should().Be(50);
        inventory.ReservedQuantity.Should().Be(50);
        inventory.Reservations.Should().HaveCount(1);
    }
    
    [Fact]
    public void ReserveStock_WithInsufficientQuantity_ShouldFail()
    {
        // Arrange
        var inventory = Inventory.Create(ListingId, SellerId, 100).Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(30);
        var buyerId = UserId.New();
        
        // Act
        var result = inventory.ReserveStock(150, buyerId, expiresAt);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(InventoryErrors.OutOfStock);
    }
    
    [Fact]
    public void CommitStock_AfterReserve_ShouldSucceed()
    {
        // Arrange
        var inventory = Inventory.Create(ListingId, SellerId, 100).Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(30);
        var buyerId = UserId.New();
        inventory.ReserveStock(50, buyerId, expiresAt);
        
        var reservationId = inventory.Reservations.First().Id;
        
        // Act
        var result = inventory.CommitStock(50, reservationId);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        inventory.ReservedQuantity.Should().Be(0);
        inventory.SoldQuantity.Should().Be(50);
        inventory.AvailableQuantity.Should().Be(50);
    }
    
    [Fact]
    public void QuantityInvariant_ShouldAlwaysHold()
    {
        // Arrange
        var inventory = Inventory.Create(ListingId, SellerId, 100).Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(30);
        var buyerId = UserId.New();
        
        // Act
        inventory.ReserveStock(30, buyerId, expiresAt);
        inventory.Restock(20);
        
        // Assert
        var invariant = inventory.TotalQuantity == 
            (inventory.AvailableQuantity + inventory.ReservedQuantity + inventory.SoldQuantity);
        invariant.Should().BeTrue();
    }
    
    [Fact]
    public void SetLowStockThreshold_WhenBelowThreshold_ShouldMarkLow()
    {
        // Arrange
        var inventory = Inventory.Create(ListingId, SellerId, 100).Value;
        inventory.SetLowStockThreshold(50);
        
        // Act
        var expiresAt = DateTime.UtcNow.AddMinutes(30);
        var buyerId = UserId.New();
        inventory.ReserveStock(60, buyerId, expiresAt);
        
        // Assert
        inventory.IsLowStock.Should().BeTrue();
    }
}
```

---

### Day 4️⃣️: EF Core Configurations

#### Task 4.1: Create Inventory Configuration
**File**: `BE/PRN232_EbayClone/PRN232_EbayClone.Infrastructure/Persistence/Configurations/InventoryConfiguration.cs`

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PRN232_EbayClone.Domain.Listings.Inventory.Entities;
using PRN232_EbayClone.Domain.Listings.Inventory.ValueObjects;
using PRN232_EbayClone.Domain.Listings.ValueObjects;
using PRN232_EbayClone.Domain.Users.ValueObjects;

namespace PRN232_EbayClone.Infrastructure.Persistence.Configurations;

public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
{
    public void Configure(EntityTypeBuilder<Inventory> builder)
    {
        builder.ToTable("inventory");
        
        builder.HasKey(x => x.Id)
            .HasName("pk_inventory");
        
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => InventoryId.From(value))
            .HasColumnName("id")
            .IsRequired();
        
        builder.Property(x => x.ListingId)
            .HasConversion(
                id => id.Value,
                value => ListingId.From(value))
            .HasColumnName("listing_id")
            .IsRequired();
        
        builder.Property(x => x.SellerId)
            .HasConversion(
                id => id.Value,
                value => UserId.From(value))
            .HasColumnName("seller_id")
            .IsRequired();
        
        builder.Property(x => x.TotalQuantity)
            .HasColumnName("total_quantity")
            .IsRequired();
        
        builder.Property(x => x.AvailableQuantity)
            .HasColumnName("available_quantity")
            .IsRequired();
        
        builder.Property(x => x.ReservedQuantity)
            .HasColumnName("reserved_quantity")
            .IsRequired();
        
        builder.Property(x => x.SoldQuantity)
            .HasColumnName("sold_quantity")
            .IsRequired();
        
        builder.Property(x => x.ThresholdQuantity)
            .HasColumnName("threshold_quantity")
            .IsRequired(false);
        
        builder.Property(x => x.IsLowStock)
            .HasColumnName("is_low_stock")
            .IsRequired()
            .HasDefaultValue(false);
        
        builder.Property(x => x.LastLowStockNotificationAt)
            .HasColumnName("last_low_stock_notification_at")
            .IsRequired(false);
        
        builder.Property(x => x.LastUpdatedAt)
            .HasColumnName("last_updated_at")
            .IsRequired();
        
        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();
        
        // Foreign Keys
        builder.HasOne<Listing>()
            .WithMany()
            .HasForeignKey(x => x.ListingId)
            .HasConstraintName("fk_inventory_listing_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.SellerId)
            .HasConstraintName("fk_inventory_seller_id")
            .OnDelete(DeleteBehavior.Cascade);
        
        // Unique constraint
        builder.HasIndex(x => x.ListingId)
            .IsUnique()
            .HasDatabaseName("uk_inventory_listing_id");
        
        // Regular indexes
        builder.HasIndex(x => x.SellerId)
            .HasDatabaseName("idx_inventory_seller_id");
        
        builder.HasIndex(x => new { x.SellerId, x.IsLowStock })
            .HasDatabaseName("idx_inventory_is_low_stock");
        
        builder.HasIndex(x => x.LastUpdatedAt)
            .HasDatabaseName("idx_inventory_updated_at")
            .IsDescending();
    }
}
```

---

### Day 5️⃣: Apply Migrations & Validation

#### Task 5.1: Generate & Apply Migration
```bash
cd BE

# Generate migration
dotnet ef migrations add AddInventoryManagement \
  -p PRN232_EbayClone.Infrastructure \
  -s PRN232_EbayClone.Api

# Apply to database
dotnet ef database update \
  -p PRN232_EbayClone.Infrastructure \
  -s PRN232_EbayClone.Api
```

#### Task 5.2: Run Tests
```bash
# Build solution
dotnet build PRN232_EbayClone.sln

# Run Unit Tests
dotnet test PRN232_EbayClone.Tests/PRN232_EbayClone.Tests.csproj \
  --filter "InventoryEntityTests"

# Run All Tests (to ensure no breaking changes)
dotnet test PRN232_EbayClone.Tests/PRN232_EbayClone.Tests.csproj \
  --verbosity detailed
```

---

## ✅ Day 1 Checklist

- [ ] Migration file created & migration runs without error
- [ ] InventoryId value object compiles
- [ ] Enums created with all types
- [ ] InventoryErrors static class created
- [ ] InventoryReservation value object compiles
- [ ] Inventory aggregate root compiles (all parts)
- [ ] InventoryAdjustment entity created
- [ ] Unit tests created & pass
- [ ] EF Core configuration created
- [ ] Migration applied to database
- [ ] No other tests broken
- [ ] Push to git with commit: `[Thành] Add Inventory domain model phase 1`

---

## 📝 File Checklist

**Must Create** (9 files):
- [ ] Migration file: `*_AddInventoryManagement.cs`
- [ ] Value Objects: `InventoryId.cs`, `InventoryReservation.cs`
- [ ] Enums: `InventoryReservationType.cs` 
- [ ] Errors: `InventoryErrors.cs`
- [ ] Entities: `Inventory.cs`, `InventoryAdjustment.cs`
- [ ] Configuration: `InventoryConfiguration.cs`
- [ ] Tests: `InventoryEntityTests.cs`

**Already Exists** (integrate):
- EF Core DbContext → Add InventoryConfiguration
- Global Using → Already has what we need

---

## 🎯 Success Criteria

✅ **All domain entities compile without errors**  
✅ **Database migration applies successfully**  
✅ **Unit tests: 7+ passing tests**  
✅ **No existing tests broken**  
✅ **Quantity invariant always holds** (Total = Available + Reserved + Sold)  

---

## Next: Phase 2 (Week 2)
After Day 5, move to:
- Application layer (Commands/Queries)
- Repository implementations
- Integration tests with DB transactions
