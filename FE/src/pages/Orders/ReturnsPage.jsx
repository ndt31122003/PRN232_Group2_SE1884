import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import OrderService from "../../services/OrderService";
import "./ResolutionCenterPage.scss";

const STATUS_OPTIONS = [
    { value: "open-needs-attention", label: "Open returns - needs attention" },
    { value: "open-returns-replacements", label: "Open returns/replacements" },
    { value: "open-replacements", label: "Open replacements" },
    { value: "open-returns", label: "Open returns" },
    { value: "in-progress", label: "Returns in progress" },
    { value: "shipped", label: "Returns shipped" },
    { value: "delivered", label: "Returns delivered" },
    { value: "closed", label: "Closed returns/replacements" },
];

const PERIOD_OPTIONS = [
    { value: "last30", label: "Last 30 days" },
    { value: "last90", label: "Last 90 days" },
    { value: "last180", label: "Last 6 months" },
    { value: "thisYear", label: "This year" },
    { value: "custom", label: "Custom range" },
];

const SEARCH_OPTIONS = [
    { value: "buyer", label: "Buyer username" },
    { value: "order", label: "Order number" },
    { value: "item", label: "Item title" },
    { value: "tracking", label: "Tracking number" },
];

const SORT_OPTIONS = [
    { value: "date-desc", label: "Date requested" },
    { value: "date-asc", label: "Date requested (oldest)" },
    { value: "status", label: "Status" },
    { value: "buyer", label: "Buyer" },
    { value: "due-date", label: "Buyer return due date" },
];

const PAGE_SIZE_OPTIONS = [25, 50, 100, 200];

const DEFAULT_FORM_STATE = {
    status: STATUS_OPTIONS[1].value,
    period: PERIOD_OPTIONS[1].value,
    searchBy: SEARCH_OPTIONS[0].value,
    keyword: "",
    sortBy: SORT_OPTIONS[0].value,
    perPage: 50,
};

const DEFAULT_FILTERS = {
    ...DEFAULT_FORM_STATE,
    pageNumber: 1,
};

const RETURN_STATUS_MAP = {
    "open-needs-attention": "NeedsAttention",
    "open-returns-replacements": "OpenReturnsReplacements",
    "open-replacements": "OpenReplacements",
    "open-returns": "OpenReturns",
    "in-progress": "InProgress",
    shipped: "Shipped",
    delivered: "Delivered",
    closed: "Closed",
};

const PERIOD_MAP = {
    last30: "Last30Days",
    last90: "Last90Days",
    last180: "Last180Days",
    thisYear: "ThisYear",
    custom: "Custom",
};

const RETURN_SEARCH_MAP = {
    buyer: "BuyerUsername",
    order: "OrderNumber",
    item: "ItemTitle",
    tracking: "TrackingNumber",
};

const RETURN_SORT_MAP = {
    "date-desc": { sortBy: "DateRequested", sortDescending: true },
    "date-asc": { sortBy: "DateRequested", sortDescending: false },
    status: { sortBy: "ReturnStatus", sortDescending: true },
    buyer: { sortBy: "Buyer", sortDescending: false },
    "due-date": { sortBy: "DueDate", sortDescending: false },
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

const buildReturnQuery = (filters) => {
    const sortConfig = RETURN_SORT_MAP[filters.sortBy] ?? RETURN_SORT_MAP["date-desc"];
    const trimmedKeyword = filters.keyword?.trim();

    const params = {
        status: RETURN_STATUS_MAP[filters.status] ?? RETURN_STATUS_MAP["open-returns-replacements"],
        period: PERIOD_MAP[filters.period] ?? PERIOD_MAP.last90,
        sortBy: sortConfig.sortBy,
        sortDescending: sortConfig.sortDescending,
        pageNumber: filters.pageNumber ?? 1,
        pageSize: filters.perPage,
    };

    if (trimmedKeyword) {
        params.keyword = trimmedKeyword;
        params.searchBy = RETURN_SEARCH_MAP[filters.searchBy] ?? RETURN_SEARCH_MAP.buyer;
    }

    return params;
};

const ReturnsPage = () => {
    const [filters, setFilters] = useState(DEFAULT_FILTERS);
    const [formState, setFormState] = useState(DEFAULT_FORM_STATE);
    const [records, setRecords] = useState([]);
    const [totalCount, setTotalCount] = useState(0);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [actionMessage, setActionMessage] = useState(null);
    const [actionError, setActionError] = useState(null);

    useEffect(() => {
        const controller = new AbortController();
        let isCancelled = false;

        const loadReturns = async () => {
            setIsLoading(true);
            setError(null);

            try {
                const params = buildReturnQuery(filters);
                const response = await OrderService.getReturnRequests(params, controller.signal);
                const payload = response?.data ?? {};

                if (isCancelled) {
                    return;
                }

                const items = Array.isArray(payload.items) ? payload.items : [];
                setRecords(items);
                setTotalCount(payload.totalCount ?? items.length ?? 0);
            } catch (fetchError) {
                if (controller.signal.aborted || isCancelled) {
                    return;
                }

                setError("Unable to load return requests.");
                setRecords([]);
                setTotalCount(0);
            } finally {
                if (!isCancelled && !controller.signal.aborted) {
                    setIsLoading(false);
                }
            }
        };

        loadReturns();

        return () => {
            isCancelled = true;
            controller.abort();
        };
    }, [filters]);

    const handleFieldChange = (field) => (event) => {
        const value = field === "perPage" ? Number(event.target.value) : event.target.value;
        setFormState((prev) => ({ ...prev, [field]: value }));
    };

    const handleStatusSelect = (status) => {
        setActionMessage(null);
        setActionError(null);
        setFormState((prev) => ({ ...prev, status }));
        setFilters((prev) => ({ ...prev, status, pageNumber: 1 }));
    };

    const applyFilters = (event) => {
        event.preventDefault();
        setActionMessage(null);
        setActionError(null);
        setFilters({ ...formState, pageNumber: 1 });
    };

    const resetFilters = () => {
        setActionMessage(null);
        setActionError(null);
        setFormState(DEFAULT_FORM_STATE);
        setFilters(DEFAULT_FILTERS);
    };

    const handleSortChange = (event) => {
        const value = event.target.value;
        setFormState((prev) => ({ ...prev, sortBy: value }));
        setFilters((prev) => ({ ...prev, sortBy: value, pageNumber: 1 }));
    };

    const handlePerPageChange = (event) => {
        const value = Number(event.target.value);
        setFormState((prev) => ({ ...prev, perPage: value }));
        setFilters((prev) => ({ ...prev, perPage: value, pageNumber: 1 }));
    };

    const navigate = useNavigate();
    const location = useLocation();

    useEffect(() => {
        if (location.state?.flashMessage || location.state?.flashError) {
            setActionMessage(location.state.flashMessage ?? null);
            setActionError(location.state.flashError ?? null);
            navigate(location.pathname + location.search, { replace: true });
        }
    }, [location, navigate]);

    const handleApproveReturn = (record) => {
        if (!record?.requestId) {
            return;
        }

        setActionMessage(null);
        setActionError(null);
        navigate(`/order/returns/${record.requestId}/approve`, { state: { record } });
    };

    const handleRejectReturn = (record) => {
        if (!record?.requestId) {
            return;
        }

        setActionMessage(null);
        setActionError(null);
        navigate(`/order/returns/${record.requestId}/decline`, { state: { record } });
    };

    return (
        <div className="resolution-page">
            <header className="resolution-page__header">
                <div>
                    <h1 className="resolution-page__title">Manage return requests</h1>
                    <p className="resolution-page__subtitle">
                        Review all open and closed return or replacement requests from your buyers.
                    </p>
                </div>
                <a
                    href="https://www.ebay.com/sellerhelp/suggestions"
                    className="resolution-page__comment-link"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    Send us your comments
                </a>
            </header>

            <section className="resolution-page__filters">
                <div className="resolution-page__filters-row">
                    <div className="resolution-page__field">
                        <span className="resolution-page__label">Status:</span>
                        <div className="resolution-page__segment-group">
                            {STATUS_OPTIONS.map((option) => (
                                <button
                                    key={option.value}
                                    type="button"
                                    className={
                                        option.value === formState.status
                                            ? "resolution-page__segment-button resolution-page__segment-button--active"
                                            : "resolution-page__segment-button"
                                    }
                                    onClick={() => handleStatusSelect(option.value)}
                                >
                                    {option.label}
                                </button>
                            ))}
                        </div>
                    </div>
                </div>
                <form className="resolution-page__filters-row" onSubmit={applyFilters}>
                    <div className="resolution-page__field">
                        <label className="resolution-page__label" htmlFor="returns-period">
                            Period:
                        </label>
                        <select
                            id="returns-period"
                            value={formState.period}
                            onChange={handleFieldChange("period")}
                        >
                            {PERIOD_OPTIONS.map((option) => (
                                <option key={option.value} value={option.value}>
                                    {option.label}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className="resolution-page__field resolution-page__field--grow">
                        <label className="resolution-page__label" htmlFor="returns-search-by">
                            Search by:
                        </label>
                        <div className="resolution-page__search">
                            <select
                                id="returns-search-by"
                                value={formState.searchBy}
                                onChange={handleFieldChange("searchBy")}
                            >
                                {SEARCH_OPTIONS.map((option) => (
                                    <option key={option.value} value={option.value}>
                                        {option.label}
                                    </option>
                                ))}
                            </select>
                            <input
                                type="text"
                                placeholder="Enter your search keyword"
                                value={formState.keyword}
                                onChange={handleFieldChange("keyword")}
                            />
                        </div>
                    </div>
                    <div className="resolution-page__actions">
                        <button type="submit" className="resolution-page__primary">
                            Search
                        </button>
                        <button type="button" className="resolution-page__secondary" onClick={resetFilters}>
                            Reset
                        </button>
                    </div>
                </form>
            </section>

            {(actionMessage || actionError) && (
                <div
                    className={`resolution-page__alert ${actionError ? "resolution-page__alert--error" : "resolution-page__alert--success"
                        }`}
                >
                    {actionError ?? actionMessage}
                </div>
            )}

            <section className="resolution-page__results">
                <div className="resolution-page__results-summary">
                    <span>Results: {totalCount}</span>
                </div>
                <div className="resolution-page__sort">
                    <label htmlFor="returns-sort">Sort by:</label>
                    <select id="returns-sort" value={filters.sortBy} onChange={handleSortChange}>
                        {SORT_OPTIONS.map((option) => (
                            <option key={option.value} value={option.value}>
                                {option.label}
                            </option>
                        ))}
                    </select>
                </div>
            </section>

            <section className="resolution-page__table">
                <table>
                    <thead>
                        <tr>
                            <th>Action</th>
                            <th>Items</th>
                            <th>Status</th>
                            <th>Details</th>
                            <th>Refund</th>
                            <th>Buyer</th>
                        </tr>
                    </thead>
                    <tbody>
                        {isLoading ? (
                            <tr>
                                <td colSpan={6} className="resolution-page__empty">
                                    Loading return requests...
                                </td>
                            </tr>
                        ) : error ? (
                            <tr>
                                <td colSpan={6} className="resolution-page__empty">
                                    {error}
                                </td>
                            </tr>
                        ) : records.length === 0 ? (
                            <tr>
                                <td colSpan={6} className="resolution-page__empty">
                                    We didn\'t find any results. Try searching with different criteria.
                                </td>
                            </tr>
                        ) : (
                            records.map((record) => {
                                const refund = formatCurrencyLabel(record.refundAmount, record.refundCurrency);
                                const isOrderPaid = record?.isOrderPaid ?? true;
                                const restockingFeeLabel = record?.restockingFeeAmount != null
                                    ? formatCurrencyLabel(record.restockingFeeAmount, record.restockingFeeCurrency ?? record.orderTotalCurrency)
                                    : null;
                                const estimatedRefund = !refund && isOrderPaid && record?.orderTotalAmount != null && record.orderTotalCurrency
                                    ? (() => {
                                        const total = Number(record.orderTotalAmount);
                                        if (Number.isNaN(total)) {
                                            return null;
                                        }

                                        const restocking = record.restockingFeeAmount != null ? Number(record.restockingFeeAmount) : 0;
                                        if (Number.isNaN(restocking)) {
                                            return null;
                                        }

                                        const value = total - restocking;
                                        if (value < 0) {
                                            return null;
                                        }

                                        return formatCurrencyLabel(value, record.orderTotalCurrency);
                                    })()
                                    : null;

                                let refundTitle;
                                let refundSubtitle;

                                if (!isOrderPaid) {
                                    refundTitle = "No payment received";
                                    refundSubtitle = "Buyer hasn't paid yet";
                                } else if (refund) {
                                    refundTitle = refund;
                                    refundSubtitle = restockingFeeLabel ? `Restocking fee ${restockingFeeLabel}` : null;
                                } else if (estimatedRefund) {
                                    refundTitle = estimatedRefund;
                                    refundSubtitle = restockingFeeLabel
                                        ? `Estimated after ${restockingFeeLabel} restocking fee`
                                        : "Estimated refund once processed";
                                } else {
                                    refundTitle = "-";
                                    refundSubtitle = restockingFeeLabel ? `Restocking fee ${restockingFeeLabel}` : null;
                                }

                                const requestedAt = formatDateTime(record.requestedAtUtc);
                                const buyerInfo = formatBuyer(record.buyerDisplayName, record.buyerUsername);

                                return (
                                    <tr key={record.requestId}>
                                        <td data-label="Action">
                                            <div className="resolution-page__cell">
                                                <span className="resolution-page__cell-title">
                                                    {record.actionLabel ?? "-"}
                                                </span>
                                                {record.requiresSellerAction && (
                                                    <span className="resolution-page__badge resolution-page__badge--attention">
                                                        Needs your action
                                                    </span>
                                                )}
                                                {requestedAt && (
                                                    <span className="resolution-page__cell-subtitle">
                                                        Requested {requestedAt}
                                                    </span>
                                                )}
                                                {record.requiresSellerAction &&
                                                    (record.actionLabel?.toLowerCase() === "review request") && (
                                                        <div className="resolution-page__inline-actions">
                                                            <button
                                                                type="button"
                                                                className="resolution-page__inline-button resolution-page__inline-button--approve"
                                                                onClick={() => handleApproveReturn(record)}
                                                            >
                                                                Accept
                                                            </button>
                                                            <button
                                                                type="button"
                                                                className="resolution-page__inline-button resolution-page__inline-button--decline"
                                                                onClick={() => handleRejectReturn(record)}
                                                            >
                                                                Decline
                                                            </button>
                                                        </div>
                                                    )}
                                            </div>
                                        </td>
                                        <td data-label="Items">
                                            <div className="resolution-page__cell">
                                                <span className="resolution-page__cell-title">{record.itemsSummary ?? "-"}</span>
                                                <span className="resolution-page__cell-subtitle">Order #{record.orderNumber}</span>
                                                {record.trackingNumber && (
                                                    <span className="resolution-page__cell-subtitle">
                                                        Tracking: {record.trackingNumber}
                                                        {record.returnCarrier ? ` · ${record.returnCarrier}` : ""}
                                                    </span>
                                                )}
                                            </div>
                                        </td>
                                        <td data-label="Status">
                                            <span className={`resolution-page__status-badge resolution-page__status-badge--${record.statusCategory ?? "default"}`}>
                                                {record.statusDisplay ?? "-"}
                                            </span>
                                        </td>
                                        <td data-label="Details">{record.details ?? "-"}</td>
                                        <td data-label="Refund">
                                            <div className="resolution-page__cell">
                                                <span className="resolution-page__cell-title">{refundTitle}</span>
                                                {refundSubtitle && (
                                                    <span className="resolution-page__cell-subtitle">{refundSubtitle}</span>
                                                )}
                                            </div>
                                        </td>
                                        <td data-label="Buyer">{buyerInfo}</td>
                                    </tr>
                                );
                            })
                        )}
                    </tbody>
                </table>
            </section>

            <footer className="resolution-page__footer">
                <div className="resolution-page__items-per-page">
                    <span>Items per page:</span>
                    <select value={filters.perPage} onChange={handlePerPageChange}>
                        {PAGE_SIZE_OPTIONS.map((option) => (
                            <option key={option} value={option}>
                                {option}
                            </option>
                        ))}
                    </select>
                </div>
            </footer>
        </div>
    );
};

export default ReturnsPage;
