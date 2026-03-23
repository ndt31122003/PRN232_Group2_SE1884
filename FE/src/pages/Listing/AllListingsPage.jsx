import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import ListingService from "../../services/ListingService";
import Notice from "../../components/Common/CustomNotification";
import "./AllListingsPage.css";

const STATUS_COPY = {
    active: {
        heading: "Manage active listings",
        subtitle: "Keep an eye on listings that are currently live on eBay.",
        emptyTitle: "You don't have any active listings yet.",
        emptySubtitle: "Create a listing to start selling. Your active listings will appear in this table."
    },
    unsold: {
        heading: "Manage unsold listings",
        subtitle: "Relist items that didn't sell or move them back into drafts.",
        emptyTitle: "No unsold listings.",
        emptySubtitle: "When a listing ends without a sale, you'll see it here for quick relisting."
    },
    drafts: {
        heading: "Manage drafts",
        subtitle: "Pick up where you left off and finish your draft listings.",
        emptyTitle: "No draft listings saved.",
        emptySubtitle: "Save a draft while creating a listing and it will show up here for quick access."
    },
    scheduled: {
        heading: "Manage scheduled listings",
        subtitle: "Stay on top of the listings that are queued to go live soon.",
        emptyTitle: "No scheduled listings yet.",
        emptySubtitle: "When you schedule a listing, it will appear here until it goes live."
    },
    ended: {
        heading: "Manage ended listings",
        subtitle: "Review completed listings and decide what to do next.",
        emptyTitle: "No ended listings available.",
        emptySubtitle: "After a listing ends you'll see it here for reporting and relist options."
    }
};

const TABLE_COLUMNS = [
    { key: "item", label: "Item" },
    { key: "sku", label: "Custom label (SKU)" },
    { key: "currentPrice", label: "Current price" },
    { key: "discount", label: "Discount" },
    { key: "available", label: "Available quantity" },
    { key: "reserve", label: "Reserve price" },
    { key: "shipping", label: "Shipping cost" },
    { key: "startPrice", label: "Start price" },
    { key: "soldQty", label: "Sold qty" },
    { key: "duration", label: "Duration" },
    { key: "endDate", label: "End date" },
    { key: "startDate", label: "Start date" },
    { key: "watchersCount", label: "Watchers" },
    { key: "bidsCount", label: "Bids" },
    { key: "offersCount", label: "Offers" },
    { key: "bestOfferAmount", label: "Best Offer" }
];

const normalizeFailures = (rawFailures) => {
    if (!Array.isArray(rawFailures)) {
        return [];
    }

    return rawFailures.map((failure) => ({
        listingId: failure?.listingId ?? failure?.ListingId ?? null,
        code: failure?.code ?? failure?.Code ?? null,
        message: failure?.message ?? failure?.Message ?? "Unexpected error."
    }));
};

const buildFailureSummary = (failures, limit = 3) => {
    if (!Array.isArray(failures) || failures.length === 0) {
        return { total: 0, summary: "" };
    }

    const summary = failures
        .slice(0, limit)
        .map((failure) => {
            const identifier = failure.listingId
                ? `Listing ${failure.listingId}`
                : failure.code ?? "Listing";
            return `${identifier}: ${failure.message}`;
        })
        .filter(Boolean)
        .join(" • ");

    const overflow = failures.length > limit ? ` • +${failures.length - limit} more` : "";
    return { total: failures.length, summary: summary ? `${summary}${overflow}` : overflow.trim() };
};

const extractListingId = (row) => {
    if (!row || typeof row !== "object") {
        return null;
    }

    const candidates = ["Id", "id", "ListingId", "listingId"];
    for (const key of candidates) {
        if (key in row && row[key] != null) {
            const raw = row[key];
            if (typeof raw === "string" && raw.trim().length > 0) {
                return raw;
            }
            if (typeof raw === "number" || typeof raw === "bigint") {
                return raw.toString();
            }
            if (typeof raw === "object" && raw !== null && typeof raw.toString === "function") {
                const asString = raw.toString();
                if (asString && asString !== "[object Object]") {
                    return asString;
                }
            }
        }
    }

    return null;
};

const evaluateEditRule = (status, row) => {
    if (!row) {
        return { canEdit: false, reason: "Select a listing to edit." };
    }

    switch (status) {
        case "drafts":
            return { canEdit: true };
        case "scheduled": {
            const rawStart = row.StartDate ?? row.startDate;
            if (!rawStart) {
                return { canEdit: true };
            }

            const startDate = new Date(rawStart);
            if (Number.isNaN(startDate.getTime())) {
                return { canEdit: true };
            }

            const timeUntilStart = startDate.getTime() - Date.now();
            const twoHours = 1000 * 60 * 60 * 2;
            if (timeUntilStart <= twoHours) {
                return {
                    canEdit: false,
                    reason: "Scheduled listings can't be edited within 2 hours of the start time."
                };
            }

            return { canEdit: true };
        }
        case "active": {
            const format = Number(row.Format ?? row.format);
            if (Number.isFinite(format) && format === 1) {
                const bidsCount = Number(row.BidsCount ?? row.bidsCount ?? 0);
                if (Number.isFinite(bidsCount) && bidsCount > 0) {
                    return {
                        canEdit: false,
                        reason: "Auctions with bids can't be revised on eBay."
                    };
                }

                const rawEnd = row.EndDate ?? row.endDate;
                if (rawEnd) {
                    const endDate = new Date(rawEnd);
                    if (!Number.isNaN(endDate.getTime())) {
                        const timeUntilEnd = endDate.getTime() - Date.now();
                        const twelveHours = 1000 * 60 * 60 * 12;
                        if (timeUntilEnd <= twelveHours) {
                            return {
                                canEdit: false,
                                reason: "Auctions ending in 12 hours or less can't be revised."
                            };
                        }
                    }
                }
            }

            return { canEdit: true };
        }
        default:
            return {
                canEdit: false,
                reason: "This listing status can't be edited on eBay."
            };
    }
};

const buildEditMeta = (status, selectedRows) => {
    if (!Array.isArray(selectedRows) || selectedRows.length === 0) {
        return {
            canEdit: false,
            reason: "Select a listing to edit."
        };
    }

    if (selectedRows.length > 1) {
        return {
            canEdit: false,
            reason: "Editing is limited to one listing at a time."
        };
    }

    return evaluateEditRule(status, selectedRows[0]);
};

const buildStatusToolbarActions = (status, context) => {
    const {
        selectedCount,
        selectedRows,
        onEditSelected = () => { },
        onDeleteDrafts = () => { },
        onCopyDrafts = () => { },
        onRelist = () => { },
        onRelistFixedPrice = () => { },
        onSellSimilar = () => { },
        isBusy,
        actionMenuItems = []
    } = context;

    const editMeta = buildEditMeta(status, selectedRows);
    const selectionHint = selectedCount === 0 ? "Select at least one listing to continue." : undefined;

    switch (status) {
        case "drafts":
            return [
                {
                    label: "Resume drafts",
                    variant: "primary",
                    disabled: !editMeta.canEdit || isBusy,
                    onClick: onEditSelected,
                    title: editMeta.canEdit
                        ? (isBusy ? "Please wait for the current action to finish." : undefined)
                        : editMeta.reason
                },
                {
                    label: "Delete drafts",
                    variant: "outline",
                    disabled: selectedCount === 0 || isBusy,
                    onClick: onDeleteDrafts,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint
                },
                {
                    label: "Create a copy",
                    variant: "outline",
                    disabled: selectedCount === 0 || isBusy,
                    onClick: onCopyDrafts,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint
                }
            ];
        case "scheduled":
            return [
                {
                    label: "Edit",
                    variant: "primary",
                    disabled: !editMeta.canEdit || isBusy,
                    onClick: onEditSelected,
                    title: editMeta.canEdit
                        ? (isBusy ? "Please wait for the current action to finish." : undefined)
                        : editMeta.reason
                },
                {
                    label: "Actions",
                    variant: "outline",
                    hasCaret: true,
                    disabled: selectedCount === 0 || isBusy,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint,
                    menuItems: actionMenuItems
                }
            ];
        case "active":
            return [
                {
                    label: "Edit",
                    variant: "primary",
                    disabled: !editMeta.canEdit || isBusy,
                    onClick: onEditSelected,
                    title: editMeta.canEdit
                        ? (isBusy ? "Please wait for the current action to finish." : undefined)
                        : editMeta.reason
                },
                {
                    label: "Sell similar",
                    variant: "outline",
                    disabled: selectedCount === 0 || isBusy,
                    onClick: onSellSimilar,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint
                },
                {
                    label: "Actions",
                    variant: "outline",
                    hasCaret: true,
                    disabled: selectedCount === 0 || isBusy,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint,
                    menuItems: actionMenuItems
                }
            ];
        case "unsold":
            return [
                {
                    label: "Relist",
                    variant: "primary",
                    disabled: selectedCount === 0 || isBusy,
                    onClick: onRelist,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint
                },
                {
                    label: "Relist as fixed price",
                    variant: "outline",
                    disabled: selectedCount === 0 || isBusy,
                    onClick: onRelistFixedPrice,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint
                },
                {
                    label: "Sell similar",
                    variant: "outline",
                    disabled: selectedCount === 0 || isBusy,
                    onClick: onSellSimilar,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint
                },
                {
                    label: "Actions",
                    variant: "outline",
                    hasCaret: true,
                    disabled: selectedCount === 0 || isBusy,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint,
                    menuItems: actionMenuItems
                }
            ];
        case "ended":
            return [
                {
                    label: "Relist",
                    variant: "primary",
                    disabled: selectedCount === 0 || isBusy,
                    onClick: onRelist,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint
                },
                {
                    label: "Relist as fixed price",
                    variant: "outline",
                    disabled: selectedCount === 0 || isBusy,
                    onClick: onRelistFixedPrice,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint
                },
                {
                    label: "Sell similar",
                    variant: "outline",
                    disabled: selectedCount === 0 || isBusy,
                    onClick: onSellSimilar,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint
                },
                {
                    label: "Actions",
                    variant: "outline",
                    hasCaret: true,
                    disabled: selectedCount === 0 || isBusy,
                    title: isBusy ? "Please wait for the current action to finish." : selectionHint,
                    menuItems: actionMenuItems
                }
            ];
        default:
            return [];
    }
};

const SOLD_STATUS_LABELS = { 0: "Sold", 1: "Unsold" };
const RELIST_STATUS_LABELS = { 0: "Relisted", 1: "Not relisted" };
const FORMAT_LABELS = { 1: "Auction", 2: "Buy It Now" };
const DURATION_LABELS = {
    0: "Good 'Til Cancelled",
    3: "3 days",
    5: "5 days",
    7: "7 days",
    10: "10 days"
};
const CURRENCY_FIELD_KEYS = ["CurrentPrice", "StartPrice", "ReservePrice", "ShippingCost", "BuyItNowPrice", "BestOfferAmount"];
const CURRENCY_FIELDS = new Set(CURRENCY_FIELD_KEYS);
const CURRENCY_FIELDS_NORMALIZED = new Set(CURRENCY_FIELD_KEYS.map((key) => key.toLowerCase()));
const RELIST_MODES = {
    sameFormat: 0,
    fixedPrice: 1
};
const HEADER_LABEL_MAP = {
    Sku: "Custom label (SKU)",
    Title: "Item",
    CurrentPrice: "Current price",
    Discounts: "Discount",
    AvailableQuantity: "Available quantity",
    SoldQuantity: "Sold quantity",
    StartPrice: "Start price",
    ReservePrice: "Reserve price",
    ShippingCost: "Shipping cost",
    Duration: "Duration",
    StartDate: "Start date",
    EndDate: "End date",
    BuyItNowPrice: "Buy it now price",
    SoldStatus: "Sold status",
    RelistStatus: "Relist status",
    Format: "Format",
    WatchersCount: "Watchers",
    BidsCount: "Bids",
    OffersCount: "Offers",
    BestOfferAmount: "Best Offer"
};

const COLUMN_CONFIG_STORAGE_KEY = "listing-table-config";

const readColumnConfigMap = () => {
    if (typeof window === "undefined") {
        return {};
    }

    try {
        const raw = window.localStorage.getItem(COLUMN_CONFIG_STORAGE_KEY);
        if (!raw) {
            return {};
        }

        const parsed = JSON.parse(raw);
        if (parsed && typeof parsed === "object") {
            return parsed;
        }
    } catch (error) {
        console.error("Failed to parse column config", error);
    }

    return {};
};

const loadStatusColumnConfig = (status) => {
    if (!status) {
        return null;
    }

    const map = readColumnConfigMap();
    const config = map[status];
    if (config && typeof config === "object") {
        return {
            order: Array.isArray(config.order) ? config.order : [],
            hidden: Array.isArray(config.hidden) ? config.hidden : []
        };
    }

    return null;
};

const saveStatusColumnConfig = (status, config) => {
    if (typeof window === "undefined" || !status) {
        return;
    }

    const map = readColumnConfigMap();
    map[status] = config;

    try {
        window.localStorage.setItem(COLUMN_CONFIG_STORAGE_KEY, JSON.stringify(map));
    } catch (error) {
        console.error("Failed to persist column config", error);
    }
};

const clearStatusColumnConfig = (status) => {
    if (typeof window === "undefined" || !status) {
        return;
    }

    const map = readColumnConfigMap();
    if (status in map) {
        delete map[status];
        const keys = Object.keys(map);
        if (keys.length === 0) {
            window.localStorage.removeItem(COLUMN_CONFIG_STORAGE_KEY);
        } else {
            window.localStorage.setItem(COLUMN_CONFIG_STORAGE_KEY, JSON.stringify(map));
        }
    }
};

const applyColumnConfig = (columns, config) => {
    if (!Array.isArray(columns) || columns.length === 0) {
        return [];
    }

    if (!config) {
        return columns.map((column) => ({ ...column }));
    }

    const hidden = new Set(Array.isArray(config.hidden) ? config.hidden : []);
    const order = Array.isArray(config.order) ? config.order : [];
    const orderIndex = new Map(order.map((key, index) => [key, index]));

    return columns
        .filter((column) => !hidden.has(column.key))
        .sort((a, b) => {
            const aIndex = orderIndex.has(a.key) ? orderIndex.get(a.key) : Number.MAX_SAFE_INTEGER;
            const bIndex = orderIndex.has(b.key) ? orderIndex.get(b.key) : Number.MAX_SAFE_INTEGER;
            return aIndex - bIndex;
        })
        .map((column) => ({ ...column }));
};

const buildCustomizeDraft = (columns, config) => {
    if (!Array.isArray(columns)) {
        return [];
    }

    const hidden = new Set(config?.hidden ?? []);
    const order = Array.isArray(config?.order) ? config.order : [];
    const columnsByKey = new Map(columns.map((column) => [column.key, column]));
    const seen = new Set();
    const ordered = [];

    order.forEach((key) => {
        if (columnsByKey.has(key) && !seen.has(key)) {
            ordered.push(columnsByKey.get(key));
            seen.add(key);
        }
    });

    columns.forEach((column) => {
        if (!seen.has(column.key)) {
            ordered.push(column);
            seen.add(column.key);
        }
    });

    return ordered.map((column) => ({
        key: column.key,
        label: column.label ?? column.key,
        enabled: !hidden.has(column.key)
    }));
};

const ToolbarDropdownButton = ({ label, variant, disabled, title, menuItems = [] }) => {
    const [isOpen, setIsOpen] = useState(false);
    const containerRef = useRef(null);

    useEffect(() => {
        if (!isOpen) {
            return;
        }

        const handleClickOutside = (event) => {
            if (containerRef.current && !containerRef.current.contains(event.target)) {
                setIsOpen(false);
            }
        };

        document.addEventListener("mousedown", handleClickOutside);
        return () => document.removeEventListener("mousedown", handleClickOutside);
    }, [isOpen]);

    const toggleMenu = () => {
        if (disabled || menuItems.length === 0) {
            return;
        }
        setIsOpen((prev) => !prev);
    };

    const handleSelect = (item) => {
        if (disabled || item.disabled) {
            return;
        }
        setIsOpen(false);
        item.onClick?.();
    };

    return (
        <div className="listing-dashboard__dropdown" ref={containerRef}>
            <button
                type="button"
                className={`listing-dashboard__action-btn listing-dashboard__action-btn--${variant}`}
                disabled={disabled || menuItems.length === 0}
                onClick={toggleMenu}
                title={title}
            >
                <span>{label}</span>
                <span className="listing-dashboard__action-caret">▾</span>
            </button>
            {isOpen && menuItems.length > 0 && (
                <div className="listing-dashboard__dropdown-menu">
                    {menuItems.map((item) => {
                        const itemClasses = ["listing-dashboard__dropdown-item"];
                        if (item.variant === "danger") {
                            itemClasses.push("listing-dashboard__dropdown-item--danger");
                        }

                        return (
                            <button
                                key={item.label}
                                type="button"
                                className={itemClasses.join(" ")}
                                onClick={() => handleSelect(item)}
                                disabled={item.disabled}
                                title={item.title}
                            >
                                {item.label}
                            </button>
                        );
                    })}
                </div>
            )}
        </div>
    );
};

const prettifyHeader = (key) => {
    if (!key) return "";
    const normalizedKey = key.charAt(0).toUpperCase() + key.slice(1);
    return HEADER_LABEL_MAP[normalizedKey] ?? HEADER_LABEL_MAP[key] ?? normalizedKey.replace(/([a-z])([A-Z])/g, "$1 $2");
};

const DATE_TIME_FORMATTER = new Intl.DateTimeFormat(undefined, {
    dateStyle: "medium",
    timeStyle: "short"
});

const formatDateTime = (value) => {
    if (!value) {
        return "-";
    }

    const date = value instanceof Date ? value : new Date(value);
    if (Number.isNaN(date.getTime())) {
        return String(value);
    }

    return DATE_TIME_FORMATTER.format(date);
};

const formatDraftExpiration = (value) => {
    const date = value instanceof Date ? value : new Date(value);
    if (Number.isNaN(date.getTime())) {
        return String(value ?? "-");
    }

    const diffMs = date.getTime() - Date.now();
    const oneDayMs = 1000 * 60 * 60 * 24;

    if (diffMs <= 0) {
        return "Expired";
    }

    const daysLeft = Math.ceil(diffMs / oneDayMs);
    return daysLeft === 1 ? "1 day left" : `${daysLeft} days left`;
};

const AllListingsPage = () => {
    const { statusSlug = "active" } = useParams();
    const navigate = useNavigate();

    const copy = useMemo(
        () => STATUS_COPY[statusSlug] ?? STATUS_COPY.active,
        [statusSlug]
    );

    const [isLoading, setIsLoading] = useState(false);
    const [paging, setPaging] = useState({ items: [], totalCount: 0, pageNumber: 1, pageSize: 20 });
    const [availableColumns, setAvailableColumns] = useState(TABLE_COLUMNS.map((c) => ({ ...c })));
    const [visibleColumns, setVisibleColumns] = useState(TABLE_COLUMNS.map((c) => ({ ...c })));
    const [searchInput, setSearchInput] = useState("");
    const [searchTerm, setSearchTerm] = useState("");
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(20);
    const [isPerformingAction, setIsPerformingAction] = useState(false);
    const [isProcessingCsv, setIsProcessingCsv] = useState(false);
    const [refreshKey, setRefreshKey] = useState(0);
    const [selectedIds, setSelectedIds] = useState([]);
    const [formatFilter, setFormatFilter] = useState("");
    const [stockFilter, setStockFilter] = useState("");
    const [isCustomizeOpen, setIsCustomizeOpen] = useState(false);
    const [customizeDraft, setCustomizeDraft] = useState([]);
    const headerCheckboxRef = useRef(null);
    const uploadInputRef = useRef(null);

    useEffect(() => {
        setSearchTerm("");
        setSearchInput("");
        setPageNumber(1);
        setPageSize(20);
        setSelectedIds([]);
        setFormatFilter("");
        setStockFilter("");
    }, [statusSlug]);

    useEffect(() => {
        let cancelled = false;

        const load = async () => {
            setIsLoading(true);
            try {
                const params = {
                    searchTerm: searchTerm || undefined,
                    pageNumber,
                    pageSize,
                    format: formatFilter || undefined,
                    outOfStock: stockFilter === "out" ? true : (stockFilter === "in" ? false : undefined)
                };

                const data = statusSlug === "unsold"
                    ? await ListingService.listUnsoldNotRelisted(params)
                    : await ListingService.listByStatus(statusSlug, params);

                if (cancelled) return;

                const items = Array.isArray(data?.items) ? data.items : [];

                let inferredColumns;

                if (items.length > 0) {
                    const first = items[0];
                    const inferred = Object.keys(first).filter((k) => !["Id", "id"].includes(k));
                    inferredColumns = inferred.map((k) => ({
                        key: k,
                        label: k === "Thumbnail" ? "Image" : prettifyHeader(k)
                    }));
                } else {
                    inferredColumns = TABLE_COLUMNS.map((c) => ({ ...c }));
                }

                setAvailableColumns(inferredColumns);

                const config = loadStatusColumnConfig(statusSlug);
                setVisibleColumns(applyColumnConfig(inferredColumns, config));

                setPaging({
                    items,
                    totalCount: data?.totalCount ?? data?.TotalCount ?? 0,
                    pageNumber: data?.pageNumber ?? data?.PageNumber ?? pageNumber,
                    pageSize: data?.pageSize ?? data?.PageSize ?? pageSize
                });

                setPageNumber(data?.pageNumber ?? data?.PageNumber ?? pageNumber);
                setPageSize(data?.pageSize ?? data?.PageSize ?? pageSize);
                setSelectedIds([]);
            } catch (error) {
                console.error("Failed to load listings", error);
                if (!cancelled) {
                    setPaging((prev) => ({ ...prev, items: [], totalCount: 0 }));
                    setSelectedIds([]);
                }
            } finally {
                if (!cancelled) {
                    setIsLoading(false);
                }
            }
        };

        load();

        return () => {
            cancelled = true;
        };
    }, [statusSlug, searchTerm, pageNumber, pageSize, formatFilter, stockFilter, refreshKey]);

    const handleSearchSubmit = (event) => {
        event.preventDefault();
        setPageNumber(1);
        setSearchTerm(searchInput.trim());
    };

    const handleClearSearch = () => {
        setSearchInput("");
        setSearchTerm("");
        setPageNumber(1);
    };

    const handlePrevPage = () => {
        setPageNumber((prev) => Math.max(1, prev - 1));
    };

    const handleNextPage = (totalPages) => {
        setPageNumber((prev) => {
            const next = Math.min(totalPages, prev + 1);
            return next === prev ? prev : next;
        });
    };

    const handlePageSizeChange = (event) => {
        const nextSize = Number(event.target.value) || 20;
        setPageSize(nextSize);
        setPageNumber(1);
    };

    const handleOpenCustomize = useCallback(() => {
        const config = loadStatusColumnConfig(statusSlug);
        setCustomizeDraft(buildCustomizeDraft(availableColumns, config));
        setIsCustomizeOpen(true);
    }, [availableColumns, statusSlug]);

    const handleCustomizeToggle = useCallback((key) => {
        setCustomizeDraft((prev) => prev.map((item) => (
            item.key === key
                ? { ...item, enabled: !item.enabled }
                : item
        )));
    }, []);

    const handleCustomizeReorder = useCallback((key, direction) => {
        setCustomizeDraft((prev) => {
            const index = prev.findIndex((item) => item.key === key);
            if (index === -1) {
                return prev;
            }

            const target = index + direction;
            if (target < 0 || target >= prev.length) {
                return prev;
            }

            const next = [...prev];
            const [moved] = next.splice(index, 1);
            next.splice(target, 0, moved);
            return next;
        });
    }, []);

    const handleCustomizeCancel = useCallback(() => {
        setIsCustomizeOpen(false);
    }, []);

    const handleCustomizeReset = useCallback(() => {
        clearStatusColumnConfig(statusSlug);
        setCustomizeDraft(buildCustomizeDraft(availableColumns, null));
        setVisibleColumns(applyColumnConfig(availableColumns, null));
    }, [availableColumns, statusSlug]);

    const handleCustomizeSave = useCallback(() => {
        const visibleDraft = customizeDraft.filter((item) => item.enabled);
        if (visibleDraft.length === 0) {
            Notice({
                msg: "Choose at least one column",
                desc: "Keep one or more columns visible to continue.",
                isSuccess: false
            });
            return;
        }

        const config = {
            order: customizeDraft.map((item) => item.key),
            hidden: customizeDraft.filter((item) => !item.enabled).map((item) => item.key)
        };

        saveStatusColumnConfig(statusSlug, config);
        setVisibleColumns(applyColumnConfig(availableColumns, config));
        setIsCustomizeOpen(false);
    }, [availableColumns, customizeDraft, statusSlug]);

    useEffect(() => {
        if (!isCustomizeOpen) {
            return;
        }

        const config = loadStatusColumnConfig(statusSlug);
        setCustomizeDraft(buildCustomizeDraft(availableColumns, config));
    }, [availableColumns, isCustomizeOpen, statusSlug]);

    const rows = useMemo(
        () => (Array.isArray(paging.items) ? paging.items : []),
        [paging.items]
    );
    const customizeHasVisible = useMemo(
        () => customizeDraft.some((item) => item.enabled),
        [customizeDraft]
    );
    const totalPages = Math.max(1, Math.ceil((paging.totalCount ?? 0) / (pageSize || 1)));

    const allRowIds = useMemo(() => {
        if (!Array.isArray(rows)) {
            return [];
        }
        const seen = new Set();
        const result = [];
        rows.forEach((row) => {
            const id = extractListingId(row);
            if (id && !seen.has(id)) {
                seen.add(id);
                result.push(id);
            }
        });
        return result;
    }, [rows]);

    const selectedRows = useMemo(() => {
        if (!selectedIds.length) {
            return [];
        }
        const idSet = new Set(selectedIds);
        return rows.filter((row) => {
            const id = extractListingId(row);
            return id ? idSet.has(id) : false;
        });
    }, [rows, selectedIds]);

    const selectedIdSet = useMemo(() => new Set(selectedIds), [selectedIds]);

    useEffect(() => {
        const checkbox = headerCheckboxRef.current;
        if (!checkbox) {
            return;
        }

        if (!rows.length) {
            checkbox.checked = false;
            checkbox.indeterminate = false;
            return;
        }

        if (selectedIds.length === 0) {
            checkbox.checked = false;
            checkbox.indeterminate = false;
            return;
        }

        if (selectedIds.length === allRowIds.length && allRowIds.length > 0) {
            checkbox.checked = true;
            checkbox.indeterminate = false;
            return;
        }

        checkbox.checked = false;
        checkbox.indeterminate = true;
    }, [allRowIds.length, rows.length, selectedIds.length]);

    const handleToggleRow = useCallback((row) => {
        const id = extractListingId(row);
        if (!id) {
            return;
        }

        setSelectedIds((prev) => (
            prev.includes(id)
                ? prev.filter((value) => value !== id)
                : [...prev, id]
        ));
    }, []);

    const handleToggleAll = useCallback(() => {
        if (!allRowIds.length) {
            setSelectedIds([]);
            return;
        }

        if (selectedIds.length === allRowIds.length) {
            setSelectedIds([]);
        } else {
            setSelectedIds(allRowIds);
        }
    }, [allRowIds, selectedIds.length]);

    const handleRowEdit = useCallback((row) => {
        const evaluation = evaluateEditRule(statusSlug, row);
        if (!evaluation.canEdit) {
            return;
        }

        const id = extractListingId(row);
        if (!id) {
            return;
        }

        navigate(`/listing-form/${encodeURIComponent(id)}`);
    }, [navigate, statusSlug]);

    const handleEditSelected = useCallback(() => {
        if (selectedRows.length !== 1) {
            return;
        }
        handleRowEdit(selectedRows[0]);
    }, [handleRowEdit, selectedRows]);

    const handleBulkDeleteDrafts = useCallback(async () => {
        if (isPerformingAction || selectedIds.length === 0) {
            return;
        }

        setIsPerformingAction(true);
        try {
            const result = await ListingService.bulkDeleteDrafts(selectedIds);
            const deleted = result?.deletedCount ?? 0;
            const failures = Array.isArray(result?.failures) ? result.failures : [];
            const failedCount = failures.length;

            if (deleted > 0) {
                Notice({
                    msg: `Deleted ${deleted} draft${deleted > 1 ? "s" : ""}.`
                });
            }

            if (failedCount > 0) {
                const summary = failures
                    .slice(0, 3)
                    .map((failure) => failure?.message || failure?.code)
                    .filter(Boolean)
                    .join(" • ");
                const overflow = failedCount > 3 ? ` • +${failedCount - 3} more` : "";

                Notice({
                    msg: `Could not delete ${failedCount} draft${failedCount > 1 ? "s" : ""}.`,
                    desc: summary ? `${summary}${overflow}` : undefined,
                    isSuccess: false
                });
            }

            if (deleted === 0 && failedCount === 0) {
                Notice({
                    msg: "Nothing deleted",
                    desc: "No draft listings matched your selection.",
                    isSuccess: false
                });
            }

            setSelectedIds([]);
            const shouldMoveBack = deleted > 0 && deleted >= rows.length && pageNumber > 1;
            if (shouldMoveBack) {
                setPageNumber((prev) => Math.max(1, prev - 1));
            } else {
                setRefreshKey((prev) => prev + 1);
            }
        } catch (error) {
            console.error("Failed to delete drafts", error);
            Notice({
                msg: "Failed to delete drafts",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
        } finally {
            setIsPerformingAction(false);
        }
    }, [isPerformingAction, pageNumber, rows.length, selectedIds]);

    const handleBulkCopyDrafts = useCallback(async () => {
        if (isPerformingAction || selectedIds.length === 0) {
            return;
        }

        setIsPerformingAction(true);
        try {
            const result = await ListingService.bulkCopyDrafts(selectedIds);
            const copied = result?.copiedCount ?? 0;
            const failures = Array.isArray(result?.failures) ? result.failures : [];
            const failedCount = failures.length;

            if (copied > 0) {
                Notice({
                    msg: `Created ${copied} draft cop${copied > 1 ? "ies" : "y"}.`
                });
            }

            if (failedCount > 0) {
                const summary = failures
                    .slice(0, 3)
                    .map((failure) => failure?.message || failure?.code)
                    .filter(Boolean)
                    .join(" • ");
                const overflow = failedCount > 3 ? ` • +${failedCount - 3} more` : "";

                Notice({
                    msg: `Could not copy ${failedCount} draft${failedCount > 1 ? "s" : ""}.`,
                    desc: summary ? `${summary}${overflow}` : undefined,
                    isSuccess: false
                });
            }

            if (copied === 0 && failedCount === 0) {
                Notice({
                    msg: "Nothing copied",
                    desc: "No draft listings matched your selection.",
                    isSuccess: false
                });
            }

            setSelectedIds([]);
            setRefreshKey((prev) => prev + 1);
        } catch (error) {
            console.error("Failed to copy drafts", error);
            Notice({
                msg: "Failed to copy drafts",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
        } finally {
            setIsPerformingAction(false);
        }
    }, [isPerformingAction, selectedIds]);

    const handleDownload = useCallback(async () => {
        if (isProcessingCsv) {
            return;
        }

        if (!rows.length) {
            Notice({
                msg: "Nothing to export",
                desc: "Select at least one listing or ensure the current view has results.",
                isSuccess: false
            });
            return;
        }

        if (typeof document === "undefined") {
            return;
        }

        const selectedIdsForExport = selectedRows
            .map((row) => extractListingId(row))
            .filter((id) => typeof id === "string" && id.trim().length > 0);

        const params = {
            status: statusSlug,
            searchTerm: searchTerm || undefined
        };

        if (selectedIdsForExport.length > 0) {
            params.listingIds = selectedIdsForExport;
        }

        setIsProcessingCsv(true);

        try {
            const blob = await ListingService.exportCsv(params);

            if (!(blob instanceof Blob)) {
                throw new Error("CSV download failed.");
            }

            const fileName = `listings-${statusSlug}-${Date.now()}.csv`;
            const url = URL.createObjectURL(blob);
            const anchor = document.createElement("a");
            anchor.href = url;
            anchor.download = fileName;
            document.body.appendChild(anchor);
            anchor.click();
            document.body.removeChild(anchor);
            URL.revokeObjectURL(url);

            const exportedCount = selectedIdsForExport.length;
            const message = exportedCount > 0
                ? `Exported ${exportedCount} listing${exportedCount === 1 ? "" : "s"} to CSV.`
                : "CSV export ready for the current filters.";

            Notice({ msg: message });
        } catch (error) {
            console.error("Failed to export listings", error);
            Notice({
                msg: "Failed to export CSV",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
        } finally {
            setIsProcessingCsv(false);
        }
    }, [isProcessingCsv, rows, searchTerm, selectedRows, statusSlug]);

    const performRelist = useCallback(async (listingIds, mode = RELIST_MODES.sameFormat) => {
        if (!Array.isArray(listingIds) || listingIds.length === 0) {
            return { count: 0, listingIds: [] };
        }

        const label = mode === RELIST_MODES.fixedPrice ? "relist as fixed price" : "relist";

        try {
            const result = await ListingService.relist(listingIds, mode);
            const relistedCountRaw = Number(result?.relistedCount ?? result?.RelistedCount ?? 0);
            const relistedCount = Number.isFinite(relistedCountRaw) && relistedCountRaw > 0
                ? relistedCountRaw
                : listingIds.length;
            const createdIds = Array.isArray(result?.listingIds)
                ? result.listingIds
                : Array.isArray(result?.ListingIds)
                    ? result.ListingIds
                    : [];

            const successVerb = mode === RELIST_MODES.fixedPrice ? "Relisted as fixed price" : "Relisted";
            Notice({
                msg: `${successVerb} ${relistedCount} listing${relistedCount === 1 ? "" : "s"}.`
            });

            return { count: relistedCount, listingIds: createdIds };
        } catch (error) {
            console.error("Failed to relist listings", error);
            Notice({
                msg: `Failed to ${label} listings`,
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
            throw error;
        }
    }, []);

    const performSellSimilar = useCallback(async (listingIds) => {
        if (!Array.isArray(listingIds) || listingIds.length === 0) {
            return { count: 0, listingIds: [] };
        }

        try {
            const result = await ListingService.sellSimilar(listingIds);
            const createdCountRaw = Number(result?.createdCount ?? result?.CreatedCount ?? 0);
            const createdCount = Number.isFinite(createdCountRaw) && createdCountRaw > 0
                ? createdCountRaw
                : listingIds.length;
            const createdIds = Array.isArray(result?.listingIds)
                ? result.listingIds
                : Array.isArray(result?.ListingIds)
                    ? result.ListingIds
                    : [];

            Notice({
                msg: `Created ${createdCount} draft${createdCount === 1 ? "" : "s"} via Sell similar.`
            });

            return { count: createdCount, listingIds: createdIds };
        } catch (error) {
            console.error("Failed to create sell-similar drafts", error);
            Notice({
                msg: "Failed to create Sell similar drafts",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
            throw error;
        }
    }, []);

    const handleRelistSelected = useCallback(async (mode = RELIST_MODES.sameFormat) => {
        if (isPerformingAction || selectedIds.length === 0) {
            return;
        }

        if (!["unsold", "ended"].includes(statusSlug)) {
            Notice({
                msg: "Relist is only available on the unsold or ended tabs.",
                isSuccess: false
            });
            return;
        }

        setIsPerformingAction(true);
        try {
            await performRelist(selectedIds, mode);
            setSelectedIds([]);
            setRefreshKey((prev) => prev + 1);
        } catch (error) {
            // Errors already surfaced through Notice.
        } finally {
            setIsPerformingAction(false);
        }
    }, [isPerformingAction, performRelist, selectedIds, statusSlug]);

    const handleSellSimilarSelected = useCallback(async () => {
        if (isPerformingAction || selectedIds.length === 0) {
            return;
        }

        if (!["active", "unsold", "ended"].includes(statusSlug)) {
            Notice({
                msg: "Sell similar is available on active, unsold, or ended listings.",
                isSuccess: false
            });
            return;
        }

        setIsPerformingAction(true);
        try {
            const { count, listingIds } = await performSellSimilar(selectedIds);
            setSelectedIds([]);

            if (count === 1 && listingIds.length > 0) {
                navigate(`/listing-form/${encodeURIComponent(listingIds[0])}`);
                return;
            }

            setRefreshKey((prev) => prev + 1);
        } catch (error) {
            // handled via Notice
        } finally {
            setIsPerformingAction(false);
        }
    }, [isPerformingAction, performSellSimilar, selectedIds, statusSlug]);

    const handleEndListing = useCallback(async () => {
        if (isPerformingAction) {
            return;
        }

        if (selectedIds.length === 0) {
            Notice({
                msg: "Select at least one listing",
                desc: "Choose which listings to end before continuing.",
                isSuccess: false
            });
            return;
        }

        if (statusSlug !== "active") {
            Notice({
                msg: "End listing is only available on active listings",
                desc: "Switch to the Active tab to end listings.",
                isSuccess: false
            });
            return;
        }

        setIsPerformingAction(true);
        try {
            const result = await ListingService.endListings(selectedIds);

            const endedCountRaw = Number(result?.endedCount ?? result?.EndedCount ?? 0);
            const returnedIds = Array.isArray(result?.listingIds)
                ? result.listingIds
                : Array.isArray(result?.ListingIds)
                    ? result.ListingIds
                    : [];
            const endedCount = Number.isFinite(endedCountRaw) && endedCountRaw > 0
                ? endedCountRaw
                : returnedIds.length;

            if (endedCount > 0) {
                Notice({
                    msg: `Ended ${endedCount} listing${endedCount === 1 ? "" : "s"}.`
                });
            }

            const failures = normalizeFailures(result?.failures ?? result?.Failures);
            const { total: failedCount, summary } = buildFailureSummary(failures);

            if (failedCount > 0) {
                Notice({
                    msg: `Could not end ${failedCount} listing${failedCount === 1 ? "" : "s"}.`,
                    desc: summary || undefined,
                    isSuccess: false
                });
            }

            if (endedCount === 0 && failedCount === 0) {
                Notice({
                    msg: "No listings were ended",
                    desc: "None of the selected listings met the criteria to end.",
                    isSuccess: false
                });
            }

            setSelectedIds([]);
            if (endedCount > 0) {
                setRefreshKey((prev) => prev + 1);
            }
        } catch (error) {
            console.error("Failed to end listings", error);
            Notice({
                msg: "Failed to end listings",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
        } finally {
            setIsPerformingAction(false);
        }
    }, [isPerformingAction, selectedIds, statusSlug]);

    const handleSendOffers = useCallback(async () => {
        if (isPerformingAction) {
            return;
        }

        if (selectedIds.length === 0) {
            Notice({
                msg: "Select listings to send offers",
                desc: "Pick one or more listings before sending an offer to interested buyers.",
                isSuccess: false
            });
            return;
        }

        if (statusSlug !== "active") {
            Notice({
                msg: "Send offers is limited to active listings",
                desc: "Switch to the Active tab to reach interested buyers.",
                isSuccess: false
            });
            return;
        }

        setIsPerformingAction(true);
        try {
            const result = await ListingService.sendOffers(selectedIds);
            const queuedCount = Number(result?.queuedCount ?? result?.QueuedCount ?? 0) || 0;

            if (queuedCount > 0) {
                Notice({
                    msg: `Queued offers for ${queuedCount} listing${queuedCount === 1 ? "" : "s"}.`
                });
            }

            const failures = normalizeFailures(result?.failures ?? result?.Failures);
            const { total: failedCount, summary } = buildFailureSummary(failures);

            if (failedCount > 0) {
                Notice({
                    msg: `Could not send offers for ${failedCount} listing${failedCount === 1 ? "" : "s"}.`,
                    desc: summary || undefined,
                    isSuccess: false
                });
            }

            if (queuedCount === 0 && failedCount === 0) {
                Notice({
                    msg: "No offers were queued",
                    desc: "The selected listings are not eligible for offers yet.",
                    isSuccess: false
                });
            }

            setSelectedIds([]);
        } catch (error) {
            console.error("Failed to send offers", error);
            Notice({
                msg: "Failed to send offers",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
        } finally {
            setIsPerformingAction(false);
        }
    }, [isPerformingAction, selectedIds, statusSlug]);

    const handlePromoteListing = useCallback(async () => {
        if (isPerformingAction) {
            return;
        }

        if (selectedIds.length === 0) {
            Notice({
                msg: "Select listings to promote",
                desc: "Pick at least one listing before creating a promotion.",
                isSuccess: false
            });
            return;
        }

        if (statusSlug !== "active") {
            Notice({
                msg: "Promotions are only available for active listings",
                desc: "Switch to the Active tab to surface your items.",
                isSuccess: false
            });
            return;
        }

        setIsPerformingAction(true);
        try {
            const result = await ListingService.promoteListings(selectedIds);
            const submittedCount = Number(result?.submittedCount ?? result?.SubmittedCount ?? 0) || 0;

            if (submittedCount > 0) {
                Notice({
                    msg: `Promotion submitted for ${submittedCount} listing${submittedCount === 1 ? "" : "s"}.`
                });
            }

            const failures = normalizeFailures(result?.failures ?? result?.Failures);
            const { total: failedCount, summary } = buildFailureSummary(failures);

            if (failedCount > 0) {
                Notice({
                    msg: `Could not promote ${failedCount} listing${failedCount === 1 ? "" : "s"}.`,
                    desc: summary || undefined,
                    isSuccess: false
                });
            }

            if (submittedCount === 0 && failedCount === 0) {
                Notice({
                    msg: "No promotions submitted",
                    desc: "The selected listings require additional configuration before promotion.",
                    isSuccess: false
                });
            }

            setSelectedIds([]);
        } catch (error) {
            console.error("Failed to promote listings", error);
            Notice({
                msg: "Failed to promote listings",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
        } finally {
            setIsPerformingAction(false);
        }
    }, [isPerformingAction, selectedIds, statusSlug]);

    const handleAddToSaleEvent = useCallback(() => {
        if (selectedIds.length === 0) {
            Notice({
                msg: "Select listings to add",
                desc: "Choose the listings you want to include in a sale event.",
                isSuccess: false
            });
            return;
        }

        navigate("/marketing/sale-events/create", {
            state: {
                listingIds: selectedIds.slice()
            }
        });
    }, [navigate, selectedIds]);

    const handleEndSchedule = useCallback(() => {
        if (selectedIds.length === 0) {
            Notice({
                msg: "Select scheduled listings",
                desc: "Pick the scheduled listings you want to end early.",
                isSuccess: false
            });
            return;
        }

        Notice({
            msg: "End schedule not implemented",
            desc: "Hook up the scheduler service to cancel upcoming listings like eBay's flow.",
            isSuccess: false
        });
    }, [selectedIds]);

    const handleSendToDrafts = useCallback(async (sourceLabel = "selected listings") => {
        if (isPerformingAction) {
            return;
        }

        if (selectedIds.length === 0) {
            Notice({
                msg: "Select listings to move",
                desc: "Choose the listings that should be copied into drafts.",
                isSuccess: false
            });
            return;
        }

        setIsPerformingAction(true);
        try {
            const result = await ListingService.sendToDrafts(selectedIds);
            const createdCountRaw = Number(result?.createdCount ?? result?.CreatedCount ?? 0);
            const createdIds = Array.isArray(result?.listingIds)
                ? result.listingIds
                : Array.isArray(result?.ListingIds)
                    ? result.ListingIds
                    : [];
            const createdCount = Number.isFinite(createdCountRaw) && createdCountRaw > 0
                ? createdCountRaw
                : createdIds.length;

            if (createdCount > 0) {
                Notice({
                    msg: `Copied ${createdCount} ${createdCount === 1 ? "listing" : "listings"} from ${sourceLabel} into drafts.`
                });
            }

            const failures = normalizeFailures(result?.failures ?? result?.Failures);
            const { total: failedCount, summary } = buildFailureSummary(failures);

            if (failedCount > 0) {
                Notice({
                    msg: `Could not move ${failedCount} listing${failedCount === 1 ? "" : "s"} to drafts.`,
                    desc: summary || undefined,
                    isSuccess: false
                });
            }

            if (createdCount === 0 && failedCount === 0) {
                Notice({
                    msg: "No listings copied to drafts",
                    desc: "The selected listings are not eligible to be duplicated as drafts.",
                    isSuccess: false
                });
            }

            setSelectedIds([]);
            if (createdCount > 0) {
                setRefreshKey((prev) => prev + 1);
            }
        } catch (error) {
            console.error("Failed to send listings to drafts", error);
            Notice({
                msg: "Failed to send listings to drafts",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
        } finally {
            setIsPerformingAction(false);
        }
    }, [isPerformingAction, selectedIds]);

    const handleArchiveListings = useCallback(async () => {
        if (isPerformingAction) {
            return;
        }

        if (selectedIds.length === 0) {
            Notice({
                msg: "Select listings to archive",
                desc: "Pick which ended listings to archive before continuing.",
                isSuccess: false
            });
            return;
        }

        if (!["unsold", "ended"].includes(statusSlug)) {
            Notice({
                msg: "Archive is only available for ended listings",
                desc: "Switch to the Ended tab to move items into archive.",
                isSuccess: false
            });
            return;
        }

        setIsPerformingAction(true);
        try {
            const result = await ListingService.archiveListings(selectedIds);
            const archivedCountRaw = Number(result?.archivedCount ?? result?.ArchivedCount ?? 0);
            const archivedIds = Array.isArray(result?.listingIds)
                ? result.listingIds
                : Array.isArray(result?.ListingIds)
                    ? result.ListingIds
                    : [];
            const archivedCount = Number.isFinite(archivedCountRaw) && archivedCountRaw > 0
                ? archivedCountRaw
                : archivedIds.length;

            if (archivedCount > 0) {
                Notice({
                    msg: `Archived ${archivedCount} listing${archivedCount === 1 ? "" : "s"}.`
                });
            }

            const failures = normalizeFailures(result?.failures ?? result?.Failures);
            const { total: failedCount, summary } = buildFailureSummary(failures);

            if (failedCount > 0) {
                Notice({
                    msg: `Could not archive ${failedCount} listing${failedCount === 1 ? "" : "s"}.`,
                    desc: summary || undefined,
                    isSuccess: false
                });
            }

            if (archivedCount === 0 && failedCount === 0) {
                Notice({
                    msg: "No listings archived",
                    desc: "Only ended listings can be archived.",
                    isSuccess: false
                });
            }

            setSelectedIds([]);
            if (archivedCount > 0) {
                setRefreshKey((prev) => prev + 1);
            }
        } catch (error) {
            console.error("Failed to archive listings", error);
            Notice({
                msg: "Failed to archive listings",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
        } finally {
            setIsPerformingAction(false);
        }
    }, [isPerformingAction, selectedIds, statusSlug]);

    const handleRelistWithChanges = useCallback(() => {
        if (selectedRows.length !== 1) {
            Notice({
                msg: "Select one listing",
                desc: "Relist with changes works on a single listing at a time.",
                isSuccess: false
            });
            return;
        }

        handleRowEdit(selectedRows[0]);
    }, [handleRowEdit, selectedRows]);

    const actionMenuItems = useMemo(() => {
        const hasSelection = selectedIds.length > 0;
        const busy = isPerformingAction;

        switch (statusSlug) {
            case "active":
                return [
                    {
                        label: "End listing",
                        onClick: handleEndListing,
                        disabled: !hasSelection || busy,
                        variant: "danger"
                    },
                    {
                        label: "Revise",
                        onClick: handleEditSelected,
                        disabled: selectedRows.length !== 1 || busy
                    },
                    {
                        label: "Send offers",
                        onClick: handleSendOffers,
                        disabled: !hasSelection || busy
                    },
                    {
                        label: "Promote listing",
                        onClick: handlePromoteListing,
                        disabled: !hasSelection || busy
                    },
                    {
                        label: "Add to sale event",
                        onClick: handleAddToSaleEvent,
                        disabled: !hasSelection || busy
                    }
                ];
            case "scheduled":
                return [
                    {
                        label: "Edit",
                        onClick: handleEditSelected,
                        disabled: selectedRows.length !== 1 || busy
                    },
                    {
                        label: "End schedule",
                        onClick: handleEndSchedule,
                        disabled: !hasSelection || busy,
                        variant: "danger"
                    },
                    {
                        label: "Send to drafts",
                        onClick: () => handleSendToDrafts("scheduled listings"),
                        disabled: !hasSelection || busy
                    }
                ];
            case "unsold":
                return [
                    {
                        label: "Relist with changes",
                        onClick: handleRelistWithChanges,
                        disabled: selectedRows.length !== 1 || busy
                    },
                    {
                        label: "Sell similar",
                        onClick: handleSellSimilarSelected,
                        disabled: !hasSelection || busy
                    },
                    {
                        label: "Send to drafts",
                        onClick: () => handleSendToDrafts("unsold listings"),
                        disabled: !hasSelection || busy
                    },
                    {
                        label: "Archive",
                        onClick: handleArchiveListings,
                        disabled: !hasSelection || busy,
                        variant: "danger"
                    }
                ];
            case "ended":
                return [
                    {
                        label: "Relist with changes",
                        onClick: handleRelistWithChanges,
                        disabled: selectedRows.length !== 1 || busy
                    },
                    {
                        label: "Sell similar",
                        onClick: handleSellSimilarSelected,
                        disabled: !hasSelection || busy
                    },
                    {
                        label: "Send to drafts",
                        onClick: () => handleSendToDrafts("ended listings"),
                        disabled: !hasSelection || busy
                    },
                    {
                        label: "Archive",
                        onClick: handleArchiveListings,
                        disabled: !hasSelection || busy,
                        variant: "danger"
                    }
                ];
            default:
                return [];
        }
    }, [
        handleAddToSaleEvent,
        handleArchiveListings,
        handleEditSelected,
        handleEndListing,
        handleEndSchedule,
        handlePromoteListing,
        handleRelistWithChanges,
        handleSellSimilarSelected,
        handleSendOffers,
        handleSendToDrafts,
        isPerformingAction,
        selectedIds.length,
        selectedRows.length,
        statusSlug
    ]);

    const handleUploadClick = useCallback(() => {
        if (isProcessingCsv) {
            return;
        }

        if (uploadInputRef.current) {
            uploadInputRef.current.click();
        }
    }, [isProcessingCsv]);

    // Accepts CSV uploads generated from the export endpoint to update price/quantity fields in bulk.
    const handleUploadFile = useCallback(async (event) => {
        const file = event?.target?.files?.[0];
        if (!file) {
            return;
        }

        if (isProcessingCsv) {
            if (uploadInputRef.current) {
                uploadInputRef.current.value = "";
            }
            return;
        }

        setIsProcessingCsv(true);

        try {
            const result = await ListingService.importCsv(file);
            const updatedCount = Number(result?.updatedCount ?? result?.UpdatedCount ?? 0);
            const rawFailures = Array.isArray(result?.failures)
                ? result.failures
                : Array.isArray(result?.Failures)
                    ? result.Failures
                    : [];

            const failures = rawFailures.map((failure) => ({
                rowNumber: failure?.rowNumber ?? failure?.RowNumber ?? null,
                listingId: failure?.listingId ?? failure?.ListingId ?? null,
                message: failure?.message ?? failure?.Message ?? "Unexpected error."
            }));

            if (updatedCount > 0) {
                Notice({
                    msg: `Updated ${updatedCount} listing${updatedCount === 1 ? "" : "s"} from CSV.`
                });
            }

            if (failures.length > 0) {
                const summary = failures
                    .slice(0, 3)
                    .map((failure) => {
                        const prefix = failure.rowNumber
                            ? `Row ${failure.rowNumber}`
                            : failure.listingId
                                ? `Listing ${failure.listingId}`
                                : "Row";
                        return `${prefix}: ${failure.message}`;
                    })
                    .filter(Boolean)
                    .join(" • ");
                const overflow = failures.length > 3 ? ` • +${failures.length - 3} more` : "";

                Notice({
                    msg: `Could not update ${failures.length} row${failures.length === 1 ? "" : "s"}.`,
                    desc: summary ? `${summary}${overflow}` : undefined,
                    isSuccess: false
                });
            }

            if (updatedCount === 0 && failures.length === 0) {
                Notice({
                    msg: "No changes detected",
                    desc: "Upload contained no valid updates for the current listings.",
                    isSuccess: false
                });
            }

            if (updatedCount > 0) {
                setRefreshKey((prev) => prev + 1);
            }
            setSelectedIds([]);
        } catch (error) {
            console.error("Failed to import CSV", error);
            Notice({
                msg: "Failed to import CSV",
                desc: error?.response?.data?.detail ?? error?.message ?? "Unexpected error.",
                isSuccess: false
            });
        } finally {
            setIsProcessingCsv(false);
            if (uploadInputRef.current) {
                uploadInputRef.current.value = "";
            }
        }
    }, [isProcessingCsv]);

    const toolbarActions = useMemo(
        () => buildStatusToolbarActions(statusSlug, {
            selectedCount: selectedRows.length,
            selectedRows,
            onEditSelected: handleEditSelected,
            onDeleteDrafts: handleBulkDeleteDrafts,
            onCopyDrafts: handleBulkCopyDrafts,
            onRelist: () => handleRelistSelected(RELIST_MODES.sameFormat),
            onRelistFixedPrice: () => handleRelistSelected(RELIST_MODES.fixedPrice),
            onSellSimilar: handleSellSimilarSelected,
            isBusy: isPerformingAction,
            actionMenuItems
        }),
        [actionMenuItems, handleBulkCopyDrafts, handleBulkDeleteDrafts, handleEditSelected, handleRelistSelected, handleSellSimilarSelected, isPerformingAction, selectedRows, statusSlug]
    );

    const renderCell = (row, key) => {
        const val = row[key];
        if (val === null || val === undefined || val === "") {
            return "-";
        }

        const keyLower = key.toLowerCase();

        if (keyLower.includes("thumbnail") || keyLower.includes("image")) {
            return (
                <img
                    src={val}
                    alt={row.Title ?? row.title ?? ""}
                    style={{ width: 56, height: 56, objectFit: "cover", borderRadius: 6 }}
                />
            );
        }

        if (statusSlug === "drafts" && keyLower === "expiredat") {
            return formatDraftExpiration(val);
        }

        if (keyLower.includes("date") || (keyLower.endsWith("at") && !keyLower.endsWith("format")) || keyLower.endsWith("time")) {
            return formatDateTime(val);
        }

        if (keyLower === "soldstatus") {
            const normalized = typeof val === "string" ? val : Number.isFinite(val) ? val : String(val);
            const asNumber = Number(val);
            return SOLD_STATUS_LABELS[val] ?? SOLD_STATUS_LABELS[asNumber] ?? normalized;
        }

        if (keyLower === "reliststatus") {
            const normalized = typeof val === "string" ? val : Number.isFinite(val) ? val : String(val);
            const asNumber = Number(val);
            return RELIST_STATUS_LABELS[val] ?? RELIST_STATUS_LABELS[asNumber] ?? normalized;
        }

        if (keyLower === "format") {
            const normalized = typeof val === "string" ? val : Number.isFinite(val) ? val : String(val);
            const asNumber = Number(val);
            return FORMAT_LABELS[val] ?? FORMAT_LABELS[asNumber] ?? normalized;
        }

        if (keyLower === "duration") {
            const normalized = typeof val === "string" ? val : Number.isFinite(val) ? val : String(val);
            const asNumber = Number(val);
            return DURATION_LABELS[val] ?? DURATION_LABELS[asNumber] ?? normalized;
        }

        if (keyLower === "offerscount" || keyLower === "bestofferamount" || keyLower === "bidscount") {
            const rowId = extractListingId(row);
            const asNumber = Number(val);
            if (asNumber > 0 || keyLower === "bestofferamount") {
                let displayVal = val.toLocaleString();
                let targetPath = `/marketing/offers?listingId=${encodeURIComponent(rowId)}`;

                if (keyLower === "bestofferamount") {
                    displayVal = asNumber.toLocaleString(undefined, { style: "currency", currency: "USD" });
                    targetPath = `/marketing/offers?listingId=${encodeURIComponent(rowId)}`;
                } else if (keyLower === "bidscount") {
                    targetPath = `/marketing/bids?listingId=${encodeURIComponent(rowId)}`;
                }

                return (
                    <button
                        type="button"
                        onClick={(e) => {
                            e.stopPropagation();
                            navigate(targetPath);
                        }}
                        style={{ color: "#0041b2", textDecoration: "underline", background: "none", border: "none", padding: 0, cursor: "pointer", fontWeight: "600" }}
                    >
                        {displayVal}
                    </button>
                );
            }
        }

        if (keyLower === "currentprice" || keyLower === "startprice") {
            const formatVal = row.Format ?? row.format;
            const isAuction = formatVal === 1 || String(formatVal).toLowerCase() === "auction";
            const binPrice = row.BuyItNowPrice ?? row.buyItNowPrice;

            if (isAuction && typeof binPrice === "number" && binPrice > 0) {
                return (
                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                        <span>{Number(val).toLocaleString(undefined, { style: "currency", currency: "USD" })}</span>
                        <span style={{ fontSize: '12px', color: '#707070', whiteSpace: 'nowrap' }}>(BIN: {binPrice.toLocaleString(undefined, { style: "currency", currency: "USD" })})</span>
                    </div>
                );
            }
        }

        if (typeof val === "number") {
            if (CURRENCY_FIELDS.has(key) || CURRENCY_FIELDS_NORMALIZED.has(keyLower)) {
                return val.toLocaleString(undefined, { style: "currency", currency: "USD" });
            }
            return val.toLocaleString();
        }

        return String(val);
    };

    return (
        <div className="listing-dashboard">
            <header className="listing-dashboard__header">
                <div className="listing-dashboard__heading-row">
                    <div className="listing-dashboard__page-title">
                        <h1>{copy.heading}</h1>
                        <p>{copy.subtitle}</p>
                    </div>
                    <button
                        type="button"
                        className="listing-dashboard__cta"
                        onClick={() => navigate("/listing-form")}
                    >
                        Create listing
                    </button>
                </div>
            </header>

            <section className="listing-dashboard__toolbar">
                <div className="listing-dashboard__search-row">
                    <form className="listing-dashboard__search" onSubmit={handleSearchSubmit}>
                        <svg
                            className="listing-dashboard__search-icon"
                            width="16"
                            height="16"
                            viewBox="0 0 20 20"
                            aria-hidden="true"
                        >
                            <path
                                d="M8.75 2a6.75 6.75 0 1 1 0 13.5A6.75 6.75 0 0 1 8.75 2Zm0 1.5a5.25 5.25 0 1 0 0 10.5 5.25 5.25 0 0 0 0-10.5Zm6.22 9.72 3.53 3.53-1.06 1.06-3.53-3.53 1.06-1.06Z"
                                fill="currentColor"
                            />
                        </svg>
                        <input
                            id="listing-dashboard-search"
                            type="search"
                            placeholder="Search by title, SKU, or item number"
                            autoComplete="off"
                            value={searchInput}
                            onChange={(event) => setSearchInput(event.target.value)}
                        />
                        <div className="listing-dashboard__search-actions">
                            <button type="submit">Search</button>
                            {searchTerm && (
                                <button type="button" onClick={handleClearSearch}>
                                    Clear
                                </button>
                            )}
                        </div>
                    </form>

                    {statusSlug === "active" && (
                        <div className="listing-dashboard__filters" style={{ display: "flex", gap: "12px", marginLeft: "24px" }}>
                            <select
                                value={formatFilter}
                                onChange={(e) => setFormatFilter(e.target.value)}
                                className="rounded border border-gray-300 py-1 px-3 text-sm focus:border-blue-500 focus:outline-none"
                                style={{ height: "40px" }}
                            >
                                <option value="">All Formats</option>
                                <option value="1">Auction</option>
                                <option value="2">Fixed Price</option>
                            </select>

                            <select
                                value={stockFilter}
                                onChange={(e) => setStockFilter(e.target.value)}
                                className="rounded border border-gray-300 py-1 px-3 text-sm focus:border-blue-500 focus:outline-none"
                                style={{ height: "40px" }}
                            >
                                <option value="">All Stock</option>
                                <option value="in">In Stock</option>
                                <option value="out">Out of Stock</option>
                            </select>
                        </div>
                    )}
                </div>

                <div className="listing-dashboard__controls-row">
                    <div className="listing-dashboard__controls-left">
                        <span className="listing-dashboard__results">
                            Results: {(paging.totalCount ?? 0).toLocaleString()}
                        </span>
                        {paging.totalCount > 0 && (
                            <span className="listing-dashboard__page-indicator">
                                Page {pageNumber} of {totalPages}
                            </span>
                        )}
                    </div>

                    <div className="listing-dashboard__pagination-controls">
                        <button type="button" onClick={handlePrevPage} disabled={pageNumber <= 1}>
                            Previous
                        </button>
                        <button
                            type="button"
                            onClick={() => handleNextPage(totalPages)}
                            disabled={pageNumber >= totalPages}
                        >
                            Next
                        </button>
                        <label className="listing-dashboard__page-size">
                            Rows per page
                            <select value={pageSize} onChange={handlePageSizeChange}>
                                {[10, 20, 50, 100].map((size) => (
                                    <option key={size} value={size}>
                                        {size}
                                    </option>
                                ))}
                            </select>
                        </label>
                    </div>

                    <div className="listing-dashboard__table-tools">
                        <button
                            type="button"
                            className="listing-dashboard__table-tool-btn"
                            onClick={handleOpenCustomize}
                        >
                            Customize table
                        </button>
                        <button
                            type="button"
                            className="listing-dashboard__table-tool-btn"
                            onClick={handleDownload}
                            disabled={isPerformingAction || isProcessingCsv}
                        >
                            Download ▾
                        </button>
                        <button
                            type="button"
                            className="listing-dashboard__table-tool-btn"
                            onClick={handleUploadClick}
                            disabled={isPerformingAction || isProcessingCsv}
                            title="Upload a CSV exported from this page to update listings in bulk"
                        >
                            Upload
                        </button>
                    </div>
                </div>
                <div className="listing-dashboard__actions-row">
                    {toolbarActions.map(({ label, variant, hasCaret, disabled, onClick, title, menuItems = [] }) => (
                        hasCaret ? (
                            <ToolbarDropdownButton
                                key={label}
                                label={label}
                                variant={variant}
                                disabled={disabled || menuItems.length === 0}
                                title={title}
                                menuItems={menuItems}
                            />
                        ) : (
                            <button
                                key={label}
                                type="button"
                                className={`listing-dashboard__action-btn listing-dashboard__action-btn--${variant}`}
                                disabled={disabled}
                                onClick={onClick}
                                title={title}
                            >
                                <span>{label}</span>
                            </button>
                        )
                    ))}
                </div>
            </section >

            {isCustomizeOpen && (
                <div className="listing-customize" role="dialog" aria-modal="true">
                    <div className="listing-customize__dialog">
                        <div className="listing-customize__header">
                            <div>
                                <h2>Customize columns</h2>
                                <p>Choose which columns appear on the {copy.heading.toLowerCase()} table.</p>
                            </div>
                            <button type="button" onClick={handleCustomizeCancel} className="listing-customize__close" aria-label="Close">
                                X
                            </button>
                        </div>
                        <div className="listing-customize__body">
                            {customizeDraft.length === 0 ? (
                                <div className="listing-customize__empty">
                                    Columns will appear here once data is available.
                                </div>
                            ) : (
                                <ul className="listing-customize__list">
                                    {customizeDraft.map((item, index) => (
                                        <li key={item.key} className="listing-customize__row">
                                            <span className="listing-customize__row-label">{item.label}</span>
                                            <div className="listing-customize__row-controls">
                                                <button
                                                    type="button"
                                                    onClick={() => handleCustomizeReorder(item.key, -1)}
                                                    disabled={index === 0}
                                                    aria-label={`Move ${item.label} up`}
                                                >
                                                    ↑
                                                </button>
                                                <button
                                                    type="button"
                                                    onClick={() => handleCustomizeReorder(item.key, 1)}
                                                    disabled={index === customizeDraft.length - 1}
                                                    aria-label={`Move ${item.label} down`}
                                                >
                                                    ↓
                                                </button>
                                                <label className="listing-customize__toggle">
                                                    <input
                                                        type="checkbox"
                                                        checked={item.enabled}
                                                        onChange={() => handleCustomizeToggle(item.key)}
                                                    />
                                                    Show
                                                </label>
                                            </div>
                                        </li>
                                    ))}
                                </ul>
                            )}
                        </div>
                        <div className="listing-customize__footer">
                            <button
                                type="button"
                                className="listing-customize__reset"
                                onClick={handleCustomizeReset}
                                disabled={customizeDraft.length === 0}
                            >
                                Reset to default
                            </button>
                            <div className="listing-customize__footer-actions">
                                <button type="button" onClick={handleCustomizeCancel} className="listing-customize__footer-btn">
                                    Cancel
                                </button>
                                <button
                                    type="button"
                                    onClick={handleCustomizeSave}
                                    className="listing-customize__footer-btn listing-customize__footer-btn--primary"
                                    disabled={!customizeHasVisible}
                                >
                                    Save
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}

            <section className="listing-dashboard__table-card">
                <div className="listing-dashboard__table-wrapper">
                    <table className="listing-dashboard__table">
                        <thead>
                            <tr>
                                <th className="listing-dashboard__checkbox-cell">
                                    <input
                                        ref={headerCheckboxRef}
                                        type="checkbox"
                                        aria-label="Select all listings"
                                        onChange={handleToggleAll}
                                    />
                                </th>
                                <th className="listing-dashboard__actions-header">Actions</th>
                                {visibleColumns.map(({ key, label }) => (
                                    <th key={key}>
                                        <div className="listing-dashboard__header-cell">
                                            <span>{label}</span>
                                        </div>
                                    </th>
                                ))}
                            </tr>
                        </thead>
                        <tbody>
                            {isLoading ? (
                                <tr>
                                    <td colSpan={visibleColumns.length + 2} className="listing-dashboard__table-empty">
                                        Loading...
                                    </td>
                                </tr>
                            ) : rows.length === 0 ? (
                                <tr>
                                    <td colSpan={visibleColumns.length + 2} className="listing-dashboard__table-empty">
                                        <div className="listing-dashboard__empty-title">{copy.emptyTitle}</div>
                                        <div>{copy.emptySubtitle}</div>
                                    </td>
                                </tr>
                            ) : (
                                rows.map((row, index) => {
                                    const rowId = extractListingId(row);
                                    const selectionKey = rowId ?? row.Id ?? row.id ?? row.Sku ?? row.sku ?? row.Title ?? row.title ?? `row-${index}`;
                                    const editMeta = evaluateEditRule(statusSlug, row);

                                    return (
                                        <tr key={selectionKey}>
                                            <td className="listing-dashboard__checkbox-cell">
                                                <input
                                                    type="checkbox"
                                                    aria-label={`select-${selectionKey}`}
                                                    checked={rowId ? selectedIdSet.has(rowId) : false}
                                                    onChange={() => handleToggleRow(row)}
                                                    disabled={!rowId}
                                                />
                                            </td>
                                            <td className="listing-dashboard__actions-cell">
                                                <button
                                                    type="button"
                                                    className="listing-dashboard__row-action"
                                                    onClick={() => navigate(`/p/${rowId}`)}
                                                    style={{ color: "#3665f3", fontWeight: "600" }}
                                                >
                                                    View as Buyer
                                                </button>
                                                <button
                                                    type="button"
                                                    className="listing-dashboard__row-action"
                                                    onClick={() => handleRowEdit(row)}
                                                    disabled={!editMeta.canEdit}
                                                    title={editMeta.canEdit ? undefined : editMeta.reason}
                                                >
                                                    Edit
                                                </button>
                                            </td>
                                            {visibleColumns.map(({ key }) => (
                                                <td key={key}>{renderCell(row, key)}</td>
                                            ))}
                                        </tr>
                                    );
                                })
                            )}
                        </tbody>
                    </table>
                </div>
            </section>
            <input
                ref={uploadInputRef}
                type="file"
                accept=".csv"
                style={{ display: "none" }}
                onChange={handleUploadFile}
            />
        </div >
    );
};

export default AllListingsPage;

