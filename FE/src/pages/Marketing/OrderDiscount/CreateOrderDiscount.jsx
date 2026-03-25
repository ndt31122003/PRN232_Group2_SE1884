import { useState } from "react";
import { useNavigate } from "react-router-dom";
import instance from "../../../utils/axiosCustomize";
import { getCurrentUserId } from "../../../utils/jwtUtils";
import "./CreateOrderDiscount.scss";

const CreateOrderDiscount = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    thresholdType: "spend", // spend or quantity
    thresholdAmount: "",
    thresholdQuantity: "",
    discountValue: "",
    discountUnit: "percent", // percent or fixed
    maxDiscount: "",
    startDate: "",
    endDate: "",
    applyToAllItems: true,
    tiers: [],
    includedItems: [],
    excludedItems: [],
    includedCategories: [],
    excludedCategories: []
  });

  const [showTiers, setShowTiers] = useState(false);
  const [currentTier, setCurrentTier] = useState({ thresholdValue: "", discountValue: "" });

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value
    }));
  };

  const handleAddTier = () => {
    if (currentTier.thresholdValue && currentTier.discountValue) {
      setFormData(prev => ({
        ...prev,
        tiers: [...prev.tiers, { ...currentTier }]
      }));
      setCurrentTier({ thresholdValue: "", discountValue: "" });
    }
  };

  const handleRemoveTier = (index) => {
    setFormData(prev => ({
      ...prev,
      tiers: prev.tiers.filter((_, i) => i !== index)
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    
    // Prepare data for API
    const payload = {
      sellerId: getCurrentUserId() ?? '00000000-0000-0000-0000-000000000000',
      name: formData.name,
      description: formData.description || null,
      discountValue: parseFloat(formData.discountValue),
      discountUnit: formData.discountUnit === "percent" ? 1 : 2,
      maxDiscount: formData.maxDiscount ? parseFloat(formData.maxDiscount) : null,
      startDate: new Date(formData.startDate).toISOString(),
      endDate: new Date(formData.endDate).toISOString(),
      tiers: formData.tiers.length > 0 ? formData.tiers.map(t => ({
        thresholdValue: parseFloat(t.thresholdValue),
        discountValue: parseFloat(t.discountValue)
      })) : null,
      includedItemIds: formData.includedItems.length > 0 ? formData.includedItems : null,
      excludedItemIds: formData.excludedItems.length > 0 ? formData.excludedItems : null,
      includedCategoryIds: formData.includedCategories.length > 0 ? formData.includedCategories : null,
      excludedCategoryIds: formData.excludedCategories.length > 0 ? formData.excludedCategories : null
    };

    const endpoint = formData.thresholdType === "spend" 
      ? "/order-discounts/spend-based"
      : "/order-discounts/quantity-based";

    if (formData.thresholdType === "spend") {
      payload.thresholdAmount = parseFloat(formData.thresholdAmount);
    } else {
      payload.thresholdQuantity = parseInt(formData.thresholdQuantity);
    }

    try {
      const response = await instance.post(endpoint, payload);

      if (response.status === 200 || response.status === 201) {
        navigate("/marketing");
      } else {
        console.error("Failed to create discount");
      }
    } catch (error) {
      console.error("Error creating discount:", error);
    }
  };

  return (
    <div className="create-order-discount">
      <div className="create-order-discount__header">
        <h1>Create Order Discount</h1>
        <p>Set up automatic discounts based on order value or quantity</p>
      </div>

      <form onSubmit={handleSubmit} className="discount-form">
        {/* Basic Information */}
        <section className="form-section">
          <h2>Basic Information</h2>
          
          <div className="form-group">
            <label htmlFor="name">Discount Name *</label>
            <input
              type="text"
              id="name"
              name="name"
              value={formData.name}
              onChange={handleInputChange}
              required
              placeholder="e.g., Summer Sale 10% Off"
            />
          </div>

          <div className="form-group">
            <label htmlFor="description">Description</label>
            <textarea
              id="description"
              name="description"
              value={formData.description}
              onChange={handleInputChange}
              placeholder="Optional description for internal use"
              rows="3"
            />
          </div>
        </section>

        {/* Threshold Type */}
        <section className="form-section">
          <h2>Discount Trigger</h2>
          
          <div className="form-group">
            <label>Threshold Type *</label>
            <div className="radio-group">
              <label className="radio-label">
                <input
                  type="radio"
                  name="thresholdType"
                  value="spend"
                  checked={formData.thresholdType === "spend"}
                  onChange={handleInputChange}
                />
                <span>Spend-based (minimum order amount)</span>
              </label>
              <label className="radio-label">
                <input
                  type="radio"
                  name="thresholdType"
                  value="quantity"
                  checked={formData.thresholdType === "quantity"}
                  onChange={handleInputChange}
                />
                <span>Quantity-based (minimum item count)</span>
              </label>
            </div>
          </div>

          {formData.thresholdType === "spend" ? (
            <div className="form-group">
              <label htmlFor="thresholdAmount">Minimum Order Amount ($) *</label>
              <input
                type="number"
                id="thresholdAmount"
                name="thresholdAmount"
                value={formData.thresholdAmount}
                onChange={handleInputChange}
                required
                min="0"
                step="0.01"
                placeholder="50.00"
              />
            </div>
          ) : (
            <div className="form-group">
              <label htmlFor="thresholdQuantity">Minimum Item Quantity *</label>
              <input
                type="number"
                id="thresholdQuantity"
                name="thresholdQuantity"
                value={formData.thresholdQuantity}
                onChange={handleInputChange}
                required
                min="1"
                step="1"
                placeholder="3"
              />
            </div>
          )}
        </section>

        {/* Discount Value */}
        <section className="form-section">
          <h2>Discount Amount</h2>
          
          <div className="form-group">
            <label>Discount Type *</label>
            <div className="radio-group">
              <label className="radio-label">
                <input
                  type="radio"
                  name="discountUnit"
                  value="percent"
                  checked={formData.discountUnit === "percent"}
                  onChange={handleInputChange}
                />
                <span>Percentage (%)</span>
              </label>
              <label className="radio-label">
                <input
                  type="radio"
                  name="discountUnit"
                  value="fixed"
                  checked={formData.discountUnit === "fixed"}
                  onChange={handleInputChange}
                />
                <span>Fixed Amount ($)</span>
              </label>
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="discountValue">
              Discount Value {formData.discountUnit === "percent" ? "(%)" : "($)"} *
            </label>
            <input
              type="number"
              id="discountValue"
              name="discountValue"
              value={formData.discountValue}
              onChange={handleInputChange}
              required
              min="0.01"
              max={formData.discountUnit === "percent" ? "100" : undefined}
              step="0.01"
              placeholder={formData.discountUnit === "percent" ? "10" : "5.00"}
            />
          </div>

          <div className="form-group">
            <label htmlFor="maxDiscount">Maximum Discount Amount ($)</label>
            <input
              type="number"
              id="maxDiscount"
              name="maxDiscount"
              value={formData.maxDiscount}
              onChange={handleInputChange}
              min="0"
              step="0.01"
              placeholder="Optional cap on discount"
            />
            <small>Leave empty for no maximum limit</small>
          </div>
        </section>

        {/* Multi-tier Discounts */}
        <section className="form-section">
          <h2>Multi-tier Discounts (Optional)</h2>
          <p className="section-description">
            Offer increasing discounts for higher spending or quantities
          </p>
          
          <button
            type="button"
            className="btn-secondary"
            onClick={() => setShowTiers(!showTiers)}
          >
            {showTiers ? "Hide Tiers" : "Add Tiers"}
          </button>

          {showTiers && (
            <div className="tier-builder">
              <div className="tier-input-group">
                <input
                  type="number"
                  placeholder={formData.thresholdType === "spend" ? "Amount" : "Quantity"}
                  value={currentTier.thresholdValue}
                  onChange={(e) => setCurrentTier(prev => ({ ...prev, thresholdValue: e.target.value }))}
                  min="0"
                  step="0.01"
                />
                <input
                  type="number"
                  placeholder="Discount value"
                  value={currentTier.discountValue}
                  onChange={(e) => setCurrentTier(prev => ({ ...prev, discountValue: e.target.value }))}
                  min="0"
                  step="0.01"
                />
                <button type="button" onClick={handleAddTier} className="btn-primary">
                  Add Tier
                </button>
              </div>

              {formData.tiers.length > 0 && (
                <div className="tiers-list">
                  <h4>Configured Tiers:</h4>
                  {formData.tiers.map((tier, index) => (
                    <div key={index} className="tier-item">
                      <span>
                        {formData.thresholdType === "spend" ? "$" : ""}{tier.thresholdValue}
                        {formData.thresholdType === "quantity" ? " items" : ""} → 
                        {formData.discountUnit === "percent" ? " " : " $"}{tier.discountValue}
                        {formData.discountUnit === "percent" ? "%" : ""}
                      </span>
                      <button
                        type="button"
                        onClick={() => handleRemoveTier(index)}
                        className="btn-remove"
                      >
                        Remove
                      </button>
                    </div>
                  ))}
                </div>
              )}
            </div>
          )}
        </section>

        {/* Date Range */}
        <section className="form-section">
          <h2>Schedule</h2>
          
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="startDate">Start Date *</label>
              <input
                type="datetime-local"
                id="startDate"
                name="startDate"
                value={formData.startDate}
                onChange={handleInputChange}
                required
              />
            </div>

            <div className="form-group">
              <label htmlFor="endDate">End Date *</label>
              <input
                type="datetime-local"
                id="endDate"
                name="endDate"
                value={formData.endDate}
                onChange={handleInputChange}
                required
              />
            </div>
          </div>
        </section>

        {/* Form Actions */}
        <div className="form-actions">
          <button type="button" onClick={() => navigate("/marketing")} className="btn-secondary">
            Cancel
          </button>
          <button type="submit" className="btn-primary">
            Create Discount
          </button>
        </div>
      </form>
    </div>
  );
};

export default CreateOrderDiscount;
