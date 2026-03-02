import React, { useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import PaymentService from "../../services/PaymentService";
import "./PayoutsPage.scss";

const PERIOD_OPTIONS = [
  { value: "last30Days", label: "Last 30 days" },
  { value: "last60Days", label: "Last 60 days" },
  { value: "last90Days", label: "Last 90 days" },
  { value: "custom", label: "Custom" }
];

const SEARCH_OPTIONS = [
  { value: "payoutId", label: "Payout ID" },
  { value: "orderNumber", label: "Order number" },
  { value: "buyerUsername", label: "Buyer username" }
];

const PAGE_SIZE = 20;

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

const buildQueryParams = (period, fromDate, toDate, searchBy, keyword, page) => {
  const params = {
    period,
    searchBy,
    page,
    pageSize: PAGE_SIZE
  };

  if (keyword && keyword.trim().length > 0) {
    params.keyword = keyword.trim();
  }

  if (period === "custom" && fromDate && toDate) {
    const from = new Date(fromDate);
    const to = new Date(toDate);
    if (!Number.isNaN(from.getTime()) && !Number.isNaN(to.getTime())) {
      params.fromUtc = from.toISOString();
      params.toUtc = to.toISOString();
    }
  }

  return params;
};

const PayoutsPage = () => {
  const [period, setPeriod] = useState("last30Days");
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");
  const [searchBy, setSearchBy] = useState("payoutId");
  const [keyword, setKeyword] = useState("");
  const [page, setPage] = useState(1);

  const [payouts, setPayouts] = useState([]);
  const [totals, setTotals] = useState({ totalCount: 0, totalAmount: 0, currency: "USD" });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const [selectedPayoutId, setSelectedPayoutId] = useState(null);
  const [detailData, setDetailData] = useState(null);
  const [detailError, setDetailError] = useState("");
  const [isDetailLoading, setIsDetailLoading] = useState(false);

  useEffect(() => {
    const controller = new AbortController();

    const fetchPayouts = async () => {
      if (period === "custom" && (!fromDate || !toDate)) {
        setPayouts([]);
        setTotals((current) => ({ ...current, totalCount: 0, totalAmount: 0 }));
        return;
      }

      setIsLoading(true);
      setError("");

      try {
        const params = buildQueryParams(period, fromDate, toDate, searchBy, keyword, page);
        const response = await PaymentService.getPayouts(params, controller.signal);
        const data = response?.data ?? {};

        setPayouts(Array.isArray(data.payouts) ? data.payouts : []);
        setTotals({
          totalCount: data.totalCount ?? 0,
          totalAmount: data.totalAmount ?? 0,
          currency: data.currency ?? "USD"
        });
      } catch (err) {
        if (!controller.signal.aborted) {
          console.error("Failed to load payouts", err);
          setError("Unable to load payouts right now.");
        }
      } finally {
        if (!controller.signal.aborted) {
          setIsLoading(false);
        }
      }
    };

    fetchPayouts();

    return () => controller.abort();
  }, [period, fromDate, toDate, searchBy, keyword, page]);

  const resetFilters = () => {
    setPeriod("last30Days");
    setFromDate("");
    setToDate("");
    setSearchBy("payoutId");
    setKeyword("");
    setPage(1);
  };

  const totalPages = useMemo(() => {
    if (totals.totalCount <= 0) {
      return 1;
    }
    return Math.max(1, Math.ceil(totals.totalCount / PAGE_SIZE));
  }, [totals.totalCount]);

  const fetchDetail = async (payoutId) => {
    setSelectedPayoutId(payoutId);
    setDetailData(null);
    setDetailError("");
    setIsDetailLoading(true);

    try {
      const response = await PaymentService.getPayoutDetail(payoutId);
      setDetailData(response?.data ?? null);
    } catch (err) {
      console.error("Failed to load payout detail", err);
      setDetailError("Unable to load payout details right now.");
    } finally {
      setIsDetailLoading(false);
    }
  };

  const closeDetail = () => {
    setSelectedPayoutId(null);
    setDetailData(null);
    setDetailError("");
  };

  const renderPagination = () => {
    if (totalPages <= 1) {
      return null;
    }

    return (
      <div className="payments-payouts__pagination">
        <button
          type="button"
          onClick={() => setPage((current) => Math.max(1, current - 1))}
          disabled={page === 1}
        >
          Previous
        </button>
        <span>
          Page {page} of {totalPages}
        </span>
        <button
          type="button"
          onClick={() => setPage((current) => Math.min(totalPages, current + 1))}
          disabled={page === totalPages}
        >
          Next
        </button>
      </div>
    );
  };

  const renderDetailContent = () => {
    if (isDetailLoading) {
      return <div className="payments-payouts__detail-message">Loading payout detail…</div>;
    }

    if (detailError) {
      return <div className="payments-payouts__detail-message is-error">{detailError}</div>;
    }

    if (!detailData) {
      return <div className="payments-payouts__detail-message">Select a payout to see details.</div>;
    }

    return (
      <div className="payments-payouts__detail-body">
        <div className="payments-payouts__detail-overview">
          <div>
            <h3>{detailData.payoutId}</h3>
            <p>{formatDateTime(detailData.payoutDateUtc)}</p>
          </div>
          <div>
            <span className={`payments-payouts__status payments-payouts__status--${detailData.status?.toLowerCase()}`}>
              {detailData.status}
            </span>
            <strong>{formatCurrency(detailData.amount, detailData.currency)}</strong>
            <p>{detailData.memo}</p>
          </div>
          <div>
            <span>Account</span>
            <p>{detailData.account}</p>
          </div>
        </div>

        <table className="payments-payouts__detail-table">
          <thead>
            <tr>
              <th>Order</th>
              <th>Buyer</th>
              <th>Status</th>
              <th>Gross</th>
              <th>Fees</th>
              <th>Net</th>
              <th />
            </tr>
          </thead>
          <tbody>
            {detailData.transactions?.length ? (
              detailData.transactions.map((transaction) => (
                <tr key={transaction.orderId}>
                  <td>
                    <div className="payments-payouts__detail-order">
                      <span>{transaction.orderNumber}</span>
                      <small>{formatDate(transaction.orderedAtUtc)}</small>
                    </div>
                  </td>
                  <td>{transaction.buyerUsername || "—"}</td>
                  <td>{transaction.status || "—"}</td>
                  <td>{formatCurrency(transaction.grossAmount, transaction.currency)}</td>
                  <td>{formatCurrency(transaction.totalFees, transaction.currency)}</td>
                  <td>{formatCurrency(transaction.netAmount, transaction.currency)}</td>
                  <td>
                    <Link
                      to={`/order/all?order=${transaction.orderNumber}`}
                      className="payments-payouts__link"
                    >
                      View order
                    </Link>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan={7} className="payments-payouts__detail-message">
                  No transactions found for this payout.
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    );
  };

  return (
    <div className="payments-payouts">
      <div className="payments-payouts__header">
        <span className="payments-payouts__crumb">Payments &gt; Payouts</span>
        <h1>Payouts</h1>
      </div>

      <section className="payments-payouts__filters">
        <label className="payments-payouts__control">
          <span>Time period</span>
          <select
            value={period}
            onChange={(event) => {
              setPeriod(event.target.value);
              setPage(1);
            }}
          >
            {PERIOD_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </label>

        {period === "custom" && (
          <div className="payments-payouts__custom-range">
            <label>
              <span>From</span>
              <input
                type="date"
                value={fromDate}
                onChange={(event) => setFromDate(event.target.value)}
              />
            </label>
            <label>
              <span>To</span>
              <input
                type="date"
                value={toDate}
                onChange={(event) => setToDate(event.target.value)}
              />
            </label>
          </div>
        )}

        <label className="payments-payouts__control">
          <span>Search by</span>
          <select
            value={searchBy}
            onChange={(event) => {
              setSearchBy(event.target.value);
              setPage(1);
            }}
          >
            {SEARCH_OPTIONS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </label>

        <label className="payments-payouts__control payments-payouts__control--keyword">
          <span>Keyword</span>
          <input
            type="text"
            placeholder="Enter keyword"
            value={keyword}
            onChange={(event) => {
              setKeyword(event.target.value);
              setPage(1);
            }}
          />
        </label>

        <button type="button" className="payments-payouts__reset" onClick={resetFilters}>
          Reset
        </button>
      </section>

      <section className="payments-payouts__summary">
        <div className="payments-payouts__summary-card">
          <span>Total payouts</span>
          <strong>{formatCurrency(totals.totalAmount, totals.currency)}</strong>
          <small>
            {totals.totalCount} {totals.totalCount === 1 ? "payout" : "payouts"}
          </small>
        </div>
      </section>

      {isLoading ? (
        <div className="payments-payouts__message">Loading payouts…</div>
      ) : error ? (
        <div className="payments-payouts__message is-error">{error}</div>
      ) : payouts.length === 0 ? (
        <div className="payments-payouts__message">No payouts found for the selected filters.</div>
      ) : (
        <>
          <table className="payments-payouts__table">
            <thead>
              <tr>
                <th>Payout date</th>
                <th>Payout ID</th>
                <th>Payout method</th>
                <th>Status</th>
                <th>Memo</th>
                <th>Amount</th>
              </tr>
            </thead>
            <tbody>
              {payouts.map((payout) => (
                <tr key={payout.payoutId}>
                  <td>{formatDate(payout.payoutDateUtc)}</td>
                  <td>
                    <button
                      type="button"
                      className="payments-payouts__link"
                      onClick={() => fetchDetail(payout.payoutId)}
                    >
                      {payout.payoutId}
                    </button>
                    <span className="payments-payouts__transactions-count">
                      {payout.transactionCount} {payout.transactionCount === 1 ? "transaction" : "transactions"}
                    </span>
                  </td>
                  <td>{payout.account}</td>
                  <td>
                    <span className={`payments-payouts__status payments-payouts__status--${payout.status?.toLowerCase()}`}>
                      {payout.status}
                    </span>
                  </td>
                  <td>{payout.memo}</td>
                  <td>{formatCurrency(payout.amount, payout.currency)}</td>
                </tr>
              ))}
            </tbody>
          </table>

          {renderPagination()}
        </>
      )}

      {selectedPayoutId && (
        <div className="payments-payouts__overlay" role="dialog" aria-modal="true">
          <div className="payments-payouts__drawer">
            <header className="payments-payouts__detail-header">
              <h2>Payout details</h2>
              <button type="button" onClick={closeDetail} aria-label="Close detail">
                ✕
              </button>
            </header>
            {renderDetailContent()}
          </div>
        </div>
      )}
    </div>
  );
};

export default PayoutsPage;
