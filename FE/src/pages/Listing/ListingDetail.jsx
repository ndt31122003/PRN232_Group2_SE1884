import React, { useCallback, useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import axios from "../../utils/axiosCustomize";
import Notice from "../../components/Common/CustomNotification";
import "./ListingDetail.scss";

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

const ListingDetail = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [loading, setLoading] = useState(true);
  const [listing, setListing] = useState(null);
  const [saleEvents, setSaleEvents] = useState([]);

  const loadListing = useCallback(async () => {
    try {
      const response = await axios.get(`listings/${id}`, { suppressErrorNotice: true });
      setListing(response?.data ?? null);
    } catch (error) {
      Notice({
        msg: "Could not load listing.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    }
  }, [id]);

  const loadActiveSaleEvents = useCallback(async () => {
    try {
      const response = await axios.get(`sale-events/listing/${id}/active`, { suppressErrorNotice: true });
      const events = response?.data ?? [];
      setSaleEvents(Array.isArray(events) ? events : []);
    } catch (error) {
      console.error("Could not load sale events:", error);
      setSaleEvents([]);
    }
  }, [id]);

  useEffect(() => {
    const load = async () => {
      setLoading(true);
      await Promise.all([loadListing(), loadActiveSaleEvents()]);
      setLoading(false);
    };

    load();
  }, [loadListing, loadActiveSaleEvents]);

  const activeSaleEvent = saleEvents.length > 0 ? saleEvents[0] : null;

  const getAppliedTier = () => {
    if (!activeSaleEvent || !listing) {
      return null;
    }

    const tiers = activeSaleEvent.discountTiers ?? [];
    return tiers.length > 0 ? tiers[0] : null;
  };

  const appliedTier = getAppliedTier();

  const originalPrice = listing?.currentPrice ?? listing?.price ?? 0;
  const salePrice = activeSaleEvent && activeSaleEvent.mode === SaleEventMode.DiscountAndSaleEvent
    ? calculateSalePrice(originalPrice, appliedTier)
    : originalPrice;

  const showSaleEndDate = activeSaleEvent && activeSaleEvent.endDate
    ? dayjs(activeSaleEvent.endDate).diff(dayjs(), "day") <= 7
    : false;

  if (loading) {
    return (
      <div className="listing-detail">
        <div className="listing-detail__loading">Loading listing details...</div>
      </div>
    );
  }

  if (!listing) {
    return (
      <div className="listing-detail">
        <div className="listing-detail__error">Listing not found.</div>
      </div>
    );
  }

  return (
    <div className="listing-detail">
      <header className="listing-detail__header">
        <button
          type="button"
          className="listing-detail__back"
          onClick={() => navigate(-1)}
        >
          ← Back
        </button>
      </header>

      <div className="listing-detail__content">
        <div className="listing-detail__image">
          {listing.thumbnailUrl || listing.thumbnail ? (
            <img src={listing.thumbnailUrl || listing.thumbnail} alt={listing.title} />
          ) : (
            <div className="listing-detail__image-placeholder">No image</div>
          )}
        </div>

        <div className="listing-detail__info">
          <h1 className="listing-detail__title">{listing.title}</h1>

          {activeSaleEvent ? (
            <div className="listing-detail__sale-section">
              <div className="listing-detail__sale-badge">
                {activeSaleEvent.buyerMessageLabel || "Sale Event"}
              </div>

              <div className="listing-detail__pricing">
                {activeSaleEvent.mode === SaleEventMode.DiscountAndSaleEvent ? (
                  <>
                    <div className="listing-detail__price-original">
                      {formatCurrency(originalPrice)}
                    </div>
                    <div className="listing-detail__price-sale">
                      {formatCurrency(salePrice)}
                    </div>
                  </>
                ) : (
                  <div className="listing-detail__price-original">
                    {formatCurrency(originalPrice)}
                  </div>
                )}
              </div>

              {showSaleEndDate ? (
                <div className="listing-detail__sale-end">
                  Sale ends {dayjs(activeSaleEvent.endDate).format("MMM D, YYYY")}
                </div>
              ) : null}

              {activeSaleEvent.offerFreeShipping ? (
                <div className="listing-detail__free-shipping">
                  Free shipping
                </div>
              ) : null}
            </div>
          ) : (
            <div className="listing-detail__pricing">
              <div className="listing-detail__price">
                {formatCurrency(originalPrice)}
              </div>
            </div>
          )}

          <div className="listing-detail__description">
            <h2>Description</h2>
            <p>{listing.description || "No description available."}</p>
          </div>

          {listing.sku ? (
            <div className="listing-detail__meta">
              <strong>SKU:</strong> {listing.sku}
            </div>
          ) : null}

          {listing.availableQuantity !== undefined && listing.availableQuantity !== null ? (
            <div className="listing-detail__meta">
              <strong>Available:</strong> {listing.availableQuantity} {listing.availableQuantity === 1 ? "item" : "items"}
            </div>
          ) : null}
        </div>
      </div>
    </div>
  );
};

export default ListingDetail;
