import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import {
    ResponsiveContainer,
    BarChart,
    Bar,
    CartesianGrid,
    XAxis,
    YAxis,
    Tooltip
} from 'recharts';
import {
    ChevronDownIcon,
    ArrowDownTrayIcon,
    PrinterIcon,
    MagnifyingGlassIcon
} from '@heroicons/react/24/outline';
import './PerformanceSalesPage.scss';
import OrderService from '../../services/OrderService';
import { PerformanceService } from '../../services/Performance';

const SALES_OPTIONS = [
    { value: 'today', label: 'Today' },
    { value: 'last_7_days', label: 'Last 7 days' },
    { value: 'last_31_days', label: 'Last 31 days' },
    { value: 'last_90_days', label: 'Last 90 days' },
    { value: 'this_year', label: 'This year' }
];

const COMPARE_OPTIONS = [
    { value: 'previous_period', label: 'Previous period' },
    { value: 'previous_year', label: 'Previous year' },
    { value: 'none', label: 'Do not compare' }
];

const MS_PER_DAY = 24 * 60 * 60 * 1000;

const PERIOD_TO_API = {
    today: 'today',
    last_7_days: 'last_7_days',
    last_31_days: 'last_31_days',
    last_90_days: 'last_90_days',
    this_year: 'this_year',
};

const COMPARE_TO_API = {
    previous_period: 'previous_period',
    previous_year: 'previous_year',
    none: 'none',
};

const mapPeriodToApi = (value) => PERIOD_TO_API[value] ?? 'last_31_days';

const mapCompareToApi = (value) => COMPARE_TO_API[value] ?? 'previous_period';

const toNumber = (value, fallback = 0) => {
    const numeric = Number(value);
    return Number.isFinite(numeric) ? numeric : fallback;
};

const normalizeBuyerInsights = (source, fallback = {}) => {
    const data = source ?? {};
    return {
        totalBuyers: toNumber(data.totalBuyers ?? data.TotalBuyers, toNumber(fallback.totalBuyers, 0)),
        change: data.change ?? data.Change ?? fallback.change ?? 'N/A',
        oneTime: toNumber(data.oneTime ?? data.OneTime, toNumber(fallback.oneTime, 0)),
        repeat: toNumber(data.repeat ?? data.Repeat, toNumber(fallback.repeat, 0)),
        percentOfTotal: data.percentOfTotal ?? data.PercentOfTotal ?? fallback.percentOfTotal ?? '0.0%',
    };
};

const normalizeSalesReportPayload = (payload, fallbackSummary, fallbackInsights) => {
    const summaryDefaults = fallbackSummary ?? {};
    const insightsFallback = fallbackInsights ?? summaryDefaults.buyerInsights ?? {};

    if (!payload) {
        return {
            updatedAt: summaryDefaults.updatedAt ?? formatUpdatedTimestamp(),
            reportRange: summaryDefaults.reportRange ?? '',
            compareRange: summaryDefaults.compareRange ?? '',
            buyerInsights: normalizeBuyerInsights(null, insightsFallback),
            emptyListingsMessage: summaryDefaults.emptyListingsMessage ?? '',
        };
    }

    const buyerInsightsSource = payload.buyerInsights ?? payload.BuyerInsights ?? null;

    return {
        updatedAt: payload.updatedAt ?? payload.UpdatedAt ?? summaryDefaults.updatedAt ?? formatUpdatedTimestamp(),
        reportRange: payload.reportRange ?? payload.ReportRange ?? summaryDefaults.reportRange ?? '',
        compareRange: payload.compareRange ?? payload.CompareRange ?? summaryDefaults.compareRange ?? '',
        buyerInsights: normalizeBuyerInsights(buyerInsightsSource, insightsFallback),
        emptyListingsMessage: payload.emptyListingsMessage ?? payload.EmptyListingsMessage ?? summaryDefaults.emptyListingsMessage ?? '',
    };
};

const formatCurrency = (value, currency = 'USD') => new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency,
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
}).format(Number.isFinite(value) ? value : 0);

const clampDateToMidnight = (value) => {
    const date = new Date(value);
    date.setHours(0, 0, 0, 0);
    return date;
};

const createRangeFromPeriod = (period, reference = new Date()) => {
    const end = clampDateToMidnight(reference);
    const start = new Date(end);

    switch (period) {
        case 'today':
            break;
        case 'last_7_days':
            start.setDate(end.getDate() - 6);
            break;
        case 'last_31_days':
            start.setDate(end.getDate() - 30);
            break;
        case 'last_90_days':
            start.setDate(end.getDate() - 89);
            break;
        case 'this_year':
            start.setMonth(0, 1);
            break;
        default:
            start.setDate(end.getDate() - 30);
            break;
    }

    return { start, end };
};

const formatRangeLabel = (start, end) => {
    const formatter = { month: 'short', day: 'numeric', year: 'numeric' };
    const startLabel = start.toLocaleDateString('en-US', formatter);
    const endLabel = end.toLocaleDateString('en-US', formatter);
    return `${startLabel} - ${endLabel}`;
};

const differenceInDays = (start, end) => Math.max(1, Math.round((end - start) / MS_PER_DAY) + 1);

const computeComparisonRange = (range, compareMode) => {
    const days = differenceInDays(range.start, range.end);

    if (compareMode === 'none') {
        return null;
    }

    if (compareMode === 'previous_year') {
        const start = new Date(range.start);
        const end = new Date(range.end);
        start.setFullYear(start.getFullYear() - 1);
        end.setFullYear(end.getFullYear() - 1);
        return {
            start,
            end,
            label: `${formatRangeLabel(start, end)} (${days} days)`
        };
    }

    const end = new Date(range.start);
    end.setDate(end.getDate() - 1);
    const start = new Date(end);
    start.setDate(end.getDate() - (days - 1));

    return {
        start,
        end,
        label: `${formatRangeLabel(start, end)} (${days} days)`
    };
};

const formatPercentageChange = (current, previous) => {
    const safeCurrent = Number.isFinite(current) ? current : 0;
    const safePrevious = Number.isFinite(previous) ? previous : 0;

    if (safePrevious === 0) {
        return safeCurrent === 0 ? '0.0%' : 'N/A';
    }

    const change = ((safeCurrent - safePrevious) / safePrevious) * 100;
    const rounded = Math.round(change * 10) / 10;
    return `${rounded >= 0 ? '+' : ''}${rounded.toFixed(1)}%`;
};

function formatUpdatedTimestamp() {
    return new Date().toLocaleString('en-US', {
        month: 'short',
        day: 'numeric',
        hour: 'numeric',
        minute: '2-digit'
    });
}

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

const getBuyerIdentifier = (order) => (
    order?.buyerId ??
    order?.BuyerId ??
    order?.buyer?.id ??
    order?.buyer?.Id ??
    order?.buyerUsername ??
    order?.BuyerUsername ??
    order?.buyer?.username ??
    order?.Buyer?.Username ??
    null
);

const getOrderCurrency = (order) => (
    order?.total?.currency ??
    order?.total?.Currency ??
    order?.Total?.currency ??
    order?.Total?.Currency ??
    order?.currency ??
    order?.Currency ??
    null
);

const getOrderDateKey = (order) => {
    const raw = order?.paidAt ?? order?.PaidAt ?? order?.orderedAt ?? order?.OrderedAt;
    if (!raw) {
        return null;
    }

    const date = new Date(raw);
    if (Number.isNaN(date.getTime())) {
        return null;
    }

    date.setHours(0, 0, 0, 0);
    return date.toISOString().slice(0, 10);
};

const computeTotalSalesAmount = (orders) => orders.reduce(
    (sum, order) => sum + parseMoneyValue(order?.total ?? order?.Total ?? order?.totalAmount ?? order?.TotalAmount),
    0
);

const detectCurrencyFromOrders = (orders) => {
    for (const order of orders) {
        const currency = getOrderCurrency(order);
        if (currency) {
            return currency;
        }
    }

    return 'USD';
};

const buildChartSeries = (orders, range) => {
    const totals = new Map();

    orders.forEach((order) => {
        const key = getOrderDateKey(order);
        if (!key) {
            return;
        }

        const currentTotal = totals.get(key) ?? 0;
        const amount = parseMoneyValue(order?.total ?? order?.Total ?? order?.totalAmount ?? order?.TotalAmount);
        totals.set(key, currentTotal + amount);
    });

    const start = new Date(range.start);
    start.setHours(0, 0, 0, 0);
    const end = new Date(range.end);
    end.setHours(0, 0, 0, 0);

    const data = [];
    for (let cursor = new Date(start); cursor <= end; cursor = new Date(cursor.getTime() + MS_PER_DAY)) {
        const key = cursor.toISOString().slice(0, 10);
        const label = cursor.toLocaleDateString('en-US', { month: 'short', day: 'numeric' });
        const total = totals.get(key) ?? 0;
        data.push({ label, sales: Number(total.toFixed(2)) });
    }

    return data;
};

const aggregateListings = (orders) => {
    const aggregated = new Map();

    orders.forEach((order) => {
        const items = getOrderItems(order);
        if (items.length === 0) {
            return;
        }

        const subTotal = parseMoneyValue(order?.subTotal ?? order?.SubTotal);
        const taxes = parseMoneyValue(order?.taxAmount ?? order?.TaxAmount);
        const shipping = parseMoneyValue(order?.shippingCost ?? order?.ShippingCost);
        const platform = parseMoneyValue(order?.platformFee ?? order?.PlatformFee);
        const discount = parseMoneyValue(order?.discountAmount ?? order?.DiscountAmount);

        const itemsTotal = items.reduce((sum, item) => sum + parseMoneyValue(item?.totalPrice ?? item?.TotalPrice), 0);
        const allocationBase = subTotal > 0 ? subTotal : (itemsTotal > 0 ? itemsTotal : 0);

        const totalQuantity = items.reduce((sum, item) => sum + Number(item?.quantity ?? item?.Quantity ?? 0), 0);
        const quantityBase = totalQuantity > 0 ? totalQuantity : items.length;

        items.forEach((item) => {
            const quantity = Number(item?.quantity ?? item?.Quantity ?? 0) || 0;
            const lineTotal = parseMoneyValue(item?.totalPrice ?? item?.TotalPrice);
            const title = item?.title ?? item?.Title ?? 'Untitled listing';
            const listingId = item?.listingId ?? item?.ListingId ?? null;

            const key = listingId ? `listing-${listingId}` : `title-${title.toLowerCase()}`;

            const share = allocationBase > 0
                ? Math.min(Math.max(lineTotal / allocationBase, 0), 1)
                : quantityBase > 0
                    ? Math.min(Math.max(quantity / quantityBase, 0), 1)
                    : 0;

            const taxShare = taxes * share;
            const shippingShare = shipping * share;
            const platformShare = platform * share;
            const discountShare = discount * share;

            const totalCostContribution = Math.max((taxShare + shippingShare + platformShare) - discountShare, 0);
            const netSalesContribution = lineTotal - (taxShare + shippingShare + platformShare) + discountShare;

            const existing = aggregated.get(key) ?? {
                id: listingId ?? key,
                title,
                quantitySold: 0,
                totalSales: 0,
                taxes: 0,
                totalCost: 0,
                netSales: 0,
            };

            aggregated.set(key, {
                ...existing,
                quantitySold: existing.quantitySold + quantity,
                totalSales: Number((existing.totalSales + lineTotal).toFixed(2)),
                taxes: Number((existing.taxes + taxShare).toFixed(2)),
                totalCost: Number((existing.totalCost + totalCostContribution).toFixed(2)),
                netSales: Number((existing.netSales + netSalesContribution).toFixed(2)),
            });
        });
    });

    return Array.from(aggregated.values()).sort((a, b) => b.totalSales - a.totalSales);
};

const computeBuyerInsights = (orders, currentTotal, previousTotal, compareMode) => {
    const buyerCounts = new Map();

    orders.forEach((order) => {
        const key = getBuyerIdentifier(order);
        if (!key) {
            return;
        }
        buyerCounts.set(key, (buyerCounts.get(key) ?? 0) + 1);
    });

    let oneTime = 0;
    let repeat = 0;
    buyerCounts.forEach((count) => {
        if (count > 1) {
            repeat += 1;
        } else {
            oneTime += 1;
        }
    });

    const totalBuyers = buyerCounts.size;
    const percentOfTotal = totalBuyers > 0
        ? `${((repeat / totalBuyers) * 100).toFixed(1)}%`
        : '0.0%';

    const change = compareMode === 'none'
        ? 'N/A'
        : formatPercentageChange(currentTotal, previousTotal);

    return {
        totalBuyers,
        change,
        oneTime,
        repeat,
        percentOfTotal,
    };
};

const startOfDayUtcIso = (date) => {
    const copy = new Date(date);
    copy.setHours(0, 0, 0, 0);
    return copy.toISOString();
};

const endOfDayUtcIso = (date) => {
    const copy = new Date(date);
    copy.setHours(23, 59, 59, 999);
    return copy.toISOString();
};

const MAX_PAGES = 25;

const fetchOrdersForRange = async (range, signal) => {
    if (!range?.start || !range?.end) {
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

const PerformanceSalesPage = () => {
    const { t } = useTranslation('global');
    const [period, setPeriod] = useState('last_31_days');
    const [compare, setCompare] = useState('previous_period');
    const [dateRange, setDateRange] = useState(() => createRangeFromPeriod('last_31_days'));
    const [downloadOpen, setDownloadOpen] = useState(false);
    const [listingsDownloadOpen, setListingsDownloadOpen] = useState(false);
    const [searchTerm, setSearchTerm] = useState('');
    const [chartData, setChartData] = useState([]);
    const [listingsData, setListingsData] = useState([]);
    const [report, setReport] = useState({ updatedAt: '', reportRange: '', compareRange: '', buyerInsights: { totalBuyers: 0, change: 'N/A', oneTime: 0, repeat: 0, percentOfTotal: '0.0%' }, emptyListingsMessage: '' });
    const [currencyCode, setCurrencyCode] = useState('USD');
    const [isLoading, setIsLoading] = useState(false);

    const comparisonRange = useMemo(() => computeComparisonRange(dateRange, compare), [dateRange, compare]);

    const baseReportSummary = useMemo(() => ({
        updatedAt: formatUpdatedTimestamp(),
        reportRange: `${formatRangeLabel(dateRange.start, dateRange.end)} (${differenceInDays(dateRange.start, dateRange.end)} days)`,
        compareRange: comparisonRange
            ? comparisonRange.label
            : t('performance.sales.noComparisonSelected', 'No comparison selected'),
        buyerInsights: {
            totalBuyers: 0,
            change: compare === 'none' ? 'N/A' : '0.0%',
            oneTime: 0,
            repeat: 0,
            percentOfTotal: '0.0%',
        },
        emptyListingsMessage: '',
    }), [comparisonRange, compare, dateRange.end, dateRange.start, t]);

    const loadSalesData = useCallback(async (signal) => {
        setIsLoading(true);
        setReport({ ...baseReportSummary });

        const apiParams = {
            period: mapPeriodToApi(period),
            compare: mapCompareToApi(compare),
        };

        try {
            const salesReportPromise = PerformanceService.getPerformanceSales(
                apiParams,
                { signal }
            )
                .then((response) => response?.data?.data ?? response?.data ?? null)
                .catch((error) => {
                    if (!signal?.aborted) {
                        console.error('Failed to load sales report metadata', error);
                    }
                    return null;
                });

            const [reportPayload, primaryOrders, comparisonOrders] = await Promise.all([
                salesReportPromise,
                fetchOrdersForRange(dateRange, signal),
                comparisonRange
                    ? fetchOrdersForRange(comparisonRange, signal)
                    : Promise.resolve([]),
            ]);

            if (signal?.aborted) {
                return;
            }

            const detectedCurrency = detectCurrencyFromOrders(primaryOrders) || 'USD';
            const chartSeries = buildChartSeries(primaryOrders, dateRange);
            const listingRows = aggregateListings(primaryOrders);

            const currentTotalAmount = computeTotalSalesAmount(primaryOrders);
            const previousTotalAmount = comparisonRange ? computeTotalSalesAmount(comparisonOrders) : 0;
            const fallbackInsights = computeBuyerInsights(
                primaryOrders,
                currentTotalAmount,
                previousTotalAmount,
                compare
            );

            const reportMetadata = normalizeSalesReportPayload(
                reportPayload,
                baseReportSummary,
                fallbackInsights
            );

            setCurrencyCode(detectedCurrency);
            setChartData(chartSeries);
            setListingsData(listingRows);
            setReport(reportMetadata);
        } catch (error) {
            if (!signal?.aborted) {
                console.error('Failed to load sales data:', error);
            }
        } finally {
            setIsLoading(false);
        }
    }, [baseReportSummary, compare, comparisonRange, dateRange, period]);

    useEffect(() => {
        const controller = new AbortController();

        loadSalesData(controller.signal);

        return () => {
            controller.abort();
        };
    }, [loadSalesData]);

    const totalSales = useMemo(() => chartData.reduce((sum, item) => sum + (item.sales ?? 0), 0), [chartData]);
    const averageSales = useMemo(
        () => (chartData.length > 0 ? totalSales / chartData.length : 0),
        [chartData, totalSales]
    );

    const filteredListings = useMemo(() => {
        if (!searchTerm.trim()) {
            return listingsData;
        }

        const normalizedSearch = searchTerm.trim().toLowerCase();
        return listingsData.filter((listing) =>
            listing.title.toLowerCase().includes(normalizedSearch)
        );
    }, [listingsData, searchTerm]);

    const closeMenus = () => {
        setDownloadOpen(false);
        setListingsDownloadOpen(false);
    };

    const handlePeriodChange = (value) => {
        setPeriod(value);
        setDateRange(createRangeFromPeriod(value));
    };

    const handleCompareChange = (value) => {
        setCompare(value);
    };

    const handlePrint = (event) => {
        event.stopPropagation();
        window.print();
    };

    const createCsvContent = useCallback((variant) => {
        const header = variant === 'store'
            ? ['Date', 'Total Sales', 'Store Category']
            : ['Date', 'Total Sales'];

        const rows = chartData.map((entry) => {
            if (variant === 'store') {
                return [entry.label, entry.sales, 'Default'];
            }

            return [entry.label, entry.sales];
        });

        const escapeCsv = (value) => {
            const raw = String(value ?? '');
            if (/[",\n]/.test(raw)) {
                return `"${raw.replace(/"/g, '""')}"`;
            }
            return raw;
        };

        return [header, ...rows]
            .map((row) => row.map(escapeCsv).join(','))
            .join('\r\n');
    }, [chartData]);

    const downloadCsvFallback = useCallback((variant) => {
        const csv = createCsvContent(variant);
        const blob = new Blob([csv], { type: 'text/csv;charset=utf-8;' });
        const filename = variant === 'store'
            ? 'listing-sales-report-with-store-category.csv'
            : 'listing-sales-report.csv';

        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.setAttribute('download', filename);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    }, [createCsvContent]);

    const handleDownloadSelection = async (event, variant, closeMenu) => {
        event.stopPropagation();
        try {
            const params = {
                period: mapPeriodToApi(period),
                compare: mapCompareToApi(compare),
            };
            const format = variant === 'store' ? 'store' : 'standard';
            const response = await PerformanceService.downloadSalesReport(params, format);
            const blobData = response?.data;

            const blob = blobData instanceof Blob
                ? blobData
                : new Blob([blobData ?? ''], { type: 'text/csv;charset=utf-8;' });

            const filename = format === 'store'
                ? 'listing-sales-report-with-store-category.csv'
                : 'listing-sales-report.csv';

            const url = URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            URL.revokeObjectURL(url);
        } catch (error) {
            console.error('Failed to download sales report from server. Falling back to client CSV.', error);
            downloadCsvFallback(variant);
        } finally {
            closeMenu(false);
        }
    };

    return (
        <div className="performance-sales-page" onClick={closeMenus} aria-busy={isLoading}>
            <header className="sales-header">
                <div className="sales-header__copy">
                    <h1>{t('performance.sales.heading', 'Review your sales')}</h1>
                    <p>
                        {`${t('performance.sales.updatedPrefix', 'Updated')} ${report.updatedAt}. ${t('performance.sales.updatedSuffix', 'We show sales and charges on the dates they occur. Amounts may not reflect latest sales.')}`}
                    </p>
                </div>
                <a href="#" className="feedback-link">
                    {t('performance.summary.feedback', 'Tell us what you think about this page')}
                </a>
            </header>

            <section className="filters-card performance-card" onClick={(e) => e.stopPropagation()}>
                <div className="filters-card__controls">
                    <div className="filter-field">
                        <label>{t('performance.sales.salesFor', 'Sales for')}</label>
                        <div className="select-wrap">
                            <select value={period} onChange={(e) => handlePeriodChange(e.target.value)}>
                                {SALES_OPTIONS.map((option) => (
                                    <option key={option.value} value={option.value}>
                                        {t(`performance.sales.period.${option.value}`, option.label)}
                                    </option>
                                ))}
                            </select>
                            <ChevronDownIcon />
                        </div>
                    </div>
                    <div className="filter-field">
                        <label>{t('performance.sales.comparedTo', 'Compared to')}</label>
                        <div className="select-wrap">
                            <select value={compare} onChange={(e) => handleCompareChange(e.target.value)}>
                                {COMPARE_OPTIONS.map((option) => (
                                    <option key={option.value} value={option.value}>
                                        {t(`performance.sales.compare.${option.value}`, option.label)}
                                    </option>
                                ))}
                            </select>
                            <ChevronDownIcon />
                        </div>
                    </div>
                </div>
                <div className="filters-card__actions">
                    <button
                        type="button"
                        className="generate-btn"
                        onClick={() => loadSalesData()}
                        disabled={isLoading}
                    >
                        {isLoading
                            ? t('performance.sales.generating', 'Generating...')
                            : t('performance.sales.generate', 'Generate report')
                        }
                    </button>
                    <button
                        type="button"
                        className="reset-btn"
                        onClick={() => {
                            handlePeriodChange('last_31_days');
                            handleCompareChange('previous_period');
                        }}
                    >
                        {t('performance.sales.reset', 'Reset')}
                    </button>
                </div>
            </section>

            <section className="performance-card report-card">
                <div className="report-card__header">
                    <div>
                        <h2>{`${t('performance.sales.reportForPrefix', 'Report for')} ${report.reportRange}`}</h2>
                        <p>{`${t('performance.sales.comparedRangePrefix', 'Compared to')} ${report.compareRange}`}</p>
                    </div>
                    <div className="report-card__actions" onClick={(e) => e.stopPropagation()}>
                        <button type="button" className="text-link" onClick={handlePrint}>
                            <PrinterIcon />
                            {t('performance.sales.printReport', 'Print report')}
                        </button>
                        <div className={`download-menu ${downloadOpen ? 'is-open' : ''}`}>
                            <button
                                type="button"
                                className="download-btn"
                                onClick={(e) => {
                                    e.stopPropagation();
                                    setDownloadOpen((prev) => !prev);
                                }}
                            >
                                <ArrowDownTrayIcon />
                                {t('performance.sales.download', 'Download')}
                                <ChevronDownIcon />
                            </button>
                            {downloadOpen && (
                                <ul>
                                    <li onClick={(event) => handleDownloadSelection(event, 'standard', setDownloadOpen)}>
                                        {t('performance.sales.download.listingSales', 'Listings sales report')}
                                    </li>
                                    <li onClick={(event) => handleDownloadSelection(event, 'store', setDownloadOpen)}>
                                        {t('performance.sales.download.listingSalesStore', 'Listings sales report with Store Category')}
                                    </li>
                                </ul>
                            )}
                        </div>
                    </div>
                </div>

                <div className="sales-chart">
                    <div className="sales-chart__summary">
                        <div className="sales-chart__metric">
                            <span className="label">{t('performance.sales.totalSales', 'Total sales')}</span>
                            <span className="value">{formatCurrency(totalSales, currencyCode)}</span>
                        </div>
                        <div className="sales-chart__metric">
                            <span className="label">{t('performance.sales.averageDailySales', 'Average daily sales')}</span>
                            <span className="value">{formatCurrency(averageSales, currencyCode)}</span>
                        </div>
                    </div>
                    <div className="sales-chart__visual">
                        <ResponsiveContainer width="100%" height={320}>
                            <BarChart data={chartData}>
                                <CartesianGrid vertical={false} stroke="#e5e7eb" strokeDasharray="3 3" />
                                <XAxis
                                    dataKey="label"
                                    tickLine={false}
                                    axisLine={{ stroke: '#e5e7eb' }}
                                    minTickGap={16}
                                />
                                <YAxis
                                    tickLine={false}
                                    axisLine={{ stroke: '#e5e7eb' }}
                                />
                                <Tooltip
                                    cursor={{ fill: 'rgba(29, 78, 216, 0.12)' }}
                                    formatter={(value) => [formatCurrency(value, currencyCode), t('performance.sales.tooltipSales', 'Sales')]}
                                />
                                <Bar dataKey="sales" fill="#1d4ed8" radius={[6, 6, 0, 0]} />
                            </BarChart>
                        </ResponsiveContainer>
                    </div>
                </div>

                <div className="buyer-insights">
                    <div className="buyer-insights__header">
                        <h3>{t('performance.sales.buyerInsights', 'Buyer insights')}</h3>
                        <span>{t('performance.sales.compareToPrior', 'Compared to the prior time period')}</span>
                    </div>
                    <div className="buyer-insights__grid">
                        <div className="buyer-insight">
                            <span className="label">{t('performance.sales.totalBuyers', 'Total buyers')}</span>
                            <div className="buyer-insight__metrics">
                                <span className="value">{report.buyerInsights.totalBuyers}</span>
                                <span className="delta">{report.buyerInsights.change}</span>
                            </div>
                        </div>
                        <div className="buyer-insight">
                            <span className="label">{t('performance.sales.breakdown', 'Breakdown')}</span>
                            <div className="breakdown-row">
                                <span>{t('performance.sales.oneTime', 'One-time buyers')}</span>
                                <span>{report.buyerInsights.oneTime}</span>
                            </div>
                            <div className="breakdown-row">
                                <span>{t('performance.sales.repeat', 'Repeat buyers')}</span>
                                <span>{report.buyerInsights.repeat}</span>
                            </div>
                        </div>
                        <div className="buyer-insight">
                            <span className="label">{t('performance.sales.percentTotal', 'Percent of total')}</span>
                            <span className="value">{report.buyerInsights.percentOfTotal}</span>
                        </div>
                    </div>
                </div>
            </section>

            <section className="performance-card listings-card" onClick={(e) => e.stopPropagation()}>
                <div className="listings-card__header">
                    <div className="listings-card__header-left">
                        <h2>{t('performance.sales.listingsInsights', 'Listings insights')}</h2>
                        <div className="listing-tabs">
                            <button type="button" className="tab is-active">{t('performance.sales.filter.soldAdvertising', 'Sold via advertising (0)')}</button>
                            <button type="button" className="tab">{t('performance.sales.filter.soldBestOffer', 'Sold via Best offer (0)')}</button>
                            <button type="button" className="tab">{t('performance.sales.filter.soldInitiated', 'Sold via Seller initiated offer (0)')}</button>
                        </div>
                    </div>
                    <div className="listings-card__header-right">
                        <div className="search-group">
                            <label htmlFor="sales-search" className="visually-hidden">{t('performance.sales.searchListings', 'Search listings')}</label>
                            <input
                                id="sales-search"
                                type="text"
                                value={searchTerm}
                                placeholder={t('performance.sales.searchPlaceholder', 'Search by Item Title, Keywords')}
                                onChange={(event) => setSearchTerm(event.target.value)}
                            />
                            <button
                                type="button"
                                className="icon-btn"
                                onClick={(event) => event.stopPropagation()}
                            >
                                <MagnifyingGlassIcon />
                            </button>
                        </div>
                        <div className="listings-card__actions">
                            <div className={`download-menu small ${listingsDownloadOpen ? 'is-open' : ''}`}>
                                <button
                                    type="button"
                                    className="download-btn"
                                    onClick={(e) => {
                                        e.stopPropagation();
                                        setListingsDownloadOpen((prev) => !prev);
                                    }}
                                >
                                    <ArrowDownTrayIcon />
                                    {t('performance.sales.download', 'Download')}
                                    <ChevronDownIcon />
                                </button>
                                {listingsDownloadOpen && (
                                    <ul>
                                        <li onClick={(event) => handleDownloadSelection(event, 'standard', setListingsDownloadOpen)}>
                                            {t('performance.sales.download.listingSales', 'Listings sales report')}
                                        </li>
                                        <li onClick={(event) => handleDownloadSelection(event, 'store', setListingsDownloadOpen)}>
                                            {t('performance.sales.download.listingSalesStore', 'Listings sales report with Store Category')}
                                        </li>
                                    </ul>
                                )}
                            </div>
                        </div>
                    </div>
                </div>

                <div className="listings-card__table">
                    <table>
                        <thead>
                            <tr>
                                <th>{t('performance.sales.listing', 'Listing')}</th>
                                <th>{t('performance.sales.quantitySold', 'Quantity sold')}</th>
                                <th>{t('performance.sales.totalSales', 'Total sales')}</th>
                                <th>{t('performance.sales.taxes', 'Taxes and government fees')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {filteredListings.length === 0 ? (
                                <tr>
                                    <td colSpan="4">{t('performance.sales.noListings', report.emptyListingsMessage)}</td>
                                </tr>
                            ) : (
                                filteredListings.map((listing) => (
                                    <tr key={listing.id}>
                                        <td>{listing.title}</td>
                                        <td>{listing.quantitySold}</td>
                                        <td>{formatCurrency(listing.totalSales, currencyCode)}</td>
                                        <td>{formatCurrency(listing.taxes, currencyCode)}</td>
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

export default PerformanceSalesPage;