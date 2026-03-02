import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import OrderService from "../../services/OrderService";
import "./ResolutionCenterPage.scss";

const isoToDateInput = (isoValue) => {
    if (!isoValue) {
        return "";
    }

    const date = new Date(isoValue);
    if (Number.isNaN(date.getTime())) {
        return "";
    }

    return date.toISOString().slice(0, 10);
};

const toUtcEndOfDayIso = (dateInput) => {
    if (!dateInput) {
        return null;
    }

    const [year, month, day] = dateInput.split("-").map(Number);
    if ([year, month, day].some((part) => Number.isNaN(part) || part <= 0)) {
        return null;
    }

    const utcDate = Date.UTC(year, month - 1, day, 23, 59, 59, 999);
    const date = new Date(utcDate);
    if (Number.isNaN(date.getTime())) {
        return null;
    }

    return date.toISOString();
};

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
    const totalSource = order.total ?? order.Total ?? null;
    const totalAmount = typeof totalSource === "number" ? totalSource : totalSource?.amount ?? totalSource?.Amount ?? null;
    const totalCurrency = typeof totalSource === "string" ? totalSource : totalSource?.currency ?? totalSource?.Currency ?? order.currency ?? order.Currency ?? "USD";

    return {
        id: order.id ?? order.Id ?? "",
        orderNumber: order.orderNumber ?? order.OrderNumber ?? "",
        items: itemsSource.map((item) => ({
            title: item?.title ?? item?.Title ?? "",
            quantity: item?.quantity ?? item?.Quantity ?? 1,
            price: item?.price ?? item?.Price ?? null,
        })),
        totalAmount,
        totalCurrency,
        paidAt: order.paidAt ?? order.PaidAt ?? order.paidAtUtc ?? order.PaidAtUtc ?? null,
        paymentStatus: order.paymentStatus ?? order.PaymentStatus ?? null,
    };
};

const ReturnRequestActionPage = () => {
    const { requestId, mode } = useParams();
    const normalizedMode = mode === "decline" ? "decline" : "approve";
    const navigate = useNavigate();

    const [record, setRecord] = useState(null);
    const [order, setOrder] = useState(null);
    const [isLoading, setIsLoading] = useState(true);
    const [loadError, setLoadError] = useState(null);
    const [formInitialized, setFormInitialized] = useState(false);
    const [form, setForm] = useState({
        dueDate: "",
        note: "",
    });
    const [submitError, setSubmitError] = useState("");
    const [isSubmitting, setIsSubmitting] = useState(false);

    useEffect(() => {
        if (mode !== "approve" && mode !== "decline") {
            navigate("/order/returns", { replace: true });
        }
    }, [mode, navigate]);

    useEffect(() => {
        if (!requestId) {
            setLoadError("Return request not found.");
            setIsLoading(false);
            return;
        }

        const controller = new AbortController();

        const load = async () => {
            setIsLoading(true);
            setLoadError(null);

            try {
                const [requestResponse, orderResponse] = await Promise.all([
                    OrderService.getReturnRequestById(requestId, controller.signal),
                    OrderService.getOrderByReturnRequestId(requestId, controller.signal),
                ]);

                const payload = requestResponse?.data ?? requestResponse;
                const orderPayload = orderResponse?.data ?? orderResponse;

                if (!payload) {
                    setLoadError("Return request not found.");
                    return;
                }

                setRecord(payload);
                setOrder(orderPayload ? normalizeOrder(orderPayload) : null);
                setFormInitialized(false);
            } catch (fetchError) {
                if (controller.signal.aborted) {
                    return;
                }

                setLoadError(extractErrorMessage(fetchError, "Unable to load this return request."));
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

        setForm({
            dueDate: normalizedMode === "approve" ? isoToDateInput(record.buyerReturnDueAtUtc) : "",
            note: record.sellerNote ?? "",
        });
        setFormInitialized(true);
    }, [formInitialized, record, normalizedMode]);

    const handleBack = () => {
        navigate("/order/returns");
    };

    const handleFieldChange = (field) => (event) => {
        const value = event?.target?.value ?? "";
        setForm((prev) => ({
            ...prev,
            [field]: value,
        }));
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        if (!record) {
            return;
        }

        setSubmitError("");
        setIsSubmitting(true);

        try {
            if (normalizedMode === "approve") {
                const payload = {};

                if (form.dueDate) {
                    const dueDateIso = toUtcEndOfDayIso(form.dueDate);
                    if (!dueDateIso) {
                        setSubmitError("Invalid date format. Please use YYYY-MM-DD.");
                        setIsSubmitting(false);
                        return;
                    }

                    payload.buyerReturnDueAtUtc = dueDateIso;
                }

                if (form.note.trim()) {
                    payload.sellerNote = form.note.trim();
                }

                await OrderService.approveReturnRequest(record.requestId, payload);
                navigate("/order/returns", { state: { flashMessage: "Return request approved successfully." } });
            } else {
                const payload = form.note.trim() ? { sellerNote: form.note.trim() } : {};
                await OrderService.rejectReturnRequest(record.requestId, payload);
                navigate("/order/returns", { state: { flashMessage: "Return request declined." } });
            }
        } catch (submissionError) {
            setSubmitError(extractErrorMessage(submissionError, "Unable to process this request."));
            setIsSubmitting(false);
        }
    };

    const pageTitle = normalizedMode === "approve" ? "Approve return request" : "Decline return request";
    const submitLabel = isSubmitting ? "Processing..." : normalizedMode === "approve" ? "Approve" : "Decline";

    const primaryCurrency = order?.totalCurrency ?? record?.orderTotalCurrency ?? record?.refundCurrency ?? "USD";
    const orderTotalAmount = order?.totalAmount ?? (record?.orderTotalAmount != null ? Number(record.orderTotalAmount) : null);
    const restockingFeeAmount = record?.restockingFeeAmount != null ? Number(record.restockingFeeAmount) : null;
    const restockingFeeLabel = restockingFeeAmount != null
        ? formatCurrencyLabel(restockingFeeAmount, record?.restockingFeeCurrency ?? primaryCurrency)
        : null;

    const isOrderPaid = (() => {
        if (order) {
            if (order.paidAt) {
                return true;
            }

            if (typeof order.paymentStatus === "string" && order.paymentStatus.toLowerCase() === "paid") {
                return true;
            }
        }

        if (record?.isOrderPaid != null) {
            return Boolean(record.isOrderPaid);
        }

        return false;
    })();

    const refundLabel = record?.refundAmount != null
        ? formatCurrencyLabel(record.refundAmount, record.refundCurrency ?? primaryCurrency)
        : null;

    const expectedRefundAmount = isOrderPaid && orderTotalAmount != null
        ? (() => {
            const total = Number(orderTotalAmount);
            if (Number.isNaN(total)) {
                return null;
            }

            const restocking = restockingFeeAmount != null ? restockingFeeAmount : 0;
            if (Number.isNaN(restocking)) {
                return null;
            }

            const estimated = total - restocking;
            return estimated >= 0 ? estimated : null;
        })()
        : null;

    const expectedRefundLabel = expectedRefundAmount != null
        ? formatCurrencyLabel(expectedRefundAmount, primaryCurrency)
        : null;

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
                    Back to returns
                </button>

                <header className="resolution-action-page__header">
                    <div>
                        <h1 className="resolution-action-page__title">{pageTitle}</h1>
                        <p className="resolution-action-page__subtitle">
                            {record?.orderNumber
                                ? `Order #${record.orderNumber}`
                                : "Review the buyer's request before responding."}
                        </p>
                    </div>
                </header>

                {isLoading ? (
                    <div className="resolution-action-page__status">Loading return request...</div>
                ) : loadError ? (
                    <div className="resolution-action-page__status resolution-action-page__status--error">{loadError}</div>
                ) : !record ? (
                    <div className="resolution-action-page__status resolution-action-page__status--error">
                        Return request not found.
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
                                    <dt>Preferred resolution</dt>
                                    <dd>{record.preferredResolution || "-"}</dd>
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
                            {isOrderPaid ? (
                                refundLabel ? (
                                    <p className="resolution-dialog__note">Refund issued to buyer: {refundLabel}</p>
                                ) : expectedRefundLabel ? (
                                    <p className="resolution-dialog__note">
                                        Estimated refund: {expectedRefundLabel}
                                        {restockingFeeLabel ? ` (after ${restockingFeeLabel} restocking fee)` : ""}. Issue this once you have the item back.
                                    </p>
                                ) : (
                                    <p className="resolution-dialog__note">
                                        Issue the appropriate refund once you have reviewed the returned item.
                                    </p>
                                )
                            ) : (
                                <p className="resolution-dialog__note">
                                    Buyer has not completed payment for this order, so no refund is required.
                                </p>
                            )}
                        </section>

                        {normalizedMode === "approve" && (
                            <div className="resolution-dialog__control">
                                <label className="resolution-dialog__label" htmlFor="return-due-date">
                                    Buyer return due date (optional)
                                </label>
                                <input
                                    id="return-due-date"
                                    type="date"
                                    value={form.dueDate}
                                    onChange={handleFieldChange("dueDate")}
                                    disabled={isSubmitting}
                                />
                                <p className="resolution-dialog__hint">
                                    Leave blank to keep the current return deadline.
                                </p>
                            </div>
                        )}

                        <div className="resolution-dialog__control">
                            <label className="resolution-dialog__label" htmlFor="return-note">
                                Message to buyer (optional)
                            </label>
                            <textarea
                                id="return-note"
                                rows={4}
                                value={form.note}
                                onChange={handleFieldChange("note")}
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

export default ReturnRequestActionPage;
