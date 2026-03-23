import React, { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import axios from "../../utils/axiosCustomize";
import Notice from "../Common/CustomNotification";
import "./ShoppingCart.scss";

const SaleEventMode = {
  DiscountAndSaleEvent: 1,
  SaleEventOnly: 2
};

const SaleEventDiscountType = {
  Percent: 1,
  Amount: 2
};

const formatCurrency = (value) => {
  if (value === null || value === undefined) {
    return "$0.00";
  }

  const numeric = Number(value);
  if (!Number.isFinite(numeric)) {
    return "$0.00";
  }

  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 2
  }).format(numeric);
};

const calculateSalePrice = (originalPrice, tier) => {
  if (!tier) {
    return originalPrice;
  }

  if (tier.discountType === SaleEventDiscountType.Percent) {
    const discount = originalPrice * (tier.discountValue / 100);
    return Math.max(0, originalPrice - discount);
  }

  if (tier.discountType === SaleEventDiscountType.Amount) {
    return Math.max(0, originalPrice - tier.discountValue);
  }

  return originalPrice;
};

const ShoppingCart = () => {
  const navigate = useNavigate();

  const [loading, setLoading] = useState(true);
  const [cartItems, setCartItems] = useState([]);
  const [saleEventsByListing, setSaleEventsByListing] = useState({});

  const loadCart = useCallback(async () => {
    try {
      const response = await axios.get("cart", { suppressErrorNotice: true });
      const items = response?.data?.items ?? [];
      setCartItems(Array.isArray(items) ? items : []);
      return items;
    } catch (error) {
      Notice({
        msg: "Could not load cart.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
      return [];
    }
  }, []);

  const loadSaleEventsForItems = useCallback(async (items) => {
    if (!Array.isArray(items) || items.length === 0) {
      setSaleEventsByListing({});
      return;
    }

    const saleEventsMap = {};

    await Promise.all(
      items.map(async (item) => {
        const listingId = item.listingId ?? item.ListingId;
        if (!listingId) {
          return;
        }

        try {
          const response = await axios.get(`sale-events/listing/${listingId}/active`, {
            suppressErrorNotice: true
          });
          const events = response?.data ?? [];
          if (Array.isArray(events) && events.length > 0) {
            saleEventsMap[listingId] = events[0];
          }
        } catch (error) {
          console.error(`Could not load sale events for listing ${listingId}:`, error);
        }
      })
    );

    setSaleEventsByListing(saleEventsMap);
  }, []);

  useEffect(() => {
    const load = async () => {
      setLoading(true);
      const items = await loadCart();
      await loadSaleEventsForItems(items);
      setLoading(false);
    };

    load();
  }, [loadCart, loadSaleEventsForItems]);

  const handleRemoveItem = async (itemId) => {
    try {
      await axios.delete(`cart/items/${itemId}`, { suppressErrorNotice: true });
      Notice({ msg: "Item removed from cart.", isSuccess: true });
      const items = await loadCart();
      await loadSaleEventsForItems(items);
    } catch (error) {
      Notice({
        msg: "Could not remove item.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    }
  };

  const handleUpdateQuantity = async (itemId, newQuantity) => {
    if (newQuantity < 1) {
      return;
    }

    try {
      await axios.put(`cart/items/${itemId}`, { quantity: newQuantity }, { suppressErrorNotice: true });
      const items = await loadCart();
      await loadSaleEventsForItems(items);
    } catch (error) {
      Notice({
        msg: "Could not update quantity.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    }
  };

  const calculateItemTotal = (item) => {
    const listingId = item.listingId ?? item.ListingId;
    const saleEvent = saleEventsByListing[listingId];
    const quantity = item.quantity ?? 1;
    const originalPrice = item.price ?? 0;

    if (!saleEvent || saleEvent.mode !== SaleEventMode.DiscountAndSaleEvent) {
      return originalPrice * quantity;
    }

    const tiers = saleEvent.discountTiers ?? [];
    const appliedTier = tiers.length > 0 ? tiers[0] : null;
    const salePrice = calculateSalePrice(originalPrice, appliedTier);

    return salePrice * quantity;
  };

  const calculateSubtotal = () => {
    return cartItems.reduce((sum, item) => sum + calculateItemTotal(item), 0);
  };

  const calculateTotalDiscount = () => {
    return cartItems.reduce((sum, item) => {
      const listingId = item.listingId ?? item.ListingId;
      const saleEvent = saleEventsByListing[listingId];
      const quantity = item.quantity ?? 1;
      const originalPrice = item.price ?? 0;

      if (!saleEvent || saleEvent.mode !== SaleEventMode.DiscountAndSaleEvent) {
        return sum;
      }

      const tiers = saleEvent.discountTiers ?? [];
      const appliedTier = tiers.length > 0 ? tiers[0] : null;
      const salePrice = calculateSalePrice(originalPrice, appliedTier);
      const discount = (originalPrice - salePrice) * quantity;

      return sum + discount;
    }, 0);
  };

  const handleCheckout = () => {
    navigate("/checkout");
  };

  if (loading) {
    return (
      <div className="shopping-cart">
        <div className="shopping-cart__loading">Loading cart...</div>
      </div>
    );
  }

  if (cartItems.length === 0) {
    return (
      <div className="shopping-cart">
        <div className="shopping-cart__empty">
          <h2>Your cart is empty</h2>
          <p>Add items to your cart to get started.</p>
        </div>
      </div>
    );
  }

  const subtotal = calculateSubtotal();
  const totalDiscount = calculateTotalDiscount();

  return (
    <div className="shopping-cart">
      <header className="shopping-cart__header">
        <h1>Shopping cart ({cartItems.length} {cartItems.length === 1 ? "item" : "items"})</h1>
      </header>

      <div className="shopping-cart__content">
        <div className="shopping-cart__items">
          {cartItems.map((item) => {
            const listingId = item.listingId ?? item.ListingId;
            const saleEvent = saleEventsByListing[listingId];
            const quantity = item.quantity ?? 1;
            const originalPrice = item.price ?? 0;

            let salePrice = originalPrice;
            let appliedTier = null;

            if (saleEvent && saleEvent.mode === SaleEventMode.DiscountAndSaleEvent) {
              const tiers = saleEvent.discountTiers ?? [];
              appliedTier = tiers.length > 0 ? tiers[0] : null;
              salePrice = calculateSalePrice(originalPrice, appliedTier);
            }

            const itemTotal = salePrice * quantity;
            const itemDiscount = (originalPrice - salePrice) * quantity;

            return (
              <div key={item.id ?? item.Id} className="shopping-cart__item">
                <div className="shopping-cart__item-image">
                  {item.thumbnailUrl || item.thumbnail ? (
                    <img src={item.thumbnailUrl || item.thumbnail} alt={item.title} />
                  ) : (
                    <div className="shopping-cart__item-image-placeholder">No image</div>
                  )}
                </div>

                <div className="shopping-cart__item-details">
                  <h3 className="shopping-cart__item-title">{item.title}</h3>

                  {saleEvent ? (
                    <div className="shopping-cart__item-sale">
                      <div className="shopping-cart__item-sale-badge">
                        {saleEvent.name}
                      </div>
                      {saleEvent.mode === SaleEventMode.DiscountAndSaleEvent && itemDiscount > 0 ? (
                        <div className="shopping-cart__item-discount">
                          You save {formatCurrency(itemDiscount)}
                        </div>
                      ) : null}
                    </div>
                  ) : null}

                  <div className="shopping-cart__item-pricing">
                    {saleEvent && saleEvent.mode === SaleEventMode.DiscountAndSaleEvent && salePrice < originalPrice ? (
                      <>
                        <span className="shopping-cart__item-price-original">
                          {formatCurrency(originalPrice)}
                        </span>
                        <span className="shopping-cart__item-price-sale">
                          {formatCurrency(salePrice)}
                        </span>
                      </>
                    ) : (
                      <span className="shopping-cart__item-price">
                        {formatCurrency(originalPrice)}
                      </span>
                    )}
                  </div>

                  <div className="shopping-cart__item-quantity">
                    <label>Quantity:</label>
                    <input
                      type="number"
                      min="1"
                      value={quantity}
                      onChange={(e) => handleUpdateQuantity(item.id ?? item.Id, parseInt(e.target.value, 10))}
                    />
                  </div>

                  <button
                    type="button"
                    className="shopping-cart__item-remove"
                    onClick={() => handleRemoveItem(item.id ?? item.Id)}
                  >
                    Remove
                  </button>
                </div>

                <div className="shopping-cart__item-total">
                  {formatCurrency(itemTotal)}
                </div>
              </div>
            );
          })}
        </div>

        <div className="shopping-cart__summary">
          <h2>Order summary</h2>

          <div className="shopping-cart__summary-row">
            <span>Subtotal:</span>
            <span>{formatCurrency(subtotal)}</span>
          </div>

          {totalDiscount > 0 ? (
            <div className="shopping-cart__summary-row shopping-cart__summary-row--discount">
              <span>Sale event discount:</span>
              <span>-{formatCurrency(totalDiscount)}</span>
            </div>
          ) : null}

          <div className="shopping-cart__summary-row shopping-cart__summary-row--total">
            <span>Total:</span>
            <span>{formatCurrency(subtotal)}</span>
          </div>

          <button
            type="button"
            className="shopping-cart__checkout-btn"
            onClick={handleCheckout}
          >
            Proceed to checkout
          </button>
        </div>
      </div>
    </div>
  );
};

export default ShoppingCart;
