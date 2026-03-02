import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import StoreService from "../../services/StoreService";
import Notice from "../../components/Common/CustomNotification";
import "./CreateStorePage.scss";

const STORE_TYPES = [
    {
        type: 0, // Basic
        name: "Basic",
        monthlyFee: 0,
        finalValueFee: 12.9,
        listingLimit: 250,
        features: [
            "Free monthly subscription",
            "250 listings per month",
            "12.9% final value fee per sale"
        ]
    },
    {
        type: 1, // Premium
        name: "Premium",
        monthlyFee: 21.95,
        finalValueFee: 10.9,
        listingLimit: 1000,
        features: [
            "$21.95 monthly subscription",
            "1,000 listings per month",
            "10.9% final value fee per sale",
            "Priority customer support"
        ]
    },
    {
        type: 2, // Anchor
        name: "Anchor",
        monthlyFee: 299.95,
        finalValueFee: 9.9,
        listingLimit: "Unlimited",
        features: [
            "$299.95 monthly subscription",
            "Unlimited listings",
            "9.9% final value fee per sale",
            "Dedicated account manager",
            "Advanced analytics"
        ]
    }
];

const CreateStorePage = () => {
    const navigate = useNavigate();
    const [step, setStep] = useState(1);
    const [selectedPlan, setSelectedPlan] = useState(null);
    const [storeInfo, setStoreInfo] = useState({
        name: "",
        description: ""
    });
    const [logo, setLogo] = useState(null);
    const [banner, setBanner] = useState(null);
    const [loading, setLoading] = useState(false);

    const handlePlanSelect = (plan) => {
        setSelectedPlan(plan);
        setStep(2);
    };

    const handleStoreInfoChange = (field, value) => {
        setStoreInfo(prev => ({ ...prev, [field]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!selectedPlan || !storeInfo.name.trim()) {
            Notice("error", "Please fill in all required fields");
            return;
        }

        setLoading(true);
        try {
            const response = await StoreService.createStore({
                name: storeInfo.name,
                description: storeInfo.description || null,
                storeType: selectedPlan.type
            });

            if (response?.data?.StoreId || response?.data?.storeId) {
                Notice("success", "Store created successfully!");
                const id = response.data.StoreId || response.data.storeId;
                navigate(`/store/settings?storeId=${id}`);
            }
        } catch (error) {
            Notice("error", error?.response?.data?.detail || "Failed to create store");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="create-store-page">
            <div className="container">
                <div className="page-header">
                    <h1>Create Your Store</h1>
                    <p>Choose a subscription plan and set up your store profile</p>
                </div>

                {step === 1 && (
                    <div className="step-content">
                        <h2>Choose Your Subscription Plan</h2>
                        <div className="plans-grid">
                            {STORE_TYPES.map(plan => (
                                <div
                                    key={plan.type}
                                    className={`plan-card ${selectedPlan?.type === plan.type ? 'plan-card--selected' : ''}`}
                                    onClick={() => handlePlanSelect(plan)}
                                >
                                    <h3>{plan.name}</h3>
                                    <div className="plan-price">
                                        {plan.monthlyFee === 0 ? (
                                            <span className="price-main">Free</span>
                                        ) : (
                                            <>
                                                <span className="price-currency">$</span>
                                                <span className="price-main">{plan.monthlyFee}</span>
                                                <span className="price-period">/month</span>
                                            </>
                                        )}
                                    </div>
                                    <div className="plan-features">
                                        <ul>
                                            {plan.features.map((feature, idx) => (
                                                <li key={idx}>{feature}</li>
                                            ))}
                                        </ul>
                                    </div>
                                    <div className="plan-action">
                                        {selectedPlan?.type === plan.type ? (
                                            <span className="selected-badge">Selected</span>
                                        ) : (
                                            <span className="select-btn">Select Plan</span>
                                        )}
                                    </div>
                                </div>
                            ))}
                        </div>
                    </div>
                )}

                {step === 2 && (
                    <div className="step-content">
                        <button onClick={() => setStep(1)} className="back-btn">
                            ← Back to Plans
                        </button>

                        <h2>Store Information</h2>
                        <form onSubmit={handleSubmit}>
                            <div className="form-group">
                                <label htmlFor="store-name">Store Name *</label>
                                <input
                                    type="text"
                                    id="store-name"
                                    required
                                    maxLength={255}
                                    value={storeInfo.name}
                                    onChange={(e) => handleStoreInfoChange("name", e.target.value)}
                                    placeholder="Enter your store name"
                                />
                            </div>

                            <div className="form-group">
                                <label htmlFor="store-description">Description</label>
                                <textarea
                                    id="store-description"
                                    rows="5"
                                    maxLength={2000}
                                    value={storeInfo.description}
                                    onChange={(e) => handleStoreInfoChange("description", e.target.value)}
                                    placeholder="Describe your store..."
                                />
                            </div>

                            <div className="form-actions">
                                <button type="button" onClick={() => setStep(1)} className="btn-secondary">
                                    Back
                                </button>
                                <button type="submit" className="btn-primary" disabled={loading}>
                                    {loading ? "Creating..." : "Create Store"}
                                </button>
                            </div>
                        </form>
                    </div>
                )}
            </div>
        </div>
    );
};

export default CreateStorePage;

