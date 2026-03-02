import React, { useEffect, useMemo, useState } from "react";
import PaymentService from "../../services/PaymentService";
import "./SummaryPage.scss";

const formatCurrency = (amount, currency = "USD") => {
  try {
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency
    }).format(Number(amount ?? 0));
  } catch (error) {
    return `${Number(amount ?? 0).toFixed(2)} ${currency}`;
  }
};

const formatDateTime = (value) => {
  if (!value) {
    return "—";
  }

  const date = new Date(value);
  if (Number.isNaN(date.getTime())) {
    return "—";
  }

  return new Intl.DateTimeFormat("en-US", {
    month: "short",
    day: "2-digit",
    year: "numeric",
    hour: "2-digit",
    minute: "2-digit"
  }).format(date);
};

const formatDate = (value) => {
  if (!value) {
    return "—";
  }

  const date = new Date(value);
  if (Number.isNaN(date.getTime())) {
    return "—";
  }

  return new Intl.DateTimeFormat("en-US", {
    month: "short",
    day: "2-digit",
    year: "numeric"
  }).format(date);
};

const SummaryPage = () => {
  const [summary, setSummary] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const controller = new AbortController();

    const fetchSummary = async () => {
      setIsLoading(true);
      setError("");
      try {
        const response = await PaymentService.getSummary(controller.signal);
        setSummary(response?.data ?? null);
      } catch (err) {
        if (!controller.signal.aborted) {
          console.error("Failed to load payments summary", err);
          setError("Unable to load payment summary right now.");
        }
      } finally {
        if (!controller.signal.aborted) {
          setIsLoading(false);
        }
      }
    };

    fetchSummary();

    return () => controller.abort();
  }, []);

  const currency = summary?.funds?.currency ?? "USD";

  const totals = useMemo(() => {
    if (!summary?.funds) {
      return {
        total: 0,
        available: 0,
        processing: 0,
        onHold: 0
      };
    }

    const available = Number(summary.funds.available ?? 0);
    const processing = Number(summary.funds.processing ?? 0);
    const onHold = Number(summary.funds.onHold ?? 0);
    return {
      total: available + processing + onHold,
      available,
      processing,
      onHold
    };
  }, [summary]);

  const renderRecentActivity = () => {
    if (isLoading) {
      return <div className="payments-summary__activity-message">Loading recent activity…</div>;
    }

    if (error) {
      return <div className="payments-summary__activity-message is-error">{error}</div>;
    }

    if (!summary?.recentActivity?.length) {
      return <div className="payments-summary__activity-message">No recent activity.</div>;
    }

    return (
      <ul className="payments-summary__activity-list">
        {summary.recentActivity.map((item) => (
          <li key={item.id}>
            <div>
              <span className="payments-summary__activity-title">{item.description}</span>
              <span className="payments-summary__activity-meta">
                {item.type} • {formatDateTime(item.occurredAtUtc)}
              </span>
            </div>
            <div className="payments-summary__activity-amount">
              <span>{formatCurrency(item.amount, item.currency)}</span>
              {item.orderNumber && (
                <small>Order {item.orderNumber}</small>
              )}
            </div>
          </li>
        ))}
      </ul>
    );
  };

  return (
    <div className="payments-summary">
      <div className="payments-summary__header">
        <span className="payments-summary__crumb">Payments &gt; Summary</span>
        <h1>Your financial summary</h1>
      </div>

      {isLoading && !summary ? (
        <div className="payments-summary__message">Loading summary…</div>
      ) : error && !summary ? (
        <div className="payments-summary__message is-error">{error}</div>
      ) : (
        <>
          <section className="payments-summary__funds">
            <article className="payments-summary__funds-card payments-summary__funds-card--primary">
              <div>
                <span>Total funds</span>
                <strong>{formatCurrency(totals.total, currency)}</strong>
              </div>
              <button type="button" disabled>
                View all funds
              </button>
            </article>
            <article className="payments-summary__funds-card">
              <h3>Available funds</h3>
              <strong>{formatCurrency(totals.available, currency)}</strong>
              <small>Ready for payout</small>
            </article>
            <article className="payments-summary__funds-card">
              <h3>Processing funds</h3>
              <strong>{formatCurrency(totals.processing, currency)}</strong>
              <small>Releasing soon</small>
            </article>
            <article className="payments-summary__funds-card">
              <h3>On hold</h3>
              <strong>{formatCurrency(totals.onHold, currency)}</strong>
              <small>Pending resolution</small>
            </article>
          </section>

          <section className="payments-summary__grid">
            <article className="payments-summary__card">
              <header>
                <h2>Payout settings</h2>
                <button type="button" disabled>
                  Change payout account
                </button>
              </header>
              <div className="payments-summary__card-body">
                <div>
                  <span>Account</span>
                  <strong>{summary?.schedule?.account || "—"}</strong>
                </div>
                <div>
                  <span>Frequency</span>
                  <strong>{summary?.schedule?.frequency || "—"}</strong>
                </div>
                <div>
                  <span>Next payout</span>
                  <strong>{formatDateTime(summary?.schedule?.nextPayoutUtc)}</strong>
                </div>
                <div>
                  <span>Last payout</span>
                  <strong>
                    {summary?.schedule?.lastPayoutUtc
                      ? `${formatCurrency(summary.schedule.lastPayoutAmount, summary.schedule.currency)} on ${formatDate(summary.schedule.lastPayoutUtc)}`
                      : "—"}
                  </strong>
                </div>
              </div>
            </article>

            <article className="payments-summary__card">
              <header>
                <h2>Recent activity</h2>
                <button type="button" disabled>
                  See all activity
                </button>
              </header>
              {renderRecentActivity()}
            </article>
          </section>
        </>
      )}
    </div>
  );
};

export default SummaryPage;
