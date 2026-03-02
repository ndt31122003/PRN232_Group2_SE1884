import React, { useCallback, useEffect, useMemo, useState } from "react";
import Notice from "../../components/Common/CustomNotification";
import ReportService from "../../services/ReportService";
import "./ReportsCommon.scss";
import "./DownloadsPage.scss";

const SOURCE_FILTERS = [
  { value: "all", label: "All" },
  { value: "listings", label: "Listings" },
  { value: "orders", label: "Orders" }
];

const SOURCE_CONFIG = {
  listings: {
    value: "listings",
    label: "Listings",
    description: "Export inventory level insight for all of your listings.",
    types: [
      {
        value: "all-active",
        label: "All active listings",
        description: "View the details of all your listings that are active as of now."
      },
      {
        value: "unsold",
        label: "Unsold listings",
        description: "View your unsold and ended listings within the last 90 days."
      },
      {
        value: "scheduled",
        label: "All scheduled listings",
        description: "View the details of your listings that are currently scheduled."
      }
    ],
    summaryTitle: "Summary of Listing reports columns:",
    summaryColumns: [
      "Item number",
      "Title",
      "Variation details",
      "Custom label (SKU)",
      "Available quantity",
      "Format",
      "Price",
      "Sold quantity",
      "Watchers",
      "Bids",
      "Listing site",
      "Additional columns"
    ],
    showDateRange: false
  },
  orders: {
    value: "orders",
    label: "Orders",
    description: "Pull a transaction level report for customer orders.",
    types: [
      {
        value: "all-orders",
        label: "All orders",
        description: "Include all completed, cancelled, and in-progress orders."
      },
      {
        value: "awaiting-payment",
        label: "Awaiting payment",
        description: "Orders that are pending payment from buyers."
      },
      {
        value: "awaiting-shipment",
        label: "Awaiting shipment",
        description: "Orders that are ready to be fulfilled."
      },
      {
        value: "awaiting-shipment-overdue",
        label: "Awaiting shipment - overdue",
        description: "Orders past expected ship date that still need fulfillment."
      },
      {
        value: "awaiting-shipment-24h",
  label: "Awaiting shipment - ship within 24 hours",
        description: "Orders that must ship within the next 24 hours."
      },
      {
        value: "awaiting-expedited",
        label: "Awaiting expedited shipment",
        description: "Orders with expedited shipping services selected."
      },
      {
        value: "paid-shipped",
        label: "Paid and shipped",
        description: "Orders that have been paid and marked as shipped."
      },
      {
        value: "paid-awaiting-feedback",
        label: "Paid - awaiting your feedback",
        description: "Completed orders that still need seller feedback."
      },
      {
        value: "shipped-awaiting-feedback",
        label: "Shipped - awaiting your feedback",
        description: "Shipped orders pending seller feedback."
      }
    ],
    summaryTitle: "Summary of Order reports columns:",
    summaryColumns: [
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
    ],
    showDateRange: true
  }
};

const DEFAULT_SOURCE = SOURCE_CONFIG.orders ?? Object.values(SOURCE_CONFIG)[0];

const DATE_RANGE_PRESETS = [
  { value: "today", label: "Today" },
  { value: "yesterday", label: "Yesterday" },
  { value: "this-week", label: "This Week" },
  { value: "last-week", label: "Last Week" },
  { value: "this-month", label: "This Month" },
  { value: "last-month", label: "Last Month" },
  { value: "this-year", label: "This Year" },
  { value: "last-year", label: "Last Year" },
  { value: "last-90-days", label: "Last 90 Days" },
  {
    value: "custom",
    label: "Custom range",
    description: "Use a specific start and end date for your export.",
    requiresCustomInput: true
  }
];

const normalizeDownloadItem = (raw) => ({
  id: raw?.id ?? raw?.Id ?? "",
  referenceCode: raw?.referenceCode ?? raw?.ReferenceCode ?? "",
  source: raw?.source ?? raw?.Source ?? "",
  type: raw?.type ?? raw?.Type ?? "",
  status: raw?.status ?? raw?.Status ?? "",
  requestedAtUtc: raw?.requestedAtUtc ?? raw?.RequestedAtUtc ?? null,
  completedAtUtc: raw?.completedAtUtc ?? raw?.CompletedAtUtc ?? null,
  fileUrl: raw?.fileUrl ?? raw?.FileUrl ?? null,
  dateRange: raw?.dateRange ?? raw?.DateRange ?? null
});

const convertDateInputToIso = (value) => {
  if (!value) {
    return undefined;
  }

  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) {
    return undefined;
  }

  return parsed.toISOString();
};

const formatDateTime = (value) => {
  if (!value) {
    return "-";
  }

  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) {
    return "-";
  }

  const dateFormatter = new Intl.DateTimeFormat("en-US", {
    month: "short",
    day: "2-digit",
    year: "numeric"
  });

  const timeFormatter = new Intl.DateTimeFormat("en-US", {
    hour: "numeric",
    minute: "2-digit",
    hour12: true,
    timeZoneName: "short"
  });

  const timeParts = timeFormatter.formatToParts(parsed);
  const hour = timeParts.find((part) => part.type === "hour")?.value ?? "";
  const minute = timeParts.find((part) => part.type === "minute")?.value ?? "";
  const dayPeriod = (timeParts.find((part) => part.type === "dayPeriod")?.value ?? "").toLowerCase();
  const timeZoneName = timeParts.find((part) => part.type === "timeZoneName")?.value ?? "";

  return `${dateFormatter.format(parsed)} at ${hour}:${minute}${dayPeriod} ${timeZoneName}`.trim();
};

const resolveDownloadStatus = (download) => {
  const normalizedStatus = (download?.status ?? "").toLowerCase();
  const hasFile = Boolean(download?.fileUrl);

  if (normalizedStatus === "completed") {
    return { label: "Completed", modifier: "completed", canDownload: hasFile };
  }

  if (normalizedStatus === "failed") {
    return { label: "Failed", modifier: "failed", canDownload: false };
  }

  if (hasFile) {
    return { label: "Completed", modifier: "completed", canDownload: true };
  }

  return { label: "Failed", modifier: "failed", canDownload: false };
};

const DownloadsPage = () => {
  const [sourceFilter, setSourceFilter] = useState("all");
  const [pageSize, setPageSize] = useState(25);
  const [pageNumber, setPageNumber] = useState(1);
  const [downloads, setDownloads] = useState([]);
  const [totalCount, setTotalCount] = useState(0);
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [activeSheet, setActiveSheet] = useState(null);
  const [selectedSource, setSelectedSource] = useState(DEFAULT_SOURCE?.value ?? "");
  const [selectedType, setSelectedType] = useState(DEFAULT_SOURCE?.types?.[0]?.value ?? "");
  const [selectedDateRange, setSelectedDateRange] = useState("last-90-days");
  const [customRange, setCustomRange] = useState({
    start: "",
    end: "",
    timezone: "PDT"
  });
  const [isSubmitting, setIsSubmitting] = useState(false);

  const selectedSourceConfig = SOURCE_CONFIG[selectedSource] ?? DEFAULT_SOURCE ?? Object.values(SOURCE_CONFIG)[0];
  const selectedTypeOption = useMemo(
    () => selectedSourceConfig?.types.find((type) => type.value === selectedType),
    [selectedSourceConfig, selectedType]
  );
  const selectedDateRangeOption = useMemo(
    () => DATE_RANGE_PRESETS.find((option) => option.value === selectedDateRange),
    [selectedDateRange]
  );

  useEffect(() => {
    if (!selectedSourceConfig) {
      return;
    }

    if (!selectedSourceConfig.types.some((type) => type.value === selectedType)) {
      setSelectedType(selectedSourceConfig.types[0]?.value ?? "");
    }

    if (selectedSourceConfig.showDateRange && !selectedDateRange) {
      setSelectedDateRange(DATE_RANGE_PRESETS[0]?.value ?? "");
    }
  }, [selectedSourceConfig, selectedType, selectedDateRange]);

  const resolveSourceLabel = useCallback((key) => {
    if (!key || key === "all") {
      return undefined;
    }

    const configEntry = SOURCE_CONFIG[key];
    return configEntry?.label ?? key;
  }, []);

  const buildQueryParams = useCallback(() => {
    const params = {
      pageNumber,
      pageSize
    };

    const resolvedSource = resolveSourceLabel(sourceFilter);
    if (resolvedSource) {
      params.source = resolvedSource;
    }

    return params;
  }, [pageNumber, pageSize, resolveSourceLabel, sourceFilter]);

  const loadDownloads = useCallback(
    async (signal) => {
      setIsLoading(true);
      setErrorMessage("");

      try {
        const result = await ReportService.getReportDownloads(buildQueryParams(), signal);
        const items = Array.isArray(result?.items) ? result.items : [];

        setDownloads(items.map(normalizeDownloadItem));
        setTotalCount(result?.totalCount ?? 0);
      } catch (error) {
        if (signal?.aborted || error?.name === "CanceledError" || error?.code === "ERR_CANCELED") {
          return;
        }

        console.error("Failed to load report downloads", error);
        setErrorMessage("Unable to load downloads right now. Please try again later.");
      } finally {
        setIsLoading(false);
      }
    },
    [buildQueryParams]
  );

  useEffect(() => {
    const controller = new AbortController();

    loadDownloads(controller.signal);

    return () => controller.abort();
  }, [loadDownloads]);

  const isCustomRangeValid = !selectedDateRangeOption?.requiresCustomInput
    || (Boolean(customRange.start) && Boolean(customRange.end));

  const isDownloadFormValid = Boolean(selectedSourceConfig)
    && Boolean(selectedTypeOption)
    && (!selectedSourceConfig?.showDateRange
      || (Boolean(selectedDateRangeOption) && isCustomRangeValid));

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

  const handleDownload = (download) => {
    if (!download?.fileUrl) {
      Notice({ msg: "Report failed to generate. Please request a new download.", isSuccess: false });
      return;
    }

    window.open(download.fileUrl, "_blank", "noopener");
  };

  const closeModal = () => {
    setActiveSheet(null);
    setIsModalOpen(false);
    setIsSubmitting(false);
  };

  const handleModalDownload = async () => {
    if (!isDownloadFormValid || isSubmitting) {
      return;
    }

    const payload = {
      source: selectedSourceConfig?.label ?? resolveSourceLabel(selectedSource) ?? selectedSource,
      type: selectedTypeOption?.label ?? selectedType
    };

    if (selectedSourceConfig?.showDateRange && selectedDateRangeOption?.requiresCustomInput) {
      const rangeStartIso = convertDateInputToIso(customRange.start);
      const rangeEndIso = convertDateInputToIso(customRange.end);

      if (rangeStartIso) {
        payload.rangeStartUtc = rangeStartIso;
      }

      if (rangeEndIso) {
        payload.rangeEndUtc = rangeEndIso;
      }

      if (customRange.timezone) {
        payload.timeZone = customRange.timezone;
      }
    }

    setIsSubmitting(true);

    try {
      const result = await ReportService.createReportDownload(payload);

      if (result?.fileUrl) {
        const opened = window.open(result.fileUrl, "_blank", "noopener");
        if (!opened) {
          Notice({ msg: "Download ready. Please enable pop-ups to access the file.", isSuccess: false });
        } else {
          Notice({ msg: "Download ready", isSuccess: true });
        }
      } else {
        Notice({ msg: "Download ready. Use the downloads list to access the file.", isSuccess: true });
      }
      closeModal();
      await loadDownloads();
    } catch (error) {
      if (error?.name === "CanceledError" || error?.code === "ERR_CANCELED") {
        return;
      }

      console.error("Failed to create report download", error);
      Notice({ msg: "Unable to start download. Please try again.", isSuccess: false });
    } finally {
      setIsSubmitting(false);
    }
  };

  const openSheet = (sheet) => {
    if (sheet === "dateRange" && !selectedSourceConfig?.showDateRange) {
      return;
    }

    setActiveSheet(sheet);
  };

  const formatCustomRangeLabel = () => {
    if (!customRange.start || !customRange.end) {
      return "Select";
    }

    const reformat = (value) => {
      const match = /^([0-9]{4})-([0-9]{2})-([0-9]{2})$/.exec(value);
      if (!match) {
        return value;
      }
      const [, year, month, day] = match;
      return `${month}/${day}/${year}`;
    };

    return `${reformat(customRange.start)} - ${reformat(customRange.end)}`;
  };

  const resolveDateRangeValue = () => {
    if (!selectedSourceConfig?.showDateRange) {
      return null;
    }

    if (selectedDateRangeOption?.requiresCustomInput) {
      return `${formatCustomRangeLabel()} ${customRange.timezone}`.trim();
    }

    return selectedDateRangeOption?.label ?? "Select";
  };

  const renderSheetOptions = () => {
    if (!activeSheet) {
      return null;
    }

    if (activeSheet === "source") {
      return Object.values(SOURCE_CONFIG).map((config) => {
        const isActive = config.value === selectedSource;
        return (
          <button
            key={config.value}
            type="button"
            className={`reports-downloads__sheet-option${isActive ? " is-active" : ""}`}
            onClick={() => {
              setSelectedSource(config.value);
              setSelectedType(config.types[0]?.value ?? "");
              setActiveSheet(null);
            }}
          >
            <span className="reports-downloads__sheet-radio" aria-hidden="true">
              {isActive && <span />}
            </span>
            <span>
              <span className="reports-downloads__sheet-option-title">{config.label}</span>
              <span className="reports-downloads__sheet-option-desc">{config.description}</span>
            </span>
          </button>
        );
      });
    }

    if (activeSheet === "type") {
      return selectedSourceConfig?.types.map((option) => {
        const isActive = option.value === selectedType;
        return (
          <button
            key={option.value}
            type="button"
            className={`reports-downloads__sheet-option${isActive ? " is-active" : ""}`}
            onClick={() => {
              setSelectedType(option.value);
              setActiveSheet(null);
            }}
          >
            <span className="reports-downloads__sheet-radio" aria-hidden="true">
              {isActive && <span />}
            </span>
            <span>
              <span className="reports-downloads__sheet-option-title">{option.label}</span>
              <span className="reports-downloads__sheet-option-desc">{option.description}</span>
            </span>
          </button>
        );
      });
    }

    if (activeSheet === "dateRange") {
      return (
        <>
          {DATE_RANGE_PRESETS.map((option) => {
            const isActive = option.value === selectedDateRange;
            return (
              <button
                key={option.value}
                type="button"
                className={`reports-downloads__sheet-option${isActive ? " is-active" : ""}`}
                onClick={() => {
                  setSelectedDateRange(option.value);
                  if (!option.requiresCustomInput) {
                    setActiveSheet(null);
                  }
                }}
              >
                <span className="reports-downloads__sheet-radio" aria-hidden="true">
                  {isActive && <span />}
                </span>
                <span>
                  <span className="reports-downloads__sheet-option-title">{option.label}</span>
                  {option.description && (
                    <span className="reports-downloads__sheet-option-desc">{option.description}</span>
                  )}
                </span>
              </button>
            );
          })}
          {selectedDateRangeOption?.requiresCustomInput && (
            <div className="reports-downloads__custom-range">
              <div className="reports-downloads__custom-range-row">
                <label>
                  <span>Start date</span>
                  <input
                    type="date"
                    value={customRange.start}
                    onChange={(event) =>
                      setCustomRange((prev) => ({ ...prev, start: event.target.value }))
                    }
                  />
                </label>
                <label>
                  <span>End date</span>
                  <input
                    type="date"
                    value={customRange.end}
                    onChange={(event) =>
                      setCustomRange((prev) => ({ ...prev, end: event.target.value }))
                    }
                  />
                </label>
              </div>
              <label className="reports-downloads__timezone">
                <span>Time zone</span>
                <select
                  value={customRange.timezone}
                  onChange={(event) =>
                    setCustomRange((prev) => ({ ...prev, timezone: event.target.value }))
                  }
                >
                  <option value="PDT">Pacific Time (PDT)</option>
                  <option value="UTC">Coordinated Universal Time (UTC)</option>
                  <option value="EST">Eastern Time (EST)</option>
                </select>
              </label>
            </div>
          )}
        </>
      );
    }

    return null;
  };

  const sheetTitleMap = {
    source: "Select report source",
    type: "Select report type",
    dateRange: "Select date range"
  };

  return (
    <div className="reports-shell reports-downloads" data-testid="reports-downloads">
      <div className="reports-shell__comments">
        <a href="#comments" onClick={(event) => event.preventDefault()}>
          Comments
        </a>
      </div>

      <section className="reports-shell__hero">
        <div className="reports-shell__hero-copy">
          <h1>Downloads</h1>
          <h2>Get more information about your business</h2>
          <p>
            Manage your business and gain insights by downloading up-to-date reports. Choose a report
            and export it whenever you need a fresh snapshot.
          </p>
        </div>
        <button
          type="button"
          className="reports-shell__hero-action"
          onClick={() => setIsModalOpen(true)}
        >
          Download report
        </button>
      </section>

      <section className="reports-shell__filters">
        <label htmlFor="reports-downloads-source" className="reports-downloads__filter">
          <span>Source</span>
          <select
            id="reports-downloads-source"
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
        <table className="reports-shell__table reports-downloads__table">
          <thead>
            <tr>
              <th>Source</th>
              <th>Type</th>
              <th>Ref ID</th>
              <th>Requested</th>
              <th>Status</th>
              <th>Action</th>
            </tr>
          </thead>
          <tbody>
            {isLoading ? (
              <tr>
                <td colSpan={6} className="reports-shell__empty">
                  Loading downloads...
                </td>
              </tr>
            ) : errorMessage ? (
              <tr>
                <td colSpan={6} className="reports-shell__empty">
                  {errorMessage}
                </td>
              </tr>
            ) : downloads.length === 0 ? (
              <tr>
                <td colSpan={6} className="reports-shell__empty">
                  No downloads found for the selected source.
                </td>
              </tr>
            ) : (
              downloads.map((download) => {
                const { label: statusLabel, modifier: statusModifier, canDownload } = resolveDownloadStatus(download);
                const reference = download.referenceCode || download.id;
                const actionLabel = canDownload ? "Download" : "Unavailable";

                return (
                  <tr key={download.id ?? reference}>
                    <td>{download.source}</td>
                    <td>{download.type}</td>
                    <td>{reference}</td>
                    <td>{formatDateTime(download.requestedAtUtc)}</td>
                    <td>
                      <span
                        className={`reports-downloads__status${statusModifier ? ` reports-downloads__status--${statusModifier}` : ""}`}
                      >
                        {statusLabel}
                      </span>
                    </td>
                    <td>
                      <button
                        type="button"
                        className="reports-downloads__action"
                        onClick={() => handleDownload(download)}
                        disabled={!canDownload}
                      >
                        {actionLabel}
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
        <div className="reports-downloads__results">
          Results per page:
          <select value={pageSize} onChange={handlePageSizeChange}>
            {[25, 50, 75, 100].map((size) => (
              <option key={size} value={size}>
                {size}
              </option>
            ))}
          </select>
          <span className="reports-downloads__total">Total {totalCount}</span>
        </div>
        <div className="reports-downloads__pagination">
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
        <div className="reports-downloads__modal-backdrop" role="dialog" aria-modal="true">
          <div className="reports-downloads__modal" role="document">
            <header className="reports-downloads__modal-header">
              <button
                type="button"
                className="reports-downloads__modal-close"
                aria-label="Close"
                onClick={closeModal}
              >
                X
              </button>
              <h2>Download report</h2>
            </header>
            <div className="reports-downloads__modal-content">
              <div className="reports-downloads__modal-left">
                <h3>Select report</h3>
                <button
                  type="button"
                  className="reports-downloads__select-card"
                  onClick={() => openSheet("source")}
                >
                  <span className="reports-downloads__select-label">Source</span>
                  <span className="reports-downloads__select-value reports-downloads__select-value--badge">
                    {selectedSourceConfig?.label ?? "Select"}
                  </span>
                </button>
                <button
                  type="button"
                  className="reports-downloads__select-card"
                  onClick={() => openSheet("type")}
                >
                  <span className="reports-downloads__select-label">Type</span>
                  <span className="reports-downloads__select-value reports-downloads__select-value--badge">
                    {selectedTypeOption?.label ?? "Select report type"}
                  </span>
                </button>
                {selectedSourceConfig?.showDateRange && (
                  <button
                    type="button"
                    className="reports-downloads__select-card"
                    onClick={() => openSheet("dateRange")}
                  >
                    <span className="reports-downloads__select-label">Date range</span>
                    <span className="reports-downloads__select-value reports-downloads__select-value--badge">
                      {resolveDateRangeValue()}
                    </span>
                  </button>
                )}
              </div>
              <aside className="reports-downloads__modal-summary">
                <h4>{selectedSourceConfig?.summaryTitle}</h4>
                <ul>
                  {(selectedSourceConfig?.summaryColumns ?? []).map((column) => (
                    <li key={column}>{column}</li>
                  ))}
                </ul>
              </aside>
              {activeSheet && (
                <div className="reports-downloads__sheet" aria-label={sheetTitleMap[activeSheet]}>
                  <header className="reports-downloads__sheet-header">
                    <h3>{sheetTitleMap[activeSheet]}</h3>
                    <button
                      type="button"
                      className="reports-downloads__sheet-done"
                      onClick={() => setActiveSheet(null)}
                    >
                      Done
                    </button>
                  </header>
                  <div className="reports-downloads__sheet-body">{renderSheetOptions()}</div>
                </div>
              )}
            </div>
            <footer className="reports-downloads__modal-footer">
              <button type="button" className="reports-downloads__modal-cancel" onClick={closeModal}>
                Cancel
              </button>
              <button
                type="button"
                className="reports-downloads__modal-download"
                onClick={handleModalDownload}
                disabled={!isDownloadFormValid || isSubmitting}
              >
                {isSubmitting ? "Processing..." : "Download"}
              </button>
            </footer>
          </div>
        </div>
      )}
    </div>
  );
};

export default DownloadsPage;
