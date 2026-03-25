import React, { useCallback, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import dayjs from "dayjs";
import SaleEventService, { SaleEventStatusLabel } from "../../../services/SaleEventService";
import Notice from "../../../components/Common/CustomNotification";
import "./SaleEventAnalytics.scss";

const formatCurrency = (value) => {
  if (value === null || value === undefined) {
    return "$0.00";
  }

  const numeric = Number(value);
  if (!Number.isFinite(numeric)) {
    return "$0.00";
  }

  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 2
  }).format(numeric);
};

const formatNumber = (value) => {
  if (value === null || value === undefined) {
    return "0";
  }

  const numeric = Number(value);
  if (!Number.isFinite(numeric)) {
    return "0";
  }

  return new Intl.NumberFormat("en-US").format(numeric);
};

const SaleEventAnalytics = () => {
  const navigate = useNavigate();
  const { id } = useParams();

  const [loading, setLoading] = useState(true);
  const [saleEvent, setSaleEvent] = useState(null);
  const [performance, setPerformance] = useState(null);
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const loadSaleEvent = useCallback(async () => {
    try {
      const data = await SaleEventService.getSaleEventById(id);
      if (!data) {
        Notice({ msg: "Sale event not found.", isSuccess: false });
        navigate("/marketing/sale-events");
        return;
      }
      setSaleEvent(data);
    } catch (error) {
      Notice({
        msg: "Could not load sale event.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
      navigate("/marketing/sale-events");
    }
  }, [id, navigate]);

  const loadPerformance = useCallback(async () => {
    setLoading(true);
    try {
      const query = {};
      if (startDate) {
        query.startDate = dayjs(startDate).utc().toISOString();
      }
      if (endDate) {
        query.endDate = dayjs(endDate).utc().toISOString();
      }

      const data = await SaleEventService.getPerformanceMetrics(id, query);
      setPerformance(data);
    } catch (error) {
      Notice({
        msg: "Could not load performance metrics.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    } finally {
      setLoading(false);
    }
  }, [id, startDate, endDate]);

  useEffect(() => {
    loadSaleEvent();
  }, [loadSaleEvent]);

  useEffect(() => {
    loadPerformance();
  }, [loadPerformance]);

  const handleExport = () => {
    if (!performance) {
      return;
    }

    const csvRows = [];
    csvRows.push(["Sale Event Performance Report"]);
    csvRows.push([]);
    csvRows.push(["Sale Event", saleEvent?.name || ""]);
    csvRows.push(["Status", SaleEventStatusLabel[saleEvent?.status] || ""]);
    csvRows.push(["Start Date", saleEvent?.startDate ? dayjs(saleEvent.startDate).format("YYYY-MM-DD HH:mm") : ""]);
    csvRows.push(["End Date", saleEvent?.endDate ? dayjs(saleEvent.endDate).format("YYYY-MM-DD HH:mm") : ""]);
    csvRows.push([]);
    csvRows.push(["Overall Metrics"]);
    csvRows.push(["Order Count", performance.orderCount || 0]);
    csvRows.push(["Total Sales Revenue", performance.totalSalesRevenue || 0]);
    csvRows.push(["Total Discount Amount", performance.totalDiscountAmount || 0]);
    csvRows.push(["Total Items Sold", performance.totalItemsSold || 0]);
    csvRows.push(["Average Discount Per Order", performance.averageDiscountPerOrder || 0]);
    csvRows.push(["Average Order Value", performance.averageOrderValue || 0]);
    csvRows.push([]);

    if (performance.tierPerformance && performance.tierPerformance.length > 0) {
      csvRows.push(["Tier Performance"]);
      csvRows.push(["Tier", "Priority", "Order Count", "Total Sales Revenue", "Total Discount Amount"]);
      performance.tierPerformance.forEach((tier) => {
        csvRows.push([
          tier.tierLabel || `Tier ${tier.priority}`,
          tier.priority,
          tier.orderCount || 0,
          tier.totalSalesRevenue || 0,
          tier.totalDiscountAmount || 0
        ]);
      });
    }

    const csvContent = csvRows.map((row) => row.join(",")).join("\n");
    const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
    const link = document.createElement("a");
    const url = URL.createObjectURL(blob);
    link.setAttribute("href", url);
    link.setAttribute("download", `sale-event-${id}-performance.csv`);
    link.style.visibility = "hidden";
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

    Notice({ msg: "Performance report exported.", isSuccess: true });
  };

  if (loading && !performance) {
    return (
      <div className="sale-event-analytics">
        <div className="sale-event-analytics__loading">Loading performance metrics...</div>
      </div>
    );
  }

  return (
    <div className="sale-event-analytics">
      <header className="sale-event-analytics__header">
        <div>
          <button
            type="button"
            className="sale-event-analytics__back"
            onClick={() => navigate("/marketing/sale-events")}
          >
            ← Back to sale events
          </button>
          <h1>{saleEvent?.name || "Sale Event"}</h1>
          <p>Performance analytics and metrics</p>
        </div>
        <div className="sale-event-analytics__actions">
          <button type="button" className="btn-secondary" onClick={loadPerformance} disabled={loading}>
            Refresh
          </button>
          <button type="button" className="btn-primary" onClick={handleExport} disabled={!performance}>
            Export CSV
          </button>
        </div>
      </header>

      <section className="sale-event-analytics__filters">
        <label>
          <span>Start date</span>
          <input
            type="date"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
          />
        </label>
        <label>
          <span>End date</span>
          <input
            type="date"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
          />
        </label>
        <button type="button" className="btn-secondary" onClick={loadPerformance}>
          Apply filters
        </button>
        <button
          type="button"
          className="btn-secondary"
          onClick={() => {
            setStartDate("");
            setEndDate("");
          }}
        >
          Clear
        </button>
      </section>

      {performance ? (
        <>
          <section className="sale-event-analytics__overview">
            <h2>Overall Performance</h2>
            <div className="sale-event-analytics__metrics">
              <div className="sale-event-analytics__metric">
                <div className="sale-event-analytics__metric-label">Order Count</div>
                <div className="sale-event-analytics__metric-value">{formatNumber(performance.orderCount)}</div>
              </div>
              <div className="sale-event-analytics__metric">
                <div className="sale-event-analytics__metric-label">Total Sales Revenue</div>
                <div className="sale-event-analytics__metric-value">{formatCurrency(performance.totalSalesRevenue)}</div>
              </div>
              <div className="sale-event-analytics__metric">
                <div className="sale-event-analytics__metric-label">Total Discount Amount</div>
                <div className="sale-event-analytics__metric-value">{formatCurrency(performance.totalDiscountAmount)}</div>
              </div>
              <div className="sale-event-analytics__metric">
                <div className="sale-event-analytics__metric-label">Total Items Sold</div>
                <div className="sale-event-analytics__metric-value">{formatNumber(performance.totalItemsSold)}</div>
              </div>
              <div className="sale-event-analytics__metric">
                <div className="sale-event-analytics__metric-label">Average Discount Per Order</div>
                <div className="sale-event-analytics__metric-value">{formatCurrency(performance.averageDiscountPerOrder)}</div>
              </div>
              <div className="sale-event-analytics__metric">
                <div className="sale-event-analytics__metric-label">Average Order Value</div>
                <div className="sale-event-analytics__metric-value">{formatCurrency(performance.averageOrderValue)}</div>
              </div>
            </div>
          </section>

          {performance.tierPerformance && performance.tierPerformance.length > 0 ? (
            <section className="sale-event-analytics__tiers">
              <h2>Tier Performance Breakdown</h2>
              <div className="sale-event-analytics__table-wrapper">
                <table className="sale-event-analytics__table">
                  <thead>
                    <tr>
                      <th>Tier</th>
                      <th>Priority</th>
                      <th>Order Count</th>
                      <th>Total Sales Revenue</th>
                      <th>Total Discount Amount</th>
                      <th>Avg Discount Per Order</th>
                    </tr>
                  </thead>
                  <tbody>
                    {performance.tierPerformance.map((tier) => {
                      const avgDiscount = tier.orderCount > 0
                        ? tier.totalDiscountAmount / tier.orderCount
                        : 0;

                      return (
                        <tr key={tier.tierId}>
                          <td>{tier.tierLabel || `Tier ${tier.priority}`}</td>
                          <td>{tier.priority}</td>
                          <td>{formatNumber(tier.orderCount)}</td>
                          <td>{formatCurrency(tier.totalSalesRevenue)}</td>
                          <td>{formatCurrency(tier.totalDiscountAmount)}</td>
                          <td>{formatCurrency(avgDiscount)}</td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              </div>
            </section>
          ) : null}

          {performance.lastUpdated ? (
            <div className="sale-event-analytics__footer">
              Last updated: {dayjs(performance.lastUpdated).format("DD MMM YYYY HH:mm")}
            </div>
          ) : null}
        </>
      ) : (
        <div className="sale-event-analytics__empty">
          <p>No performance data available yet.</p>
          <p>Metrics will appear once orders are placed during this sale event.</p>
        </div>
      )}
    </div>
  );
};

export default SaleEventAnalytics;
