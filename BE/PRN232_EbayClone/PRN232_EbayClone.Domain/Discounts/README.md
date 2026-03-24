# Hệ thống Discount - Aligned với eBay

Hệ thống discount được thiết kế để align với 5 loại discount options của eBay:

## 1. Coupon (Mã giảm giá)
- **Entity**: `Coupon` (đã có sẵn)
- **Mô tả**: Mã giảm giá mà người mua nhập vào khi checkout
- **Tính năng**:
  - Nhiều loại coupon (Extra Percent Off, Buy X Get Y, etc.)
  - Điều kiện áp dụng (minimum order value, excluded categories)
  - Giới hạn sử dụng (usage limit, usage per user)

## 2. Order Discount (Giảm giá đơn hàng)
- **Entity**: `OrderDiscount`
- **Mô tả**: Giảm giá áp dụng trực tiếp cho đơn hàng, không cần mã
- **Tính năng**:
  - Giảm theo % hoặc số tiền cố định
  - Giới hạn giảm tối đa (max discount)
  - Điều kiện đơn hàng tối thiểu (minimum order value)
  - Thời gian áp dụng

**Ví dụ**: "Giảm 10% cho đơn hàng từ $50 trở lên"

## 3. Sale Event (Sự kiện giảm giá)
- **Entity**: `SaleEvent` (đã có sẵn)
- **Mô tả**: Sự kiện sale với nhiều tầng giảm giá cho các sản phẩm
- **Tính năng**:
  - Nhiều discount tiers với priority
  - Áp dụng cho danh sách listings cụ thể
  - Free shipping option
  - Highlight percentage

## 4. Shipping Discount (Giảm giá vận chuyển)
- **Entity**: `ShippingDiscount`
- **Mô tả**: Giảm giá hoặc miễn phí vận chuyển
- **Tính năng**:
  - Free shipping (miễn phí hoàn toàn)
  - Giảm % hoặc số tiền cố định phí ship
  - Điều kiện đơn hàng tối thiểu
  - Thời gian áp dụng

**Ví dụ**: "Free shipping cho đơn từ $25", "Giảm 50% phí ship"

## 5. Volume Pricing (Giá theo số lượng)
- **Entity**: `VolumePricing`
- **Mô tả**: Giảm giá khi mua số lượng lớn
- **Tính năng**:
  - Nhiều tiers theo số lượng
  - Giảm theo % hoặc số tiền
  - Áp dụng cho listing cụ thể hoặc toàn bộ store

**Ví dụ**:
- Mua 2-4 items: giảm 5%
- Mua 5-9 items: giảm 10%
- Mua 10+ items: giảm 15%

## Cách sử dụng

### 1. Tạo discount
```csharp
// Order Discount
var orderDiscount = OrderDiscount.Create(
    sellerId: sellerId,
    name: "Summer Sale",
    description: "10% off orders over $50",
    discountValue: 10m,
    discountUnit: DiscountUnit.Percent,
    maxDiscount: 20m,
    minimumOrderValue: 50m,
    startDate: DateTime.UtcNow,
    endDate: DateTime.UtcNow.AddDays(30)
);

// Shipping Discount
var shippingDiscount = ShippingDiscount.Create(
    sellerId: sellerId,
    name: "Free Shipping",
    description: "Free shipping on orders over $25",
    discountValue: 0m,
    discountUnit: DiscountUnit.FixedAmount,
    isFreeShipping: true,
    minimumOrderValue: 25m,
    startDate: DateTime.UtcNow,
    endDate: DateTime.UtcNow.AddDays(30)
);

// Volume Pricing
var volumePricing = VolumePricing.Create(
    sellerId: sellerId,
    listingId: listingId,
    name: "Bulk Discount",
    description: "Save more when you buy more",
    startDate: DateTime.UtcNow,
    endDate: DateTime.UtcNow.AddDays(90),
    tierDefinitions: new[]
    {
        new VolumePricingTierDefinition(2, 5m, DiscountUnit.Percent),
        new VolumePricingTierDefinition(5, 10m, DiscountUnit.Percent),
        new VolumePricingTierDefinition(10, 15m, DiscountUnit.Percent)
    }
);
```

### 2. Tính toán discount
```csharp
var calculationService = new DiscountCalculationService();

var result = calculationService.CalculateTotalDiscount(
    orderSubtotal: orderTotal,
    shippingCost: shippingCost,
    totalItemCount: itemCount,
    applicableDiscounts: new IDiscount[] 
    { 
        orderDiscount, 
        shippingDiscount, 
        volumePricing 
    }
);

if (result.IsSuccess)
{
    var orderDiscountAmount = result.Value.OrderDiscount;
    var shippingDiscountAmount = result.Value.ShippingDiscount;
    var totalDiscountAmount = result.Value.TotalDiscount;
}
```

## Interface IDiscount

Tất cả các loại discount đều implement interface `IDiscount`:

```csharp
public interface IDiscount
{
    Guid Id { get; }
    DiscountType Type { get; }
    string Name { get; }
    bool IsActive { get; }
    DateTime StartDate { get; }
    DateTime EndDate { get; }
    
    Result<Money> CalculateDiscount(Money orderTotal, int itemCount);
    bool IsApplicable(DateTime currentDate);
}
```

## Lưu ý

1. **Coupon** và **SaleEvent** đã tồn tại trong hệ thống, cần refactor để implement `IDiscount`
2. Các discount có thể áp dụng đồng thời (stackable)
3. Mỗi loại discount có logic tính toán riêng
4. `DiscountCalculationService` xử lý việc tổng hợp nhiều discount
