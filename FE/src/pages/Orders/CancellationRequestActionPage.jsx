import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import OrderService from "../../services/OrderService";
import "./ResolutionCenterPage.scss";

const formatCurrency = (amount, currency) => {
    if (amount == null || !currency) {
        return null;
    }

    try {
        return new Intl.NumberFormat(undefined, {
            style: "currency",
            currency,
            maximumFractionDigits: 2,
        }).format(amount);
    } catch (error) {
        return `${amount} ${currency}`;
    }
};

const formatCurrencyLabel = (amount, currency) => formatCurrency(amount, currency) ?? (amount != null && currency ? `${amount} ${currency}` : null);

const formatBuyer = (displayName, username) => {
    if (!displayName && !username) {
        return "-";
    }

    if (!username || displayName === username) {
        return displayName ?? username;
    }

    return `${displayName} (${username})`;
};

const formatDateTime = (isoValue) => {
    if (!isoValue) {
        return null;
    }

    try {
        return new Date(isoValue).toLocaleString();
    } catch (error) {
        return null;
    }
};

const extractErrorMessage = (err, fallback) => {
    if (!err) {
        return fallback;
    }

    const response = err.response?.data;
    if (typeof response === "string") {
        return response;
    }

    return response?.title || response?.detail || fallback;
};

const normalizeOrder = (order) => {
    if (!order) {
        return null;
    }

    const itemsSource = order.items ?? order.Items ?? [];

    return {
        id: order.id ?? order.Id ?? "",
        orderNumber: order.orderNumber ?? order.OrderNumber ?? "",
        items: itemsSource.map((item) => ({
            title: item?.title ?? item?.Title ?? "",
            quantity: item?.quantity ?? item?.Quantity ?? 1,
            price: item?.price ?? item?.Price ?? null,
        })),
        totalAmount: order.total?.amount ?? order.Total?.Amount ?? order.total ?? order.Total ?? null,
        totalCurrency: order.total?.currency ?? order.Total?.Currency ?? order.currency ?? order.Currency ?? "USD",
        paidAt: order.paidAt ?? order.PaidAt ?? order.paidAtUtc ?? order.PaidAtUtc ?? null,
        paymentStatus: order.paymentStatus ?? order.PaymentStatus ?? null,
    };
};

const CancellationRequestActionPage = () => {
    const { requestId, mode } = useParams();
    const normalizedMode = mode === "decline" ? "decline" : "approve";
    const navigate = useNavigate();

    const [record, setRecord] = useState(null);
    const [order, setOrder] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [loadError, setLoadError] = useState(null);
    const [formInitialized, setFormInitialized] = useState(false);
    const [form, setForm] = useState({ note: "" });
    const [submitError, setSubmitError] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    useEffect(() => {
        if (mode !== "approve" && mode !== "decline") {
            navigate("/order/cancellations", { replace: true });
        }
    }, [mode, navigate]);

    useEffect(() => {
        if (!requestId) {
            setLoadError("Cancellation request not found.");
            setIsLoading(false);
            return;
        }

        const controller = new AbortController();
        const load = async () => {
            setIsLoading(true);
            setLoadError(null);

            try {
                const [requestResponse, orderResponse] = await Promise.all([
                    OrderService.getCancellationRequestById(requestId, controller.signal),
                    OrderService.getOrderByCancellationRequestId(requestId, controller.signal)
                ]);

                const payload = requestResponse?.data ?? requestResponse;
                const orderPayload = orderResponse?.data ?? orderResponse;

                if (!payload) {
                    setLoadError("Cancellation request not found.");
                    return;
                }

                setRecord(payload);
                setOrder(orderPayload ? normalizeOrder(orderPayload) : null);
                setFormInitialized(false);
            } catch (fetchError) {
                if (controller.signal.aborted) {
                    return;
                }

                setLoadError(extractErrorMessage(fetchError, "Unable to load this cancellation request."));
            } finally {
                if (!controller.signal.aborted) {
                    setIsLoading(false);
                }
            }
        };

        load();

        return () => controller.abort();
    }, [requestId]);

    useEffect(() => {
        if (formInitialized || !record) {
            return;
        }

        setForm({ note: record.sellerNote ?? "" });
        setFormInitialized(true);
    }, [formInitialized, record]);

    const handleBack = () => {
        navigate("/order/cancellations");
    };

    const handleFieldChange = (event) => {
        const value = event?.target?.value ?? "";
        setForm({ note: value });
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        if (!record) {
            return;
        }

        setSubmitError("");
        setIsSubmitting(true);

        try {
            const payload = form.note.trim() ? { sellerNote: form.note.trim() } : {};

            if (normalizedMode === "approve") {
                await OrderService.approveCancellationRequest(record.requestId, payload);
                navigate("/order/cancellations", { state: { flashMessage: "Cancellation request approved successfully." } });
            } else {
                await OrderService.rejectCancellationRequest(record.requestId, payload);
                navigate("/order/cancellations", { state: { flashMessage: "Cancellation request declined." } });
            }
        } catch (submissionError) {
            setSubmitError(extractErrorMessage(submissionError, "Unable to process this request."));
            setIsSubmitting(false);
        }
    };

    const pageTitle = normalizedMode === "approve" ? "Approve cancellation request" : "Decline cancellation request";
    const submitLabel = isSubmitting ? "Processing..." : normalizedMode === "approve" ? "Approve" : "Decline";

    const isOrderPaid = Boolean(order?.paidAt || (typeof order?.paymentStatus === "string" && order.paymentStatus.toLowerCase() === "paid"));
    const refundLabel = (() => {
        if (isOrderPaid && order) {
            return formatCurrencyLabel(order.totalAmount, order.totalCurrency);
        }

        if (record?.refundAmount != null) {
            return formatCurrencyLabel(record.refundAmount, record.refundCurrency);
        }

        return null;
    })();

    const primaryItemTitle = record?.itemsSummary || order?.items?.[0]?.title || "-";

    return (
        <div className="resolution-action-page">
            <div className="resolution-action-page__inner">
                <button
                    type="button"
                    className="resolution-action-page__back"
                    onClick={handleBack}
                    disabled={isSubmitting}
                >
                    Back to cancellations
                </button>

                <header className="resolution-action-page__header">
                    <div>
                        <h1 className="resolution-action-page__title">{pageTitle}</h1>
                        <p className="resolution-action-page__subtitle">
                            {record?.orderNumber
                                ? `Order #${record.orderNumber}`
                                : "Review the buyer's cancellation request before responding."}
                        </p>
                    </div>
                </header>

                {isLoading ? (
                    <div className="resolution-action-page__status">Loading cancellation request...</div>
                ) : loadError ? (
                    <div className="resolution-action-page__status resolution-action-page__status--error">{loadError}</div>
                ) : !record ? (
                    <div className="resolution-action-page__status resolution-action-page__status--error">
                        Cancellation request not found.
                    </div>
                ) : (
                    <form className="resolution-action-page__form" onSubmit={handleSubmit}>
                        <section className="resolution-dialog__section">
                            <h2 className="resolution-dialog__heading">Request overview</h2>
                            <dl className="resolution-dialog__definition">
                                <div>
                                    <dt>Buyer</dt>
                                    <dd>{formatBuyer(record.buyerDisplayName, record.buyerUsername)}</dd>
                                </div>
                                <div>
                                    <dt>Items</dt>
                                    <dd>{primaryItemTitle}</dd>
                                </div>
                                <div>
                                    <dt>Reason</dt>
                                    <dd>{record.reason || "-"}</dd>
                                </div>
                                <div>
                                    <dt>Requested on</dt>
                                    <dd>{formatDateTime(record.requestedAtUtc) || "-"}</dd>
                                </div>
                            </dl>
                        </section>

                        {record.buyerNote && (
                            <section className="resolution-dialog__section">
                                <h2 className="resolution-dialog__heading">Buyer note</h2>
                                <p className="resolution-dialog__note">{record.buyerNote}</p>
                            </section>
                        )}

                        {record.details && (
                            <section className="resolution-dialog__section">
                                <h2 className="resolution-dialog__heading">Details</h2>
                                <p className="resolution-dialog__note">{record.details}</p>
                            </section>
                        )}

                        <section className="resolution-dialog__section resolution-dialog__section--highlight">
                            <h2 className="resolution-dialog__heading">Refund summary</h2>
                            {refundLabel ? (
                                <p className="resolution-dialog__note">Refund to buyer: {refundLabel}</p>
                            ) : (
                                <p className="resolution-dialog__note">
                                    No refund will be issued because this order has not been paid yet.
                                </p>
                            )}
                        </section>

                        <div className="resolution-dialog__control">
                            <label className="resolution-dialog__label" htmlFor="cancellation-note">
                                Message to buyer (optional)
                            </label>
                            <textarea
                                id="cancellation-note"
                                rows={4}
                                value={form.note}
                                onChange={handleFieldChange}
                                placeholder="Add any clarification for the buyer."
                                disabled={isSubmitting}
                            />
                        </div>

                        {submitError && (
                            <div className="resolution-dialog__error">{submitError}</div>
                        )}

                        <div className="resolution-dialog__actions">
                            <button
                                type="button"
                                className="resolution-dialog__button resolution-dialog__button--ghost"
                                onClick={handleBack}
                                disabled={isSubmitting}
                            >
                                Cancel
                            </button>
                            <button
                                type="submit"
                                className="resolution-dialog__button resolution-dialog__button--primary"
                                disabled={isSubmitting}
                            >
                                {submitLabel}
                            </button>
                        </div>
                    </form>
                )}
            </div>
        </div>
    );
};

export default CancellationRequestActionPage;
