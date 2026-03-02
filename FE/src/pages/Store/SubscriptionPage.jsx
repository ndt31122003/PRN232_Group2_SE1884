import React, { useState, useEffect } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import StoreService from "../../services/StoreService";
import Notice from "../../components/Common/CustomNotification";
import "./SubscriptionPage.scss";

const PLAN_INFO = {
    0: { name: "Basic", color: "#6c757d" },
    1: { name: "Premium", color: "#007bff" },
    2: { name: "Anchor", color: "#28a745" }
};

const SubscriptionPage = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const storeId = searchParams.get("storeId");
    
    const [loading, setLoading] = useState(true);
    const [storeData, setStoreData] = useState(null);

    useEffect(() => {
        if (storeId) {
            fetchStoreData();
        } else {
            setLoading(false);
            Notice("error", "No store selected. Please select a store first.");
            navigate("/stores");
        }
    }, [storeId, navigate]);

    const fetchStoreData = async () => {
        try {
            const response = await StoreService.getStoreById(storeId);
            if (response?.data) {
                setStoreData(response.data);
            }
        } catch (error) {
            Notice("error", "Failed to load store data");
        } finally {
            setLoading(false);
        }
    };

    if (loading) {
        return <div className="loading">Loading...</div>;
    }

    if (!storeData) {
        return (
            <div className="subscription-page">
                <div className="container">
                    <div className="error-message">
                        <p>Failed to load store subscription information</p>
                    </div>
                </div>
            </div>
        );
    }

    // Support both PascalCase and camelCase
    const rawSubscription = storeData.ActiveSubscription || storeData.activeSubscription;
    const subscription = rawSubscription ? {
        monthlyFee: rawSubscription.MonthlyFee ?? rawSubscription.monthlyFee ?? 0,
        currency: rawSubscription.Currency ?? rawSubscription.currency ?? "USD",
        finalValueFeePercentage: rawSubscription.FinalValueFeePercentage ?? rawSubscription.finalValueFeePercentage ?? 0,
        listingLimit: rawSubscription.ListingLimit ?? rawSubscription.listingLimit ?? 0,
        startDate: rawSubscription.StartDate ?? rawSubscription.startDate,
        endDate: rawSubscription.EndDate ?? rawSubscription.endDate,
        status: rawSubscription.Status ?? rawSubscription.status
    } : null;
    
    const storeType = storeData.StoreType ?? storeData.storeType ?? 0;
    const planInfo = PLAN_INFO[storeType];

    return (
        <div className="subscription-page">
            <div className="container">
                <div className="page-header">
                    <h1>Subscription & Fees</h1>
                    <p>Manage your store subscription and view fees</p>
                </div>

                <div className="current-plan">
                    <div className="plan-card-large" style={{ borderColor: planInfo.color }}>
                        <div className="plan-badge" style={{ background: planInfo.color }}>
                            {planInfo.name}
                        </div>
                        <h2>Current Subscription</h2>
                        
                        <div className="plan-details">
                            <div className="detail-item">
                                <span className="label">Monthly Fee:</span>
                                <span className="value">
                                    {subscription.monthlyFee === 0 
                                        ? "Free" 
                                        : `$${subscription.monthlyFee.toFixed(2)}`
                                    }
                                </span>
                            </div>
                            
                            <div className="detail-item">
                                <span className="label">Final Value Fee:</span>
                                <span className="value">{subscription.finalValueFeePercentage}%</span>
                            </div>
                            
                            <div className="detail-item">
                                <span className="label">Listing Limit:</span>
                                <span className="value">
                                    {subscription.listingLimit === 2147483647 
                                        ? "Unlimited" 
                                        : subscription.listingLimit.toLocaleString()
                                    }
                                </span>
                            </div>
                        </div>

                        <div className="subscription-dates">
                            <div className="date-item">
                                <span className="label">Start Date:</span>
                                <span className="value">
                                    {new Date(subscription.startDate).toLocaleDateString()}
                                </span>
                            </div>
                            
                            {subscription.endDate && (
                                <div className="date-item">
                                    <span className="label">End Date:</span>
                                    <span className="value">
                                        {new Date(subscription.endDate).toLocaleDateString()}
                                    </span>
                                </div>
                            )}
                        </div>
                    </div>
                </div>

                <div className="upgrade-section">
                    <h2>Upgrade Your Plan</h2>
                    <p className="info-text">Upgrade or downgrade your subscription plan anytime</p>
                    <button className="btn-secondary" disabled>
                        Manage Subscription (Coming Soon)
                    </button>
                </div>

                <div className="fees-info">
                    <h2>Understanding Your Fees</h2>
                    <div className="info-grid">
                        <div className="info-card">
                            <h3>Monthly Subscription</h3>
                            <p>Flat fee charged monthly based on your plan</p>
                            <div className="fee-amount">
                                {subscription.monthlyFee === 0 
                                    ? "Free" 
                                    : `$${subscription.monthlyFee}/month`
                                }
                            </div>
                        </div>
                        
                        <div className="info-card">
                            <h3>Final Value Fee</h3>
                            <p>Percentage charged on each successful sale</p>
                            <div className="fee-amount">
                                {subscription.finalValueFeePercentage}%
                            </div>
                        </div>
                        
                        <div className="info-card">
                            <h3>Listing Limit</h3>
                            <p>Maximum listings you can create per month</p>
                            <div className="fee-amount">
                                {subscription.listingLimit === 2147483647 
                                    ? "Unlimited" 
                                    : `${subscription.listingLimit.toLocaleString()} listings`
                                }
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default SubscriptionPage;

