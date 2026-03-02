import React, { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import utc from "dayjs/plugin/utc";
import Notice from "../../../components/Common/CustomNotification";
import SaleEventService, { SaleEventDiscountType, SaleEventMode } from "../../../services/SaleEventService";
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

const createInitialForm = () => {
  const start = dayjs().add(30, "minute");
  const end = start.add(7, "day");

  return {
    name: "",
    description: "",
    mode: "discount",
    startDate: start.format(DATETIME_FORMAT),
    endDate: end.format(DATETIME_FORMAT),
    offerFreeShipping: false,
    includeSkippedItems: true,
    blockPriceIncreaseRevisions: true,
    highlightPercentage: ""
  };
};

const createBlankTier = (priority = 1, listingIds = []) => ({
  clientId: typeof crypto !== "undefined" && crypto.randomUUID ? crypto.randomUUID() : `tier-${Date.now()}-${Math.random()}`,
  priority: String(priority),
  discountType: "Percent",
  discountValue: "",
  label: "",
  listingIds: Array.isArray(listingIds) ? Array.from(new Set(listingIds.map((id) => (typeof id === "string" ? id : String(id ?? "")).trim()).filter(Boolean))) : []
});

const mapModeToEnum = (mode) => (mode === "highlight" ? SaleEventMode.SaleEventOnly : SaleEventMode.DiscountAndSaleEvent);

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

const CreateSaleEvent = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const initialListingIdsRef = useRef(null);
  if (initialListingIdsRef.current === null) {
    const raw = location?.state?.listingIds;
    if (Array.isArray(raw)) {
      const normalized = raw
        .map((id) => {
          if (typeof id === "string") {
            return id.trim();
          }
          if (typeof id === "number" || typeof id === "bigint") {
            return id.toString();
          }
          if (id && typeof id === "object" && typeof id.toString === "function") {
            const text = id.toString();
            return text !== "[object Object]" ? text : "";
          }
          return "";
        })
        .filter(Boolean);
      initialListingIdsRef.current = Array.from(new Set(normalized));
    } else {
      initialListingIdsRef.current = [];
    }
  }
  const initialListingIds = initialListingIdsRef.current;

  const [form, setForm] = useState(() => createInitialForm());
  const [tiers, setTiers] = useState(() => [createBlankTier(1, initialListingIds)]);
  const [activeTierId, setActiveTierId] = useState(() => (tiers[0] ? tiers[0].clientId : null));
  const [errors, setErrors] = useState({});
  const [submitting, setSubmitting] = useState(false);
  const [listingSearchTerm, setListingSearchTerm] = useState("");
  const [currentSearch, setCurrentSearch] = useState("");
  const [listingPage, setListingPage] = useState(emptyListingPage);
  const [loadingListings, setLoadingListings] = useState(false);

  useEffect(() => {
    if (initialListingIds.length > 0) {
      Notice({
        msg: `${initialListingIds.length} listing${initialListingIds.length === 1 ? "" : "s"} added from Listings.`,
        isSuccess: true
      });
    }
  }, [initialListingIds.length]);

  const activeTier = useMemo(() => tiers.find((tier) => tier.clientId === activeTierId) ?? null, [tiers, activeTierId]);

  const totalAssignedListings = useMemo(() => {
    const set = new Set();
    tiers.forEach((tier) => {
      tier.listingIds.forEach((id) => set.add(id));
    });
    return set.size;
  }, [tiers]);

  const ensureActiveTier = useCallback(() => {
    if (form.mode !== "discount") {
      setActiveTierId(null);
      return;
    }

    setTiers((prev) => {
      if (prev.length === 0) {
        const next = [createBlankTier(1)];
        setActiveTierId(next[0].clientId);
        return next;
      }

      if (!prev.some((tier) => tier.clientId === activeTierId)) {
        setActiveTierId(prev[0].clientId);
      }

      return prev;
    });
  }, [activeTierId, form.mode]);

  useEffect(() => {
    ensureActiveTier();
  }, [ensureActiveTier]);

  const fetchListings = useCallback(async (pageNumber = 1, searchTerm = "") => {
    if (form.mode !== "discount") {
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
        excludeAlreadyAssigned: true
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
  }, [form.mode]);

  useEffect(() => {
    if (form.mode !== "discount") {
      setListingPage(emptyListingPage);
      return;
    }

    fetchListings(1, currentSearch);
  }, [form.mode, currentSearch, fetchListings]);

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

  const updateTierField = (clientId, key, value) => {
    setTiers((prev) => prev.map((tier) => (tier.clientId === clientId ? { ...tier, [key]: value } : tier)));
    const errorKey = `tier.${clientId}.${key}`;
    setErrors((prev) => {
      if (!prev[errorKey]) {
        return prev;
      }
      const next = { ...prev };
      delete next[errorKey];
      return next;
    });
  };

  const addTier = () => {
    setTiers((prev) => {
      const nextPriority = prev.length > 0 ? Math.max(...prev.map((tier) => Number(tier.priority) || 0)) + 1 : 1;
      const nextTier = createBlankTier(nextPriority);
      const next = [...prev, nextTier];
      setActiveTierId(nextTier.clientId);
      return next;
    });
  };

  const removeTier = (clientId) => {
    setTiers((prev) => {
      const filtered = prev.filter((tier) => tier.clientId !== clientId);
      if (clientId === activeTierId && filtered.length > 0) {
        setActiveTierId(filtered[0].clientId);
      } else if (filtered.length === 0) {
        setActiveTierId(null);
      }
      return filtered;
    });
  };

  const toggleListingSelection = (listingId) => {
    if (!activeTier) {
      Notice({ msg: "Select a discount tier first.", isSuccess: false });
      return;
    }

    setTiers((prev) => prev.map((tier) => {
      const set = new Set(tier.listingIds);
      if (tier.clientId === activeTier.clientId) {
        if (set.has(listingId)) {
          set.delete(listingId);
        } else {
          set.add(listingId);
        }
      } else {
        set.delete(listingId);
      }

      return {
        ...tier,
        listingIds: Array.from(set)
      };
    }));
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

    if (form.mode === "discount") {
      if (tiers.length === 0) {
        nextErrors.tiers = "Add at least one discount tier.";
      }

      const priorities = new Set();
      tiers.forEach((tier) => {
        const priorityKey = `tier.${tier.clientId}.priority`;
        const discountValueKey = `tier.${tier.clientId}.discountValue`;
        const listingKey = `tier.${tier.clientId}.listings`;

        const priority = Number(tier.priority);
        if (!Number.isFinite(priority) || priority <= 0) {
          nextErrors[priorityKey] = "Priority must be a positive number.";
        } else if (priorities.has(priority)) {
          nextErrors[priorityKey] = "Priority must be unique.";
        } else {
          priorities.add(priority);
        }

        const discountValue = Number(tier.discountValue);
        if (!(discountValue > 0)) {
          nextErrors[discountValueKey] = "Enter a discount value.";
        } else if (tier.discountType === "Percent" && discountValue > 90) {
          nextErrors[discountValueKey] = "Percent discount must be 90 or below.";
        }

        if (!Array.isArray(tier.listingIds) || tier.listingIds.length === 0) {
          nextErrors[listingKey] = "Assign at least one listing.";
        }
      });
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
      highlightPercentage: form.mode === "highlight" ? parseNumberOrNull(form.highlightPercentage) : null,
      tiers: null
    };

    if (payload.mode === SaleEventMode.DiscountAndSaleEvent) {
      payload.highlightPercentage = null;
      payload.tiers = tiers.map((tier) => ({
        discountType: mapDiscountTypeToEnum(tier.discountType),
        discountValue: Number(tier.discountValue),
        priority: Number(tier.priority),
        label: tier.label.trim() ? tier.label.trim() : null,
        listingIds: tier.listingIds
      }));
    }

    return payload;
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const validation = validate();
    if (Object.keys(validation).length > 0) {
      Notice({ msg: "Review the highlighted fields.", isSuccess: false });
      return;
    }

    const payload = buildPayload();
    setSubmitting(true);
    try {
      const result = await SaleEventService.createSaleEvent(payload);
      Notice({ msg: "Sale event created successfully.", isSuccess: true });
      if (result?.id) {
        navigate(`/marketing/sale-events?created=${result.id}`);
      } else {
        navigate("/marketing/sale-events");
      }
    } catch (error) {
      Notice({
        msg: "Failed to create sale event.",
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

  const renderTierError = (tier, key) => {
    const errorKey = `tier.${tier.clientId}.${key}`;
    return errors[errorKey] ? <div className="sale-event-create__error">{errors[errorKey]}</div> : null;
  };

  return (
    <form className="sale-event-create" onSubmit={handleSubmit} noValidate>
      <header className="sale-event-create__header">
        <div>
          <h1>Create sale event</h1>
          <p>Build an eBay-style markdown event with discount tiers and curated listings.</p>
        </div>
        <div className="sale-event-create__header-actions">
          <button type="button" className="btn-secondary" onClick={() => navigate("/marketing/sale-events")} disabled={submitting}>
            Cancel
          </button>
          <button type="submit" className="btn-primary" disabled={submitting}>
            {submitting ? "Saving…" : "Launch sale event"}
          </button>
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
            />
          </label>
          <label className="sale-event-field">
            <span>Start date</span>
            <input
              type="datetime-local"
              value={form.startDate}
              onChange={(event) => updateFormField("startDate", event.target.value)}
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
                onChange={() => updateFormField("mode", "discount")}
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
                onChange={() => {
                  updateFormField("mode", "highlight");
                  setTiers([]);
                  setActiveTierId(null);
                  setListingSearchTerm("");
                  setCurrentSearch("");
                  setListingPage(emptyListingPage);
                }}
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
            />
            <span>Offer free shipping during sale</span>
          </label>
          <label>
            <input
              type="checkbox"
              checked={form.includeSkippedItems}
              onChange={(event) => updateFormField("includeSkippedItems", event.target.checked)}
            />
            <span>Include listings previously skipped</span>
          </label>
          <label>
            <input
              type="checkbox"
              checked={form.blockPriceIncreaseRevisions}
              onChange={(event) => updateFormField("blockPriceIncreaseRevisions", event.target.checked)}
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
              required
            />
            {errors.highlightPercentage ? (
              <div className="sale-event-create__error">{errors.highlightPercentage}</div>
            ) : null}
          </label>
        ) : null}
      </section>

      {form.mode === "discount" ? (
        <section className="sale-event-card">
          <div className="sale-event-card__header">
            <h2>Discount tiers</h2>
            <button type="button" className="btn-secondary" onClick={addTier}>
              Add tier
            </button>
          </div>
          {errors.tiers ? <div className="sale-event-create__error">{errors.tiers}</div> : null}
          <div className="sale-event-create__tiers">
            {tiers.map((tier, index) => (
              <div
                key={tier.clientId}
                className={`sale-event-create__tier ${activeTierId === tier.clientId ? "is-active" : ""}`}
                onClick={() => setActiveTierId(tier.clientId)}
                role="button"
                tabIndex={0}
                onKeyPress={() => setActiveTierId(tier.clientId)}
              >
                <div className="sale-event-create__tier-head">
                  <h3>Tier {index + 1}</h3>
                  <button
                    type="button"
                    className="btn-link"
                    onClick={(event) => {
                      event.stopPropagation();
                      removeTier(tier.clientId);
                    }}
                  >
                    Remove
                  </button>
                </div>
                <div className="sale-event-grid sale-event-grid--tier">
                  <label className="sale-event-field">
                    <span>Priority</span>
                    <input
                      type="number"
                      min={1}
                      value={tier.priority}
                      onChange={(event) => updateTierField(tier.clientId, "priority", event.target.value)}
                      required
                    />
                    {renderTierError(tier, "priority")}
                  </label>
                  <label className="sale-event-field">
                    <span>Discount type</span>
                    <select
                      value={tier.discountType}
                      onChange={(event) => updateTierField(tier.clientId, "discountType", event.target.value)}
                    >
                      <option value="Percent">Percent off</option>
                      <option value="Amount">Amount off</option>
                    </select>
                  </label>
                  <label className="sale-event-field">
                    <span>Discount value</span>
                    <input
                      type="number"
                      min={0}
                      step="0.01"
                      value={tier.discountValue}
                      onChange={(event) => updateTierField(tier.clientId, "discountValue", event.target.value)}
                      required
                    />
                    {renderTierError(tier, "discountValue")}
                  </label>
                  <label className="sale-event-field">
                    <span>Buyer message label</span>
                    <input
                      type="text"
                      value={tier.label}
                      onChange={(event) => updateTierField(tier.clientId, "label", event.target.value)}
                      placeholder="Optional badge text"
                    />
                  </label>
                </div>
                <div className="sale-event-create__tier-footer">
                  <span>{tier.listingIds.length} listing{tier.listingIds.length === 1 ? "" : "s"} assigned</span>
                  {renderTierError(tier, "listings")}
                </div>
              </div>
            ))}
          </div>
        </section>
      ) : null}

      {form.mode === "discount" ? (
        <section className="sale-event-card">
          <div className="sale-event-card__header">
            <h2>Select listings</h2>
            <div className="sale-event-card__subhead">
              <span>{totalAssignedListings} unique listing{totalAssignedListings === 1 ? "" : "s"} selected</span>
              {activeTier ? (
                <span>Editing tier priority {activeTier.priority}</span>
              ) : (
                <span>Select a tier card to assign listings.</span>
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
                    <th>Assign</th>
                    <th>Listing</th>
                    <th>Price</th>
                    <th>Format</th>
                    <th>Created</th>
                  </tr>
                </thead>
                <tbody>
                  {listingPage.items.map((listing) => {
                    const isSelected = Boolean(activeTier && activeTier.listingIds.includes(listing.listingId));
                    return (
                      <tr key={listing.listingId}>
                        <td>
                          <input
                            type="checkbox"
                            checked={isSelected}
                            disabled={!activeTier || listing.isAlreadyAssigned}
                            onChange={() => toggleListingSelection(listing.listingId)}
                          />
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
                              {listing.isAlreadyAssigned ? (
                                <div className="sale-event-create__listing-warning">Already assigned to another event.</div>
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
      ) : null}
    </form>
  );
};

export default CreateSaleEvent;
