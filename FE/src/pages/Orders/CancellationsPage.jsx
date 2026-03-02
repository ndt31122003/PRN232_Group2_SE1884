import React, { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import OrderService from "../../services/OrderService";
import "./ResolutionCenterPage.scss";

const STATUS_OPTIONS = [
    { value: "open", label: "Open cancellations" },
    { value: "requests", label: "Cancellation requests" },
    { value: "in-progress", label: "Cancellations in progress" },
    { value: "declined", label: "Cancellations declined" },
    { value: "cancelled", label: "Canceled" },
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
    { value: "request", label: "Cancellation ID" },
];

const SORT_OPTIONS = [
    { value: "date-desc", label: "Date requested" },
    { value: "date-asc", label: "Date requested (oldest)" },
];

const PAGE_SIZE_OPTIONS = [25, 50, 100, 200];

const DEFAULT_FORM_STATE = {
    status: STATUS_OPTIONS[0].value,
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

const CANCELLATION_STATUS_MAP = {
    open: "Open",
    requests: "Requests",
    "in-progress": "InProgress",
    declined: "Declined",
    cancelled: "Cancelled",
};

const PERIOD_MAP = {
    last30: "Last30Days",
    last90: "Last90Days",
    last180: "Last180Days",
    thisYear: "ThisYear",
    custom: "Custom",
};

const CANCELLATION_SEARCH_MAP = {
    buyer: "BuyerUsername",
    request: "CancelId",
};

const CANCELLATION_SORT_MAP = {
    "date-desc": { sortBy: "DateRequested", sortDescending: true },
    "date-asc": { sortBy: "DateRequested", sortDescending: false },
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

const buildCancellationQuery = (filters) => {
    const { sortBy, sortDescending } = CANCELLATION_SORT_MAP[filters.sortBy] ?? CANCELLATION_SORT_MAP["date-desc"];
    const trimmedKeyword = filters.keyword?.trim();

    const params = {
        status: CANCELLATION_STATUS_MAP[filters.status] ?? CANCELLATION_STATUS_MAP.open,
        period: PERIOD_MAP[filters.period] ?? PERIOD_MAP.last90,
        sortBy,
        sortDescending,
        pageNumber: filters.pageNumber ?? 1,
        pageSize: filters.perPage,
    };

    if (trimmedKeyword) {
        params.keyword = trimmedKeyword;
        params.searchBy = CANCELLATION_SEARCH_MAP[filters.searchBy] ?? CANCELLATION_SEARCH_MAP.buyer;
    }

    return params;
};

const CancellationsPage = () => {
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

        const loadCancellations = async () => {
            setIsLoading(true);
            setError(null);

            try {
                const params = buildCancellationQuery(filters);
                const response = await OrderService.getCancellationRequests(params, controller.signal);
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

                setError("Unable to load cancellation requests.");
                setRecords([]);
                setTotalCount(0);
            } finally {
                if (!isCancelled && !controller.signal.aborted) {
                    setIsLoading(false);
                }
            }
        };

        loadCancellations();

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

    const handleApproveCancellation = (record) => {
        if (!record?.requestId) {
            return;
        }

        setActionMessage(null);
        setActionError(null);
        navigate(`/order/cancellations/${record.requestId}/approve`, { state: { record } });
    };

    const handleRejectCancellation = (record) => {
        if (!record?.requestId) {
            return;
        }

        setActionMessage(null);
        setActionError(null);
        navigate(`/order/cancellations/${record.requestId}/decline`, { state: { record } });
    };

    return (
        <div className="resolution-page">
            <header className="resolution-page__header">
                <div>
                    <h1 className="resolution-page__title">Manage cancellations</h1>
                    <p className="resolution-page__subtitle">
                        Track cancellation activity and respond to buyer requests in one place.
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
                        <label className="resolution-page__label" htmlFor="cancellations-period">
                            Period:
                        </label>
                        <select
                            id="cancellations-period"
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
                        <label className="resolution-page__label" htmlFor="cancellations-search-by">
                            Search by:
                        </label>
                        <div className="resolution-page__search">
                            <select
                                id="cancellations-search-by"
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
                    <label htmlFor="cancellations-sort">Sort by:</label>
                    <select
                        id="cancellations-sort"
                        value={filters.sortBy}
                        onChange={handleSortChange}
                    >
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
                                    Loading cancellation requests...
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
                                const requestedAt = formatDateTime(record.requestedAtUtc);

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
                                                                onClick={() => handleApproveCancellation(record)}
                                                            >
                                                                Accept
                                                            </button>
                                                            <button
                                                                type="button"
                                                                className="resolution-page__inline-button resolution-page__inline-button--decline"
                                                                onClick={() => handleRejectCancellation(record)}
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
                                            </div>
                                        </td>
                                        <td data-label="Status">
                                            <span className={`resolution-page__status-badge resolution-page__status-badge--${record.statusCategory ?? "default"}`}>
                                                {record.statusDisplay ?? "-"}
                                            </span>
                                        </td>
                                        <td data-label="Details">{record.details ?? "-"}</td>
                                        <td data-label="Refund">{refund ?? "-"}</td>
                                        <td data-label="Buyer">{formatBuyer(record.buyerDisplayName, record.buyerUsername)}</td>
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

export default CancellationsPage;
