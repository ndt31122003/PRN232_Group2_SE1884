import React, { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { useLocation, useNavigate, useSearchParams } from "react-router-dom";
import dayjs from "dayjs";
import OrderService from "../../services/OrderService";
import ReportService from "../../services/ReportService";
import Notice from "../../components/Common/CustomNotification";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import "./AllOrdersPage.scss";

const PERIOD_OPTIONS = [
    { value: "Last7Days", label: "Last 7 days" },
    { value: "Last30Days", label: "Last 30 days" },
    { value: "Last90Days", label: "Last 90 days" },
    { value: "ThisMonth", label: "This month" },
    { value: "LastMonth", label: "Last month" },
    { value: "ThisYear", label: "This year" },
    { value: "LastYear", label: "Last year" },
    { value: "Custom", label: "Custom range" },
];

const SEARCH_BY_OPTIONS = [
    { value: "OrderNumber", label: "Order number" },
    { value: "BuyerUsername", label: "Buyer username" },
    { value: "BuyerName", label: "Buyer name" },
    { value: "ItemTitle", label: "Item title" },
    { value: "ItemId", label: "Item ID" },
    { value: "CustomLabel", label: "Custom label" },
];

const PAGE_SIZE_OPTIONS = [25, 50, 100];

const SORT_OPTIONS = [
    { key: "DatePaid-desc", value: "DatePaid", direction: true, label: "Date paid (newest first)" },
    { key: "DatePaid-asc", value: "DatePaid", direction: false, label: "Date paid (oldest first)" },
    { key: "Buyer-desc", value: "Buyer", direction: true, label: "Buyer name (Z to A)" },
    { key: "Buyer-asc", value: "Buyer", direction: false, label: "Buyer name (A to Z)" },
    { key: "Total-desc", value: "Total", direction: true, label: "Order total (high to low)" },
    { key: "Total-asc", value: "Total", direction: false, label: "Order total (low to high)" },
];

const ROW_MENU_ACTIONS = [
    { key: "print-packing-slip", label: "Print packing slip" },
    { key: "mark-as-shipped", label: "Mark as shipped" },
    { key: "add-tracking", label: "Add tracking" },
    { key: "archive", label: "Archive" },
];

const STORED_FEEDBACK_OPTIONS = [
    { key: "GREAT_COMMUNICATION", label: "Great communication. A pleasure to do business with." },
    { key: "PROMPT_PAYMENT", label: "Thanks for your prompt payment!" },
    { key: "EXCELLENT_BUYER", label: "Excellent buyer - highly recommended." },
    { key: "SMOOTH_TRANSACTION", label: "Smooth transaction. Welcome back anytime." },
];

const AWAITING_SHIPMENT_FALLBACK_CODES = [
    "AwaitingShipment",
    "AwaitingShipmentOverdue",
    "AwaitingShipmentShipWithin24h",
    "AwaitingExpeditedShipment",
];

const DELIVERABLE_STATUS_CODES = new Set([
    "PaidAndShipped",
    "ShippedAwaitingFeedback",
    "Delivered",
    "DeliveryFailed",
]);

const FEEDBACK_ELIGIBLE_STATUS_CODES = new Set([
    "PaidAndShipped",
    "PaidAwaitingFeedback",
    "ShippedAwaitingFeedback",
    "Delivered",
]);

const SHIPPING_STATUS_NAMES = [
    "Pending",
    "LabelCreated",
    "Shipped",
    "InTransit",
    "OutForDelivery",
    "Delivered",
    "Returned",
];

const CANCELLABLE_SHIPPING_STATUSES = new Set(["Pending", "LabelCreated"]);

const BASE_STATUS_SLUG_MAP = {
    all: null,
    "awaiting-payment": "AwaitingPayment",
    "awaiting-shipment": "AwaitingShipment",
    "paid-shipped": "PaidAndShipped",
    delivered: "Delivered",
    "delivery-failed": "DeliveryFailed",
    archived: "Archived",
    cancellations: "Cancelled",
};

const BASE_STATUS_CODE_MAP = Object.entries(BASE_STATUS_SLUG_MAP).reduce(
    (acc, [slug, code]) => {
        if (code) {
            acc[code] = slug;
        }
        return acc;
    },
    { "": "all" }
);

const slugifyStatusKey = (value) =>
    value
        .replace(/([a-z0-9])([A-Z])/g, "$1-$2")
        .replace(/\s+/g, "-")
        .replace(/_+/g, "-")
        .replace(/-+/g, "-")
        .toLowerCase();

const escapeHtml = (value) => {
    if (value === null || value === undefined) {
        return "";
    }

    return String(value)
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#39;");
};

const humanizeStatusCode = (value) => {
    if (!value) {
        return "All orders";
    }

    const slug = slugifyStatusKey(String(value));
    if (!slug) {
        return String(value);
    }

    return slug
        .split("-")
        .filter(Boolean)
        .map((segment) => segment.charAt(0).toUpperCase() + segment.slice(1))
        .join(" ");
};

const DEFAULT_FILTER = {
    PageNumber: 1,
    PageSize: 25,
    Period: "Last90Days",
    SortBy: "DatePaid",
    SortDescending: true,
    Status: null,
    FromDate: "",
    ToDate: "",
    Keyword: "",
    SearchBy: "OrderNumber",
};

const parsePositiveNumber = (value, fallback) => {
    const parsed = Number(value);
    return Number.isFinite(parsed) && parsed > 0 ? parsed : fallback;
};

const serializeFiltersToSearchParams = (filters, statusSlug) => {
    const params = new URLSearchParams();

    const normalizedStatusSlug = typeof statusSlug === "string" && statusSlug.trim()
        ? statusSlug.trim().toLowerCase()
        : "all";
    params.set("status", normalizedStatusSlug);

    if (filters.PageNumber > DEFAULT_FILTER.PageNumber) {
        params.set("page", String(filters.PageNumber));
    }

    if (filters.PageSize !== DEFAULT_FILTER.PageSize) {
        params.set("pageSize", String(filters.PageSize));
    }

    if (filters.Period !== DEFAULT_FILTER.Period) {
        params.set("period", filters.Period);
    }

    if (filters.SortBy !== DEFAULT_FILTER.SortBy) {
        params.set("sortBy", filters.SortBy);
    }

    if (filters.SortDescending !== DEFAULT_FILTER.SortDescending) {
        params.set("sortDescending", filters.SortDescending ? "true" : "false");
    }

    if (filters.Period === "Custom") {
        if (filters.FromDate) {
            params.set("fromDate", filters.FromDate);
        }
        if (filters.ToDate) {
            params.set("toDate", filters.ToDate);
        }
    }

    const keyword = filters.Keyword.trim();
    if (keyword) {
        params.set("keyword", keyword);
        params.set("searchBy", filters.SearchBy);
    }

    return params;
};

const normalizeShippingStatus = (value) => {
    if (value === null || value === undefined) {
        return "";
    }
    if (typeof value === "string") {
        return value;
    }
    if (typeof value === "number" && Number.isInteger(value) && value >= 0 && value < SHIPPING_STATUS_NAMES.length) {
        return SHIPPING_STATUS_NAMES[value];
    }
    return String(value);
};

const normalizeStatus = (status) => ({
    id:
        status?.id ??
        status?.Id ??
        status?.statusCode ??
        status?.StatusCode ??
        status?.statusName ??
        status?.StatusName ??
        Math.random().toString(36).slice(2),
    code: status?.statusCode ?? status?.StatusCode ?? "",
    name: status?.statusName ?? status?.StatusName ?? "",
    color: status?.statusColor ?? status?.StatusColor ?? "#64748b",
});

const normalizeOrderItem = (item) => {
    if (!item) {
        return {
            id: null,
            listingId: null,
            variationId: null,
            title: "Untitled item",
            imageUrl: "",
            sku: "",
            quantity: 0,
            unitPrice: null,
            totalPrice: null,
        };
    }

    const resolvedQuantity = Number(item?.quantity ?? item?.Quantity ?? 0);

    return {
        id: item?.id ?? item?.Id ?? item?.orderItemId ?? item?.OrderItemId ?? null,
        listingId: item?.listingId ?? item?.ListingId ?? null,
        variationId: item?.variationId ?? item?.VariationId ?? null,
        title: item?.title ?? item?.Title ?? "Untitled item",
        imageUrl: item?.imageUrl ?? item?.ImageUrl ?? "",
        sku: item?.sku ?? item?.Sku ?? "",
        quantity: Number.isFinite(resolvedQuantity) ? resolvedQuantity : 0,
        unitPrice: item?.unitPrice ?? item?.UnitPrice ?? null,
        totalPrice: item?.totalPrice ?? item?.TotalPrice ?? null,
    };
};

const normalizeShipment = (shipment) => {
    if (!shipment) {
        return {
            id: null,
            orderItemId: "",
            trackingNumber: "",
            carrier: "",
            shippedAt: null,
            shippingLabelId: null,
            createdAt: null,
            updatedAt: null,
        };
    }

    const orderItemIdentifier = shipment?.orderItemId ?? shipment?.OrderItemId ?? "";

    return {
        id: shipment?.id ?? shipment?.Id ?? null,
        orderItemId: orderItemIdentifier ? orderItemIdentifier.toString() : "",
        trackingNumber: shipment?.trackingNumber ?? shipment?.TrackingNumber ?? "",
        carrier: shipment?.carrier ?? shipment?.Carrier ?? "",
        shippedAt: shipment?.shippedAt ?? shipment?.ShippedAt ?? null,
        shippingLabelId: shipment?.shippingLabelId ?? shipment?.ShippingLabelId ?? null,
        createdAt: shipment?.createdAt ?? shipment?.CreatedAt ?? null,
        updatedAt: shipment?.updatedAt ?? shipment?.UpdatedAt ?? null,
    };
};

const normalizeOrder = (order) => {
    const rawItems = order?.items ?? order?.Items ?? [];
    const rawShipments = order?.shipments ?? order?.Shipments ?? [];

    const normalizedItems = Array.isArray(rawItems) ? rawItems.map(normalizeOrderItem) : [];
    const normalizedShipments = Array.isArray(rawShipments) ? rawShipments.map(normalizeShipment) : [];

    const rawFeedback = order?.sellerFeedback ?? order?.SellerFeedback ?? null;
    const sellerFeedback = rawFeedback
        ? {
            id: rawFeedback?.id ?? rawFeedback?.Id ?? null,
            orderId: rawFeedback?.orderId ?? rawFeedback?.OrderId ?? null,
            sellerId: rawFeedback?.sellerId ?? rawFeedback?.SellerId ?? null,
            buyerId: rawFeedback?.buyerId ?? rawFeedback?.BuyerId ?? null,
            comment: rawFeedback?.comment ?? rawFeedback?.Comment ?? "",
            usesStoredFeedback:
                rawFeedback?.usesStoredFeedback ?? rawFeedback?.UsesStoredFeedback ?? false,
            storedFeedbackKey:
                rawFeedback?.storedFeedbackKey ?? rawFeedback?.StoredFeedbackKey ?? null,
            createdAt: rawFeedback?.createdAt ?? rawFeedback?.CreatedAt ?? null,
            followUpComment:
                rawFeedback?.followUpComment ?? rawFeedback?.FollowUpComment ?? null,
            followUpCommentedAt:
                rawFeedback?.followUpCommentedAt ?? rawFeedback?.FollowUpCommentedAt ?? null,
        }
        : null;

    return {
        id: order?.id ?? order?.Id,
        orderNumber: order?.orderNumber ?? order?.OrderNumber ?? "",
        buyerFullName: order?.buyerFullName ?? order?.BuyerFullName ?? "",
        buyerUsername: order?.buyerUsername ?? order?.BuyerUsername ?? "",
        items: normalizedItems,
        shipments: normalizedShipments,
        subTotal: order?.subTotal ?? order?.SubTotal ?? null,
        total: order?.total ?? order?.Total ?? null,
        shippingCost: order?.shippingCost ?? order?.ShippingCost ?? null,
        taxAmount: order?.taxAmount ?? order?.TaxAmount ?? null,
        statusCode: order?.statusCode ?? order?.StatusCode ?? "",
        statusName: order?.statusName ?? order?.StatusName ?? "",
        statusColor: order?.statusColor ?? order?.StatusColor ?? "#64748b",
        shippingStatus: normalizeShippingStatus(order?.shippingStatus ?? order?.ShippingStatus ?? ""),
        fulfillmentType: order?.fulfillmentType ?? order?.FulfillmentType ?? "",
        orderedAt: order?.orderedAt ?? order?.OrderedAt ?? null,
        paidAt: order?.paidAt ?? order?.PaidAt ?? null,
        shippedAt: order?.shippedAt ?? order?.ShippedAt ?? null,
        deliveredAt: order?.deliveredAt ?? order?.DeliveredAt ?? null,
        sellerFeedback,
    };
};

const formatCurrency = (value) => {
    if (value === null || value === undefined) {
        return "-";
    }
    return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
    }).format(value);
};

const formatDate = (value) => {
    if (!value) {
        return "-";
    }
    const parsed = dayjs(value);
    return parsed.isValid() ? parsed.format("MMM D, YYYY") : "-";
};

const buildQueryParams = (filters) => {
    const params = {
        PageNumber: filters.PageNumber,
        PageSize: filters.PageSize,
        Period: filters.Period,
        SortBy: filters.SortBy,
        SortDescending: filters.SortDescending,
    };

    if (filters.Status) {
        params.Status = filters.Status;
    }

    if (filters.Period === "Custom") {
        if (filters.FromDate) {
            params.FromDate = dayjs(filters.FromDate).startOf("day").toISOString();
        }
        if (filters.ToDate) {
            params.ToDate = dayjs(filters.ToDate).endOf("day").toISOString();
        }
    }

    if (filters.Keyword.trim()) {
        params.Keyword = filters.Keyword.trim();
        params.SearchBy = filters.SearchBy;
    }

    return params;
};

const TRACKING_EDITOR_INITIAL_STATE = {
    isOpen: false,
    orderId: null,
    orderItemId: null,
    orderNumber: "",
    itemTitle: "",
    itemImage: "",
    shippedAt: "",
    trackingNumber: "",
    carrier: "",
    existingShipments: [],
    isSubmitting: false,
    error: "",
};

const AllOrdersPage = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const [searchParams] = useSearchParams();
    const statusSlugFromQuery = (() => {
        const raw = searchParams.get("status");
        const normalized = (raw ?? "").trim().toLowerCase();
        return normalized || "all";
    })();
    const previousStatusSlugRef = useRef(statusSlugFromQuery);
    const statusSlugJustChanged = previousStatusSlugRef.current !== statusSlugFromQuery;
    const initialStatusFromSlug = Object.prototype.hasOwnProperty.call(BASE_STATUS_SLUG_MAP, statusSlugFromQuery)
        ? BASE_STATUS_SLUG_MAP[statusSlugFromQuery] ?? null
        : undefined;

    const [filters, setFilters] = useState(() => {
        const paramsRecord = Object.fromEntries(searchParams.entries());
        const initial = { ...DEFAULT_FILTER };
        initial.PageNumber = parsePositiveNumber(
            paramsRecord.page,
            DEFAULT_FILTER.PageNumber
        );
        initial.PageSize = parsePositiveNumber(
            paramsRecord.pageSize,
            DEFAULT_FILTER.PageSize
        );
        initial.Period = paramsRecord.period ?? DEFAULT_FILTER.Period;
        initial.SortBy = paramsRecord.sortBy ?? DEFAULT_FILTER.SortBy;
        initial.SortDescending = (paramsRecord.sortDescending ?? "true") !== "false";
        initial.SearchBy = SEARCH_BY_OPTIONS.some((option) => option.value === paramsRecord.searchBy)
            ? paramsRecord.searchBy
            : DEFAULT_FILTER.SearchBy;
        initial.Keyword = paramsRecord.keyword ?? DEFAULT_FILTER.Keyword;

        if (initialStatusFromSlug !== undefined) {
            initial.Status = initialStatusFromSlug;
        }

        if (initial.Period === "Custom") {
            initial.FromDate = paramsRecord.fromDate ?? "";
            initial.ToDate = paramsRecord.toDate ?? "";
        }

        return initial;
    });

    const [searchDraft, setSearchDraft] = useState(filters.Keyword);
    const [statuses, setStatuses] = useState([]);
    const [orders, setOrders] = useState([]);
    const [selectedOrderIds, setSelectedOrderIds] = useState([]);
    const [pagination, setPagination] = useState({
        totalCount: 0,
        pageNumber: filters.PageNumber,
        pageSize: filters.PageSize,
    });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [openMenuId, setOpenMenuId] = useState(null);
    const [pendingAction, setPendingAction] = useState(null);
    const [trackingEditor, setTrackingEditor] = useState(() => ({ ...TRACKING_EDITOR_INITIAL_STATE }));
    const menuRefs = useRef({});
    const masterCheckboxRef = useRef(null);
    const syncingFromSlugRef = useRef(false);
    const [isGeneratingReport, setIsGeneratingReport] = useState(false);

    const resolvedStatusLabel = useMemo(() => {
        if (!filters.Status) {
            return "All orders";
        }

        const statusValue = String(filters.Status).toLowerCase();
        const match = statuses.find((status) => {
            const code = status?.code ?? "";
            return code && String(code).toLowerCase() === statusValue;
        });

        return match?.name ?? humanizeStatusCode(filters.Status);
    }, [filters.Status, statuses]);

    const resolvedPeriodLabel = useMemo(() => {
        const match = PERIOD_OPTIONS.find((option) => option.value === filters.Period);
        return match?.label ?? filters.Period;
    }, [filters.Period]);

    const isCustomRangeValid = useMemo(() => {
        if (filters.Period !== "Custom") {
            return true;
        }

        if (!filters.FromDate || !filters.ToDate) {
            return false;
        }

        const from = dayjs(filters.FromDate);
        const to = dayjs(filters.ToDate);
        if (!from.isValid() || !to.isValid()) {
            return false;
        }

        return !from.isAfter(to);
    }, [filters.FromDate, filters.Period, filters.ToDate]);

    const hasOrders = orders.length > 0;
    const printDisabled = !hasOrders || loading;
    const downloadDisabled = !hasOrders || !isCustomRangeValid || isGeneratingReport || loading;

    const closeTrackingEditor = useCallback(() => {
        setTrackingEditor({ ...TRACKING_EDITOR_INITIAL_STATE });
    }, []);

    const handleTrackingBackdropClick = useCallback(
        (event) => {
            if (event.target === event.currentTarget) {
                closeTrackingEditor();
            }
        },
        [closeTrackingEditor]
    );

    const applyShipmentsUpdate = useCallback(
        (orderId, shipmentsPayload) => {
            if (!orderId) {
                return;
            }

            setOrders((prev) =>
                prev.map((order) => {
                    if (!order?.id) {
                        return order;
                    }

                    const matches =
                        order.id.toString().toLowerCase() === orderId.toString().toLowerCase();

                    if (!matches) {
                        return order;
                    }

                    const normalizedShipments = Array.isArray(shipmentsPayload)
                        ? shipmentsPayload
                            .map(normalizeShipment)
                            .sort((a, b) => {
                                const first = dayjs(a?.shippedAt ?? 0);
                                const second = dayjs(b?.shippedAt ?? 0);
                                return first.valueOf() - second.valueOf();
                            })
                        : [];

                    return {
                        ...order,
                        shipments: normalizedShipments,
                    };
                })
            );
        },
        [setOrders]
    );

    const handleTrackingFieldChange = useCallback((field, value) => {
        setTrackingEditor((prev) => ({
            ...prev,
            [field]: value,
        }));
    }, []);

    const handleOpenTrackingEditor = useCallback(
        (order, item) => {
            if (!order?.id || !item) {
                return;
            }

            const rawOrderItemId = item?.id ?? item?.orderItemId ?? item?.OrderItemId ?? null;
            if (!rawOrderItemId) {
                return;
            }

            const lookupId = rawOrderItemId.toString().toLowerCase();
            const shipmentsForItem = (order.shipments ?? []).filter((shipment) => {
                const shipmentOrderItemId = shipment?.orderItemId ?? shipment?.OrderItemId ?? null;
                return (
                    shipmentOrderItemId &&
                    shipmentOrderItemId.toString().toLowerCase() === lookupId
                );
            });

            setTrackingEditor({
                ...TRACKING_EDITOR_INITIAL_STATE,
                isOpen: true,
                orderId: order.id,
                orderNumber: order.orderNumber ?? order.OrderNumber ?? "",
                orderItemId: rawOrderItemId.toString(),
                itemTitle: item?.title ?? item?.Title ?? "Untitled item",
                itemImage: item?.imageUrl ?? item?.ImageUrl ?? "",
                shippedAt: dayjs().format("YYYY-MM-DD"),
                existingShipments: shipmentsForItem,
            });
        },
        []
    );

    const handleTrackingSubmit = useCallback(
        async (event) => {
            event.preventDefault();

            if (!trackingEditor.isOpen || trackingEditor.isSubmitting) {
                return;
            }

            const trimmedTracking = trackingEditor.trackingNumber.trim();
            if (!trimmedTracking) {
                setTrackingEditor((prev) => ({
                    ...prev,
                    error: "Tracking number is required.",
                }));
                return;
            }

            const trimmedCarrier = trackingEditor.carrier.trim();
            if (!trimmedCarrier) {
                setTrackingEditor((prev) => ({
                    ...prev,
                    error: "Carrier is required.",
                }));
                return;
            }

            const shipDateIso = (() => {
                if (!trackingEditor.shippedAt) {
                    return dayjs().toISOString();
                }
                const parsed = dayjs(trackingEditor.shippedAt);
                return parsed.isValid() ? parsed.endOf("day").toISOString() : dayjs().toISOString();
            })();

            const existingPayload = (trackingEditor.existingShipments ?? []).map((shipment) => ({
                shipmentId: shipment?.id ?? shipment?.Id ?? null,
                orderItemId:
                    shipment?.orderItemId ?? shipment?.OrderItemId ?? trackingEditor.orderItemId,
                trackingNumber: shipment?.trackingNumber ?? shipment?.TrackingNumber ?? "",
                carrier: shipment?.carrier ?? shipment?.Carrier ?? "",
                shippedAt: (() => {
                    const value = shipment?.shippedAt ?? shipment?.ShippedAt ?? null;
                    if (!value) {
                        return null;
                    }
                    const parsed = dayjs(value);
                    return parsed.isValid() ? parsed.toISOString() : null;
                })(),
                shippingLabelId: shipment?.shippingLabelId ?? shipment?.ShippingLabelId ?? null,
            }));

            const payload = {
                shipments: [
                    ...existingPayload,
                    {
                        shipmentId: null,
                        orderItemId: trackingEditor.orderItemId,
                        trackingNumber: trimmedTracking,
                        carrier: trimmedCarrier,
                        shippedAt: shipDateIso,
                        shippingLabelId: null,
                    },
                ],
                clearedOrderItemIds: null,
            };

            setTrackingEditor((prev) => ({
                ...prev,
                isSubmitting: true,
                error: "",
            }));

            try {
                const response = await OrderService.upsertShipments(trackingEditor.orderId, payload);
                const responseData = response?.data ?? [];
                applyShipmentsUpdate(trackingEditor.orderId, responseData);
                closeTrackingEditor();
            } catch (err) {
                console.error("Failed to upsert shipments", err);
                const message =
                    err?.response?.data?.detail ??
                    err?.response?.data?.message ??
                    err?.message ??
                    "Unable to add tracking. Please try again.";

                setTrackingEditor((prev) => ({
                    ...prev,
                    isSubmitting: false,
                    error: message,
                }));
            }
        },
        [trackingEditor, applyShipmentsUpdate, closeTrackingEditor]
    );

    const selectedIdSet = useMemo(() => new Set(selectedOrderIds), [selectedOrderIds]);
    const selectableOrderIds = useMemo(
        () => orders.map((order) => order?.id).filter((id) => id !== undefined && id !== null),
        [orders]
    );
    const allOrdersSelected = selectableOrderIds.length > 0 && selectableOrderIds.every((id) => selectedIdSet.has(id));
    const hasSelections = selectedOrderIds.length > 0;

    useEffect(() => {
        setSelectedOrderIds((prev) => prev.filter((id) => selectableOrderIds.includes(id)));
    }, [selectableOrderIds]);

    useEffect(() => {
        if (!masterCheckboxRef.current) {
            return;
        }

        // Reflect partial selections in the master checkbox UI.
        masterCheckboxRef.current.indeterminate = hasSelections && !allOrdersSelected;
    }, [hasSelections, allOrdersSelected]);

    const handleToggleSelectAll = () => {
        if (selectableOrderIds.length === 0) {
            return;
        }
        setSelectedOrderIds((prev) => (prev.length === selectableOrderIds.length ? [] : [...selectableOrderIds]));
    };

    const handleToggleOrderSelection = (orderId) => {
        if (!orderId) {
            return;
        }
        setSelectedOrderIds((prev) => {
            const exists = prev.includes(orderId);
            return exists ? prev.filter((id) => id !== orderId) : [...prev, orderId];
        });
    };

    useEffect(() => {
        let ignore = false;
        const controller = new AbortController();

        const fetchStatuses = async () => {
            try {
                const response = await OrderService.getStatuses(controller.signal);
                console.log("Fetched statuses", response);
                if (!ignore) {
                    const normalized = (response?.data ?? []).map(normalizeStatus);
                    setStatuses(normalized);
                }
            } catch (err) {
                if (controller.signal.aborted) {
                    return;
                }
                console.error("Failed to load order statuses", err);
            }
        };

        fetchStatuses();

        return () => {
            ignore = true;
            controller.abort();
        };
    }, []);

    const statusSlugToCode = useMemo(() => {
        const map = { ...BASE_STATUS_SLUG_MAP };
        statuses.forEach((status) => {
            const rawCode = status?.code ?? "";
            const trimmedCode = typeof rawCode === "string" ? rawCode.trim() : "";
            if (!trimmedCode) {
                return;
            }
            const knownSlug = BASE_STATUS_CODE_MAP[trimmedCode];
            const slug = knownSlug ?? slugifyStatusKey(trimmedCode);
            if (!Object.prototype.hasOwnProperty.call(map, slug)) {
                map[slug] = trimmedCode;
            }
        });
        return map;
    }, [statuses]);

    const statusCodeToSlug = useMemo(() => {
        const map = { ...BASE_STATUS_CODE_MAP };
        statuses.forEach((status) => {
            const rawCode = status?.code ?? "";
            const trimmedCode = typeof rawCode === "string" ? rawCode.trim() : "";
            if (!trimmedCode) {
                return;
            }
            const slug = BASE_STATUS_CODE_MAP[trimmedCode] ?? slugifyStatusKey(trimmedCode);
            map[trimmedCode] = slug;
        });
        return map;
    }, [statuses]);

    const handleToggleMenu = (orderId) => {
        setOpenMenuId((prev) => (prev === orderId ? null : orderId));
    };

    const handleRowMenuAction = (order, actionKey) => {
        setOpenMenuId(null);
        if (!order) {
            return;
        }
        console.info("Row action selected", {
            action: actionKey,
            orderId: order.id,
        });
    };

    const computeReportDateRange = useCallback(() => {
        const now = dayjs();
        const endOfToday = now.endOf("day");

        switch (filters.Period) {
            case "Last7Days": {
                const start = endOfToday.subtract(6, "day").startOf("day");
                return {
                    rangeStartUtc: start.toISOString(),
                    rangeEndUtc: endOfToday.toISOString(),
                };
            }
            case "Last30Days": {
                const start = endOfToday.subtract(29, "day").startOf("day");
                return {
                    rangeStartUtc: start.toISOString(),
                    rangeEndUtc: endOfToday.toISOString(),
                };
            }
            case "Last90Days": {
                const start = endOfToday.subtract(89, "day").startOf("day");
                return {
                    rangeStartUtc: start.toISOString(),
                    rangeEndUtc: endOfToday.toISOString(),
                };
            }
            case "ThisMonth": {
                const start = now.startOf("month");
                return {
                    rangeStartUtc: start.toISOString(),
                    rangeEndUtc: endOfToday.toISOString(),
                };
            }
            case "LastMonth": {
                const lastMonth = now.subtract(1, "month");
                const start = lastMonth.startOf("month");
                const end = lastMonth.endOf("month");
                return {
                    rangeStartUtc: start.toISOString(),
                    rangeEndUtc: end.toISOString(),
                };
            }
            case "ThisYear": {
                const start = now.startOf("year");
                return {
                    rangeStartUtc: start.toISOString(),
                    rangeEndUtc: endOfToday.toISOString(),
                };
            }
            case "LastYear": {
                const lastYearStart = now.subtract(1, "year").startOf("year");
                const lastYearEnd = lastYearStart.endOf("year");
                return {
                    rangeStartUtc: lastYearStart.toISOString(),
                    rangeEndUtc: lastYearEnd.toISOString(),
                };
            }
            case "Custom": {
                if (!isCustomRangeValid) {
                    return null;
                }

                const start = dayjs(filters.FromDate);
                const end = dayjs(filters.ToDate);

                if (!start.isValid() || !end.isValid()) {
                    return null;
                }

                return {
                    rangeStartUtc: start.startOf("day").toISOString(),
                    rangeEndUtc: end.endOf("day").toISOString(),
                };
            }
            default:
                return null;
        }
    }, [filters.FromDate, filters.Period, filters.ToDate, isCustomRangeValid]);

    const handlePrintOrders = useCallback(() => {
        if (loading) {
            return;
        }

        if (!hasOrders) {
            Notice({ msg: "There are no orders to print.", isSuccess: false });
            return;
        }

        const generationTime = dayjs().format("MMM D, YYYY h:mm A");
        const rowsHtml = orders
            .map((order, index) => {
                const buyerName = order.buyerFullName || order.buyerUsername || "Unknown buyer";
                const items = Array.isArray(order.items) ? order.items : [];
                const itemsHtml = items.length > 0
                    ? `<ul class="order-items">${items
                        .map((item) => `<li>${escapeHtml(item?.title ?? item?.Title ?? "Untitled item")} x ${escapeHtml(String(item?.quantity ?? 0))}</li>`)
                        .join("")}</ul>`
                    : "<span>-</span>";

                return `
                    <tr>
                        <td>${index + 1}</td>
                        <td>${escapeHtml(order.orderNumber || "-")}</td>
                        <td>${escapeHtml(buyerName)}</td>
                        <td>${itemsHtml}</td>
                        <td>${escapeHtml(formatCurrency(order.subTotal))}</td>
                        <td>${escapeHtml(formatCurrency(order.total))}</td>
                        <td>${escapeHtml(formatDate(order.orderedAt))}</td>
                        <td>${escapeHtml(formatDate(order.paidAt))}</td>
                        <td>${escapeHtml(order.statusName || order.statusCode || "-")}</td>
                    </tr>
                `;
            })
            .join("");

        const customRangeDetails = filters.Period === "Custom"
            ? `<p><strong>Date range:</strong> ${escapeHtml(filters.FromDate || "-")} - ${escapeHtml(filters.ToDate || "-")}</p>`
            : "";

        const html = `<!doctype html>
<html>
<head>
<meta charset="utf-8" />
<title>Orders report</title>
<style>
body { font-family: "Segoe UI", Arial, sans-serif; color: #0f172a; margin: 24px; }
h1 { font-size: 22px; margin: 0 0 12px; }
p { margin: 4px 0; font-size: 13px; }
table { width: 100%; border-collapse: collapse; margin-top: 16px; font-size: 12px; }
th, td { border: 1px solid #cbd5e1; padding: 8px; text-align: left; vertical-align: top; }
th { background-color: #f1f5f9; }
.order-items { margin: 0; padding-left: 18px; }
.meta { margin-top: 12px; color: #475569; }
</style>
</head>
<body>
<h1>Orders report</h1>
<p><strong>Status:</strong> ${escapeHtml(resolvedStatusLabel)}</p>
<p><strong>Period:</strong> ${escapeHtml(resolvedPeriodLabel)}</p>
${customRangeDetails}
<p class="meta">Generated on ${escapeHtml(generationTime)}.</p>
<table>
<thead>
<tr>
    <th>#</th>
    <th>Order</th>
    <th>Buyer</th>
    <th>Items</th>
    <th>Subtotal</th>
    <th>Total</th>
    <th>Date sold</th>
    <th>Date paid</th>
    <th>Status</th>
</tr>
</thead>
<tbody>
${rowsHtml}
</tbody>
</table>
</body>
</html>`;

        const iframe = document.createElement("iframe");
        iframe.style.position = "fixed";
        iframe.style.width = "0";
        iframe.style.height = "0";
        iframe.style.border = "0";
        iframe.style.top = "-10000px";
        iframe.setAttribute("aria-hidden", "true");
        document.body.appendChild(iframe);

        const cleanup = () => {
            window.setTimeout(() => {
                if (iframe.parentNode) {
                    iframe.parentNode.removeChild(iframe);
                }
            }, 0);
        };

        const { contentWindow } = iframe;
        if (!contentWindow) {
            cleanup();
            Notice({ msg: "Unable to open print preview in this browser.", isSuccess: false });
            return;
        }

        const printDocument = contentWindow.document;
        try {
            printDocument.open();
            printDocument.write(html);
            printDocument.close();
        } catch (error) {
            console.error("Failed to prepare print content", error);
            cleanup();
            Notice({ msg: "Unable to prepare print preview.", isSuccess: false });
            return;
        }

        const triggerPrint = () => {
            try {
                contentWindow.focus();
                contentWindow.print();
            } catch (error) {
                console.warn("Unable to trigger print dialog", error);
            } finally {
                cleanup();
            }
        };

        window.setTimeout(triggerPrint, 20);
    }, [filters.FromDate, filters.Period, filters.ToDate, hasOrders, loading, orders, resolvedPeriodLabel, resolvedStatusLabel]);

    const handleDownloadReport = useCallback(async () => {
        if (loading) {
            return;
        }

        if (!hasOrders) {
            Notice({ msg: "There are no orders to include in the report.", isSuccess: false });
            return;
        }

        if (!isCustomRangeValid) {
            Notice({ msg: "Select a valid date range before downloading.", isSuccess: false });
            return;
        }

        if (isGeneratingReport) {
            return;
        }

        const payload = {
            source: "Orders",
            type: resolvedStatusLabel || "All orders",
        };

        const dateRange = computeReportDateRange();
        if (dateRange?.rangeStartUtc) {
            payload.rangeStartUtc = dateRange.rangeStartUtc;
        }
        if (dateRange?.rangeEndUtc) {
            payload.rangeEndUtc = dateRange.rangeEndUtc;
        }

        setIsGeneratingReport(true);

        try {
            const timeZone = (() => {
                try {
                    return Intl.DateTimeFormat().resolvedOptions().timeZone;
                } catch (error) {
                    return undefined;
                }
            })();

            if (timeZone) {
                payload.timeZone = timeZone;
            }
            const response = await ReportService.createReportDownload(payload);
            const result = response?.data ?? response ?? {};

            const fileUrl = result?.fileUrl ?? result?.FileUrl ?? result?.downloadUrl ?? result?.DownloadUrl ?? "";
            const fileName = result?.fileName ?? result?.FileName ?? "";

            if (fileUrl) {
                const link = document.createElement("a");
                link.href = fileUrl;
                link.target = "_blank";
                link.rel = "noopener";
                if (fileName) {
                    link.download = fileName;
                }
                document.body.appendChild(link);
                link.click();
                document.body.removeChild(link);
                Notice({ msg: "Report download started.", isSuccess: true });
            } else {
                Notice({ msg: "Report request submitted. Check Downloads once it is ready.", isSuccess: true });
            }
        } catch (err) {
            console.error("Failed to download orders report", err);
            const message =
                err?.response?.data?.detail ??
                err?.response?.data?.message ??
                err?.message ??
                "Unable to start report download. Please try again.";
            Notice({ msg: message, isSuccess: false });
        } finally {
            setIsGeneratingReport(false);
        }
    }, [computeReportDateRange, hasOrders, isCustomRangeValid, isGeneratingReport, loading, resolvedStatusLabel]);

    const applyStatusUpdate = (statusPayload) => {
        if (!statusPayload) {
            return;
        }

        const orderId = statusPayload.orderId ?? statusPayload.OrderId;
        if (!orderId) {
            return;
        }

        const nextStatusCode = statusPayload.statusCode ?? statusPayload.StatusCode;
        const nextStatusName = statusPayload.statusName ?? statusPayload.StatusName;
        const nextStatusColor = statusPayload.statusColor ?? statusPayload.StatusColor;
        const nextShippingStatus = normalizeShippingStatus(
            statusPayload.shippingStatus ?? statusPayload.ShippingStatus ?? null
        );
        const nextPaidAt = statusPayload.paidAt ?? statusPayload.PaidAt;
        const nextShippedAt = statusPayload.shippedAt ?? statusPayload.ShippedAt;
        const nextDeliveredAt = statusPayload.deliveredAt ?? statusPayload.DeliveredAt;

        setOrders((prev) =>
            prev.map((order) => {
                if (!order?.id) {
                    return order;
                }
                const matches = order.id.toString().toLowerCase() === orderId.toString().toLowerCase();
                if (!matches) {
                    return order;
                }

                return {
                    ...order,
                    statusCode: nextStatusCode ?? order.statusCode,
                    statusName: nextStatusName ?? order.statusName,
                    statusColor: nextStatusColor ?? order.statusColor,
                    shippingStatus: nextShippingStatus || order.shippingStatus,
                    paidAt: nextPaidAt ?? order.paidAt,
                    shippedAt: nextShippedAt ?? order.shippedAt,
                    deliveredAt: nextDeliveredAt ?? order.deliveredAt,
                };
            })
        );
    };

    const withRowAction = async (order, actionKey, callback) => {
        if (!order?.id || typeof callback !== "function") {
            return;
        }

        setPendingAction({ orderId: order.id, actionKey });
        setError("");

        try {
            const response = await callback();
            const statusData = response?.data ?? response;
            applyStatusUpdate(statusData);
        } catch (err) {
            console.error(`Failed to execute ${actionKey}`, err);
            const message = err?.response?.data?.detail ?? err?.response?.data?.message ?? err?.message;
            setError(message || "Unable to update order status. Please try again.");
        } finally {
            setPendingAction(null);
        }
    };

    const handleMarkDelivered = (order) => {
        withRowAction(order, "delivered", () =>
            OrderService.updateDeliveryStatus(order.id, {
                outcome: 0,
                note: "",
            })
        );
    };

    const handleMarkDeliveryFailed = (order) => {
        withRowAction(order, "delivery-failed", () =>
            OrderService.updateDeliveryStatus(order.id, {
                outcome: 1,
                note: "",
            })
        );
    };

    const handleNavigateToShipping = (order) => {
        if (!order) {
            return;
        }
        const targetPath = `/order/ship/${order.id}`;
        const from = `${location.pathname}${location.search}`;
        navigate(targetPath, {
            state: {
                order,
                from,
            },
        });
    };

    const handleNavigateToCancellation = (order) => {
        if (!order) {
            return;
        }
        const targetPath = `/order/cancel/${order.id}`;
        const from = `${location.pathname}${location.search}`;
        navigate(targetPath, {
            state: {
                order,
                from,
            },
        });
    };

    const handleNavigateToOrderDetail = (order) => {
        if (!order) {
            return;
        }
        const targetPath = `/order/detail/${order.id}`;
        const from = `${location.pathname}${location.search}`;
        navigate(targetPath, {
            state: {
                order,
                from,
            },
        });
    };

    const derivedStatusFromSlug = Object.prototype.hasOwnProperty.call(statusSlugToCode, statusSlugFromQuery)
        ? statusSlugToCode[statusSlugFromQuery] ?? null
        : undefined;

    useEffect(() => {
        if (derivedStatusFromSlug === undefined) {
            return;
        }
        let didUpdate = false;
        setFilters((prev) => {
            const nextStatus = derivedStatusFromSlug ?? null;
            if ((prev.Status ?? null) === (nextStatus ?? null)) {
                return prev;
            }
            didUpdate = true;
            return { ...prev, Status: nextStatus, PageNumber: 1 };
        });
        if (didUpdate) {
            syncingFromSlugRef.current = true;
        }
    }, [derivedStatusFromSlug]);

    useEffect(() => {
        if (syncingFromSlugRef.current) {
            if ((filters.Status ?? null) !== (derivedStatusFromSlug ?? null)) {
                return;
            }
            syncingFromSlugRef.current = false;
        }

        if (
            statusSlugJustChanged &&
            derivedStatusFromSlug !== undefined &&
            (filters.Status ?? null) !== (derivedStatusFromSlug ?? null)
        ) {
            return;
        }

        const buildingStatusSlug = (() => {
            const currentStatus = filters.Status ?? "";
            if (!currentStatus) {
                return "all";
            }
            return statusCodeToSlug[currentStatus] ?? slugifyStatusKey(currentStatus);
        })();

        const params = serializeFiltersToSearchParams(filters, buildingStatusSlug);
        const targetSearch = params.toString();
        const currentSearch = location.search.startsWith("?") ? location.search.slice(1) : location.search;
        const targetPath = "/order/all";
        const currentPath = location.pathname;

        const targetHref = `${targetPath}${targetSearch ? `?${targetSearch}` : ""}`;
        const currentHref = `${currentPath}${location.search}`;

        if (currentHref !== targetHref || currentSearch !== targetSearch || currentPath !== targetPath) {
            navigate(targetHref, { replace: true });
        }
    }, [
        filters,
        statusCodeToSlug,
        statusSlugJustChanged,
        derivedStatusFromSlug,
        location.pathname,
        location.search,
        navigate,
    ]);

    useEffect(() => {
        previousStatusSlugRef.current = statusSlugFromQuery;
    }, [statusSlugFromQuery]);

    useEffect(() => {
        setSearchDraft(filters.Keyword);
    }, [filters.Keyword]);

    useEffect(() => {
        if (filters.Period === "Custom") {
            if (!filters.FromDate || !filters.ToDate) {
                setError("Select a start and end date to load orders.");
                setOrders([]);
                setPagination((prev) => ({ ...prev, totalCount: 0 }));
                return;
            }

            if (dayjs(filters.FromDate).isAfter(filters.ToDate)) {
                setError("Start date must be before or equal to end date.");
                return;
            }
        }

        let ignore = false;
        const controller = new AbortController();

        const fetchOrders = async () => {
            setLoading(true);
            setError("");
            try {
                const params = buildQueryParams(filters);
                const awaitingShipmentPrefix = "AwaitingShipment";
                const isAwaitingShipmentFilter = (filters.Status ?? "") === awaitingShipmentPrefix;

                if (isAwaitingShipmentFilter) {
                    const matchingStatuses = statuses
                        .map((status) => status?.code)
                        .filter((code) => typeof code === "string" && code.startsWith(awaitingShipmentPrefix));
                    const fallbackStatuses = AWAITING_SHIPMENT_FALLBACK_CODES;
                    const awaitingCodes = matchingStatuses.length > 0 ? matchingStatuses : fallbackStatuses;

                    delete params.Status;
                    params.Statuses = awaitingCodes;
                    params.Status = awaitingCodes.join(",");
                }

                const response = await OrderService.getOrders(params, controller.signal);
                if (ignore) {
                    return;
                }
                const data = response?.data ?? {};
                const items = data.items ?? data.Items ?? [];
                let normalizedOrders = items.map(normalizeOrder);

                if (isAwaitingShipmentFilter) {
                    normalizedOrders = normalizedOrders.filter(
                        (order) =>
                            typeof order?.statusCode === "string" && order.statusCode.startsWith(awaitingShipmentPrefix)
                    );
                }

                setOrders(normalizedOrders);
                setPagination({
                    totalCount: isAwaitingShipmentFilter
                        ? data.totalCount ?? data.TotalCount ?? normalizedOrders.length
                        : data.totalCount ?? data.TotalCount ?? 0,
                    pageNumber: data.pageNumber ?? data.PageNumber ?? filters.PageNumber,
                    pageSize: data.pageSize ?? data.PageSize ?? filters.PageSize,
                });
            } catch (err) {
                if (controller.signal.aborted) {
                    return;
                }
                console.error("Failed to load orders", err);
                setError("Unable to load orders. Please try again.");
                setOrders([]);
                setPagination((prev) => ({ ...prev, totalCount: 0 }));
            } finally {
                if (!ignore) {
                    setLoading(false);
                }
            }
        };

        fetchOrders();

        return () => {
            ignore = true;
            controller.abort();
        };
    }, [filters, statuses]);

    const statusOptions = useMemo(() => {
        const baseOption = [{ value: "", label: "All orders" }];
        const dynamicOptions = statuses.map((status) => ({
            value: status.code ?? "",
            label: status.name ?? "",
        }));
        return [...baseOption, ...dynamicOptions];
    }, [statuses]);

    const currentSortKey = useMemo(() => {
        const match = SORT_OPTIONS.find(
            (option) => option.value === filters.SortBy && option.direction === filters.SortDescending
        );
        return match?.key ?? SORT_OPTIONS[0].key;
    }, [filters.SortBy, filters.SortDescending]);

    const appliedStatus = filters.Status ?? null;

    const updateFilters = (patch, options = {}) => {
        setFilters((prev) => {
            const next = { ...prev, ...patch };
            if (!options.preservePage) {
                next.PageNumber = patch.PageNumber ?? 1;
            }
            return next;
        });
    };

    const handleStatusChange = (event) => {
        const next = event.target.value;
        if ((next || null) === (appliedStatus ?? null)) {
            return;
        }
        updateFilters({ Status: next || null }, {});
    };

    const handlePeriodChange = (event) => {
        const nextPeriod = event.target.value;
        if (nextPeriod === filters.Period) {
            return;
        }
        if (nextPeriod === "Custom") {
            updateFilters({ Period: nextPeriod, FromDate: filters.FromDate, ToDate: filters.ToDate }, {});
        } else {
            updateFilters({ Period: nextPeriod, FromDate: "", ToDate: "" }, {});
        }
    };

    const handleDateChange = (field, value) => {
        if (filters[field] === value) {
            return;
        }
        updateFilters({ [field]: value }, {});
    };

    const handlePageSizeChange = (event) => {
        const nextSize = Number(event.target.value);
        if (nextSize === filters.PageSize) {
            return;
        }
        updateFilters({ PageSize: nextSize }, {});
    };

    const handlePageChange = (direction) => {
        const nextPage = filters.PageNumber + direction;
        if (nextPage < 1) {
            return;
        }
        const maxPage = Math.max(1, Math.ceil(pagination.totalCount / filters.PageSize));
        if (nextPage > maxPage) {
            return;
        }
        updateFilters({ PageNumber: nextPage }, { preservePage: true });
    };

    const handleSortChange = (event) => {
        const selected = SORT_OPTIONS.find((option) => option.key === event.target.value);
        if (!selected) {
            return;
        }
        if (selected.value === filters.SortBy && selected.direction === filters.SortDescending) {
            return;
        }
        updateFilters({ SortBy: selected.value, SortDescending: selected.direction }, {});
    };

    const handleBulkShip = () => {
        if (!hasSelections) {
            return;
        }

        const selectedOrders = orders.filter((order) => order?.id && selectedIdSet.has(order.id));
        if (selectedOrders.length === 0) {
            Notice({ msg: "Select at least one order to continue.", isSuccess: false });
            return;
        }

        navigate("/order/bulk-ship", {
            state: {
                orderIds: selectedOrders.map((order) => order.id),
                orders: selectedOrders,
                from: `${location.pathname}${location.search}`,
            },
        });
    };

    const handleBulkFeedback = () => {
        if (!hasSelections) {
            return;
        }

        const selectedOrders = orders.filter((order) => order?.id && selectedIdSet.has(order.id));
        if (selectedOrders.length === 0) {
            Notice({ msg: "Select at least one order to continue.", isSuccess: false });
            return;
        }

        navigate("/order/bulk-feedback", {
            state: {
                orderIds: selectedOrders.map((order) => order.id),
                orders: selectedOrders,
                from: `${location.pathname}${location.search}`,
            },
        });
    };

    const handleSearchSubmit = (event) => {
        event.preventDefault();
        const trimmed = searchDraft.trim();
        if (trimmed === filters.Keyword.trim()) {
            return;
        }
        updateFilters({ Keyword: trimmed }, {});
    };

    const handleSearchByChange = (event) => {
        const nextValue = event.target.value;
        if (nextValue === filters.SearchBy) {
            return;
        }
        updateFilters({ SearchBy: nextValue }, {});
    };

    const resetFilters = () => {
        setFilters({ ...DEFAULT_FILTER, PageNumber: 1, PageSize: filters.PageSize });
        setSearchDraft("");
    };

    const countOrderItems = (items) => (Array.isArray(items) ? items.length : 0);

    const assignMenuRef = (orderId) => (node) => {
        if (node) {
            menuRefs.current[orderId] = node;
        } else {
            delete menuRefs.current[orderId];
        }
    };

    useEffect(() => {
        if (!openMenuId) {
            return;
        }

        const handleClickOutside = (event) => {
            const menuNode = menuRefs.current[openMenuId];
            if (menuNode && !menuNode.contains(event.target)) {
                setOpenMenuId(null);
            }
        };

        const handleEscape = (event) => {
            if (event.key === "Escape") {
                setOpenMenuId(null);
            }
        };

        document.addEventListener("mousedown", handleClickOutside);
        document.addEventListener("keydown", handleEscape);

        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
            document.removeEventListener("keydown", handleEscape);
        };
    }, [openMenuId]);

    useEffect(() => {
        if (!trackingEditor.isOpen) {
            return;
        }

        const handleEscape = (event) => {
            if (event.key === "Escape") {
                closeTrackingEditor();
            }
        };

        document.addEventListener("keydown", handleEscape);
        return () => {
            document.removeEventListener("keydown", handleEscape);
        };
    }, [trackingEditor.isOpen, closeTrackingEditor]);

    const maxPage = Math.max(1, Math.ceil(pagination.totalCount / filters.PageSize));

    return (
        <div className="orders-page">
            <header className="orders-page__header">
                <h1 className="orders-page__title">Manage all orders</h1>
                <button type="button" className="orders-page__comments">
                    Comments
                </button>
            </header>

            <section className="orders-page__filters">
                <div className="filters__field">
                    <span className="filters__label">Status:</span>
                    <select
                        className="filters__select"
                        value={filters.Status ?? ""}
                        onChange={handleStatusChange}
                    >
                        {statusOptions.map((option) => (
                            <option key={option.value || "__all__"} value={option.value}>
                                {option.label}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="filters__field">
                    <span className="filters__label">Period:</span>
                    <select
                        className="filters__select"
                        value={filters.Period}
                        onChange={handlePeriodChange}
                    >
                        {PERIOD_OPTIONS.map((option) => (
                            <option key={option.value} value={option.value}>
                                {option.label}
                            </option>
                        ))}
                    </select>
                </div>

                {filters.Period === "Custom" && (
                    <div className="filters__custom-range">
                        <label className="filters__label" htmlFor="from-date">
                            From
                        </label>
                        <input
                            id="from-date"
                            type="date"
                            className="filters__input"
                            value={filters.FromDate}
                            onChange={(event) => handleDateChange("FromDate", event.target.value)}
                        />
                        <label className="filters__label" htmlFor="to-date">
                            To
                        </label>
                        <input
                            id="to-date"
                            type="date"
                            className="filters__input"
                            value={filters.ToDate}
                            onChange={(event) => handleDateChange("ToDate", event.target.value)}
                        />
                    </div>
                )}

                <form className="filters__search" onSubmit={handleSearchSubmit}>
                    <div className="filters__field">
                        <span className="filters__label">Search by:</span>
                        <select
                            className="filters__select"
                            value={filters.SearchBy}
                            onChange={handleSearchByChange}
                        >
                            {SEARCH_BY_OPTIONS.map((option) => (
                                <option key={option.value} value={option.value}>
                                    {option.label}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div className="filters__search-input">
                        <input
                            type="search"
                            className="filters__input filters__input--search"
                            placeholder="Search..."
                            value={searchDraft}
                            onChange={(event) => setSearchDraft(event.target.value)}
                        />
                        <button type="submit" className="filters__search-btn" aria-label="Search">
                            <svg
                                width="16"
                                height="16"
                                viewBox="0 0 16 16"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M11.742 10.344l3.387 3.387-1.398 1.398-3.387-3.387a6 6 0 111.398-1.398zM6.5 11a4.5 4.5 0 100-9 4.5 4.5 0 000 9z"
                                    fill="currentColor"
                                />
                            </svg>
                        </button>
                    </div>
                </form>

                <button type="button" className="filters__reset" onClick={resetFilters}>
                    Reset
                </button>
            </section>

            {error && <div className="orders-page__error">{error}</div>}

            <section className="orders-results">
                <div className="orders-results__top">
                    <span className="orders-results__count">Results: {pagination.totalCount}</span>
                    <div className="orders-results__tools">
                        <button
                            type="button"
                            className={`orders-results__link${printDisabled ? " orders-results__link--disabled" : ""}`}
                            onClick={handlePrintOrders}
                            disabled={printDisabled}
                        >
                            Print
                        </button>
                        <span className="orders-results__divider">|</span>
                        <button
                            type="button"
                            className={`orders-results__link${downloadDisabled ? " orders-results__link--disabled" : ""}`}
                            onClick={handleDownloadReport}
                            disabled={downloadDisabled}
                            aria-busy={isGeneratingReport ? "true" : "false"}
                        >
                            {isGeneratingReport ? "Preparing..." : "Download report"}
                        </button>
                        <div className="orders-results__sort">
                            <span className="orders-results__sort-label">Sort by:</span>
                            <select
                                className="orders-results__sort-select"
                                value={currentSortKey}
                                onChange={handleSortChange}
                            >
                                {SORT_OPTIONS.map((option) => (
                                    <option key={option.key} value={option.key}>
                                        {option.label}
                                    </option>
                                ))}
                            </select>
                        </div>
                    </div>
                </div>

                <div className="orders-results__actions">
                    <button
                        type="button"
                        className={`orders-results__action${hasSelections ? " orders-results__action--active" : ""}`}
                        disabled={!hasSelections}
                        onClick={handleBulkShip}
                    >
                        <span className="orders-results__action-label">Shipping</span>
                        <span className="orders-results__action-icon" aria-hidden="true">
                            <svg
                                width="8"
                                height="5"
                                viewBox="0 0 8 5"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M1 1.25L4 4.25L7 1.25"
                                    stroke="currentColor"
                                    strokeWidth="1.2"
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                />
                            </svg>
                        </span>
                    </button>
                    <button
                        type="button"
                        className={`orders-results__action${hasSelections ? " orders-results__action--active" : ""}`}
                        disabled={!hasSelections}
                    >
                        <span className="orders-results__action-label">Print coupon</span>
                    </button>
                    <button
                        type="button"
                        className={`orders-results__action${hasSelections ? " orders-results__action--active" : ""}`}
                        disabled={!hasSelections}
                    >
                        <span className="orders-results__action-label">Relist</span>
                    </button>
                    <button
                        type="button"
                        className={`orders-results__action${hasSelections ? " orders-results__action--active" : ""}`}
                        disabled={!hasSelections}
                        onClick={handleBulkFeedback}
                    >
                        <span className="orders-results__action-label">Leave feedback</span>
                    </button>
                    <button
                        type="button"
                        className={`orders-results__action${hasSelections ? " orders-results__action--active" : ""}`}
                        disabled={!hasSelections}
                    >
                        <span className="orders-results__action-label">More</span>
                        <span className="orders-results__action-icon" aria-hidden="true">
                            <svg
                                width="8"
                                height="5"
                                viewBox="0 0 8 5"
                                fill="none"
                                xmlns="http://www.w3.org/2000/svg"
                            >
                                <path
                                    d="M1 1.25L4 4.25L7 1.25"
                                    stroke="currentColor"
                                    strokeWidth="1.2"
                                    strokeLinecap="round"
                                    strokeLinejoin="round"
                                />
                            </svg>
                        </span>
                    </button>
                </div>

                <div className="orders-results__table-wrapper">
                    <table className="orders-table">
                        <thead>
                            <tr>
                                <th className="orders-table__selection-header">
                                    <div className="orders-table__selection-control">
                                        <input
                                            ref={masterCheckboxRef}
                                            type="checkbox"
                                            className="orders-table__checkbox"
                                            aria-label="Select all orders on this page"
                                            checked={allOrdersSelected}
                                            disabled={selectableOrderIds.length === 0}
                                            onChange={handleToggleSelectAll}
                                        />
                                    </div>
                                </th>
                                <th>Actions</th>
                                <th>Order</th>
                                <th>Items</th>
                                <th>Subtotal</th>
                                <th>Total</th>
                                <th>Date sold</th>
                                <th>Date paid</th>
                            </tr>
                        </thead>
                        <tbody>
                            {loading ? (
                                <tr className="orders-table__loading-row">
                                    <td colSpan={8}>
                                        <div className="orders-table__loading">
                                            <LoadingScreen isLoadingTable={true} />
                                            <span className="orders-table__loading-text">Loading orders...</span>
                                        </div>
                                    </td>
                                </tr>
                            ) : orders.length === 0 ? (
                                <tr>
                                    <td colSpan={8} className="orders-table__empty-row">
                                        We didn't find any results. Try searching with different criteria.
                                    </td>
                                </tr>
                            ) : (
                                orders.map((order) => {
                                    const items = Array.isArray(order.items) ? order.items : [];
                                    const isMenuOpen = openMenuId === order.id;
                                    const orderStatusStyle = order.statusColor
                                        ? { backgroundColor: order.statusColor }
                                        : undefined;
                                    const currentPendingAction =
                                        pendingAction?.orderId === order.id ? pendingAction.actionKey : null;
                                    const allowDeliveryUpdates = DELIVERABLE_STATUS_CODES.has(order.statusCode);
                                    const showMarkDelivered = allowDeliveryUpdates && order.statusCode !== "Delivered";
                                    const showMarkFailed = allowDeliveryUpdates && order.statusCode !== "DeliveryFailed";
                                    const showCancelButton =
                                        CANCELLABLE_SHIPPING_STATUSES.has(order.shippingStatus) &&
                                        order.statusCode !== "Cancelled";
                                    const isSelected = selectedIdSet.has(order.id);
                                    const rowClassName = `orders-table__row${isSelected ? " orders-table__row--selected" : ""}`;
                                    const dynamicMenuActions = [];

                                    const shipmentsByItemId = new Map();
                                    (order.shipments ?? []).forEach((shipment) => {
                                        const shipmentOrderItemId = shipment?.orderItemId ?? shipment?.OrderItemId ?? null;
                                        if (!shipmentOrderItemId) {
                                            return;
                                        }
                                        const key = shipmentOrderItemId.toString().toLowerCase();
                                        if (!shipmentsByItemId.has(key)) {
                                            shipmentsByItemId.set(key, []);
                                        }
                                        shipmentsByItemId.get(key).push(shipment);
                                    });

                                    if (showCancelButton) {
                                        dynamicMenuActions.push({
                                            key: "cancel-order",
                                            label: "Cancel order",
                                            onClick: () => handleNavigateToCancellation(order),
                                            disabled: Boolean(currentPendingAction),
                                        });
                                    }

                                    if (showMarkDelivered) {
                                        dynamicMenuActions.push({
                                            key: "mark-delivered",
                                            label:
                                                currentPendingAction === "delivered"
                                                    ? "Updating..."
                                                    : "Mark delivered",
                                            onClick: () => handleMarkDelivered(order),
                                            disabled: Boolean(currentPendingAction),
                                        });
                                    }

                                    if (showMarkFailed) {
                                        dynamicMenuActions.push({
                                            key: "mark-delivery-failed",
                                            label:
                                                currentPendingAction === "delivery-failed"
                                                    ? "Updating..."
                                                    : "Delivery failed",
                                            onClick: () => handleMarkDeliveryFailed(order),
                                            disabled: Boolean(currentPendingAction),
                                        });
                                    }

                                    const combinedMenuActions = [
                                        ...dynamicMenuActions,
                                        ...ROW_MENU_ACTIONS.map((action) => ({
                                            key: action.key,
                                            label: action.label,
                                            onClick: () => {
                                                handleRowMenuAction(order, action.key);
                                            },
                                            disabled: false,
                                        })),
                                    ];

                                    return (
                                        <tr key={order.id} className={rowClassName}>
                                            <td className="orders-table__selection-cell">
                                                <div className="orders-table__selection-control">
                                                    <input
                                                        type="checkbox"
                                                        className="orders-table__checkbox"
                                                        checked={isSelected}
                                                        onChange={() => handleToggleOrderSelection(order.id)}
                                                        aria-label={`Select order ${order.orderNumber || order.id}`}
                                                    />
                                                </div>
                                            </td>
                                            <td className="orders-table__actions-cell">
                                                <button
                                                    type="button"
                                                    className="orders-table__primary-action"
                                                    onClick={() => handleNavigateToShipping(order)}
                                                    disabled={Boolean(currentPendingAction)}
                                                >
                                                    Get shipping label
                                                </button>
                                                <div
                                                    className={`orders-table__menu-wrapper${isMenuOpen ? " orders-table__menu-wrapper--open" : ""
                                                        }`}
                                                    ref={assignMenuRef(order.id)}
                                                >
                                                    <button
                                                        type="button"
                                                        className="orders-table__menu-trigger"
                                                        onClick={() => handleToggleMenu(order.id)}
                                                        aria-haspopup="menu"
                                                        aria-expanded={isMenuOpen}
                                                    >
                                                        <span className="orders-table__sr-only">More actions</span>
                                                        <svg
                                                            width="20"
                                                            height="20"
                                                            viewBox="0 0 20 20"
                                                            fill="none"
                                                            xmlns="http://www.w3.org/2000/svg"
                                                            aria-hidden="true"
                                                        >
                                                            <circle cx="4" cy="10" r="1.6" fill="currentColor" />
                                                            <circle cx="10" cy="10" r="1.6" fill="currentColor" />
                                                            <circle cx="16" cy="10" r="1.6" fill="currentColor" />
                                                        </svg>
                                                    </button>
                                                    {isMenuOpen && (
                                                        <ul className="orders-table__menu" role="menu">
                                                            {combinedMenuActions.map((action) => (
                                                                <li key={action.key} className="orders-table__menu-item" role="none">
                                                                    <button
                                                                        type="button"
                                                                        className="orders-table__menu-button"
                                                                        role="menuitem"
                                                                        onClick={() => {
                                                                            if (action.disabled) {
                                                                                return;
                                                                            }
                                                                            setOpenMenuId(null);
                                                                            action.onClick();
                                                                        }}
                                                                        disabled={action.disabled}
                                                                    >
                                                                        {action.label}
                                                                    </button>
                                                                </li>
                                                            ))}
                                                        </ul>
                                                    )}
                                                </div>
                                            </td>
                                            <td className="orders-table__order-cell">
                                                <div className="orders-table__order-meta">
                                                    <div className="orders-table__order-header">
                                                        <button
                                                            type="button"
                                                            className="orders-table__order-link"
                                                            onClick={() => handleNavigateToOrderDetail(order)}
                                                        >
                                                            <span className="orders-table__order-number">
                                                                Order {order.orderNumber || "-"}
                                                            </span>
                                                        </button>
                                                        {order.statusName && (
                                                            <span
                                                                className="orders-table__status-badge"
                                                                style={orderStatusStyle}
                                                            >
                                                                {order.statusName}
                                                            </span>
                                                        )}
                                                    </div>
                                                    <div className="orders-table__buyer">
                                                        <span>{order.buyerFullName || order.buyerUsername || "Unknown buyer"}</span>
                                                        {order.buyerFullName && order.buyerUsername && (
                                                            <span className="orders-table__buyer-handle">@{order.buyerUsername}</span>
                                                        )}
                                                    </div>
                                                </div>

                                                {items.length === 0 ? (
                                                    <div className="orders-table__no-items">No items were found for this order.</div>
                                                ) : (
                                                    <div className="orders-table__items">
                                                        {items.map((item, index) => {
                                                            const rawOrderItemId = item?.id ?? item?.orderItemId ?? item?.OrderItemId ?? null;
                                                            const lookupId = rawOrderItemId ? rawOrderItemId.toString().toLowerCase() : null;
                                                            const itemShipments = lookupId ? shipmentsByItemId.get(lookupId) ?? [] : [];
                                                            const trackingButtonLabel = itemShipments.length > 0 ? "Manage tracking" : "+ Add tracking";
                                                            const trackingDisabled = !rawOrderItemId;
                                                            const itemKey = rawOrderItemId ? rawOrderItemId.toString() : `${order.id}-${index}`;

                                                            return (
                                                                <div key={itemKey} className="orders-table__item">
                                                                    <div className="orders-table__item-thumb">
                                                                        {item?.imageUrl || item?.ImageUrl ? (
                                                                            <img
                                                                                src={item.imageUrl ?? item.ImageUrl}
                                                                                alt={item.title ?? item.Title ?? "Order item"}
                                                                                loading="lazy"
                                                                            />
                                                                        ) : (
                                                                            <div className="orders-table__item-placeholder">No image</div>
                                                                        )}
                                                                    </div>
                                                                    <div className="orders-table__item-content">
                                                                        <div className="orders-table__item-header">
                                                                            <span className="orders-table__item-title">
                                                                                {item.title ?? item.Title ?? "Untitled item"}
                                                                            </span>
                                                                        </div>
                                                                        {item?.sku && (
                                                                            <div className="orders-table__item-meta">SKU: {item.sku}</div>
                                                                        )}
                                                                        {itemShipments.length > 0 && (
                                                                            <div className="orders-table__shipment-list">
                                                                                {itemShipments.map((shipment) => (
                                                                                    <span
                                                                                        key={shipment.id ?? shipment.trackingNumber}
                                                                                        className="orders-table__shipment-pill"
                                                                                    >
                                                                                        <span>{shipment.trackingNumber || "No tracking"}</span>
                                                                                        {shipment.carrier && <span> • {shipment.carrier}</span>}
                                                                                    </span>
                                                                                ))}
                                                                            </div>
                                                                        )}
                                                                        <button
                                                                            type="button"
                                                                            className="orders-table__item-tracking-btn"
                                                                            onClick={() => handleOpenTrackingEditor(order, item)}
                                                                            disabled={trackingDisabled}
                                                                        >
                                                                            {trackingButtonLabel}
                                                                        </button>
                                                                    </div>
                                                                </div>
                                                            );
                                                        })}
                                                    </div>
                                                )}
                                            </td>
                                            <td>{countOrderItems(order.items)}</td>
                                            <td>{formatCurrency(order.subTotal)}</td>
                                            <td>{formatCurrency(order.total)}</td>
                                            <td>{formatDate(order.orderedAt)}</td>
                                            <td>{formatDate(order.paidAt)}</td>
                                        </tr>
                                    );
                                })
                            )}
                        </tbody>
                    </table>
                </div>
            </section>

            <footer className="orders-page__footer">
                {maxPage > 1 && (
                    <div className="orders-page__pager">
                        <button
                            type="button"
                            className="orders-page__pager-btn"
                            onClick={() => handlePageChange(-1)}
                            disabled={filters.PageNumber <= 1 || loading}
                        >
                            Previous
                        </button>
                        <span className="orders-page__pager-info">
                            Page {filters.PageNumber} of {maxPage}
                        </span>
                        <button
                            type="button"
                            className="orders-page__pager-btn"
                            onClick={() => handlePageChange(1)}
                            disabled={filters.PageNumber >= maxPage || loading}
                        >
                            Next
                        </button>
                    </div>
                )}

                <div className="orders-page__page-size">
                    <span className="orders-page__page-size-label">Items per page:</span>
                    <select
                        className="orders-page__page-size-select"
                        value={filters.PageSize}
                        onChange={handlePageSizeChange}
                    >
                        {PAGE_SIZE_OPTIONS.map((size) => (
                            <option key={size} value={size}>
                                {size}
                            </option>
                        ))}
                    </select>
                </div>
            </footer>

            {trackingEditor.isOpen && (
                <div
                    className="orders-table__dialog-backdrop"
                    onMouseDown={handleTrackingBackdropClick}
                >
                    <form
                        className="orders-table__dialog"
                        role="dialog"
                        aria-modal="true"
                        aria-labelledby="orders-table-tracking-title"
                        onMouseDown={(event) => event.stopPropagation()}
                        onSubmit={handleTrackingSubmit}
                    >
                        <div className="orders-table__dialog-header">
                            <div>
                                <h2 className="orders-table__dialog-title" id="orders-table-tracking-title">
                                    {trackingEditor.existingShipments.length > 0 ? "Manage tracking" : "Add tracking"}
                                </h2>
                                {trackingEditor.orderNumber && (
                                    <div className="orders-table__dialog-order">
                                        Order {trackingEditor.orderNumber}
                                    </div>
                                )}
                            </div>
                            <button
                                type="button"
                                className="orders-table__dialog-close"
                                onClick={closeTrackingEditor}
                                aria-label="Close add tracking dialog"
                            >
                                <svg width="16" height="16" viewBox="0 0 16 16" aria-hidden="true">
                                    <path
                                        d="M3.22 3.22a.75.75 0 011.06 0L8 6.94l3.72-3.72a.75.75 0 111.06 1.06L9.06 8l3.72 3.72a.75.75 0 11-1.06 1.06L8 9.06l-3.72 3.72a.75.75 0 11-1.06-1.06L6.94 8 3.22 4.28a.75.75 0 010-1.06z"
                                        fill="currentColor"
                                    />
                                </svg>
                            </button>
                        </div>

                        <div className="orders-table__dialog-body">
                            <div className="orders-table__dialog-item">
                                <div className="orders-table__dialog-thumb">
                                    {trackingEditor.itemImage ? (
                                        <img
                                            src={trackingEditor.itemImage}
                                            alt={trackingEditor.itemTitle || "Order item"}
                                            loading="lazy"
                                        />
                                    ) : (
                                        <div className="orders-table__item-placeholder">No image</div>
                                    )}
                                </div>
                                <div className="orders-table__dialog-item-content">
                                    <span className="orders-table__dialog-item-title">
                                        {trackingEditor.itemTitle || "Untitled item"}
                                    </span>
                                </div>
                            </div>

                            {trackingEditor.existingShipments.length > 0 && (
                                <div className="orders-table__dialog-section">
                                    <span className="orders-table__dialog-label">Existing tracking</span>
                                    <div className="orders-table__dialog-existing">
                                        {trackingEditor.existingShipments.map((shipment) => (
                                            <div key={shipment.id ?? shipment.trackingNumber}>
                                                {shipment.trackingNumber || "No tracking"}
                                                {shipment.carrier && ` • ${shipment.carrier}`}
                                            </div>
                                        ))}
                                    </div>
                                </div>
                            )}

                            <div className="orders-table__dialog-section">
                                <label className="orders-table__dialog-label" htmlFor="tracking-number-input">
                                    Tracking number
                                </label>
                                <input
                                    id="tracking-number-input"
                                    className="orders-table__dialog-input"
                                    value={trackingEditor.trackingNumber}
                                    onChange={(event) => handleTrackingFieldChange("trackingNumber", event.target.value)}
                                    placeholder="Enter tracking number"
                                    autoFocus
                                />
                            </div>

                            <div className="orders-table__dialog-section">
                                <label className="orders-table__dialog-label" htmlFor="tracking-carrier-input">
                                    Carrier
                                </label>
                                <input
                                    id="tracking-carrier-input"
                                    className="orders-table__dialog-input"
                                    value={trackingEditor.carrier}
                                    onChange={(event) => handleTrackingFieldChange("carrier", event.target.value)}
                                    placeholder="Enter carrier (e.g., USPS)"
                                />
                            </div>

                            <div className="orders-table__dialog-section">
                                <label className="orders-table__dialog-label" htmlFor="tracking-date-input">
                                    Ship date
                                </label>
                                <input
                                    id="tracking-date-input"
                                    className="orders-table__dialog-input"
                                    type="date"
                                    value={trackingEditor.shippedAt}
                                    onChange={(event) => handleTrackingFieldChange("shippedAt", event.target.value)}
                                />
                            </div>

                            {trackingEditor.error && (
                                <div className="orders-table__dialog-error">{trackingEditor.error}</div>
                            )}

                            <div className="orders-table__dialog-actions">
                                <button
                                    type="button"
                                    className="orders-table__dialog-button orders-table__dialog-button--ghost"
                                    onClick={closeTrackingEditor}
                                    disabled={trackingEditor.isSubmitting}
                                >
                                    Cancel
                                </button>
                                <button
                                    type="submit"
                                    className="orders-table__dialog-button orders-table__dialog-button--primary"
                                    disabled={trackingEditor.isSubmitting}
                                >
                                    {trackingEditor.isSubmitting ? "Saving..." : "Save tracking"}
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            )}
        </div>
    );
};

export default AllOrdersPage;