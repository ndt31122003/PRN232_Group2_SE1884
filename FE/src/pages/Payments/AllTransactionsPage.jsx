import React, { useEffect, useMemo, useState } from "react";
import PaymentService from "../../services/PaymentService";
import "./AllTransactionsPage.scss";

const STATUS_OPTIONS = [
  { value: "all", label: "All statuses" },
  { value: "processing", label: "Processing" },
  { value: "available", label: "Available" },
  { value: "onhold", label: "On hold" },
  { value: "payout", label: "Payout" },
  { value: "charge", label: "Charge" },
  { value: "repayment", label: "Repayment" }
];

const TYPE_OPTIONS = [
  { value: "all", label: "All types" },
  { value: "sale", label: "Sale" },
  { value: "shippingLabel", label: "Shipping label" },
  { value: "fee", label: "Fee" },
  { value: "claim", label: "Claim" },
  { value: "payout", label: "Payout" },
  { value: "adjustment", label: "Adjustment" },
  { value: "refund", label: "Refund" }
];

const PERIOD_OPTIONS = [
  { value: "30", label: "Last 30 days" },
  { value: "60", label: "Last 60 days" },
  { value: "90", label: "Last 90 days" }
];

const SEARCH_FIELDS = [
  { value: "orderNumber", label: "Order number" },
  { value: "buyer", label: "Buyer username" },
  { value: "itemId", label: "Item ID" },
  { value: "caseId", label: "Case ID" },
  { value: "trackingNumber", label: "Tracking number" }
];

const SUMMARY_BUCKETS = [
  { key: "available", label: "Available funds" },
  { key: "processing", label: "Processing" },
  { key: "onhold", label: "On hold" },
  { key: "payout", label: "Payouts" },
  { key: "charge", label: "Charges and payments" },
  { key: "repayment", label: "Repayment" }
];

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

const formatDate = (isoString) => {
  const date = new Date(isoString);
  if (Number.isNaN(date.getTime())) {
    return "—";
  }

  return new Intl.DateTimeFormat("en-US", {
    month: "short",
    day: "numeric",
    year: "numeric"
  }).format(date);
};

const AllTransactionsPage = () => {
  const [status, setStatus] = useState("all");
  const [type, setType] = useState("all");
  const [period, setPeriod] = useState("90");
  const [searchField, setSearchField] = useState("orderNumber");
  const [searchQuery, setSearchQuery] = useState("");
  const [transactions, setTransactions] = useState([]);
  const [summaryCounts, setSummaryCounts] = useState({});
  const [currentBalance, setCurrentBalance] = useState(0);
  const [currency, setCurrency] = useState("USD");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const typeLabelMap = useMemo(() => {
    return TYPE_OPTIONS.reduce((acc, option) => {
      acc[option.value] = option.label;
      return acc;
    }, {});
  }, []);

  useEffect(() => {
    const controller = new AbortController();
    const fetchTransactions = async () => {
      setIsLoading(true);
      setError("");
      try {
        const params = {
          status,
          type,
          periodDays: Number(period) || 90,
          searchField,
          search: searchQuery.trim() || undefined
        };
        const response = await PaymentService.getTransactions(params, controller.signal);
        const payload = response?.data ?? {};

        setTransactions(payload.transactions ?? []);
        const normalizedSummary = SUMMARY_BUCKETS.reduce((acc, bucket) => {
          acc[bucket.key] = payload.summaryCounts?.[bucket.key] ?? 0;
          return acc;
        }, {});
        setSummaryCounts(normalizedSummary);
        setCurrentBalance(payload.currentBalance ?? 0);
        setCurrency(payload.currency ?? "USD");
      } catch (err) {
        if (!controller.signal.aborted) {
          console.error("Failed to load transactions", err);
          setError("Unable to load transactions right now.");
        }
      } finally {
        if (!controller.signal.aborted) {
          setIsLoading(false);
        }
      }
    };

    fetchTransactions();

    return () => controller.abort();
  }, [status, type, period, searchField, searchQuery]);

  const resetFilters = () => {
    setStatus("all");
    setType("all");
    setPeriod("90");
    setSearchField("orderNumber");
    setSearchQuery("");
  };

  return (
    <div className="payments-transactions">
      <div className="payments-transactions__header">
        <h1>All transactions</h1>
        <div className="payments-transactions__actions">
          <button type="button" className="pt-btn pt-btn--secondary" onClick={resetFilters}>
            Reset
          </button>
          <button type="button" className="pt-btn pt-btn--primary">
            Export
          </button>
        </div>
      </div>

      <p className="payments-transactions__helper">
        Review every activity across your eBay funds, including processing, on-hold, and paid-out transactions.
      </p>

      <div className="payments-transactions__filters">
        <label className="pt-filter">
          <span>Statuses</span>
          <select value={status} onChange={(event) => setStatus(event.target.value)}>
            {STATUS_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </label>

        <label className="pt-filter">
          <span>Types</span>
          <select value={type} onChange={(event) => setType(event.target.value)}>
            {TYPE_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </label>

        <label className="pt-filter">
          <span>Time period</span>
          <select value={period} onChange={(event) => setPeriod(event.target.value)}>
            {PERIOD_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </label>

        <label className="pt-filter">
          <span>Search by</span>
          <div className="pt-filter__search">
            <select value={searchField} onChange={(event) => setSearchField(event.target.value)}>
              {SEARCH_FIELDS.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
            <input
              type="search"
              placeholder="Search"
              value={searchQuery}
              onChange={(event) => setSearchQuery(event.target.value)}
            />
          </div>
        </label>
      </div>

      <div className="payments-transactions__summary">
        {SUMMARY_BUCKETS.map((bucket) => (
          <button
            key={bucket.key}
            type="button"
            className={`pt-summary-chip${status === bucket.key ? " pt-summary-chip--active" : ""}`}
            onClick={() => setStatus(bucket.key)}
          >
            <span>{bucket.label}</span>
            <span className="pt-summary-chip__count">{summaryCounts[bucket.key] ?? 0}</span>
          </button>
        ))}
      </div>

      <section className="payments-transactions__funds">
        <header>
          <h2>Total funds</h2>
          <span className="payments-transactions__funds-hint">
            These funds consist of all of your earnings, whether or not they are available to be paid out to you.
          </span>
        </header>
        <div className="payments-transactions__funds-balance">
          <span className="payments-transactions__funds-label">Current balance</span>
          <strong>{formatCurrency(currentBalance, currency)}</strong>
        </div>
      </section>

      <div className="payments-transactions__table">
        {isLoading ? (
          <div className="payments-transactions__empty">Loading transactions...</div>
        ) : error ? (
          <div className="payments-transactions__empty">{error}</div>
        ) : transactions.length === 0 ? (
          <div className="payments-transactions__empty">No results</div>
        ) : (
          <div className="payments-transactions__table-scroll">
            <table>
              <thead>
                <tr>
                  <th>Date</th>
                  <th>Type</th>
                  <th>Description</th>
                  <th>Reference</th>
                  <th>Status</th>
                  <th className="is-right">Amount</th>
                  <th className="is-right">Balance impact</th>
                </tr>
              </thead>
              <tbody>
                {transactions.map((txn) => (
                  <tr key={txn.id}>
                    <td>{formatDate(txn.occurredAt)}</td>
                    <td className="payments-transactions__type">{typeLabelMap[txn.type] ?? txn.type}</td>
                    <td>{txn.description}</td>
                    <td>{txn.orderNumber ?? "—"}</td>
                    <td>{txn.status}</td>
                    <td className="is-right">{formatCurrency(txn.amount, txn.currency)}</td>
                    <td className={`is-right ${txn.balanceImpact >= 0 ? "is-positive" : "is-negative"}`}>
                      {formatCurrency(txn.balanceImpact, txn.currency)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

export default AllTransactionsPage;
