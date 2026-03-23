# Marketing Module Integration Notes

## Structure

```
FE/src/pages/Marketing/
├── MarketingLayout.jsx          # Layout with sidebar (wraps all marketing pages)
├── MarketingLayout.scss
├── PromotionsOverview.jsx       # Main promotions page (table view)
├── PromotionsOverview.scss
├── SaleEvents/
│   ├── SaleEventsSummary.jsx   # Now rendered inside MarketingLayout
│   └── CreateSaleEvent.jsx     # Standalone (no sidebar)
└── (Future: Coupons will move here from ../Coupons/)
```

## Sidebar Navigation

The sidebar shows:
- Summary (Coming soon)
- **Promotions** (Active - shows 5 discount types)
- Offers (Coming soon) - with "NEW" badge
- Buyer groups (Coming soon)
- Social (Coming soon)

## Promotions Page Layout

Matches eBay's design:
- **Header**: Title + subtitle
- **Performance section**: Stats card (placeholder for now)
- **Your discounts section**: Table with 5 discount types
  - Columns: Name | Discount type | Start date | Status | Actions
  - Each row has: Icon, Title, Description, Type label, Status badge, Create/View buttons

## 5 Discount Types

1. **Coupon** ✅
   - Icon: 🎟️
   - Type: Code-based discount
   - Status: Available
   - Actions: Create → `/marketing/coupons/create`, View all → `/marketing/coupons`

2. **Order discount** 🔜
   - Icon: 💰
   - Type: Automatic discount
   - Status: Coming Soon
   - Actions: Disabled

3. **Sale event** ✅
   - Icon: 🎉
   - Type: Event-based discount
   - Status: Available
   - Actions: Create → `/marketing/sale-events/create`, View all → `/marketing/sale-events`

4. **Shipping discount** 🔜
   - Icon: 📦
   - Type: Shipping promotion
   - Status: Coming Soon
   - Actions: Disabled

5. **Volume pricing** 🔜
   - Icon: 📊
   - Type: Quantity discount
   - Status: Coming Soon
   - Actions: Disabled

## Router Structure

```javascript
{
  path: "marketing",
  element: <MarketingLayout />,  // Has sidebar
  children: [
    { index: true, element: <PromotionsOverview /> },
    { path: "coupons", element: <CouponsSummary /> },
    { path: "sale-events", element: <SaleEventsSummary /> }
  ]
},
// Create pages are standalone (no sidebar)
{ path: "marketing/coupons/create", element: <CreateSellerCoupon /> },
{ path: "marketing/sale-events/create", element: <CreateSaleEvent /> }
```

## Next Steps

1. Move Coupons folder into Marketing folder
2. Create OrderDiscounts, ShippingDiscounts, VolumePricing pages
3. Implement actual data fetching for performance stats
4. Add filters and search to the table
5. Implement Summary page with analytics
