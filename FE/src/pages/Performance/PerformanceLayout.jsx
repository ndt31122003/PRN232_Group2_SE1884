import React, { useMemo } from 'react';
import { Outlet } from 'react-router-dom';
import { useTranslation } from 'react-i18next';
import LeftNav from '../../components/Navigation/LeftNav/LeftNav';
import './PerformanceLayout.scss';

const PerformanceLayout = () => {
    const { t } = useTranslation('global');

    const menuItems = useMemo(() => ([
        {
            id: 'summary',
            label: t('performance.tabs.summary', 'Summary'),
            path: '/performance/summary'
        },
        {
            id: 'seller-level',
            label: t('performance.tabs.sellerLevel', 'Seller Level'),
            path: '/performance/seller-level'
        },
        {
            id: 'sales',
            label: t('performance.tabs.sales', 'Sales'),
            path: '/performance/sales'
        },
        {
            id: 'traffic',
            label: t('performance.tabs.traffic', 'Traffic'),
            path: '/performance/traffic'
        },
        {
            id: 'service-metrics',
            label: t('performance.tabs.serviceMetrics', 'Service Metrics'),
            path: '/performance/service-metrics'
        },
        {
            id: 'stock',
            label: t('performance.tabs.stock', 'Stock'),
            path: '/performance/stock'
        }
    ]), [t]);

    return (
        <div className="performance-layout container">
            <LeftNav menuItems={menuItems} />
            <main className="performance-layout__content">
                <Outlet />
            </main>
        </div>
    );
};

export default PerformanceLayout;