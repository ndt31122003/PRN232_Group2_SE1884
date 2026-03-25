# Phân Tích Disputes Data

## Users trong Database

- **User 1 (Alice/demo1)**: `70000000-0000-0000-0000-000000000001`
- **User 2 (demo2)**: `70000000-0000-0000-0000-000000000002`
- **User 3 (demo3)**: `70000000-0000-0000-0000-000000000003`

## Listings

TẤT CẢ listings đều được tạo bởi User 1 (Alice):
- `created_by = '70000000-0000-0000-0000-000000000001'`

## Disputes trong Database

| Dispute ID | Listing ID | Raised By (Buyer) | Listing Owner (Seller) | Status |
|------------|------------|-------------------|------------------------|--------|
| ...440001 | 71000...001 | User 2 (demo2) | User 1 (Alice) | WaitingSeller |
| ...440002 | 71000...003 | User 3 (demo3) | User 1 (Alice) | Resolved |
| ...440003 | 71000...005 | User 2 (demo2) | User 1 (Alice) | Resolved |
| ...440004 | 72000...001 | User 1 (Alice) | User 1 (Alice) | Escalated |
| ...440005 | 72000...007 | User 3 (demo3) | User 1 (Alice) | Resolved |
| ...440006 | 73000...001 | User 1 (Alice) | User 1 (Alice) | Closed |
| ...440007 | 73000...005 | User 2 (demo2) | User 1 (Alice) | Open |

## Kết Quả Mong Đợi

### User 1 (Alice - demo1) - `70000000-0000-0000-0000-000000000001`
**Role**: SELLER (owner của tất cả listings)

**Nên thấy**: TẤT CẢ 7 disputes (vì là seller của tất cả listings)
- Dispute 1, 2, 3, 4, 5, 6, 7

**Endpoint**: `/api/seller/disputes`

---

### User 2 (demo2) - `70000000-0000-0000-0000-000000000002`
**Role**: BUYER

**Nên thấy**: CHỈ 3 disputes mà họ tạo ra
- Dispute 1 (440001) - WaitingSeller
- Dispute 3 (440003) - Resolved  
- Dispute 7 (440007) - Open

**Endpoint**: `/api/buyer/disputes`

---

### User 3 (demo3) - `70000000-0000-0000-0000-000000000003`
**Role**: BUYER

**Nên thấy**: CHỈ 2 disputes mà họ tạo ra
- Dispute 2 (440002) - Resolved
- Dispute 5 (440005) - Resolved

**Endpoint**: `/api/buyer/disputes`

---

## Logic Backend Mới

### BuyerDisputesController (`/api/buyer/disputes`)
```csharp
// Chỉ lấy disputes mà buyer này tạo ra
query = query.Where(d => d.RaisedById == buyerId);
```

### SellerDisputesController (`/api/seller/disputes`)
```csharp
// Lấy tất cả listings của seller
var sellerListings = await listingRepository.GetBySellerIdAsync(sellerId);
// Lấy disputes trên những listings đó
foreach (var listingId in listingIds) {
    var disputes = await disputeRepository.GetDisputesByListingIdAsync(listingId);
}
```

### DisputesController (`/api/disputes`)
```csharp
// Lấy cả hai: disputes raised by user HOẶC disputes on user's listings
query = query.Where(d => 
    d.RaisedById == currentUserId || 
    (d.Listing != null && d.Listing.CreatedBy == currentUserId));
```

---

## Frontend Logic

```javascript
// Buyer only: gọi /api/buyer/disputes
if (isBuyer && !isSeller) {
    result = await DisputeService.getBuyerDisputes(currentUserId, filterParams);
}
// Seller only: gọi /api/seller/disputes  
else if (isSeller && !isBuyer) {
    result = await DisputeService.getSellerDisputes(filterParams);
}
// Both roles: gọi /api/disputes
else if (isSeller && isBuyer) {
    result = await DisputeService.getDisputes(filterParams);
}
```

---

## Cách Test

1. **Login as demo2** (buyer):
   - Mở browser console
   - Vào `/disputes`
   - Kiểm tra network tab: Nên gọi `/api/buyer/disputes`
   - Nên chỉ thấy 3 disputes (440001, 440003, 440007)

2. **Login as demo3** (buyer):
   - Nên chỉ thấy 2 disputes (440002, 440005)

3. **Login as Alice/demo1** (seller):
   - Nên thấy tất cả 7 disputes

4. **Kiểm tra backend log**:
   - Nên thấy log: `[BuyerDisputesController] GetBuyerDisputes called by buyer: xxx`
   - Hoặc: `[SellerDisputesController] GetSellerDisputes called by seller: xxx`
