import { useState } from "react";
import { useNavigate } from "react-router-dom";
import instance from "../../../utils/axiosCustomize";
import { getCurrentUserId } from "../../../utils/jwtUtils";
import "./CreateVolumePricing.scss";

const emptyTier = () => ({ minQuantity: "", discountValue: "", discountUnit: "percent" });

const CreateVolumePricing = () => {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    name: "",
    description: "",
    appliesToAll: true,
    listingId: "",
    startDate: "",
    endDate: "",
  });
  const [tiers, setTiers] = useState([emptyTier()]);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");

  const handleFormChange = (e) => {
    const { name, value } = e.target;
    setForm((p) => ({ ...p, [name]: value }));
  };

  const handleTierChange = (index, field, value) => {
    setTiers((prev) => prev.map((t, i) => (i === index ? { ...t, [field]: value } : t)));
  };

  const addTier = () => setTiers((p) => [...p, emptyTier()]);

  const removeTier = (index) => {
    if (tiers.length <= 1) return;
    setTiers((p) => p.filter((_, i) => i !== index));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    // Validate tiers
    const tierRequests = tiers.map((t) => ({
      minQuantity: parseInt(t.minQuantity, 10),
      discountValue: parseFloat(t.discountValue),
      discountUnit: t.discountUnit,
    }));

    if (tierRequests.some((t) => !t.minQuantity || !t.discountValue || t.minQuantity <= 0 || t.discountValue <= 0)) {
      setError("All tiers must have valid minimum quantity and discount value.");
      return;
    }

    const sorted = [...tierRequests].sort((a, b) => a.minQuantity - b.minQuantity);
    const hasDupes = sorted.some((t, i) => i > 0 && t.minQuantity === sorted[i - 1].minQuantity);
    if (hasDupes) {
      setError("Tier minimum quantities must be unique.");
      return;
    }

    setSubmitting(true);
    try {
      const sellerId = getCurrentUserId() ?? "00000000-0000-0000-0000-000000000000";
      const payload = {
        sellerId,
        listingId: form.appliesToAll === "all" || !form.listingId ? null : form.listingId,
        name: form.name,
        description: form.description || null,
        startDate: new Date(form.startDate).toISOString(),
        endDate: new Date(form.endDate).toISOString(),
        tiers: tierRequests,
      };

      await instance.post("/volume-pricings", payload);
      navigate("/marketing/volume-pricing");
    } catch (err) {
      console.error("API error response:", err.response?.data);
      const apiError = err.response?.data?.detail || err.response?.data?.title || err.response?.data?.message || "Failed to create volume pricing. Please check your inputs.";
      setError(apiError);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="create-volume-pricing">
      <button className="btn-back-link" onClick={() => navigate("/marketing/volume-pricing")}>
        &lt; Back to promotions
      </button>

      <div className="create-volume-pricing__header">
        <h1>Create volume pricing</h1>
      </div>

      {error && <div className="form-error">{error}</div>}

      <form onSubmit={handleSubmit} className="discount-form">
        <section className="form-section">
          <h2>Basic Information</h2>
          <div className="form-group">
            <label htmlFor="name">Rule Name *</label>
            <input
              id="name" name="name" type="text" required
              value={form.name} onChange={handleFormChange}
              placeholder="e.g., Buy More Save More"
            />
          </div>
          <div className="form-group">
            <label htmlFor="description">Description</label>
            <textarea
              id="description" name="description" rows="3"
              value={form.description} onChange={handleFormChange}
              placeholder="Optional description"
            />
          </div>
        </section>

        <section className="form-section">
          <h2>Applies To</h2>
          <div className="radio-group">
            <label className="radio-label">
              <input
                type="radio" checked={form.appliesToAll === "all" || form.appliesToAll === true}
                onChange={() => setForm((p) => ({ ...p, appliesToAll: "all", listingId: "" }))}
              />
              <span>All listings</span>
            </label>
            <label className="radio-label">
              <input
                type="radio" checked={form.appliesToAll === "specific"}
                onChange={() => setForm((p) => ({ ...p, appliesToAll: "specific" }))}
              />
              <span>Specific listing</span>
            </label>
          </div>
          {form.appliesToAll === "specific" && (
            <div className="form-group">
              <label htmlFor="listingId">Listing ID *</label>
              <input
                id="listingId" name="listingId" type="text"
                value={form.listingId} onChange={handleFormChange}
                placeholder="Enter listing UUID"
              />
            </div>
          )}
        </section>

        <section className="form-section">
          <h2>Quantity Tiers</h2>
          <p className="section-note">Add tiers with increasing quantity thresholds and discount values.</p>

          <div className="tier-builder">
            <div className="tier-header">
              <span>Min quantity</span>
              <span>Discount value</span>
              <span>Type</span>
              <span></span>
            </div>

            {tiers.map((tier, i) => (
              <div key={i} className="tier-row">
                <div className="tier-prefix">
                  <span>Buy</span>
                  <input
                    type="number" min="1" placeholder="2"
                    value={tier.minQuantity}
                    onChange={(e) => handleTierChange(i, "minQuantity", e.target.value)}
                  />
                </div>
                <div className="tier-prefix">
                  <span style={{opacity: 0}}>.</span>
                  <input
                    type="number" min="0.01" step="0.01" placeholder="10"
                    value={tier.discountValue}
                    onChange={(e) => handleTierChange(i, "discountValue", e.target.value)}
                  />
                </div>
                <div className="tier-prefix" style={{marginTop: '25px'}}>
                  <select
                    value={tier.discountUnit}
                    onChange={(e) => handleTierChange(i, "discountUnit", e.target.value)}
                  >
                    <option value="percent">% off</option>
                    <option value="fixed">$ off</option>
                  </select>
                </div>
                <div style={{marginTop: '25px'}}>
                  <button
                    type="button" className="btn-remove"
                    onClick={() => removeTier(i)} disabled={tiers.length <= 1}
                  >
                    &times;
                  </button>
                </div>
              </div>
            ))}

            <button type="button" className="btn-add-tier" onClick={addTier}>
              + Add tier
            </button>
          </div>
        </section>

        <section className="form-section">
          <h2>Schedule</h2>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="startDate">Start Date *</label>
              <input
                id="startDate" name="startDate" type="datetime-local"
                required value={form.startDate} onChange={handleFormChange}
              />
            </div>
            <div className="form-group">
              <label htmlFor="endDate">End Date *</label>
              <input
                id="endDate" name="endDate" type="datetime-local"
                required value={form.endDate} onChange={handleFormChange}
              />
            </div>
          </div>
        </section>

        <div className="form-actions">
          <button type="button" className="btn-secondary" onClick={() => navigate("/marketing/volume-pricing")}>
            Cancel
          </button>
          <button type="submit" className="btn-primary" disabled={submitting}>
            {submitting ? "Saving…" : "Save"}
          </button>
        </div>
      </form>
    </div>
  );
};

export default CreateVolumePricing;
