import React, { useEffect, useMemo, useState } from "react";
import PaymentService from "../../services/PaymentService";
import "./ReportsPage.scss";

const PERIOD_OPTIONS = [
  { value: "thisMonth", label: "This month" },
  { value: "lastMonth", label: "Last month" },
  { value: "last90Days", label: "Last 90 days" },
  { value: "thisYear", label: "This year" }
];

const COMPARE_OPTIONS = [
  { value: "none", label: "Pick a month" },
  { value: "previousPeriod", label: "Previous period" },
  { value: "samePeriodLastYear", label: "Same period last year" }
];

const CARD_DEFINITIONS = [
  { key: "orderProceeds", title: "Order proceeds" },
  { key: "refunds", title: "Refunds" },
  { key: "expenses", title: "Expenses" },
  { key: "netTransfers", title: "Net transfers" },
  { key: "other", title: "Other", isWide: true }
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

const formatDateRange = (startUtc, endUtc) => {
  if (!startUtc || !endUtc) {
    return "—";
  }

  const start = new Date(startUtc);
  const end = new Date(endUtc);
  if (Number.isNaN(start.getTime()) || Number.isNaN(end.getTime())) {
    return "—";
  }

  const formatter = new Intl.DateTimeFormat("en-US", {
    month: "short",
    day: "2-digit"
  });

  const startLabel = formatter.format(start);
  const endLabel = formatter.format(end);
  const yearLabel = end.getUTCFullYear();

  return `${startLabel} - ${endLabel}, ${yearLabel}`;
};

const ReportsPage = () => {
  const [period, setPeriod] = useState("thisMonth");
  const [compared, setCompared] = useState("none");
  const [reportData, setReportData] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  const currency = reportData?.current?.currency ?? "USD";

  const comparisonByKey = useMemo(() => {
    const comparison = reportData?.comparison;
    if (!comparison) {
      return {};
    }

    return CARD_DEFINITIONS.reduce((acc, definition) => {
      const section = comparison?.sections?.[definition.key];
      if (section) {
        acc[definition.key] = section;
      }
      return acc;
    }, {});
  }, [reportData]);

  useEffect(() => {
    const controller = new AbortController();

    const fetchReport = async () => {
      setIsLoading(true);
      setError("");
      try {
        const response = await PaymentService.getReport({ period, compared }, controller.signal);
        setReportData(response?.data ?? null);
      } catch (err) {
        if (!controller.signal.aborted) {
          console.error("Failed to load payment report", err);
          setError("Unable to load reporting data right now.");
        }
      } finally {
        if (!controller.signal.aborted) {
          setIsLoading(false);
        }
      }
    };

    fetchReport();

    return () => controller.abort();
  }, [period, compared]);

  const renderLines = (lines = []) => (
    <ul className="payments-report__card-lines">
      {lines.map((line) => (
        <li key={line.label}>
          <span>{line.label}</span>
          <span>{formatCurrency(line.amount, currency)}</span>
        </li>
      ))}
    </ul>
  );

  const renderCard = (definition, section) => {
    const comparisonSection = comparisonByKey[definition.key];
    const total = section?.total ?? 0;
    const comparisonTotal = comparisonSection?.total ?? 0;
    const delta = total - comparisonTotal;
    const hasComparison = reportData?.comparison && comparisonSection;

    return (
      <article
        key={definition.key}
        className={`payments-report__card${definition.isWide ? " payments-report__card--wide" : ""}`}
      >
        <header>
          <h3>{definition.title}</h3>
          <p>{formatCurrency(total, currency)}</p>
          {hasComparison && (
            <span className={`payments-report__card-delta ${delta >= 0 ? "is-positive" : "is-negative"}`}>
              {delta >= 0 ? "▲" : "▼"} {formatCurrency(Math.abs(delta), currency)}
            </span>
          )}
        </header>
        {renderLines(section?.lines ?? [])}
      </article>
    );
  };

  const comparisonRange = reportData?.comparison
    ? formatDateRange(reportData.comparison.startUtc, reportData.comparison.endUtc)
    : null;

  return (
    <div className="payments-report">
      <div className="payments-report__header">
        <span className="payments-report__crumb">Payments &gt; Reports</span>
        <h1>Reports</h1>
        <div className="payments-report__actions">
          <button type="button" className="payments-report__action" disabled>
            Download PDF
          </button>
          <button type="button" className="payments-report__action" disabled>
            Download CSV
          </button>
        </div>
      </div>

      <section className="payments-report__overview">
        <div className="payments-report__overview-header">
          <h2>Financial overview</h2>
          {comparisonRange && (
            <span className="payments-report__comparison-hint">Compared to {comparisonRange}</span>
          )}
        </div>
        <div className="payments-report__controls">
          <label className="payments-report__control">
            <span>Time period</span>
            <select value={period} onChange={(event) => setPeriod(event.target.value)}>
              {PERIOD_OPTIONS.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <label className="payments-report__control">
            <span>Compared to</span>
            <select value={compared} onChange={(event) => setCompared(event.target.value)}>
              {COMPARE_OPTIONS.map((option) => (
                <option key={option.value} value={option.value}>
                  {option.label}
                </option>
              ))}
            </select>
          </label>
          <div className="payments-report__daterange">
            {formatDateRange(reportData?.current?.startUtc, reportData?.current?.endUtc)}
          </div>
        </div>
      </section>

      {isLoading ? (
        <div className="payments-report__message">Loading report...</div>
      ) : error ? (
        <div className="payments-report__message is-error">{error}</div>
      ) : !reportData?.current ? (
        <div className="payments-report__message">No report data found.</div>
      ) : (
        <section className="payments-report__cards">
          {CARD_DEFINITIONS.map((definition) =>
            renderCard(
              definition,
              reportData.current?.sections?.[definition.key]
            )
          )}
        </section>
      )}
    </div>
  );
};

export default ReportsPage;
