import React, { useEffect, useMemo, useState } from "react";
import dayjs from "dayjs";
import { useNavigate } from "react-router-dom";
import OrderService from "../../services/OrderService";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import Notice from "../../components/Common/CustomNotification";
import "./ShippingLabelsPage.scss";

const DEFAULT_FILTERS = {
    fromDate: "",
    toDate: "",
    limit: 50,
};

const LIMIT_OPTIONS = [25, 50, 100, 200];

const formatCurrency = (amount, currency) => {
    if (amount === null || amount === undefined) {
        return "-";
    }

    try {
        return new Intl.NumberFormat("en-US", {
            style: "currency",
            currency: currency || "USD",
        }).format(amount);
    } catch (error) {
        return `${amount.toFixed(2)} ${currency || "USD"}`;
    }
};

const formatDateTime = (value) => {
    if (!value) {
        return "-";
    }
    const parsed = dayjs(value);
    return parsed.isValid() ? parsed.format("MMM D, YYYY h:mm A") : "-";
};

const buildAbsoluteUrl = (path) => {
    if (!path) {
        return "";
    }
    if (/^https?:/i.test(path)) {
        return path;
    }
    const baseUrl = OrderService?.defaultBaseUrl ?? window.location.origin;
    try {
        const normalisedBase = baseUrl.replace(/\/$/, "");
        return new URL(path, `${normalisedBase}/`).toString();
    } catch (error) {
        console.warn("Unable to build label URL", error);
        return path;
    }
};

const ShippingLabelsPage = () => {
    const navigate = useNavigate();
    const [filters, setFilters] = useState(DEFAULT_FILTERS);
    const [formState, setFormState] = useState(DEFAULT_FILTERS);
    const [labels, setLabels] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [voidingLabelId, setVoidingLabelId] = useState(null);

    const requestParams = useMemo(() => {
        const params = { limit: filters.limit };
        if (filters.fromDate) {
            params.fromDate = dayjs(filters.fromDate).startOf("day").toISOString();
        }
        if (filters.toDate) {
            params.toDate = dayjs(filters.toDate).endOf("day").toISOString();
        }
        return params;
    }, [filters]);

    useEffect(() => {
        let ignore = false;
        const controller = new AbortController();

        const fetchLabels = async () => {
            setLoading(true);
            setError("");
            try {
                const response = await OrderService.getShippingLabels(requestParams, controller.signal);
                if (ignore) {
                    return;
                }
                setLabels(response?.data ?? []);
            } catch (err) {
                if (controller.signal.aborted) {
                    return;
                }
                console.error("Failed to load shipping labels", err);
                const message = err?.response?.data?.detail ?? err?.message ?? "Unable to load shipping labels.";
                setError(message);
                setLabels([]);
            } finally {
                if (!ignore) {
                    setLoading(false);
                }
            }
        };

        fetchLabels();

        return () => {
            ignore = true;
            controller.abort();
        };
    }, [requestParams]);

    const handleFormChange = (field) => (event) => {
        const value = field === "limit" ? Number(event.target.value) : event.target.value;
        setFormState((prev) => ({ ...prev, [field]: value }));
    };

    const applyFilters = (event) => {
        event.preventDefault();
        if (formState.fromDate && formState.toDate) {
            if (dayjs(formState.fromDate).isAfter(dayjs(formState.toDate))) {
                setError("Start date must be before or equal to end date.");
                return;
            }
        }
        setFilters({ ...formState });
    };

    const resetFilters = () => {
        setFormState(DEFAULT_FILTERS);
        setFilters(DEFAULT_FILTERS);
        setError("");
    };

    const handleReprint = (label) => {
        if (!label?.labelUrl) {
            Notice({
                msg: "Label unavailable",
                desc: "This label does not have a downloadable URL.",
                isSuccess: false,
                place: "bottomRight",
            });
            return;
        }
        const url = buildAbsoluteUrl(label.labelUrl);
        window.open(url, "_blank", "noopener,noreferrer");
    };

    const handlePrintAnother = (label) => {
        if (!label?.orderId) {
            return;
        }
        navigate(`/order/ship/${label.orderId}`, {
            state: {
                from: "/order/shipping-labels",
            },
        });
    };

    const handleVoid = async (label) => {
        if (!label || label.isVoided) {
            return;
        }

        const reason = window.prompt("Provide a reason to void this label (optional):", "");
        if (reason === null) {
            return;
        }

        setVoidingLabelId(label.labelId);
        try {
            const response = await OrderService.voidShippingLabel(
                label.orderId,
                label.labelId,
                { reason: reason || undefined }
            );
            const updated = response?.data;
            setLabels((prev) =>
                prev.map((item) => (item.labelId === label.labelId ? { ...item, ...updated } : item))
            );
            Notice({
                msg: "Label voided",
                desc: "The shipping label has been voided successfully.",
                isSuccess: true,
                place: "bottomRight",
            });
        } catch (err) {
            console.error("Failed to void label", err);
            const message = err?.response?.data?.detail ?? err?.message ?? "Unable to void this label.";
            Notice({
                msg: "Void failed",
                desc: message,
                isSuccess: false,
                place: "bottomRight",
            });
        } finally {
            setVoidingLabelId(null);
        }
    };

    const getVoidButtonLabel = (label) => {
        if (label.isVoided) {
            return "Voided";
        }
        if (voidingLabelId === label.labelId) {
            return "Voiding...";
        }
        return "Void label";
    };

    const renderTableBody = () => {
        if (loading) {
            return (
                <tr>
                    <td colSpan={7} className="shipping-labels-table__loading">
                        <LoadingScreen isLoadingTable={true} />
                        <span>Loading shipping labels…</span>
                    </td>
                </tr>
            );
        }

        if (!labels.length) {
            return (
                <tr>
                    <td colSpan={7} className="shipping-labels-table__empty">
                        You haven\'t purchased any labels that match these filters yet.
                    </td>
                </tr>
            );
        }

        return labels.map((label) => {
            const costLabel = formatCurrency(label?.cost?.amount ?? label?.costAmount, label?.cost?.currency ?? label?.costCurrency);
            const printedAt = formatDateTime(label.purchasedAt);
            const voidedAt = formatDateTime(label.voidedAt);
            const orderIdDisplay = label?.orderId ? String(label.orderId).slice(0, 8) : null;
            const orderIdText = orderIdDisplay ? `#${orderIdDisplay}…` : "ID unavailable";

            return (
                <tr
                    key={label.labelId}
                    className={label.isVoided ? "shipping-labels-table__row shipping-labels-table__row--voided" : "shipping-labels-table__row"}
                >
                    <td data-label="Order">
                        <div className="shipping-labels-table__order">
                            <span className="shipping-labels-table__order-number">Order {label.orderNumber}</span>
                            <span className="shipping-labels-table__order-id">{orderIdText}</span>
                        </div>
                    </td>
                    <td data-label="Carrier & service">
                        <div className="shipping-labels-table__service">
                            <span>{label.carrier}</span>
                            <span className="shipping-labels-table__service-name">{label.serviceName}</span>
                        </div>
                    </td>
                    <td data-label="Tracking">
                        {label.trackingNumber ? (
                            <a
                                href={`https://www.google.com/search?q=${encodeURIComponent(label.trackingNumber)}`}
                                target="_blank"
                                rel="noopener noreferrer"
                                className="shipping-labels-table__tracking"
                            >
                                {label.trackingNumber}
                            </a>
                        ) : (
                            <span>-</span>
                        )}
                    </td>
                    <td data-label="Cost">{costLabel}</td>
                    <td data-label="Printed">{printedAt}</td>
                    <td data-label="Status">
                        {label.isVoided ? (
                            <span className="shipping-labels-table__status shipping-labels-table__status--voided">
                                Voided{voidedAt ? ` · ${voidedAt}` : ""}
                            </span>
                        ) : (
                            <span className="shipping-labels-table__status shipping-labels-table__status--active">
                                Active
                            </span>
                        )}
                    </td>
                    <td data-label="Actions">
                        <div className="shipping-labels-table__actions">
                            <button
                                type="button"
                                className="shipping-labels-page__link"
                                onClick={() => handleReprint(label)}
                            >
                                Reprint
                            </button>
                            <button
                                type="button"
                                className="shipping-labels-page__link"
                                onClick={() => handlePrintAnother(label)}
                            >
                                Print another label
                            </button>
                            <button
                                type="button"
                                className="shipping-labels-page__link shipping-labels-page__link--danger"
                                onClick={() => handleVoid(label)}
                                disabled={label.isVoided || voidingLabelId === label.labelId}
                            >
                                {getVoidButtonLabel(label)}
                            </button>
                        </div>
                        {label.isVoided && label.voidReason && (
                            <div className="shipping-labels-table__void-reason">Reason: {label.voidReason}</div>
                        )}
                    </td>
                </tr>
            );
        });
    };

    return (
        <div className="shipping-labels-page">
            <header className="shipping-labels-page__header">
                <div>
                    <h1 className="shipping-labels-page__title">Shipping labels</h1>
                    <p className="shipping-labels-page__subtitle">
                        Review and manage the labels you\'ve purchased for your orders.
                    </p>
                </div>
                <button
                    type="button"
                    className="shipping-labels-page__primary"
                    onClick={() => navigate("/order/all?status=all")}
                >
                    View all orders
                </button>
            </header>

            <section className="shipping-labels-page__filters">
                <form className="shipping-labels-page__filter-form" onSubmit={applyFilters}>
                    <div className="shipping-labels-page__filter-group">
                        <label htmlFor="shipping-labels-from">From</label>
                        <input
                            id="shipping-labels-from"
                            type="date"
                            value={formState.fromDate}
                            onChange={handleFormChange("fromDate")}
                        />
                    </div>
                    <div className="shipping-labels-page__filter-group">
                        <label htmlFor="shipping-labels-to">To</label>
                        <input
                            id="shipping-labels-to"
                            type="date"
                            value={formState.toDate}
                            onChange={handleFormChange("toDate")}
                        />
                    </div>
                    <div className="shipping-labels-page__filter-group">
                        <label htmlFor="shipping-labels-limit">Show</label>
                        <select
                            id="shipping-labels-limit"
                            value={formState.limit}
                            onChange={handleFormChange("limit")}
                        >
                            {LIMIT_OPTIONS.map((option) => (
                                <option key={option} value={option}>
                                    {option}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className="shipping-labels-page__filter-actions">
                        <button type="submit" className="shipping-labels-page__primary" disabled={loading}>
                            Apply
                        </button>
                        <button type="button" className="shipping-labels-page__secondary" onClick={resetFilters}>
                            Reset
                        </button>
                    </div>
                </form>
            </section>

            {error && <div className="shipping-labels-page__error">{error}</div>}

            <section className="shipping-labels-page__table-section">
                <table className="shipping-labels-table">
                    <thead>
                        <tr>
                            <th>Order</th>
                            <th>Carrier &amp; service</th>
                            <th>Tracking</th>
                            <th>Cost</th>
                            <th>Printed</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        {renderTableBody()}
                    </tbody>
                </table>
            </section>
        </div>
    );
};

export default ShippingLabelsPage;
