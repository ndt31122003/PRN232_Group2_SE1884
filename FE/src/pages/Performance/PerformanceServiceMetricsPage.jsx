import React, { useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { ChevronDownIcon } from '@heroicons/react/24/outline';
import {
    ResponsiveContainer,
    LineChart,
    Line,
    CartesianGrid,
    XAxis,
    YAxis,
    Tooltip,
    Legend
} from 'recharts';
import { LoadingScreen } from '../../components/LoadingScreen/LoadingScreen';
import { PerformanceService } from '../../services/Performance';
import './PerformanceServiceMetricsPage.scss';

const METRIC_TABS = [
    { id: 'itemNotAsDescribed', label: 'Item not as described' },
    { id: 'itemNotReceived', label: 'Item not received' }
];

const MARKETPLACE_OPTIONS = [
    { id: 'ebay-com', label: 'ebay.com' },
    { id: 'ebay-uk', label: 'ebay.co.uk' },
    { id: 'ebay-de', label: 'ebay.de' }
];

const PROGRAM_OPTIONS = [
    { id: 'all', label: 'All categories' },
    { id: 'motors', label: 'eBay Motors (I49)' },
    { id: 'collectibles', label: 'Collectibles' }
];

const PERIOD_OPTIONS = [
    { id: 'current', label: 'Current rate' },
    { id: 'previous', label: 'Previous year' },
    { id: 'custom', label: 'Custom range' }
];

const BEST_PRACTICES = [
    {
        id: 'tracking',
        title: 'Upload tracking',
        description: 'Adding tracking info may reduce your chances of getting item not received requests by up to 60%.'
    },
    {
        id: 'ship-on-time',
        title: 'Ship on time',
        description: 'Sending the item within the handling time you agreed to may decrease the number of item not received requests by up to 65%.'
    },
    {
        id: 'service',
        title: 'Choose the right service',
        description: 'Using the exact shipping service that the buyer requested may also help you reduce the number of item not received requests.'
    }
];

const REASON_CATEGORIES = [
    {
        id: 'missingParts',
        label: 'Missing parts or pieces',
        color: '#a855f7'
    },
    {
        id: 'arrivedDamaged',
        label: 'Arrived damaged',
        color: '#1d4ed8'
    },
    {
        id: 'defective',
        label: 'Not working or defective',
        color: '#0f766e'
    },
    {
        id: 'wrongItem',
        label: 'Wrong item',
        color: '#f97316'
    },
    {
        id: 'notMatch',
        label: "Doesn't match description or photos",
        color: '#22c55e'
    },
    {
        id: 'notAuthentic',
        label: 'Not authentic',
        color: '#6366f1'
    }
];

const REASON_CATEGORY_MAP = REASON_CATEGORIES.reduce((accumulator, category) => {
    accumulator[category.id] = category;
    return accumulator;
}, {});

const CLASSIFICATION_FALLBACK_LABELS = {
    low: 'Low',
    average: 'Average',
    'above-average': 'Above average',
    high: 'High',
    'very-high': 'Very high'
};

const DEFAULT_GAUGE_MAX = 5;

const clamp = (value, min, max) => Math.max(min, Math.min(max, value));

const normalizeRateValue = (value) => {
    if (value === null || value === undefined) {
        return 0;
    }

    const cleaned = typeof value === 'string' ? value.replace(/%/g, '').trim() : value;
    const numeric = Number(cleaned ?? 0);
    if (!Number.isFinite(numeric)) {
        return 0;
    }

    return Math.abs(numeric) <= 1 ? numeric * 100 : numeric;
};

const formatPercentLabel = (percent) => `${(Number.isFinite(percent) ? percent : 0).toFixed(2)}%`;

const formatNumber = (value) => Number(value ?? 0).toLocaleString('en-US');

const formatFullDate = (date) => date.toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric'
});

const formatRangeLabel = (start, end) => {
    const startLabel = start.toLocaleDateString('en-US', { month: 'short', year: 'numeric' });
    const endLabel = end.toLocaleDateString('en-US', { month: 'short', year: 'numeric' });
    return `${startLabel} - ${endLabel}`;
};

const toGaugePosition = (ratePercent, scaleMax) => {
    const safeMax = scaleMax > 0 ? scaleMax : DEFAULT_GAUGE_MAX;
    const clamped = clamp(ratePercent, 0, safeMax);
    return (clamped / safeMax) * 100;
};

const toDateOrNull = (value) => {
    if (!value) {
        return null;
    }

    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? null : date;
};

const computeAnalysisRange = (periodKey) => {
    const today = new Date();
    today.setHours(23, 59, 59, 999);

    if (periodKey === 'previous') {
        const year = today.getFullYear() - 1;
        const start = new Date(year, 0, 1, 0, 0, 0, 0);
        const end = new Date(year, 11, 31, 23, 59, 59, 999);
        return { start, end };
    }

    if (periodKey === 'custom') {
        const end = new Date(today.getFullYear(), today.getMonth(), today.getDate(), 23, 59, 59, 999);
        const start = new Date(end);
        start.setMonth(start.getMonth() - 6);
        start.setHours(0, 0, 0, 0);
        return { start, end };
    }

    const end = today;
    const start = new Date(end);
    start.setFullYear(start.getFullYear() - 1);
    start.setDate(start.getDate() + 1);
    start.setHours(0, 0, 0, 0);
    return { start, end };
};

const buildReasonDistribution = (reasons) => {
    const baseline = REASON_CATEGORIES.map(({ id, label, color }) => ({
        id,
        label,
        color,
        count: 0
    }));

    if (!Array.isArray(reasons)) {
        return baseline;
    }

    const additional = new Map();

    reasons.forEach((reason) => {
        if (!reason) {
            return;
        }

        const rawId = (reason.id ?? reason.Id ?? '').toString().trim();
        const normalizedId = rawId || (reason.label ?? reason.Label ?? 'other');
        const count = Number(reason.count ?? reason.Count ?? 0);
        const label = reason.label ?? reason.Label ?? normalizedId;

        const category = REASON_CATEGORY_MAP[normalizedId];
        if (category) {
            const index = baseline.findIndex((entry) => entry.id === category.id);
            if (index >= 0) {
                baseline[index] = {
                    ...baseline[index],
                    count: baseline[index].count + count
                };
            }
            return;
        }

        const existing = additional.get(normalizedId);
        if (existing) {
            existing.count += count;
        } else {
            additional.set(normalizedId, {
                id: normalizedId,
                label,
                color: '#6b7280',
                count
            });
        }
    });

    return [
        ...baseline,
        ...Array.from(additional.values())
    ];
};

const normalizeMetric = (metric) => {
    const rate = Number(metric?.rate ?? metric?.Rate ?? 0);
    const peerRate = Number(metric?.peerRate ?? metric?.PeerRate ?? 0);

    const ratePercent = normalizeRateValue(rate);
    const peerRatePercent = normalizeRateValue(peerRate);
    const gaugeMax = Math.max(DEFAULT_GAUGE_MAX, ratePercent, peerRatePercent);

    const classificationKey = String(metric?.classification ?? metric?.Classification ?? 'low').toLowerCase();

    return {
        id: (metric?.metricId ?? metric?.MetricId ?? '').toString(),
        totalTransactions: Number(metric?.totalTransactions ?? metric?.TotalTransactions ?? 0),
        issueCount: Number(metric?.issueCount ?? metric?.IssueCount ?? 0),
        rate: ratePercent,
        rateLabel: metric?.rateLabel ?? metric?.RateLabel ?? formatPercentLabel(ratePercent),
        peerRate: peerRatePercent,
        peerRateLabel: metric?.peerRateLabel ?? metric?.PeerRateLabel ?? formatPercentLabel(peerRatePercent),
        classification: classificationKey,
        uniqueBuyerCount: Number(metric?.uniqueBuyerCount ?? metric?.UniqueBuyerCount ?? 0),
        youGaugeValue: toGaugePosition(ratePercent, gaugeMax),
        peerGaugeValue: toGaugePosition(peerRatePercent, gaugeMax),
        reasons: buildReasonDistribution(metric?.reasons ?? metric?.Reasons),
        trendData: normalizeTrendData(metric?.trendData ?? metric?.TrendData)
    };
};

const normalizeTrendData = (rawData) => {
    if (!Array.isArray(rawData) || rawData.length === 0) {
        return [];
    }

    return rawData.map((point) => ({
        date: point.date ?? point.Date ?? '',
        label: point.label ?? point.Label ?? '',
        yourRate: normalizeRateValue(point.yourRate ?? point.YourRate ?? 0),
        peerRate: normalizeRateValue(point.peerRate ?? point.PeerRate ?? 0)
    }));
};

const createEmptyMetricsData = (periodKey) => ({
    periodKey,
    rangeStart: null,
    rangeEnd: null,
    itemNotAsDescribed: normalizeMetric(),
    itemNotReceived: normalizeMetric()
});

const PerformanceServiceMetricsPage = () => {
    const { t } = useTranslation('global');
    const [activeTab, setActiveTab] = useState(METRIC_TABS[0].id);
    const [marketplace, setMarketplace] = useState(MARKETPLACE_OPTIONS[0].id);
    const [program, setProgram] = useState(PROGRAM_OPTIONS[1].id);
    const [selectedPeriod, setSelectedPeriod] = useState(PERIOD_OPTIONS[0].id);
    const [metricsData, setMetricsData] = useState(() => createEmptyMetricsData(PERIOD_OPTIONS[0].id));
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const controller = new AbortController();
        let mounted = true;

        const loadData = async () => {
            setLoading(true);
            setError(null);

            try {
                const response = await PerformanceService.getPerformanceServiceMetrics(
                    selectedPeriod,
                    { signal: controller.signal }
                );

                if (!mounted || controller.signal.aborted) {
                    return;
                }

                // API returns data directly in response.data
                const payload = response?.data ?? null;
                console.log('Service Metrics API Response:', payload);

                if (!payload || !payload.itemNotAsDescribed || !payload.itemNotReceived) {
                    setMetricsData(createEmptyMetricsData(selectedPeriod));
                    setError('Unable to load service metrics data.');
                    return;
                }

                const normalizedPayload = {
                    periodKey: payload.periodKey ?? payload.PeriodKey ?? selectedPeriod,
                    rangeStart: payload.rangeStart ?? payload.RangeStart ?? null,
                    rangeEnd: payload.rangeEnd ?? payload.RangeEnd ?? null,
                    itemNotAsDescribed: normalizeMetric(payload.itemNotAsDescribed ?? payload.ItemNotAsDescribed),
                    itemNotReceived: normalizeMetric(payload.itemNotReceived ?? payload.ItemNotReceived)
                };

                console.log('Normalized Metrics Data:', normalizedPayload);
                console.log('Item Not As Described:', normalizedPayload.itemNotAsDescribed);
                console.log('Item Not Received:', normalizedPayload.itemNotReceived);
                setMetricsData(normalizedPayload);
            } catch (err) {
                if (!mounted || controller.signal.aborted) {
                    return;
                }

                setMetricsData(createEmptyMetricsData(selectedPeriod));
                setError('Unable to load service metrics data.');
            } finally {
                if (mounted && !controller.signal.aborted) {
                    setLoading(false);
                }
            }
        };

        loadData();

        return () => {
            mounted = false;
            controller.abort();
        };
    }, [selectedPeriod]);

    const metricsByTab = useMemo(() => ({
        itemNotAsDescribed: metricsData.itemNotAsDescribed,
        itemNotReceived: metricsData.itemNotReceived
    }), [metricsData]);

    const rangeStartDate = useMemo(() => toDateOrNull(metricsData.rangeStart), [metricsData.rangeStart]);
    const rangeEndDate = useMemo(() => toDateOrNull(metricsData.rangeEnd), [metricsData.rangeEnd]);

    const rangeLabel = useMemo(() => {
        if (!rangeStartDate || !rangeEndDate) {
            return '';
        }

        return formatRangeLabel(rangeStartDate, rangeEndDate);
    }, [rangeStartDate, rangeEndDate]);

    const resolvePeriodOptionLabel = (option) => {
        const periodLabels = {
            current: t('performance.serviceMetrics.currentRate', 'Current rate'),
            previous: t('performance.serviceMetrics.previousRate', 'Previous year'),
            custom: t('performance.serviceMetrics.customRange', 'Custom range (last 6 months)')
        };

        if (option.id === selectedPeriod && rangeStartDate && rangeEndDate) {
            const prefix = periodLabels[option.id] ?? option.label;
            return `${prefix}: ${rangeLabel}`;
        }

        const preview = computeAnalysisRange(option.id);
        if (!preview) {
            return t(`performance.serviceMetrics.period.${option.id}`, option.label);
        }

        const label = formatRangeLabel(preview.start, preview.end);
        const prefix = periodLabels[option.id] ?? option.label;
        return `${prefix}: ${label}`;
    };

    const metrics = metricsByTab[activeTab] ?? normalizeMetric();
    const hasSales = metrics.totalTransactions > 0;

    const lastYearDate = useMemo(() => {
        const reference = new Date();
        reference.setFullYear(reference.getFullYear() - 1);
        reference.setHours(0, 0, 0, 0);
        return reference;
    }, []);

    const classificationText = useMemo(() => {
        const fallback = CLASSIFICATION_FALLBACK_LABELS[metrics.classification] ?? metrics.classification;
        return t(`performance.serviceMetrics.classification.${metrics.classification}`, fallback);
    }, [metrics.classification, t]);

    if (loading && !hasSales && !error) {
        return <LoadingScreen />;
    }

    return (
        <div className="service-metrics-page" aria-busy={loading}>
            <header className="service-metrics-header">
                <div>
                    <h1>{t('performance.serviceMetrics.heading', 'Service Metrics')}</h1>
                    <p>
                        {t(
                            'performance.serviceMetrics.description',
                            'Good service is good for business. This dashboard shows you how your service levels compare to other sellers with a similar selling profile.'
                        )}
                    </p>
                </div>
                <a href="#" className="service-metrics-feedback">
                    {t('performance.serviceMetrics.feedback', 'See your seller level')}
                </a>
            </header>

            <section className="service-metrics-card performance-card">
                <div className="service-metrics-tabs">
                    {METRIC_TABS.map((tab) => (
                        <button
                            key={tab.id}
                            type="button"
                            className={tab.id === activeTab ? 'service-metrics-tab is-active' : 'service-metrics-tab'}
                            onClick={() => setActiveTab(tab.id)}
                        >
                            {t(`performance.serviceMetrics.tabs.${tab.id}`, tab.label)}
                        </button>
                    ))}
                </div>

                <div className="service-metrics-filters">
                    <div className="service-metrics-filter">
                        <label htmlFor="service-metrics-site">
                            {t('performance.serviceMetrics.siteLabel', 'Site')}
                        </label>
                        <div className="service-metrics-filter__select">
                            <select
                                id="service-metrics-site"
                                value={marketplace}
                                onChange={(event) => setMarketplace(event.target.value)}
                                aria-label={t('performance.serviceMetrics.siteLabel', 'Site')}
                            >
                                {MARKETPLACE_OPTIONS.map((option) => (
                                    <option key={option.id} value={option.id}>
                                        {option.label}
                                    </option>
                                ))}
                            </select>
                            <ChevronDownIcon />
                        </div>
                    </div>
                    <div className="service-metrics-filter">
                        <label htmlFor="service-metrics-program">
                            {t('performance.serviceMetrics.programLabel', 'Program')}
                        </label>
                        <div className="service-metrics-filter__select">
                            <select
                                id="service-metrics-program"
                                value={program}
                                onChange={(event) => setProgram(event.target.value)}
                                aria-label={t('performance.serviceMetrics.programLabel', 'Program')}
                            >
                                {PROGRAM_OPTIONS.map((option) => (
                                    <option key={option.id} value={option.id}>
                                        {option.label}
                                    </option>
                                ))}
                            </select>
                            <ChevronDownIcon />
                        </div>
                    </div>
                    <div className="service-metrics-filter">
                        <label htmlFor="service-metrics-period">
                            {t('performance.serviceMetrics.periodLabel', 'Current rate')}
                        </label>
                        <div className="service-metrics-filter__select">
                            <select
                                id="service-metrics-period"
                                value={selectedPeriod}
                                onChange={(event) => setSelectedPeriod(event.target.value)}
                                aria-label={t('performance.serviceMetrics.periodLabel', 'Current rate')}
                            >
                                {PERIOD_OPTIONS.map((option) => (
                                    <option key={option.id} value={option.id}>
                                        {resolvePeriodOptionLabel(option)}
                                    </option>
                                ))}
                            </select>
                            <ChevronDownIcon />
                        </div>
                    </div>
                </div>

                {error && !hasSales ? (
                    <div className="service-metrics-empty">
                        <p>{error}</p>
                    </div>
                ) : !hasSales ? (
                    <div className="service-metrics-empty">
                        <p>
                            {t(
                                'performance.serviceMetrics.noSales',
                                'You have no sales since {{date}}',
                                { date: formatFullDate(lastYearDate) }
                            )}
                        </p>
                    </div>
                ) : (
                    <div className="service-metrics-body">
                        <div className="service-metrics-panel">
                            <div className="service-metrics-panel__copy">
                                <h2>
                                    {activeTab === 'itemNotAsDescribed'
                                        ? t('performance.serviceMetrics.panel.describedTitle', 'Item not as described returns')
                                        : t('performance.serviceMetrics.panel.receivedTitle', 'Item not received requests')}
                                </h2>
                                <p>
                                    {t('performance.serviceMetrics.panel.compareCopy', 'See how you compare to your peers.')}
                                </p>
                                <div className="service-metrics-panel__details">
                                    {rangeLabel && (
                                        <p>
                                            Current rate: {rangeLabel}
                                        </p>
                                    )}
                                    <p>
                                        Your rate: {metrics.rateLabel} ({classificationText})
                                    </p>
                                    <p>
                                        Total transactions: {formatNumber(metrics.totalTransactions)}
                                    </p>
                                    <p>
                                        {activeTab === 'itemNotAsDescribed' ? 'Item not as described' : 'Not received'}: {formatNumber(metrics.issueCount)}
                                    </p>
                                    <p>
                                        Unique buyers: {formatNumber(metrics.uniqueBuyerCount)}
                                    </p>
                                    <p className="service-metrics-panel__formula">
                                        {`${formatNumber(metrics.issueCount)} / ${formatNumber(metrics.totalTransactions)} = ${metrics.rateLabel}`}
                                    </p>
                                    <a href="#" className="service-metrics-panel__link">
                                        {t('performance.serviceMetrics.learnMore', 'Learn More')}
                                    </a>
                                </div>
                            </div>
                            <div className="service-metrics-gauge" aria-hidden="true">
                                <div className="service-metrics-gauge__scale">
                                    <span className="service-metrics-gauge__band service-metrics-gauge__band--very-high">
                                        {t('performance.serviceMetrics.scale.veryHigh', 'Very high')}
                                    </span>
                                    <span className="service-metrics-gauge__band service-metrics-gauge__band--high">
                                        {t('performance.serviceMetrics.scale.high', 'High')}
                                    </span>
                                    <span className="service-metrics-gauge__band service-metrics-gauge__band--average">
                                        {t('performance.serviceMetrics.scale.average', 'Average')}
                                    </span>
                                    <span className="service-metrics-gauge__band service-metrics-gauge__band--low">
                                        {t('performance.serviceMetrics.scale.low', 'Low')}
                                    </span>
                                    <div className="service-metrics-gauge__bar" />
                                    <div
                                        className="service-metrics-gauge__marker service-metrics-gauge__marker--peer"
                                        style={{ bottom: `${metrics.peerGaugeValue}%` }}
                                    >
                                        <span>{t('performance.serviceMetrics.peerLabel', 'Peers')}</span>
                                        <strong>{metrics.peerRateLabel}</strong>
                                    </div>
                                    <div
                                        className="service-metrics-gauge__marker service-metrics-gauge__marker--you"
                                        style={{ bottom: `${metrics.youGaugeValue}%` }}
                                    >
                                        <span>{t('performance.serviceMetrics.youLabel', 'You')}</span>
                                        <strong>{metrics.rateLabel}</strong>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {metrics.trendData && metrics.trendData.length > 0 && (
                            <div className="service-metrics-chart">
                                <h3>{t('performance.serviceMetrics.trendTitle', 'Rate trend over time')}</h3>
                                <ResponsiveContainer width="100%" height={300}>
                                    <LineChart data={metrics.trendData}>
                                        <CartesianGrid strokeDasharray="3 3" stroke="#e5e7eb" />
                                        <XAxis
                                            dataKey="label"
                                            tickLine={false}
                                            axisLine={{ stroke: '#e5e7eb' }}
                                        />
                                        <YAxis
                                            tickLine={false}
                                            axisLine={{ stroke: '#e5e7eb' }}
                                            tickFormatter={(value) => `${value.toFixed(2)}%`}
                                        />
                                        <Tooltip
                                            formatter={(value) => `${Number(value).toFixed(2)}%`}
                                        />
                                        <Legend />
                                        <Line
                                            type="monotone"
                                            dataKey="yourRate"
                                            name={t('performance.serviceMetrics.yourRate', 'Your rate')}
                                            stroke="#1d4ed8"
                                            strokeWidth={2}
                                            dot={{ r: 4 }}
                                        />
                                        <Line
                                            type="monotone"
                                            dataKey="peerRate"
                                            name={t('performance.serviceMetrics.peerRate', 'Peer rate')}
                                            stroke="#9ca3af"
                                            strokeWidth={2}
                                            strokeDasharray="5 5"
                                            dot={{ r: 4 }}
                                        />
                                    </LineChart>
                                </ResponsiveContainer>
                            </div>
                        )}

                        <aside className="service-metrics-side">
                            {activeTab === 'itemNotAsDescribed' ? (
                                <div className="service-metrics-reasons">
                                    <h3>{t('performance.serviceMetrics.reasonsTitle', 'The reasons for your returns')}</h3>
                                    <p>{t('performance.serviceMetrics.reasonsCopy', 'Take a look at how your returns break down.')}</p>
                                    <div className="service-metrics-reasons__summary">
                                        <span className="service-metrics-reasons__count">{formatNumber(metrics.issueCount)}</span>
                                        <span className="service-metrics-reasons__label">
                                            {t('performance.serviceMetrics.reasonsCount', 'Returns')}
                                        </span>
                                    </div>
                                    <ul className="service-metrics-reasons__list">
                                        {metrics.reasons.map((reason) => (
                                            <li key={reason.id}>
                                                <span
                                                    className="service-metrics-reasons__dot"
                                                    style={{ backgroundColor: reason.color }}
                                                />
                                                <span className="service-metrics-reasons__text">{reason.label}</span>
                                                <span className="service-metrics-reasons__value">{formatNumber(reason.count)}</span>
                                            </li>
                                        ))}
                                    </ul>
                                </div>
                            ) : (
                                <div className="service-metrics-practices">
                                    <h3>{t('performance.serviceMetrics.bestPractices', 'Best practices')}</h3>
                                    <p>{t('performance.serviceMetrics.bestPracticesCopy', 'Follow these simple tips to minimize problems with delivery.')}</p>
                                    <ul>
                                        {BEST_PRACTICES.map((practice) => (
                                            <li key={practice.id}>
                                                <strong>{t(`performance.serviceMetrics.bestPractice.${practice.id}.title`, practice.title)}</strong>
                                                <span>
                                                    {t(
                                                        `performance.serviceMetrics.bestPractice.${practice.id}.description`,
                                                        practice.description
                                                    )}
                                                </span>
                                            </li>
                                        ))}
                                    </ul>
                                </div>
                            )}
                        </aside>
                    </div>
                )}
            </section>
        </div>
    );
};

export default PerformanceServiceMetricsPage;
