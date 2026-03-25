import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { Link } from 'react-router-dom';
import { ArrowPathIcon } from '@heroicons/react/24/outline';
import { LoadingScreen } from '../../components/LoadingScreen/LoadingScreen';
import { PerformanceService } from '../../services/Performance';
import './PerformanceInventoryPage.scss';

const EMPTY_DASHBOARD = {
    totalListings: 0,
    availableQuantity: 0,
    reservedQuantity: 0,
    soldQuantity: 0,
    lowStockListings: 0,
    outOfStockListings: 0,
    criticalListings: []
};

const STOCK_FLOW_STEPS = [
    {
        id: 'listing-created',
        title: '1. Listing created',
        description: 'When a seller creates a listing, inventory is initialized in the same persistence flow so stock starts with the listing source of truth.'
    },
    {
        id: 'checkout-reserved',
        title: '2. Checkout reserves stock',
        description: 'Reserve requests reduce sellable stock and create reservation rows with locking to reduce oversell during concurrent checkout.'
    },
    {
        id: 'payment-commit',
        title: '3. Successful payment commits stock',
        description: 'Committed stock moves from reserved to sold, giving the seller a clean picture of fulfilled demand.'
    },
    {
        id: 'cancel-release',
        title: '4. Cancellation releases stock',
        description: 'When a cancellation is approved, matching active reservations are released so the quantity becomes sellable again.'
    },
    {
        id: 'return-restock',
        title: '5. Return refund restocks',
        description: 'When returned items are received back, the refund completion flow restocks sold inventory into availability.'
    }
];

const toNumber = (value) => {
    const numeric = Number(value ?? 0);
    return Number.isFinite(numeric) ? numeric : 0;
};

const formatDateTime = (value) => {
    if (!value) {
        return 'No updates yet';
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) {
        return 'No updates yet';
    }

    return date.toLocaleString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric',
        hour: 'numeric',
        minute: '2-digit'
    });
};

const normalizeCriticalListing = (item) => ({
    listingId: item?.listingId ?? item?.ListingId ?? '',
    title: item?.title ?? item?.Title ?? 'Untitled listing',
    sku: item?.sku ?? item?.Sku ?? 'N/A',
    availableQuantity: toNumber(item?.availableQuantity ?? item?.AvailableQuantity),
    reservedQuantity: toNumber(item?.reservedQuantity ?? item?.ReservedQuantity),
    soldQuantity: toNumber(item?.soldQuantity ?? item?.SoldQuantity),
    thresholdQuantity: item?.thresholdQuantity ?? item?.ThresholdQuantity ?? null,
    lastUpdatedAt: item?.lastUpdatedAt ?? item?.LastUpdatedAt ?? null
});

const normalizeDashboard = (payload) => ({
    totalListings: toNumber(payload?.totalListings ?? payload?.TotalListings),
    availableQuantity: toNumber(payload?.availableQuantity ?? payload?.AvailableQuantity),
    reservedQuantity: toNumber(payload?.reservedQuantity ?? payload?.ReservedQuantity),
    soldQuantity: toNumber(payload?.soldQuantity ?? payload?.SoldQuantity),
    lowStockListings: toNumber(payload?.lowStockListings ?? payload?.LowStockListings),
    outOfStockListings: toNumber(payload?.outOfStockListings ?? payload?.OutOfStockListings),
    criticalListings: Array.isArray(payload?.criticalListings ?? payload?.CriticalListings)
        ? (payload?.criticalListings ?? payload?.CriticalListings).map(normalizeCriticalListing)
        : []
});

const getListingState = (listing) => {
    if (listing.availableQuantity <= 0) {
        return {
            label: 'Out of stock',
            tone: 'critical'
        };
    }

    const threshold = listing.thresholdQuantity == null ? 5 : toNumber(listing.thresholdQuantity);

    if (listing.availableQuantity <= threshold) {
        return {
            label: 'Low stock',
            tone: 'warning'
        };
    }

    return {
        label: 'Healthy',
        tone: 'healthy'
    };
};

const PerformanceInventoryPage = () => {
    const [dashboard, setDashboard] = useState(EMPTY_DASHBOARD);
    const [loading, setLoading] = useState(true);
    const [refreshing, setRefreshing] = useState(false);
    const [error, setError] = useState('');

    const fetchDashboard = useCallback(async ({ silent = false } = {}) => {
        if (!silent) {
            setLoading(true);
        }

        setRefreshing(true);
        setError('');

        try {
            const response = await PerformanceService.getInventoryDashboard();
            const payload = response?.data?.data ?? response?.data ?? null;
            setDashboard(normalizeDashboard(payload));
        } catch (requestError) {
            setError('Unable to load stock data right now. Please try again in a few minutes.');
            setDashboard(EMPTY_DASHBOARD);
        } finally {
            setLoading(false);
            setRefreshing(false);
        }
    }, []);

    useEffect(() => {
        fetchDashboard();
    }, [fetchDashboard]);

    const summaryCards = useMemo(() => ([
        {
            id: 'total-listings',
            label: 'Tracked listings',
            value: dashboard.totalListings,
            accent: 'neutral'
        },
        {
            id: 'available-quantity',
            label: 'Available units',
            value: dashboard.availableQuantity,
            accent: 'healthy'
        },
        {
            id: 'reserved-quantity',
            label: 'Reserved units',
            value: dashboard.reservedQuantity,
            accent: 'info'
        },
        {
            id: 'sold-quantity',
            label: 'Sold units',
            value: dashboard.soldQuantity,
            accent: 'neutral'
        },
        {
            id: 'low-stock-listings',
            label: 'Low-stock listings',
            value: dashboard.lowStockListings,
            accent: 'warning'
        },
        {
            id: 'out-of-stock-listings',
            label: 'Out-of-stock listings',
            value: dashboard.outOfStockListings,
            accent: 'critical'
        }
    ]), [dashboard]);

    if (loading) {
        return <LoadingScreen />;
    }

    return (
        <div className="performance-inventory" aria-busy={refreshing} aria-live="polite">
            <header className="performance-inventory__hero">
                <div className="performance-inventory__hero-copy">
                    <span className="performance-inventory__eyebrow">Performance / Stock</span>
                    <h1>Stock control dashboard</h1>
                    <p>
                        Follow the stock flow already implemented in the backend and keep an eye on listings that need replenishment.
                    </p>
                </div>

                <div className="performance-inventory__hero-actions">
                    <button
                        type="button"
                        className="performance-inventory__refresh-btn"
                        onClick={() => fetchDashboard({ silent: true })}
                        disabled={refreshing}
                    >
                        <ArrowPathIcon />
                        <span>{refreshing ? 'Refreshing...' : 'Refresh data'}</span>
                    </button>
                    <Link to="/listings/active" className="performance-inventory__link-btn">
                        View active listings
                    </Link>
                </div>
            </header>

            {error && (
                <div className="performance-inventory__error" role="status">
                    {error}
                </div>
            )}

            <section className="performance-inventory__summary-grid">
                {summaryCards.map((card) => (
                    <article
                        key={card.id}
                        className={`performance-inventory__summary-card performance-inventory__summary-card--${card.accent}`}
                    >
                        <span className="performance-inventory__summary-label">{card.label}</span>
                        <strong className="performance-inventory__summary-value">{card.value.toLocaleString('en-US')}</strong>
                    </article>
                ))}
            </section>

            <section className="performance-inventory__content-grid">
                <article className="performance-inventory__panel performance-inventory__panel--flow">
                    <div className="performance-inventory__panel-head">
                        <div>
                            <h2>Implemented stock flow</h2>
                            <p>The UI tab mirrors what is already wired on the backend inventory lifecycle.</p>
                        </div>
                    </div>

                    <div className="performance-inventory__flow-list">
                        {STOCK_FLOW_STEPS.map((step) => (
                            <article key={step.id} className="performance-inventory__flow-step">
                                <h3>{step.title}</h3>
                                <p>{step.description}</p>
                            </article>
                        ))}
                    </div>
                </article>

                <article className="performance-inventory__panel performance-inventory__panel--insight">
                    <div className="performance-inventory__panel-head">
                        <div>
                            <h2>Operational focus</h2>
                            <p>What the seller should check first when stock pressure increases.</p>
                        </div>
                    </div>

                    <ul className="performance-inventory__insights">
                        <li>
                            <strong>{dashboard.outOfStockListings.toLocaleString('en-US')}</strong>
                            <span>Listings are already out of stock and should be replenished or ended.</span>
                        </li>
                        <li>
                            <strong>{dashboard.lowStockListings.toLocaleString('en-US')}</strong>
                            <span>Listings are approaching their threshold and may soon stop converting well.</span>
                        </li>
                        <li>
                            <strong>{dashboard.reservedQuantity.toLocaleString('en-US')}</strong>
                            <span>Units are currently tied to checkout or order activity and are not sellable yet.</span>
                        </li>
                        <li>
                            <strong>{dashboard.soldQuantity.toLocaleString('en-US')}</strong>
                            <span>Units have already been committed as completed stock movement.</span>
                        </li>
                    </ul>

                    <div className="performance-inventory__quick-links">
                        <Link to="/order/all?status=awaiting-shipment">Open awaiting-shipment orders</Link>
                        <Link to="/order/returns">Review return requests</Link>
                        <Link to="/listings/active">Restock affected listings</Link>
                    </div>
                </article>
            </section>

            <section className="performance-inventory__panel performance-inventory__panel--table">
                <div className="performance-inventory__panel-head">
                    <div>
                        <h2>Critical listings</h2>
                        <p>Listings returned by the inventory dashboard because they are low or out of stock.</p>
                    </div>
                    <span className="performance-inventory__table-count">
                        {dashboard.criticalListings.length.toLocaleString('en-US')} listing{dashboard.criticalListings.length === 1 ? '' : 's'}
                    </span>
                </div>

                {dashboard.criticalListings.length === 0 ? (
                    <div className="performance-inventory__empty-state">
                        <h3>No critical listings right now</h3>
                        <p>Your inventory dashboard is not flagging any listings for urgent action.</p>
                    </div>
                ) : (
                    <div className="performance-inventory__table-wrap">
                        <table className="performance-inventory__table">
                            <thead>
                                <tr>
                                    <th>Listing</th>
                                    <th>SKU</th>
                                    <th>Status</th>
                                    <th>Available</th>
                                    <th>Reserved</th>
                                    <th>Sold</th>
                                    <th>Threshold</th>
                                    <th>Last update</th>
                                </tr>
                            </thead>
                            <tbody>
                                {dashboard.criticalListings.map((listing) => {
                                    const state = getListingState(listing);

                                    return (
                                        <tr key={listing.listingId || `${listing.sku}-${listing.title}`}>
                                            <td>
                                                <div className="performance-inventory__listing-cell">
                                                    <strong>{listing.title}</strong>
                                                    {listing.listingId && (
                                                        <span>{listing.listingId}</span>
                                                    )}
                                                </div>
                                            </td>
                                            <td>{listing.sku}</td>
                                            <td>
                                                <span className={`performance-inventory__status performance-inventory__status--${state.tone}`}>
                                                    {state.label}
                                                </span>
                                            </td>
                                            <td>{listing.availableQuantity.toLocaleString('en-US')}</td>
                                            <td>{listing.reservedQuantity.toLocaleString('en-US')}</td>
                                            <td>{listing.soldQuantity.toLocaleString('en-US')}</td>
                                            <td>{listing.thresholdQuantity == null ? 'Not set' : toNumber(listing.thresholdQuantity).toLocaleString('en-US')}</td>
                                            <td>{formatDateTime(listing.lastUpdatedAt)}</td>
                                        </tr>
                                    );
                                })}
                            </tbody>
                        </table>
                    </div>
                )}
            </section>
        </div>
    );
};

export default PerformanceInventoryPage;