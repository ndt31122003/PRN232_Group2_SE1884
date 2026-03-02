import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import Notice from "../../components/Common/CustomNotification";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import OrderService from "../../services/OrderService";
import {
    buyerDisplayName,
    collectBulkOrderContext,
    formatAddressLines,
    formatCurrency,
    formatDate,
    normalizeOrderForBulk,
} from "./bulkHelpers";
import "./BulkFeedbackPage.scss";

const MAX_COMMENT_LENGTH = 80;
const DEFAULT_COMMENT = "Great buyer! Fast payment. Thank you!";

const initialStatusMap = (orderIds) => Object.fromEntries(orderIds.map((orderId) => [orderId, { state: "idle" }]));

const trimComment = (value) => {
    const text = value ?? "";
    return text.length > MAX_COMMENT_LENGTH ? text.slice(0, MAX_COMMENT_LENGTH) : text;
};

const BulkFeedbackPage = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const { fromPath, orderIds: contextOrderIds, snapshotMap } = useMemo(() => collectBulkOrderContext(location), [location]);

    const [orderIds] = useState(contextOrderIds);
    const [ordersById, setOrdersById] = useState(snapshotMap);
    const [orderStatuses, setOrderStatuses] = useState(() => initialStatusMap(contextOrderIds));
    const [ordersLoading, setOrdersLoading] = useState(contextOrderIds.length > 0 && Object.keys(snapshotMap).length < contextOrderIds.length);
    const [ordersError, setOrdersError] = useState("");

    const [feedbackDrafts, setFeedbackDrafts] = useState(() => {
        const drafts = {};
        contextOrderIds.forEach((orderId) => {
            drafts[orderId] = DEFAULT_COMMENT;
        });
        return drafts;
    });
    const [sharedComment, setSharedComment] = useState(DEFAULT_COMMENT);
    const [submissionState, setSubmissionState] = useState({ isSubmitting: false, successCount: 0, failureCount: 0, error: "" });

    useEffect(() => {
        setOrderStatuses((prev) => {
            const next = { ...prev };
            orderIds.forEach((orderId) => {
                if (!next[orderId]) {
                    next[orderId] = { state: "idle" };
                }
                const order = ordersById[orderId];
                if (order?.sellerFeedback) {
                    next[orderId] = {
                        state: "skipped",
                        message: "Feedback already left.",
                        feedback: order.sellerFeedback,
                    };
                }
            });
            return next;
        });
    }, [orderIds, ordersById]);

    useEffect(() => {
        setFeedbackDrafts((prev) => {
            const next = { ...prev };
            orderIds.forEach((orderId) => {
                if (!next[orderId]) {
                    next[orderId] = DEFAULT_COMMENT;
                }
            });
            return next;
        });
    }, [orderIds]);

    useEffect(() => {
        const missingIds = orderIds.filter((orderId) => !ordersById[orderId]);
        if (missingIds.length === 0) {
            setOrdersLoading(false);
            return;
        }

        let ignore = false;
        const controller = new AbortController();

        const fetchOrders = async () => {
            setOrdersLoading(true);
            setOrdersError("");

            const responses = await Promise.all(
                missingIds.map(async (orderId) => {
                    try {
                        const response = await OrderService.getOrderById(orderId, controller.signal);
                        const data = response?.data ?? response;
                        return { orderId, data };
                    } catch (err) {
                        if (controller.signal.aborted) {
                            return { orderId, aborted: true };
                        }
                        console.error("Failed to load order", orderId, err);
                        return { orderId, error: err };
                    }
                })
            );

            if (ignore) {
                return;
            }

            const successes = responses.filter((entry) => entry.data).map((entry) => normalizeOrderForBulk(entry.data));
            if (successes.length > 0) {
                setOrdersById((prev) => {
                    const next = { ...prev };
                    successes.forEach((order) => {
                        if (order.id) {
                            next[order.id] = order;
                        }
                    });
                    return next;
                });
            }

            const failures = responses.filter((entry) => entry.error && !entry.aborted);
            if (failures.length > 0) {
                setOrdersError("Some orders could not be loaded. Review the selection or try again.");
                setOrderStatuses((prev) => {
                    const next = { ...prev };
                    failures.forEach((entry) => {
                        next[entry.orderId] = {
                            state: "error",
                            message: "Unable to load order details.",
                        };
                    });
                    return next;
                });
            }

            setOrdersLoading(false);
        };

        fetchOrders();

        return () => {
            ignore = true;
            controller.abort();
        };
    }, [orderIds, ordersById]);

    const orders = useMemo(() => orderIds.map((orderId) => ordersById[orderId]).filter((order) => order && order.id), [orderIds, ordersById]);

    const handleBack = useCallback(() => {
        navigate(fromPath, { replace: true });
    }, [fromPath, navigate]);

    const handleSharedCommentChange = (event) => {
        setSharedComment(trimComment(event.target.value));
    };

    const applySharedComment = () => {
        const trimmed = trimComment(sharedComment);
        setFeedbackDrafts((prev) => {
            const next = { ...prev };
            orderIds.forEach((orderId) => {
                const status = orderStatuses[orderId];
                if (status?.state === "skipped") {
                    return;
                }
                next[orderId] = trimmed;
            });
            return next;
        });
    };

    const handleCommentChange = (orderId) => (event) => {
        const value = trimComment(event.target.value);
        setFeedbackDrafts((prev) => ({ ...prev, [orderId]: value }));
    };

    const resetSubmissionError = () => {
        setSubmissionState((prev) => ({ ...prev, error: "" }));
    };

    const handleSubmit = useCallback(
        async (event) => {
            event.preventDefault();

            const actionableOrders = orderIds
                .filter((orderId) => {
                    const status = orderStatuses[orderId];
                    return status?.state !== "skipped" && status?.state !== "error";
                })
                .map((orderId) => ordersById[orderId])
                .filter((order) => order && order.id);

            if (actionableOrders.length === 0) {
                Notice({ msg: "No eligible orders to leave feedback.", isSuccess: false });
                return;
            }

            setSubmissionState({ isSubmitting: true, successCount: 0, failureCount: 0, error: "" });
            setOrderStatuses((prev) => {
                const next = { ...prev };
                actionableOrders.forEach((order) => {
                    next[order.id] = { state: "processing" };
                });
                return next;
            });

            let successCount = 0;
            let failureCount = 0;

            for (const order of actionableOrders) {
                const draft = trimComment(feedbackDrafts[order.id] ?? "");
                if (draft.length === 0) {
                    failureCount += 1;
                    setOrderStatuses((prev) => ({
                        ...prev,
                        [order.id]: {
                            state: "error",
                            message: "Feedback comment cannot be empty.",
                        },
                    }));
                    continue;
                }

                try {
                    const response = await OrderService.leaveFeedback(order.id, {
                        UseStoredFeedback: false,
                        Comment: draft,
                        StoredFeedbackKey: null,
                    });
                    const data = response?.data ?? response;
                    successCount += 1;
                    setOrderStatuses((prev) => ({
                        ...prev,
                        [order.id]: {
                            state: "success",
                            message: "Feedback sent",
                        },
                    }));
                    if (data?.feedback ?? data?.Feedback) {
                        setOrdersById((prev) => {
                            const next = { ...prev };
                            const existing = next[order.id] ?? order;
                            next[order.id] = { ...existing, sellerFeedback: data.feedback ?? data.Feedback };
                            return next;
                        });
                    }
                } catch (err) {
                    failureCount += 1;
                    const message =
                        err?.response?.data?.detail ??
                        err?.response?.data?.message ??
                        err?.message ??
                        "Unable to leave feedback.";
                    setOrderStatuses((prev) => ({
                        ...prev,
                        [order.id]: {
                            state: "error",
                            message,
                        },
                    }));
                }
            }

            setSubmissionState({
                isSubmitting: false,
                successCount,
                failureCount,
                error: failureCount > 0 ? "Some feedback could not be sent." : "",
            });

            if (successCount > 0 && failureCount === 0) {
                Notice({ msg: `Left feedback for ${successCount} order${successCount === 1 ? "" : "s"}.`, isSuccess: true });
            } else if (successCount > 0) {
                Notice({
                    msg: `Left feedback for ${successCount} order${successCount === 1 ? "" : "s"}. ${failureCount} order${failureCount === 1 ? "" : "s"} failed.`,
                    isSuccess: false,
                });
            } else {
                Notice({ msg: "No feedback was sent. Review the errors below.", isSuccess: false });
            }
        },
        [feedbackDrafts, orderIds, orderStatuses, ordersById]
    );

    const disableSubmit = submissionState.isSubmitting || ordersLoading || orders.length === 0;

    if (orderIds.length === 0) {
        return (
            <div className="bulk-feedback bulk-feedback--empty">
                <div className="bulk-feedback__empty-card">
                    <h1>No orders selected</h1>
                    <p>Select orders from the orders list to leave feedback in bulk.</p>
                    <button type="button" className="bulk-feedback__primary" onClick={handleBack}>
                        Back to orders
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="bulk-feedback">
            <header className="bulk-feedback__header">
                <div className="bulk-feedback__branding" aria-label="eBay">
                    <span className="bulk-feedback__logo bulk-feedback__logo--red">e</span>
                    <span className="bulk-feedback__logo bulk-feedback__logo--green">b</span>
                    <span className="bulk-feedback__logo bulk-feedback__logo--yellow">a</span>
                    <span className="bulk-feedback__logo bulk-feedback__logo--blue">y</span>
                </div>
                <button type="button" className="bulk-feedback__back" onClick={handleBack}>
                    ← Back to orders
                </button>
            </header>

            <main className="bulk-feedback__main">
                <section className="bulk-feedback__intro">
                    <h1>Leave feedback in bulk</h1>
                    <p>Share a quick thank you with your buyers and keep your seller reputation shining.</p>
                    <p className="bulk-feedback__intro-meta">
                        Selected orders: <strong>{orderIds.length}</strong>
                    </p>
                </section>

                <section className="bulk-feedback__controls" aria-label="Bulk comment tools">
                    <div className="bulk-feedback__control-field">
                        <label htmlFor="bulk-feedback-template">Feedback comment</label>
                        <textarea
                            id="bulk-feedback-template"
                            value={sharedComment}
                            onChange={handleSharedCommentChange}
                            maxLength={MAX_COMMENT_LENGTH}
                            rows={2}
                        />
                        <div className="bulk-feedback__control-meta">
                            <span>{sharedComment.length}/{MAX_COMMENT_LENGTH}</span>
                            <button type="button" onClick={applySharedComment}>
                                Apply to all
                            </button>
                        </div>
                    </div>
                </section>

                <form className="bulk-feedback__content" onSubmit={handleSubmit}>
                    {submissionState.error && (
                        <div className="bulk-feedback__alert" role="alert">
                            <div>
                                <strong>We ran into a problem</strong>
                                <p>{submissionState.error}</p>
                            </div>
                            <button type="button" onClick={resetSubmissionError}>
                                Dismiss
                            </button>
                        </div>
                    )}

                    {ordersLoading && (
                        <div className="bulk-feedback__placeholder">
                            <LoadingScreen isOverlay={true} />
                            <span>Loading selected orders…</span>
                        </div>
                    )}

                    {ordersError && (
                        <div className="bulk-feedback__alert" role="alert">
                            <div>
                                <strong>Order details incomplete</strong>
                                <p>{ordersError}</p>
                            </div>
                        </div>
                    )}

                    <div className="bulk-feedback__orders">
                        {orders.map((order) => {
                            const status = orderStatuses[order.id] ?? { state: "idle" };
                            const comment = feedbackDrafts[order.id] ?? "";
                            const addressLines = formatAddressLines(order.shipToAddress);
                            const disabled = status.state === "skipped" || status.state === "success";

                            return (
                                <article key={order.id} className={`bulk-feedback__order bulk-feedback__order--${status.state}`}>
                                    <header className="bulk-feedback__order-header">
                                        <div>
                                            <h3>Order {order.orderNumber || order.id.slice(0, 8)}</h3>
                                            <p>Buyer: {buyerDisplayName(order)}</p>
                                        </div>
                                        <span className="bulk-feedback__order-total">{formatCurrency(order.total)}</span>
                                    </header>
                                    <div className="bulk-feedback__order-body">
                                        <div className="bulk-feedback__order-section">
                                            <span className="bulk-feedback__order-label">Sold on</span>
                                            <span>{formatDate(order.orderedAt)}</span>
                                        </div>
                                        <div className="bulk-feedback__order-section">
                                            <span className="bulk-feedback__order-label">Items</span>
                                            <span>{order.items.length}</span>
                                        </div>
                                        <div className="bulk-feedback__order-section">
                                            <span className="bulk-feedback__order-label">Ship to</span>
                                            <span>
                                                {order.shipToName || buyerDisplayName(order)}
                                                {addressLines.length > 0 && (
                                                    <>
                                                        <br />
                                                        {addressLines.join(", ")}
                                                    </>
                                                )}
                                            </span>
                                        </div>
                                    </div>
                                    <div className="bulk-feedback__order-draft">
                                        <label>
                                            <span>Feedback comment</span>
                                            <textarea
                                                value={comment}
                                                onChange={handleCommentChange(order.id)}
                                                maxLength={MAX_COMMENT_LENGTH}
                                                rows={2}
                                                disabled={disabled}
                                            />
                                        </label>
                                        <div className="bulk-feedback__draft-meta">
                                            <span>{comment.length}/{MAX_COMMENT_LENGTH}</span>
                                            {status.state === "skipped" && <span className="bulk-feedback__tag">Already sent</span>}
                                        </div>
                                    </div>
                                    <footer className="bulk-feedback__order-footer">
                                        {status.state === "idle" && <span className="bulk-feedback__status">Ready</span>}
                                        {status.state === "processing" && <span className="bulk-feedback__status">Sending…</span>}
                                        {status.state === "success" && <span className="bulk-feedback__status bulk-feedback__status--success">Feedback sent</span>}
                                        {status.state === "error" && (
                                            <span className="bulk-feedback__status bulk-feedback__status--error">{status.message || "Unable to send feedback."}</span>
                                        )}
                                        {status.state === "skipped" && (
                                            <span className="bulk-feedback__status">Feedback already left</span>
                                        )}
                                    </footer>
                                </article>
                            );
                        })}
                        {orders.length === 0 && !ordersLoading && (
                            <div className="bulk-feedback__empty-orders">
                                <p>We could not load any order details. Try returning to the orders list and selecting them again.</p>
                            </div>
                        )}
                    </div>

                    <footer className="bulk-feedback__footer">
                        <div className="bulk-feedback__summary">
                            <div>
                                <span>Orders selected</span>
                                <strong>{orderIds.length}</strong>
                            </div>
                            <div>
                                <span>Feedback sent</span>
                                <strong>{submissionState.successCount}</strong>
                            </div>
                            <div>
                                <span>Failed</span>
                                <strong>{submissionState.failureCount}</strong>
                            </div>
                        </div>
                        <div className="bulk-feedback__actions">
                            <button type="button" className="bulk-feedback__secondary" onClick={handleBack} disabled={submissionState.isSubmitting}>
                                Cancel
                            </button>
                            <button type="submit" className="bulk-feedback__primary" disabled={disableSubmit}>
                                {submissionState.isSubmitting ? "Sending…" : "Leave feedback"}
                            </button>
                        </div>
                    </footer>
                </form>
            </main>
        </div>
    );
};

export default BulkFeedbackPage;
