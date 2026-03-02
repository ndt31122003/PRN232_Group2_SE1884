import React, { useEffect, useMemo, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import dayjs from "dayjs";
import OrderService from "../../services/OrderService";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import "./OrderDetailPage.scss";

const PRICE_FORMATTER = new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
});

const TIMELINE_STEPS = [
    { key: "buyer-paid", label: "Buyer paid" },
    { key: "ready", label: "Ready to ship" },
    { key: "authentication", label: "Authentication" },
    { key: "delivery", label: "Delivery" },
];

const normalizeStatusHistory = (history) => ({
    fromStatusCode: history?.fromStatusCode ?? history?.FromStatusCode ?? "",
    fromStatusName: history?.fromStatusName ?? history?.FromStatusName ?? "",
    toStatusCode: history?.toStatusCode ?? history?.ToStatusCode ?? "",
    toStatusName: history?.toStatusName ?? history?.ToStatusName ?? "",
    changedAt: history?.changedAt ?? history?.ChangedAt ?? null,
});

const normalizeItem = (item) => ({
    listingId: item?.listingId ?? item?.ListingId ?? "",
    variationId: item?.variationId ?? item?.VariationId ?? null,
    title: item?.title ?? item?.Title ?? "Untitled item",
    imageUrl: item?.imageUrl ?? item?.ImageUrl ?? "",
    sku: item?.sku ?? item?.Sku ?? "",
    quantity: item?.quantity ?? item?.Quantity ?? 0,
    unitPrice: item?.unitPrice ?? item?.UnitPrice ?? null,
    totalPrice: item?.totalPrice ?? item?.TotalPrice ?? null,
});

const normalizeOrderDetail = (order) => ({
    id: order?.id ?? order?.Id ?? "",
    orderNumber: order?.orderNumber ?? order?.OrderNumber ?? "",
    buyerFullName: order?.buyerFullName ?? order?.BuyerFullName ?? "",
    buyerUsername: order?.buyerUsername ?? order?.BuyerUsername ?? "",
    subTotal: order?.subTotal ?? order?.SubTotal ?? null,
    shippingCost: order?.shippingCost ?? order?.ShippingCost ?? null,
    platformFee: order?.platformFee ?? order?.PlatformFee ?? null,
    taxAmount: order?.taxAmount ?? order?.TaxAmount ?? null,
    discountAmount: order?.discountAmount ?? order?.DiscountAmount ?? null,
    total: order?.total ?? order?.Total ?? null,
    statusCode: order?.statusCode ?? order?.StatusCode ?? "",
    statusName: order?.statusName ?? order?.StatusName ?? "",
    statusColor: order?.statusColor ?? order?.StatusColor ?? "#0f172a",
    shippingStatus: order?.shippingStatus ?? order?.ShippingStatus ?? "",
    fulfillmentType: order?.fulfillmentType ?? order?.FulfillmentType ?? "",
    orderedAt: order?.orderedAt ?? order?.OrderedAt ?? null,
    paidAt: order?.paidAt ?? order?.PaidAt ?? null,
    shippedAt: order?.shippedAt ?? order?.ShippedAt ?? null,
    deliveredAt: order?.deliveredAt ?? order?.DeliveredAt ?? null,
    cancelledAt: order?.cancelledAt ?? order?.CancelledAt ?? null,
    archivedAt: order?.archivedAt ?? order?.ArchivedAt ?? null,
    shipToName: order?.shipToName ?? order?.ShipToName ?? "",
    shipToAddress: order?.shipToAddress ?? order?.ShipToAddress ?? "",
    shipToPhone: order?.shipToPhone ?? order?.ShipToPhone ?? "",
    shipToEmail: order?.shipToEmail ?? order?.ShipToEmail ?? "",
    statusHistory: Array.isArray(order?.statusHistory ?? order?.StatusHistory)
        ? (order?.statusHistory ?? order?.StatusHistory).map(normalizeStatusHistory)
        : [],
    items: Array.isArray(order?.items ?? order?.Items)
        ? (order?.items ?? order?.Items).map(normalizeItem)
        : [],
});

const formatCurrency = (value) => {
    if (value === null || value === undefined) {
        return "-";
    }
    return PRICE_FORMATTER.format(value);
};

const formatDate = (value) => {
    if (!value) {
        return "-";
    }
    const parsed = dayjs(value);
    return parsed.isValid() ? parsed.format("MMM D, YYYY") : "-";
};

const formatDateRange = (start, end) => {
    const startDate = dayjs(start);
    const endDate = dayjs(end);
    if (!startDate.isValid() || !endDate.isValid()) {
        return "";
    }
    return `${startDate.format("MMM D, YYYY")} - ${endDate.format("MMM D, YYYY")}`;
};

const formatStatusText = (value) => {
    if (!value) {
        return "";
    }
    const result = String(value)
        .replace(/_/g, " ")
        .replace(/([a-z0-9])([A-Z])/g, "$1 $2")
        .replace(/\s+/g, " ")
        .trim()
        .toLowerCase();
    if (!result) {
        return "";
    }
    return result.replace(/(^|\s)[a-z]/g, (match) => match.toUpperCase());
};

const mapShippingStatusToStep = (shippingStatus, statusCode) => {
    const normalized = String(shippingStatus ?? "").toLowerCase();
    switch (normalized) {
        case "pending":
            return 0;
        case "labelcreated":
        case "label created":
            return 1;
        case "shipped":
        case "intransit":
        case "outfordelivery":
        case "out for delivery":
            return 2;
        case "delivered":
            return 3;
        default: {
            const status = String(statusCode ?? "").toLowerCase();
            if (status.includes("awaiting")) {
                return 1;
            }
            if (status.includes("shipped")) {
                return 2;
            }
            if (status.includes("delivered")) {
                return 3;
            }
            return 0;
        }
    }
};

const buildAddressLines = (address) => {
    if (!address) {
        return [];
    }
    if (Array.isArray(address)) {
        return address
            .map((line) => String(line ?? "").trim())
            .filter((line) => line.length > 0);
    }
    return String(address)
        .split(/\r?\n/)
        .map((line) => line.trim())
        .filter((line) => line.length > 0);
};

const DEFAULT_BACK_PATH = "/order/all?status=all";

const OrderDetailPage = () => {
    const { orderId } = useParams();
    const navigate = useNavigate();
    const location = useLocation();

    const orderFromState = location.state?.order ?? null;
    const fromPath = location.state?.from ?? DEFAULT_BACK_PATH;

    const [order, setOrder] = useState(() => (orderFromState ? normalizeOrderDetail(orderFromState) : null));
    const [loading, setLoading] = useState(!orderFromState);
    const [error, setError] = useState("");
    const [reloadKey, setReloadKey] = useState(0);

    useEffect(() => {
        if (!orderId) {
            setError("Order identifier is missing.");
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
                setOrder(normalizeOrderDetail(data));
            } catch (err) {
                if (controller.signal.aborted) {
                    return;
                }
                const message = err?.response?.data?.detail ?? err?.response?.data?.message ?? err?.message;
                setError(message || "Unable to load order details. Please try again later.");
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
    }, [orderId, reloadKey]);

    const primaryItem = useMemo(() => {
        if (!order?.items || order.items.length === 0) {
            return null;
        }
        return order.items[0];
    }, [order]);

    const itemCount = useMemo(() => {
        if (!order?.items) {
            return 0;
        }
        return order.items.reduce((total, item) => total + (Number(item.quantity) || 0), 0);
    }, [order]);

    const shippingStepIndex = useMemo(
        () => mapShippingStatusToStep(order?.shippingStatus, order?.statusCode),
        [order?.shippingStatus, order?.statusCode]
    );

    const timelineSteps = useMemo(
        () =>
            TIMELINE_STEPS.map((step, index) => {
                const state = index < shippingStepIndex ? "complete" : index === shippingStepIndex ? "current" : "upcoming";
                let caption = "";
                if (step.key === "buyer-paid" && order?.paidAt) {
                    caption = `Paid ${formatDate(order.paidAt)}`;
                } else if (step.key === "ready" && order?.shippedAt) {
                    caption = `Label created ${formatDate(order.shippedAt)}`;
                } else if (step.key === "delivery" && order?.deliveredAt) {
                    caption = `Delivered ${formatDate(order.deliveredAt)}`;
                }
                return {
                    ...step,
                    state,
                    caption,
                };
            }),
        [order?.paidAt, order?.shippedAt, order?.deliveredAt, shippingStepIndex]
    );

    const estimatedDeliveryWindow = useMemo(() => {
        if (!order?.paidAt) {
            return "";
        }
        const start = dayjs(order.paidAt).add(5, "day");
        const end = dayjs(order.paidAt).add(10, "day");
        return formatDateRange(start, end);
    }, [order?.paidAt]);

    const pricingLines = useMemo(() => {
        if (!order) {
            return [];
        }
        const lines = [
            { key: "subTotal", label: "Item subtotal", value: order.subTotal },
            { key: "shippingCost", label: "Shipping", value: order.shippingCost },
            { key: "taxAmount", label: "Sales tax", value: order.taxAmount },
        ];
        if (order.discountAmount) {
            lines.push({ key: "discountAmount", label: "Discounts", value: -Math.abs(order.discountAmount) });
        }
        return lines.filter((line) => line.value !== null && line.value !== undefined);
    }, [order]);

    const sellerBreakdown = useMemo(() => {
        if (!order) {
            return [];
        }
        const entries = [{ key: "orderTotal", label: "Order total", value: order.total }];
        if (order.platformFee) {
            entries.push({ key: "platformFee", label: "Platform fees", value: -Math.abs(order.platformFee) });
        }
        if (order.discountAmount) {
            entries.push({ key: "discounts", label: "Discounts", value: -Math.abs(order.discountAmount) });
        }
        const earningsBase = (order.total ?? 0) - (order.platformFee ?? 0) - (order.discountAmount ?? 0);
        entries.push({ key: "earnings", label: "Estimated earnings", value: earningsBase, isTotal: true });
        return entries;
    }, [order]);

    const shippingAddressLines = useMemo(() => buildAddressLines(order?.shipToAddress), [order?.shipToAddress]);

    const buyerDisplayName = order?.buyerFullName || order?.buyerUsername || "Unknown buyer";

    const fundStatusLabel = (() => {
        if (!order?.statusCode) {
            return "Pending";
        }
        if (order.statusCode.toLowerCase().includes("awaiting")) {
            return "On hold";
        }
        if (order.statusCode.toLowerCase().includes("delivered")) {
            return "Available";
        }
        return "Processing";
    })();

    const handleBack = () => {
        navigate(fromPath, { replace: false });
    };

    const handleRetry = () => {
        setReloadKey((prev) => prev + 1);
    };

    const handleShip = () => {
        if (!order?.id) {
            return;
        }
        navigate(`/order/ship/${order.id}`, {
            state: { order, from: `/order/detail/${order.id}` },
        });
    };

    const handleCancelFlow = () => {
        if (!order?.id) {
            return;
        }
        navigate(`/order/cancel/${order.id}`, {
            state: { order, from: `/order/detail/${order.id}` },
        });
    };

    if (!order && loading) {
        return (
            <div className="order-detail order-detail--loading">
                <LoadingScreen />
            </div>
        );
    }

    return (
        <div className="order-detail">
            <div className="order-detail__toolbar">
                <button type="button" className="order-detail__back-button" onClick={handleBack}>
                    <span aria-hidden="true">&#8592;</span>
                    <span>Back to orders</span>
                </button>
            </div>

            <header className="order-detail__masthead">
                <div className="order-detail__masthead-info">
                    <div className="order-detail__product">
                        <div className="order-detail__product-thumb">
                            {primaryItem?.imageUrl ? (
                                <img src={primaryItem.imageUrl} alt={primaryItem.title} loading="lazy" />
                            ) : (
                                <span className="order-detail__product-placeholder">No image</span>
                            )}
                        </div>
                        <div className="order-detail__product-content">
                            <span className="order-detail__product-label">Order details</span>
                            <h1 className="order-detail__product-title">
                                {primaryItem?.title || `Order ${order?.orderNumber ?? ""}`}
                            </h1>
                            <div className="order-detail__product-meta">
                                <span>Order {order?.orderNumber || "-"}</span>
                                {order?.statusName && (
                                    <span className="order-detail__pill" style={{ backgroundColor: order.statusColor || "#0f172a" }}>
                                        {order.statusName}
                                    </span>
                                )}
                                {order?.shippingStatus && (
                                    <span className="order-detail__pill order-detail__pill--neutral">
                                        {formatStatusText(order.shippingStatus)}
                                    </span>
                                )}
                            </div>
                        </div>
                    </div>
                    <div className="order-detail__product-stats">
                        <div className="order-detail__stat">
                            <span className="order-detail__stat-label">Ordered</span>
                            <span className="order-detail__stat-value">{formatDate(order?.orderedAt)}</span>
                        </div>
                        <div className="order-detail__stat">
                            <span className="order-detail__stat-label">Paid</span>
                            <span className="order-detail__stat-value">
                                {order?.paidAt ? formatDate(order.paidAt) : "Not yet"}
                            </span>
                        </div>
                        <div className="order-detail__stat">
                            <span className="order-detail__stat-label">Items</span>
                            <span className="order-detail__stat-value">{itemCount}</span>
                        </div>
                    </div>
                </div>
                <div className="order-detail__masthead-actions">
                    <button type="button" className="order-detail__button order-detail__button--primary" onClick={handleShip}>
                        Purchase shipping label
                    </button>
                    <button type="button" className="order-detail__button order-detail__button--ghost" disabled>
                        Print packing slip
                    </button>
                </div>
            </header>

            {error && (
                <div className="order-detail__alert">
                    <span>{error}</span>
                    <button type="button" className="order-detail__alert-action" onClick={handleRetry}>
                        Retry
                    </button>
                </div>
            )}

            <div className="order-detail__grid">
                <div className="order-detail__column order-detail__column--primary">
                    <section className="order-detail__card order-detail__card--progress">
                        <div className="order-detail__card-header">
                            <div>
                                <h2 className="order-detail__card-title">Ready to ship</h2>
                                <p className="order-detail__card-subtitle">
                                    Make sure you ship the order within the handling time you specified in the listing.
                                </p>
                            </div>
                            <div className="order-detail__delivery-window">
                                <span className="order-detail__delivery-label">Estimated delivery window</span>
                                <span className="order-detail__delivery-value">
                                    {estimatedDeliveryWindow || "Not available"}
                                </span>
                            </div>
                        </div>
                        <div className="order-detail__progress">
                            {timelineSteps.map((step, index) => (
                                <div key={step.key} className={`order-detail__progress-step order-detail__progress-step--${step.state}`}>
                                    <div className="order-detail__progress-marker">
                                        {step.state === "complete" ? (
                                            <svg width="16" height="16" viewBox="0 0 16 16" aria-hidden="true">
                                                <path
                                                    d="M6.667 10.114L4.553 8l-.94.94 3.054 3.054 6-6L11.727 5l-5.06 5.114z"
                                                    fill="currentColor"
                                                />
                                            </svg>
                                        ) : (
                                            <span>{index + 1}</span>
                                        )}
                                    </div>
                                    <div className="order-detail__progress-text">
                                        <span className="order-detail__progress-label">{step.label}</span>
                                        {step.caption && (
                                            <span className="order-detail__progress-caption">{step.caption}</span>
                                        )}
                                    </div>
                                </div>
                            ))}
                        </div>
                    </section>

                    <section className="order-detail__card order-detail__card--shipping">
                        <div className="order-detail__card-header">
                            <h2 className="order-detail__card-title">Shipping</h2>
                            <button type="button" className="order-detail__button order-detail__button--tertiary" disabled>
                                Add tracking
                            </button>
                        </div>
                        <div className="order-detail__shipping-grid">
                            <div className="order-detail__shipping-block">
                                <span className="order-detail__shipping-label">Ship to</span>
                                <div className="order-detail__shipping-value">
                                    {order?.shipToName && <div>{order.shipToName}</div>}
                                    {shippingAddressLines.length > 0 ? (
                                        shippingAddressLines.map((line, index) => (
                                            <div key={`${line}-${index}`}>{line}</div>
                                        ))
                                    ) : (
                                        <div>Shipping address available after confirmation.</div>
                                    )}
                                    {order?.shipToPhone && <div>Phone: {order.shipToPhone}</div>}
                                    {order?.shipToEmail && <div>Email: {order.shipToEmail}</div>}
                                </div>
                            </div>
                            <div className="order-detail__shipping-block">
                                <span className="order-detail__shipping-label">Shipping service</span>
                                <span className="order-detail__shipping-value order-detail__shipping-value--muted">
                                    Select a service when purchasing a label
                                </span>
                            </div>
                            <div className="order-detail__shipping-block">
                                <span className="order-detail__shipping-label">Tracking</span>
                                <span className="order-detail__shipping-value order-detail__shipping-value--muted">
                                    No tracking yet
                                </span>
                            </div>
                        </div>
                    </section>

                    <section className="order-detail__card order-detail__card--items">
                        <div className="order-detail__card-header">
                            <h2 className="order-detail__card-title">Items</h2>
                            <span className="order-detail__chip order-detail__chip--secondary">{itemCount} item(s)</span>
                        </div>
                        {order?.items?.length ? (
                            <table className="order-detail__items-table">
                                <thead>
                                    <tr>
                                        <th scope="col">Item</th>
                                        <th scope="col">Quantity</th>
                                        <th scope="col">Price</th>
                                        <th scope="col">Total</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {order.items.map((item, index) => (
                                        <tr key={`${item.listingId}-${index}`}>
                                            <td>
                                                <div className="order-detail__item-row">
                                                    <div className="order-detail__item-thumb">
                                                        {item.imageUrl ? (
                                                            <img src={item.imageUrl} alt={item.title} loading="lazy" />
                                                        ) : (
                                                            <span>No image</span>
                                                        )}
                                                    </div>
                                                    <div className="order-detail__item-text">
                                                        <span className="order-detail__item-name">{item.title || "Untitled item"}</span>
                                                        {item.sku && (
                                                            <span className="order-detail__item-meta">SKU: {item.sku}</span>
                                                        )}
                                                    </div>
                                                </div>
                                            </td>
                                            <td>{item.quantity}</td>
                                            <td>{item.unitPrice !== null ? formatCurrency(item.unitPrice) : "-"}</td>
                                            <td>{item.totalPrice !== null ? formatCurrency(item.totalPrice) : "-"}</td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        ) : (
                            <div className="order-detail__empty-card">No items were found for this order.</div>
                        )}
                    </section>
                </div>

                <aside className="order-detail__column order-detail__column--aside">
                    <section className="order-detail__card order-detail__card--order-info">
                        <div className="order-detail__card-header">
                            <h2 className="order-detail__card-title">Order</h2>
                        </div>
                        <dl className="order-detail__description-list">
                            <div>
                                <dt>Order no.</dt>
                                <dd>{order?.orderNumber || "-"}</dd>
                            </div>
                            <div>
                                <dt>Date sold</dt>
                                <dd>{formatDate(order?.orderedAt)}</dd>
                            </div>
                            <div>
                                <dt>Date paid</dt>
                                <dd>{order?.paidAt ? formatDate(order.paidAt) : "Not yet"}</dd>
                            </div>
                            <div>
                                <dt>Buyer</dt>
                                <dd>
                                    {buyerDisplayName}
                                    {order?.buyerUsername && buyerDisplayName !== `@${order.buyerUsername}` && (
                                        <span className="order-detail__muted"> @{order.buyerUsername}</span>
                                    )}
                                </dd>
                            </div>
                        </dl>
                        <button type="button" className="order-detail__button order-detail__button--secondary" disabled>
                            Contact buyer
                        </button>
                    </section>

                    <section className="order-detail__card order-detail__card--payment">
                        <div className="order-detail__card-header">
                            <h2 className="order-detail__card-title">Payment</h2>
                            <span className="order-detail__chip order-detail__chip--primary">{fundStatusLabel}</span>
                        </div>
                        <div className="order-detail__payment-section">
                            <h3 className="order-detail__payment-heading">What your buyer paid</h3>
                            <ul className="order-detail__payment-list">
                                {pricingLines.map((line) => (
                                    <li key={line.key}>
                                        <span>{line.label}</span>
                                        <span>{formatCurrency(line.value)}</span>
                                    </li>
                                ))}
                                <li className="order-detail__payment-total">
                                    <span>Order total</span>
                                    <span>{formatCurrency(order?.total)}</span>
                                </li>
                            </ul>
                        </div>
                        <div className="order-detail__payment-section">
                            <h3 className="order-detail__payment-heading">What you earned</h3>
                            <ul className="order-detail__payment-list">
                                {sellerBreakdown.map((entry) => (
                                    <li key={entry.key} className={entry.isTotal ? "order-detail__payment-total" : undefined}>
                                        <span>{entry.label}</span>
                                        <span>{formatCurrency(entry.value)}</span>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    </section>

                    <section className="order-detail__card order-detail__card--actions">
                        <div className="order-detail__card-header">
                            <h2 className="order-detail__card-title">Quick actions</h2>
                        </div>
                        <div className="order-detail__actions">
                            <button type="button" className="order-detail__button order-detail__button--primary" onClick={handleShip}>
                                Purchase shipping label
                            </button>
                            <button type="button" className="order-detail__button order-detail__button--secondary" onClick={handleCancelFlow}>
                                Open cancellation flow
                            </button>
                        </div>
                    </section>
                </aside>
            </div>

            {loading && order && (
                <div className="order-detail__overlay">
                    <LoadingScreen isOverlay={true} sizeSmall={true} />
                </div>
            )}
        </div>
    );
};

export default OrderDetailPage;
