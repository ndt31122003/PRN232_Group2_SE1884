import { useState, useEffect, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import instance from "../../../utils/axiosCustomize";
import { getCurrentUserId } from "../../../utils/jwtUtils";
import "./ShippingDiscountsSummary.scss";

const ShippingDiscountsSummary = () => {
  const navigate = useNavigate();
  const [discounts, setDiscounts] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchDiscounts = useCallback(async () => {
    try {
      setLoading(true);
      const sellerId = getCurrentUserId() ?? "00000000-0000-0000-0000-000000000000";
      const res = await instance.get(`/shipping-discounts/seller/${sellerId}`);
      setDiscounts(res.data ?? []);
    } catch {
      setDiscounts([]);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchDiscounts();
  }, [fetchDiscounts]);

  const handleToggle = async (discount) => {
    try {
      const endpoint = discount.isActive ? "deactivate" : "activate";
      await instance.post(`/shipping-discounts/${discount.id}/${endpoint}`);
      await fetchDiscounts();
    } catch (err) {
      alert("Failed to update status");
    }
  };

  const handleDelete = async (discount) => {
    if (!window.confirm(`Delete "${discount.name}"?`)) return;
    try {
      await instance.delete(`/shipping-discounts/${discount.id}`);
      await fetchDiscounts();
    } catch {
      alert("Failed to delete discount");
    }
  };

  const formatDate = (d) =>
    new Date(d).toLocaleDateString("en-US", { year: "numeric", month: "short", day: "numeric" });

  const getDiscountLabel = (d) => {
    if (d.isFreeShipping) return "Free shipping";
    return d.discountUnit === 1 ? `${d.discountValue}% off shipping` : `$${d.discountValue} off shipping`;
  };

  return (
    <div className="shipping-discounts-summary">
      <div className="shipping-discounts-summary__header">
        <div>
          <h1>Shipping discounts</h1>
          <p>Offer reduced or free shipping to encourage buyers to purchase more.</p>
        </div>
        <button
          className="btn-primary"
          onClick={() => navigate("/marketing/shipping-discounts/create")}
        >
          Create shipping discount
        </button>
      </div>

      {loading ? (
        <div className="shipping-discounts-summary__empty">Loading…</div>
      ) : discounts.length === 0 ? (
        <div className="shipping-discounts-summary__empty">
          <p>No shipping discounts yet.</p>
          <p>Create one to start offering shipping promotions to buyers.</p>
        </div>
      ) : (
        <div className="shipping-discounts-summary__table-wrapper">
          <table className="sd-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Discount</th>
                <th>Min. order</th>
                <th>Start date</th>
                <th>End date</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {discounts.map((d) => (
                <tr key={d.id}>
                  <td className="sd-table__name">{d.name}</td>
                  <td>{getDiscountLabel(d)}</td>
                  <td>{d.minimumOrderValue ? `$${d.minimumOrderValue}` : "—"}</td>
                  <td>{formatDate(d.startDate)}</td>
                  <td>{formatDate(d.endDate)}</td>
                  <td>
                    <span className={`sd-badge ${d.isActive ? "sd-badge--active" : "sd-badge--inactive"}`}>
                      {d.isActive ? "Active" : "Inactive"}
                    </span>
                  </td>
                  <td className="sd-table__actions">
                    <button className="btn-link" onClick={() => handleToggle(d)}>
                      {d.isActive ? "Deactivate" : "Activate"}
                    </button>
                    <button className="btn-link btn-link--danger" onClick={() => handleDelete(d)}>
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default ShippingDiscountsSummary;
