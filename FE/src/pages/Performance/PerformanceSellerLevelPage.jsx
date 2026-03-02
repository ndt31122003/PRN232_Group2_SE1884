import React, { useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { LoadingScreen } from '../../components/LoadingScreen/LoadingScreen';
import { PerformanceService } from '../../services/Performance';
import './PerformanceSellerLevelPage.scss';

const DEFAULT_SELLER_LEVEL = {
    region: 'US',
    currentSellerLevel: 'Above Standard',
    ifEvaluatedTodayLevel: 'Above Standard',
    transactionDefectRate: 0,
    lateShipmentRate: 0,
    trackingUploadedOnTimeRate: 0,
    casesClosedWithoutSellerResolutionRate: 0,
    transactionsLast12Months: 0,
    salesLast12Months: 0,
    currency: 'USD',
    nextEvaluationDate: null,
};

const PerformanceSellerLevelPage = () => {
    const { t } = useTranslation('global');
    const [sellerLevel, setSellerLevel] = useState(DEFAULT_SELLER_LEVEL);
    const [loading, setLoading] = useState(true);
    const [isFetching, setIsFetching] = useState(false);
    const [hasError, setHasError] = useState(false);

    useEffect(() => {
        let ignore = false;

        const fetchSellerLevel = async () => {
            setIsFetching(true);
            setHasError(false);

            try {
                const response = await PerformanceService.getPerformanceSellerLevel();
                const payload = response?.data?.data ?? response?.data ?? null;

                if (!ignore && payload) {
                    setSellerLevel({
                        region: payload.region ?? DEFAULT_SELLER_LEVEL.region,
                        currentSellerLevel: payload.currentSellerLevel ?? DEFAULT_SELLER_LEVEL.currentSellerLevel,
                        ifEvaluatedTodayLevel: payload.ifEvaluatedTodayLevel ?? DEFAULT_SELLER_LEVEL.ifEvaluatedTodayLevel,
                        transactionDefectRate: Number(payload.transactionDefectRate ?? 0),
                        lateShipmentRate: Number(payload.lateShipmentRate ?? 0),
                        trackingUploadedOnTimeRate: Number(payload.trackingUploadedOnTimeRate ?? 0),
                        casesClosedWithoutSellerResolutionRate: Number(payload.casesClosedWithoutSellerResolutionRate ?? 0),
                        transactionsLast12Months: Number(payload.transactionsLast12Months ?? 0),
                        salesLast12Months: Number(payload.salesLast12Months ?? 0),
                        currency: payload.currency ?? DEFAULT_SELLER_LEVEL.currency,
                        nextEvaluationDate: payload.nextEvaluationDate ?? DEFAULT_SELLER_LEVEL.nextEvaluationDate,
                    });
                }
            } catch (error) {
                if (!ignore) {
                    setHasError(true);
                    setSellerLevel(DEFAULT_SELLER_LEVEL);
                }
            } finally {
                if (!ignore) {
                    setLoading(false);
                    setIsFetching(false);
                }
            }
        };

        fetchSellerLevel();

        return () => {
            ignore = true;
        };
    }, []);

    const formatPercent = (value) => `${Number.isFinite(value) ? value.toFixed(2) : '0.00'}%`;
    const formatNumber = (value) => Number(value ?? 0).toLocaleString('en-US');
    const formatCurrency = (value, currency) => new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: currency || 'USD',
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
    }).format(Number.isFinite(value) ? value : 0);

    const formatEvaluationDate = (value) => {
        if (!value) {
            return t('performance.sellerLevel.nextEvaluationFallback', 'Next evaluation date to be determined');
        }

        const date = new Date(value);
        if (Number.isNaN(date.getTime())) {
            return t('performance.sellerLevel.nextEvaluationFallback', 'Next evaluation date to be determined');
        }

        return t('performance.sellerLevel.nextEvaluationLabel', 'Next evaluation {{date}}', {
            date: date.toLocaleDateString('en-US', {
                month: 'short',
                day: 'numeric',
            }),
        });
    };

    const formatEvaluationChip = (value) => {
        if (!value) {
            return t('performance.sellerLevel.nextEvaluationShortFallback', 'To be determined');
        }

        const date = new Date(value);
        if (Number.isNaN(date.getTime())) {
            return t('performance.sellerLevel.nextEvaluationShortFallback', 'To be determined');
        }

        return date.toLocaleDateString('en-US', {
            month: 'short',
            day: 'numeric',
        });
    };

    const levelMessage = useMemo(() => {
        const currentLevel = (sellerLevel.currentSellerLevel ?? '').toLowerCase();

        if (currentLevel.includes('top')) {
            return t(
                'performance.sellerLevel.message.topRated',
                'You are Top Rated. Keep meeting shipping and service benchmarks to retain your benefits.'
            );
        }

        if (currentLevel.includes('above')) {
            return t(
                'performance.sellerLevel.message.aboveStandard',
                'You are Above Standard. Focus on defect, shipment, and case metrics to earn Top Rated status.'
            );
        }

        if (currentLevel.includes('below')) {
            return t(
                'performance.sellerLevel.message.belowStandard',
                'You are Below Standard. Improve your metrics before the next evaluation to avoid selling limits.'
            );
        }

        return t(
            'performance.sellerLevel.message.collecting',
            'We are still collecting enough transactions to evaluate your seller level.'
        );
    }, [sellerLevel.currentSellerLevel, t]);

    const metrics = useMemo(() => ([
        {
            id: 'transaction-defect-rate',
            label: t('performance.sellerLevel.transactionDefectRate', 'Transaction defect rate'),
            value: formatPercent(sellerLevel.transactionDefectRate),
        },
        {
            id: 'late-shipment-rate',
            label: t('performance.sellerLevel.lateShipmentRate', 'Late shipment rate'),
            value: formatPercent(sellerLevel.lateShipmentRate),
        },
        {
            id: 'tracking-uploaded',
            label: t('performance.sellerLevel.trackingUploaded', 'Tracking uploaded on time and validated'),
            value: formatPercent(sellerLevel.trackingUploadedOnTimeRate),
        },
        {
            id: 'cases-closed',
            label: t('performance.sellerLevel.casesClosedRate', 'Cases closed without seller resolution rate'),
            value: formatPercent(sellerLevel.casesClosedWithoutSellerResolutionRate),
        },
    ]), [sellerLevel, t]);

    const statistics = useMemo(() => ([
        {
            id: 'transactions-last-12-months',
            label: t('performance.sellerLevel.transactions', 'Transactions (last 12 months)'),
            value: formatNumber(sellerLevel.transactionsLast12Months),
        },
        {
            id: 'sales-last-12-months',
            label: t('performance.sellerLevel.sales', 'Sales (last 12 months)'),
            value: formatCurrency(sellerLevel.salesLast12Months, sellerLevel.currency),
        },
    ]), [sellerLevel, t]);

    if (loading) {
        return <LoadingScreen />;
    }

    return (
        <div className="performance-seller-level" aria-busy={isFetching} aria-live="polite">
            <header className="performance-seller-level__header">
                <div className="performance-seller-level__title-group">
                    <h1>
                        Seller level (Region: US)
                    </h1>
                    <p className="performance-seller-level__subtitle">
                        {t('performance.sellerLevel.ifEvaluatedToday', 'If we evaluated you today')}
                        :
                        <span className="performance-seller-level__status">
                            {sellerLevel.ifEvaluatedTodayLevel}
                        </span>
                    </p>
                </div>
                <p className="performance-seller-level__badge">
                    {t('performance.sellerLevel.currentLevel', 'Current seller level')}
                    :
                    <span>{sellerLevel.currentSellerLevel}</span>
                </p>
            </header>

            {hasError && (
                <div className="performance-seller-level__error" role="status">
                    {t('performance.sellerLevel.error', 'We could not refresh seller level. Showing the most recent data.')}
                </div>
            )}

            <section className="performance-seller-level__overview">
                <div className="performance-seller-level__status-card">
                    <h3>{t('performance.sellerLevel.healthHeading', 'Performance health')}</h3>
                    <p>{levelMessage}</p>
                </div>
                <div className="performance-seller-level__summary">
                    <div className="performance-seller-level__summary-item">
                        <span className="performance-seller-level__summary-label">
                            {t('performance.sellerLevel.transactionsSummary', 'Transactions (12 months)')}
                        </span>
                        <strong>{formatNumber(sellerLevel.transactionsLast12Months)}</strong>
                    </div>
                    <div className="performance-seller-level__summary-item">
                        <span className="performance-seller-level__summary-label">
                            {t('performance.sellerLevel.salesSummary', 'Sales (12 months)')}
                        </span>
                        <strong>{formatCurrency(sellerLevel.salesLast12Months, sellerLevel.currency)}</strong>
                    </div>
                    <div className="performance-seller-level__summary-item">
                        <span className="performance-seller-level__summary-label">
                            {t('performance.sellerLevel.nextEvaluation', 'Next evaluation')}
                        </span>
                        <strong>{formatEvaluationChip(sellerLevel.nextEvaluationDate)}</strong>
                    </div>
                </div>
            </section>

            <section className="performance-seller-level__card">
                <div className="performance-seller-level__metrics">
                    {metrics.map((metric) => (
                        <div key={metric.id} className="performance-seller-level__metric">
                            <p className="performance-seller-level__metric-label">{metric.label}</p>
                            <p className="performance-seller-level__metric-value">{metric.value}</p>
                        </div>
                    ))}
                </div>

                <div className="performance-seller-level__divider" aria-hidden="true" />

                <div className="performance-seller-level__statistics">
                    {statistics.map((stat) => (
                        <div key={stat.id} className="performance-seller-level__stat">
                            <p className="performance-seller-level__stat-label">{stat.label}</p>
                            <p className="performance-seller-level__stat-value">{stat.value}</p>
                        </div>
                    ))}
                </div>
            </section>

            <footer className="performance-seller-level__footer">
                <span>{formatEvaluationDate(sellerLevel.nextEvaluationDate)}</span>
            </footer>
        </div>
    );
};

export default PerformanceSellerLevelPage;
