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

  const listings = {
    title: "Listings",
    items: [
      { key: "drafts", label: "Drafts", count: 0 },
      { key: "active", label: "Active listings", count: 0 },
      { key: "questions", label: "With questions", count: 0 },
      { key: "open_offers", label: "With open offers from buyers", count: 0 },
      { key: "all_auctions", label: "All auctions", count: 0 },
      { key: "reserve_met", label: "With reserve met", count: 0 },
      { key: "ending_today", label: "Auctions ending today", count: 0 },
      { key: "renewing_today", label: "Buy It Now renewing today", count: 0 },
      { key: "scheduled", label: "Scheduled listings", count: 0 },
      { key: "unsold", label: "Unsold and not relisted", count: 0 },
    ]
  };

  const orders = {
    title: "Orders",
    items: [
      { key: "awaiting_shipment", label: "Awaiting shipment - print shipping label", count: 0 },
      { key: "returns", label: "All open returns/replacements", count: 0 },
      { key: "cancellations", label: "Open cancellations", count: 0 },
      { key: "awaiting_payment", label: "Awaiting payment", count: 0 },
      { key: "shipped", label: "Shipped and awaiting your feedback", count: 0 },
      { key: "eligible", label: "Orders eligible for combined purchases", count: 0 },
    ]
  };

  const heroMetrics = useMemo(() => {
    return [
      {
        key: "views",
        icon: (
          <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path d="M12 4.5C6.5 4.5 1.73 8.31 0 12c1.73 3.69 6.5 7.5 12 7.5s10.27-3.81 12-7.5c-1.73-3.69-6.5-7.5-12-7.5zM12 17c-2.76 0-5-2.24-5-5s2.24-5 5-5 5 2.24 5 5-2.24 5-5 5zm0-8c-1.66 0-3 1.34-3 3s1.34 3 3 3 3-1.34 3-3-1.34-3-3-3z"/>
          </svg>
        ),
        value: header?.listingViewsLast90Days?.toLocaleString() ?? "0",
        label: "Listing views (90d)"
      },
      {
        key: "sales",
        icon: (
          <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path d="M20 4H4c-1.11 0-1.99.89-1.99 2L2 18c0 1.11.89 2 2 2h16c1.11 0 2-.89 2-2V6c0-1.11-.89-2-2-2zm0 14H4v-4h16v4zm0-6H4V8h16v4zM4 6h16v2H4V6zm8 7c1.1 0 2-.9 2-2s-.9-2-2-2-2 .9-2 2 .9 2 2 2z"/>
          </svg>
        ),
        value: formatCurrency(header?.salesLast90Days, header?.salesCurrency || "USD"),
        label: "Sales (90d)"
      },
      {
        key: "orders",
        icon: (
          <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
            <path d="M20 4H4c-1.1 0-2 .9-2 2v12c0 1.1.9 2 2 2h16c1.1 0 2-.9 2-2V6c0-1.1-.9-2-2-2zm0 14H4V6h16v12zm-8-3h6v-2h-6v2zm-2 0H5v-2h5v2zm8-4h-6v-2h6v2zm-8 0H5v-2h5v2zm8-4h-6V5h6v2zm-8 0H5V5h5v2z"/>
          </svg>
        ),
        value: header?.ordersLast90Days?.toLocaleString() ?? "0",
        label: "Orders (90d)"
      }
    ];
  }, [header]);

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

  return (
    <div className="overview">
      <div className="overview-top-link">
        <a href="#opt-out">Opt out of Seller Hub</a>
      </div>

      <section className="overview-hero">
        <div className="overview-hero__identity">
          <div className="overview-hero__avatar" aria-hidden="true">
            <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                <path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm0 16H5V5h14v14zm-5.04-6.71l-2.75 3.54-1.96-2.36L6.5 17h11l-3.54-4.71z"/>
            </svg>
          </div>
          <h1>{header?.sellerName ?? "ng756543"}</h1>
        </div>

        <div className="overview-hero__metrics">
          {heroMetrics.map((metric) => (
            <div key={metric.key} className="overview-hero__metric">
              <div className="overview-hero__metric-top">
                <span className="overview-hero__metric-icon">{metric.icon}</span>
                <span className="overview-hero__metric-value">{metric.value}</span>
              </div>
              <span className="overview-hero__metric-label">{metric.label}</span>
            </div>
          ))}
        </div>

        <Link to="/listing-form" className="overview-hero__cta">
          Create listing
        </Link>
      </section>

      <div className="overview-status overview-status--success">
        <span className="overview-status__icon" aria-hidden="true">
          <svg viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
             <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm-2 15l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"/>
          </svg>
        </span>
        <div>
          <strong>You're all caught up!</strong>
          <p>New tasks, like orders to ship or offers to review, will show up here.</p>
        </div>
      </div>

      <div className="overview__grid">
        <section className="overview-card">
          <header className="overview-card__header">
            <h2>{listings.title}</h2>
          </header>
          <ul className="overview-card__list">
            <li className="overview-card__row overview-card__row--link-only">
              <Link to="/listing-form">Create listing</Link>
            </li>
            {listings.items?.map((item) => (
              <li key={item.key} className="overview-card__row overview-card__row--disabled">
                <span>
                  <span className="overview-card__link-label">{item.label}</span>
                  <span className="overview-card__count">{item.count}</span>
                </span>
              </li>
            ))}
          </ul>
        </section>

        <section className="overview-card">
          <header className="overview-card__header">
            <h2>{orders.title}</h2>
          </header>
          <ul className="overview-card__list">
            <li className="overview-card__row overview-card__row--link-only">
              <a href="#see-all">See all orders</a>
            </li>
            {orders.items?.map((item) => (
              <li key={item.key} className="overview-card__row overview-card__row--disabled">
                <span>
                  <span className="overview-card__link-label">{item.label}</span>
                  <span className="overview-card__count">{item.count}</span>
                </span>
              </li>
            ))}
            <li className="overview-card__row overview-card__row--link-only" style={{ borderBottom: 'none' }}>
              <a href="#show-more">Show more ⌄</a>
            </li>
          </ul>
        </section>

        <section className="overview-card overview-card--sales">
          <header className="overview-card__header">
            <h2>Sales</h2>
          </header>

          <div className="overview-sales-chart">
            <div className="overview-sales-chart__title">
              Chart for sales data across 31 days
            </div>
            
            <div className="overview-sales-chart__layout">
              <div className="overview-sales-chart__y-axis">$0</div>
              <div className="overview-sales-chart__graph-container">
                <svg viewBox="0 0 100 100" preserveAspectRatio="none">
                  <line x1="0" y1="100" x2="100" y2="100" stroke="#cccccc" strokeWidth="2" vectorEffect="non-scaling-stroke" />
                </svg>
                <div className="overview-sales-chart__x-axis">
                  <span>Feb 23</span>
                  <span>Mar 2</span>
                  <span>Mar 9</span>
                  <span>Mar 18</span>
                  <span>Mar 25</span>
                </div>
                <div className="overview-sales-chart__x-label">Month</div>
              </div>
            </div>
          </div>

          <ul className="overview-sales-summary">
            <li>
              <span><a href="#today">Today</a></span>
              <span>$0.00</span>
            </li>
            <li>
              <span><a href="#last7">Last 7 days</a></span>
              <span>$0.00</span>
            </li>
            <li>
              <span><a href="#last31">Last 31 days</a></span>
              <span>$0.00</span>
            </li>
            <li>
              <span><a href="#last90">Last 90 days</a></span>
              <span>$0.00</span>
            </li>
          </ul>
          <div className="overview-sales-footer">
            Data for Feb 23 - Mar 25 at 11:33am PDT. Percentage change relative to prior period. Performance statistics are rounded to the nearest tenth. Data includes shipping and sales tax.
          </div>
        </section>
      </div>
    </div>
  );
};

export default OverviewPage;
