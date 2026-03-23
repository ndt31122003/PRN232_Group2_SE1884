import React, { useCallback, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import axios from "../../utils/axiosCustomize";
import Notice from "../../components/Common/CustomNotification";
import "./CheckoutPage.scss";

const SaleEventMode = {
  DiscountAndSaleEvent: 1,
  SaleEventOnly: 2
};

const SaleEventDiscountType = {
  Percent: 1,
  Amount: 2
};

const SaleEventStatus = {
  Draft: 1,
  Scheduled: 2,
  Active: 3,
  Ended: 4,
  Cancelled: 5
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

const CheckoutPage = () => {
  const navigate = useNavigate();

  const [loading, setLoading] = useState(true);
  const [revalidating, setRevalidating] = useState(false);
  const [cartItems, setCartItems] = useState([]);
  const [saleEventsByListing, setSaleEventsByListing] = useState({});
  const [revalidationChanges, setRevalidationChanges] = useState([]);
  const [showChangesModal, setShowChangesModal] = useState(false);

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

  const revalidateSaleEvents = useCallback(async (items) => {
    if (!Array.isArray(items) || items.length === 0) {
      setSaleEventsByListing({});
      return { saleEventsMap: {}, changes: [] };
    }

    const saleEventsMap = {};
    const changes = [];

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
            const saleEvent = events[0];

            const isExpired = dayjs(saleEvent.endDate).isBefore(dayjs());
            const isDeactivated = saleEvent.status !== SaleEventStatus.Active;
            const isListingRemoved = !saleEvent.discountTiers?.some((tier) =>
              tier.listings?.some((listing) => listing.listingId === listingId)
            );

            if (isExpired) {
              changes.push({
                listingId,
                listingTitle: item.title,
                reason: "Sale event has expired",
                saleEventName: saleEvent.name
              });
            } else if (isDeactivated) {
              changes.push({
                listingId,
                listingTitle: item.title,
                reason: "Sale event has been deactivated",
                saleEventName: saleEvent.name
              });
            } else if (isListingRemoved) {
              changes.push({
                listingId,
                listingTitle: item.title,
                reason: "Listing has been removed from sale event",
                saleEventName: saleEvent.name
              });
            } else {
              saleEventsMap[listingId] = saleEvent;
            }
          }
        } catch (error) {
          console.error(`Could not revalidate sale event for listing ${listingId}:`, error);
        }
      })
    );

    setSaleEventsByListing(saleEventsMap);
    return { saleEventsMap, changes };
  }, []);

  useEffect(() => {
    const load = async () => {
      setLoading(true);
      const items = await loadCart();
      const { changes } = await revalidateSaleEvents(items);

      if (changes.length > 0) {
        setRevalidationChanges(changes);
        setShowChangesModal(true);
      }

      setLoading(false);
    };

    load();
  }, [loadCart, revalidateSaleEvents]);

  const handleRevalidate = async () => {
    setRevalidating(true);
    const items = await loadCart();
    const { changes } = await revalidateSaleEvents(items);

    if (changes.length > 0) {
      setRevalidationChanges(changes);
      setShowChangesModal(true);
      Notice({
        msg: "Sale event changes detected",
        desc: `${changes.length} ${changes.length === 1 ? "item has" : "items have"} sale event changes.`,
        isSuccess: false
      });
    } else {
      Notice({ msg: "All sale events are still valid.", isSuccess: true });
    }

    setRevalidating(false);
  };

  const handleCloseChangesModal = () => {
    setShowChangesModal(false);
  };

  const handlePlaceOrder = async () => {
    setRevalidating(true);
    const items = await loadCart();
    const { changes } = await revalidateSaleEvents(items);

    if (changes.length > 0) {
      setRevalidationChanges(changes);
      setShowChangesModal(true);
      setRevalidating(false);
      Notice({
        msg: "Cannot place order",
        desc: "Some sale events have changed. Please review the changes.",
        isSuccess: false
      });
      return;
    }

    try {
      await axios.post("orders", {}, { suppressErrorNotice: true });
      Notice({ msg: "Order placed successfully!", isSuccess: true });
      navigate("/orders");
    } catch (error) {
      Notice({
        msg: "Could not place order.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    } finally {
      setRevalidating(false);
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

  if (loading) {
    return (
      <div className="checkout-page">
        <div className="checkout-page__loading">Loading checkout...</div>
      </div>
    );
  }

  if (cartItems.length === 0) {
    return (
      <div className="checkout-page">
        <div className="checkout-page__empty">
          <h2>Your cart is empty</h2>
          <p>Add items to your cart before checking out.</p>
          <button type="button" className="btn-primary" onClick={() => navigate("/")}>
            Continue shopping
          </button>
        </div>
      </div>
    );
  }

  const subtotal = calculateSubtotal();
  const totalDiscount = calculateTotalDiscount();
  const total = subtotal;

  return (
    <div className="checkout-page">
      <header className="checkout-page__header">
        <h1>Checkout</h1>
        <button
          type="button"
          className="checkout-page__revalidate"
          onClick={handleRevalidate}
          disabled={revalidating}
        >
          {revalidating ? "Checking..." : "Revalidate sale events"}
        </button>
      </header>

      <div className="checkout-page__content">
        <div className="checkout-page__items">
          <h2>Order items</h2>
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

            return (
              <div key={item.id ?? item.Id} className="checkout-page__item">
                <div className="checkout-page__item-image">
                  {item.thumbnailUrl || item.thumbnail ? (
                    <img src={item.thumbnailUrl || item.thumbnail} alt={item.title} />
                  ) : (
                    <div className="checkout-page__item-image-placeholder">No image</div>
                  )}
                </div>

                <div className="checkout-page__item-details">
                  <h3>{item.title}</h3>
                  {saleEvent ? (
                    <div className="checkout-page__item-sale-badge">
                      {saleEvent.name}
                    </div>
                  ) : null}
                  <div className="checkout-page__item-quantity">Qty: {quantity}</div>
                </div>

                <div className="checkout-page__item-pricing">
                  {saleEvent && saleEvent.mode === SaleEventMode.DiscountAndSaleEvent && salePrice < originalPrice ? (
                    <>
                      <div className="checkout-page__item-price-original">
                        {formatCurrency(originalPrice)}
                      </div>
                      <div className="checkout-page__item-price-sale">
                        {formatCurrency(salePrice)}
                      </div>
                    </>
                  ) : (
                    <div className="checkout-page__item-price">
                      {formatCurrency(originalPrice)}
                    </div>
                  )}
                </div>

                <div className="checkout-page__item-total">
                  {formatCurrency(itemTotal)}
                </div>
              </div>
            );
          })}
        </div>

        <div className="checkout-page__summary">
          <h2>Order summary</h2>

          <div className="checkout-page__summary-row">
            <span>Subtotal:</span>
            <span>{formatCurrency(subtotal)}</span>
          </div>

          {totalDiscount > 0 ? (
            <div className="checkout-page__summary-row checkout-page__summary-row--discount">
              <span>Sale event discount:</span>
              <span>-{formatCurrency(totalDiscount)}</span>
            </div>
          ) : null}

          <div className="checkout-page__summary-row checkout-page__summary-row--total">
            <span>Total:</span>
            <span>{formatCurrency(total)}</span>
          </div>

          <button
            type="button"
            className="checkout-page__place-order-btn"
            onClick={handlePlaceOrder}
            disabled={revalidating}
          >
            {revalidating ? "Processing..." : "Place order"}
          </button>
        </div>
      </div>

      {showChangesModal ? (
        <div className="checkout-page__modal-overlay" onClick={handleCloseChangesModal}>
          <div className="checkout-page__modal" onClick={(e) => e.stopPropagation()}>
            <header className="checkout-page__modal-header">
              <h2>Sale event changes detected</h2>
              <button
                type="button"
                className="checkout-page__modal-close"
                onClick={handleCloseChangesModal}
              >
                ×
              </button>
            </header>

            <div className="checkout-page__modal-content">
              <p>The following items have sale event changes:</p>
              <ul className="checkout-page__changes-list">
                {revalidationChanges.map((change, index) => (
                  <li key={index}>
                    <strong>{change.listingTitle}</strong>
                    <div className="checkout-page__change-reason">
                      {change.reason} ({change.saleEventName})
                    </div>
                  </li>
                ))}
              </ul>
              <p className="checkout-page__modal-note">
                The sale pricing has been removed from these items. Your order total has been updated.
              </p>
            </div>

            <footer className="checkout-page__modal-footer">
              <button
                type="button"
                className="btn-primary"
                onClick={handleCloseChangesModal}
              >
                Continue
              </button>
            </footer>
          </div>
        </div>
      ) : null}
    </div>
  );
};

export default CheckoutPage;
