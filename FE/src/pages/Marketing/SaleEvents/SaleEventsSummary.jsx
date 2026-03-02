import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import SaleEventService, { SaleEventStatusLabel } from "../../../services/SaleEventService";
import Notice from "../../../components/Common/CustomNotification";
import "./SaleEventsSummary.scss";

const formatDateRange = (start, end) => {
  const startDate = start ? dayjs(start) : null;
  const endDate = end ? dayjs(end) : null;

  if (!startDate?.isValid() || !endDate?.isValid()) {
    return "--";
  }

  const sameDay = startDate.isSame(endDate, "day");
  const datePart = sameDay
    ? startDate.format("DD MMM YYYY")
    : `${startDate.format("DD MMM YYYY")} – ${endDate.format("DD MMM YYYY")}`;

  const timeSuffix = `${startDate.format("HH:mm")} – ${endDate.format("HH:mm")}`;
  return `${datePart} (${timeSuffix})`;
};

const SaleEventsSummary = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [saleEvents, setSaleEvents] = useState([]);
  const [busyIds, setBusyIds] = useState(new Set());

  const loadSaleEvents = useCallback(async () => {
    setLoading(true);
    try {
      const data = await SaleEventService.getSaleEvents();
      setSaleEvents(Array.isArray(data) ? data : []);
    } catch (error) {
      Notice({
        msg: "Could not load sale events.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    } finally {
      setLoading(false);
    }
  }, []);

  const setBusy = useCallback((saleEventId, isBusy) => {
    setBusyIds((prev) => {
      const next = new Set(prev);
      if (isBusy) {
        next.add(saleEventId);
      } else {
        next.delete(saleEventId);
      }
      return next;
    });
  }, []);

  useEffect(() => {
    loadSaleEvents();
  }, [loadSaleEvents]);

  const rows = useMemo(() => saleEvents.slice().sort((a, b) => {
    const startA = a?.startDate ? new Date(a.startDate).getTime() : 0;
    const startB = b?.startDate ? new Date(b.startDate).getTime() : 0;
    return startA - startB;
  }), [saleEvents]);

  const hasEvents = rows.length > 0;

  const handleDeleteSaleEvent = useCallback(async (saleEvent) => {
    const confirmed = window.confirm(`Delete sale event "${saleEvent.name}"? This cannot be undone.`);
    if (!confirmed) {
      return;
    }

    setBusy(saleEvent.id, true);
    try {
      await SaleEventService.deleteSaleEvent(saleEvent.id);
      Notice({ msg: "Sale event deleted.", isSuccess: true });
      await loadSaleEvents();
    } catch (error) {
      Notice({
        msg: "Could not delete sale event.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    } finally {
      setBusy(saleEvent.id, false);
    }
  }, [loadSaleEvents, setBusy]);

  return (
    <div className="sale-events-summary">
      <div className="sale-events-summary__header">
        <div>
          <h1>Sale events</h1>
          <p>Launch markdown events to promote listings and plan limited-time discounts.</p>
        </div>
        <div className="sale-events-summary__actions">
          <button type="button" className="btn-secondary" onClick={loadSaleEvents} disabled={loading}>
            Refresh
          </button>
          <button
            type="button"
            className="btn-primary"
            onClick={() => navigate("/marketing/sale-events/create")}
          >
            Create sale event
          </button>
        </div>
      </div>

      {loading && !hasEvents ? (
        <div className="sale-events-summary__empty">Loading sale events…</div>
      ) : null}

      {!loading && !hasEvents ? (
        <div className="sale-events-summary__empty">
          <p>No sale events yet.</p>
          <p>Create your first markdown event to highlight key listings.</p>
        </div>
      ) : null}

      {hasEvents ? (
        <div className="sale-events-summary__table-wrapper">
          <table className="sale-events-summary__table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Status</th>
                <th>Schedule</th>
                <th>Tiers</th>
                <th>Listings</th>
                <th>Flags</th>
                <th className="sale-events-summary__actions-header">Actions</th>
              </tr>
            </thead>
            <tbody>
              {rows.map((event) => {
                const status = SaleEventStatusLabel[event.status] ?? "Unknown";
                const schedule = formatDateRange(event.startDate, event.endDate);
                const tiers = event.tierCount > 0 ? `${event.tierCount} tier${event.tierCount === 1 ? "" : "s"}` : "--";
                const listings = event.listingCount > 0 ? `${event.listingCount}` : "--";
                const flags = [];
                if (event.offerFreeShipping) {
                  flags.push("Free shipping");
                }
                if (event.includeSkippedItems) {
                  flags.push("Include skipped items");
                }
                if (event.blockPriceIncreaseRevisions) {
                  flags.push("Locks price increases");
                }
                if (event.highlightPercentage) {
                  flags.push(`Highlight ${Number(event.highlightPercentage).toFixed(0)}%`);
                }

                const isBusy = busyIds.has(event.id);

                return (
                  <tr key={event.id}>
                    <td>
                      <div className="sale-events-summary__name">{event.name}</div>
                      <div className="sale-events-summary__meta">ID: {event.id}</div>
                    </td>
                    <td>
                      <span className={`sale-events-summary__status sale-events-summary__status--${(SaleEventStatusLabel[event.status] ?? "unknown").toLowerCase()}`}>
                        {status}
                      </span>
                    </td>
                    <td>{schedule}</td>
                    <td>{tiers}</td>
                    <td>{listings}</td>
                    <td>
                      {flags.length > 0 ? (
                        <ul className="sale-events-summary__flag-list">
                          {flags.map((flag) => (
                            <li key={flag}>{flag}</li>
                          ))}
                        </ul>
                      ) : (
                        "--"
                      )}
                    </td>
                    <td className="sale-events-summary__actions-cell">
                      <button
                        type="button"
                        className="btn-link"
                        onClick={() => handleDeleteSaleEvent(event)}
                        disabled={isBusy}
                      >
                        {isBusy ? "Deleting…" : "Delete"}
                      </button>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      ) : null}
    </div>
  );
};

export default SaleEventsSummary;
