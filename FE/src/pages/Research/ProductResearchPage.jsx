import React, { useCallback, useEffect, useMemo, useRef, useState } from "react";
import "./ProductResearchPage.scss";
import ResearchService from "../../services/ResearchService";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import CategoryModal from "../Listing/Modal/CategoryModal";

const RANGE_OPTIONS = [
  { value: 7, label: "Last 7 days" },
  { value: 14, label: "Last 14 days" },
  { value: 30, label: "Last 30 days" },
  { value: 90, label: "Last 90 days" }
];

const DEFAULT_KEYWORD = "";
const DEFAULT_RANGE = 30;
const DEFAULT_PAGE = 1;
const DEFAULT_PAGE_SIZE = 20;
const MAX_PAGE_SIZE = 100;
const PAGE_SIZE_OPTIONS = [10, 20, 50];

const CONDITION_OPTIONS = [
  { value: "new", label: "New" },
  { value: "new-other", label: "New other (see details)" },
  { value: "new-defects", label: "New with defects" },
  { value: "seller-refurbished", label: "Seller refurbished" },
  { value: "like-new", label: "Like New" },
  { value: "used", label: "Used" },
  { value: "very-good", label: "Very Good" },
  { value: "good", label: "Good" },
  { value: "acceptable", label: "Acceptable" },
  { value: "parts", label: "For parts or not working" }
];

const FORMAT_FILTER_OPTIONS = [
  { value: "auction", label: "Auction" },
  { value: "best-offer", label: "Best Offer accepted" },
  { value: "fixed", label: "Fixed price" }
];

const DEFAULT_FILTERS = {
  conditions: [],
  formats: [],
  minPrice: "",
  maxPrice: "",
  topRatedOnly: false,
  categoryId: null,
  categoryPath: []
};

const DEFAULT_CRITERIA = {
  keyword: DEFAULT_KEYWORD,
  days: DEFAULT_RANGE,
  format: undefined,
  minPrice: undefined,
  maxPrice: undefined,
  freeShippingOnly: false,
  categoryId: undefined,
  page: DEFAULT_PAGE,
  pageSize: DEFAULT_PAGE_SIZE
};

const formatMoney = (money) => {
  if (!money || typeof money.amount !== "number") {
    return "-";
  }

  const currency = money.currency || "USD";
  try {
    return new Intl.NumberFormat("en-US", { style: "currency", currency }).format(money.amount);
  } catch (error) {
    console.warn("Unable to format currency", error);
    return `${money.amount.toFixed(2)} ${currency}`;
  }
};

const formatShipping = (money) => {
  if (!money || typeof money.amount !== "number") {
    return "-";
  }

  if (money.amount === 0) {
    return "Free shipping";
  }

  return `+${formatMoney(money)}`;
};

const formatNumber = (value) => {
  if (value === null || value === undefined) {
    return "-";
  }

  return new Intl.NumberFormat("en-US").format(value);
};

const toDate = (value) => {
  if (!value) {
    return null;
  }

  if (typeof value === "string" && /^\d{4}-\d{2}-\d{2}$/.test(value)) {
    const [year, month, day] = value.split("-").map(Number);
    return new Date(Date.UTC(year, month - 1, day));
  }

  const parsed = new Date(value);
  return Number.isNaN(parsed.getTime()) ? null : parsed;
};

const formatDateLabel = (value) => {
  const date = toDate(value);
  if (!date) {
    return "-";
  }

  return new Intl.DateTimeFormat("en-US", {
    month: "short",
    day: "numeric",
    year: "numeric"
  }).format(date);
};

const parsePriceInput = (value) => {
  if (value === "" || value === null || value === undefined) {
    return undefined;
  }

  const numeric = Number(value);
  if (!Number.isFinite(numeric) || numeric < 0) {
    return undefined;
  }

  return Math.round(numeric * 100) / 100;
};

const formatFilterCurrency = (value) => {
  if (value === undefined || value === null) {
    return "";
  }

  return new Intl.NumberFormat("en-US", {
    style: "currency",
    currency: "USD",
    minimumFractionDigits: 0,
    maximumFractionDigits: 2
  }).format(value);
};

const cloneFilterState = (source) => ({
  conditions: [...(source?.conditions ?? [])],
  formats: [...(source?.formats ?? [])],
  minPrice: source?.minPrice ?? "",
  maxPrice: source?.maxPrice ?? "",
  topRatedOnly: Boolean(source?.topRatedOnly),
  categoryId: source?.categoryId ?? null,
  categoryPath: Array.isArray(source?.categoryPath)
    ? source.categoryPath
      .filter((node) => node && typeof node.id === "string" && node.name)
      .map((node) => ({ id: node.id, name: node.name }))
    : []
});

const sanitizeFilterState = (source) => {
  const next = cloneFilterState(source ?? DEFAULT_FILTERS);
  next.conditions = Array.from(new Set(next.conditions.filter(Boolean)));
  next.formats = Array.from(new Set(next.formats.filter(Boolean)));

  const parsedMin = parsePriceInput(next.minPrice);
  const parsedMax = parsePriceInput(next.maxPrice);

  next.minPrice = parsedMin === undefined ? "" : String(parsedMin);
  next.maxPrice = parsedMax === undefined ? "" : String(parsedMax);

  if (parsedMin !== undefined && parsedMax !== undefined && parsedMin > parsedMax) {
    next.minPrice = String(parsedMax);
    next.maxPrice = String(parsedMin);
  }

  if (typeof next.categoryId === "string") {
    const trimmed = next.categoryId.trim();
    next.categoryId = trimmed.length > 0 ? trimmed : null;
  } else {
    next.categoryId = null;
  }

  if (!next.categoryId) {
    next.categoryPath = [];
  } else {
    next.categoryPath = Array.isArray(next.categoryPath)
      ? next.categoryPath
        .filter((node) => node && typeof node.id === "string" && node.name)
        .map((node) => ({ id: node.id, name: node.name }))
      : [];
  }

  return next;
};

const ProductResearchPage = () => {
  const [searchTerm, setSearchTerm] = useState(DEFAULT_KEYWORD);
  const [timeRange, setTimeRange] = useState(DEFAULT_RANGE.toString());
  const [filters, setFilters] = useState(() => cloneFilterState(DEFAULT_FILTERS));
  const [draftFilters, setDraftFilters] = useState(() => cloneFilterState(DEFAULT_FILTERS));
  const [criteria, setCriteria] = useState(() => ({ ...DEFAULT_CRITERIA }));
  const [activeTab, setActiveTab] = useState("sold");
  const [showTrends, setShowTrends] = useState(true);
  const [data, setData] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [openFilter, setOpenFilter] = useState(null);
  const [lockFilters, setLockFilters] = useState(false);
  const [isCategoryModalOpen, setIsCategoryModalOpen] = useState(false);
  const filterSectionRef = useRef(null);

  useEffect(() => {
    if (!openFilter) {
      return undefined;
    }

    const handleClickOutside = (event) => {
      if (!filterSectionRef.current?.contains(event.target)) {
        setOpenFilter(null);
        setDraftFilters(cloneFilterState(filters));
      }
    };

    const handleEscape = (event) => {
      if (event.key === "Escape") {
        setOpenFilter(null);
        setDraftFilters(cloneFilterState(filters));
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    document.addEventListener("keyup", handleEscape);

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
      document.removeEventListener("keyup", handleEscape);
    };
  }, [openFilter, filters]);

  useEffect(() => {
    if (lockFilters && openFilter) {
      setOpenFilter(null);
      setDraftFilters(cloneFilterState(filters));
    }
  }, [filters, lockFilters, openFilter]);

  useEffect(() => {
    if (lockFilters) {
      setIsCategoryModalOpen(false);
    }
  }, [lockFilters]);

  const buildCriteria = useCallback(
    (activeFilters, overrides = {}) => {
      const keywordSource = typeof overrides.keyword === "string" ? overrides.keyword : searchTerm;
      const trimmedKeyword = keywordSource.trim();

      const resolvedDays = (() => {
        if (overrides.days !== undefined) {
          const explicit = Number(overrides.days);
          return Number.isNaN(explicit) ? DEFAULT_RANGE : explicit;
        }

        const inferred = Number(timeRange);
        return Number.isNaN(inferred) ? DEFAULT_RANGE : inferred;
      })();

      let minPrice = parsePriceInput(activeFilters.minPrice);
      let maxPrice = parsePriceInput(activeFilters.maxPrice);

      if (minPrice !== undefined && maxPrice !== undefined && minPrice > maxPrice) {
        [minPrice, maxPrice] = [maxPrice, minPrice];
      }

      const hasAuction = activeFilters.formats.includes("auction");
      const hasFixed = activeFilters.formats.includes("fixed");

      const format = (() => {
        if (hasAuction && !hasFixed) {
          return "auction";
        }

        if (hasFixed && !hasAuction) {
          return "fixed";
        }

        return undefined;
      })();

      const rawPageSize = overrides.pageSize ?? DEFAULT_PAGE_SIZE;
      const parsedPageSize = Number(rawPageSize);
      const sanitizedPageSize = Number.isFinite(parsedPageSize) && parsedPageSize > 0
        ? Math.min(Math.round(parsedPageSize), MAX_PAGE_SIZE)
        : DEFAULT_PAGE_SIZE;

      const rawPage = overrides.page ?? DEFAULT_PAGE;
      const parsedPage = Number(rawPage);
      const sanitizedPage = Number.isFinite(parsedPage) && parsedPage > 0
        ? Math.floor(parsedPage)
        : DEFAULT_PAGE;

      return {
        keyword: trimmedKeyword,
        days: resolvedDays,
        format,
        minPrice,
        maxPrice,
        freeShippingOnly: false,
        categoryId: activeFilters.categoryId ?? undefined,
        page: sanitizedPage,
        pageSize: sanitizedPageSize
      };
    },
    [searchTerm, timeRange]
  );

  const updateFiltersAndCriteria = useCallback(
    (nextFilters, overrides = {}) => {
      const normalized = sanitizeFilterState(nextFilters);
      setFilters(normalized);
      setDraftFilters(cloneFilterState(normalized));
      setCriteria((previous) => buildCriteria(normalized, {
        ...overrides,
        keyword: typeof overrides.keyword === "string"
          ? overrides.keyword
          : previous?.keyword ?? DEFAULT_KEYWORD,
        days: overrides.days ?? previous?.days ?? DEFAULT_RANGE,
        pageSize: overrides.pageSize ?? previous?.pageSize ?? DEFAULT_PAGE_SIZE
      }));
    },
    [buildCriteria]
  );

  const handleToggleDropdown = useCallback(
    (name) => {
      if (lockFilters) {
        return;
      }

      if (openFilter === name) {
        setOpenFilter(null);
        setDraftFilters(cloneFilterState(filters));
        return;
      }

      setDraftFilters(cloneFilterState(filters));
      setOpenFilter(name);
    },
    [filters, lockFilters, openFilter]
  );

  const handleConditionToggle = useCallback((value) => {
    setDraftFilters((prev) => {
      const present = prev.conditions.includes(value);
      const nextConditions = present
        ? prev.conditions.filter((condition) => condition !== value)
        : [...prev.conditions, value];

      return {
        ...prev,
        conditions: nextConditions
      };
    });
  }, []);

  const handleFormatToggle = useCallback((value) => {
    setDraftFilters((prev) => {
      const present = prev.formats.includes(value);
      const nextFormats = present
        ? prev.formats.filter((format) => format !== value)
        : [...prev.formats, value];

      return {
        ...prev,
        formats: nextFormats
      };
    });
  }, []);

  const handleDraftPriceChange = useCallback((field, inputValue) => {
    setDraftFilters((prev) => ({
      ...prev,
      [field]: inputValue
    }));
  }, []);

  const handleDraftTopRatedChange = useCallback((checked) => {
    setDraftFilters((prev) => ({
      ...prev,
      topRatedOnly: checked
    }));
  }, []);

  const handleApplyFilters = useCallback(() => {
    updateFiltersAndCriteria(draftFilters);
    setOpenFilter(null);
  }, [draftFilters, updateFiltersAndCriteria]);

  const fetchResearch = useCallback(async (nextCriteria) => {
    setIsLoading(true);
    setError(null);

    try {
      const response = await ResearchService.getProductResearch(nextCriteria);
      setData(response);
    } catch (fetchError) {
      console.error(fetchError);
      setError("We couldn't load research data. Please try again.");
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchResearch(criteria);
  }, [criteria, fetchResearch]);

  const selectedPanel = useMemo(() => {
    if (!data) {
      return null;
    }

    return activeTab === "sold"
      ? data?.sold ?? data?.Sold ?? null
      : data?.active ?? data?.Active ?? null;
  }, [activeTab, data]);

  const summaryMetrics = selectedPanel?.summary ?? selectedPanel?.Summary ?? [];
  const listings = selectedPanel?.listings ?? selectedPanel?.Listings ?? [];
  const pagination = selectedPanel?.pagination ?? selectedPanel?.Pagination ?? null;
  const currentPage = pagination?.page ?? criteria.page ?? DEFAULT_PAGE;
  const rawPageSize = pagination?.pageSize ?? criteria.pageSize ?? DEFAULT_PAGE_SIZE;
  const currentPageSize = rawPageSize > 0 ? rawPageSize : DEFAULT_PAGE_SIZE;
  const totalCount = pagination?.totalCount ?? listings.length;
  const totalPages = totalCount > 0 ? Math.max(1, Math.ceil(totalCount / currentPageSize)) : 1;
  const canGoPrev = currentPage > 1;
  const canGoNext = currentPage < totalPages;
  const paginationTotalCount = totalCount;
  const resolvedPageSizeOptions = useMemo(() => {
    const unique = new Set(PAGE_SIZE_OPTIONS);
    unique.add(currentPageSize);
    return Array.from(unique).sort((a, b) => a - b);
  }, [currentPageSize]);
  const paginationLabel = totalCount > 0
    ? `Page ${Math.min(currentPage, totalPages)} of ${totalPages}`
    : "Page 1 of 1";
  const firstItemIndex = totalCount === 0 ? 0 : (currentPage - 1) * currentPageSize + 1;
  const lastItemIndex = totalCount === 0 ? 0 : Math.min(totalCount, currentPage * currentPageSize);
  const rangeLabel = totalCount === 0
    ? "No results"
    : `Showing ${formatNumber(firstItemIndex)}-${formatNumber(lastItemIndex)} of ${formatNumber(totalCount)}`;
  const prevDisabled = totalCount === 0 || activeTab !== "active" || !canGoPrev;
  const nextDisabled = totalCount === 0 || activeTab !== "active" || !canGoNext;
  const pageSizeSelectDisabled = activeTab !== "active" || totalCount === 0;

  const trendPolyline = useMemo(() => {
    const source = selectedPanel?.trend ?? selectedPanel?.Trend ?? [];
    if (!source.length) {
      return "";
    }

    const values = source.map((entry) => entry.averagePrice ?? entry.AveragePrice ?? 0);
    const max = Math.max(1, ...values);
    const min = Math.min(0, ...values);
    const padding = (max - min) * 0.1;
    const scaledMax = max + padding;
    const scaledMin = Math.max(min - padding, 0);
    const range = Math.max(1, scaledMax - scaledMin);
    const step = source.length > 1 ? 100 / (source.length - 1) : 100;

    return source
      .map((entry, index) => {
        const x = index * step;
        const value = entry.averagePrice ?? entry.AveragePrice ?? 0;
        const y = 100 - ((value - scaledMin) / range) * 100;
        return `${x.toFixed(2)},${y.toFixed(2)}`;
      })
      .join(" ");
  }, [selectedPanel]);

  const dateRangeLabel = useMemo(() => {
    const range = data?.range ?? data?.Range;
    if (!range) {
      return "—";
    }

    const from = formatDateLabel(range.from ?? range.From);
    const to = formatDateLabel(range.to ?? range.To);
    return from && to ? `${from} – ${to}` : "—";
  }, [data]);

  const selectedRangeOption = useMemo(
    () => RANGE_OPTIONS.find((option) => option.value === (criteria.days ?? DEFAULT_RANGE)) ?? RANGE_OPTIONS[2],
    [criteria.days]
  );

  const activeFilterChips = useMemo(() => {
    const chips = [];

    if (filters.categoryId) {
      const pathLabel = Array.isArray(filters.categoryPath) && filters.categoryPath.length > 0
        ? filters.categoryPath.map((node) => node.name).join(" > ")
        : "Selected category";
      chips.push({ type: "category", value: filters.categoryId, label: `Category: ${pathLabel}` });
    }

    filters.conditions.forEach((value) => {
      const option = CONDITION_OPTIONS.find((item) => item.value === value);
      const label = option?.label ?? value;
      chips.push({ type: "condition", value, label: `Condition: ${label}` });
    });

    filters.formats.forEach((value) => {
      const option = FORMAT_FILTER_OPTIONS.find((item) => item.value === value);
      const label = option?.label ?? value;
      chips.push({ type: "format", value, label: `Format: ${label}` });
    });

    const min = parsePriceInput(filters.minPrice);
    if (min !== undefined) {
      chips.push({ type: "minPrice", value: min, label: `Min ${formatFilterCurrency(min)}` });
    }

    const max = parsePriceInput(filters.maxPrice);
    if (max !== undefined) {
      chips.push({ type: "maxPrice", value: max, label: `Max ${formatFilterCurrency(max)}` });
    }

    if (filters.topRatedOnly) {
      chips.push({ type: "topRated", value: true, label: "Top rated sellers" });
    }

    return chips;
  }, [filters]);

  const selectedCategoryLabel = useMemo(() => {
    if (!filters.categoryId || !Array.isArray(filters.categoryPath) || filters.categoryPath.length === 0) {
      return "";
    }

    return filters.categoryPath.map((node) => node.name).join(" > ");
  }, [filters.categoryId, filters.categoryPath]);

  const handleRemoveFilter = useCallback((chip) => {
    const next = cloneFilterState(filters);

    switch (chip.type) {
      case "category":
        next.categoryId = null;
        next.categoryPath = [];
        break;
      case "condition":
        next.conditions = next.conditions.filter((condition) => condition !== chip.value);
        break;
      case "format":
        next.formats = next.formats.filter((format) => format !== chip.value);
        break;
      case "minPrice":
        next.minPrice = "";
        break;
      case "maxPrice":
        next.maxPrice = "";
        break;
      case "topRated":
        next.topRatedOnly = false;
        break;
      default:
        break;
    }

    updateFiltersAndCriteria(next);
  }, [filters, updateFiltersAndCriteria]);

  const handleResearch = useCallback(() => {
    const keyword = searchTerm.trim();
    const parsedDays = Number(timeRange);
    const daysOption = Number.isNaN(parsedDays) ? DEFAULT_RANGE : parsedDays;
    setCriteria((previous) => buildCriteria(filters, {
      keyword,
      days: daysOption,
      pageSize: previous?.pageSize ?? DEFAULT_PAGE_SIZE
    }));
  }, [buildCriteria, filters, searchTerm, timeRange]);

  const handleReset = useCallback(() => {
    setSearchTerm(DEFAULT_KEYWORD);
    setTimeRange(DEFAULT_RANGE.toString());
    setOpenFilter(null);
    setIsCategoryModalOpen(false);
    updateFiltersAndCriteria(DEFAULT_FILTERS, {
      keyword: DEFAULT_KEYWORD,
      days: DEFAULT_RANGE,
      pageSize: DEFAULT_PAGE_SIZE
    });
  }, [updateFiltersAndCriteria]);

  const handleClearFilters = useCallback(() => {
    setOpenFilter(null);
    setIsCategoryModalOpen(false);
    updateFiltersAndCriteria(DEFAULT_FILTERS);
  }, [updateFiltersAndCriteria]);

  const handleOpenCategoryModal = useCallback(() => {
    if (lockFilters) {
      return;
    }

    setIsCategoryModalOpen(true);
  }, [lockFilters]);

  const handleCloseCategoryModal = useCallback(() => {
    setIsCategoryModalOpen(false);
  }, []);

  const handleCategorySelect = useCallback((selection) => {
    if (!selection) {
      return;
    }

    const mappedPath = Array.isArray(selection.path)
      ? selection.path
        .filter((node) => node && typeof node.id === "string" && node.name)
        .map((node) => ({ id: node.id, name: node.name }))
      : [];

    const nextFilters = {
      ...filters,
      categoryId: selection.id ?? null,
      categoryPath: mappedPath
    };

    updateFiltersAndCriteria(nextFilters);
    setIsCategoryModalOpen(false);
  }, [filters, updateFiltersAndCriteria]);

  const handleClearCategory = useCallback(() => {
    if (lockFilters) {
      return;
    }

    updateFiltersAndCriteria({
      ...filters,
      categoryId: null,
      categoryPath: []
    });
  }, [filters, lockFilters, updateFiltersAndCriteria]);

  const handlePrevPage = useCallback(() => {
    if (activeTab !== "active") {
      return;
    }

    if (paginationTotalCount === 0) {
      return;
    }

    setCriteria((previous) => {
      const current = previous?.page ?? DEFAULT_PAGE;
      if (current <= 1) {
        return previous;
      }

      return {
        ...previous,
        page: current - 1
      };
    });
  }, [activeTab, paginationTotalCount]);

  const handleNextPage = useCallback(() => {
    if (activeTab !== "active") {
      return;
    }

    if (paginationTotalCount === 0) {
      return;
    }

    setCriteria((previous) => {
      const current = previous?.page ?? DEFAULT_PAGE;
      const pageSize = previous?.pageSize ?? DEFAULT_PAGE_SIZE;
      const totalPagesForActive = paginationTotalCount > 0
        ? Math.max(1, Math.ceil(paginationTotalCount / pageSize))
        : 1;

      if (current >= totalPagesForActive) {
        return previous;
      }

      return {
        ...previous,
        page: current + 1
      };
    });
  }, [activeTab, paginationTotalCount]);

  const handlePageSizeChange = useCallback((event) => {
    if (activeTab !== "active") {
      return;
    }

    const rawValue = Number(event.target.value);
    if (Number.isNaN(rawValue) || rawValue <= 0) {
      return;
    }

    const sanitized = Math.min(Math.round(rawValue), MAX_PAGE_SIZE);

    setCriteria((previous) => buildCriteria(filters, {
      keyword: previous?.keyword ?? DEFAULT_KEYWORD,
      days: previous?.days ?? DEFAULT_RANGE,
      pageSize: sanitized,
      page: DEFAULT_PAGE
    }));
  }, [activeTab, buildCriteria, filters]);

  const isResearchDisabled = isLoading;
  const showEmptyState = !isLoading && totalCount === 0;
  const hasActiveFilters = activeFilterChips.length > 0;

  return (
    <>
      <div className="product-research">
        <header className="product-research__header">
          <div>
            <h1>Research products</h1>
            <p>Understand what buyers are actively searching for and how similar listings perform.</p>
          </div>
          <button className="product-research__tour">Take a tour</button>
        </header>

        <section className="product-research__search" ref={filterSectionRef}>
          <div className="product-research__search-bar">
            <label htmlFor="product-research-query" className="sr-only">Search keyword</label>
            <div className="product-research__search-field">
              <span className="product-research__search-icon" aria-hidden="true">
                <svg width="16" height="16" viewBox="0 0 16 16" fill="none">
                  <path d="M7.25 2.5a4.75 4.75 0 1 1-3.36 8.11l-2.02 2.02a.75.75 0 1 1-1.06-1.06l2.02-2.02A4.75 4.75 0 0 1 7.25 2.5Zm0 1.5a3.25 3.25 0 1 0 0 6.5 3.25 3.25 0 0 0 0-6.5Z" fill="currentColor" />
                </svg>
              </span>
              <input
                id="product-research-query"
                type="text"
                value={searchTerm}
                onChange={(event) => setSearchTerm(event.target.value)}
                placeholder="Search by keyword"
              />
            </div>
            <div className="product-research__actions">
              <select value={timeRange} onChange={(event) => setTimeRange(event.target.value)}>
                {RANGE_OPTIONS.map((option) => (
                  <option key={option.value} value={option.value.toString()}>{option.label}</option>
                ))}
              </select>
              <button
                type="button"
                className="product-research__primary"
                onClick={handleResearch}
                disabled={isResearchDisabled}
              >
                {isLoading ? "Loading..." : "Research"}
              </button>
              <button type="button" className="product-research__reset" onClick={handleReset} disabled={isLoading}>
                Reset
              </button>
            </div>
          </div>

          <div className="product-research__meta">
            <div className="product-research__category">
              <span>Category selected:</span>
              <button
                type="button"
                className="product-research__pill"
                title={selectedCategoryLabel || undefined}
                disabled
              >
                {selectedCategoryLabel || "All Categories"}
              </button>
              <button
                type="button"
                className="product-research__link"
                onClick={handleOpenCategoryModal}
                disabled={lockFilters}
              >
                Select a different category
              </button>
              {filters.categoryId && (
                <button
                  type="button"
                  className="product-research__link"
                  onClick={handleClearCategory}
                  disabled={lockFilters}
                >
                  Clear category
                </button>
              )}
            </div>
          </div>

          <div className="product-research__filters">
            <div className="product-research__filters-group">
              <div className={`product-research__filter${openFilter === "condition" ? " product-research__filter--open" : ""}`}>
                <button
                  type="button"
                  className="product-research__filter-chip"
                  onClick={() => handleToggleDropdown("condition")}
                  disabled={lockFilters}
                >
                  Condition filter
                  <span className="product-research__filter-arrow" aria-hidden="true">▾</span>
                </button>
                {openFilter === "condition" && (
                  <div className="product-research__dropdown">
                    <div className="product-research__dropdown-section">
                      {CONDITION_OPTIONS.map((option) => (
                        <label key={option.value} className="product-research__dropdown-option">
                          <input
                            type="checkbox"
                            checked={draftFilters.conditions.includes(option.value)}
                            onChange={() => handleConditionToggle(option.value)}
                          />
                          <span>{option.label}</span>
                        </label>
                      ))}
                    </div>
                    <div className="product-research__dropdown-footer">
                      <button type="button" onClick={handleApplyFilters}>Apply</button>
                    </div>
                  </div>
                )}
              </div>

              <div className={`product-research__filter${openFilter === "format" ? " product-research__filter--open" : ""}`}>
                <button
                  type="button"
                  className="product-research__filter-chip"
                  onClick={() => handleToggleDropdown("format")}
                  disabled={lockFilters}
                >
                  Format filter
                  <span className="product-research__filter-arrow" aria-hidden="true">▾</span>
                </button>
                {openFilter === "format" && (
                  <div className="product-research__dropdown">
                    <div className="product-research__dropdown-section">
                      {FORMAT_FILTER_OPTIONS.map((option) => (
                        <label key={option.value} className="product-research__dropdown-option">
                          <input
                            type="checkbox"
                            checked={draftFilters.formats.includes(option.value)}
                            onChange={() => handleFormatToggle(option.value)}
                          />
                          <span>{option.label}</span>
                        </label>
                      ))}
                    </div>
                    <div className="product-research__dropdown-footer">
                      <button type="button" onClick={handleApplyFilters}>Apply</button>
                    </div>
                  </div>
                )}
              </div>

              <div className={`product-research__filter${openFilter === "price" ? " product-research__filter--open" : ""}`}>
                <button
                  type="button"
                  className="product-research__filter-chip"
                  onClick={() => handleToggleDropdown("price")}
                  disabled={lockFilters}
                >
                  Price filter
                  <span className="product-research__filter-arrow" aria-hidden="true">▾</span>
                </button>
                {openFilter === "price" && (
                  <div className="product-research__dropdown product-research__dropdown--price">
                    <div className="product-research__dropdown-row">
                      <input
                        type="number"
                        min="0"
                        step="0.01"
                        placeholder="$"
                        value={draftFilters.minPrice}
                        onChange={(event) => handleDraftPriceChange("minPrice", event.target.value)}
                      />
                      <span className="product-research__price-separator">-</span>
                      <input
                        type="number"
                        min="0"
                        step="0.01"
                        placeholder="$$"
                        value={draftFilters.maxPrice}
                        onChange={(event) => handleDraftPriceChange("maxPrice", event.target.value)}
                      />
                    </div>
                    <div className="product-research__dropdown-footer">
                      <button type="button" onClick={handleApplyFilters}>Apply</button>
                    </div>
                  </div>
                )}
              </div>

              <div className={`product-research__filter${openFilter === "topRated" ? " product-research__filter--open" : ""}`}>
                <button
                  type="button"
                  className="product-research__filter-chip"
                  onClick={() => handleToggleDropdown("topRated")}
                  disabled={lockFilters}
                >
                  Top rated
                  <span className="product-research__filter-arrow" aria-hidden="true">▾</span>
                </button>
                {openFilter === "topRated" && (
                  <div className="product-research__dropdown product-research__dropdown--compact">
                    <label className="product-research__dropdown-option product-research__dropdown-option--switch">
                      <input
                        type="checkbox"
                        checked={draftFilters.topRatedOnly}
                        onChange={(event) => handleDraftTopRatedChange(event.target.checked)}
                      />
                      <span>Show top rated sellers only</span>
                    </label>
                    <div className="product-research__dropdown-footer">
                      <button type="button" onClick={handleApplyFilters}>Apply</button>
                    </div>
                  </div>
                )}
              </div>
            </div>
            <div className="product-research__actions-secondary">
              <button
                type="button"
                className="product-research__filter-chip product-research__filter-chip--ghost"
                disabled
              >
                More filters
              </button>
              <label className="product-research__lock">
                <span className="product-research__lock-text">Lock selected filters</span>
                <input
                  type="checkbox"
                  className="product-research__lock-input"
                  checked={lockFilters}
                  onChange={(event) => setLockFilters(event.target.checked)}
                />
                <span className="product-research__lock-switch" aria-hidden="true">
                  <span className="product-research__lock-thumb" />
                </span>
              </label>
            </div>
          </div>

          <div className="product-research__applied">
            <span className="product-research__applied-heading">Applied filters</span>
            {!hasActiveFilters ? (
              <span className="product-research__chips-empty">None yet</span>
            ) : (
              <>
                {activeFilterChips.map((chip) => (
                  <button
                    type="button"
                    key={`${chip.type}-${String(chip.value ?? "on")}`}
                    className="chip chip--active"
                    onClick={() => handleRemoveFilter(chip)}
                  >
                    {chip.label}
                    <span className="chip__remove" aria-hidden="true">×</span>
                  </button>
                ))}
                <button type="button" className="chip chip--ghost" onClick={handleClearFilters}>Clear filters</button>
              </>
            )}
          </div>
        </section>

        {error && (
          <div className="product-research__error" role="alert">
            {error}
          </div>
        )}

        <div className="product-research__panel">
          <div className="product-research__tabs">
            {[
              { key: "sold", label: "Sold" },
              { key: "active", label: "Active" }
            ].map((tab) => (
              <button
                key={tab.key}
                type="button"
                className={`product-research__tab${tab.key === activeTab ? " product-research__tab--active" : ""}`}
                onClick={() => setActiveTab(tab.key)}
              >
                {tab.label}
              </button>
            ))}
          </div>

          <div className="product-research__panel-header">
            <div>
              <p className="product-research__date-range">{dateRangeLabel}</p>
              <p className="product-research__muted">{selectedRangeOption.label}</p>
            </div>
            <button
              type="button"
              className={`switch${showTrends ? " switch--on" : ""}`}
              onClick={() => setShowTrends((value) => !value)}
            >
              <span className="switch__label">{activeTab === "sold" ? "Show sales trends" : "Show current trends"}</span>
              <span className="switch__track" aria-hidden="true">
                <span className="switch__thumb" />
              </span>
            </button>
          </div>

          <div className="product-research__summary">
            {summaryMetrics.map((metric) => (
              <div key={metric.key} className="product-research__summary-item">
                <span className="product-research__summary-value">{metric.value}</span>
                <span className="product-research__summary-label">{metric.label}</span>
              </div>
            ))}
          </div>

          <div className="product-research__chart">
            <div className="product-research__chart-header">
              <h2>{activeTab === "sold" ? "Avg sold price" : "Avg listing price"}</h2>
            </div>
            {showTrends && trendPolyline ? (
              <svg viewBox="0 0 100 100" preserveAspectRatio="none" role="img" aria-label="Trend over time">
                <defs>
                  <linearGradient id="product-trend" x1="0" x2="0" y1="0" y2="1">
                    <stop offset="0%" stopColor="rgba(37, 99, 235, 0.35)" />
                    <stop offset="100%" stopColor="rgba(37, 99, 235, 0.05)" />
                  </linearGradient>
                </defs>
                <polygon points={`0,100 ${trendPolyline} 100,100`} fill="url(#product-trend)" />
                <polyline points={trendPolyline} fill="none" stroke="rgba(37, 99, 235, 0.8)" strokeWidth="2" strokeLinejoin="round" strokeLinecap="round" />
              </svg>
            ) : (
              <div className="product-research__chart-empty">{showTrends ? "No trend data yet." : "Trends are hidden."}</div>
            )}
          </div>

          <div className="product-research__table-wrapper">
            {isLoading && (
              <div className="product-research__loading" aria-hidden="true">
                <LoadingScreen isOverlay />
              </div>
            )}

            <table className="product-research__table">
              <thead>
                <tr>
                  <th>Listing</th>
                  <th>Actions</th>
                  {activeTab === "sold" ? <th>Avg sold price</th> : <th>Listing price</th>}
                  <th>Avg shipping</th>
                  <th>{activeTab === "sold" ? "Total sold" : "Watchers"}</th>
                  <th>{activeTab === "sold" ? "Item sales" : "Promoted listing"}</th>
                  <th>{activeTab === "sold" ? "Bids" : "Start date"}</th>
                  {activeTab === "sold" && <th>Date last sold</th>}
                </tr>
              </thead>
              <tbody>
                {showEmptyState ? (
                  <tr>
                    <td colSpan={activeTab === "sold" ? 8 : 7} className="product-research__empty">
                      No listings found for the current filters.
                    </td>
                  </tr>
                ) : (
                  listings.map((listing, index) => {
                    const listingId = listing.listingId ?? listing.ListingId ?? `${activeTab}-${index}`;
                    const title = listing.title ?? listing.Title ?? "Untitled listing";
                    const imageUrl = listing.imageUrl ?? listing.ImageUrl;
                    const pricingType = listing.pricingType ?? listing.PricingType;
                    const price = listing.price ?? listing.Price;
                    const shipping = listing.shipping ?? listing.Shipping;
                    const totalSold = listing.totalSold ?? listing.TotalSold;
                    const watchers = listing.watchers ?? listing.Watchers;
                    const totalSales = listing.totalSales ?? listing.TotalSales;
                    const promoted = listing.promoted ?? listing.Promoted;
                    const startDate = listing.startDate ?? listing.StartDate;
                    const lastSoldAt = listing.lastSoldAt ?? listing.LastSoldAt;

                    return (
                      <tr key={listingId}>
                        <td>
                          <div className="product-research__listing">
                            <div className="product-research__thumbnail" aria-hidden="true">
                              {imageUrl ? <img src={imageUrl} alt="" /> : <div className="product-research__thumbnail-placeholder" />}
                            </div>
                            <div>
                              <p className="product-research__listing-title">{title}</p>
                              {pricingType && <span className="product-research__muted">{pricingType}</span>}
                            </div>
                          </div>
                        </td>
                        <td>
                          <button type="button" className="product-research__action">Edit</button>
                        </td>
                        <td>
                          <div className="product-research__cell">
                            <span>{formatMoney(price)}</span>
                            {activeTab === "sold" && pricingType && (
                              <span className="product-research__muted">{pricingType}</span>
                            )}
                          </div>
                        </td>
                        <td>{formatShipping(shipping)}</td>
                        <td>{activeTab === "sold" ? formatNumber(totalSold) : formatNumber(watchers)}</td>
                        <td>
                          {activeTab === "sold" ? (
                            formatMoney(totalSales)
                          ) : (
                            <span className={`product-research__badge${promoted ? " product-research__badge--on" : ""}`}>
                              {promoted ? "✓" : "-"}
                            </span>
                          )}
                        </td>
                        <td>{activeTab === "sold" ? "-" : formatDateLabel(startDate)}</td>
                        {activeTab === "sold" && <td>{formatDateLabel(lastSoldAt)}</td>}
                      </tr>
                    );
                  })
                )}
              </tbody>
            </table>
          </div>

          <div className="product-research__pagination">
            <div className="product-research__pager">
              <button
                type="button"
                aria-label="Previous page"
                onClick={handlePrevPage}
                disabled={prevDisabled}
              >
                {"\u2039"}
              </button>
              <span>{paginationLabel}</span>
              <button
                type="button"
                aria-label="Next page"
                onClick={handleNextPage}
                disabled={nextDisabled}
              >
                {"\u203a"}
              </button>
            </div>
            <div className="product-research__results">
              <span className="product-research__range" aria-live="polite">{rangeLabel}</span>
              <label htmlFor="results-per-page">Results per page</label>
              <select
                id="results-per-page"
                value={currentPageSize}
                onChange={handlePageSizeChange}
                disabled={pageSizeSelectDisabled}
              >
                {resolvedPageSizeOptions.map((size) => (
                  <option key={size} value={size}>{size}</option>
                ))}
              </select>
            </div>
          </div>
        </div>
        <CategoryModal
          isOpen={isCategoryModalOpen}
          onClose={handleCloseCategoryModal}
          onSelect={handleCategorySelect}
          initialSelected={filters.categoryPath}
        />
      </div>
    </>
  );
};
export default ProductResearchPage;
