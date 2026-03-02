import React, { useState, useEffect, useCallback } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import PolicyService from "../../services/PolicyService";
import Notice from "../../components/Common/CustomNotification";
import "./PoliciesPage.scss";

const PoliciesPage = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const storeId = searchParams.get("storeId");
    
    const [activeTab, setActiveTab] = useState("shipping");
    const [shippingPolicies, setShippingPolicies] = useState([]);
    const [loading, setLoading] = useState(false);
    const [loadingPolicies, setLoadingPolicies] = useState(true);
    const [editingPolicy, setEditingPolicy] = useState(null);

    const [shippingForm, setShippingForm] = useState({
        carrier: "",
        serviceName: "",
        costAmount: "",
        currency: "USD",
        handlingTimeDays: 1,
        isDefault: false
    });

    // Return Policy State
    const [returnPolicy, setReturnPolicy] = useState(null);
    const [loadingReturnPolicy, setLoadingReturnPolicy] = useState(true);
    const [returnForm, setReturnForm] = useState({
        acceptReturns: true,
        returnPeriodDays: "30",
        refundMethod: "0", // MoneyBack
        returnShippingPaidBy: "0" // Buyer
    });

    const handleShippingSubmit = async (e) => {
        e.preventDefault();

        // If editing, use update handler
        if (editingPolicy) {
            await handleUpdatePolicy(e);
            return;
        }

        if (!storeId) {
            Notice("error", "Store ID is required");
            return;
        }

        setLoading(true);
        try {
            const requestData = {
                Carrier: shippingForm.carrier,
                ServiceName: shippingForm.serviceName,
                CostAmount: parseFloat(shippingForm.costAmount),
                Currency: shippingForm.currency,
                HandlingTimeDays: parseInt(shippingForm.handlingTimeDays),
                IsDefault: shippingForm.isDefault
            };
            
            const response = await PolicyService.createShippingPolicy(storeId, requestData);

            if (response?.data) {
                Notice("success", "Shipping policy created successfully!");
                setShippingForm({
                    carrier: "",
                    serviceName: "",
                    costAmount: "",
                    currency: "USD",
                    handlingTimeDays: 1,
                    isDefault: false
                });
                // Refresh policies list
                await fetchShippingPolicies();
            }
        } catch (error) {
            Notice("error", error?.response?.data?.detail || "Failed to create shipping policy");
        } finally {
            setLoading(false);
        }
    };

    const handleShippingChange = (field, value) => {
        setShippingForm(prev => ({ ...prev, [field]: value }));
    };

    const handleEditPolicy = (policy) => {
        setEditingPolicy(policy);
        setShippingForm({
            carrier: policy.Carrier || policy.carrier || "",
            serviceName: policy.ServiceName || policy.serviceName || "",
            costAmount: policy.CostAmount || policy.costAmount || "",
            currency: policy.Currency || policy.currency || "USD",
            handlingTimeDays: policy.HandlingTimeDays || policy.handlingTimeDays || 1,
            isDefault: policy.IsDefault || policy.isDefault || false
        });
    };

    const handleCancelEdit = () => {
        setEditingPolicy(null);
        setShippingForm({
            carrier: "",
            serviceName: "",
            costAmount: "",
            currency: "USD",
            handlingTimeDays: 1,
            isDefault: false
        });
    };

    const handleUpdatePolicy = async (e) => {
        e.preventDefault();

        if (!storeId || !editingPolicy) {
            Notice("error", "Store ID and policy ID are required");
            return;
        }

        setLoading(true);
        try {
            const policyId = editingPolicy.Id || editingPolicy.id;
            const requestData = {
                Carrier: shippingForm.carrier,
                ServiceName: shippingForm.serviceName,
                CostAmount: parseFloat(shippingForm.costAmount),
                Currency: shippingForm.currency,
                HandlingTimeDays: parseInt(shippingForm.handlingTimeDays),
                IsDefault: shippingForm.isDefault
            };
            
            const response = await PolicyService.updateShippingPolicy(storeId, policyId, requestData);

            if (response?.data || response?.status === 200) {
                Notice("success", "Shipping policy updated successfully!");
                handleCancelEdit();
                await fetchShippingPolicies();
            }
        } catch (error) {
            Notice("error", error?.response?.data?.detail || "Failed to update shipping policy");
        } finally {
            setLoading(false);
        }
    };

    const handleDeletePolicy = async (policyId) => {
        if (!window.confirm("Are you sure you want to delete this policy?")) {
            return;
        }

        if (!storeId) {
            Notice("error", "Store ID is required");
            return;
        }

        setLoading(true);
        try {
            const response = await PolicyService.deleteShippingPolicy(storeId, policyId);

            if (response?.data || response?.status === 200) {
                Notice("success", "Shipping policy deleted successfully!");
                await fetchShippingPolicies();
            }
        } catch (error) {
            Notice("error", error?.response?.data?.detail || "Failed to delete shipping policy");
        } finally {
            setLoading(false);
        }
    };

    // Return Policy Handlers
    const handleReturnChange = (field, value) => {
        setReturnForm(prev => ({ ...prev, [field]: value }));
    };

    const handleReturnSubmit = async (e) => {
        e.preventDefault();

        if (!storeId) {
            Notice("error", "Store ID is required");
            return;
        }

        setLoading(true);
        try {
            const requestData = {
                AcceptReturns: returnForm.acceptReturns,
                ReturnPeriodDays: returnForm.acceptReturns ? parseInt(returnForm.returnPeriodDays) : null,
                RefundMethod: returnForm.acceptReturns ? parseInt(returnForm.refundMethod) : null,
                ReturnShippingPaidBy: returnForm.acceptReturns ? parseInt(returnForm.returnShippingPaidBy) : null
            };
            
            let response;
            if (returnPolicy) {
                // Update existing policy
                response = await PolicyService.updateReturnPolicy(storeId, requestData);
            } else {
                // Create new policy
                response = await PolicyService.createReturnPolicy(storeId, requestData);
            }

            if (response?.data || response?.status === 200) {
                Notice("success", `Return policy ${returnPolicy ? "updated" : "created"} successfully!`);
                await fetchReturnPolicy();
            }
        } catch (error) {
            Notice("error", error?.response?.data?.detail || `Failed to ${returnPolicy ? "update" : "create"} return policy`);
        } finally {
            setLoading(false);
        }
    };

    const handleDeleteReturnPolicy = async () => {
        if (!window.confirm("Are you sure you want to delete this return policy?")) {
            return;
        }

        if (!storeId) {
            Notice("error", "Store ID is required");
            return;
        }

        setLoading(true);
        try {
            const response = await PolicyService.deleteReturnPolicy(storeId);

            if (response?.data || response?.status === 200) {
                Notice("success", "Return policy deleted successfully!");
                setReturnPolicy(null);
                setReturnForm({
                    acceptReturns: true,
                    returnPeriodDays: "30",
                    refundMethod: "0",
                    returnShippingPaidBy: "0"
                });
            }
        } catch (error) {
            Notice("error", error?.response?.data?.detail || "Failed to delete return policy");
        } finally {
            setLoading(false);
        }
    };

    const fetchShippingPolicies = useCallback(async () => {
        if (!storeId) return;
        
        setLoadingPolicies(true);
        try {
            const response = await PolicyService.getShippingPolicies(storeId);
            if (response?.data) {
                setShippingPolicies(response.data);
            }
        } catch (error) {
            console.error("Failed to fetch shipping policies:", error);
        } finally {
            setLoadingPolicies(false);
        }
    }, [storeId]);

    const fetchReturnPolicy = useCallback(async () => {
        if (!storeId) return;
        
        setLoadingReturnPolicy(true);
        try {
            const response = await PolicyService.getReturnPolicy(storeId);
            if (response?.status === 204 || !response?.data) {
                // No return policy exists yet
                setReturnPolicy(null);
                setReturnForm({
                    acceptReturns: true,
                    returnPeriodDays: "30",
                    refundMethod: "0",
                    returnShippingPaidBy: "0"
                });
            } else if (response?.data) {
                const data = response.data;
                setReturnPolicy(data);
                setReturnForm({
                    acceptReturns: data.AcceptReturns || data.acceptReturns,
                    returnPeriodDays: data.ReturnPeriodDays !== undefined ? data.ReturnPeriodDays.toString() : data.returnPeriodDays?.toString() || "30",
                    refundMethod: data.RefundMethod !== undefined ? data.RefundMethod.toString() : data.refundMethod?.toString() || "0",
                    returnShippingPaidBy: data.ReturnShippingPaidBy !== undefined ? data.ReturnShippingPaidBy.toString() : data.returnShippingPaidBy?.toString() || "0"
                });
            }
        } catch (error) {
            console.error("Failed to fetch return policy:", error);
        } finally {
            setLoadingReturnPolicy(false);
        }
    }, [storeId]);

    useEffect(() => {
        if (!storeId) {
            Notice("error", "No store selected. Please select a store first.");
            navigate("/stores");
        } else {
            fetchShippingPolicies();
            fetchReturnPolicy();
        }
    }, [storeId, navigate, fetchShippingPolicies, fetchReturnPolicy]);

    if (!storeId) {
        return (
            <div className="policies-page">
                <div className="container">
                    <div className="error-message">
                        <p>Store ID is required. Please go back and select a store.</p>
                    </div>
                </div>
            </div>
        );
    }

    return (
        <div className="policies-page">
            <div className="container">
                <div className="page-header">
                    <h1>Store Policies</h1>
                    <p>Manage shipping and return policies for your store</p>
                </div>

                <div className="tabs">
                    <button
                        className={`tab ${activeTab === "shipping" ? "tab--active" : ""}`}
                        onClick={() => setActiveTab("shipping")}
                    >
                        Shipping Policies
                    </button>
                    <button
                        className={`tab ${activeTab === "return" ? "tab--active" : ""}`}
                        onClick={() => setActiveTab("return")}
                    >
                        Return Policies
                    </button>
                </div>

                {activeTab === "shipping" && (
                    <div className="tab-content">
                        <div className="form-section">
                            <h2>{editingPolicy ? "Edit Shipping Policy" : "Add Shipping Policy"}</h2>
                            <form onSubmit={handleShippingSubmit} className="policy-form">
                                <div className="form-row">
                                    <div className="form-group">
                                        <label htmlFor="carrier">Carrier *</label>
                                        <select
                                            id="carrier"
                                            required
                                            value={shippingForm.carrier}
                                            onChange={(e) => handleShippingChange("carrier", e.target.value)}
                                        >
                                            <option value="">Select carrier</option>
                                            <option value="eBay">eBay</option>
                                            <option value="FedEx">FedEx</option>
                                            <option value="UPS">UPS</option>
                                            <option value="USPS">USPS</option>
                                        </select>
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="service-name">Service Name *</label>
                                        <input
                                            type="text"
                                            id="service-name"
                                            required
                                            maxLength={100}
                                            value={shippingForm.serviceName}
                                            onChange={(e) => handleShippingChange("serviceName", e.target.value)}
                                            placeholder="e.g. Priority Mail"
                                        />
                                    </div>
                                </div>

                                <div className="form-row">
                                    <div className="form-group">
                                        <label htmlFor="cost-amount">Cost *</label>
                                        <input
                                            type="number"
                                            id="cost-amount"
                                            step="0.01"
                                            min="0"
                                            required
                                            value={shippingForm.costAmount}
                                            onChange={(e) => handleShippingChange("costAmount", e.target.value)}
                                            placeholder="0.00"
                                        />
                                    </div>

                                    <div className="form-group">
                                        <label htmlFor="currency">Currency *</label>
                                        <select
                                            id="currency"
                                            required
                                            value={shippingForm.currency}
                                            onChange={(e) => handleShippingChange("currency", e.target.value)}
                                        >
                                            <option value="USD">USD</option>
                                            <option value="EUR">EUR</option>
                                            <option value="GBP">GBP</option>
                                        </select>
                                    </div>
                                </div>

                                <div className="form-group">
                                    <label htmlFor="handling-time">Handling Time (Days) *</label>
                                    <input
                                        type="number"
                                        id="handling-time"
                                        min="0"
                                        max="30"
                                        required
                                        value={shippingForm.handlingTimeDays}
                                        onChange={(e) => handleShippingChange("handlingTimeDays", e.target.value)}
                                    />
                                    <small>Number of business days to process order</small>
                                </div>

                                <div className="form-group">
                                    <label className="checkbox-label">
                                        <input
                                            type="checkbox"
                                            checked={shippingForm.isDefault}
                                            onChange={(e) => handleShippingChange("isDefault", e.target.checked)}
                                        />
                                        <span>Set as default shipping policy</span>
                                    </label>
                                </div>

                                <div className="form-actions">
                                    <button type="submit" className="btn-primary" disabled={loading}>
                                        {loading ? (editingPolicy ? "Updating..." : "Creating...") : (editingPolicy ? "Update Policy" : "Add Policy")}
                                    </button>
                                    {editingPolicy && (
                                        <button type="button" className="btn-secondary" onClick={handleCancelEdit} disabled={loading}>
                                            Cancel
                                        </button>
                                    )}
                                </div>
                            </form>
                        </div>

                        <div className="policies-list">
                            <h2>Existing Policies</h2>
                            {shippingPolicies.length === 0 ? (
                                <p className="empty-message">No shipping policies yet</p>
                            ) : (
                                <ul>
                                    {shippingPolicies.map(policy => {
                                        const policyId = policy.Id || policy.id;
                                        return (
                                            <li key={policyId} className="policy-item">
                                                <div className="policy-info">
                                                    <div className="policy-title">
                                                        <strong>{policy.Carrier || policy.carrier} - {policy.ServiceName || policy.serviceName}</strong>
                                                        {policy.IsDefault || policy.isDefault ? <span className="badge">Default</span> : null}
                                                    </div>
                                                    <span className="policy-cost">
                                                        ${policy.CostAmount || policy.costAmount} {policy.Currency || policy.currency}
                                                    </span>
                                                </div>
                                                <div className="policy-actions">
                                                    <button
                                                        type="button"
                                                        className="btn-edit"
                                                        onClick={() => handleEditPolicy(policy)}
                                                        disabled={loading}
                                                    >
                                                        Edit
                                                    </button>
                                                    <button
                                                        type="button"
                                                        className="btn-delete"
                                                        onClick={() => handleDeletePolicy(policyId)}
                                                        disabled={loading}
                                                    >
                                                        Delete
                                                    </button>
                                                </div>
                                            </li>
                                        );
                                    })}
                                </ul>
                            )}
                        </div>
                    </div>
                )}

                {activeTab === "return" && (
                    <div className="tab-content">
                        {loadingReturnPolicy ? (
                            <div className="loading">Loading...</div>
                        ) : (
                            <>
                                <div className="form-section">
                                    <h2>{returnPolicy ? "Edit Return Policy" : "Create Return Policy"}</h2>
                                    <form onSubmit={handleReturnSubmit} className="policy-form">
                                        <div className="form-group">
                                            <label className="checkbox-label">
                                                <input
                                                    type="checkbox"
                                                    checked={returnForm.acceptReturns}
                                                    onChange={(e) => handleReturnChange("acceptReturns", e.target.checked)}
                                                />
                                                <span>Accept returns</span>
                                            </label>
                                        </div>

                                        {returnForm.acceptReturns && (
                                            <>
                                                <div className="form-group">
                                                    <label htmlFor="return-period">Return Period *</label>
                                                    <select
                                                        id="return-period"
                                                        required
                                                        value={returnForm.returnPeriodDays}
                                                        onChange={(e) => handleReturnChange("returnPeriodDays", e.target.value)}
                                                    >
                                                        <option value="30">30 days</option>
                                                        <option value="60">60 days</option>
                                                    </select>
                                                </div>

                                                <div className="form-group">
                                                    <label htmlFor="refund-method">Refund Method *</label>
                                                    <select
                                                        id="refund-method"
                                                        required
                                                        value={returnForm.refundMethod}
                                                        onChange={(e) => handleReturnChange("refundMethod", e.target.value)}
                                                    >
                                                        <option value="0">Money Back</option>
                                                        <option value="1">Exchange</option>
                                                        <option value="2">Store Credit</option>
                                                    </select>
                                                </div>

                                                <div className="form-group">
                                                    <label htmlFor="return-shipping-paid-by">Return Shipping Paid By *</label>
                                                    <select
                                                        id="return-shipping-paid-by"
                                                        required
                                                        value={returnForm.returnShippingPaidBy}
                                                        onChange={(e) => handleReturnChange("returnShippingPaidBy", e.target.value)}
                                                    >
                                                        <option value="0">Buyer</option>
                                                        <option value="1">Seller</option>
                                                    </select>
                                                </div>
                                            </>
                                        )}

                                        <div className="form-actions">
                                            <button type="submit" className="btn-primary" disabled={loading}>
                                                {loading ? "Saving..." : (returnPolicy ? "Update Policy" : "Create Policy")}
                                            </button>
                                            {returnPolicy && (
                                                <button type="button" className="btn-delete" onClick={handleDeleteReturnPolicy} disabled={loading}>
                                                    Delete Policy
                                                </button>
                                            )}
                                        </div>
                                    </form>
                                </div>

                                {returnPolicy && (
                                    <div className="policies-list">
                                        <h2>Current Policy</h2>
                                        <div className="policy-item">
                                            <div className="policy-info">
                                                <div className="policy-title">
                                                    <strong>{returnPolicy.AcceptReturns || returnPolicy.acceptReturns ? "Accepts Returns" : "Does Not Accept Returns"}</strong>
                                                </div>
                                                {(returnPolicy.AcceptReturns || returnPolicy.acceptReturns) && (
                                                    <div className="policy-details" style={{ marginTop: "0.5rem", fontSize: "0.875rem", color: "#6b7280" }}>
                                                        <div>Period: {(returnPolicy.ReturnPeriodDays || returnPolicy.returnPeriodDays) === 30 ? "30 days" : "60 days"}</div>
                                                        <div>Refund: {
                                                            (returnPolicy.RefundMethod ?? returnPolicy.refundMethod) === 0 ? "Money Back" :
                                                            (returnPolicy.RefundMethod ?? returnPolicy.refundMethod) === 1 ? "Exchange" : "Store Credit"
                                                        }</div>
                                                        <div>Shipping: {(returnPolicy.ReturnShippingPaidBy ?? returnPolicy.returnShippingPaidBy) === 0 ? "Buyer pays" : "Seller pays"}</div>
                                                    </div>
                                                )}
                                            </div>
                                        </div>
                                    </div>
                                )}
                            </>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
};

export default PoliciesPage;
