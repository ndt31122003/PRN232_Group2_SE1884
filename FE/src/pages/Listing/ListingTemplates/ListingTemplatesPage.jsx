import { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import ListingTemplateService from "../../../services/ListingTemplateService";
import Notice from "../../../components/Common/CustomNotification";

import "../AllListingsPage.css";
import "./ListingTemplatesPage.css";

const PAGE_SIZE = 20;
const TEMPLATE_LIMIT = 50;

const formatDate = (raw) => {
    if (!raw) {
        return "--";
    }

    try {
        const date = new Date(raw);
        if (Number.isNaN(date.getTime())) {
            return "--";
        }

        return date.toLocaleString(undefined, {
            year: "numeric",
            month: "short",
            day: "numeric",
            hour: "2-digit",
            minute: "2-digit"
        });
    } catch (error) {
        return "--";
    }
};

const safeParseJson = (raw) => {
    if (!raw || typeof raw !== "string") {
        return null;
    }

    try {
        return JSON.parse(raw);
    } catch (error) {
        return null;
    }
};

const normalizeMediaEntry = (media, seed, fallbackName) => {
    if (!media) {
        return null;
    }

    if (typeof media === "string") {
        return {
            id: `template-media-${seed}-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`,
            name: fallbackName ?? (typeof seed === "number" && seed === 0 ? "Primary photo" : "Photo"),
            type: "image",
            src: media,
            preview: null,
            size: null,
            uploading: false
        };
    }

    const urlCandidate =
        typeof media.url === "string" && media.url.trim().length
            ? media.url
            : typeof media.src === "string" && media.src.trim().length
                ? media.src
                : null;

    if (!urlCandidate) {
        return null;
    }

    return {
        id: media.id ?? `template-media-${seed}-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`,
        name:
            media.name ??
            fallbackName ??
            (typeof seed === "number" && seed === 0 ? "Primary photo" : "Photo"),
        type: media.type ?? "image",
        src: urlCandidate,
        preview: media.preview ?? null,
        size: media.size ?? null,
        uploading: false
    };
};

const deriveScheduleFields = (scheduledStartTime) => {
    const defaults = {
        scheduleEnabled: false,
        date: "",
        hour: "12",
        minute: "00",
        ampm: "AM"
    };

    if (!scheduledStartTime) {
        return defaults;
    }

    const scheduled = new Date(scheduledStartTime);
    if (Number.isNaN(scheduled.getTime())) {
        return defaults;
    }

    const minutes = String(scheduled.getMinutes()).padStart(2, "0");
    const hours24 = scheduled.getHours();
    const ampm = hours24 >= 12 ? "PM" : "AM";
    let hourValue = hours24 % 12;
    if (hourValue === 0) {
        hourValue = 12;
    }

    return {
        scheduleEnabled: true,
        date: scheduled.toISOString().slice(0, 10),
        hour: String(hourValue).padStart(2, "0"),
        minute: minutes,
        ampm
    };
};

const normalizeFormState = (formState) => {
    if (!formState || typeof formState !== "object") {
        return null;
    }

    const files = Array.isArray(formState.files)
        ? formState.files
            .map((item, index) => normalizeMediaEntry(item, index))
            .filter(Boolean)
        : [];

    const scheduleEnabled = Boolean(formState.scheduleEnabled);

    return {
        listingId: null,
        listingStatus: "Draft",
        files,
        title: formState.title ?? "",
        sku: formState.sku ?? "",
        listingDescription: formState.listingDescription ?? "",
        conditionDescription: formState.conditionDescription ?? "",
        startingBid: formState.startingBid ?? "",
        buyItNowPrice: formState.buyItNowPrice ?? "",
        reservePrice: formState.reservePrice ?? "",
        fixedPrice: formState.fixedPrice ?? "",
        minimumOffer: formState.minimumOffer ?? "",
        autoAcceptOffer: formState.autoAcceptOffer ?? "",
        selectedCategoryId: formState.selectedCategoryId ?? null,
        categoryPath: Array.isArray(formState.categoryPath) ? formState.categoryPath : [],
        categoryDetail: formState.categoryDetail ?? null,
        selectedConditionId: formState.selectedConditionId ?? null,
        conditions: Array.isArray(formState.conditions) ? formState.conditions : [],
        specificSelections: formState.specificSelections ?? {},
        customSpecificInputs: formState.customSpecificInputs ?? {},
        openSpecificId: formState.openSpecificId ?? null,
        quantity: formState.quantity != null ? String(formState.quantity) : "",
        priceFormat: formState.priceFormat ?? "1",
        auctionDuration: formState.auctionDuration ?? "7",
        scheduleEnabled,
        date: scheduleEnabled ? (formState.date ?? "") : "",
        hour: scheduleEnabled ? (formState.hour ?? "12") : "12",
        minute: scheduleEnabled ? (formState.minute ?? "00") : "00",
        ampm: scheduleEnabled ? (formState.ampm ?? "AM") : "AM",
        allowOfferEnabled: Boolean(formState.allowOfferEnabled)
    };
};

const deriveDraftFromListingPayload = (listingPayload) => {
    if (!listingPayload || typeof listingPayload !== "object") {
        return null;
    }

    const files = Array.isArray(listingPayload.listingImages)
        ? listingPayload.listingImages
            .map((item, index) => normalizeMediaEntry(item, index))
            .filter(Boolean)
        : [];

    const allowOffers = Boolean(listingPayload.allowOffers);
    const scheduleFields = deriveScheduleFields(listingPayload.scheduledStartTime);

    const priceFormat = (() => {
        if (Array.isArray(listingPayload.variations) && listingPayload.variations.length > 0) {
            return "2";
        }

        if (listingPayload.price != null && listingPayload.price !== "") {
            return "2";
        }

        return "1";
    })();

    return {
        listingId: null,
        listingStatus: "Draft",
        files,
        title: listingPayload.title ?? "",
        sku: listingPayload.sku ?? "",
        listingDescription: listingPayload.listingDescription ?? "",
        conditionDescription: listingPayload.conditionDescription ?? "",
        startingBid:
            listingPayload.startPrice != null ? String(listingPayload.startPrice) : "",
        buyItNowPrice:
            listingPayload.buyItNowPrice != null ? String(listingPayload.buyItNowPrice) : "",
        reservePrice:
            listingPayload.reservePrice != null ? String(listingPayload.reservePrice) : "",
        fixedPrice: listingPayload.price != null ? String(listingPayload.price) : "",
        minimumOffer:
            allowOffers && listingPayload.minimumOffer != null
                ? String(listingPayload.minimumOffer)
                : "",
        autoAcceptOffer:
            allowOffers && listingPayload.autoAcceptOffer != null
                ? String(listingPayload.autoAcceptOffer)
                : "",
        selectedCategoryId: listingPayload.categoryId ?? null,
        categoryPath: Array.isArray(listingPayload.categoryPath) ? listingPayload.categoryPath : [],
        categoryDetail: listingPayload.categoryDetail ?? null,
        selectedConditionId: listingPayload.conditionId ?? null,
        conditions: Array.isArray(listingPayload.conditions) ? listingPayload.conditions : [],
        specificSelections: listingPayload.specificSelections ?? {},
        customSpecificInputs: listingPayload.customSpecificInputs ?? {},
        openSpecificId: listingPayload.openSpecificId ?? null,
        quantity: listingPayload.quantity != null ? String(listingPayload.quantity) : "",
        priceFormat,
        auctionDuration:
            listingPayload.duration != null ? String(listingPayload.duration) : "7",
        scheduleEnabled: scheduleFields.scheduleEnabled,
        date: scheduleFields.date,
        hour: scheduleFields.hour,
        minute: scheduleFields.minute,
        ampm: scheduleFields.ampm,
        allowOfferEnabled: allowOffers
    };
};

const buildVariationKey = (attributeValues, fallbackIndex) => {
    const entries = Object.entries(attributeValues ?? {})
        .map(([name, value]) => [String(name ?? ""), String(value ?? "")])
        .filter(([, value]) => value.length > 0)
        .sort(([a], [b]) => a.localeCompare(b));

    if (!entries.length) {
        return `variation-${fallbackIndex}`;
    }

    return entries.map(([name, value]) => `${name}:${value}`).join("|");
};

const buildVariationStateFromListingPayload = (listingPayload) => {
    if (!Array.isArray(listingPayload?.variations) || listingPayload.variations.length === 0) {
        return null;
    }

    const attributeMap = new Map();
    const optionMap = new Map();

    const variationRows = listingPayload.variations
        .map((variation, index) => {
            const attributeValues = {};

            (variation?.variationSpecifics ?? []).forEach((specific) => {
                const name = specific?.name;
                const values = Array.isArray(specific?.values) ? specific.values : [];
                const firstValue = values.length ? String(values[0]).trim() : "";

                if (!name || !firstValue) {
                    return;
                }

                attributeValues[name] = firstValue;

                if (!optionMap.has(name)) {
                    optionMap.set(name, new Set());
                }
                optionMap.get(name).add(firstValue);

                if (!attributeMap.has(name)) {
                    const attributeId =
                        `attr-${name.toLowerCase().replace(/[^a-z0-9]+/g, "-") || Math.random().toString(36).slice(2, 10)}`;
                    attributeMap.set(name, { id: attributeId, name });
                }
            });

            if (!Object.keys(attributeValues).length) {
                return null;
            }

            const photoSource = Array.isArray(variation?.variationImages)
                ? variation.variationImages.find((image) => image?.isPrimary)?.url ??
                variation.variationImages[0]?.url
                : null;

            const photo = photoSource
                ? normalizeMediaEntry({ url: photoSource, name: "Variation photo" }, `variation-${index}`, "Variation photo")
                : null;

            return {
                key: buildVariationKey(attributeValues, index),
                attributeValues,
                price: variation?.price != null ? String(variation.price) : "",
                quantity: variation?.quantity != null ? String(variation.quantity) : "",
                sku: variation?.sku ?? "",
                photo
            };
        })
        .filter(Boolean);

    if (!variationRows.length) {
        return null;
    }

    const attributes = Array.from(attributeMap.values()).map((attribute, index) => ({
        ...attribute,
        selected: index === 0
    }));

    const selectedOptions = {};
    optionMap.forEach((set, name) => {
        selectedOptions[name] = Array.from(set);
    });

    const defaultPhotos = Array.isArray(listingPayload.listingImages)
        ? listingPayload.listingImages
            .map((item, index) => normalizeMediaEntry(item, index))
            .filter(Boolean)
        : [];

    return {
        attributes,
        selectedOptions,
        variationRows,
        excludedKeys: [],
        defaultPhotos,
        categoryId: listingPayload.categoryId ?? null,
        categoryName: listingPayload.categoryDetail?.name ?? null,
        mode: variationRows.length && attributes.length ? "summary" : "edit"
    };
};

const normalizeVariationState = (state) => {
    if (!state || typeof state !== "object") {
        return null;
    }

    const attributes = Array.isArray(state.attributes)
        ? state.attributes
            .map((attribute, index) => ({
                id: attribute?.id ?? `attr-${index}-${Math.random().toString(36).slice(2, 10)}`,
                name: attribute?.name ?? "",
                selected: Boolean(attribute?.selected)
            }))
            .filter((attribute) => attribute.id && attribute.name)
        : [];

    const variationRows = Array.isArray(state.variationRows)
        ? state.variationRows
            .map((row, index) => {
                const attributeValues =
                    row?.attributeValues && typeof row.attributeValues === "object"
                        ? Object.fromEntries(
                            Object.entries(row.attributeValues).map(([key, value]) => [
                                key,
                                String(value ?? "")
                            ])
                        )
                        : {};

                if (!Object.keys(attributeValues).length) {
                    return null;
                }

                return {
                    key: row?.key ?? buildVariationKey(attributeValues, index),
                    attributeValues,
                    price: row?.price != null ? String(row.price) : "",
                    quantity: row?.quantity != null ? String(row.quantity) : "",
                    sku: row?.sku ?? "",
                    photo: normalizeMediaEntry(row?.photo, `variation-${index}`, "Variation photo")
                };
            })
            .filter(Boolean)
        : [];

    if (!variationRows.length) {
        return null;
    }

    const selectedOptions =
        state.selectedOptions && typeof state.selectedOptions === "object"
            ? Object.fromEntries(
                Object.entries(state.selectedOptions).map(([key, value]) => [
                    key,
                    Array.isArray(value)
                        ? value.map((option) => String(option ?? "")).filter(Boolean)
                        : []
                ])
            )
            : {};

    const defaultPhotos = Array.isArray(state.defaultPhotos)
        ? state.defaultPhotos
            .map((item, index) => normalizeMediaEntry(item, index))
            .filter(Boolean)
        : [];

    return {
        attributes,
        selectedOptions,
        variationRows,
        excludedKeys: Array.isArray(state.excludedKeys) ? state.excludedKeys : [],
        defaultPhotos,
        categoryId: state.categoryId ?? null,
        categoryName: state.categoryName ?? null,
        mode: state.mode === "summary" ? "summary" : "edit"
    };
};

const prepareTemplateState = (template) => {
    if (!template) {
        return { listingDraft: null, variationData: null };
    }

    const parsedPayload = safeParseJson(template.payloadJson);
    if (!parsedPayload) {
        return { listingDraft: null, variationData: null };
    }

    const isComposite =
        parsedPayload &&
        typeof parsedPayload === "object" &&
        (parsedPayload.version === 2 || parsedPayload.formState || parsedPayload.listingRequest || parsedPayload.listing);

    const listingPayload = isComposite
        ? parsedPayload.listingRequest ?? parsedPayload.listing ?? parsedPayload.payload ?? null
        : parsedPayload;

    const formState = isComposite ? normalizeFormState(parsedPayload.formState) : null;
    const variationState = isComposite ? normalizeVariationState(parsedPayload.variationState) : null;

    const listingDraft = formState ?? deriveDraftFromListingPayload(listingPayload);
    const variationData = variationState ?? buildVariationStateFromListingPayload(listingPayload);

    if (!listingDraft) {
        return { listingDraft: null, variationData: variationData ?? null };
    }

    return {
        listingDraft: {
            ...listingDraft,
            listingId: null,
            listingStatus: "Draft"
        },
        variationData: variationData ?? null
    };
};

const ListingTemplatesPage = () => {
    const navigate = useNavigate();

    const [searchInput, setSearchInput] = useState("");
    const [searchTerm, setSearchTerm] = useState("");
    const [pageNumber, setPageNumber] = useState(1);
    const [isLoading, setIsLoading] = useState(false);
    const [pendingDeleteId, setPendingDeleteId] = useState(null);

    const [templates, setTemplates] = useState([]);
    const [totalCount, setTotalCount] = useState(0);
    const [refreshKey, setRefreshKey] = useState(0);
    const [openMenuId, setOpenMenuId] = useState(null);

    const pageSize = PAGE_SIZE;
    const actionContainersRef = useRef(new Map());

    const registerActionContainer = (id, element) => {
        const registry = actionContainersRef.current;
        if (!registry) {
            return;
        }

        if (element) {
            registry.set(id, element);
        } else {
            registry.delete(id);
        }
    };

    useEffect(() => {
        if (!openMenuId) {
            return undefined;
        }

        const handleDocumentClick = (event) => {
            const registry = actionContainersRef.current;
            const container = registry?.get(openMenuId);
            if (container && !container.contains(event.target)) {
                setOpenMenuId(null);
            }
        };

        const handleEsc = (event) => {
            if (event.key === "Escape") {
                setOpenMenuId(null);
            }
        };

        document.addEventListener("mousedown", handleDocumentClick);
        document.addEventListener("keydown", handleEsc);

        return () => {
            document.removeEventListener("mousedown", handleDocumentClick);
            document.removeEventListener("keydown", handleEsc);
        };
    }, [openMenuId]);

    useEffect(() => {
        let isMounted = true;

        const loadTemplates = async () => {
            setIsLoading(true);
            try {
                const response = await ListingTemplateService.list({
                    searchTerm: searchTerm ? searchTerm : undefined,
                    pageNumber,
                    pageSize
                });

                if (!isMounted) {
                    return;
                }

                const items = Array.isArray(response?.items) ? response.items : [];
                const count = Number(response?.totalCount ?? response?.TotalCount ?? 0);

                const maxStartIndex = (pageNumber - 1) * pageSize;
                if (pageNumber > 1 && count <= maxStartIndex) {
                    setPageNumber((prev) => Math.max(1, prev - 1));
                    return;
                }

                setTemplates(items);
                setTotalCount(count);
            } catch (error) {
                if (isMounted) {
                    // eslint-disable-next-line no-console
                    console.error("Failed to load templates", error);
                    Notice({ msg: "Unable to load templates.", isSuccess: false });
                    setTemplates([]);
                    setTotalCount(0);
                }
            } finally {
                if (isMounted) {
                    setIsLoading(false);
                }
            }
        };

        loadTemplates();

        return () => {
            isMounted = false;
        };
    }, [pageNumber, pageSize, refreshKey, searchTerm]);

    const totalPages = useMemo(
        () => Math.max(1, Math.ceil((totalCount || 0) / pageSize)),
        [totalCount, pageSize]
    );

    const currentPage = useMemo(
        () => Math.min(pageNumber, totalPages),
        [pageNumber, totalPages]
    );

    const resultsLabel = useMemo(() => {
        const count = Number.isFinite(totalCount) ? totalCount : 0;
        return count === 1 ? "1 template" : `${count} templates`;
    }, [totalCount]);

    const isAtTemplateLimit = totalCount >= TEMPLATE_LIMIT;

    const handleSearchInputChange = (event) => {
        setSearchInput(event.target.value);
    };

    const handleSearchSubmit = (event) => {
        event.preventDefault();
        const trimmed = searchInput.trim();
        setPageNumber(1);
        setSearchTerm(trimmed);
    };

    const handleClearSearch = () => {
        setSearchInput("");
        setSearchTerm("");
        setPageNumber(1);
    };

    const handlePrevPage = () => {
        setPageNumber((prev) => Math.max(1, prev - 1));
    };

    const handleNextPage = () => {
        setPageNumber((prev) => {
            const next = Math.min(totalPages, prev + 1);
            return next === prev ? prev : next;
        });
    };

    const closeMenu = () => setOpenMenuId(null);

    const handleCreateListingFromTemplate = async (templateId) => {
        try {
            const template = await ListingTemplateService.getById(templateId);
            if (!template) {
                Notice({ msg: "Template not found.", isSuccess: false });
                return;
            }

            const { listingDraft, variationData } = prepareTemplateState(template);

            if (!listingDraft) {
                Notice({ msg: "Template payload is invalid.", isSuccess: false });
                return;
            }

            navigate("/listing-form", {
                state: {
                    listingDraft,
                    variationData,
                    fromTemplateId: templateId
                }
            });
        } catch (error) {
            // eslint-disable-next-line no-console
            console.error("Failed to apply template", error);
            Notice({ msg: "Unable to load template payload.", isSuccess: false });
        } finally {
            closeMenu();
        }
    };

    const handleDeleteTemplate = async (templateId) => {
        if (!templateId || pendingDeleteId) {
            return;
        }

        setPendingDeleteId(templateId);
        try {
            await ListingTemplateService.remove(templateId);
            Notice({ msg: "Template deleted.", isSuccess: true });

            const nextTotalCount = Math.max(0, totalCount - 1);
            const currentTemplateCount = templates.length;
            const shouldMoveBack =
                currentTemplateCount <= 1 &&
                pageNumber > 1 &&
                nextTotalCount <= (pageNumber - 1) * pageSize;

            setTemplates((prev) => prev.filter((item) => item.id !== templateId));
            setTotalCount(nextTotalCount);

            if (shouldMoveBack) {
                setPageNumber((prev) => Math.max(1, prev - 1));
            } else {
                setRefreshKey((prev) => prev + 1);
            }
        } catch (error) {
            // eslint-disable-next-line no-console
            console.error("Failed to delete template", error);
            Notice({ msg: "Unable to delete template.", isSuccess: false });
        } finally {
            setPendingDeleteId(null);
        }
    };

    const handleCloneTemplate = async (templateId) => {
        if (isAtTemplateLimit) {
            Notice({
                msg: `You've reached the ${TEMPLATE_LIMIT} template limit.`,
                desc: "Delete an existing template before cloning a new one.",
                isSuccess: false
            });
            return;
        }

        try {
            const template = await ListingTemplateService.getById(templateId);
            if (!template) {
                Notice({ msg: "Template not found.", isSuccess: false });
                return;
            }

            const cloneName = `${template.name ?? "Template"} Copy`;
            await ListingTemplateService.clone(templateId, { nameOverride: cloneName });

            Notice({ msg: "Template cloned.", isSuccess: true });
            setRefreshKey((prev) => prev + 1);
        } catch (error) {
            // eslint-disable-next-line no-console
            console.error("Failed to clone template", error);
            Notice({ msg: "Unable to clone template.", isSuccess: false });
        }
    };

    const handleCreateNewTemplate = () => {
        if (isAtTemplateLimit) {
            Notice({
                msg: `You've reached the ${TEMPLATE_LIMIT} template limit.`,
                desc: "Delete an existing template before creating a new one.",
                isSuccess: false
            });
            return;
        }

        navigate("/listing-form", {
            state: {
                listingDraft: {
                    listingStatus: "Draft"
                },
                isCreatingTemplate: true
            }
        });
    };

    const handleEditTemplate = async (templateId) => {
        try {
            const template = await ListingTemplateService.getById(templateId);
            if (!template) {
                Notice({ msg: "Template not found.", isSuccess: false });
                return;
            }

            const { listingDraft, variationData } = prepareTemplateState(template);

            if (!listingDraft) {
                Notice({ msg: "Template payload is invalid.", isSuccess: false });
                return;
            }

            navigate("/listing-form", {
                state: {
                    listingDraft,
                    variationData,
                    isEditingTemplate: true,
                    templateId,
                    templateName: template.name,
                    templateDescription: template.description,
                    templateFormatLabel: template.formatLabel,
                    templateThumbnailUrl: template.thumbnailUrl
                }
            });
        } catch (error) {
            // eslint-disable-next-line no-console
            console.error("Failed to edit template", error);
            Notice({ msg: "Unable to load template for editing.", isSuccess: false });
        } finally {
            closeMenu();
        }
    };

    return (
        <div className="listing-dashboard listing-template-dashboard">
            <header className="listing-dashboard__header">
                <div className="listing-dashboard__heading-row">
                    <div className="listing-dashboard__page-title">
                        <h1>Manage listing templates</h1>
                        <p>Save up to {TEMPLATE_LIMIT} templates. Editing a template won't change any listings already made with that template.</p>
                    </div>
                    <button
                        type="button"
                        className="listing-dashboard__cta"
                        onClick={handleCreateNewTemplate}
                        disabled={isAtTemplateLimit}
                        title={isAtTemplateLimit ? `You can only keep ${TEMPLATE_LIMIT} templates at a time.` : undefined}
                    >
                        Create template
                    </button>
                </div>
            </header>

            <section className="listing-dashboard__toolbar">
                {isAtTemplateLimit && (
                    <div className="listing-template-limit" role="status" aria-live="polite">
                        You have {TEMPLATE_LIMIT} templates. Delete a template to add more.
                    </div>
                )}
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
                            id="listing-template-search"
                            type="search"
                            placeholder="Search by template name or description"
                            autoComplete="off"
                            value={searchInput}
                            onChange={handleSearchInputChange}
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
                </div>
            </section>

            <div className="listing-dashboard__table-card listing-template-table__card">
                <div className="listing-dashboard__table-wrapper">
                    <table className="listing-dashboard__table listing-template-table">
                        <thead>
                            <tr>
                                <th className="listing-template-table__header listing-template-table__header--name">Template</th>
                                <th>Format</th>
                                <th>Last updated</th>
                                <th>Created</th>
                                <th className="listing-dashboard__actions-header">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            {isLoading ? (
                                <tr>
                                    <td colSpan={5} className="listing-dashboard__table-empty">
                                        Loading templates...
                                    </td>
                                </tr>
                            ) : templates.length === 0 ? (
                                <tr>
                                    <td colSpan={5} className="listing-dashboard__table-empty">
                                        <div className="listing-dashboard__empty-title">
                                            {searchTerm
                                                ? "No templates match your search."
                                                : "You don't have any templates yet."}
                                        </div>
                                        <p>
                                            Save time by creating templates for your most common listing configurations.
                                        </p>
                                        {!searchTerm && (
                                            <button
                                                type="button"
                                                className="listing-dashboard__row-action listing-template-table__action-primary"
                                                onClick={handleCreateNewTemplate}
                                            >
                                                Create template
                                            </button>
                                        )}
                                    </td>
                                </tr>
                            ) : (
                                templates.map((template) => (
                                    <tr key={template.id}>
                                        <td className="listing-template-table__name-cell">
                                            <div className="listing-template-table__item">
                                                <div
                                                    className={
                                                        template.thumbnailUrl
                                                            ? "listing-template-table__thumbnail"
                                                            : "listing-template-table__thumbnail listing-template-table__thumbnail--empty"
                                                    }
                                                >
                                                    {template.thumbnailUrl ? (
                                                        <img src={template.thumbnailUrl} alt={template.name ?? "Template thumbnail"} />
                                                    ) : (
                                                        <span>No image</span>
                                                    )}
                                                </div>
                                                <div className="listing-template-table__meta">
                                                    <span className="listing-template-table__name" title={template.name}>
                                                        {template.name ?? "Untitled template"}
                                                    </span>
                                                    {template.description && (
                                                        <span
                                                            className="listing-template-table__description"
                                                            title={template.description}
                                                        >
                                                            {template.description}
                                                        </span>
                                                    )}
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <span className="listing-template-table__format">
                                                {template.formatLabel ?? "Custom format"}
                                            </span>
                                        </td>
                                        <td>{formatDate(template.updatedAt)}</td>
                                        <td>{formatDate(template.createdAt)}</td>
                                        <td className="listing-dashboard__actions-cell">
                                            <div
                                                className="listing-template-table__actions"
                                                ref={(element) => registerActionContainer(template.id, element)}
                                            >
                                                <button
                                                    type="button"
                                                    className="listing-template-table__menu-trigger"
                                                    onClick={() =>
                                                        setOpenMenuId((prev) => (prev === template.id ? null : template.id))
                                                    }
                                                    aria-haspopup="true"
                                                    aria-expanded={openMenuId === template.id}
                                                >
                                                    Actions
                                                    <span aria-hidden="true" className="listing-template-table__menu-icon">▾</span>
                                                </button>
                                                {openMenuId === template.id && (
                                                    <div
                                                        className="listing-template-table__menu"
                                                        role="menu"
                                                    >
                                                        <button
                                                            type="button"
                                                            className="listing-template-table__menu-item"
                                                            onClick={() => {
                                                                closeMenu();
                                                                handleCreateListingFromTemplate(template.id);
                                                            }}
                                                            role="menuitem"
                                                        >
                                                            Use template
                                                        </button>
                                                        <button
                                                            type="button"
                                                            className="listing-template-table__menu-item"
                                                            onClick={() => {
                                                                closeMenu();
                                                                handleEditTemplate(template.id);
                                                            }}
                                                            role="menuitem"
                                                        >
                                                            Edit
                                                        </button>
                                                        <button
                                                            type="button"
                                                            className="listing-template-table__menu-item"
                                                            onClick={() => {
                                                                closeMenu();
                                                                handleCloneTemplate(template.id);
                                                            }}
                                                            disabled={isAtTemplateLimit}
                                                            title={isAtTemplateLimit ? `Delete a template to clone another.` : undefined}
                                                            role="menuitem"
                                                        >
                                                            Clone
                                                        </button>
                                                        <button
                                                            type="button"
                                                            className="listing-template-table__menu-item listing-template-table__menu-item--danger"
                                                            onClick={() => {
                                                                closeMenu();
                                                                handleDeleteTemplate(template.id);
                                                            }}
                                                            disabled={pendingDeleteId === template.id}
                                                            role="menuitem"
                                                        >
                                                            {pendingDeleteId === template.id ? "Deleting..." : "Delete"}
                                                        </button>
                                                    </div>
                                                )}
                                            </div>
                                        </td>
                                    </tr>
                                ))
                            )}
                        </tbody>
                    </table>
                </div>
            </div>

            <div className="listing-dashboard__controls-row">
                <div className="listing-dashboard__controls-left">
                    <span className="listing-dashboard__results">{resultsLabel}</span>
                    <span className="listing-dashboard__page-indicator">Page {currentPage} of {totalPages}</span>
                </div>
                <div className="listing-dashboard__pagination-controls">
                    <button type="button" onClick={handlePrevPage} disabled={currentPage <= 1}>
                        Previous
                    </button>
                    <button type="button" onClick={handleNextPage} disabled={currentPage >= totalPages}>
                        Next
                    </button>
                </div>
            </div>
        </div>
    );
};

export default ListingTemplatesPage;
