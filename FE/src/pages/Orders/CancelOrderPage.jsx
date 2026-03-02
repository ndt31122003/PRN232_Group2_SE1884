import React, { useEffect, useMemo, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import OrderService from "../../services/OrderService";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import "./CancelOrderPage.scss";

const PRICE_FORMATTER = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
});

const DATE_FORMATTER = new Intl.DateTimeFormat("en-US", {
    month: "short",
    day: "numeric",
    year: "numeric",
});

const CANCELLATION_REASONS = [
    { value: 4, label: "Out of stock or damaged" },
    { value: 0, label: "Buyer asked to cancel" },
    { value: 3, label: "Issue with buyer's shipping address" },
    { value: 1, label: "Buyer changed their mind" },
    { value: 6, label: "Duplicate order" },
    { value: 5, label: "Pricing error" },
    { value: 2, label: "Buyer hasn't paid" },
    { value: 7, label: "Suspected fraud" },
    { value: 99, label: "Other" },
];

const normalizeOrder = (order) => ({
    id: order?.id ?? order?.Id ?? "",
    orderNumber: order?.orderNumber ?? order?.OrderNumber ?? "",
    buyerFullName: order?.buyerFullName ?? order?.BuyerFullName ?? "",
    buyerUsername: order?.buyerUsername ?? order?.BuyerUsername ?? "",
    items: order?.items ?? order?.Items ?? [],
    subTotal: order?.subTotal ?? order?.SubTotal ?? null,
    shippingCost: order?.shippingCost ?? order?.ShippingCost ?? null,
    taxAmount: order?.taxAmount ?? order?.TaxAmount ?? null,
    total: order?.total ?? order?.Total ?? null,
    orderedAt: order?.orderedAt ?? order?.OrderedAt ?? null,
});

const formatCurrency = (value) => {
    if (value === null || value === undefined) {
        return PRICE_FORMATTER.format(0);
    }
    return PRICE_FORMATTER.format(value);
};

const formatDate = (value) => {
    if (!value) {
        return "-";
    }
    try {
        const date = value instanceof Date ? value : new Date(value);
        if (Number.isNaN(date.getTime())) {
            return "-";
        }
        return DATE_FORMATTER.format(date);
    } catch (error) {
        console.warn("Unable to format date", error);
        return "-";
    }
};

const primaryItemFromOrder = (order) => {
    if (!order?.items?.length) {
        return null;
    }
    return order.items[0];
};

const CancelOrderPage = () => {
    const navigate = useNavigate();
    const location = useLocation();
    const { orderId } = useParams();

    const fromPath = location.state?.from ?? "/order/all?status=all";

    const [order, setOrder] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [reason, setReason] = useState("");
    const [note, setNote] = useState("");
    const [submitting, setSubmitting] = useState(false);

    useEffect(() => {
        if (!orderId) {
            setError("Invalid order selection.");
            setLoading(false);
            return;
        }

        let ignore = false;
        const controller = new AbortController();

        const fetchOrder = async () => {
            try {
                setLoading(true);
                setError("");
                const response = await OrderService.getOrderById(orderId, controller.signal);
                if (ignore) {
                    return;
                }
                const data = response?.data ?? response;
                setOrder(normalizeOrder(data));
            } catch (err) {
                if (controller.signal.aborted) {
                    return;
                }
                console.error("Failed to load order", err);
                setError("Unable to load order details. Return to orders and try again.");
            } finally {
                if (!ignore) {
                    setLoading(false);
                }
            }
        };

        fetchOrder();

        return () => {
            ignore = true;
            controller.abort();
        };
    }, [orderId]);

    const item = useMemo(() => primaryItemFromOrder(order), [order]);

    const refundTotal = useMemo(() => order?.total ?? 0, [order]);

    const handleBack = () => {
        if (fromPath) {
            navigate(fromPath, { replace: true });
        } else {
            navigate("/order/all?status=all", { replace: true });
        }
    };

    const handleSubmit = async (event) => {
        event.preventDefault();
        if (!reason) {
            setError("Select a reason for cancelling the order.");
            return;
        }
        if (!orderId) {
            setError("Invalid order selection.");
            return;
        }

        try {
            setSubmitting(true);
            setError("");
            await OrderService.createCancellationRequest(
                orderId,
                {
                    reason: Number(reason),
                    sellerNote: note.trim() ? note.trim() : null,
                }
            );
            navigate("/order/all?status=cancellations", { replace: true });
        } catch (err) {
            console.error("Failed to submit cancellation", err);
            const message =
                err?.response?.data?.detail ??
                err?.response?.data?.message ??
                err?.message ??
                "Unable to submit cancellation request. Please try again.";
            setError(message);
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <div className="cancel-order-page">
            <header className="cancel-order-page__header">
                <button type="button" className="cancel-order-page__back" onClick={handleBack}>
                    ← Back to orders
                </button>
                <h1 className="cancel-order-page__title">Cancellation</h1>
            </header>

            {loading ? (
                <div className="cancel-order-page__loading">
                    <LoadingScreen isOverlay={false} isLoadingTable={true} />
                    <span>Loading order details…</span>
                </div>
            ) : error && !order ? (
                <div className="cancel-order-page__error">{error}</div>
            ) : (
                <div className="cancel-order-page__content">
                    <form className="cancel-order-page__form" onSubmit={handleSubmit}>
                        <section className="cancel-card">
                            <h2 className="cancel-card__title">What's your reason for cancelling?</h2>
                            <label className="cancel-card__label" htmlFor="cancellation-reason">
                                Reason
                            </label>
                            <div className="cancel-card__select-wrapper">
                                <select
                                    id="cancellation-reason"
                                    className="cancel-card__select"
                                    value={reason}
                                    onChange={(event) => setReason(event.target.value)}
                                    required
                                >
                                    <option value="" disabled>
                                        Select
                                    </option>
                                    {CANCELLATION_REASONS.map((option) => (
                                        <option key={option.value} value={option.value}>
                                            {option.label}
                                        </option>
                                    ))}
                                </select>
                                <span className="cancel-card__select-icon" aria-hidden="true">▾</span>
                            </div>

                            <label className="cancel-card__label" htmlFor="cancellation-note">
                                Message to buyer (optional)
                            </label>
                            <textarea
                                id="cancellation-note"
                                className="cancel-card__textarea"
                                placeholder="Let the buyer know what's happening."
                                value={note}
                                onChange={(event) => setNote(event.target.value)}
                                rows={4}
                            />

                            {error && order && (
                                <div className="cancel-card__inline-error">{error}</div>
                            )}

                            <div className="cancel-card__actions">
                                <button
                                    type="submit"
                                    className="cancel-card__submit"
                                    disabled={submitting}
                                >
                                    {submitting ? "Submitting…" : "Submit"}
                                </button>
                                <button
                                    type="button"
                                    className="cancel-card__secondary"
                                    onClick={handleBack}
                                    disabled={submitting}
                                >
                                    Keep order
                                </button>
                            </div>
                        </section>
                    </form>

                    <aside className="cancel-summary">
                        <div className="cancel-summary__card">
                            <h2 className="cancel-summary__title">Order summary</h2>
                            <dl className="cancel-summary__meta">
                                <div className="cancel-summary__meta-row">
                                    <dt>Order number</dt>
                                    <dd>{order?.orderNumber || "-"}</dd>
                                </div>
                                <div className="cancel-summary__meta-row">
                                    <dt>Order total</dt>
                                    <dd>{formatCurrency(order?.total)}</dd>
                                </div>
                                <div className="cancel-summary__meta-row">
                                    <dt>Purchase date</dt>
                                    <dd>{formatDate(order?.orderedAt)}</dd>
                                </div>
                                <div className="cancel-summary__meta-row">
                                    <dt>Buyer username</dt>
                                    <dd>{order?.buyerUsername || "-"}</dd>
                                </div>
                            </dl>

                            {item && (
                                <div className="cancel-summary__item">
                                    <div className="cancel-summary__item-title">{item?.title ?? item?.Title ?? "Item"}</div>
                                    <div className="cancel-summary__item-qty">
                                        Qty {item?.quantity ?? item?.Quantity ?? 1}
                                    </div>
                                    <div className="cancel-summary__item-price">
                                        {formatCurrency(item?.price ?? item?.Price ?? order?.subTotal)}
                                    </div>
                                </div>
                            )}
                        </div>

                        <div className="cancel-summary__card">
                            <h2 className="cancel-summary__title">Total refund to buyer</h2>
                            <dl className="cancel-summary__meta cancel-summary__meta--compact">
                                <div className="cancel-summary__meta-row">
                                    <dt>Purchase price</dt>
                                    <dd>{formatCurrency(order?.subTotal)}</dd>
                                </div>
                                <div className="cancel-summary__meta-row">
                                    <dt>Original shipping</dt>
                                    <dd>{formatCurrency(order?.shippingCost)}</dd>
                                </div>
                                <div className="cancel-summary__meta-row">
                                    <dt>Tax</dt>
                                    <dd>{formatCurrency(order?.taxAmount)}</dd>
                                </div>
                                <div className="cancel-summary__meta-row cancel-summary__meta-row--total">
                                    <dt>Refund total</dt>
                                    <dd>{formatCurrency(refundTotal)}</dd>
                                </div>
                            </dl>
                            <p className="cancel-summary__note">
                                We will deduct the total refund from your available payout balance. If your balance is insufficient, you authorize us to charge the saved payout method on file.
                            </p>
                        </div>
                    </aside>
                </div>
            )}
        </div>
    );
};

export default CancelOrderPage;
