export const COUPON_TYPE_CATALOG = [
  {
    id: "9E1D4EA5-5B09-48BE-BE90-E2790F6BA537",
    label: "Extra % off",
    summary: "Give buyers an instant percentage discount across eligible items.",
    defaultUnit: "Percent",
    unitLocked: true,
    requiresMinOrderValue: false,
    conditionMode: "none",
    heroTagline: "Perfect for storewide flash sales and holiday drops.",
    colorToken: "#2067b4"
  },
  {
    id: "0D0C32FE-349C-4857-B20A-2D3F8DB91ED4",
    label: "Extra % off Y or more items",
    summary: "Reward multi-item baskets with a higher percent off.",
    defaultUnit: "Percent",
    unitLocked: true,
    requiresMinOrderValue: false,
    conditionMode: "single",
    heroTagline: "Boost average order value by encouraging bundles.",
    colorToken: "#6c2eb9",
    conditionFields: [
      {
        key: "saveEveryItems",
        label: "Minimum items in cart",
        type: "number",
        min: 1,
        step: 1,
        helper: "Buy quantity that unlocks the extra percent off."
      },
      {
        key: "conditionDescription",
        label: "Buyer message (optional)",
        type: "text",
        placeholder: "e.g. Buy 3+ items, save extra"
      }
    ]
  },
  {
    id: "CFA2E0F1-B720-4590-A7D4-4CE0844F9671",
    label: "Extra $ off $ or more",
    summary: "Offer a dollar discount when the cart hits a spend threshold.",
    defaultUnit: "Amount",
    unitLocked: true,
    requiresMinOrderValue: true,
    conditionMode: "none",
    heroTagline: "Great for pushing accessories or upsells.",
    colorToken: "#d33f49"
  },
  {
    id: "7EAA19CF-6B36-4A1C-B7B5-A9ABCB7EEFF2",
    label: "Buy X get Y at % off",
    summary: "Classic bundle incentive with a partial discount on the bonus item.",
    defaultUnit: "Percent",
    unitLocked: true,
    requiresMinOrderValue: false,
    conditionMode: "single",
    heroTagline: "Drive attachment sales with tailored bundles.",
    colorToken: "#116149",
    conditionFields: [
      {
        key: "buyQuantity",
        label: "Items buyer must purchase",
        type: "number",
        min: 1,
        step: 1
      },
      {
        key: "getQuantity",
        label: "Bonus items eligible",
        type: "number",
        min: 1,
        step: 1
      },
      {
        key: "getDiscountPercent",
        label: "Discount % on bonus items",
        type: "number",
        min: 1,
        max: 100,
        step: 1
      },
      {
        key: "conditionDescription",
        label: "Buyer message (optional)",
        type: "text",
        placeholder: "e.g. Buy 2, get 1 50% off"
      }
    ]
  },
  {
    id: "ED9D5151-6F8C-4628-A5A9-4C24867E5673",
    label: "Buy X get Y free",
    summary: "Give away the bonus item when the buyer qualifies.",
    defaultUnit: "Amount",
    unitLocked: true,
    requiresMinOrderValue: false,
    conditionMode: "single",
    heroTagline: "Move seasonal stock with aggressive bundles.",
    colorToken: "#fa8c16",
    conditionFields: [
      {
        key: "buyQuantity",
        label: "Items buyer must purchase",
        type: "number",
        min: 1,
        step: 1
      },
      {
        key: "getQuantity",
        label: "Items buyer gets free",
        type: "number",
        min: 1,
        step: 1
      },
      {
        key: "conditionDescription",
        label: "Buyer message (optional)",
        type: "text",
        placeholder: "e.g. Buy 3 tees, get 1 free"
      }
    ]
  },
  {
    id: "2C5A6A6A-FE7E-4813-A134-70572B5AB90A",
    label: "Extra % off $ or more",
    summary: "Unlocked percent discount once buyers hit a spend target.",
    defaultUnit: "Percent",
    unitLocked: true,
    requiresMinOrderValue: true,
    conditionMode: "none",
    heroTagline: "Encourage higher cart values without managing bundles.",
    colorToken: "#0f4c75"
  },
  {
    id: "773F8D9B-EB8E-4FF4-A21E-4BB2FA5407F4",
    label: "Extra $ off X or more items",
    summary: "Dollar discount triggered by multi-item carts.",
    defaultUnit: "Amount",
    unitLocked: true,
    requiresMinOrderValue: false,
    conditionMode: "single",
    heroTagline: "Perfect for lots or product families.",
    colorToken: "#9c27b0",
    conditionFields: [
      {
        key: "saveEveryItems",
        label: "Minimum items in cart",
        type: "number",
        min: 1,
        step: 1
      },
      {
        key: "conditionDescription",
        label: "Buyer message (optional)",
        type: "text",
        placeholder: "e.g. Save $10 when you buy 4"
      }
    ]
  },
  {
    id: "990C28B3-753E-41B1-A798-965CF46B7DCD",
    label: "Extra $ off",
    summary: "Flat dollar discount across eligible items.",
    defaultUnit: "Amount",
    unitLocked: true,
    requiresMinOrderValue: false,
    conditionMode: "none",
    heroTagline: "Simple promotion buyers instantly understand.",
    colorToken: "#ff5a5f"
  },
  {
    id: "7A5A0B7A-ED8F-4B91-A7C3-59E5363B76F3",
    label: "Extra $ off each item",
    summary: "Discount applies to every eligible item individually.",
    defaultUnit: "Amount",
    unitLocked: true,
    requiresMinOrderValue: false,
    conditionMode: "none",
    heroTagline: "Ideal for depth offers across large catalogs.",
    colorToken: "#0070ba"
  },
  {
    id: "3B980145-62B6-4AE6-9CF8-7838BC7B84E0",
    label: "Save $ for every X items",
    summary: "Structured savings applied per quantity tier.",
    defaultUnit: "Amount",
    unitLocked: true,
    requiresMinOrderValue: false,
    conditionMode: "single",
    heroTagline: "Scale discounts as quantities climb.",
    colorToken: "#37474f",
    conditionFields: [
      {
        key: "saveEveryAmount",
        label: "Savings per tier",
        type: "number",
        min: 1,
        step: 0.5
      },
      {
        key: "saveEveryItems",
        label: "Items per tier",
        type: "number",
        min: 1,
        step: 1
      },
      {
        key: "conditionDescription",
        label: "Buyer message (optional)",
        type: "text",
        placeholder: "e.g. Save $5 every 3 items"
      }
    ]
  },
  {
    id: "51F2ED38-06BB-496E-B5CB-7AA3057C21B7",
    label: "Save $ for every $ spent",
    summary: "Customers earn savings as they spend more.",
    defaultUnit: "Amount",
    unitLocked: true,
    requiresMinOrderValue: false,
    conditionMode: "single",
    heroTagline: "Reward loyal buyers for bigger carts.",
    colorToken: "#ff7f50",
    conditionFields: [
      {
        key: "saveEveryAmount",
        label: "Spend per reward tier",
        type: "number",
        min: 1,
        step: 1
      },
      {
        key: "conditionDescription",
        label: "Buyer message (optional)",
        type: "text",
        placeholder: "e.g. Save $5 every $50"
      }
    ]
  }
];

export const getCouponTypeById = (id) =>
  COUPON_TYPE_CATALOG.find((type) => type.id === id) || null;

