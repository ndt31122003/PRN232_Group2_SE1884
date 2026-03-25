using System;
using System.Linq;
using FluentAssertions;
using Xunit;
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
        var result = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, quantity);
        
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
        var result = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, quantity);
        
        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(InventoryErrors.InvalidQuantity);
    }
    
    [Fact]
    public void ReserveStock_WithSufficientQuantity_ShouldSucceed()
    {
        // Arrange
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 100).Value;
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
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 100).Value;
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
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 100).Value;
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
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 100).Value;
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
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 100).Value;
        inventory.SetLowStockThreshold(50);
        
        // Act
        var expiresAt = DateTime.UtcNow.AddMinutes(30);
        var buyerId = UserId.New();
        inventory.ReserveStock(60, buyerId, expiresAt);
        
        // Assert
        inventory.IsLowStock.Should().BeTrue();
    }

    [Fact]
    public void ConfigureLowStockAlert_WithThresholdAndEmail_ShouldPersistConfiguration()
    {
        // Arrange
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 25).Value;

        // Act
        var result = inventory.ConfigureLowStockAlert(20, true);

        // Assert
        result.IsSuccess.Should().BeTrue();
        inventory.ThresholdQuantity.Should().Be(20);
        inventory.EmailNotificationsEnabled.Should().BeTrue();
        inventory.IsLowStock.Should().BeFalse();
    }

    [Fact]
    public void ConfigureLowStockAlert_WithEmailAndNoThreshold_ShouldFail()
    {
        // Arrange
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 25).Value;

        // Act
        var result = inventory.ConfigureLowStockAlert(null, true);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(InventoryErrors.EmailAlertRequiresThreshold);
    }

    [Fact]
    public void Restock_WhenInventoryRecovers_ShouldClearNotificationTimestamp()
    {
        // Arrange
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 10).Value;
        inventory.ConfigureLowStockAlert(10, true);
        inventory.MarkLowStockNotificationSent(DateTime.UtcNow);

        // Act
        inventory.Restock(5);

        // Assert
        inventory.IsLowStock.Should().BeFalse();
        inventory.LastLowStockNotificationAt.Should().BeNull();
    }
    
    [Fact]
    public void ReleaseStock_AfterReserve_ShouldSuccess()
    {
        // Arrange
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 100).Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(30);
        var buyerId = UserId.New();
        inventory.ReserveStock(50, buyerId, expiresAt);
        
        var reservationId = inventory.Reservations.First().Id;
        
        // Act
        var result = inventory.ReleaseStock(50, reservationId);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        inventory.AvailableQuantity.Should().Be(100);
        inventory.ReservedQuantity.Should().Be(0);
    }
    
    [Fact]
    public void RestockFromReturn_ShouldUpdateQuantities()
    {
        // Arrange
        var inventory = Domain.Listings.Inventory.Entities.Inventory.Create(ListingId, SellerId, 100).Value;
        var expiresAt = DateTime.UtcNow.AddMinutes(30);
        var buyerId = UserId.New();
        
        // Reserve and commit
        inventory.ReserveStock(50, buyerId, expiresAt);
        var reservationId = inventory.Reservations.First().Id;
        inventory.CommitStock(50, reservationId);
        
        // Act - Restock 20 items from return
        var result = inventory.RestockFromReturn(20);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        inventory.AvailableQuantity.Should().Be(70);
        inventory.SoldQuantity.Should().Be(30);
        inventory.TotalQuantity.Should().Be(120);
    }
}
