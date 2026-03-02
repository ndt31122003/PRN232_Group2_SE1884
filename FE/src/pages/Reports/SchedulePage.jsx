import React, { useCallback, useEffect, useState } from "react";
import Notice from "../../components/Common/CustomNotification";
import ReportService from "../../services/ReportService";
import "./ReportsCommon.scss";
import "./SchedulePage.scss";

const SOURCE_FILTERS = [
  { value: "all", label: "All" },
  { value: "orders", label: "Orders" }
];

const SOURCE_OPTIONS = [
  {
    value: "orders",
    label: "Orders",
    description: "Receive order activity reports delivered automatically."
  }
];

const ORDER_TYPE_OPTIONS = [
  { value: "all-orders", label: "All orders" },
  { value: "awaiting-payment", label: "Awaiting payment" },
  { value: "awaiting-shipment", label: "Awaiting shipment" },
  { value: "awaiting-shipment-overdue", label: "Awaiting shipment - overdue" },
  { value: "awaiting-shipment-24h", label: "Awaiting shipment — ship within 24 hours" },
  { value: "awaiting-expedited", label: "Awaiting expedited shipment" },
  { value: "paid-shipped", label: "Paid and shipped" },
  { value: "paid-awaiting-feedback", label: "Paid - awaiting your feedback" },
  { value: "shipped-awaiting-feedback", label: "Shipped - awaiting your feedback" }
];

const FREQUENCY_OPTIONS = [
  { value: "hourly", label: "Hourly", enumValue: 0 },
  { value: "daily", label: "Daily", enumValue: 1 },
  { value: "weekly", label: "Weekly", enumValue: 2 },
  { value: "monthly", label: "Monthly", enumValue: 3 }
];

const ORDER_SUMMARY_COLUMNS = [
  "Sales record number",
  "Order number",
  "Buyer username",
  "Buyer email",
  "Buyer note",
  "Buyer address",
  "Ship to name",
  "Ship to address",
  "Item number",
  "Item title",
  "Additional columns"
];

const FREQUENCY_LABEL_BY_ENUM = FREQUENCY_OPTIONS.reduce((accumulator, option) => {
  if (typeof option.enumValue === "number") {
    accumulator[option.enumValue] = option.label;
  }
  return accumulator;
}, {});

const normalizeScheduleItem = (raw) => ({
  id: raw?.id ?? raw?.Id ?? "",
  source: raw?.source ?? raw?.Source ?? "",
  type: raw?.type ?? raw?.Type ?? "",
  frequency: raw?.frequency ?? raw?.Frequency ?? null,
  createdAtUtc: raw?.createdAtUtc ?? raw?.CreatedAtUtc ?? null,
  lastRunAtUtc: raw?.lastRunAtUtc ?? raw?.LastRunAtUtc ?? null,
  nextRunAtUtc: raw?.nextRunAtUtc ?? raw?.NextRunAtUtc ?? null,
  endDateUtc: raw?.endDateUtc ?? raw?.EndDateUtc ?? null,
  isActive: raw?.isActive ?? raw?.IsActive ?? false,
  deliveryEmail: raw?.deliveryEmail ?? raw?.DeliveryEmail ?? null
});

const formatDateTime = (value) => {
  if (!value) {
    return "—";
  }

  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) {
    return "—";
  }

  const dateFormatter = new Intl.DateTimeFormat("en-US", {
    month: "short",
    day: "2-digit",
    year: "numeric"
  });

  const timeFormatter = new Intl.DateTimeFormat("en-US", {
    hour: "numeric",
    minute: "2-digit",
    hour12: true
  });

  return `${dateFormatter.format(parsed)} ${timeFormatter.format(parsed)}`;
};

const SchedulePage = () => {
  const [sourceFilter, setSourceFilter] = useState("all");
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize, setPageSize] = useState(25);
  const [schedules, setSchedules] = useState([]);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [activeSheet, setActiveSheet] = useState(null);
  const [selectedSource, setSelectedSource] = useState(SOURCE_OPTIONS[0].value);
  const [selectedType, setSelectedType] = useState(ORDER_TYPE_OPTIONS[0].value);
  const [selectedFrequency, setSelectedFrequency] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const resolveSourceLabel = useCallback((key) => {
    if (!key || key === "all") {
      return undefined;
    }

    const option = SOURCE_OPTIONS.find((item) => item.value === key);
    return option?.label ?? key;
  }, []);

  const buildQueryParams = useCallback(() => {
    const params = {
      pageNumber,
      pageSize,
      onlyActive: true
    };

    const resolvedSource = resolveSourceLabel(sourceFilter);
    if (resolvedSource) {
      params.source = resolvedSource;
    }

    return params;
  }, [pageNumber, pageSize, resolveSourceLabel, sourceFilter]);

  const loadSchedules = useCallback(
    async (signal) => {
      setIsLoading(true);
      setErrorMessage("");

      try {
        const result = await ReportService.getReportSchedules(buildQueryParams(), signal);
        const items = Array.isArray(result?.items) ? result.items : [];

        setSchedules(items.map(normalizeScheduleItem));
        setTotalCount(result?.totalCount ?? 0);
      } catch (error) {
        if (signal?.aborted || error?.name === "CanceledError" || error?.code === "ERR_CANCELED") {
          return;
        }

        console.error("Failed to load report schedules", error);
        setErrorMessage("Unable to load schedules right now. Please try again later.");
      } finally {
        setIsLoading(false);
      }
    },
    [buildQueryParams]
  );

  const selectedSourceOption = SOURCE_OPTIONS.find((option) => option.value === selectedSource);
  const selectedTypeOption = ORDER_TYPE_OPTIONS.find((option) => option.value === selectedType);
  const selectedFrequencyOption = FREQUENCY_OPTIONS.find((option) => option.value === selectedFrequency);
  const isScheduleFormValid = Boolean(selectedSourceOption) && Boolean(selectedTypeOption) && Boolean(selectedFrequencyOption);

  useEffect(() => {
    const controller = new AbortController();

    loadSchedules(controller.signal);

    return () => controller.abort();
  }, [loadSchedules]);

  const closeModal = () => {
    setActiveSheet(null);
    setIsModalOpen(false);
    setIsSubmitting(false);
  };

  const handleCreateSchedule = async () => {
    if (!isScheduleFormValid || isSubmitting) {
      return;
    }

    const payload = {
      source: resolveSourceLabel(selectedSource) ?? selectedSourceOption?.label ?? selectedSource,
      type: selectedTypeOption?.label ?? selectedType,
      frequency: selectedFrequencyOption?.enumValue
    };

    setIsSubmitting(true);

    try {
      await ReportService.createReportSchedule(payload);
      Notice({ msg: "Schedule created", isSuccess: true });
      closeModal();
      await loadSchedules();
    } catch (error) {
      if (error?.name === "CanceledError" || error?.code === "ERR_CANCELED") {
        return;
      }

      console.error("Failed to create report schedule", error);
      Notice({ msg: "Unable to create schedule. Please try again.", isSuccess: false });
    } finally {
      setIsSubmitting(false);
    }
  };

  const totalPages = Math.max(1, Math.ceil(totalCount / Math.max(pageSize, 1)));
  const canGoPrevious = pageNumber > 1;
  const canGoNext = pageNumber < totalPages;

  const handleSourceFilterChange = (event) => {
    setSourceFilter(event.target.value);
    setPageNumber(1);
  };

  const handlePageSizeChange = (event) => {
    const nextSize = Number(event.target.value);
    setPageSize(Number.isNaN(nextSize) ? 25 : nextSize);
    setPageNumber(1);
  };

  const goToPreviousPage = () => {
    if (!canGoPrevious) {
      return;
    }

    setPageNumber((prev) => Math.max(prev - 1, 1));
  };

  const goToNextPage = () => {
    if (!canGoNext) {
      return;
    }

    setPageNumber((prev) => prev + 1);
  };

  const visibleSchedules = schedules;

  const openSheet = (sheet) => {
    setActiveSheet(sheet);
  };

  const renderSheetContent = () => {
    if (!activeSheet) {
      return null;
    }

    if (activeSheet === "source") {
      return SOURCE_OPTIONS.map((option) => {
        const isActive = option.value === selectedSource;
        return (
          <button
            key={option.value}
            type="button"
            className={`schedule-modal__sheet-option${isActive ? " is-active" : ""}`}
            onClick={() => {
              setSelectedSource(option.value);
              setActiveSheet(null);
            }}
          >
            <span className="schedule-modal__sheet-radio" aria-hidden="true">{isActive && <span />}</span>
            <span className="schedule-modal__sheet-copy">
              <span className="schedule-modal__sheet-title">{option.label}</span>
              <span className="schedule-modal__sheet-desc">{option.description}</span>
            </span>
          </button>
        );
      });
    }

    if (activeSheet === "type") {
      return ORDER_TYPE_OPTIONS.map((option) => {
        const isActive = option.value === selectedType;
        return (
          <button
            key={option.value}
            type="button"
            className={`schedule-modal__sheet-option${isActive ? " is-active" : ""}`}
            onClick={() => {
              setSelectedType(option.value);
              setActiveSheet(null);
            }}
          >
            <span className="schedule-modal__sheet-radio" aria-hidden="true">{isActive && <span />}</span>
            <span className="schedule-modal__sheet-copy">
              <span className="schedule-modal__sheet-title">{option.label}</span>
            </span>
          </button>
        );
      });
    }

    if (activeSheet === "frequency") {
      return FREQUENCY_OPTIONS.map((option) => {
        const isActive = option.value === selectedFrequency;
        return (
          <button
            key={option.value}
            type="button"
            className={`schedule-modal__sheet-option${isActive ? " is-active" : ""}`}
            onClick={() => {
              setSelectedFrequency(option.value);
              setActiveSheet(null);
            }}
          >
            <span className="schedule-modal__sheet-radio" aria-hidden="true">{isActive && <span />}</span>
            <span className="schedule-modal__sheet-copy">
              <span className="schedule-modal__sheet-title">{option.label}</span>
            </span>
          </button>
        );
      });
    }

    return null;
  };

  const sheetTitle = {
    source: "Select report source",
    type: "Select report type",
    frequency: "Select frequency"
  };

  return (
    <div className="reports-shell reports-schedule" data-testid="reports-schedule">
      <div className="reports-shell__comments">
        <a href="#comments" onClick={(event) => event.preventDefault()}>
          Comments
        </a>
      </div>

      <section className="reports-shell__hero">
        <div className="reports-shell__hero-copy">
          <h1>Schedule</h1>
          <h2>Set up scheduled reports</h2>
          <p>Automate the download process to receive certain reports on a regularly scheduled basis.</p>
        </div>
        <button type="button" className="reports-shell__hero-action" onClick={() => setIsModalOpen(true)}>
          Schedule report
        </button>
      </section>

  <section className="reports-shell__filters">
        <label htmlFor="schedule-source-filter">
          <span>Source</span>
          <select
            id="schedule-source-filter"
            value={sourceFilter}
            onChange={handleSourceFilterChange}
          >
            {SOURCE_FILTERS.map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>
        </label>
      </section>

      <div className="reports-shell__table-wrap">
        <table className="reports-shell__table reports-schedule__table">
          <thead>
            <tr>
              <th>Source</th>
              <th>Name</th>
              <th>Frequency</th>
              <th>End Date</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {isLoading ? (
              <tr>
                <td colSpan={5} className="reports-shell__empty">Loading schedules…</td>
              </tr>
            ) : errorMessage ? (
              <tr>
                <td colSpan={5} className="reports-shell__empty">{errorMessage}</td>
              </tr>
            ) : visibleSchedules.length === 0 ? (
              <tr>
                <td colSpan={5} className="reports-shell__empty">No schedule history found.</td>
              </tr>
            ) : (
              visibleSchedules.map((schedule) => {
                const frequencyValue = schedule.frequency;
                const frequencyLabel = FREQUENCY_LABEL_BY_ENUM[frequencyValue] ?? frequencyValue ?? "—";

                return (
                  <tr key={schedule.id}>
                    <td>{schedule.source}</td>
                    <td>{schedule.type}</td>
                    <td>{frequencyLabel}</td>
                    <td>{formatDateTime(schedule.endDateUtc)}</td>
                    <td>
                      <button type="button" className="reports-schedule__inline-action" disabled>
                        View
                      </button>
                    </td>
                  </tr>
                );
              })
            )}
          </tbody>
        </table>
      </div>

      <footer className="reports-shell__footer">
        <div className="reports-schedule__results">
          Results per page:
          <select value={pageSize} onChange={handlePageSizeChange}>
            {[25, 50, 75, 100].map((size) => (
              <option key={size} value={size}>
                {size}
              </option>
            ))}
          </select>
          <span className="reports-schedule__total">Total {totalCount}</span>
        </div>
        <div className="reports-schedule__pagination">
          <button type="button" onClick={goToPreviousPage} disabled={!canGoPrevious}>
            Previous
          </button>
          <span>
            Page {pageNumber} of {Number.isFinite(totalPages) ? totalPages : 1}
          </span>
          <button type="button" onClick={goToNextPage} disabled={!canGoNext}>
            Next
          </button>
        </div>
      </footer>

      {isModalOpen && (
        <div className="schedule-modal" role="dialog" aria-modal="true">
          <div className="schedule-modal__backdrop" onClick={closeModal} />
          <div className="schedule-modal__panel" role="document">
            <header className="schedule-modal__header">
              <button type="button" className="schedule-modal__close" aria-label="Close" onClick={closeModal}>
                ×
              </button>
              <h2>Create download schedule</h2>
            </header>
            <div className="schedule-modal__content">
              <div className="schedule-modal__main">
                <section className="schedule-modal__section">
                  <h3>Select report</h3>
                  <button type="button" className="schedule-modal__card" onClick={() => openSheet("source")}>
                    <span className="schedule-modal__card-label">Source</span>
                    <span className="schedule-modal__card-value">{selectedSourceOption?.label}</span>
                  </button>
                  <button type="button" className="schedule-modal__card" onClick={() => openSheet("type")}>
                    <span className="schedule-modal__card-label">Type</span>
                    <span className="schedule-modal__card-value">{selectedTypeOption?.label}</span>
                  </button>
                </section>

                <section className="schedule-modal__section">
                  <h3>Additional details</h3>
                  <button type="button" className="schedule-modal__card" onClick={() => openSheet("frequency")}>
                    <span className="schedule-modal__card-label">Frequency</span>
                    <span className="schedule-modal__card-value">
                      {selectedFrequencyOption?.label ?? "Select report generation frequency"}
                    </span>
                  </button>
                </section>
              </div>

              <aside className="schedule-modal__summary">
                <h4>Summary of Order reports columns:</h4>
                <ul>
                  {ORDER_SUMMARY_COLUMNS.map((column) => (
                    <li key={column}>{column}</li>
                  ))}
                </ul>
              </aside>

              {activeSheet && (
                <div className="schedule-modal__sheet" aria-label={sheetTitle[activeSheet]}>
                  <header className="schedule-modal__sheet-header">
                    <h3>{sheetTitle[activeSheet]}</h3>
                    <button type="button" className="schedule-modal__sheet-done" onClick={() => setActiveSheet(null)}>
                      Done
                    </button>
                  </header>
                  <div className="schedule-modal__sheet-body">{renderSheetContent()}</div>
                </div>
              )}
            </div>
            <footer className="schedule-modal__footer">
              <button type="button" className="schedule-modal__cancel" onClick={closeModal}>
                Cancel
              </button>
              <button
                type="button"
                className="schedule-modal__save"
                onClick={handleCreateSchedule}
                disabled={!isScheduleFormValid || isSubmitting}
              >
                {isSubmitting ? "Saving…" : "Save"}
              </button>
            </footer>
          </div>
        </div>
      )}
    </div>
  );
};

export default SchedulePage;
