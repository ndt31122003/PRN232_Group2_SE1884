import { useState, useEffect, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import instance from "../../../utils/axiosCustomize";
import { getCurrentUserId } from "../../../utils/jwtUtils";
import "./VolumePricingSummary.scss";

const VolumePricingSummary = () => {
  const navigate = useNavigate();
  const [pricings, setPricings] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchPricings = useCallback(async () => {
    try {
      setLoading(true);
      const sellerId = getCurrentUserId() ?? "00000000-0000-0000-0000-000000000000";
      const res = await instance.get(`/volume-pricings/seller/${sellerId}`);
      setPricings(res.data ?? []);
    } catch {
      setPricings([]);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { fetchPricings(); }, [fetchPricings]);

  const handleToggle = async (pricing) => {
    try {
      const endpoint = pricing.isActive ? "deactivate" : "activate";
      await instance.post(`/volume-pricings/${pricing.id}/${endpoint}`);
      await fetchPricings();
    } catch {
      alert("Failed to update status");
    }
  };

  const handleDelete = async (pricing) => {
    if (!window.confirm(`Delete "${pricing.name}"?`)) return;
    try {
      await instance.delete(`/volume-pricings/${pricing.id}`);
      await fetchPricings();
    } catch {
      alert("Failed to delete");
    }
  };

  const formatDate = (d) =>
    new Date(d).toLocaleDateString("en-US", { year: "numeric", month: "short", day: "numeric" });

  const getTiersSummary = (tiers) => {
    if (!tiers?.length) return "—";
    const first = tiers[0];
    const unit = first.discountUnit === 1 ? "%" : "$";
    const suffix = first.discountUnit === 1 ? "" : " off";
    return `${tiers.length} tier${tiers.length > 1 ? "s" : ""} (from ${unit}${first.discountValue}${suffix} @ ${first.minQuantity}+)`;
  };

  return (
    <div className="volume-pricing-summary">
      <div className="volume-pricing-summary__header">
        <div>
          <h1>Volume pricing</h1>
          <p>Offer automatic discounts when buyers purchase multiple quantities of an item.</p>
        </div>
        <button className="btn-primary" onClick={() => navigate("/marketing/volume-pricing/create")}>
          Create volume pricing
        </button>
      </div>

      {loading ? (
        <div className="volume-pricing-summary__empty">Loading…</div>
      ) : pricings.length === 0 ? (
        <div className="volume-pricing-summary__empty">
          <p>No volume pricing rules yet.</p>
          <p>Create one to reward buyers who purchase in bulk.</p>
        </div>
      ) : (
        <div className="volume-pricing-summary__table-wrapper">
          <table className="vp-table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Applies to</th>
                <th>Tiers</th>
                <th>Start date</th>
                <th>End date</th>
                <th>Status</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {pricings.map((p) => (
                <tr key={p.id}>
                  <td className="vp-table__name">
                    <div>{p.name}</div>
                    {p.description && <div className="vp-table__desc">{p.description}</div>}
                  </td>
                  <td>{p.listingId ? "Specific listing" : "All listings"}</td>
                  <td>{getTiersSummary(p.tiers)}</td>
                  <td>{formatDate(p.startDate)}</td>
                  <td>{formatDate(p.endDate)}</td>
                  <td>
                    <span className={`vp-badge ${p.isActive ? "vp-badge--active" : "vp-badge--inactive"}`}>
                      {p.isActive ? "Active" : "Inactive"}
                    </span>
                  </td>
                  <td className="vp-table__actions">
                    <button className="btn-link" onClick={() => handleToggle(p)}>
                      {p.isActive ? "Deactivate" : "Activate"}
                    </button>
                    <button className="btn-link btn-link--danger" onClick={() => handleDelete(p)}>
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

export default VolumePricingSummary;
