import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import Notice from "../../components/Common/CustomNotification";
import CouponService from "../../services/CouponService";
import "./CouponsSummary.scss";

const currencyFormatter = new Intl.NumberFormat("en-US", {
  style: "currency",
  currency: "USD",
  minimumFractionDigits: 2
});

const formatAmount = (value) => {
  if (value === null || value === undefined) {
    return "--";
  }

  return currencyFormatter.format(Number(value));
};

const formatPercent = (value) => `${Number(value).toFixed(0)}%`;

const formatDate = (value) => {
  if (!value) {
    return "--";
  }

  const date = new Date(value);
  if (Number.isNaN(date.getTime())) {
    return "--";
  }

  return date.toLocaleDateString("en-GB", {
    day: "2-digit",
    month: "short",
    year: "numeric"
  });
};

const resolveDiscountLabel = (coupon) => {
  if (coupon.discountUnit === 1) {
    return formatPercent(coupon.discountValue);
  }

  return formatAmount(coupon.discountValue);
};

const CouponRow = ({ coupon, typeCatalogMap, onToggleStatus, onDelete, busy }) => {
  const typeKey = coupon.couponTypeId ? String(coupon.couponTypeId).toLowerCase() : null;
  const type = typeKey ? typeCatalogMap.get(typeKey) : undefined;
  const discountLabel = resolveDiscountLabel(coupon);
  const validity = `${formatDate(coupon.startDate)} – ${formatDate(coupon.endDate)}`;
  const maxDiscount = coupon.maxDiscount ? formatAmount(coupon.maxDiscount) : "--";

  return (
    <tr className={coupon.isActive ? "coupon-row" : "coupon-row is-inactive"}>
      <td>
        <div className="coupon-name">{coupon.name}</div>
        <div className="coupon-code">Code: {coupon.code}</div>
      </td>
      <td>
        <div className="coupon-type">{type?.label ?? "Custom coupon"}</div>
        <div className="coupon-meta">{discountLabel}</div>
      </td>
      <td>{validity}</td>
      <td>{maxDiscount}</td>
      <td className="coupon-status-cell">
        <label className="coupon-switch">
          <input
            type="checkbox"
            checked={coupon.isActive}
            onChange={() => onToggleStatus(coupon)}
            disabled={busy}
          />
          <span className="slider" />
        </label>
      </td>
      <td className="coupon-cta-cell">
        <button
          type="button"
          className="btn-link"
          onClick={() => onDelete(coupon)}
          disabled={busy}
        >
          Delete
        </button>
      </td>
    </tr>
  );
};

const CouponsSummaryPage = () => {
  const navigate = useNavigate();
  const [coupons, setCoupons] = useState([]);
  const [loading, setLoading] = useState(true);
  const [busyIds, setBusyIds] = useState(new Set());

  const typeCatalogMap = useMemo(() => {
    const entries = CouponService.listTypes().map((item) => [item.id.toLowerCase(), item]);
    return new Map(entries);
  }, []);

  const fetchCoupons = useCallback(async () => {
    setLoading(true);
    try {
      const data = await CouponService.getSellerCoupons();
      setCoupons(Array.isArray(data) ? data : []);
    } catch (error) {
      Notice({ msg: "Không thể tải danh sách coupon.", isSuccess: false });
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchCoupons();
  }, [fetchCoupons]);

  const setBusy = useCallback((couponId, isBusy) => {
    setBusyIds((prev) => {
      const next = new Set(prev);
      if (isBusy) {
        next.add(couponId);
      } else {
        next.delete(couponId);
      }
      return next;
    });
  }, []);

  const handleToggleStatus = useCallback(async (coupon) => {
    const nextState = !coupon.isActive;
    setBusy(coupon.id, true);
    try {
      await CouponService.updateCouponStatus(coupon.id, nextState);
      setCoupons((prev) =>
        prev.map((item) =>
          item.id === coupon.id
            ? {
                ...item,
                isActive: nextState,
                updatedAt: new Date().toISOString()
              }
            : item
        )
      );
      Notice({
        msg: nextState ? "Coupon activated." : "Coupon paused.",
        isSuccess: true
      });
    } catch (error) {
      Notice({ msg: "Không thể cập nhật trạng thái coupon.", isSuccess: false });
    } finally {
      setBusy(coupon.id, false);
    }
  }, [setBusy]);

  const handleDelete = useCallback(async (coupon) => {
    const confirmDelete = window.confirm(`Delete coupon ${coupon.code}?`);
    if (!confirmDelete) {
      return;
    }

    setBusy(coupon.id, true);
    try {
      await CouponService.deleteCoupon(coupon.id);
      setCoupons((prev) => prev.filter((item) => item.id !== coupon.id));
      Notice({ msg: "Coupon deleted.", isSuccess: true });
    } catch (error) {
      Notice({ msg: "Không thể xoá coupon.", isSuccess: false });
    } finally {
      setBusy(coupon.id, false);
    }
  }, [setBusy]);

  const hasCoupons = coupons.length > 0;

  return (
    <div className="coupon-summary">
      <div className="coupon-summary__header">
        <div>
          <h1>Coupons</h1>
          <p>Manage live promotions, pause offers, or clean up expired coupons.</p>
        </div>
        <div className="coupon-summary__actions">
          <button type="button" className="btn-secondary" onClick={fetchCoupons} disabled={loading}>
            Refresh
          </button>
          <button
            type="button"
            className="btn-primary"
            onClick={() => navigate("/marketing/coupons/create")}
          >
            Create coupon
          </button>
        </div>
      </div>

      {loading ? (
        <div className="coupon-summary__empty">Loading coupons…</div>
      ) : hasCoupons ? (
        <div className="coupon-summary__table-wrapper">
          <table className="coupon-summary__table">
            <thead>
              <tr>
                <th>Coupon</th>
                <th>Offer</th>
                <th>Validity</th>
                <th>Max discount</th>
                <th>Status</th>
                <th />
              </tr>
            </thead>
            <tbody>
              {coupons.map((coupon) => (
                <CouponRow
                  key={coupon.id}
                  coupon={coupon}
                  typeCatalogMap={typeCatalogMap}
                  onToggleStatus={handleToggleStatus}
                  onDelete={handleDelete}
                  busy={busyIds.has(coupon.id)}
                />
              ))}
            </tbody>
          </table>
        </div>
      ) : (
        <div className="coupon-summary__empty">
          <p>No coupons yet. Launch your first offer to excite buyers.</p>
          <button
            type="button"
            className="btn-primary"
            onClick={() => navigate("/marketing/coupons/create")}
          >
            Create coupon
          </button>
        </div>
      )}
    </div>
  );
};

export default CouponsSummaryPage;
