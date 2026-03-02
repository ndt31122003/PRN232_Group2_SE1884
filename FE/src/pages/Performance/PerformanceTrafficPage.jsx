import React, { useEffect, useMemo, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { LoadingScreen } from '../../components/LoadingScreen/LoadingScreen';
import { ChevronDownIcon, MagnifyingGlassIcon, ArrowDownTrayIcon } from '@heroicons/react/24/outline';
import {
    ResponsiveContainer,
    AreaChart,
    BarChart,
    Area,
    Bar,
    CartesianGrid,
    XAxis,
    YAxis,
    Tooltip
} from 'recharts';
import { PerformanceService } from '../../services/Performance';
import OrderService from '../../services/OrderService';
import './PerformanceTrafficPage.scss';

const MOCK_TRAFFIC = {
    metrics: [
        { id: 'impressions', label: 'Impressions', value: '0', change: '0.0% vs prior 30 days' },
        { id: 'listingViews', label: 'Listing views', value: '0', change: '0.0% vs prior 30 days' },
        { id: 'quantitySold', label: 'Quantity sold', value: '0', change: 'N/A vs prior 30 days' },
        { id: 'ctr', label: 'Click-through rate', value: '0.0%', change: '0.0% vs prior 30 days' },
        { id: 'conversion', label: 'Sales conversion rate', value: '0.0%', change: 'N/A vs prior 30 days' }
    ],
    chartTabs: ['Impressions', 'Listing views', 'Quantity sold'],
    chartType: 'Area chart',
    sources: [
        { id: 'organic', label: 'Organic', subtitle: 'impressions on eBay', value: '0 impressions', change: '0.0% vs prior 30 days' },
        { id: 'promoted', label: 'Promoted Listings', subtitle: 'impressions on eBay and the eBay Network', value: '0 impressions', change: '0.0% vs prior 30 days' },
        { id: 'offsite', label: 'Promoted Offsite', subtitle: 'impressions off eBay', value: '0 impressions', change: '0.0% vs prior 30 days' }
    ],
    listingsEmpty: "We couldn't find any sales in the selected time period. Try changing the date range or filters."
};

const MS_PER_DAY = 24 * 60 * 60 * 1000;
// Heuristic multipliers to approximate traffic metrics from real order activity
const IMPRESSIONS_PER_LISTING = 120;
const IMPRESSIONS_PER_ORDER = 45;
const IMPRESSIONS_PER_UNIT = 30;
const VIEWS_PER_LISTING = 40;
const VIEWS_PER_ORDER = 18;
const VIEWS_PER_UNIT = 8;

const PERIOD_CONFIG = {
    past_30_days: 30,
    last_90_days: 90,
};

const normalizeChartType = (value) => {
    if (typeof value !== 'string') {
        return 'area';
    }

    const lower = value.toLowerCase();
    if (lower.includes('bar')) {
        return 'bar';
    }

    return 'area';
};

const clampToStartOfDay = (value) => {
    const date = new Date(value);
    date.setHours(0, 0, 0, 0);
    return date;
};

const buildRangeForPeriod = (periodKey) => {
    const today = clampToStartOfDay(new Date());
    const length = PERIOD_CONFIG[periodKey] ?? PERIOD_CONFIG.past_30_days;
    const start = new Date(today);
    start.setDate(today.getDate() - (length - 1));
    return { start, end: today, length };
};

const buildComparisonRange = (range) => {
    const compareEnd = new Date(range.start);
    compareEnd.setDate(compareEnd.getDate() - 1);
    const compareStart = new Date(compareEnd);
    compareStart.setDate(compareEnd.getDate() - (range.length - 1));
    return { start: compareStart, end: compareEnd, length: range.length };
};

const startOfDayUtcIso = (value) => {
    const date = new Date(value);
    date.setHours(0, 0, 0, 0);
    return date.toISOString();
};

const endOfDayUtcIso = (value) => {
    const date = new Date(value);
    date.setHours(23, 59, 59, 999);
    return date.toISOString();
};

const parseMoneyValue = (money) => {
    if (money === null || money === undefined) {
        return 0;
    }

    if (typeof money === 'number') {
        return Number.isFinite(money) ? money : 0;
    }

    if (typeof money === 'string') {
        const numeric = Number(money);
        return Number.isFinite(numeric) ? numeric : 0;
    }

    if (typeof money === 'object') {
        const amount = money.amount ?? money.Amount ?? null;
        if (amount === null || amount === undefined) {
            return 0;
        }
        const numeric = Number(amount);
        return Number.isFinite(numeric) ? numeric : 0;
    }

    return 0;
};

const getOrderItems = (order) => {
    const items = order?.items ?? order?.Items;
    return Array.isArray(items) ? items : [];
};

const getOrderCurrency = (order) => (
    order?.total?.currency
    ?? order?.total?.Currency
    ?? order?.Total?.currency
    ?? order?.Total?.Currency
    ?? order?.currency
    ?? order?.Currency
    ?? null
);

const getStatusCode = (order) => {
    const status = order?.statusCode ?? order?.StatusCode ?? order?.status?.code ?? order?.Status?.Code ?? '';
    return typeof status === 'string' ? status : '';
};

const isOrderEligible = (order) => {
    const status = getStatusCode(order);
    return !status || status.toLowerCase() !== 'draft';
};

const getOrderedAt = (order) => {
    const raw = order?.orderedAt ?? order?.OrderedAt ?? null;
    if (!raw) {
        return null;
    }

    const date = new Date(raw);
    if (Number.isNaN(date.getTime())) {
        return null;
    }

    date.setHours(0, 0, 0, 0);
    return date;
};

const roundToDecimal = (value, precision = 1) => {
    const factor = 10 ** precision;
    return Math.round(value * factor) / factor;
};

const computeTrafficMetrics = ({
    orderCount,
    distinctListings,
    quantitySold,
    grossSales,
    currency,
}) => {
    let impressions = (distinctListings * IMPRESSIONS_PER_LISTING)
        + (orderCount * IMPRESSIONS_PER_ORDER)
        + (quantitySold * IMPRESSIONS_PER_UNIT);

    let listingViews = (distinctListings * VIEWS_PER_LISTING)
        + (orderCount * VIEWS_PER_ORDER)
        + (quantitySold * VIEWS_PER_UNIT);

    if (quantitySold > 0) {
        listingViews = Math.max(listingViews, quantitySold * VIEWS_PER_UNIT);
    }

    if (orderCount > 0 || quantitySold > 0) {
        listingViews = Math.max(listingViews, orderCount * VIEWS_PER_ORDER);
        impressions = Math.max(impressions, listingViews + orderCount * IMPRESSIONS_PER_ORDER);
    }

    const top20 = Math.round(impressions * 0.62);
    const nonSearch = Math.max(0, impressions - top20);
    const ebayViews = Math.round(listingViews * 0.74);
    const externalViews = Math.max(0, listingViews - ebayViews);

    const clickThroughRate = listingViews === 0
        ? 0
        : roundToDecimal((quantitySold / listingViews) * 100, 1);

    const conversionRate = impressions === 0
        ? 0
        : roundToDecimal((quantitySold / impressions) * 100, 1);

    return {
        orderCount,
        distinctListings,
        quantitySold,
        grossSales,
        currency: currency ?? 'USD',
        impressions,
        top20Impressions: top20,
        nonSearchImpressions: nonSearch,
        listingViews,
        ebayViews,
        externalViews,
        clickThroughRate,
        conversionRate,
    };
};

const buildAggregateForOrders = (orders) => {
    if (!orders || orders.length === 0) {
        return computeTrafficMetrics({
            orderCount: 0,
            distinctListings: 0,
            quantitySold: 0,
            grossSales: 0,
            currency: 'USD',
        });
    }

    const listingIds = new Set();
    let quantitySold = 0;
    let grossSales = 0;
    let currency = null;

    orders.forEach((order) => {
        const items = getOrderItems(order);
        items.forEach((item) => {
            const listingId = item?.listingId ?? item?.ListingId ?? null;
            if (listingId) {
                listingIds.add(listingId);
            }

            const quantity = Number(item?.quantity ?? item?.Quantity ?? 0);
            if (Number.isFinite(quantity)) {
                quantitySold += quantity;
            }
        });

        grossSales += parseMoneyValue(order?.total ?? order?.Total ?? order?.totalAmount ?? order?.TotalAmount);
        if (!currency) {
            currency = getOrderCurrency(order);
        }
    });

    return computeTrafficMetrics({
        orderCount: orders.length,
        distinctListings: listingIds.size,
        quantitySold,
        grossSales,
        currency: currency ?? 'USD',
    });
};

const buildDailyTrafficSeries = (orders, range) => {
    const buckets = new Map();

    orders.forEach((order) => {
        if (!isOrderEligible(order)) {
            return;
        }

        const orderedAt = getOrderedAt(order);
        if (!orderedAt) {
            return;
        }

        if (orderedAt < range.start || orderedAt > range.end) {
            return;
        }

        const key = orderedAt.toISOString().slice(0, 10);
        if (!buckets.has(key)) {
            buckets.set(key, []);
        }

        buckets.get(key).push(order);
    });

    const series = [];
    for (let cursor = new Date(range.start); cursor <= range.end; cursor = new Date(cursor.getTime() + MS_PER_DAY)) {
        const key = cursor.toISOString().slice(0, 10);
        const dayOrders = buckets.get(key) ?? [];
        const metrics = buildAggregateForOrders(dayOrders);

        series.push({
            date: key,
            label: cursor.toLocaleDateString('en-US', { month: 'short', day: 'numeric' }),
            impressions: metrics.impressions,
            listingViews: metrics.listingViews,
            quantitySold: metrics.quantitySold,
            top20: metrics.top20Impressions,
            nonSearch: metrics.nonSearchImpressions,
            ebayViews: metrics.ebayViews,
            externalViews: metrics.externalViews,
            orders: metrics.orderCount,
            distinctListings: metrics.distinctListings,
        });
    }

    return series;
};

const buildListingMetricsMap = (orders) => {
    const map = new Map();

    orders.forEach((order) => {
        if (!isOrderEligible(order)) {
            return;
        }

        const orderId = order?.id ?? order?.Id ?? null;
        const currency = getOrderCurrency(order);
        const items = getOrderItems(order);

        items.forEach((item) => {
            const listingId = item?.listingId ?? item?.ListingId ?? null;
            const title = item?.title ?? item?.Title ?? 'Untitled listing';
            const key = listingId ? `listing-${listingId}` : `title-${title.toLowerCase()}`;

            if (!map.has(key)) {
                map.set(key, {
                    key,
                    listingId,
                    title,
                    imageUrl: item?.imageUrl ?? item?.ImageUrl ?? null,
                    orderIds: new Set(),
                    quantitySold: 0,
                    grossSales: 0,
                    currency: currency ?? 'USD',
                });
            }

            const entry = map.get(key);

            if (orderId) {
                entry.orderIds.add(orderId);
            }

            const quantity = Number(item?.quantity ?? item?.Quantity ?? 0);
            if (Number.isFinite(quantity)) {
                entry.quantitySold += quantity;
            }

            entry.grossSales += parseMoneyValue(item?.totalPrice ?? item?.TotalPrice ?? 0);

            if (!entry.currency) {
                entry.currency = currency ?? 'USD';
            }
        });
    });

    map.forEach((entry) => {
        entry.metrics = computeTrafficMetrics({
            orderCount: entry.orderIds.size,
            distinctListings: 1,
            quantitySold: entry.quantitySold,
            grossSales: entry.grossSales,
            currency: entry.currency,
        });
    });

    return map;
};

const formatPercentageChangeText = (current, previous) => {
    if (previous === 0) {
        return current === 0 ? '0.0%' : 'N/A';
    }

    const change = ((current - previous) / previous) * 100;
    const rounded = roundToDecimal(change, 1);
    const prefix = rounded >= 0 ? '+' : '';
    return `${prefix}${rounded.toFixed(1)}%`;
};

const formatRate = (value) => `${roundToDecimal(value ?? 0, 1).toFixed(1)}%`;

const formatNumber = (value) => Number(value ?? 0).toLocaleString('en-US');

const aggregateListingsTraffic = (currentOrders, previousOrders) => {
    const currentMap = buildListingMetricsMap(currentOrders ?? []);
    const previousMap = buildListingMetricsMap(previousOrders ?? []);

    return Array.from(currentMap.values())
        .map((entry) => {
            const previous = previousMap.get(entry.key);

            return {
                id: entry.listingId ?? entry.key,
                title: entry.title,
                imageUrl: entry.imageUrl,
                available: '-',
                impressions: entry.metrics.impressions,
                top20Change: formatPercentageChangeText(
                    entry.metrics.top20Impressions,
                    previous?.metrics?.top20Impressions ?? 0
                ),
                nonSearchChange: formatPercentageChangeText(
                    entry.metrics.nonSearchImpressions,
                    previous?.metrics?.nonSearchImpressions ?? 0
                ),
                ebayViews: entry.metrics.ebayViews,
                externalViews: entry.metrics.externalViews,
                quantitySold: entry.metrics.quantitySold,
                ctr: formatRate(entry.metrics.clickThroughRate),
                conversion: formatRate(entry.metrics.conversionRate),
            };
        })
        .sort((a, b) => b.impressions - a.impressions);
};

// Guard against fetching unbounded order history in a single request
const MAX_PAGES = 25;

const fetchOrdersForRange = async (range, signal) => {
    if (!range) {
        return [];
    }

    const fromDate = startOfDayUtcIso(range.start);
    const toDate = endOfDayUtcIso(range.end);

    const pageSize = 200;
    let pageNumber = 1;
    let totalCount = Number.POSITIVE_INFINITY;
    const orders = [];

    while (pageNumber <= MAX_PAGES && orders.length < totalCount) {
        const response = await OrderService.getOrders({
            PageNumber: pageNumber,
            PageSize: pageSize,
            Period: 'Custom',
            FromDate: fromDate,
            ToDate: toDate,
            SortBy: 'DatePaid',
            SortDescending: true,
        }, signal);

        const data = response?.data ?? {};
        const items = data.items ?? data.Items ?? [];
        orders.push(...items);

        const declaredTotal = data.totalCount ?? data.TotalCount;
        if (typeof declaredTotal === 'number' && Number.isFinite(declaredTotal)) {
            totalCount = declaredTotal;
        }

        if (items.length < pageSize) {
            break;
        }

        pageNumber += 1;
    }

    return orders;
};

const TAB_TO_DATA_KEY = {
    'Impressions': 'impressions',
    'Listing views': 'listingViews',
    'Quantity sold': 'quantitySold',
};

const CHART_COLORS = {
    impressions: '#2563eb',
    listingViews: '#059669',
    quantitySold: '#f97316',
};

const PerformanceTrafficPage = () => {
    const { t } = useTranslation('global');
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [period, setPeriod] = useState('past_30_days');
    const [activeTab, setActiveTab] = useState('Impressions');
    const [chartMenuOpen, setChartMenuOpen] = useState(false);
    const [periodMenuOpen, setPeriodMenuOpen] = useState(false);
    const [chartSeries, setChartSeries] = useState([]);
    const [listings, setListings] = useState([]);
    const [searchTerm, setSearchTerm] = useState('');
    const [listingStatus, setListingStatus] = useState('active');
    const [promotionFilter, setPromotionFilter] = useState('all');
    const [statusMenuOpen, setStatusMenuOpen] = useState(false);
    const [promotionMenuOpen, setPromotionMenuOpen] = useState(false);
    const [chartType, setChartType] = useState('area');
    const [chartTypeLocked, setChartTypeLocked] = useState(false);
    const chartTypeLockedRef = useRef(false);

    const mergeTraffic = useMemo(() => (
        (payload) => {
            const source = payload ?? {};
            return {
                ...MOCK_TRAFFIC,
                ...source,
                metrics: Array.isArray(source.metrics) && source.metrics.length > 0 ? source.metrics : MOCK_TRAFFIC.metrics,
                chartTabs: Array.isArray(source.chartTabs) && source.chartTabs.length > 0 ? source.chartTabs : MOCK_TRAFFIC.chartTabs,
                chartType: source.chartType ?? 'Area chart',
                sources: Array.isArray(source.sources) && source.sources.length > 0 ? source.sources : MOCK_TRAFFIC.sources,
                listingsEmpty: source.listingsEmpty ?? MOCK_TRAFFIC.listingsEmpty,
            };
        }
    ), []);

    useEffect(() => {
        chartTypeLockedRef.current = chartTypeLocked;
    }, [chartTypeLocked]);

    const activeMetricKey = TAB_TO_DATA_KEY[activeTab] ?? 'impressions';
    const lineColor = CHART_COLORS[activeMetricKey] ?? '#2563eb';

    const filteredListings = useMemo(() => {
        let result = listings;

        if (listingStatus === 'sold') {
            result = result.filter((listing) => Number(listing.quantitySold ?? 0) > 0);
        }

        // Backend does not currently differentiate promoted listings. We keep the full
        // dataset for now so the UI stays responsive when more data becomes available.
        if (promotionFilter === 'promoted') {
            result = result.filter(() => true);
        } else if (promotionFilter === 'non-promoted') {
            result = result.filter(() => true);
        }

        if (searchTerm.trim()) {
            const keyword = searchTerm.trim().toLowerCase();
            result = result.filter((listing) => listing.title.toLowerCase().includes(keyword));
        }

        return result;
    }, [listings, listingStatus, promotionFilter, searchTerm]);

    const gradientId = useMemo(() => `trafficArea-${activeMetricKey}`, [activeMetricKey]);
    const chartTypeLabel = chartType === 'bar'
        ? t('performance.traffic.barChart', 'Bar chart')
        : t('performance.traffic.areaChart', 'Area chart');

    const handleDownloadCsv = () => {
        const headers = [
            'Title',
            'Impressions',
            'eBay views',
            'External views',
            'Quantity sold',
            'Click-through rate',
            'Sales conversion rate',
        ];

        const escape = (value) => {
            const safe = value ?? '';
            if (typeof safe === 'number') {
                return safe.toString();
            }
            const text = String(safe);
            if (text.includes('"') || text.includes(',') || text.includes('\n')) {
                return `"${text.replace(/"/g, '""')}"`;
            }
            return text;
        };

        const rows = filteredListings.map((listing) => [
            escape(listing.title),
            escape(formatNumber(listing.impressions)),
            escape(formatNumber(listing.ebayViews)),
            escape(formatNumber(listing.externalViews)),
            escape(formatNumber(listing.quantitySold)),
            escape(listing.ctr),
            escape(listing.conversion),
        ].join(','));

        const csv = [headers.join(','), ...rows].join('\r\n');
        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', 'listing-traffic.csv');
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(url);
    };

    useEffect(() => {
        let ignore = false;
        const controller = new AbortController();

        const loadTraffic = async () => {
            setLoading(true);
            const range = buildRangeForPeriod(period);
            const comparisonRange = buildComparisonRange(range);

            try {
                const [trafficResponse, currentOrders, previousOrders] = await Promise.all([
                    PerformanceService.getPerformanceTraffic({ period }),
                    fetchOrdersForRange(range, controller.signal),
                    fetchOrdersForRange(comparisonRange, controller.signal),
                ]);

                if (ignore) {
                    return;
                }

                const payload = trafficResponse?.data?.data ?? trafficResponse?.data ?? null;
                const merged = mergeTraffic(payload);
                setData(merged);
                if (!chartTypeLockedRef.current) {
                    setChartType(normalizeChartType(merged.chartType));
                    setChartTypeLocked(false);
                    chartTypeLockedRef.current = false;
                }
                setChartSeries(buildDailyTrafficSeries(currentOrders, range));
                setListings(aggregateListingsTraffic(currentOrders, previousOrders));
            } catch (error) {
                if (controller.signal.aborted || ignore) {
                    return;
                }

                console.error('Failed to load performance traffic data', error);
                const fallback = mergeTraffic(null);
                setData(fallback);
                if (!chartTypeLockedRef.current) {
                    setChartType(normalizeChartType(fallback.chartType));
                    setChartTypeLocked(false);
                    chartTypeLockedRef.current = false;
                }
                setChartSeries(buildDailyTrafficSeries([], buildRangeForPeriod(period)));
                setListings([]);
            } finally {
                if (!ignore) {
                    setLoading(false);
                }
            }
        };

        loadTraffic();

        return () => {
            ignore = true;
            controller.abort();
        };
    }, [period, mergeTraffic]);

    const closeMenus = () => {
        setChartMenuOpen(false);
        setPeriodMenuOpen(false);
        setStatusMenuOpen(false);
        setPromotionMenuOpen(false);
    };

    if (loading && !data) {
        return <LoadingScreen />;
    }

    if (!data) {
        return <div>{t('common.noData', 'No data available.')}</div>;
    }

    return (
        <div className="performance-traffic-page" onClick={closeMenus} aria-busy={loading}>
            <header className="traffic-header">
                <div>
                    <h1>{t('performance.traffic.heading', 'Review your traffic')}</h1>
                    <p>{t('performance.traffic.subheading', 'See how many people visited your listings and placed an order.')}</p>
                </div>
                <div
                    className={`period-selector ${periodMenuOpen ? 'is-open' : ''}`}
                    onClick={(event) => event.stopPropagation()}
                >
                    <button type="button" onClick={() => setPeriodMenuOpen((prev) => !prev)}>
                        {period === 'past_30_days'
                            ? t('performance.traffic.period30', 'Past 30 days')
                            : t('performance.traffic.period90', 'Last 90 days')}
                        <ChevronDownIcon />
                    </button>
                    {periodMenuOpen && (
                        <ul>
                            <li onClick={() => { setPeriod('past_30_days'); setPeriodMenuOpen(false); }}>
                                {t('performance.traffic.period30', 'Past 30 days')}
                            </li>
                            <li onClick={() => { setPeriod('last_90_days'); setPeriodMenuOpen(false); }}>
                                {t('performance.traffic.period90', 'Last 90 days')}
                            </li>
                        </ul>
                    )}
                </div>
            </header>

            <section className="performance-card overall-traffic">
                <div className="metric-grid">
                    {data.metrics.map((metric) => (
                        <div key={metric.id} className="metric-card">
                            <span className="label">{metric.label}</span>
                            <span className="value">{metric.value}</span>
                            <span className="change">{metric.change}</span>
                        </div>
                    ))}
                </div>

                <div className="results-area">
                    <div className="results-card" onClick={(event) => event.stopPropagation()}>
                        <div className="results-card__header">
                            <div className="tabs">
                                {data.chartTabs.map((tab) => (
                                    <button
                                        key={tab}
                                        type="button"
                                        className={tab === activeTab ? 'is-active' : ''}
                                        onClick={() => setActiveTab(tab)}
                                    >
                                        {tab}
                                    </button>
                                ))}
                            </div>
                            <div className={`chart-selector ${chartMenuOpen ? 'is-open' : ''}`}>
                                <button
                                    type="button"
                                    onClick={(event) => {
                                        event.stopPropagation();
                                        setChartMenuOpen((prev) => !prev);
                                    }}
                                >
                                    {chartTypeLabel}
                                    <ChevronDownIcon />
                                </button>
                                {chartMenuOpen && (
                                    <ul>
                                        <li
                                            className={chartType === 'bar' ? 'is-selected' : ''}
                                            onClick={() => {
                                                setChartType('bar');
                                                setChartTypeLocked(true);
                                                chartTypeLockedRef.current = true;
                                                setChartMenuOpen(false);
                                            }}
                                        >
                                            {t('performance.traffic.barChart', 'Bar chart')}
                                        </li>
                                        <li
                                            className={chartType === 'area' ? 'is-selected' : ''}
                                            onClick={() => {
                                                setChartType('area');
                                                setChartTypeLocked(false);
                                                chartTypeLockedRef.current = false;
                                                setChartMenuOpen(false);
                                            }}
                                        >
                                            {t('performance.traffic.areaChart', 'Area chart')}
                                        </li>
                                    </ul>
                                )}
                            </div>
                        </div>
                        <div className="traffic-chart">
                            <ResponsiveContainer width="100%" height="100%">
                                {chartType === 'bar' ? (
                                    <BarChart data={chartSeries} margin={{ top: 12, right: 16, left: 8, bottom: 0 }}>
                                        <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                                        <XAxis dataKey="label" tickLine={false} axisLine={{ stroke: '#e5e7eb' }} minTickGap={16} />
                                        <YAxis tickFormatter={formatNumber} axisLine={{ stroke: '#e5e7eb' }} tickLine={false} />
                                        <Tooltip
                                            formatter={(value) => [formatNumber(value), t(`performance.traffic.tooltip.${activeMetricKey}`, activeTab)]}
                                            labelFormatter={(label) => label}
                                        />
                                        <Bar dataKey={activeMetricKey} fill={lineColor} radius={[4, 4, 0, 0]} />
                                    </BarChart>
                                ) : (
                                    <AreaChart data={chartSeries} margin={{ top: 12, right: 16, left: 8, bottom: 0 }}>
                                        <defs>
                                            <linearGradient id={gradientId} x1="0" y1="0" x2="0" y2="1">
                                                <stop offset="5%" stopColor={lineColor} stopOpacity={0.32} />
                                                <stop offset="95%" stopColor={lineColor} stopOpacity={0.05} />
                                            </linearGradient>
                                        </defs>
                                        <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                                        <XAxis dataKey="label" tickLine={false} axisLine={{ stroke: '#e5e7eb' }} minTickGap={16} />
                                        <YAxis tickFormatter={formatNumber} axisLine={{ stroke: '#e5e7eb' }} tickLine={false} />
                                        <Tooltip
                                            formatter={(value) => [formatNumber(value), t(`performance.traffic.tooltip.${activeMetricKey}`, activeTab)]}
                                            labelFormatter={(label) => label}
                                        />
                                        <Area
                                            type="monotone"
                                            dataKey={activeMetricKey}
                                            stroke={lineColor}
                                            strokeWidth={2}
                                            fill={`url(#${gradientId})`}
                                            activeDot={{ r: 5 }}
                                        />
                                    </AreaChart>
                                )}
                            </ResponsiveContainer>
                        </div>
                    </div>

                    <aside className="sources-card">
                        <h3>{t('performance.traffic.sourcesHeading', 'How buyers found your listings')}</h3>
                        <div className="sources-list">
                            {data.sources.map((source) => (
                                <div key={source.id} className="source-row">
                                    <div>
                                        <p className="source-label">{source.label}</p>
                                        <p className="source-sub">{source.subtitle}</p>
                                    </div>
                                    <div className="source-value">
                                        <span>{source.value}</span>
                                        <span>{source.change}</span>
                                    </div>
                                </div>
                            ))}
                        </div>
                    </aside>
                </div>
            </section>

            <section className="performance-card listings-traffic" onClick={(event) => event.stopPropagation()}>
                <div className="listings-traffic__header">
                    <div>
                        <h2>{t('performance.traffic.listingsHeading', 'Your listings')}</h2>
                        <p>{t('performance.traffic.listingsCopy', 'Choose how traffic to each listing impacted your sales performance.')}</p>
                    </div>
                    <div className="listings-controls">
                        <div className="filter-group">
                            <div
                                className={`filter-dropdown ${statusMenuOpen ? 'is-open' : ''}`}
                                onClick={(event) => event.stopPropagation()}
                            >
                                <button type="button" onClick={() => setStatusMenuOpen((prev) => !prev)}>
                                    {listingStatus === 'active'
                                        ? t('performance.traffic.activeListings', 'Active listings')
                                        : t('performance.traffic.soldListings', 'Sold listings')}
                                    <ChevronDownIcon />
                                </button>
                                {statusMenuOpen && (
                                    <ul>
                                        <li
                                            className={listingStatus === 'active' ? 'is-selected' : ''}
                                            onClick={() => {
                                                setListingStatus('active');
                                                setStatusMenuOpen(false);
                                            }}
                                        >
                                            {t('performance.traffic.activeListings', 'Active listings')}
                                        </li>
                                        <li
                                            className={listingStatus === 'sold' ? 'is-selected' : ''}
                                            onClick={() => {
                                                setListingStatus('sold');
                                                setStatusMenuOpen(false);
                                            }}
                                        >
                                            {t('performance.traffic.soldListings', 'Sold listings')}
                                        </li>
                                    </ul>
                                )}
                            </div>
                            <div
                                className={`filter-dropdown ${promotionMenuOpen ? 'is-open' : ''}`}
                                onClick={(event) => event.stopPropagation()}
                            >
                                <button type="button" onClick={() => setPromotionMenuOpen((prev) => !prev)}>
                                    {promotionFilter === 'promoted'
                                        ? t('performance.traffic.promotedListings', 'Promoted')
                                        : promotionFilter === 'non-promoted'
                                            ? t('performance.traffic.nonPromotedListings', 'Non-promoted')
                                            : t('performance.traffic.allListings', 'All listings')}
                                    <ChevronDownIcon />
                                </button>
                                {promotionMenuOpen && (
                                    <ul>
                                        <li
                                            className={promotionFilter === 'all' ? 'is-selected' : ''}
                                            onClick={() => {
                                                setPromotionFilter('all');
                                                setPromotionMenuOpen(false);
                                            }}
                                        >
                                            {t('performance.traffic.allListings', 'All listings')}
                                        </li>
                                        <li
                                            className={promotionFilter === 'promoted' ? 'is-selected' : ''}
                                            onClick={() => {
                                                setPromotionFilter('promoted');
                                                setPromotionMenuOpen(false);
                                            }}
                                        >
                                            {t('performance.traffic.promotedListings', 'Promoted')}
                                        </li>
                                        <li
                                            className={promotionFilter === 'non-promoted' ? 'is-selected' : ''}
                                            onClick={() => {
                                                setPromotionFilter('non-promoted');
                                                setPromotionMenuOpen(false);
                                            }}
                                        >
                                            {t('performance.traffic.nonPromotedListings', 'Non-promoted')}
                                        </li>
                                    </ul>
                                )}
                            </div>
                        </div>
                        <div className="search-download">
                            <div className="search-group" onClick={(event) => event.stopPropagation()}>
                                <label htmlFor="traffic-search" className="visually-hidden">
                                    {t('performance.traffic.searchLabel', 'Search listings')}
                                </label>
                                <MagnifyingGlassIcon className="search-icon" />
                                <input
                                    id="traffic-search"
                                    type="text"
                                    placeholder={t('performance.traffic.searchPlaceholder', 'Search by title, keywords, item number')}
                                    value={searchTerm}
                                    onChange={(event) => setSearchTerm(event.target.value)}
                                />
                            </div>
                            <button type="button" className="download-button" onClick={handleDownloadCsv}>
                                <ArrowDownTrayIcon />
                            </button>
                        </div>
                    </div>
                </div>

                <div className="listings-table">
                    <table>
                        <thead>
                            <tr>
                                <th>{t('performance.traffic.table.photo', 'Photo')}</th>
                                <th>{t('performance.traffic.table.title', 'Title')}</th>
                                <th>{t('performance.traffic.table.impressions', 'Impressions')}</th>
                                <th>{t('performance.traffic.table.ebayViews', 'eBay views')}</th>
                                <th>{t('performance.traffic.table.externalViews', 'External views')}</th>
                                <th>{t('performance.traffic.table.quantitySold', 'Quantity sold')}</th>
                                <th>{t('performance.traffic.table.ctr', 'Click-through rate')}</th>
                                <th>{t('performance.traffic.table.conversion', 'Sales conversion rate')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filteredListings.length === 0 ? (
                                <tr>
                                    <td colSpan="8">{t('performance.traffic.emptyListings', data.listingsEmpty)}</td>
                                </tr>
                            ) : (
                                filteredListings.map((listing) => (
                                    <tr key={listing.id}>
                                        <td>
                                            {listing.imageUrl ? (
                                                <img src={listing.imageUrl} alt={listing.title} className="listing-photo" />
                                            ) : (
                                                <div className="listing-photo listing-photo--placeholder" />
                                            )}
                                        </td>
                                        <td className="listing-title">{listing.title}</td>
                                        <td>{formatNumber(listing.impressions)}</td>
                                        <td>{formatNumber(listing.ebayViews)}</td>
                                        <td>{formatNumber(listing.externalViews)}</td>
                                        <td>{formatNumber(listing.quantitySold)}</td>
                                        <td>{listing.ctr}</td>
                                        <td>{listing.conversion}</td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </section>
        </div>
    );
};

export default PerformanceTrafficPage;
