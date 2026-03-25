import { useState } from "react";
import { useNavigate } from "react-router-dom";
import instance from "../../../utils/axiosCustomize";
import { getCurrentUserId } from "../../../utils/jwtUtils";
import "./CreateShippingDiscount.scss";

const CreateShippingDiscount = () => {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    name: "",
    description: "",
    isFreeShipping: true,
    discountValue: "",
    discountUnit: "percent",
    minimumOrderValue: "",
    startDate: "",
    endDate: "",
  });
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm((prev) => ({ ...prev, [name]: type === "checkbox" ? checked : value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSubmitting(true);

    try {
      const sellerId = getCurrentUserId() ?? "00000000-0000-0000-0000-000000000000";
      const payload = {
        sellerId,
        name: form.name,
        description: form.description || null,
        isFreeShipping: form.isFreeShipping,
        discountValue: form.isFreeShipping ? 100 : parseFloat(form.discountValue),
        discountUnit: form.isFreeShipping ? 1 : (form.discountUnit === "percent" ? 1 : 2),
        minimumOrderValue: form.minimumOrderValue ? parseFloat(form.minimumOrderValue) : null,
        startDate: new Date(form.startDate).toISOString(),
        endDate: new Date(form.endDate).toISOString(),
      };

      await instance.post("/shipping-discounts", payload);
      navigate("/marketing/shipping-discounts");
    } catch (err) {
      console.error("API error response:", err.response?.data);
      const apiError = err.response?.data?.detail || err.response?.data?.title || err.response?.data?.message || "Failed to create shipping discount. Please check your inputs.";
      setError(apiError);
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="create-shipping-discount">
      <button className="btn-back-link" onClick={() => navigate("/marketing/shipping-discounts")}>
        &lt; Back to promotions
      </button>

      <div className="create-shipping-discount__header">
        <h1>Create shipping discount</h1>
      </div>

      {error && <div className="form-error">{error}</div>}

      <form onSubmit={handleSubmit} className="discount-form">
        <section className="form-section">
          <h2>Basic Information</h2>
          <div className="form-group">
            <label htmlFor="name">Discount Name *</label>
            <input
              id="name" name="name" type="text" required
              value={form.name} onChange={handleChange}
              placeholder="e.g., Free Shipping on $50+"
            />
          </div>
          <div className="form-group">
            <label htmlFor="description">Description</label>
            <textarea
              id="description" name="description" rows="3"
              value={form.description} onChange={handleChange}
              placeholder="Optional internal description"
            />
          </div>
        </section>

        <section className="form-section">
          <h2>Discount Type</h2>
          <div className="radio-group">
            <label className="radio-label">
              <input
                type="radio" name="isFreeShipping"
                checked={form.isFreeShipping}
                onChange={() => setForm((p) => ({ ...p, isFreeShipping: true }))}
              />
              <span>Free shipping</span>
            </label>
            <label className="radio-label">
              <input
                type="radio" name="isFreeShipping"
                checked={!form.isFreeShipping}
                onChange={() => setForm((p) => ({ ...p, isFreeShipping: false }))}
              />
              <span>Discount on shipping cost</span>
            </label>
          </div>

          {!form.isFreeShipping && (
            <div className="shipping-inputs-container">
              <div className="form-group">
                <label htmlFor="discountValue">Discount Value *</label>
                <input
                  id="discountValue" name="discountValue" type="number" required
                  value={form.discountValue} onChange={handleChange}
                  min="0.01" step="0.01"
                  placeholder={form.discountUnit === "percent" ? "10" : "5.00"}
                />
              </div>
              <div className="form-group">
                <label htmlFor="discountUnit">Unit</label>
                <select id="discountUnit" name="discountUnit" value={form.discountUnit} onChange={handleChange}>
                  <option value="percent">% off</option>
                  <option value="fixed">$ off</option>
                </select>
              </div>
            </div>
          )}

          <div className="form-group">
            <label htmlFor="minimumOrderValue">Minimum Order Value ($)</label>
            <input
              id="minimumOrderValue" name="minimumOrderValue" type="number"
              value={form.minimumOrderValue} onChange={handleChange}
              min="0" step="0.01" placeholder="Optional — e.g. 50.00"
            />
            <small>Leave blank to apply to all orders</small>
          </div>
        </section>

        <section className="form-section">
          <h2>Schedule</h2>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="startDate">Start Date *</label>
              <input
                id="startDate" name="startDate" type="datetime-local"
                required value={form.startDate} onChange={handleChange}
              />
            </div>
            <div className="form-group">
              <label htmlFor="endDate">End Date *</label>
              <input
                id="endDate" name="endDate" type="datetime-local"
                required value={form.endDate} onChange={handleChange}
              />
            </div>
          </div>
        </section>

        <div className="form-actions">
          <button type="button" className="btn-secondary" onClick={() => navigate("/marketing/shipping-discounts")}>
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

export default CreateShippingDiscount;
