import React, { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import Notice from "../../../components/Common/CustomNotification";
import SaleEventService, { SaleEventDiscountType, SaleEventMode, SaleEventStatus } from "../../../services/SaleEventService";
import "./CreateSaleEvent.scss";

dayjs.extend(utc);

const DATETIME_FORMAT = "YYYY-MM-DDTHH:mm";
const LISTING_PAGE_SIZE = 25;
const emptyListingPage = { items: [], totalCount: 0, pageNumber: 1, pageSize: LISTING_PAGE_SIZE };

const currencyFormatter = new Intl.NumberFormat("en-US", {
  style: "currency",
  currency: "USD",
  minimumFractionDigits: 2
});

const formatCurrency = (value) => {
  if (value === null || value === undefined) {
    return "--";
  }

  const numeric = Number(value);
  if (!Number.isFinite(numeric)) {
    return "--";
  }

  return currencyFormatter.format(numeric);
};

const formatListingFormat = (format) => {
  switch (format) {
    case 1:
      return "Fixed price";
    case 2:
      return "Auction";
    default:
      return "--";
  }
};

const mapModeFromEnum = (mode) => (mode === SaleEventMode.SaleEventOnly ? "highlight" : "discount");

const mapModeToEnum = (mode) => (mode === "highlight" ? SaleEventMode.SaleEventOnly : SaleEventMode.DiscountAndSaleEvent);

const mapDiscountTypeFromEnum = (discountType) => (discountType === SaleEventDiscountType.Amount ? "Amount" : "Percent");

const mapDiscountTypeToEnum = (discountType) => (discountType === "Amount" ? SaleEventDiscountType.Amount : SaleEventDiscountType.Percent);

const parseNumberOrNull = (value) => {
  if (value === null || value === undefined) {
    return null;
  }

  if (typeof value === "number") {
    return Number.isFinite(value) ? value : null;
  }

  const trimmed = String(value).trim();
  if (!trimmed) {
    return null;
  }

  const parsed = Number(trimmed);
  return Number.isFinite(parsed) ? parsed : null;
};

const EditSaleEvent = () => {
  const navigate = useNavigate();
  const { id } = useParams();

  const [loading, setLoading] = useState(true);
  const [saleEvent, setSaleEvent] = useState(null);
  const [form, setForm] = useState(null);
  const [tiers, setTiers] = useState([]);
  const [activeTierId, setActiveTierId] = useState(null);
  const [errors, setErrors] = useState({});
  const [submitting, setSubmitting] = useState(false);
  const [listingSearchTerm, setListingSearchTerm] = useState("");
  const [currentSearch, setCurrentSearch] = useState("");
  const [listingPage, setListingPage] = useState(emptyListingPage);
  const [loadingListings, setLoadingListings] = useState(false);

  const canEditBasicInfo = useMemo(() => {
    if (!saleEvent) return false;
    const now = dayjs();
    const start = dayjs(saleEvent.startDate);
    return now.isBefore(start);
  }, [saleEvent]);

  const loadSaleEvent = useCallback(async () => {
    setLoading(true);
    try {
      const data = await SaleEventService.getSaleEventById(id);
      if (!data) {
        Notice({ msg: "Sale event not found.", isSuccess: false });
        navigate("/marketing/sale-events");
        return;
      }

      setSaleEvent(data);

      const mode = mapModeFromEnum(data.mode);
      setForm({
        name: data.name || "",
        description: data.description || "",
        mode,
        startDate: data.startDate ? dayjs(data.startDate).format(DATETIME_FORMAT) : "",
        endDate: data.endDate ? dayjs(data.endDate).format(DATETIME_FORMAT) : "",
        offerFreeShipping: Boolean(data.offerFreeShipping),
        includeSkippedItems: Boolean(data.includeSkippedItems),
        blockPriceIncreaseRevisions: Boolean(data.blockPriceIncreaseRevisions),
        highlightPercentage: data.highlightPercentage ? String(data.highlightPercentage) : ""
      });

      if (data.discountTiers && Array.isArray(data.discountTiers)) {
        const mappedTiers = data.discountTiers.map((tier) => ({
          id: tier.id,
          priority: String(tier.priority),
          discountType: mapDiscountTypeFromEnum(tier.discountType),
          discountValue: String(tier.discountValue),
          label: tier.label || "",
          listingCount: tier.listingCount || 0
        }));
        setTiers(mappedTiers);
        if (mappedTiers.length > 0) {
          setActiveTierId(mappedTiers[0].id);
        }
      }
    } catch (error) {
      Notice({
        msg: "Could not load sale event.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
      navigate("/marketing/sale-events");
    } finally {
      setLoading(false);
    }
  }, [id, navigate]);

  useEffect(() => {
    loadSaleEvent();
  }, [loadSaleEvent]);

  const activeTier = useMemo(() => tiers.find((tier) => tier.id === activeTierId) ?? null, [tiers, activeTierId]);

  const fetchListings = useCallback(async (pageNumber = 1, searchTerm = "") => {
    if (form?.mode !== "discount") {
      setListingPage(emptyListingPage);
      return;
    }

    setLoadingListings(true);
    try {
      const sanitized = searchTerm?.trim() ? searchTerm.trim() : undefined;
      const page = await SaleEventService.getEligibleListings({
        searchTerm: sanitized,
        pageNumber,
        pageSize: LISTING_PAGE_SIZE,
        excludeAlreadyAssigned: false
      });
      setListingPage(page);
    } catch (error) {
      Notice({
        msg: "Could not load listings.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    } finally {
      setLoadingListings(false);
    }
  }, [form?.mode]);

  useEffect(() => {
    if (form?.mode !== "discount") {
      setListingPage(emptyListingPage);
      return;
    }

    fetchListings(1, currentSearch);
  }, [form?.mode, currentSearch, fetchListings]);

  const updateFormField = (key, value) => {
    setForm((prev) => ({
      ...prev,
      [key]: value
    }));
    setErrors((prev) => {
      if (!prev[key]) {
        return prev;
      }
      const next = { ...prev };
      delete next[key];
      return next;
    });
  };

  const validate = () => {
    const nextErrors = {};

    if (!form.name.trim()) {
      nextErrors.name = "Enter a sale event name.";
    }

    const start = dayjs(form.startDate);
    const end = dayjs(form.endDate);
    if (!start.isValid()) {
      nextErrors.startDate = "Start date is invalid.";
    }
    if (!end.isValid()) {
      nextErrors.endDate = "End date is invalid.";
    }
    if (start.isValid() && end.isValid() && !end.isAfter(start)) {
      nextErrors.endDate = "End date must be after start date.";
    }

    if (form.mode === "highlight") {
      const highlight = parseNumberOrNull(form.highlightPercentage);
      if (highlight === null) {
        nextErrors.highlightPercentage = "Enter a highlight percentage.";
      } else if (highlight <= 0 || highlight > 90) {
        nextErrors.highlightPercentage = "Highlight must be between 1 and 90.";
      }
    }

    setErrors(nextErrors);
    return nextErrors;
  };

  const buildPayload = () => {
    const payload = {
      name: form.name.trim(),
      description: form.description.trim() ? form.description.trim() : null,
      mode: mapModeToEnum(form.mode),
      startDate: dayjs(form.startDate).utc().toISOString(),
      endDate: dayjs(form.endDate).utc().toISOString(),
      offerFreeShipping: Boolean(form.offerFreeShipping),
      includeSkippedItems: Boolean(form.includeSkippedItems),
      blockPriceIncreaseRevisions: Boolean(form.blockPriceIncreaseRevisions),
      highlightPercentage: form.mode === "highlight" ? parseNumberOrNull(form.highlightPercentage) : null
    };

    return payload;
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    
    if (!canEditBasicInfo) {
      Notice({ msg: "Cannot edit sale event after it has started.", isSuccess: false });
      return;
    }

    const validation = validate();
    if (Object.keys(validation).length > 0) {
      Notice({ msg: "Review the highlighted fields.", isSuccess: false });
      return;
    }

    const payload = buildPayload();
    setSubmitting(true);
    try {
      await SaleEventService.updateSaleEvent(id, payload);
      Notice({ msg: "Sale event updated successfully.", isSuccess: true });
      navigate("/marketing/sale-events");
    } catch (error) {
      Notice({
        msg: "Failed to update sale event.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    } finally {
      setSubmitting(false);
    }
  };

  const handleSearchSubmit = (event) => {
    event.preventDefault();
    const sanitized = listingSearchTerm.trim();
    if (sanitized === currentSearch) {
      fetchListings(1, sanitized);
    } else {
      setCurrentSearch(sanitized);
    }
  };

  const handlePageChange = (direction) => {
    const nextPage = listingPage.pageNumber + direction;
    if (nextPage < 1) {
      return;
    }

    const maxPage = Math.max(1, Math.ceil(listingPage.totalCount / listingPage.pageSize));
    if (nextPage > maxPage) {
      return;
    }

    fetchListings(nextPage, currentSearch);
  };

  const handleAssignListing = async (listingId) => {
    if (!activeTier) {
      Notice({ msg: "Select a discount tier first.", isSuccess: false });
      return;
    }

    try {
      await SaleEventService.assignListingsToTier(id, {
        tierId: activeTier.id,
        listingIds: [listingId]
      });
      Notice({ msg: "Listing assigned successfully.", isSuccess: true });
      await loadSaleEvent();
      fetchListings(listingPage.pageNumber, currentSearch);
    } catch (error) {
      Notice({
        msg: "Failed to assign listing.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    }
  };

  const handleRemoveListing = async (listingId) => {
    try {
      await SaleEventService.removeListingAssignment(id, listingId);
      Notice({ msg: "Listing removed successfully.", isSuccess: true });
      await loadSaleEvent();
      fetchListings(listingPage.pageNumber, currentSearch);
    } catch (error) {
      Notice({
        msg: "Failed to remove listing.",
        desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
        isSuccess: false
      });
    }
  };

  if (loading) {
    return (
      <div className="sale-event-create">
        <div className="sale-event-create__loading">Loading sale event...</div>
      </div>
    );
  }

  if (!form) {
    return null;
  }

  return (
    <form className="sale-event-create" onSubmit={handleSubmit} noValidate>
      <header className="sale-event-create__header">
        <div>
          <h1>Edit sale event</h1>
          <p>Update sale event details and manage tier assignments.</p>
          {!canEditBasicInfo && (
            <p className="sale-event-create__warning">
              Basic information cannot be edited after the sale has started. You can still manage tier assignments.
            </p>
          )}
        </div>
        <div className="sale-event-create__header-actions">
          <button type="button" className="btn-secondary" onClick={() => navigate("/marketing/sale-events")} disabled={submitting}>
            Cancel
          </button>
          {canEditBasicInfo && (
            <button type="submit" className="btn-primary" disabled={submitting}>
              {submitting ? "Saving…" : "Save changes"}
            </button>
          )}
        </div>
      </header>

      <section className="sale-event-card">
        <h2>Event basics</h2>
        <div className="sale-event-grid">
          <label className="sale-event-field">
            <span>Name</span>
            <input
              type="text"
              value={form.name}
              onChange={(event) => updateFormField("name", event.target.value)}
              placeholder="Back-to-school markdown"
              maxLength={90}
              disabled={!canEditBasicInfo}
              required
            />
            {errors.name ? <div className="sale-event-create__error">{errors.name}</div> : null}
          </label>
          <label className="sale-event-field">
            <span>Description</span>
            <textarea
              value={form.description}
              onChange={(event) => updateFormField("description", event.target.value)}
              placeholder="Optional description buyers can see."
              rows={3}
              disabled={!canEditBasicInfo}
            />
          </label>
          <label className="sale-event-field">
            <span>Start date</span>
            <input
              type="datetime-local"
              value={form.startDate}
              onChange={(event) => updateFormField("startDate", event.target.value)}
              disabled={!canEditBasicInfo}
              required
            />
            {errors.startDate ? <div className="sale-event-create__error">{errors.startDate}</div> : null}
          </label>
          <label className="sale-event-field">
            <span>End date</span>
            <input
              type="datetime-local"
              value={form.endDate}
              onChange={(event) => updateFormField("endDate", event.target.value)}
              disabled={!canEditBasicInfo}
              required
            />
            {errors.endDate ? <div className="sale-event-create__error">{errors.endDate}</div> : null}
          </label>
        </div>

        <div className="sale-event-create__mode">
          <span>Mode</span>
          <div className="sale-event-create__mode-options">
            <label className={form.mode === "discount" ? "is-selected" : ""}>
              <input
                type="radio"
                name="mode"
                value="discount"
                checked={form.mode === "discount"}
                disabled={!canEditBasicInfo}
                readOnly
              />
              <div>
                <strong>Discount tiers</strong>
                <p>Define markdown tiers and choose listings for each discount.</p>
              </div>
            </label>
            <label className={form.mode === "highlight" ? "is-selected" : ""}>
              <input
                type="radio"
                name="mode"
                value="highlight"
                checked={form.mode === "highlight"}
                disabled={!canEditBasicInfo}
                readOnly
              />
              <div>
                <strong>Highlight only</strong>
                <p>Boost visibility with strike-through pricing, no tiered discounts.</p>
              </div>
            </label>
          </div>
        </div>

        <div className="sale-event-create__toggles">
          <label>
            <input
              type="checkbox"
              checked={form.offerFreeShipping}
              onChange={(event) => updateFormField("offerFreeShipping", event.target.checked)}
              disabled={!canEditBasicInfo}
            />
            <span>Offer free shipping during sale</span>
          </label>
          <label>
            <input
              type="checkbox"
              checked={form.includeSkippedItems}
              onChange={(event) => updateFormField("includeSkippedItems", event.target.checked)}
              disabled={!canEditBasicInfo}
            />
            <span>Include listings previously skipped</span>
          </label>
          <label>
            <input
              type="checkbox"
              checked={form.blockPriceIncreaseRevisions}
              onChange={(event) => updateFormField("blockPriceIncreaseRevisions", event.target.checked)}
              disabled={!canEditBasicInfo}
            />
            <span>Block price increases while sale runs</span>
          </label>
        </div>

        {form.mode === "highlight" ? (
          <label className="sale-event-field sale-event-field--narrow">
            <span>Highlight percentage</span>
            <input
              type="number"
              min={1}
              max={90}
              value={form.highlightPercentage}
              onChange={(event) => updateFormField("highlightPercentage", event.target.value)}
              placeholder="e.g. 10"
              disabled={!canEditBasicInfo}
              required
            />
            {errors.highlightPercentage ? (
              <div className="sale-event-create__error">{errors.highlightPercentage}</div>
            ) : null}
          </label>
        ) : null}
      </section>

      {form.mode === "discount" ? (
        <>
          <section className="sale-event-card">
            <div className="sale-event-card__header">
              <h2>Discount tiers</h2>
              <p>Tier assignments can be edited at any time.</p>
            </div>
            <div className="sale-event-create__tiers">
              {tiers.map((tier, index) => (
                <div
                  key={tier.id}
                  className={`sale-event-create__tier ${activeTierId === tier.id ? "is-active" : ""}`}
                  onClick={() => setActiveTierId(tier.id)}
                  role="button"
                  tabIndex={0}
                  onKeyPress={() => setActiveTierId(tier.id)}
                >
                  <div className="sale-event-create__tier-head">
                    <h3>Tier {index + 1}</h3>
                  </div>
                  <div className="sale-event-grid sale-event-grid--tier">
                    <div className="sale-event-field">
                      <span>Priority</span>
                      <div>{tier.priority}</div>
                    </div>
                    <div className="sale-event-field">
                      <span>Discount type</span>
                      <div>{tier.discountType === "Percent" ? "Percent off" : "Amount off"}</div>
                    </div>
                    <div className="sale-event-field">
                      <span>Discount value</span>
                      <div>{tier.discountValue}{tier.discountType === "Percent" ? "%" : ""}</div>
                    </div>
                    <div className="sale-event-field">
                      <span>Buyer message label</span>
                      <div>{tier.label || "--"}</div>
                    </div>
                  </div>
                  <div className="sale-event-create__tier-footer">
                    <span>{tier.listingCount} listing{tier.listingCount === 1 ? "" : "s"} assigned</span>
                  </div>
                </div>
              ))}
            </div>
          </section>

          <section className="sale-event-card">
            <div className="sale-event-card__header">
              <h2>Manage listings</h2>
              <div className="sale-event-card__subhead">
                {activeTier ? (
                  <span>Editing tier priority {activeTier.priority}</span>
                ) : (
                  <span>Select a tier card to manage listings.</span>
                )}
              </div>
            </div>

            <form className="sale-event-create__search" onSubmit={handleSearchSubmit}>
              <input
                type="text"
                value={listingSearchTerm}
                onChange={(event) => setListingSearchTerm(event.target.value)}
                placeholder="Search by title, SKU, or category"
              />
              <button type="submit" className="btn-secondary" disabled={loadingListings}>
                {loadingListings ? "Searching…" : "Search"}
              </button>
            </form>

            <div className="sale-event-create__listings">
              {loadingListings ? (
                <div className="sale-event-create__loading">Loading listings…</div>
              ) : listingPage.items.length === 0 ? (
                <div className="sale-event-create__empty">No eligible listings found.</div>
              ) : (
                <table>
                  <thead>
                    <tr>
                      <th>Action</th>
                      <th>Listing</th>
                      <th>Price</th>
                      <th>Format</th>
                      <th>Created</th>
                    </tr>
                  </thead>
                  <tbody>
                    {listingPage.items.map((listing) => {
                      const isAssigned = Boolean(listing.isAlreadyAssigned);
                      return (
                        <tr key={listing.listingId}>
                          <td>
                            {isAssigned ? (
                              <button
                                type="button"
                                className="btn-link"
                                onClick={() => handleRemoveListing(listing.listingId)}
                              >
                                Remove
                              </button>
                            ) : (
                              <button
                                type="button"
                                className="btn-link"
                                onClick={() => handleAssignListing(listing.listingId)}
                                disabled={!activeTier}
                              >
                                Assign
                              </button>
                            )}
                          </td>
                          <td>
                            <div className="sale-event-create__listing-info">
                              {listing.thumbnailUrl ? (
                                <img src={listing.thumbnailUrl} alt="thumbnail" />
                              ) : null}
                              <div>
                                <div className="sale-event-create__listing-title">{listing.title}</div>
                                {listing.sku ? (
                                  <div className="sale-event-create__listing-meta">SKU: {listing.sku}</div>
                                ) : null}
                                {isAssigned ? (
                                  <div className="sale-event-create__listing-warning">Assigned to this event.</div>
                                ) : null}
                              </div>
                            </div>
                          </td>
                          <td>{formatCurrency(listing.basePrice)}</td>
                          <td>{formatListingFormat(listing.format)}</td>
                          <td>{listing.createdAt ? dayjs(listing.createdAt).format("DD MMM YYYY") : "--"}</td>
                        </tr>
                      );
                    })}
                  </tbody>
                </table>
              )}
            </div>

            {listingPage.totalCount > listingPage.pageSize ? (
              <div className="sale-event-create__pagination">
                <button
                  type="button"
                  className="btn-secondary"
                  onClick={() => handlePageChange(-1)}
                  disabled={loadingListings || listingPage.pageNumber === 1}
                >
                  Previous
                </button>
                <span>
                  Page {listingPage.pageNumber} of {Math.max(1, Math.ceil(listingPage.totalCount / listingPage.pageSize))}
                </span>
                <button
                  type="button"
                  className="btn-secondary"
                  onClick={() => handlePageChange(1)}
                  disabled={loadingListings || listingPage.pageNumber >= Math.ceil(listingPage.totalCount / listingPage.pageSize)}
                >
                  Next
                </button>
              </div>
            ) : null}
          </section>
        </>
      ) : null}
    </form>
  );
};

export default EditSaleEvent;
