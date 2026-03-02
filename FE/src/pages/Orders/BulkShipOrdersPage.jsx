import React, { useCallback, useEffect, useMemo, useRef, useState } from "react";
import dayjs from "dayjs";
import { useLocation, useNavigate } from "react-router-dom";
import Notice from "../../components/Common/CustomNotification";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import Dialog from "../../components/Dialog/Dialog";
import OrderService from "../../services/OrderService";
import ShippingServiceApi from "../../services/ShippingService";
import axiosInstance from "../../utils/axiosCustomize";
import {
    buyerDisplayName,
    collectBulkOrderContext,
    formatAddressLines,
    formatCurrency,
    formatDate,
    normalizeOrderForBulk,
} from "./bulkHelpers";
import "./BulkShipOrdersPage.scss";

const LABEL_PAPER_SIZES = [
    { value: "4x6", label: '4" x 6"' },
    { value: "4x6.75", label: '4" x 6.75"' },
    { value: "8x11", label: '8.5" x 11"' },
];

const PAYMENT_METHODS = [
    { value: "ebay-balance", label: "Your funds" },
    { value: "paypal", label: "PayPal" },
    { value: "card", label: "Credit or debit card" },
];

const buildDefaultShippingForm = () => ({
    packaging: "custom",
    weightLb: "1",
    weightOz: "0",
    lengthIn: "12",
    widthIn: "10",
    heightIn: "2",
    serviceId: "",
    insurance: "standard",
});

const parseDecimal = (value, fallback = 0) => {
    const numeric = Number(value);
    return Number.isFinite(numeric) ? numeric : fallback;
};

const convertWeightToOz = (pounds, ounces) => {
    const lb = Math.max(0, parseDecimal(pounds));
    const oz = Math.max(0, parseDecimal(ounces));
    return lb * 16 + oz;
};

const buildAbsoluteUrl = (path) => {
    if (!path) {
        return "";
    }
    if (/^https?:/i.test(path)) {
        return path;
    }

    const base = axiosInstance.defaults?.baseURL ?? window.location.origin;
    try {
        const normalizedBase = base.replace(/\/$/, "");
        const apiTrimmed = normalizedBase.endsWith("/api") ? normalizedBase.slice(0, -4) : normalizedBase;
        return new URL(path, `${apiTrimmed}/`).toString();
    } catch (error) {
        console.warn("Unable to build label URL", error);
        return path;
    }
};

const normalizeShippingService = (service) => {
    if (!service) {
        return null;
    }

    const price = Number(service?.baseCost?.amount ?? service?.BaseCost?.Amount ?? 0);

    return {
        id: service?.slug ?? service?.id ?? "",
        serviceId: service?.id ?? service?.serviceId ?? "",
        carrier: service?.carrier ?? service?.Carrier ?? "",
        serviceCode: service?.serviceCode ?? service?.ServiceCode ?? service?.slug ?? "",
        name: service?.serviceName ?? service?.ServiceName ?? "",
        savings: service?.savingsDescription ?? service?.SavingsDescription ?? "",
        window: service?.deliveryWindowLabel ?? service?.DeliveryWindowLabel ?? "",
        price,
        currency: service?.baseCost?.currency ?? service?.BaseCost?.Currency ?? "USD",
        printerRequired: Boolean(service?.printerRequired ?? service?.PrinterRequired),
        notes: service?.notes ?? service?.Notes ?? "",
    };
};

const buildShippingPayload = (form, selectedService, paymentMethod, shipDateValue) => {
    if (!selectedService) {
        return { error: "Select a shipping service to continue." };
    }

    const shipDate = dayjs(shipDateValue);
    if (!shipDate.isValid()) {
        return { error: "Choose a valid ship date." };
    }

    const weightOz = convertWeightToOz(form.weightLb, form.weightOz);
    if (weightOz <= 0) {
        return { error: "Package weight must be greater than zero." };
    }

    const payload = {
        carrier: selectedService.carrier,
        serviceCode: selectedService.serviceCode || selectedService.id,
        serviceName: selectedService.name,
        packageType: form.packaging,
        weightOz,
        lengthIn: parseDecimal(form.lengthIn),
        widthIn: parseDecimal(form.widthIn),
        heightIn: parseDecimal(form.heightIn),
        shipDate: shipDate.startOf("day").toISOString(),
        paymentMethod,
    };

    return { payload };
};

const initialStatusMap = (orderIds) => {
    const entries = orderIds.map((orderId) => [orderId, { state: "idle" }]);
    return Object.fromEntries(entries);
};

const BulkShipOrdersPage = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const { fromPath, orderIds: contextOrderIds, snapshotMap } = useMemo(() => collectBulkOrderContext(location), [location]);

    const [orderIds] = useState(contextOrderIds);
    const [ordersById, setOrdersById] = useState(snapshotMap);
    const [orderForms, setOrderForms] = useState(() => {
        const entries = contextOrderIds.map((orderId) => [orderId, buildDefaultShippingForm()]);
        return Object.fromEntries(entries);
    });
    const [orderValidationErrors, setOrderValidationErrors] = useState({});
    const [orderStatuses, setOrderStatuses] = useState(() => initialStatusMap(contextOrderIds));
    const [ordersLoading, setOrdersLoading] = useState(contextOrderIds.length > 0 && Object.keys(snapshotMap).length < contextOrderIds.length);
    const [ordersError, setOrdersError] = useState("");

    const [shipDate, setShipDate] = useState(dayjs().format("YYYY-MM-DD"));
    const [paperSize, setPaperSize] = useState(LABEL_PAPER_SIZES[0]?.value ?? "4x6");
    const [paymentMethod, setPaymentMethod] = useState(PAYMENT_METHODS[0]?.value ?? "ebay-balance");

    const [shippingServices, setShippingServices] = useState([]);
    const [shippingServicesLoading, setShippingServicesLoading] = useState(true);
    const [shippingServicesError, setShippingServicesError] = useState("");

    const [submissionState, setSubmissionState] = useState({ isSubmitting: false, successCount: 0, failureCount: 0, error: "" });
    const [reviewState, setReviewState] = useState({ isOpen: false, orders: [], total: 0 });
    const fetchedOrderIdsRef = useRef(new Set()); // avoids repeated fetch loops for the same order ids

    useEffect(() => {
        setOrderStatuses((prev) => {
            const next = { ...prev };
            orderIds.forEach((orderId) => {
                if (!next[orderId]) {
                    next[orderId] = { state: "idle" };
                }
            });
            return next;
        });
    }, [orderIds]);

    useEffect(() => {
        setOrderForms((prev) => {
            const next = { ...prev };
            let changed = false;

            orderIds.forEach((orderId) => {
                if (!next[orderId]) {
                    next[orderId] = buildDefaultShippingForm();
                    changed = true;
                }
            });

            Object.keys(next).forEach((existingId) => {
                if (!orderIds.includes(existingId)) {
                    delete next[existingId];
                    changed = true;
                }
            });

            return changed ? next : prev;
        });
    }, [orderIds]);

    useEffect(() => {
        const knownIds = fetchedOrderIdsRef.current;

        for (const id of Array.from(knownIds)) {
            if (!orderIds.includes(id)) {
                knownIds.delete(id);
            }
        }

        const missingIds = orderIds.filter((orderId) => {
            if (knownIds.has(orderId)) {
                return false;
            }

            const entry = ordersById[orderId];
            if (entry && entry.shipToAddress && entry.items && entry.items.length > 0) {
                knownIds.add(orderId);
                return false;
            }

            return true;
        });

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
                successes.forEach((order) => {
                    if (order.id) {
                        knownIds.add(order.id);
                    }
                });

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
                failures.forEach((entry) => knownIds.add(entry.orderId));

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

    useEffect(() => {
        let ignore = false;
        const controller = new AbortController();

        const fetchServices = async () => {
            try {
                setShippingServicesLoading(true);
                setShippingServicesError("");
                const response = await ShippingServiceApi.list(controller.signal);
                if (ignore) {
                    return;
                }
                const data = response?.data ?? response;
                const normalized = Array.isArray(data)
                    ? data.map(normalizeShippingService).filter((service) => service && service.id)
                    : [];
                setShippingServices(normalized);
            } catch (err) {
                if (!controller.signal.aborted) {
                    console.error("Failed to load shipping services", err);
                    setShippingServices([]);
                    setShippingServicesError("Unable to load shipping services. Refresh and try again.");
                }
            } finally {
                if (!ignore) {
                    setShippingServicesLoading(false);
                }
            }
        };

        fetchServices();

        return () => {
            ignore = true;
            controller.abort();
        };
    }, []);

    useEffect(() => {
        if (shippingServices.length === 0) {
            return;
        }

        setOrderForms((prev) => {
            const next = { ...prev };
            let changed = false;

            orderIds.forEach((orderId) => {
                const form = next[orderId];
                if (form && !form.serviceId) {
                    next[orderId] = { ...form, serviceId: shippingServices[0].id };
                    changed = true;
                }
            });

            return changed ? next : prev;
        });
    }, [orderIds, shippingServices]);

    const orders = useMemo(() => orderIds.map((orderId) => ordersById[orderId]).filter((order) => order && order.id), [orderIds, ordersById]);

    const servicesById = useMemo(() => {
        const map = {};
        shippingServices.forEach((service) => {
            map[service.id] = service;
        });
        return map;
    }, [shippingServices]);

    const estimatedTotal = useMemo(() => {
        return orders.reduce((sum, order) => {
            const form = orderForms[order.id];
            if (!form) {
                return sum;
            }
            const service = servicesById[form.serviceId];
            return sum + (service?.price ?? 0);
        }, 0);
    }, [orderForms, orders, servicesById]);

    const handleBack = useCallback(() => {
        navigate(fromPath, { replace: true });
    }, [fromPath, navigate]);

    const updateOrderForm = useCallback((orderId, field, value) => {
        setOrderForms((prev) => {
            const next = { ...prev };
            const existing = next[orderId] ?? buildDefaultShippingForm();
            next[orderId] = { ...existing, [field]: value };
            return next;
        });

        setOrderValidationErrors((prev) => {
            if (!prev[orderId]) {
                return prev;
            }
            const next = { ...prev };
            delete next[orderId];
            return next;
        });
    }, []);

    const handleOrderFieldChange = (orderId, field) => (event) => {
        const value = event?.target?.value ?? "";
        updateOrderForm(orderId, field, value);
    };

    const handleInsuranceChange = (orderId) => (event) => {
        const value = event?.target?.value ?? "standard";
        updateOrderForm(orderId, "insurance", value);
    };

    const handleShipDateChange = (event) => {
        const value = event?.target?.value ?? shipDate;
        setShipDate(value);
    };

    const handlePaperSizeChange = (event) => {
        const value = event?.target?.value ?? paperSize;
        setPaperSize(value);
    };

    const handlePaymentMethodChange = (event) => {
        const value = event?.target?.value ?? paymentMethod;
        setPaymentMethod(value);
    };

    const buildOrderSubmission = useCallback(() => {
        const errors = {};
        const prepared = [];

        orders.forEach((order) => {
            const form = orderForms[order.id] ?? buildDefaultShippingForm();
            const service = servicesById[form.serviceId];
            const { payload, error } = buildShippingPayload(form, service, paymentMethod, shipDate);

            if (error) {
                errors[order.id] = error;
                return;
            }

            prepared.push({
                order,
                form,
                service,
                payload,
            });
        });

        return { errors, prepared };
    }, [orderForms, orders, paymentMethod, servicesById, shipDate]);

    const handleOpenReview = useCallback(
        (event) => {
            event.preventDefault();

            if (orders.length === 0) {
                Notice({ msg: "Select at least one order to continue.", isSuccess: false });
                return;
            }

            const { errors, prepared } = buildOrderSubmission();

            if (Object.keys(errors).length > 0) {
                setOrderValidationErrors(errors);
                Notice({ msg: "Review the highlighted orders before continuing.", isSuccess: false });
                return;
            }

            if (prepared.length === 0) {
                Notice({ msg: "No shipping labels can be created. Check your selections.", isSuccess: false });
                return;
            }

            const total = prepared.reduce((sum, entry) => sum + (entry.service?.price ?? 0), 0);

            setOrderValidationErrors({});
            setReviewState({
                isOpen: true,
                orders: prepared,
                total,
            });
        },
        [buildOrderSubmission, orders.length]
    );

    const handleCloseReview = useCallback(() => {
        setReviewState((prev) => ({ ...prev, isOpen: false }));
    }, []);

    const handleViewLabel = useCallback(
        (orderId) => {
            const entry = orderStatuses[orderId];
            if (!entry || entry.state !== "success") {
                return;
            }
            const labelUrl = entry.result?.labelUrl;
            const absoluteUrl = buildAbsoluteUrl(labelUrl);
            if (!absoluteUrl) {
                Notice({ msg: "Label link is unavailable.", isSuccess: false });
                return;
            }
            window.open(absoluteUrl, "_blank", "noopener,noreferrer");
        },
        [orderStatuses]
    );

    const handleConfirmPurchase = useCallback(async () => {
        const pending = reviewState.orders;
        if (!pending || pending.length === 0) {
            setReviewState({ isOpen: false, orders: [], total: 0 });
            return;
        }

        setReviewState({ isOpen: false, orders: [], total: 0 });

        setSubmissionState({ isSubmitting: true, successCount: 0, failureCount: 0, error: "" });
        setOrderStatuses((prev) => {
            const next = { ...prev };
            pending.forEach((entry) => {
                next[entry.order.id] = { state: "processing" };
            });
            return next;
        });

        let successCount = 0;
        let failureCount = 0;
        const failures = [];

        for (const entry of pending) {
            try {
                const response = await OrderService.printShippingLabel(entry.order.id, entry.payload);
                const result = response?.data ?? response;
                successCount += 1;
                setOrderStatuses((prev) => ({
                    ...prev,
                    [entry.order.id]: {
                        state: "success",
                        result,
                    },
                }));
            } catch (err) {
                failureCount += 1;
                const message =
                    err?.response?.data?.detail ??
                    err?.response?.data?.message ??
                    err?.message ??
                    "Unable to create shipping label.";
                failures.push(message);
                setOrderStatuses((prev) => ({
                    ...prev,
                    [entry.order.id]: {
                        state: "error",
                        message,
                    },
                }));
            }
        }

        const submissionError = failureCount > 0 ? "Some labels could not be created." : "";

        setSubmissionState({ isSubmitting: false, successCount, failureCount, error: submissionError });

        if (successCount > 0 && failureCount === 0) {
            Notice({ msg: `Created ${successCount} shipping label${successCount === 1 ? "" : "s"}.`, isSuccess: true });
        } else if (successCount > 0) {
            Notice({
                msg: `Created ${successCount} shipping label${successCount === 1 ? "" : "s"}. ${failureCount} order${failureCount === 1 ? "" : "s"} failed.`,
                isSuccess: false,
            });
        } else {
            const detail = failures[0] ?? "No shipping labels were created. Review the errors below.";
            Notice({ msg: detail, isSuccess: false });
        }
    }, [reviewState.orders]);

    const disableSubmit =
        submissionState.isSubmitting ||
        shippingServicesLoading ||
        Boolean(shippingServicesError) ||
        ordersLoading ||
        orders.length === 0;

    if (orderIds.length === 0) {
        return (
            <div className="bulk-ship bulk-ship--empty">
                <div className="bulk-ship__empty-card">
                    <h1>No orders selected</h1>
                    <p>Select orders from the orders list to create shipping labels in bulk.</p>
                    <button type="button" className="bulk-ship__primary" onClick={handleBack}>
                        Back to orders
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="bulk-ship">
            <header className="bulk-ship__header">
                <div className="bulk-ship__branding" aria-label="eBay">
                    <span className="bulk-ship__logo bulk-ship__logo--red">e</span>
                    <span className="bulk-ship__logo bulk-ship__logo--green">b</span>
                    <span className="bulk-ship__logo bulk-ship__logo--yellow">a</span>
                    <span className="bulk-ship__logo bulk-ship__logo--blue">y</span>
                </div>
                <button type="button" className="bulk-ship__back" onClick={handleBack}>
                    ← Back to orders
                </button>
            </header>

            <main className="bulk-ship__main">
                <div className="bulk-ship__intro">
                    <h1>Buy labels in bulk</h1>
                    <p>Update each package, review the total, then confirm everything at checkout.</p>
                    <div className="bulk-ship__intro-actions" aria-label="Bulk tools">
                        <button type="button" className="bulk-ship__link" disabled>
                            Combine orders per buyer
                        </button>
                        <button type="button" className="bulk-ship__link" disabled>
                            Print pick list
                        </button>
                    </div>
                </div>

                <div className="bulk-ship__layout">
                    <section className="bulk-ship__orders-grid">
                        {ordersLoading && (
                            <div className="bulk-ship__placeholder">
                                <LoadingScreen isOverlay={true} />
                                <span>Loading selected orders…</span>
                            </div>
                        )}

                        {ordersError && (
                            <div className="bulk-ship__alert" role="alert">
                                <div>
                                    <strong>Order details incomplete</strong>
                                    <p>{ordersError}</p>
                                </div>
                            </div>
                        )}

                        {orders.map((order) => {
                            const form = orderForms[order.id] ?? buildDefaultShippingForm();
                            const service = servicesById[form.serviceId];
                            const validationError = orderValidationErrors[order.id];
                            const status = orderStatuses[order.id] ?? { state: "idle" };
                            const addressLines = formatAddressLines(order.shipToAddress);

                            return (
                                <article
                                    key={order.id}
                                    className={`bulk-ship__order-card ${validationError ? "bulk-ship__order-card--error" : ""}`}
                                >
                                    <header className="bulk-ship__order-card-header">
                                        <div className="bulk-ship__order-summary">
                                            <div className="bulk-ship__order-heading">
                                                <h2>{order.orderNumber || order.id.slice(0, 8)}</h2>
                                                <span>Expected by {formatDate(order.orderedAt)}</span>
                                            </div>
                                            <div className="bulk-ship__order-meta">
                                                <span className="bulk-ship__order-line">
                                                    Buyer paid {formatCurrency(order.total)} for shipping
                                                </span>
                                                <span className="bulk-ship__order-line">Buyer: {buyerDisplayName(order)}</span>
                                                <span className="bulk-ship__order-line">
                                                    Ship to: {order.shipToName || buyerDisplayName(order)}
                                                    {addressLines.length > 0 && ` • ${addressLines.join(", ")}`}
                                                </span>
                                            </div>
                                        </div>
                                        <div className="bulk-ship__order-controls">
                                            {shippingServicesLoading ? (
                                                <div className="bulk-ship__order-loading">
                                                    <LoadingScreen isOverlay={true} />
                                                    <span>Loading services…</span>
                                                </div>
                                            ) : shippingServicesError ? (
                                                <div className="bulk-ship__order-error">{shippingServicesError}</div>
                                            ) : (
                                                <label className="bulk-ship__field">
                                                    <span>Shipping service</span>
                                                    <select value={form.serviceId} onChange={handleOrderFieldChange(order.id, "serviceId")}>
                                                        {shippingServices.map((option) => (
                                                            <option key={option.id} value={option.id}>
                                                                {option.name}
                                                            </option>
                                                        ))}
                                                    </select>
                                                </label>
                                            )}
                                            <span className="bulk-ship__order-price">{formatCurrency(service?.price ?? 0)}</span>
                                        </div>
                                    </header>

                                    <div className="bulk-ship__order-inputs">
                                        <label className="bulk-ship__field">
                                            <span>Weight (lb)</span>
                                            <input
                                                type="number"
                                                min="0"
                                                step="0.1"
                                                value={form.weightLb}
                                                onChange={handleOrderFieldChange(order.id, "weightLb")}
                                            />
                                        </label>
                                        <label className="bulk-ship__field">
                                            <span>Weight (oz)</span>
                                            <input
                                                type="number"
                                                min="0"
                                                step="1"
                                                value={form.weightOz}
                                                onChange={handleOrderFieldChange(order.id, "weightOz")}
                                            />
                                        </label>
                                        <label className="bulk-ship__field">
                                            <span>Length (in)</span>
                                            <input
                                                type="number"
                                                min="0"
                                                step="0.1"
                                                value={form.lengthIn}
                                                onChange={handleOrderFieldChange(order.id, "lengthIn")}
                                            />
                                        </label>
                                        <label className="bulk-ship__field">
                                            <span>Width (in)</span>
                                            <input
                                                type="number"
                                                min="0"
                                                step="0.1"
                                                value={form.widthIn}
                                                onChange={handleOrderFieldChange(order.id, "widthIn")}
                                            />
                                        </label>
                                        <label className="bulk-ship__field">
                                            <span>Height (in)</span>
                                            <input
                                                type="number"
                                                min="0"
                                                step="0.1"
                                                value={form.heightIn}
                                                onChange={handleOrderFieldChange(order.id, "heightIn")}
                                            />
                                        </label>
                                    </div>

                                    <div className="bulk-ship__insurance-options" role="radiogroup" aria-label={`Insurance for order ${order.orderNumber || order.id}`}>
                                        <label className={`bulk-ship__insurance-option ${form.insurance === "standard" ? "is-active" : ""}`}>
                                            <input
                                                type="radio"
                                                name={`insurance-${order.id}`}
                                                value="standard"
                                                checked={form.insurance === "standard"}
                                                onChange={handleInsuranceChange(order.id)}
                                            />
                                            Standard insurance
                                        </label>
                                        <label className={`bulk-ship__insurance-option ${form.insurance === "shipcover" ? "is-active" : ""}`}>
                                            <input
                                                type="radio"
                                                name={`insurance-${order.id}`}
                                                value="shipcover"
                                                checked={form.insurance === "shipcover"}
                                                onChange={handleInsuranceChange(order.id)}
                                            />
                                            ShipCover Insurance (+$1.70)
                                        </label>
                                    </div>

                                    {validationError && <p className="bulk-ship__order-validation" role="alert">{validationError}</p>}

                                    <footer className="bulk-ship__order-status">
                                        {status.state === "idle" && <span className="bulk-ship__status">Ready</span>}
                                        {status.state === "processing" && <span className="bulk-ship__status">Creating label…</span>}
                                        {status.state === "success" && (
                                            <>
                                                <span className="bulk-ship__status bulk-ship__status--success">Label ready</span>
                                                <button type="button" className="bulk-ship__link" onClick={() => handleViewLabel(order.id)}>
                                                    View label
                                                </button>
                                            </>
                                        )}
                                        {status.state === "error" && (
                                            <span className="bulk-ship__status bulk-ship__status--error">{status.message || "Unable to create label."}</span>
                                        )}
                                    </footer>
                                </article>
                            );
                        })}

                        {orders.length === 0 && !ordersLoading && (
                            <div className="bulk-ship__empty-orders">
                                <p>We could not load any order details. Try returning to the orders list and selecting them again.</p>
                            </div>
                        )}
                    </section>

                    <aside className="bulk-ship__summary-panel">
                        <div className="bulk-ship__summary-header">
                            <span>Total</span>
                            <strong>{formatCurrency(estimatedTotal)}</strong>
                        </div>

                        <div className="bulk-ship__summary-row">
                            <span>Orders selected</span>
                            <span>{orders.length}</span>
                        </div>

                        <label className="bulk-ship__summary-field">
                            <span>Ship date</span>
                            <input type="date" value={shipDate} onChange={handleShipDateChange} />
                        </label>

                        <label className="bulk-ship__summary-field">
                            <span>Paper size</span>
                            <select value={paperSize} onChange={handlePaperSizeChange}>
                                {LABEL_PAPER_SIZES.map((option) => (
                                    <option key={option.value} value={option.value}>
                                        {option.label}
                                    </option>
                                ))}
                            </select>
                        </label>

                        <div className="bulk-ship__summary-payment">
                            <span className="bulk-ship__summary-label">Payment method</span>
                            <div className="bulk-ship__summary-options">
                                {PAYMENT_METHODS.map((method) => (
                                    <label key={method.value} className={`bulk-ship__summary-radio ${paymentMethod === method.value ? "is-active" : ""}`}>
                                        <input
                                            type="radio"
                                            name="payment-method"
                                            value={method.value}
                                            checked={paymentMethod === method.value}
                                            onChange={handlePaymentMethodChange}
                                        />
                                        {method.label}
                                    </label>
                                ))}
                            </div>
                        </div>

                        {submissionState.error && (
                            <div className="bulk-ship__summary-alert" role="alert">
                                <strong>Heads up</strong>
                                <p>{submissionState.error}</p>
                            </div>
                        )}

                        <div className="bulk-ship__summary-actions">
                            <button type="button" className="bulk-ship__secondary" onClick={handleBack} disabled={submissionState.isSubmitting}>
                                Cancel
                            </button>
                            <button type="button" className="bulk-ship__primary" onClick={handleOpenReview} disabled={disableSubmit}>
                                {submissionState.isSubmitting
                                    ? "Processing labels…"
                                    : `Confirm and pay ${formatCurrency(estimatedTotal)}`}
                            </button>
                        </div>

                        <div className="bulk-ship__summary-results">
                            <div>
                                <span>Labels created</span>
                                <strong>{submissionState.successCount}</strong>
                            </div>
                            <div>
                                <span>Failed</span>
                                <strong>{submissionState.failureCount}</strong>
                            </div>
                        </div>
                    </aside>
                </div>
            </main>

            <Dialog
                isOpen={reviewState.isOpen}
                title="Review and pay"
                subtitle="Pay now"
                onClose={handleCloseReview}
                footer={(
                    <div className="bulk-ship__review-footer">
                        <button type="button" className="bulk-ship__secondary" onClick={handleCloseReview}>
                            Cancel
                        </button>
                        <button type="button" className="bulk-ship__primary" onClick={handleConfirmPurchase}>
                            Confirm and pay {formatCurrency(reviewState.total)}
                        </button>
                    </div>
                )}
            >
                <div className="bulk-ship__review">
                    <aside className="bulk-ship__review-payment">
                        <div className="bulk-ship__review-total">
                            <span>USPS</span>
                            <strong>{formatCurrency(reviewState.total)}</strong>
                        </div>
                        <div className="bulk-ship__review-methods" role="radiogroup" aria-label="Payment method">
                            {PAYMENT_METHODS.map((method) => (
                                <label key={method.value} className={`bulk-ship__review-radio ${paymentMethod === method.value ? "is-active" : ""}`}>
                                    <input
                                        type="radio"
                                        name="review-payment-method"
                                        value={method.value}
                                        checked={paymentMethod === method.value}
                                        onChange={handlePaymentMethodChange}
                                    />
                                    {method.label}
                                </label>
                            ))}
                        </div>
                        <p className="bulk-ship__review-note">
                            When you click "Confirm and pay", the total label cost will be charged to your selected payment method. You will be able to print your shipping labels right away.
                        </p>
                        <p className="bulk-ship__review-paper">Paper size: {LABEL_PAPER_SIZES.find((size) => size.value === paperSize)?.label ?? '4" x 6"'}</p>
                    </aside>

                    <section className="bulk-ship__review-orders">
                        {reviewState.orders.map((entry) => (
                            <article key={entry.order.id} className="bulk-ship__review-order">
                                <header>
                                    <h3>{entry.order.orderNumber || entry.order.id.slice(0, 8)}</h3>
                                    <span>{entry.service?.name}</span>
                                </header>
                                <dl>
                                    <div>
                                        <dt>Ship to</dt>
                                        <dd>{entry.order.shipToName || buyerDisplayName(entry.order)}</dd>
                                    </div>
                                    <div>
                                        <dt>Weight</dt>
                                        <dd>
                                            {parseDecimal(entry.form.weightLb)} lb {parseDecimal(entry.form.weightOz)} oz
                                        </dd>
                                    </div>
                                    <div>
                                        <dt>Dimensions</dt>
                                        <dd>
                                            {parseDecimal(entry.form.lengthIn)} × {parseDecimal(entry.form.widthIn)} × {parseDecimal(entry.form.heightIn)} in
                                        </dd>
                                    </div>
                                    <div>
                                        <dt>Price</dt>
                                        <dd>{formatCurrency(entry.service?.price ?? 0)}</dd>
                                    </div>
                                </dl>
                            </article>
                        ))}
                    </section>
                </div>
            </Dialog>
        </div>
    );
};

export default BulkShipOrdersPage;
