import React, { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import SellerHubService from "../../services/SellerHubService";
import Notice from "../../components/Common/CustomNotification";
import "./OverviewPage.scss";

const INITIAL_STATE = {
  loading: true,
  data: null
};

const formatCurrency = (amount = 0, currency = "USD") => {
  try {
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency
    }).format(Number(amount ?? 0));
  } catch (error) {
    return `${Number(amount ?? 0).toFixed(2)} ${currency}`;
  }
};

const OverviewPage = () => {
  const [state, setState] = useState(INITIAL_STATE);

  useEffect(() => {
    let isMounted = true;

    const fetchOverview = async () => {
      try {
        const response = await SellerHubService.getOverview();
        if (!isMounted) return;
        setState({ loading: false, data: response });
      } catch (error) {
        if (!isMounted) return;
        setState({ loading: false, data: null });
        Notice({
          msg: "Unable to load Seller Hub overview",
          desc: "Please try again in a few minutes.",
          isSuccess: false
        });
      }
    };

    fetchOverview();

    return () => {
      isMounted = false;
    };
  }, []);

  const { data, loading } = state;
  const header = data?.header;
  const status = data?.status;
  const listings = data?.listings;
  const orders = data?.orders;
  const sales = data?.sales;

  const heroMetrics = useMemo(() => {
    if (!header) {
      return [];
    }

    return [
      {
        key: "views",
        label: "Listing views (90d)",
        value: header.listingViewsLast90Days?.toLocaleString() ?? "0"
      },
      {
        key: "sales",
        label: "Sales (90d)",
        value: formatCurrency(header.salesLast90Days, header.salesCurrency)
      },
      {
        key: "orders",
        label: "Orders (90d)",
        value: header.ordersLast90Days?.toLocaleString() ?? "0"
      }
    ];
  }, [header]);

  const chartModel = useMemo(() => {
    const points = sales?.chart ?? [];
    if (!points || points.length === 0) {
      return {
        polyline: ""
      };
    }

    const totals = points.map((point) => Number(point.total ?? 0));
    const max = Math.max(...totals, 1);
    const stepX = points.length > 1 ? 100 / (points.length - 1) : 100;
    const polyline = points
      .map((point, index) => {
        const value = Number(point.total ?? 0);
        const x = index * stepX;
        const y = 100 - (value / max) * 100;
        return `${x.toFixed(2)},${y.toFixed(2)}`;
      })
      .join(" ");

    return { polyline };
  }, [sales]);

  const renderSection = (section) => {
    if (!section) return null;

    return (
      <section className="overview-card">
        <header className="overview-card__header">
          <h2>{section.title}</h2>
        </header>
        <ul className="overview-card__list">
          {section.items?.map((item) => {
            const content = (
              <>
                <span className="overview-card__link-label">{item.label}</span>
                {item.count !== null && item.count !== undefined && (
                  <span className="overview-card__count">{item.count.toLocaleString()}</span>
                )}
              </>
            );

            if (item.navigationPath) {
              return (
                <li key={item.key} className="overview-card__row">
                  <Link to={item.navigationPath}>{content}</Link>
                </li>
              );
            }

            return (
              <li key={item.key} className="overview-card__row overview-card__row--disabled">
                <span>{content}</span>
              </li>
            );
          })}
        </ul>
      </section>
    );
  };

  if (loading) {
    return (
      <div className="overview overview--loading">
        <div className="overview__skeleton" />
        <div className="overview__grid">
          <div className="overview__skeleton-card" />
          <div className="overview__skeleton-card" />
          <div className="overview__skeleton-card" />
        </div>
      </div>
    );
  }

  if (!data) {
    return (
      <div className="overview">
        <div className="overview__empty">
          <h1>Seller Hub overview</h1>
          <p>We couldn't load your overview right now. Please refresh to try again.</p>
        </div>
      </div>
    );
  }

  return (
    <div className="overview">
      <section className="overview-hero">
        <div className="overview-hero__identity">
          <div className="overview-hero__avatar" aria-hidden="true">
            {header?.sellerName?.slice(0, 1)?.toUpperCase() ?? "?"}
          </div>
          <div>
            <h1>{header?.sellerName ?? "Seller Hub"}</h1>
            <p>{header?.sellerUsername}</p>
          </div>
        </div>
        <div className="overview-hero__metrics">
          {heroMetrics.map((metric) => (
            <div key={metric.key} className="overview-hero__metric">
              <span className="overview-hero__metric-label">{metric.label}</span>
              <span className="overview-hero__metric-value">{metric.value}</span>
            </div>
          ))}
        </div>
        <Link to="/listing-form" className="overview-hero__cta">
          Create listing
        </Link>
      </section>

      {status && (
        <div className={`overview-status overview-status--${status.level ?? "info"}`}>
          <span className="overview-status__icon" aria-hidden="true">●</span>
          <div>
            <strong>{status.message}</strong>
            {status.outstandingTasks > 0 && (
              <p>{status.outstandingTasks} outstanding {status.outstandingTasks === 1 ? "task" : "tasks"}.</p>
            )}
          </div>
        </div>
      )}

      <div className="overview__grid">
        {renderSection(listings)}
        {renderSection(orders)}
        <section className="overview-card overview-card--sales">
          <header className="overview-card__header">
            <h2>Sales</h2>
            <span className="overview-card__currency">{sales?.currency ?? "USD"}</span>
          </header>
          <div className="overview-sales-chart">
            {chartModel.polyline ? (
              <svg viewBox="0 0 100 100" preserveAspectRatio="none">
                <polyline
                  points={chartModel.polyline}
                  fill="rgba(49, 120, 198, 0.15)"
                  stroke="rgba(49, 120, 198, 0.85)"
                  strokeWidth="2"
                />
              </svg>
            ) : (
              <div className="overview-sales-chart__empty">No sales recorded for the selected period.</div>
            )}
          </div>
          <ul className="overview-sales-summary">
            {sales?.summary?.map((row) => (
              <li key={row.key}>
                <span>{row.label}</span>
                <span>{formatCurrency(row.total, sales.currency)}</span>
              </li>
            ))}
          </ul>
        </section>
      </div>
    </div>
  );
};

export default OverviewPage;
