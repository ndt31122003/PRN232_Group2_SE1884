import React, { useCallback, useEffect, useMemo, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import dayjs from "dayjs";
import SaleEventService, { SaleEventStatusLabel, SaleEventStatus, SaleEventMode } from "../../../services/SaleEventService";
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

const formatCurrency = (value) => {
  if (value === null || value === undefined) {
    return "--";
  }

  const numeric = Number(value);
  if (!Number.isFinite(numeric)) {
    return "--";
  }

  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 2
  }).format(numeric);
};

const SaleEventsSummary = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const [loading, setLoading] = useState(false);
  const [saleEvents, setSaleEvents] = useState([]);
  const [busyIds, setBusyIds] = useState(new Set());
  const [statusFilter, setStatusFilter] = useState("all");
  const [modeFilter, setModeFilter] = useState("all");
  const [searchTerm, setSearchTerm] = useState("");

  const loadSaleEvents = useCallback(async () => {
    setLoading(true);
    try {
      const data = await SaleEventService.getSaleEvents();
      const payload = data?.value ?? data;
      const items = Array.isArray(payload) ? payload : (payload?.items ?? payload?.Items ?? []);
      setSaleEvents(items);
      
      const createdId = searchParams.get("created");
      if (createdId) {
        Notice({ msg: "Sale event created successfully.", isSuccess: true });
      }
    } catch (error) {
      Notice({
        msg: "Could not load sale events.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    } finally {
      setLoading(false);
    }
  }, [searchParams]);

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

  const filteredEvents = useMemo(() => {
    let filtered = saleEvents;

    if (statusFilter !== "all") {
      const statusValue = SaleEventStatus[statusFilter];
      filtered = filtered.filter((event) => event.status === statusValue);
    }

    if (modeFilter !== "all") {
      const modeValue = modeFilter === "discount" ? SaleEventMode.DiscountAndSaleEvent : SaleEventMode.SaleEventOnly;
      filtered = filtered.filter((event) => event.mode === modeValue);
    }

    if (searchTerm.trim()) {
      const term = searchTerm.trim().toLowerCase();
      filtered = filtered.filter((event) => 
        event.name?.toLowerCase().includes(term) || 
        event.description?.toLowerCase().includes(term) ||
        event.id?.toLowerCase().includes(term)
      );
    }

    return filtered.sort((a, b) => {
      const startA = a?.startDate ? new Date(a.startDate).getTime() : 0;
      const startB = b?.startDate ? new Date(b.startDate).getTime() : 0;
      return startB - startA;
    });
  }, [saleEvents, statusFilter, modeFilter, searchTerm]);

  const hasEvents = filteredEvents.length > 0;

  const handleActivate = useCallback(async (saleEvent) => {
    const confirmed = window.confirm(`Activate sale event "${saleEvent.name}"?`);
    if (!confirmed) {
      return;
    }

    setBusy(saleEvent.id, true);
    try {
      await SaleEventService.activateSaleEvent(saleEvent.id);
      Notice({ msg: "Sale event activated.", isSuccess: true });
      await loadSaleEvents();
    } catch (error) {
      Notice({
        msg: "Could not activate sale event.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    } finally {
      setBusy(saleEvent.id, false);
    }
  }, [loadSaleEvents, setBusy]);

  const handleDeactivate = useCallback(async (saleEvent) => {
    const confirmed = window.confirm(`Deactivate sale event "${saleEvent.name}"?`);
    if (!confirmed) {
      return;
    }

    setBusy(saleEvent.id, true);
    try {
      await SaleEventService.deactivateSaleEvent(saleEvent.id);
      Notice({ msg: "Sale event deactivated.", isSuccess: true });
      await loadSaleEvents();
    } catch (error) {
      Notice({
        msg: "Could not deactivate sale event.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    } finally {
      setBusy(saleEvent.id, false);
    }
  }, [loadSaleEvents, setBusy]);

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

  const handleDuplicate = useCallback((saleEvent) => {
    navigate("/marketing/sale-events/create", {
      state: {
        duplicateFrom: saleEvent.id
      }
    });
  }, [navigate]);

  const handleViewPerformance = useCallback((saleEvent) => {
    navigate(`/marketing/sale-events/${saleEvent.id}/analytics`);
  }, [navigate]);

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

      <div className="sale-events-summary__filters">
        <input
          type="text"
          placeholder="Search by name, description, or ID..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          className="sale-events-summary__search"
        />
        <select
          value={statusFilter}
          onChange={(e) => setStatusFilter(e.target.value)}
          className="sale-events-summary__filter"
        >
          <option value="all">All statuses</option>
          <option value="Draft">Draft</option>
          <option value="Scheduled">Scheduled</option>
          <option value="Active">Active</option>
          <option value="Ended">Ended</option>
          <option value="Cancelled">Cancelled</option>
        </select>
        <select
          value={modeFilter}
          onChange={(e) => setModeFilter(e.target.value)}
          className="sale-events-summary__filter"
        >
          <option value="all">All modes</option>
          <option value="discount">Discount tiers</option>
          <option value="highlight">Highlight only</option>
        </select>
      </div>

      {loading && !hasEvents ? (
        <div className="sale-events-summary__empty">Loading sale events…</div>
      ) : null}

      {!loading && !hasEvents && saleEvents.length === 0 ? (
        <div className="sale-events-summary__empty">
          <p>No sale events yet.</p>
          <p>Create your first markdown event to highlight key listings.</p>
        </div>
      ) : null}

      {!loading && !hasEvents && saleEvents.length > 0 ? (
        <div className="sale-events-summary__empty">
          <p>No sale events match your filters.</p>
        </div>
      ) : null}

      {hasEvents ? (
        <div className="sale-events-summary__table-wrapper">
          <table className="sale-events-summary__table">
            <thead>
              <tr>
                <th>Name</th>
                <th>Status</th>
                <th>Mode</th>
                <th>Schedule</th>
                <th>Tiers</th>
                <th>Listings</th>
                <th>Performance</th>
                <th className="sale-events-summary__actions-header">Actions</th>
              </tr>
            </thead>
            <tbody>
              {filteredEvents.map((event) => {
                const status = SaleEventStatusLabel[event.status] ?? "Unknown";
                const mode = event.mode === SaleEventMode.DiscountAndSaleEvent ? "Discount tiers" : "Highlight only";
                const schedule = formatDateRange(event.startDate, event.endDate);
                const tiers = (event.discountTiers?.length ?? 0) > 0 ? `${event.discountTiers.length}` : "--";
                const listings = (event.totalListingsCount ?? 0) > 0 ? `${event.totalListingsCount}` : "--";
                
                const isBusy = busyIds.has(event.id);
                const canActivate = event.status === SaleEventStatus.Draft || event.status === SaleEventStatus.Scheduled;
                const canDeactivate = event.status === SaleEventStatus.Active;
                const canDelete = event.status === SaleEventStatus.Draft;
                const canEdit = event.status !== SaleEventStatus.Ended;

                return (
                  <tr key={event.id}>
                    <td>
                      <div className="sale-events-summary__name">{event.name}</div>
                      {event.description ? (
                        <div className="sale-events-summary__meta">{event.description}</div>
                      ) : null}
                    </td>
                    <td>
                      <span className={`sale-events-summary__status sale-events-summary__status--${status.toLowerCase()}`}>
                        {status}
                      </span>
                    </td>
                    <td>{mode}</td>
                    <td>{schedule}</td>
                    <td>{tiers}</td>
                    <td>{listings}</td>
                    <td>
                      {event.performanceMetrics ? (
                        <div className="sale-events-summary__performance">
                          <div>{event.performanceMetrics.orderCount || 0} orders</div>
                          <div>{formatCurrency(event.performanceMetrics.totalSalesRevenue)}</div>
                          <div>{formatCurrency(event.performanceMetrics.totalDiscountAmount)} discount</div>
                        </div>
                      ) : (
                        "--"
                      )}
                    </td>
                    <td className="sale-events-summary__actions-cell">
                      <div className="sale-events-summary__action-buttons">
                        <button
                          type="button"
                          className="btn-link"
                          onClick={() => handleViewPerformance(event)}
                          disabled={isBusy}
                        >
                          View
                        </button>
                        {canEdit && (
                          <button
                            type="button"
                            className="btn-link"
                            onClick={() => navigate(`/marketing/sale-events/${event.id}/edit`)}
                            disabled={isBusy}
                          >
                            Edit
                          </button>
                        )}
                        {canActivate && (
                          <button
                            type="button"
                            className="btn-link"
                            onClick={() => handleActivate(event)}
                            disabled={isBusy}
                          >
                            Activate
                          </button>
                        )}
                        {canDeactivate && (
                          <button
                            type="button"
                            className="btn-link"
                            onClick={() => handleDeactivate(event)}
                            disabled={isBusy}
                          >
                            Deactivate
                          </button>
                        )}
                        <button
                          type="button"
                          className="btn-link"
                          onClick={() => handleDuplicate(event)}
                          disabled={isBusy}
                        >
                          Duplicate
                        </button>
                        {canDelete && (
                          <button
                            type="button"
                            className="btn-link btn-link--danger"
                            onClick={() => handleDeleteSaleEvent(event)}
                            disabled={isBusy}
                          >
                            {isBusy ? "Deleting…" : "Delete"}
                          </button>
                        )}
                      </div>
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
