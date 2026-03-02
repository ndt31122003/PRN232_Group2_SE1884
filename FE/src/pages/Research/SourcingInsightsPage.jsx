import React, { useCallback, useEffect, useMemo, useState } from "react";
import "./SourcingInsightsPage.scss";
import ResearchService from "../../services/ResearchService";

const TABS = [
  { id: "saved", label: "Saved categories" },
  { id: "search", label: "Search & browse" }
];

const DISPLAY_OPTIONS = [
  { value: "card", label: "Card view" },
  { value: "table", label: "Table view" }
];

const SORT_OPTIONS = [
  { value: "opportunity", label: "Top opportunities first" },
  { value: "searchVolume", label: "Highest search volume" },
  { value: "activeListings", label: "Most active listings" },
  { value: "sellThrough", label: "Highest sell-through" },
  { value: "alpha", label: "Alphabetical" }
];

const LISTING_SITES = [
  { value: "ebay.com.au", label: "ebay.com.au" },
  { value: "ebay.com", label: "ebay.com" },
  { value: "ebay.co.uk", label: "ebay.co.uk" }
];

const MAX_SAVED_CATEGORIES = 150;
const PAGE_SIZE = MAX_SAVED_CATEGORIES;

const formatNumber = (value) => {
  if (typeof value !== "number" || Number.isNaN(value)) {
    return "—";
  }

  return value.toLocaleString("en-US");
};

const formatPercent = (value) => {
  if (typeof value !== "number" || Number.isNaN(value)) {
    return "—";
  }

  return `${value.toFixed(2)}%`;
};

const formatRatio = (value) => {
  if (typeof value !== "number" || Number.isNaN(value)) {
    return "—";
  }

  return value.toFixed(2);
};

const formatScore = (value) => {
  if (typeof value !== "number" || Number.isNaN(value)) {
    return "—";
  }

  return value.toFixed(2);
};

const formatDays = (value) => {
  if (typeof value !== "number" || Number.isNaN(value)) {
    return "—";
  }

  return `${value}`;
};

const arrowIcon = (
  <svg width="16" height="16" viewBox="0 0 16 16" aria-hidden="true">
    <path
      d="M5.22 3.97a.75.75 0 0 1 1.06 0l3.5 3.5a.75.75 0 0 1 0 1.06l-3.5 3.5a.75.75 0 1 1-1.06-1.06L8.19 8 5.22 5.03a.75.75 0 0 1 0-1.06Z"
      fill="currentColor"
    />
  </svg>
);

const scoreIcon = (
  <svg width="14" height="14" viewBox="0 0 16 16" aria-hidden="true">
    <path
      d="M14 8a6 6 0 1 1-12 0 6 6 0 0 1 12 0Zm-3.2-1.8a.6.6 0 0 0-.848-.848L7.1 8.203 5.9 7a.6.6 0 0 0-.848.848l1.6 1.6a.6.6 0 0 0 .848 0Z"
      fill="currentColor"
    />
  </svg>
);

const infoIcon = (
  <svg width="14" height="14" viewBox="0 0 16 16" aria-hidden="true">
    <path
      d="M8 1.5a6.5 6.5 0 1 0 0 13 6.5 6.5 0 0 0 0-13Zm0 2a.875.875 0 1 1 0 1.75.875.875 0 0 1 0-1.75ZM8 12a.75.75 0 0 1-.75-.75V7.75a.75.75 0 1 1 1.5 0v3.5A.75.75 0 0 1 8 12Z"
      fill="currentColor"
    />
  </svg>
);

const SourcingInsightsPage = () => {
  const [activeTab, setActiveTab] = useState("saved");
  const [displayMode, setDisplayMode] = useState("card");
  const [sortMode, setSortMode] = useState("opportunity");
  const [listingSite, setListingSite] = useState("ebay.com.au");
  const [categories, setCategories] = useState([]);
  const [savedIds, setSavedIds] = useState([]);
  const [searchKeyword, setSearchKeyword] = useState("");
  const [debouncedKeyword, setDebouncedKeyword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [isSavingIds, setIsSavingIds] = useState({});
  const [error, setError] = useState(null);
  const [totalCount, setTotalCount] = useState(0);
  const totalCountLabel = useMemo(() => formatNumber(totalCount), [totalCount]);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedKeyword(searchKeyword.trim());
    }, 400);

    return () => clearTimeout(handler);
  }, [searchKeyword]);

  const fetchSourcingInsights = useCallback(async () => {
    setIsLoading(true);
    setError(null);

    try {
      const response = await ResearchService.getSourcingInsights({
        keyword: debouncedKeyword,
        sort: sortMode,
        page: 1,
        pageSize: PAGE_SIZE,
        savedOnly: false
      });

      const categoriesList = response?.categories ?? [];
      const savedCategoryIds = response?.savedCategoryIds ?? [];

      setCategories(categoriesList);
      setSavedIds(savedCategoryIds);
      setTotalCount(response?.totalCount ?? categoriesList.length);
    } catch (requestError) {
      console.error("Failed to load sourcing insights", requestError);
      setCategories([]);
      setSavedIds([]);
      setTotalCount(0);
      setError("Không thể tải dữ liệu sourcing insights. Vui lòng thử lại sau.");
    } finally {
      setIsLoading(false);
    }
  }, [debouncedKeyword, sortMode]);

  useEffect(() => {
    fetchSourcingInsights();
  }, [fetchSourcingInsights]);

  const sortedCategories = useMemo(() => {
    const list = [...categories];

    switch (sortMode) {
      case "searchVolume":
        return list.sort((a, b) => (b.searchVolume ?? 0) - (a.searchVolume ?? 0));
      case "activeListings":
        return list.sort((a, b) => (b.activeListings ?? 0) - (a.activeListings ?? 0));
      case "sellThrough":
        return list.sort((a, b) => (b.sellThroughRate ?? 0) - (a.sellThroughRate ?? 0));
      case "alpha":
        return list.sort((a, b) => a.name.localeCompare(b.name));
      default:
        return list.sort((a, b) => (b.opportunityScore ?? 0) - (a.opportunityScore ?? 0));
    }
  }, [categories, sortMode]);

  const filteredCategories = useMemo(() => {
    if (!searchKeyword.trim()) {
      return sortedCategories;
    }

    const term = searchKeyword.toLowerCase();
    return sortedCategories.filter((category) =>
      category.name.toLowerCase().includes(term)
      || category.group.toLowerCase().includes(term)
    );
  }, [sortedCategories, searchKeyword]);

  const savedCategories = useMemo(
    () => sortedCategories.filter((category) => savedIds.includes(category.id)),
    [sortedCategories, savedIds]
  );

  const savedCount = savedIds.length;
  const toggleSaved = useCallback(async (categoryId) => {
    if (!categoryId || isSavingIds[categoryId]) {
      return;
    }

    setIsSavingIds((prev) => ({ ...prev, [categoryId]: true }));
    setError(null);

    try {
      if (savedIds.includes(categoryId)) {
        await ResearchService.removeSourcingCategory(categoryId);
        setSavedIds((prev) => prev.filter((id) => id !== categoryId));
      } else {
        if (savedIds.length >= MAX_SAVED_CATEGORIES) {
          setError(`Bạn đã đạt giới hạn ${MAX_SAVED_CATEGORIES} danh mục đã lưu.`);
          return;
        }

        await ResearchService.saveSourcingCategory(categoryId);
        setSavedIds((prev) => {
          if (prev.includes(categoryId)) {
            return prev;
          }

          return [...prev, categoryId];
        });
      }
    } catch (requestError) {
      console.error("Failed to update saved categories", requestError);
      setError("Không thể cập nhật danh sách danh mục đã lưu. Thử lại sau ít phút.");
    } finally {
      setIsSavingIds((prev) => {
        const next = { ...prev };
        delete next[categoryId];
        return next;
      });
    }
  }, [isSavingIds, savedIds]);

  const renderTableRows = (categories) => (
    categories.map((category) => {
      const isSaved = savedIds.includes(category.id);
      const isSaving = Boolean(isSavingIds[category.id]);
      return (
        <tr key={category.id}>
          <th scope="row">
            <div className="sourcing-insights__table-name">
              <button type="button" className="sourcing-insights__table-link">
                {category.name}
              </button>
              <span>{category.group}</span>
            </div>
          </th>
          <td>
            <span className="sourcing-insights__score">
              {scoreIcon}
              {formatScore(category.opportunityScore)}
            </span>
          </td>
          <td>{formatNumber(category.searchVolume)}</td>
          <td>{formatNumber(category.activeListings)}</td>
          <td>{formatRatio(category.searchToListingRatio)}</td>
          <td>{formatPercent(category.sellThroughRate)}</td>
          <td>{formatDays(category.averageDaysToFirstSale)}</td>
          <td>{formatPercent(category.returnRate)}</td>
          <td>{formatPercent(category.marketShare)}</td>
          <td>
            <button
              type="button"
              className={`sourcing-insights__save-toggle ${isSaved ? "is-saved" : ""}`}
              onClick={() => toggleSaved(category.id)}
              aria-pressed={isSaved}
              disabled={isSaving}
            >
              {isSaving ? "Saving..." : isSaved ? "Saved" : "Save"}
            </button>
          </td>
        </tr>
      );
    })
  );

  const savedEmptyState = (
    <div className="sourcing-insights__empty">
      <h2>You have no saved categories yet</h2>
      <p>Select categories from Search & browse to pin them here for quick monitoring.</p>
      <button type="button" onClick={() => setActiveTab("search")}>
        Go to Search & browse
      </button>
    </div>
  );

  const savedCards = (
    <div className="sourcing-insights__cards">
      {savedCategories.map((category) => {
        const isSaved = savedIds.includes(category.id);
        const isSaving = Boolean(isSavingIds[category.id]);
        return (
          <article key={category.id} className="sourcing-insights__card">
            <header className="sourcing-insights__card-header">
              <div>
                <h2>{category.name}</h2>
                <p>{category.group}</p>
              </div>
              <div className="sourcing-insights__card-actions">
                <span className={`sourcing-insights__tag sourcing-insights__tag--${category.tag.replace(/\s+/g, "-").toLowerCase()}`}>
                  {category.tag}
                </span>
                <button
                  type="button"
                  className={`sourcing-insights__save-chip ${isSaved ? "is-saved" : ""}`}
                  onClick={() => toggleSaved(category.id)}
                  disabled={isSaving}
                >
                  {isSaving ? "Saving..." : isSaved ? "Saved" : "Save"}
                </button>
              </div>
            </header>
            <dl className="sourcing-insights__metrics">
              <div>
                <dt>Search volume</dt>
                <dd>{formatNumber(category.searchVolume)}</dd>
              </div>
              <div>
                <dt>Active listings</dt>
                <dd>{formatNumber(category.activeListings)}</dd>
              </div>
              <div>
                <dt>Search-to-listing ratio</dt>
                <dd>{formatRatio(category.searchToListingRatio)}</dd>
              </div>
              <div>
                <dt>Sell-through rate (%)</dt>
                <dd>{formatPercent(category.sellThroughRate)}</dd>
              </div>
            </dl>
            <div className="sourcing-insights__card-footer">
              <span>
                Opportunity score: <strong>{formatScore(category.opportunityScore)}</strong>
              </span>
              <button type="button" className="sourcing-insights__details">
                View listing insights
                {arrowIcon}
              </button>
            </div>
          </article>
        );
      })}
    </div>
  );

  const savedTable = (
    <div className="sourcing-insights__table-wrapper">
      <table className="sourcing-insights__table" aria-label="Saved categories table">
        <thead>
          <tr>
            <th scope="col">Category name</th>
            <th scope="col">
              <span className="sourcing-insights__header-with-icon">
                Opportunity score
                {infoIcon}
              </span>
            </th>
            <th scope="col">Search volume</th>
            <th scope="col">Active listings</th>
            <th scope="col">Search-to-listing ratio</th>
            <th scope="col">Sell-through rate (%)</th>
            <th scope="col">Avg. days to first sale</th>
            <th scope="col">Return rate (%)</th>
            <th scope="col">Market share (%)</th>
            <th scope="col">Save</th>
          </tr>
        </thead>
        <tbody>
          {isLoading ? (
            <tr>
              <td colSpan={10} className="sourcing-insights__table-empty">
                Đang tải dữ liệu sourcing insights...
              </td>
            </tr>
          ) : (
            renderTableRows(savedCategories)
          )}
        </tbody>
      </table>
    </div>
  );

  const searchTable = (
    <div className="sourcing-insights__table-wrapper">
      <table className="sourcing-insights__table" aria-label="Search and browse categories">
        <thead>
          <tr>
            <th scope="col">Category name</th>
            <th scope="col">
              <span className="sourcing-insights__header-with-icon">
                Opportunity score
                {infoIcon}
              </span>
            </th>
            <th scope="col">Search volume</th>
            <th scope="col">Active listings</th>
            <th scope="col">Search-to-listing ratio</th>
            <th scope="col">Sell-through rate (%)</th>
            <th scope="col">Avg. days to first sale</th>
            <th scope="col">Return rate (%)</th>
            <th scope="col">Market share (%)</th>
            <th scope="col">Save</th>
          </tr>
        </thead>
        <tbody>
          {isLoading ? (
            <tr>
              <td colSpan={10} className="sourcing-insights__table-empty">
                Đang tải dữ liệu sourcing insights...
              </td>
            </tr>
          ) : filteredCategories.length === 0 ? (
            <tr>
              <td colSpan={10} className="sourcing-insights__table-empty">
                No categories match your search. Try another keyword.
              </td>
            </tr>
          ) : (
            renderTableRows(filteredCategories)
          )}
        </tbody>
      </table>
    </div>
  );

  return (
    <div className="sourcing-insights">
      <header className="sourcing-insights__header">
        <div className="sourcing-insights__heading">
          <h1>Sourcing insights</h1>
          <p>Monitor how saved categories are performing so you can spot sourcing gaps quickly.</p>
        </div>
        <div className="sourcing-insights__actions">
          <button type="button" className="sourcing-insights__link">Take a tour</button>
          <span aria-hidden="true" className="sourcing-insights__divider">|</span>
          <button type="button" className="sourcing-insights__link">Comments?</button>
          <div className="sourcing-insights__site">
            <label htmlFor="listing-site">Listing site:</label>
            <select
              id="listing-site"
              value={listingSite}
              onChange={(event) => setListingSite(event.target.value)}
            >
              {LISTING_SITES.map((site) => (
                <option key={site.value} value={site.value}>{site.label}</option>
              ))}
            </select>
          </div>
        </div>
      </header>

      <section className="sourcing-insights__layout" aria-label="Sourcing insights categories">
        {error && (
          <div className="sourcing-insights__status sourcing-insights__status--error" role="alert">
            {error}
          </div>
        )}

        <div className="sourcing-insights__tabs-row">
          <nav aria-label="Category views" className="sourcing-insights__tabs">
            {TABS.map((tab) => (
              <button
                key={tab.id}
                type="button"
                className={`sourcing-insights__tab ${tab.id === activeTab ? "is-active" : ""}`}
                onClick={() => setActiveTab(tab.id)}
              >
                {tab.label}
              </button>
            ))}
          </nav>
          <div className="sourcing-insights__saved">
            <span>{`${savedCount}/${MAX_SAVED_CATEGORIES}`}</span>
            <button type="button" className="sourcing-insights__info" aria-label="Saved category usage">
              {infoIcon}
            </button>
          </div>
        </div>

        <div className="sourcing-insights__controls">
          {activeTab === "saved" && (
            <label className="sourcing-insights__control">
              <span>Display:</span>
              <select value={displayMode} onChange={(event) => setDisplayMode(event.target.value)}>
                {DISPLAY_OPTIONS.map((option) => (
                  <option key={option.value} value={option.value}>{option.label}</option>
                ))}
              </select>
            </label>
          )}
          {activeTab === "search" && (
            <label className="sourcing-insights__control sourcing-insights__control--wide">
              <span>Search categories:</span>
              <input
                type="search"
                placeholder="Search by keyword"
                value={searchKeyword}
                onChange={(event) => setSearchKeyword(event.target.value)}
              />
            </label>
          )}
          <label className="sourcing-insights__control">
            <span>Sort:</span>
            <select value={sortMode} onChange={(event) => setSortMode(event.target.value)}>
              {SORT_OPTIONS.map((option) => (
                <option key={option.value} value={option.value}>{option.label}</option>
              ))}
            </select>
          </label>
          <span className="sourcing-insights__note">*All data from the last 30 days • {totalCountLabel} categories tracked</span>
        </div>

        {activeTab === "saved" && !isLoading && savedCategories.length === 0 && savedEmptyState}

        {activeTab === "saved" && savedCategories.length > 0 && (
          displayMode === "card" ? savedCards : savedTable
        )}

        {activeTab === "search" && searchTable}

      </section>
    </div>
  );
};

export default SourcingInsightsPage;
