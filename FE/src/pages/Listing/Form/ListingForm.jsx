import { useState, useRef, useEffect, useMemo, useCallback } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";

import "./ListingForm.css";

import ebayLogo from "../../../assets/images/ebay_logo.png";

import { EbayButton } from "@ebay/ui-core-react/ebay-button";
import { EbayFilePreviewCard } from "@ebay/ui-core-react/ebay-file-preview-card";
import { ReactComponent as AddImageIcon } from "@ebay/skin/dist/svg/icon/icon-add-image-24.svg";
import { ReactComponent as CalendarIcon } from "@ebay/skin/dist/svg/icon/icon-calendar-24.svg";
import { ReactComponent as RemoveIcon } from "@ebay/skin/dist/svg/icon/icon-delete-16.svg";
import { ReactComponent as EditIcon } from "@ebay/skin/dist/svg/icon/icon-pencil-16.svg";
import { ReactComponent as InfoFilledIcon } from "@ebay/skin/dist/svg/icon/icon-information-filled-16.svg";
import { ReactComponent as MenuIcon } from "@ebay/skin/dist/svg/icon/icon-menu-16.svg";

import { EbayTextbox, EbayTextboxPrefixText } from '@ebay/ui-core-react/ebay-textbox';
import { EbayLabel, EbayField, EbayFieldDescription } from '@ebay/ui-core-react/ebay-field';
import { EbaySwitch } from '@ebay/ui-core-react/ebay-switch';
import { EbaySelect, EbaySelectOption } from '@ebay/ui-core-react/ebay-select';
import { EbayFakeLink } from '@ebay/ui-core-react/ebay-fake-link';
import { EbaySectionNoticeCTA, EbayNoticeContent, EbaySectionNotice } from '@ebay/ui-core-react/ebay-section-notice';

import CategoryModal from "../Modal/CategoryModal"
import ConditionModal from "../Modal/ConditionModal"
import ListingService from "../../../services/ListingService";
import CategoryService from "../../../services/CategoryService";
import FileService from "../../../services/FileService";
import ListingTemplateService from "../../../services/ListingTemplateService";
import Notice from "../../../components/Common/CustomNotification";
import { LoadingScreen } from "../../../components/LoadingScreen/LoadingScreen";
import { getStoredUser } from "../../../utils/auth";

const parseCurrencyInput = (value) => {
  if (typeof value === "number" && Number.isFinite(value)) {
    return Math.round(value * 100) / 100;
  }

  if (typeof value !== "string") {
    return null;
  }

  const normalized = value.replace(/[^0-9.-]/g, "").trim();
  if (!normalized) {
    return null;
  }

  const parsed = Number.parseFloat(normalized);
  if (!Number.isFinite(parsed)) {
    return null;
  }

  return Math.round(parsed * 100) / 100;
};

const parseIntegerInput = (value) => {
  if (typeof value === "number" && Number.isInteger(value)) {
    return value;
  }

  const normalized = String(value ?? "").replace(/[^0-9-]/g, "").trim();
  if (!normalized) {
    return null;
  }

  const parsed = Number.parseInt(normalized, 10);
  return Number.isFinite(parsed) ? parsed : null;
};

const MAX_MEDIA_ITEMS = 25;

const DEFAULT_LISTING_STATUS = "Draft";
const LISTING_STATUS_LABELS = {
  Draft: "Draft",
  Scheduled: "Scheduled",
  Active: "Active",
  Ended: "Ended",
  1: "Draft",
  2: "Scheduled",
  3: "Active",
  4: "Ended"
};

const createMediaId = () => `media-${Date.now()}-${Math.random().toString(36).slice(2, 10)}`;

const createCustomSpecificId = () => `custom-specific-${Date.now()}-${Math.random().toString(36).slice(2, 8)}`;

const normalizeMediaItem = (item) => {
  if (!item) {
    return null;
  }

  const inferredType = (() => {
    if (item.type) {
      return item.type;
    }

    const mime = item.mimeType || "";
    if (typeof mime === "string" && mime.startsWith("video")) {
      return "video";
    }

    return "image";
  })();

  return {
    id: item.id ?? createMediaId(),
    name: item.name ?? "Media file",
    type: inferredType,
    src: item.src ?? "",
    preview: item.preview && item.preview.startsWith("blob:") ? item.preview : null,
    size: item.size,
    uploading: Boolean(item.uploading && !item.src)
  };
};

const buildVariationKeyFromAttributes = (attributeValues) => {
  const entries = Object.entries(attributeValues ?? {}).filter(([, value]) => {
    const trimmed = String(value ?? "").trim();
    return trimmed.length > 0;
  });

  if (!entries.length) {
    return `variation-${Date.now()}-${Math.random().toString(36).slice(2, 10)}`;
  }

  return entries
    .sort(([left], [right]) => left.localeCompare(right))
    .map(([attribute, option]) => `${attribute}:${option}`)
    .join("|");
};

const createStaticMediaEntry = (url, label, index = 0) => ({
  id: createMediaId(),
  name: label ?? (index === 0 ? "Primary photo" : "Photo"),
  type: "image",
  size: null,
  src: url ?? "",
  preview: null,
  uploading: false
});

const ListingForm = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const { listingId: listingIdParam } = useParams();
  const isCreatingNewListing = !listingIdParam;
  const [canManageListing, setCanManageListing] = useState(() => {
    if (!isCreatingNewListing) {
      return true;
    }
    const stored = getStoredUser();
    return Boolean(
      stored?.isEmailVerified
      && stored?.isPaymentVerified
    );
  });

  useEffect(() => {
    if (!isCreatingNewListing) {
      return;
    }

    const handleUserInfoUpdated = () => {
      const updated = getStoredUser();
      setCanManageListing(Boolean(
        updated?.isEmailVerified
        && updated?.isPaymentVerified
      ));
    };

    if (typeof window !== "undefined") {
      window.addEventListener("user-info-updated", handleUserInfoUpdated);
    }

    return () => {
      if (typeof window !== "undefined") {
        window.removeEventListener("user-info-updated", handleUserInfoUpdated);
      }
    };
  }, [isCreatingNewListing]);

  useEffect(() => {
    if (!isCreatingNewListing || canManageListing) {
      return;
    }

    const requirements = [];
    const snapshot = getStoredUser();
    if (!snapshot?.isEmailVerified) {
      requirements.push("xác minh email");
    }
    if (!snapshot?.isPaymentVerified) {
      requirements.push("xác minh thanh toán");
    }
    const desc = requirements.length > 0
      ? `Vui lòng ${requirements.join(" và ")} trước khi tạo tin đăng.`
      : "Truy cập Account settings để hoàn tất xác minh.";

    Notice({
      msg: "Chưa đủ điều kiện để tạo tin đăng.",
      desc,
      isSuccess: false
    });

    navigate("/account/settings", { replace: true });
  }, [canManageListing, isCreatingNewListing, navigate]);

  const [files, setFiles] = useState([]);
  const filesRef = useRef([]);
  const fileInputRef = useRef(null);
  const variationMediaRef = useRef(null);
  const skipNextListingLoadRef = useRef(false);

  const [variationData, setVariationData] = useState(null);
  const [listingStatus, setListingStatus] = useState(DEFAULT_LISTING_STATUS);

  const [activeListingId, setActiveListingId] = useState(null);
  const [isLoadingListing, setIsLoadingListing] = useState(false);
  const isEditMode = Boolean(activeListingId);

  const statusPermissions = useMemo(() => {
    if (!isEditMode) {
      return {
        showSchedule: true,
        allowSaveDraft: true,
        allowCategoryEdit: true,
        allowFormatSelection: true
      };
    }

    switch (listingStatus) {
      case "Active":
      case "Ended":
        return {
          showSchedule: false,
          allowSaveDraft: false,
          allowCategoryEdit: false,
          allowFormatSelection: false
        };
      default:
        return {
          showSchedule: true,
          allowSaveDraft: true,
          allowCategoryEdit: true,
          allowFormatSelection: true
        };
    }
  }, [isEditMode, listingStatus]);

  const listingStatusLabel = LISTING_STATUS_LABELS[listingStatus] ??
    (typeof listingStatus === "string" ? listingStatus : DEFAULT_LISTING_STATUS);

  const [selectedCategoryId, setSelectedCategoryId] = useState(null);
  const [categoryPath, setCategoryPath] = useState([]);
  const [categoryDetail, setCategoryDetail] = useState(null);

  const [selectedConditionId, setSelectedConditionId] = useState(null);
  const [conditions, setConditions] = useState([]);

  const [specificSelections, setSpecificSelections] = useState({});
  const [customSpecificInputs, setCustomSpecificInputs] = useState({});
  const [customSpecificRows, setCustomSpecificRows] = useState([]);
  const [openSpecificId, setOpenSpecificId] = useState(null);

  const [quantity, setQuantity] = useState("1");
  const [title, setTitle] = useState("");
  const [sku, setSku] = useState("");
  const [listingDescription, setListingDescription] = useState("");
  const [conditionDescription, setConditionDescription] = useState("");
  const [startingBid, setStartingBid] = useState("");
  const [buyItNowPrice, setBuyItNowPrice] = useState("");
  const [reservePrice, setReservePrice] = useState("");
  const [fixedPrice, setFixedPrice] = useState("");
  const [minimumOffer, setMinimumOffer] = useState("");
  const [autoAcceptOffer, setAutoAcceptOffer] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [templateContext, setTemplateContext] = useState({ mode: "listing", templateId: null });
  const [templateName, setTemplateName] = useState("");
  const [templateDescription, setTemplateDescription] = useState("");
  const [templateFormatLabel, setTemplateFormatLabel] = useState("");
  const [templateThumbnailUrl, setTemplateThumbnailUrl] = useState("");
  const [isTemplateSaving, setIsTemplateSaving] = useState(false);
  const [fromTemplateId, setFromTemplateId] = useState(null);
  const isTemplateMode = templateContext.mode === "create" || templateContext.mode === "edit";

  const priceFormats = [
    { value: "1", text: "Auction" },
    { value: "2", text: "Buy It Now" },
  ];

  const [priceFormat, setPriceFormat] = useState("1");

  const auctionDurations = [
    { value: "3", text: "3 days" },
    { value: "5", text: "5 days" },
    { value: "7", text: "7 days" },
    { value: "10", text: "10 days" }
  ];
  const [auctionDuration, setAuctionDuration] = useState("7");

  const releaseVariationMedia = useCallback((data) => {
    if (!data) {
      return;
    }

    if (Array.isArray(data.defaultPhotos)) {
      data.defaultPhotos.forEach((item) => {
        const previewUrl = item?.preview ?? item?.src;
        if (previewUrl && previewUrl.startsWith("blob:")) {
          URL.revokeObjectURL(previewUrl);
        }
      });
    }

    if (Array.isArray(data.variationRows)) {
      data.variationRows.forEach((row) => {
        const previewUrl = row?.photo?.preview ?? row?.photo?.src;
        if (previewUrl && previewUrl.startsWith("blob:")) {
          URL.revokeObjectURL(previewUrl);
        }
      });
    }
  }, []);

  useEffect(() => {
    filesRef.current = files;
  }, [files]);

  useEffect(() => () => {
    filesRef.current.forEach((item) => {
      if (item?.preview && item.preview.startsWith("blob:")) {
        URL.revokeObjectURL(item.preview);
      }
    });
  }, []);

  useEffect(() => {
    const state = location.state;

    if (state?.isCreatingTemplate) {
      setTemplateContext({ mode: "create", templateId: null });
      setTemplateName(state.templateName ?? "");
      setTemplateDescription(state.templateDescription ?? "");
      setTemplateFormatLabel(state.templateFormatLabel ?? "");
      setTemplateThumbnailUrl(state.templateThumbnailUrl ?? "");
    } else if (state?.isEditingTemplate && state?.templateId) {
      setTemplateContext({ mode: "edit", templateId: state.templateId });
      setTemplateName(state.templateName ?? "");
      setTemplateDescription(state.templateDescription ?? "");
      setTemplateFormatLabel(state.templateFormatLabel ?? "");
      setTemplateThumbnailUrl(state.templateThumbnailUrl ?? "");
    } else {
      setTemplateContext((prev) => (prev.mode === "listing" ? prev : { mode: "listing", templateId: null }));
      setTemplateName("");
      setTemplateDescription("");
      setTemplateFormatLabel("");
      setTemplateThumbnailUrl("");
    }

    if (state?.fromTemplateId) {
      setFromTemplateId(state.fromTemplateId);
    } else {
      setFromTemplateId(null);
    }
  }, [location.state]);

  useEffect(() => {
    if (!conditions.some((condition) => condition.id === selectedConditionId)) {
      setSelectedConditionId(null);
    }
  }, [conditions, selectedConditionId]);

  useEffect(() => {
    const incomingVariation = location.state?.variationData;
    const incomingDraft = location.state?.listingDraft;

    const draftListingId = incomingDraft?.listingId || location.state?.listingId;
    if (draftListingId) {
      setActiveListingId(draftListingId);
    }

    if (incomingDraft) {
      skipNextListingLoadRef.current = true;
      if (Array.isArray(incomingDraft.files)) {
        const normalized = incomingDraft.files
          .map((file) => normalizeMediaItem(file))
          .filter(Boolean);
        setFiles(normalized);
      }

      if (incomingDraft.title !== undefined) {
        setTitle(incomingDraft.title ?? "");
      }

      if (incomingDraft.sku !== undefined) {
        setSku(incomingDraft.sku ?? "");
      }

      if (incomingDraft.listingDescription !== undefined) {
        setListingDescription(incomingDraft.listingDescription ?? "");
      }

      if (incomingDraft.conditionDescription !== undefined) {
        setConditionDescription(incomingDraft.conditionDescription ?? "");
      }

      if (incomingDraft.startingBid !== undefined) {
        setStartingBid(incomingDraft.startingBid ?? "");
      }

      if (incomingDraft.buyItNowPrice !== undefined) {
        setBuyItNowPrice(incomingDraft.buyItNowPrice ?? "");
      }

      if (incomingDraft.reservePrice !== undefined) {
        setReservePrice(incomingDraft.reservePrice ?? "");
      }

      if (incomingDraft.fixedPrice !== undefined) {
        setFixedPrice(incomingDraft.fixedPrice ?? "");
      }

      if (incomingDraft.minimumOffer !== undefined) {
        setMinimumOffer(incomingDraft.minimumOffer ?? "");
      }

      if (incomingDraft.autoAcceptOffer !== undefined) {
        setAutoAcceptOffer(incomingDraft.autoAcceptOffer ?? "");
      }

      if (incomingDraft.selectedCategoryId !== undefined) {
        setSelectedCategoryId(incomingDraft.selectedCategoryId);
      }

      if (Array.isArray(incomingDraft.categoryPath)) {
        setCategoryPath(incomingDraft.categoryPath);
      }

      if (incomingDraft.categoryDetail !== undefined) {
        setCategoryDetail(incomingDraft.categoryDetail ?? null);
      }

      if (Array.isArray(incomingDraft.conditions)) {
        setConditions(incomingDraft.conditions);
      }

      if (incomingDraft.selectedConditionId !== undefined) {
        setSelectedConditionId(incomingDraft.selectedConditionId ?? null);
      }

      if (incomingDraft.specificSelections) {
        setSpecificSelections(incomingDraft.specificSelections);
      }

      if (incomingDraft.customSpecificInputs) {
        setCustomSpecificInputs(incomingDraft.customSpecificInputs);
      }

      if (Array.isArray(incomingDraft.customSpecificRows)) {
        setCustomSpecificRows(
          incomingDraft.customSpecificRows.map((row) => ({
            id: row?.id ?? createCustomSpecificId(),
            name: row?.name ?? "",
            values: Array.isArray(row?.values) && row.values.length ? row.values : [""]
          }))
        );
      }

      if (incomingDraft.openSpecificId !== undefined) {
        setOpenSpecificId(incomingDraft.openSpecificId);
      }

      if (incomingDraft.quantity !== undefined) {
        setQuantity(incomingDraft.quantity);
      }

      if (incomingDraft.priceFormat !== undefined) {
        setPriceFormat(incomingDraft.priceFormat);
      }

      if (incomingDraft.auctionDuration !== undefined) {
        setAuctionDuration(incomingDraft.auctionDuration);
      }

      if (incomingDraft.scheduleEnabled !== undefined) {
        setScheduleEnabled(Boolean(incomingDraft.scheduleEnabled));
      }

      if (incomingDraft.date !== undefined) {
        setDate(incomingDraft.date);
      }

      if (incomingDraft.hour !== undefined) {
        setHour(incomingDraft.hour);
      }

      if (incomingDraft.minute !== undefined) {
        setMinute(incomingDraft.minute);
      }

      if (incomingDraft.ampm !== undefined) {
        setAmpm(incomingDraft.ampm);
      }

      if (incomingDraft.allowOfferEnabled !== undefined) {
        setAllowOfferEnabled(Boolean(incomingDraft.allowOfferEnabled));
      }

      if (incomingDraft.listingStatus !== undefined) {
        const draftStatus = incomingDraft.listingStatus;
        setListingStatus(
          LISTING_STATUS_LABELS[draftStatus] ??
          (typeof draftStatus === "string" ? draftStatus : DEFAULT_LISTING_STATUS)
        );
      }
    }

    if (incomingVariation) {
      releaseVariationMedia(variationMediaRef.current);

      let cloned;
      try {
        cloned = JSON.parse(JSON.stringify(incomingVariation));
      } catch (error) {
        cloned = null;
      }

      if (cloned) {
        variationMediaRef.current = cloned;
        setVariationData(cloned);
      } else {
        variationMediaRef.current = null;
        setVariationData(null);
      }
    }

    if (!incomingVariation && !incomingDraft) {
      return;
    }

    const {
      variationData: _ignoredVariation,
      listingDraft: _ignoredDraft,
      ...rest
    } = location.state ?? {};
    const nextState = Object.keys(rest).length ? rest : undefined;
    navigate(location.pathname, { replace: true, state: nextState });
  }, [location.state, location.pathname, navigate, releaseVariationMedia]);

  useEffect(() => {
    if (!fromTemplateId) {
      return;
    }

    Notice({
      msg: "Template applied successfully.",
      isSuccess: true
    });
  }, [fromTemplateId]);

  useEffect(() => () => {
    releaseVariationMedia(variationMediaRef.current);
  }, [releaseVariationMedia]);

  useEffect(() => {
    if (priceFormat === "2") {
      return;
    }

    if (variationData) {
      releaseVariationMedia(variationMediaRef.current);
      variationMediaRef.current = null;
      setVariationData(null);
    }
  }, [priceFormat, releaseVariationMedia, variationData]);

  useEffect(() => {
    if (!variationData?.categoryId) {
      return;
    }

    if (selectedCategoryId && variationData.categoryId !== selectedCategoryId) {
      releaseVariationMedia(variationMediaRef.current);
      variationMediaRef.current = null;
      setVariationData(null);
    }
  }, [releaseVariationMedia, selectedCategoryId, variationData?.categoryId]);

  const specifics = Array.isArray(categoryDetail?.specifics)
    ? categoryDetail.specifics
    : [];
  const requiredSpecifics = specifics.filter((specific) => specific.isRequired);
  const additionalSpecifics = specifics.filter((specific) => !specific.isRequired);

  const hasVariations = Boolean(
    Array.isArray(variationData?.variationRows) && variationData.variationRows.length > 0
  );

  const variationSummary = useMemo(() => {
    if (!variationData || !Array.isArray(variationData.variationRows) || variationData.variationRows.length === 0) {
      return null;
    }

    const attributes = Object.entries(variationData.selectedOptions ?? {}).reduce((acc, [name, options]) => {
      if (!Array.isArray(options) || options.length === 0) {
        return acc;
      }

      acc.push({ name, values: options });
      return acc;
    }, []);

    const numericPrices = variationData.variationRows
      .map((row) => {
        const raw = typeof row.price === "number" ? row.price : parseFloat(String(row.price ?? "").replace(/[^0-9.-]/g, ""));
        return Number.isFinite(raw) ? raw : null;
      })
      .filter((value) => value !== null);

    const minPrice = numericPrices.length ? Math.min(...numericPrices) : null;
    const maxPrice = numericPrices.length ? Math.max(...numericPrices) : null;

    let hasQuantityValue = false;
    const totalQuantity = variationData.variationRows.reduce((total, row) => {
      const raw = parseInt(String(row.quantity ?? "").replace(/[^0-9-]/g, ""), 10);
      if (Number.isFinite(raw)) {
        hasQuantityValue = true;
        return total + raw;
      }
      return total;
    }, 0);

    const priceFormatter = new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: "USD",
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });

    const priceRangeLabel = (() => {
      if (minPrice === null) {
        return null;
      }

      if (maxPrice === null || Math.abs(maxPrice - minPrice) < 0.005) {
        return priceFormatter.format(minPrice);
      }

      return `${priceFormatter.format(minPrice)}-${priceFormatter.format(maxPrice)}`;
    })();

    const attributeRows = [];
    for (let index = 0; index < attributes.length; index += 2) {
      attributeRows.push(attributes.slice(index, index + 2));
    }

    return {
      attributes,
      attributeRows,
      priceRangeLabel,
      hasQuantity: hasQuantityValue,
      quantityLabel: hasQuantityValue ? totalQuantity.toLocaleString("en-US") : null
    };
  }, [variationData]);

  useEffect(() => {
    if (!hasVariations || priceFormat === "2") {
      return;
    }

    setPriceFormat("2");
  }, [hasVariations, priceFormat, setPriceFormat]);

  const handleClearVariations = () => {
    releaseVariationMedia(variationMediaRef.current);
    variationMediaRef.current = null;
    setVariationData(null);
  };

  const handleSpecificSingleChange = (specificId, value) => {
    setSpecificSelections((prev) => ({
      ...prev,
      [specificId]: value,
    }));
  };

  const handleSpecificMultiToggle = (specificId, option) => {
    setSpecificSelections((prev) => {
      const current = Array.isArray(prev[specificId]) ? prev[specificId] : [];
      const next = current.includes(option)
        ? current.filter((item) => item !== option)
        : [...current, option];

      return {
        ...prev,
        [specificId]: next,
      };
    });
  };

  const handleSpecificCustomInputChange = (specificId, value) => {
    setCustomSpecificInputs((prev) => ({
      ...prev,
      [specificId]: value,
    }));
  };

  const handleSpecificCustomAdd = (specificId, allowMultiple) => {
    const rawValue = (customSpecificInputs[specificId] ?? "").trim();
    if (!rawValue) {
      return;
    }

    setSpecificSelections((prev) => {
      if (allowMultiple) {
        const current = Array.isArray(prev[specificId]) ? prev[specificId] : [];
        if (current.includes(rawValue)) {
          return prev;
        }
        return {
          ...prev,
          [specificId]: [...current, rawValue],
        };
      }

      return {
        ...prev,
        [specificId]: rawValue,
      };
    });

    setCustomSpecificInputs((prev) => ({
      ...prev,
      [specificId]: "",
    }));

    if (!allowMultiple) {
      setOpenSpecificId(null);
    }
  };

  const clearSpecificSelection = (specificId) => {
    setSpecificSelections((prev) => {
      const next = { ...prev };
      delete next[specificId];
      return next;
    });
  };

  const handleAddCustomSpecificRow = () => {
    setCustomSpecificRows((prev) => [
      ...prev,
      {
        id: createCustomSpecificId(),
        name: "",
        values: [""],
      }
    ]);
  };

  const handleRemoveCustomSpecificRow = (rowId) => {
    setCustomSpecificRows((prev) => prev.filter((row) => row.id !== rowId));
  };

  const handleCustomSpecificNameChange = (rowId, name) => {
    setCustomSpecificRows((prev) =>
      prev.map((row) => (row.id === rowId ? { ...row, name } : row))
    );
  };

  const handleCustomSpecificValueChange = (rowId, index, value) => {
    setCustomSpecificRows((prev) =>
      prev.map((row) => {
        if (row.id !== rowId) {
          return row;
        }

        const nextValues = Array.isArray(row.values) && row.values.length
          ? [...row.values]
          : [""];
        nextValues[index] = value;

        return {
          ...row,
          values: nextValues
        };
      })
    );
  };

  const handleCustomSpecificAddValue = (rowId) => {
    setCustomSpecificRows((prev) =>
      prev.map((row) => {
        if (row.id !== rowId) {
          return row;
        }

        const nextValues = Array.isArray(row.values) ? [...row.values] : [];
        nextValues.push("");
        return {
          ...row,
          values: nextValues
        };
      })
    );
  };

  const handleCustomSpecificRemoveValue = (rowId, index) => {
    setCustomSpecificRows((prev) =>
      prev.map((row) => {
        if (row.id !== rowId) {
          return row;
        }

        const nextValues = Array.isArray(row.values) ? row.values.filter((_, idx) => idx !== index) : [];
        if (nextValues.length === 0) {
          nextValues.push("");
        }

        return {
          ...row,
          values: nextValues
        };
      })
    );
  };

  const handleSpecificSingleSelect = (specificId, option) => {
    handleSpecificSingleChange(specificId, option);
    setCustomSpecificInputs((prev) => ({
      ...prev,
      [specificId]: "",
    }));
    setOpenSpecificId(null);
  };

  const toggleSpecificDropdown = (specificId) => {
    setOpenSpecificId((prev) => {
      const isClosing = prev === specificId;
      if (isClosing) {
        setCustomSpecificInputs((inputs) => ({
          ...inputs,
          [specificId]: "",
        }));
        return null;
      }

      setCustomSpecificInputs((inputs) => ({
        ...inputs,
        [specificId]: "",
      }));
      return specificId;
    });
  };

  useEffect(() => {
    if (!openSpecificId) {
      return undefined;
    }

    const handleClickOutside = (event) => {
      const container = document.querySelector(
        `[data-specific-container="${openSpecificId}"]`
      );

      if (container && container.contains(event.target)) {
        return;
      }

      setOpenSpecificId(null);
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, [openSpecificId]);

  const renderSpecificInput = (specific) => {
    const options = Array.isArray(specific.values) ? specific.values : [];
    const allowMultiple = Boolean(specific.allowMultiple);
    const selection = allowMultiple
      ? Array.isArray(specificSelections[specific.id])
        ? specificSelections[specific.id]
        : []
      : specificSelections[specific.id] ?? "";
    const customInputValue = customSpecificInputs[specific.id] ?? "";
    const filteredOptions = options.filter((option) =>
      option.toLowerCase().includes(customInputValue.toLowerCase())
    );
    const displayText = allowMultiple
      ? selection.length > 0
        ? selection.join(", ")
        : "Select one or more options"
      : selection || "Select an option";
    const isOpen = openSpecificId === specific.id;

    return (
      <div
        key={specific.id}
        data-specific-container={specific.id}
        style={{
          display: "flex",
          alignItems: "flex-start",
          gap: "32px",
          marginBottom: "28px",
        }}
      >
        <div style={{ minWidth: "220px" }}>
          <button
            type="button"
            onClick={() => toggleSpecificDropdown(specific.id)}
            style={{
              background: "none",
              border: "none",
              padding: 0,
              fontSize: "15px",
              textDecoration: "underline dotted",
              cursor: "pointer",
              color: "#111",
            }}
          >
            {specific.name}
            {specific.isRequired && <span style={{ color: "#c40000", marginLeft: "4px" }}>*</span>}
          </button>
          {allowMultiple && (
            <div style={{ color: "#777", fontSize: "12px", marginTop: "4px" }}>
              Select all that apply
            </div>
          )}
        </div>

        <div style={{ position: "relative", flex: 1 }}>
          <button
            type="button"
            onClick={() => toggleSpecificDropdown(specific.id)}
            style={{
              width: "100%",
              maxWidth: "420px",
              minHeight: "48px",
              borderRadius: "12px",
              border: isOpen ? "2px solid #3665f3" : "1px solid #c7c7c7",
              padding: "10px 16px",
              display: "flex",
              justifyContent: "space-between",
              alignItems: "center",
              backgroundColor: "#fff",
              cursor: "pointer",
              fontSize: "15px",
              color: selection ? "#111" : "#666",
              textAlign: "left",
            }}
          >
            <span
              style={{
                flex: 1,
                paddingRight: "16px",
                whiteSpace: "nowrap",
                overflow: "hidden",
                textOverflow: "ellipsis",
              }}
            >
              {displayText}
            </span>
            <span style={{ fontSize: "18px", color: "#111" }}>{isOpen ? "▴" : "▾"}</span>
          </button>

          {(allowMultiple ? selection.length > 0 : Boolean(selection)) && (
            <div
              style={{
                marginTop: "6px",
                fontSize: "13px",
                color: "#555",
                maxWidth: "420px",
                whiteSpace: "normal",
              }}
            >
              {allowMultiple ? selection.join(", ") : selection}
            </div>
          )}

          {isOpen && (
            <div
              style={{
                position: "absolute",
                zIndex: 20,
                top: "calc(100% + 8px)",
                width: "100%",
                maxWidth: "420px",
                maxHeight: "360px",
                backgroundColor: "#fff",
                borderRadius: "12px",
                boxShadow: "0 12px 32px rgba(0,0,0,0.18)",
                border: "1px solid #ddd",
                overflow: "hidden",
                display: "flex",
                flexDirection: "column",
              }}
            >
              <div style={{ padding: "12px 14px", borderBottom: "1px solid #eee" }}>
                <input
                  type="text"
                  placeholder="Search or enter your own"
                  value={customInputValue}
                  onChange={(event) => handleSpecificCustomInputChange(specific.id, event.target.value)}
                  onKeyDown={(event) => {
                    if (event.key === "Enter") {
                      event.preventDefault();
                      handleSpecificCustomAdd(specific.id, allowMultiple);
                    } else if (event.key === "Escape") {
                      event.preventDefault();
                      setOpenSpecificId(null);
                    }
                  }}
                  style={{
                    width: "100%",
                    height: "40px",
                    borderRadius: "10px",
                    border: "1px solid #c7c7c7",
                    padding: "0 12px",
                    fontSize: "14px",
                  }}
                />
              </div>

              {options.length > 0 && (
                <div style={{ padding: "12px 0", overflowY: "auto" }}>
                  {allowMultiple ? (
                    <>
                      <div style={{ fontWeight: 600, fontSize: "13px", color: "#111", padding: "0 16px", marginBottom: "6px" }}>
                        Frequently selected
                      </div>
                      {filteredOptions.map((option) => {
                        const isChecked = selection.includes(option);
                        return (
                          <label
                            key={option}
                            style={{
                              display: "flex",
                              alignItems: "center",
                              gap: "12px",
                              padding: "10px 16px",
                              cursor: "pointer",
                              fontSize: "14px",
                              color: "#111",
                            }}
                          >
                            <input
                              type="checkbox"
                              checked={isChecked}
                              onChange={() => handleSpecificMultiToggle(specific.id, option)}
                              style={{ width: "16px", height: "16px" }}
                            />
                            <span>{option}</span>
                          </label>
                        );
                      })}
                      {filteredOptions.length === 0 && (
                        <div style={{ padding: "8px 16px", color: "#777", fontSize: "13px" }}>
                          No matches. Press Enter to add your own value.
                        </div>
                      )}
                    </>
                  ) : (
                    <div>
                      {selection && (
                        <div
                          style={{
                            padding: "0 16px 6px",
                            display: "flex",
                            justifyContent: "space-between",
                            color: "#111",
                            fontSize: "13px",
                          }}
                        >
                          <span style={{ fontWeight: 600 }}>Selected</span>
                          <button
                            type="button"
                            onClick={() => clearSpecificSelection(specific.id)}
                            style={{
                              background: "none",
                              border: "none",
                              color: "#3665f3",
                              cursor: "pointer",
                              fontSize: "13px",
                            }}
                          >
                            Clear
                          </button>
                        </div>
                      )}
                      {filteredOptions.map((option) => (
                        <button
                          key={option}
                          type="button"
                          onClick={() => handleSpecificSingleSelect(specific.id, option)}
                          style={{
                            display: "flex",
                            justifyContent: "space-between",
                            alignItems: "center",
                            width: "100%",
                            padding: "12px 16px",
                            border: "none",
                            background: option === selection ? "#f5f7ff" : "#fff",
                            cursor: "pointer",
                            fontSize: "14px",
                            color: "#111",
                          }}
                        >
                          <span>{option}</span>
                          {option === selection && <span style={{ color: "#3665f3", fontWeight: 600 }}>✓</span>}
                        </button>
                      ))}
                      {filteredOptions.length === 0 && (
                        <div style={{ padding: "8px 16px", color: "#777", fontSize: "13px" }}>
                          No matches. Press Enter to add your own value.
                        </div>
                      )}
                    </div>
                  )}
                </div>
              )}

              <div
                style={{
                  padding: "10px 16px",
                  borderTop: "1px solid #eee",
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                  gap: "12px",
                }}
              >
                <div style={{ fontSize: "12px", color: "#777" }}>
                  {allowMultiple
                    ? selection.length > 0
                      ? `Selected: ${selection.join(", ")}`
                      : "No values selected"
                    : selection
                      ? `Selected: ${selection}`
                      : "No value selected"}
                </div>
                <button
                  type="button"
                  onClick={() => handleSpecificCustomAdd(specific.id, allowMultiple)}
                  style={{
                    backgroundColor: "#3665f3",
                    color: "#fff",
                    border: "none",
                    borderRadius: "20px",
                    padding: "8px 16px",
                    fontSize: "13px",
                    fontWeight: 600,
                    cursor: customInputValue.trim().length === 0 ? "not-allowed" : "pointer",
                    opacity: customInputValue.trim().length === 0 ? 0.5 : 1,
                  }}
                  disabled={customInputValue.trim().length === 0}
                >
                  {customInputValue.trim().length > 0 ? `Add "${customInputValue}"` : "Add custom value"}
                </button>
              </div>
            </div>
          )}
        </div>
      </div>
    );
  };

  const renderCustomSpecificRow = (row) => {
    const values = Array.isArray(row.values) && row.values.length ? row.values : [""];

    return (
      <div
        key={row.id}
        style={{
          border: "1px solid #e3e7eb",
          borderRadius: "12px",
          padding: "16px",
          marginBottom: "12px",
          backgroundColor: "#fff",
          display: "flex",
          flexDirection: "column",
          gap: "12px"
        }}
      >
        <div style={{ display: "flex", gap: "12px", flexWrap: "wrap" }}>
          <div style={{ flex: "1 1 220px", minWidth: "220px" }}>
            <label style={{ display: "flex", flexDirection: "column", gap: "4px", fontSize: "13px", color: "#555" }}>
              <span>Specific name</span>
              <input
                type="text"
                value={row.name ?? ""}
                onChange={(event) => handleCustomSpecificNameChange(row.id, event.target.value)}
                placeholder="e.g. Brand, Model"
                style={{
                  height: "42px",
                  borderRadius: "10px",
                  border: "1px solid #c7c7c7",
                  padding: "0 12px",
                  fontSize: "14px"
                }}
              />
            </label>
          </div>
          <div style={{ flex: "1 1 320px", minWidth: "260px" }}>
            <label style={{ display: "flex", flexDirection: "column", gap: "4px", fontSize: "13px", color: "#555" }}>
              <span>Value(s)</span>
              {values.map((value, index) => (
                <div key={`${row.id}-value-${index}`} style={{ display: "flex", alignItems: "center", gap: "10px", marginBottom: "8px" }}>
                  <input
                    type="text"
                    value={value ?? ""}
                    onChange={(event) => handleCustomSpecificValueChange(row.id, index, event.target.value)}
                    placeholder="Enter a value"
                    style={{
                      flex: 1,
                      height: "42px",
                      borderRadius: "10px",
                      border: "1px solid #c7c7c7",
                      padding: "0 12px",
                      fontSize: "14px"
                    }}
                  />
                  <button
                    type="button"
                    onClick={() => handleCustomSpecificRemoveValue(row.id, index)}
                    disabled={values.length === 1 && (value ?? "").trim().length === 0}
                    style={{
                      border: "none",
                      background: "transparent",
                      color: "#3665f3",
                      cursor: values.length === 1 && (value ?? "").trim().length === 0 ? "not-allowed" : "pointer",
                      fontSize: "13px"
                    }}
                  >
                    Remove
                  </button>
                </div>
              ))}
              <button
                type="button"
                onClick={() => handleCustomSpecificAddValue(row.id)}
                style={{
                  alignSelf: "flex-start",
                  border: "none",
                  background: "transparent",
                  color: "#3665f3",
                  cursor: "pointer",
                  fontWeight: 600,
                  fontSize: "13px"
                }}
              >
                + Add another value
              </button>
            </label>
          </div>
        </div>
        <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center" }}>
          <span style={{ fontSize: "12px", color: "#6f7780" }}>Custom specifics show on the item page and help buyers filter search results.</span>
          <button
            type="button"
            onClick={() => handleRemoveCustomSpecificRow(row.id)}
            style={{
              border: "none",
              background: "transparent",
              color: "#c40000",
              cursor: "pointer",
              fontWeight: 600,
              fontSize: "13px"
            }}
          >
            Remove specific
          </button>
        </div>
      </div>
    );
  };
  // chọn file
  const uploadListingMedia = async (entryId, file) => {
    try {
      const response = await FileService.upload(file);
      const uploadedUrl = response?.data;

      if (!uploadedUrl || typeof uploadedUrl !== "string") {
        throw new Error("Missing uploaded file url");
      }

      setFiles((prev) =>
        prev.map((item) => {
          if (item.id !== entryId) {
            return item;
          }

          if (item.preview && item.preview.startsWith("blob:")) {
            URL.revokeObjectURL(item.preview);
          }

          return {
            ...item,
            src: uploadedUrl,
            preview: null,
            uploading: false
          };
        })
      );
    } catch (error) {
      console.error("File upload failed", error);
      Notice({
        msg: `Failed to upload ${file.name}.`,
        desc: "Please try again.",
        isSuccess: false
      });

      setFiles((prev) =>
        prev.filter((item) => {
          if (item.id !== entryId) {
            return true;
          }

          if (item.preview && item.preview.startsWith("blob:")) {
            URL.revokeObjectURL(item.preview);
          }

          return false;
        })
      );
    }
  };

  const handleFilesSelected = (event) => {
    const selectedFiles = Array.from(event.target.files ?? []);
    event.target.value = null;

    if (!selectedFiles.length) {
      return;
    }

    const accepted = selectedFiles.filter(
      (file) => file.type.startsWith("image") || file.type.startsWith("video")
    );

    if (!accepted.length) {
      Notice({ msg: "Only image or video files can be added.", isSuccess: false });
      return;
    }

    const availableSlots = Math.max(0, MAX_MEDIA_ITEMS - files.length);
    if (availableSlots <= 0) {
      Notice({ msg: "You have reached the maximum number of media items.", isSuccess: false });
      return;
    }

    const limited = accepted.slice(0, availableSlots);

    const prepared = limited.map((file) => {
      const previewUrl = URL.createObjectURL(file);
      return {
        id: createMediaId(),
        name: file.name,
        type: file.type.startsWith("image") ? "image" : "video",
        size: file.size,
        src: previewUrl,
        preview: previewUrl,
        uploading: true
      };
    });

    setFiles((prev) => [...prev, ...prepared]);

    prepared.forEach((entry, index) => {
      const originalFile = limited[index];
      uploadListingMedia(entry.id, originalFile);
    });

    if (accepted.length > limited.length) {
      Notice({
        msg: `Only ${availableSlots} file(s) were added due to the upload limit.`,
        isSuccess: false
      });
    }
  };

  const handleDelete = (id) => {
    setFiles((prev) => {
      const target = prev.find((item) => item.id === id);
      if (target?.preview && target.preview.startsWith("blob:")) {
        URL.revokeObjectURL(target.preview);
      }
      return prev.filter((item) => item.id !== id);
    });
  };

  const handleButtonClick = () => {
    if (fileInputRef.current) {
      fileInputRef.current.click();
    }
  };


  const [scheduleEnabled, setScheduleEnabled] = useState(false);
  const [date, setDate] = useState("");
  const [hour, setHour] = useState("12");
  const [minute, setMinute] = useState("00");
  const [ampm, setAmpm] = useState("AM");

  useEffect(() => {
    if (!statusPermissions.showSchedule && scheduleEnabled) {
      setScheduleEnabled(false);
    }
  }, [scheduleEnabled, statusPermissions.showSchedule]);
  const toggleScheduleEnabled = () => setScheduleEnabled(!scheduleEnabled);


  const [allowOfferEnabled, setAllowOfferEnabled] = useState(false);
  const toggleAllowOfferEnabled = () => setAllowOfferEnabled(!allowOfferEnabled);

  const [categoryModalOpen, setCategoryModalOpen] = useState(false);
  const [conditionModalOpen, setConditionModalOpen] = useState(false);

  const resetFormState = useCallback(() => {
    releaseVariationMedia(variationMediaRef.current);
    variationMediaRef.current = null;
    setFiles([]);
    setVariationData(null);
    setSelectedCategoryId(null);
    setCategoryPath([]);
    setCategoryDetail(null);
    setSelectedConditionId(null);
    setConditions([]);
    setSpecificSelections({});
    setCustomSpecificInputs({});
    setCustomSpecificRows([]);
    setOpenSpecificId(null);
    setQuantity("1");
    setTitle("");
    setSku("");
    setListingDescription("");
    setConditionDescription("");
    setStartingBid("");
    setBuyItNowPrice("");
    setReservePrice("");
    setFixedPrice("");
    setMinimumOffer("");
    setAutoAcceptOffer("");
    setIsSubmitting(false);
    setPriceFormat("1");
    setAuctionDuration("7");
    setScheduleEnabled(false);
    setDate("");
    setHour("12");
    setMinute("00");
    setAmpm("AM");
    setAllowOfferEnabled(false);
    setCategoryModalOpen(false);
    setConditionModalOpen(false);
    setListingStatus(DEFAULT_LISTING_STATUS);
  }, [releaseVariationMedia]);

  const buildListingDraft = useCallback(() => ({
    listingId: activeListingId,
    listingStatus,
    files,
    title,
    sku,
    listingDescription,
    conditionDescription,
    startingBid,
    buyItNowPrice,
    reservePrice,
    fixedPrice,
    minimumOffer,
    autoAcceptOffer,
    selectedCategoryId,
    categoryPath,
    categoryDetail,
    selectedConditionId,
    conditions,
    specificSelections,
    customSpecificInputs,
    customSpecificRows,
    openSpecificId,
    quantity,
    priceFormat,
    auctionDuration,
    scheduleEnabled,
    date,
    hour,
    minute,
    ampm,
    allowOfferEnabled
  }), [
    activeListingId,
    listingStatus,
    files,
    title,
    sku,
    listingDescription,
    conditionDescription,
    startingBid,
    buyItNowPrice,
    reservePrice,
    fixedPrice,
    minimumOffer,
    autoAcceptOffer,
    selectedCategoryId,
    categoryPath,
    categoryDetail,
    selectedConditionId,
    conditions,
    specificSelections,
    customSpecificInputs,
    customSpecificRows,
    openSpecificId,
    quantity,
    priceFormat,
    auctionDuration,
    scheduleEnabled,
    date,
    hour,
    minute,
    ampm,
    allowOfferEnabled
  ]);

  const hydrateListing = useCallback(
    async (listing) => {
      if (!listing) {
        return null;
      }

      const rawStatus = listing.status ?? listing.Status;
      const resolvedStatus = LISTING_STATUS_LABELS[rawStatus] ?? (listing.isDraft ? "Draft" : "Active");
      setListingStatus(resolvedStatus);

      releaseVariationMedia(variationMediaRef.current);
      variationMediaRef.current = null;
      setVariationData(null);

      const normalizedFiles = Array.isArray(listing.listingImages)
        ? listing.listingImages
          .filter((image) => typeof image?.url === "string" && image.url.trim().length > 0)
          .map((image, index) =>
            createStaticMediaEntry(
              image.url,
              image.isPrimary ? "Primary photo" : `Photo ${index + 1}`,
              index
            )
          )
        : [];

      setFiles(normalizedFiles);

      setTitle(listing.title ?? "");
      setSku(listing.sku ?? "");
      setListingDescription(listing.listingDescription ?? "");
      setConditionDescription(listing.conditionDescription ?? "");
      setSelectedCategoryId(listing.categoryId ?? null);

      let categoryDetailDto = null;
      if (listing.categoryId) {
        try {
          categoryDetailDto = await CategoryService.getCategoryDetail(listing.categoryId);
        } catch (error) {
          // eslint-disable-next-line no-console
          console.error("Failed to fetch category detail", error);
          Notice({
            msg: "Unable to load category details.",
            desc: error?.response?.data?.message || error?.message,
            isSuccess: false
          });
        }
      }

      setCategoryDetail(categoryDetailDto ?? null);
      setCategoryPath(categoryDetailDto ? [categoryDetailDto.name] : []);

      const normalizedConditions = Array.isArray(categoryDetailDto?.conditions)
        ? categoryDetailDto.conditions.map((condition) => ({ ...condition }))
        : [];

      if (
        listing.conditionId &&
        normalizedConditions.every((condition) => condition.id !== listing.conditionId)
      ) {
        normalizedConditions.push({
          id: listing.conditionId,
          name: listing.conditionDescription ?? "Selected condition",
          description: listing.conditionDescription ?? ""
        });
      }

      setConditions(normalizedConditions);
      setSelectedConditionId(listing.conditionId ?? null);

      const listingItemSpecifics = Array.isArray(listing.itemSpecifics)
        ? listing.itemSpecifics
        : [];

      const nextSpecificSelections = {};
      const customSpecificMap = new Map();

      const knownSpecificsByName = categoryDetailDto && Array.isArray(categoryDetailDto.specifics)
        ? new Map(
          categoryDetailDto.specifics
            .filter((specific) => typeof specific?.name === "string")
            .map((specific) => [specific.name.toLowerCase(), specific])
        )
        : new Map();

      listingItemSpecifics.forEach((itemSpecific) => {
        const rawName = String(itemSpecific?.name ?? "").trim();
        if (!rawName) {
          return;
        }

        const normalizedName = rawName.toLowerCase();
        const values = Array.isArray(itemSpecific?.values)
          ? itemSpecific.values
            .map((value) => String(value ?? "").trim())
            .filter((value) => value.length > 0)
          : [];

        if (!values.length) {
          return;
        }

        const knownSpecific = knownSpecificsByName.get(normalizedName);
        if (knownSpecific) {
          if (knownSpecific.allowMultiple) {
            const current = Array.isArray(nextSpecificSelections[knownSpecific.id])
              ? nextSpecificSelections[knownSpecific.id]
              : [];
            const merged = Array.from(new Set([...current, ...values]));
            nextSpecificSelections[knownSpecific.id] = merged;
          } else {
            nextSpecificSelections[knownSpecific.id] = values[0];
          }
          return;
        }

        if (customSpecificMap.has(normalizedName)) {
          const existing = customSpecificMap.get(normalizedName);
          const mergedValues = Array.from(new Set([...(existing.values ?? []), ...values]));
          customSpecificMap.set(normalizedName, {
            ...existing,
            values: mergedValues
          });
        } else {
          customSpecificMap.set(normalizedName, {
            id: createCustomSpecificId(),
            name: rawName,
            values: values.length ? values : [""]
          });
        }
      });

      setSpecificSelections(nextSpecificSelections);
      setCustomSpecificRows(Array.from(customSpecificMap.values()));
      setCustomSpecificInputs({});
      setOpenSpecificId(null);

      const formatValue = listing.format === 2 ? "2" : "1";
      setPriceFormat(formatValue);

      if (formatValue === "1") {
        setStartingBid(listing.startPrice != null ? String(listing.startPrice) : "");
        setReservePrice(listing.reservePrice != null ? String(listing.reservePrice) : "");
        setBuyItNowPrice(listing.buyItNowPrice != null ? String(listing.buyItNowPrice) : "");
        setAuctionDuration(String(listing.duration ?? 7));
        setFixedPrice("");
        setQuantity(listing.quantity != null ? String(listing.quantity) : "1");
      } else {
        setStartingBid("");
        setReservePrice("");
        setBuyItNowPrice("");
        setAuctionDuration(String(listing.duration ?? 7));

        const isMultiVariation = listing.type === 2;
        if (isMultiVariation && Array.isArray(listing.variations) && listing.variations.length) {
          const attributeMap = new Map();
          const optionMap = new Map();

          const variationRows = listing.variations
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
                  const matchedSpecific = categoryDetailDto?.specifics?.find(
                    (item) => item.name.toLowerCase() === name.toLowerCase()
                  );

                  const attributeId =
                    matchedSpecific?.id ??
                    `attr-${name.toLowerCase().replace(/[^a-z0-9]+/g, "-") || Math.random().toString(36).slice(2, 8)}`;

                  attributeMap.set(name, { id: attributeId, name });
                }
              });

              const variationImages = Array.isArray(variation?.variationImages)
                ? variation.variationImages
                : [];
              const prioritizedImage =
                variationImages.find((image) => image?.isPrimary) ?? variationImages[0];

              const photoEntry =
                prioritizedImage && typeof prioritizedImage.url === "string" && prioritizedImage.url.length
                  ? createStaticMediaEntry(prioritizedImage.url, "Variation photo")
                  : null;

              const key = buildVariationKeyFromAttributes(attributeValues) || `variation-${index}`;

              return {
                key,
                attributeValues,
                price: variation?.price != null ? String(variation.price) : "",
                quantity: variation?.quantity != null ? String(variation.quantity) : "",
                sku: variation?.sku ?? "",
                photo: photoEntry
              };
            })
            .filter(Boolean);

          const attributes = Array.from(attributeMap.values()).map((attribute, index) => ({
            ...attribute,
            selected: index === 0
          }));

          const selectedOptions = {};
          optionMap.forEach((set, name) => {
            selectedOptions[name] = Array.from(set);
          });

          const defaultPhotos = normalizedFiles.length
            ? normalizedFiles.map((item, index) => createStaticMediaEntry(item.src, item.name, index))
            : [];

          const variationPayload = {
            attributes,
            selectedOptions,
            variationRows,
            excludedKeys: [],
            defaultPhotos,
            categoryId: listing.categoryId ?? null,
            categoryName: categoryDetailDto?.name ?? null,
            mode:
              variationRows.length && attributes.length && Object.keys(selectedOptions).length
                ? "summary"
                : "edit"
          };

          variationMediaRef.current = variationPayload;
          setVariationData(variationPayload);

          setFixedPrice("");
          setQuantity("1");
        } else {
          setFixedPrice(listing.price != null ? String(listing.price) : "");
          setQuantity(listing.quantity != null ? String(listing.quantity) : "1");
        }
      }

      const allowOffers = Boolean(listing.allowOffers);
      setAllowOfferEnabled(allowOffers);
      setMinimumOffer(
        allowOffers && listing.minimumOffer != null ? String(listing.minimumOffer) : ""
      );
      setAutoAcceptOffer(
        allowOffers && listing.autoAcceptOffer != null ? String(listing.autoAcceptOffer) : ""
      );

      const hasSchedule = Boolean(listing.scheduledStartTime);
      if (hasSchedule) {
        const scheduledDate = new Date(listing.scheduledStartTime);
        if (!Number.isNaN(scheduledDate.getTime())) {
          setScheduleEnabled(true);
          setDate(scheduledDate.toISOString().slice(0, 10));

          const hours = scheduledDate.getHours();
          const minutes = scheduledDate.getMinutes();
          const ampmValue = hours >= 12 ? "PM" : "AM";
          let hourValue = hours % 12;
          if (hourValue === 0) {
            hourValue = 12;
          }

          setHour(String(hourValue).padStart(2, "0"));
          setMinute(String(minutes).padStart(2, "0"));
          setAmpm(ampmValue);
        } else {
          setScheduleEnabled(false);
          setDate("");
          setHour("12");
          setMinute("00");
          setAmpm("AM");
        }
      } else {
        setScheduleEnabled(false);
        setDate("");
        setHour("12");
        setMinute("00");
        setAmpm("AM");
      }

      return listing.listingId ?? null;
    }, [releaseVariationMedia]);

  useEffect(() => {
    if (skipNextListingLoadRef.current) {
      skipNextListingLoadRef.current = false;
      return;
    }

    let isMounted = true;

    const loadListing = async (identifier) => {
      setIsLoadingListing(true);
      try {
        const data = await ListingService.getById(identifier);
        if (!isMounted) {
          return;
        }

        if (!data) {
          Notice({
            msg: "Unable to load listing.",
            desc: "Please try again.",
            isSuccess: false
          });
          resetFormState();
          setActiveListingId(null);
          navigate("/listing-form", { replace: true });
          return;
        }

        const hydratedId = await hydrateListing(data);
        if (!isMounted) {
          return;
        }

        setActiveListingId(hydratedId ?? identifier);
      } catch (error) {
        if (!isMounted) {
          return;
        }

        // eslint-disable-next-line no-console
        console.error("Failed to load listing", error);
        const responseMessage = error?.response?.data?.message || error?.message;
        Notice({
          msg: "Unable to load listing.",
          desc: responseMessage || "Please try again.",
          isSuccess: false
        });
        resetFormState();
        setActiveListingId(null);
        navigate("/listing-form", { replace: true });
      } finally {
        if (isMounted) {
          setIsLoadingListing(false);
        }
      }
    };

    if (listingIdParam) {
      const identifiersMatch =
        activeListingId &&
        String(activeListingId).toLowerCase() === String(listingIdParam).toLowerCase();

      if (!identifiersMatch) {
        loadListing(listingIdParam);
      }
    } else if (activeListingId) {
      resetFormState();
      setActiveListingId(null);
    }

    return () => {
      isMounted = false;
    };
  }, [activeListingId, hydrateListing, listingIdParam, navigate, resetFormState]);

  const buildItemSpecificsPayload = () => {
    const categorySpecifics = Array.isArray(categoryDetail?.specifics)
      ? categoryDetail.specifics
      : [];

    const baseSpecifics = categorySpecifics.reduce((acc, specific) => {
      const selection = specificSelections[specific.id];
      if (selection === undefined || selection === null) {
        return acc;
      }

      const values = Array.isArray(selection) ? selection : [selection];
      const sanitized = values
        .map((value) => String(value ?? "").trim())
        .filter((value) => value.length > 0);

      if (sanitized.length === 0) {
        return acc;
      }

      acc.push({
        name: specific.name,
        values: sanitized
      });

      return acc;
    }, []);

    const sanitizedCustomSpecifics = customSpecificRows.reduce((acc, row) => {
      const trimmedName = String(row?.name ?? "").trim();
      if (!trimmedName) {
        return acc;
      }

      const values = Array.isArray(row?.values) ? row.values : [];
      const sanitizedValues = values
        .map((value) => String(value ?? "").trim())
        .filter((value) => value.length > 0);

      if (!sanitizedValues.length) {
        return acc;
      }

      acc.push({
        name: trimmedName,
        values: sanitizedValues
      });

      return acc;
    }, []);

    return [...baseSpecifics, ...sanitizedCustomSpecifics];
  };

  const buildListingImagesPayload = () => {
    const baseImages = files
      .filter((file) => !file.uploading && file?.src && file.type === "image")
      .map((file, index) => ({
        url: file.src,
        isPrimary: index === 0
      }));

    if (baseImages.length > 0) {
      return baseImages;
    }

    const variationDefaults = Array.isArray(variationData?.defaultPhotos)
      ? variationData.defaultPhotos
        .filter((item) => !item?.uploading && item?.src && item.type === "image")
        .map((item, index) => ({
          url: item.src,
          isPrimary: index === 0
        }))
      : [];

    return variationDefaults;
  };

  const buildVariationsPayload = () => {
    if (!variationData || !Array.isArray(variationData.variationRows)) {
      return { variations: [], issues: [] };
    }

    const excludedKeys = new Set(
      Array.isArray(variationData.excludedKeys) ? variationData.excludedKeys : []
    );

    const issues = [];
    const variations = [];

    variationData.variationRows.forEach((row) => {
      if (!row || excludedKeys.has(row.key)) {
        return;
      }

      const priceValue = parseCurrencyInput(row.price);
      if (priceValue === null || priceValue <= 0) {
        issues.push(`Variation ${row.key} needs a valid price.`);
        return;
      }

      const quantityValue = parseIntegerInput(row.quantity);
      if (quantityValue === null || quantityValue <= 0) {
        issues.push(`Variation ${row.key} needs a quantity greater than zero.`);
        return;
      }

      const specifics = Object.entries(row.attributeValues ?? {}).reduce((acc, [name, value]) => {
        const trimmed = String(value ?? "").trim();
        if (trimmed.length === 0) {
          return acc;
        }

        acc.push({
          name,
          values: [trimmed]
        });
        return acc;
      }, []);

      if (specifics.length === 0) {
        issues.push(`Variation ${row.key} is missing attribute selections.`);
        return;
      }

      const skuValue = (row.sku ?? "").trim() || row.key;
      const variationImages = row.photo?.src && !row.photo?.uploading
        ? [
          {
            url: row.photo.src,
            isPrimary: true
          }
        ]
        : [];

      variations.push({
        sku: skuValue,
        price: priceValue,
        quantity: quantityValue,
        variationSpecifics: specifics,
        variationImages
      });
    });

    return { variations, issues };
  };

  const buildScheduledStartTime = () => {
    if (!scheduleEnabled) {
      return null;
    }

    const trimmedDate = date.trim();
    if (!trimmedDate) {
      return null;
    }

    const [year, month, day] = trimmedDate.split("-").map((part) => Number.parseInt(part, 10));
    if (!Number.isFinite(year) || !Number.isFinite(month) || !Number.isFinite(day)) {
      return null;
    }

    const parsedHour = Number.parseInt(hour, 10);
    const parsedMinute = Number.parseInt(minute, 10);
    if (!Number.isFinite(parsedHour) || !Number.isFinite(parsedMinute)) {
      return null;
    }

    let hour24 = parsedHour % 12;
    if (ampm === "PM" && hour24 !== 12) {
      hour24 += 12;
    }
    if (ampm === "AM" && hour24 === 12) {
      hour24 = 0;
    }

    const scheduled = new Date(year, month - 1, day, hour24, parsedMinute, 0);
    if (Number.isNaN(scheduled.getTime())) {
      return null;
    }

    return scheduled.toISOString();
  };

  const handleSubmit = async (saveAsDraft = false) => {
    if (!isTemplateMode && saveAsDraft && !statusPermissions.allowSaveDraft) {
      Notice({
        msg: "Active listings can't be saved as drafts.",
        isSuccess: false
      });
      return;
    }

    if (isTemplateMode && isTemplateSaving) {
      return;
    }

    const listingHasPendingUploads = files.some((file) => file.uploading);
    const variationHasPendingUploads = Boolean(
      variationData &&
      ((Array.isArray(variationData.defaultPhotos) &&
        variationData.defaultPhotos.some((photo) => photo?.uploading)) ||
        (Array.isArray(variationData.variationRows) &&
          variationData.variationRows.some((row) => row?.photo?.uploading)))
    );

    if (listingHasPendingUploads || variationHasPendingUploads) {
      Notice({
        msg: "Please wait for all media uploads to finish before submitting.",
        isSuccess: false
      });
      return;
    }
    if (isLoadingListing) {
      Notice({
        msg: "Listing is still loading.",
        desc: "Please wait a moment and try again.",
        isSuccess: false
      });
      return;
    }
    if (!isTemplateMode && isSubmitting) {
      return;
    }

    const errors = [];

    const trimmedTitle = title.trim();
    const trimmedSku = sku.trim();
    const trimmedListingDescription = listingDescription.trim();
    const trimmedConditionDescription = conditionDescription.trim();

    if (!trimmedTitle) {
      errors.push("Title is required.");
    }

    if (!trimmedSku) {
      errors.push("SKU is required.");
    }

    if (!trimmedListingDescription) {
      errors.push("Listing description is required.");
    }

    if (!selectedCategoryId) {
      errors.push("Select a category.");
    }

    if (!trimmedConditionDescription) {
      errors.push("Condition description is required.");
    }

    if (!hasVariations) {
      requiredSpecifics.forEach((specific) => {
        const selection = specificSelections[specific.id];
        const values = Array.isArray(selection) ? selection : [selection];
        const hasValue = values.some((value) => String(value ?? "").trim().length > 0);
        if (!hasValue) {
          errors.push(`${specific.name} is required.`);
        }
      });
    }

    const itemSpecificsPayload = buildItemSpecificsPayload();
    const listingImagesPayload = buildListingImagesPayload();

    const minimumOfferValue = allowOfferEnabled ? parseCurrencyInput(minimumOffer) : null;
    const autoAcceptValue = allowOfferEnabled ? parseCurrencyInput(autoAcceptOffer) : null;

    if (allowOfferEnabled && minimumOffer && minimumOfferValue === null) {
      errors.push("Enter a valid minimum offer amount.");
    }

    if (allowOfferEnabled && autoAcceptOffer && autoAcceptValue === null) {
      errors.push("Enter a valid auto accept amount.");
    }

    const payload = {
      format: priceFormat === "2" ? 2 : 1,
      type: null,
      title: trimmedTitle,
      sku: trimmedSku,
      listingDescription: trimmedListingDescription,
      categoryId: selectedCategoryId,
      conditionId: selectedConditionId || null,
      conditionDescription: trimmedConditionDescription,
      itemSpecifics: itemSpecificsPayload,
      listingImages: listingImagesPayload.length > 0 ? listingImagesPayload : null,
      price: null,
      quantity: null,
      variations: null,
      startPrice: null,
      reservePrice: null,
      buyItNowPrice: null,
      duration: 0,
      scheduledStartTime: null,
      allowOffers: allowOfferEnabled,
      minimumOffer: allowOfferEnabled ? minimumOfferValue : null,
      autoAcceptOffer: allowOfferEnabled ? autoAcceptValue : null,
      isDraft: saveAsDraft
    };

    if (priceFormat === "1") {
      const durationValue = Number.parseInt(auctionDuration, 10);
      if (!Number.isFinite(durationValue)) {
        errors.push("Select an auction duration.");
      }

      payload.duration = Number.isFinite(durationValue) ? durationValue : 7;

      const startPriceValue = parseCurrencyInput(startingBid);
      if (startPriceValue === null || startPriceValue <= 0) {
        errors.push("Enter a valid starting bid.");
      }
      payload.startPrice = startPriceValue;

      const reservePriceValue = parseCurrencyInput(reservePrice);
      if (reservePrice && reservePriceValue === null) {
        errors.push("Enter a valid reserve price.");
      }
      payload.reservePrice = reservePriceValue;

      const buyItNowValue = parseCurrencyInput(buyItNowPrice);
      if (buyItNowPrice && buyItNowValue === null) {
        errors.push("Enter a valid Buy It Now price.");
      }
      payload.buyItNowPrice = buyItNowValue;
    } else {
      const { variations, issues } = buildVariationsPayload();
      if (issues.length > 0) {
        errors.push(...issues);
      }

      if (variations.length > 0) {
        payload.type = 2;
        payload.variations = variations;
      } else {
        const fixedPriceValue = parseCurrencyInput(fixedPrice);
        const quantityValue = parseIntegerInput(quantity);

        if (fixedPriceValue === null || fixedPriceValue <= 0) {
          errors.push("Enter a valid item price.");
        }

        if (quantityValue === null || quantityValue <= 0) {
          errors.push("Enter a valid quantity.");
        }

        payload.type = 1;
        payload.price = fixedPriceValue;
        payload.quantity = quantityValue;
      }
    }

    const scheduledStartTime = buildScheduledStartTime();
    if (scheduleEnabled) {
      if (!scheduledStartTime) {
        errors.push("Provide a valid schedule date and time.");
      }
      payload.scheduledStartTime = scheduledStartTime;
    }

    if (errors.length > 0) {
      console.warn("Form submission blocked", errors);
      Notice({
        msg: isTemplateMode ? "We couldn't save your template." : "We couldn't submit your listing.",
        desc: errors.join(" "),
        isSuccess: false
      });
      return;
    }

    const requestPayload = isEditMode
      ? {
        ...payload,
        listingId: activeListingId
      }
      : payload;

    if (isTemplateMode) {
      const primaryImageUrl = listingImagesPayload.length > 0 ? listingImagesPayload[0].url : null;

      const trimmedTemplateName = (templateName ?? "").trim();
      const trimmedTemplateDescription = (templateDescription ?? "").trim();
      const trimmedTemplateFormatLabel = (templateFormatLabel ?? "").trim();
      const trimmedTemplateThumbnailUrl = (templateThumbnailUrl ?? "").trim();

      const resolvedTemplateName = trimmedTemplateName || trimmedTitle || "Untitled template";
      const resolvedFormatLabel = trimmedTemplateFormatLabel || priceFormats.find((option) => option.value === priceFormat)?.text || null;
      const resolvedThumbnailUrl = trimmedTemplateThumbnailUrl || primaryImageUrl || null;

      const draftSnapshot = buildListingDraft();
      const sanitizedDraft = draftSnapshot
        ? JSON.parse(
          JSON.stringify({
            ...draftSnapshot,
            listingId: null,
            listingStatus: "Draft"
          })
        )
        : null;

      const sanitizedVariationState = variationData
        ? JSON.parse(JSON.stringify(variationData))
        : null;

      const listingRequestPayload = JSON.parse(JSON.stringify(payload));

      const templatePayload = {
        version: 2,
        listingRequest: listingRequestPayload,
        formState: sanitizedDraft,
        variationState: sanitizedVariationState
      };

      const templateRequestPayload = {
        name: resolvedTemplateName,
        description: trimmedTemplateDescription || null,
        formatLabel: resolvedFormatLabel,
        thumbnailUrl: resolvedThumbnailUrl,
        payload: templatePayload
      };

      try {
        setIsTemplateSaving(true);

        if (templateContext.mode === "edit" && templateContext.templateId) {
          await ListingTemplateService.update(templateContext.templateId, templateRequestPayload);
          Notice({
            msg: "Template updated.",
            isSuccess: true
          });
        } else {
          await ListingTemplateService.create(templateRequestPayload);
          Notice({
            msg: "Template saved.",
            isSuccess: true
          });
        }

  navigate("/listings/listing-templates", { replace: true });
      } catch (error) {
        // eslint-disable-next-line no-console
        console.error("Template submission failed", error);
        const responseMessage = error?.response?.data?.message || error?.message;
        Notice({
          msg: "Unable to save template.",
          desc: responseMessage || "Please try again.",
          isSuccess: false
        });
      } finally {
        setIsTemplateSaving(false);
      }

      return;
    }

    try {
      setIsSubmitting(true);
      if (isEditMode) {
        await ListingService.update(requestPayload);
      } else {
        await ListingService.create(requestPayload);
      }

      const successMessage = saveAsDraft
        ? isEditMode
          ? "Draft updated successfully."
          : "Draft saved successfully."
        : isEditMode
          ? "Listing updated successfully."
          : "Listing created successfully.";

      Notice({
        msg: successMessage,
        isSuccess: true
      });

      const resolveStatusSlug = () => {
        if (saveAsDraft) {
          return "drafts";
        }

        if (!saveAsDraft && scheduleEnabled && statusPermissions.showSchedule) {
          return "scheduled";
        }

        if (!isEditMode) {
          return "active";
        }

        const normalizedStatus = String(listingStatus).toLowerCase();
        switch (normalizedStatus) {
          case "draft":
            return "drafts";
          case "scheduled":
            return "scheduled";
          case "ended":
            return "ended";
          case "active":
          default:
            return "active";
        }
      };

      navigate(`/listings/${resolveStatusSlug()}`);
    } catch (error) {
      // eslint-disable-next-line no-console
      console.error("Listing submission failed", error);
      const responseMessage = error?.response?.data?.message || error?.message;
      Notice({
        msg: "Listing request failed.",
        desc: responseMessage || "Please try again.",
        isSuccess: false
      });
    } finally {
      setIsSubmitting(false);
    }
  };

  const primaryCategory = categoryPath.length > 0 ? categoryPath[categoryPath.length - 1] : null;
  const primaryCategoryLabel =
    typeof primaryCategory === "string"
      ? primaryCategory
      : primaryCategory?.name ?? "";
  const parentCategoryTrail = categoryPath
    .slice(0, -1)
    .map((item) => (typeof item === "string" ? item : item?.name ?? ""))
    .filter(Boolean)
    .join(" > ");

  if (!canManageListing && isCreatingNewListing) {
    return null;
  }

  return (
    <div className="listing-form">
      {isLoadingListing && (
        <div className="listing-form__loading-overlay">
          <LoadingScreen isOverlay />
        </div>
      )}
      <div
        style={{
          position: "fixed",
          top: 0,
          left: "50%",
          transform: "translateX(-50%)",
          width: "1100px",
          height: "70px",
          display: "flex",
          alignItems: "center",
          backgroundColor: "#fff",
          justifyContent: "center", // căn giữa logo
          borderBottom: "1px solid #ddd",
          zIndex: 1000,
        }}
      >
        <img src={ebayLogo} alt="eBay Logo" style={{ height: "60px" }} />
      </div>

      <div style={{ maxWidth: "1000px", margin: "0 auto", padding: "16px", marginTop: "80px" }}>


        <h1>Complete your listing</h1>

        {isEditMode && (
          <div style={{ marginBottom: "32px", display: "flex", flexDirection: "column", gap: "8px" }}>
            <span style={{ fontSize: "14px", fontWeight: 600 }}>
              Status: <span style={{ color: "#3665f3" }}>{listingStatusLabel}</span>
            </span>
            {!statusPermissions.allowFormatSelection && (
              <span style={{ fontSize: "13px", color: "#6f7780" }}>
                Format and category edits are locked while this listing is live.
              </span>
            )}
          </div>
        )}

        <div style={{ marginBottom: "48px" }}>
          <h3>PHOTOS & VIDEO</h3>
          <p>
            You can add up to 24 photos and a 1-minute video. Buyers want to see
            all details and angles.{" "}
            <a href="#7">
              Tips for taking pro photos
            </a>
          </p>

          <div style={{ marginBottom: "8px", fontSize: "14px" }}>
            {files.length}/{MAX_MEDIA_ITEMS}
          </div>

          {/* input file luôn render ở đây */}
          <input
            type="file"
            multiple
            accept="image/*,video/*"
            ref={fileInputRef}
            style={{ display: "none" }}
            onChange={handleFilesSelected}
          />
          <div style={{ display: "none" }}><RemoveIcon id="icon-delete-16" style={{ width: 24, height: 24 }} /></div>
          {/* Layout: ảnh main bên trái, grid nhỏ bên phải */}
          <div style={{ display: "flex", gap: "16px" }}>
            {/* Ô lớn hiển thị ảnh đầu tiên hoặc khung upload */}
            <div
              style={{
                border: files[0] ? "none" : "3px dashed #aaa",
                borderRadius: "8px",
                width: "350px",
                height: "350px",
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
                alignItems: "center",
                textAlign: "center",
              }}
            >
              {files[0] ? (
                <EbayFilePreviewCard
                  a11yCancelUploadText="Cancel upload"
                  deleteText="Delete"
                  file={files[0]}
                  onDelete={() => handleDelete(files[0].id)}
                />
              ) : (
                <>
                  <AddImageIcon style={{ width: 24, height: 24 }} />

                  <p style={{ fontSize: "20px" }}>Drag and drop files</p>
                  <EbayButton
                    style={{ marginTop: "12px" }}
                    priority="tertiary"
                    onClick={handleButtonClick} // ✅ chỉ button này mới mở dialog
                  >
                    Upload from computer
                  </EbayButton>
                </>
              )}
            </div>


            {/* Grid nhỏ 5 cột */}
            <div
              style={{
                display: "grid",
                gridTemplateColumns: "repeat(5, 115px)",
                gridAutoRows: "115px",
                gap: "8px",
                alignContent: "flex-start",
              }}
            >
              {/* render ảnh còn lại */}
              {files.slice(1).map((file) => (
                <EbayFilePreviewCard
                  key={file.id}
                  a11yCancelUploadText="Cancel upload"
                  deleteText="Delete"
                  file={file}
                  onDelete={() => handleDelete(file.id)}
                />
              ))}

              {/* Ô Add */}
              {files.length < MAX_MEDIA_ITEMS && (
                <div
                  onClick={handleButtonClick}
                  style={{
                    background: "#f5f5f5",
                    borderRadius: "8px",
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "center",
                    alignItems: "center",
                    cursor: "pointer",
                    fontSize: "14px",
                  }}
                >
                  <AddImageIcon style={{ width: 24, height: 24 }} />
                  <span>Add</span>
                </div>
              )}

              {/* Các ô trống giữ layout */}
              {Array.from({
                length: Math.max(
                  0,
                  15 - (files.slice(1).length + (files.length < MAX_MEDIA_ITEMS ? 1 : 0))
                ),
              }).map((_, i) => (
                <div
                  key={`empty-${i}`}
                  onClick={handleButtonClick}
                  style={{
                    background: "#f5f5f5",
                    borderRadius: "8px",
                    cursor: "pointer",
                  }}
                />
              ))}
            </div>
          </div>
        </div>

        <hr />

        <div style={{ marginBottom: "48px" }}>
          <h3>TITLE</h3>


          <EbayField layout="block">
            <EbayLabel stacked>Item title</EbayLabel>
            <EbayTextbox
              name="itemTitle"
              value={title}
              onInputChange={(event, { value }) => setTitle(value)}
              fluid
              style={{ height: "45px" }}
            />
          </EbayField>

          <EbayField layout="block">
            <EbayLabel stacked>Custom label (SKU)</EbayLabel>
            <EbayTextbox
              name="itemSKU"
              value={sku}
              onInputChange={(event, { value }) => setSku(value)}
              style={{ width: "400px" }}
            />
          </EbayField>

        </div>

        <hr />

        <div
          style={{
            marginBottom: "48px",
            display: "flex",
            justifyContent: "space-between",
            alignItems: "flex-start"
          }}
        >
          {/* Bên trái */}

          <div>
            <h3 style={{ marginBottom: "8px" }}>ITEM CATEGORY</h3>
            {!selectedCategoryId && (
              <EbaySectionNotice status="information">
                <EbayNoticeContent>
                  Select a category to enable tailored item specifics and pricing guidance.
                </EbayNoticeContent>
              </EbaySectionNotice>
            )}
            {selectedCategoryId && primaryCategoryLabel && (
              statusPermissions.allowCategoryEdit ? (
                <EbayFakeLink
                  style={{ margin: 0, fontWeight: "500" }}
                  onClick={() => setCategoryModalOpen(true)}
                >
                  <p style={{ margin: 0 }}>
                    {primaryCategoryLabel}
                  </p>
                </EbayFakeLink>
              ) : (
                <p style={{ margin: 0, fontWeight: 500 }}>{primaryCategoryLabel}</p>
              )
            )}


            {/* Đường dẫn cha */}
            {parentCategoryTrail && (
              <p style={{ margin: 0, color: "#555", fontSize: "14px" }}>
                in {parentCategoryTrail}
              </p>
            )}
          </div>

          {/* Bên phải */}
          {statusPermissions.allowCategoryEdit ? (
            <p style={{ margin: 0 }}>
              <EbayButton
                onClick={() => setCategoryModalOpen(true)}
                borderless
                priority="tertiary"
                style={{ cursor: "pointer", display: "flex", alignItems: "center", gap: "4px", border: "none" }}
              >
                <EditIcon style={{ width: "16px", height: "16px", marginRight: "4px" }} />
                <p>Edit</p>
              </EbayButton>
            </p>
          ) : (
            <p style={{ margin: 0, color: "#6f7780", fontSize: "13px" }}>
              Category cannot be changed while the listing is live.
            </p>
          )}
        </div>

        <CategoryModal
          isOpen={categoryModalOpen && statusPermissions.allowCategoryEdit}
          onClose={() => setCategoryModalOpen(false)}
          onSelect={(category) => {
            setSelectedCategoryId(category.id);
            const nextPath = Array.isArray(category.path) ? category.path : [];
            setCategoryPath(nextPath);
            setCategoryDetail(category.detail ?? null);
            const nextConditions = Array.isArray(category.detail?.conditions)
              ? category.detail.conditions
              : [];
            setConditions(nextConditions);
            setSpecificSelections({});
            setCustomSpecificInputs({});
            setCustomSpecificRows([]);
            setOpenSpecificId(null);
            releaseVariationMedia(variationMediaRef.current);
            variationMediaRef.current = null;
            setVariationData(null);
            setCategoryModalOpen(false);
          }}
          initialSelected={categoryPath}
        />

        <hr />

        <div style={{ marginBottom: "48px" }}>
          <h3>ITEM SPECIFICS</h3>
          {!selectedCategoryId && (
            <EbaySectionNotice status="information">
              <EbayNoticeContent>
                Pick a category first to see the required and optional item specifics.
              </EbayNoticeContent>
            </EbaySectionNotice>
          )}
          {selectedCategoryId && (
            <>
              {!hasVariations && (
                <>
                  <h3 style={{ marginBottom: 0, marginTop: 32 }}>Required</h3>
                  <p style={{ margin: 0, color: "#555" }}>Buyers need these details to find your item.</p>
                  {requiredSpecifics.length === 0 ? (
                    <p style={{ marginTop: 8, color: "#777" }}>This category does not publish required specifics.</p>
                  ) : (
                    <div style={{ marginTop: 12 }}>
                      {requiredSpecifics.map((specific) => renderSpecificInput(specific))}
                    </div>
                  )}
                </>
              )}
              <h3 style={{ marginBottom: 0, marginTop: 32 }}>Additional (optional)</h3>
              <p style={{ margin: 0, color: "#555" }}>Buyers also search for these details.</p>
              {additionalSpecifics.length === 0 ? (
                <p style={{ marginTop: 8, color: "#777" }}>This category does not publish additional specifics.</p>
              ) : (
                <div style={{ marginTop: 12 }}>
                  {additionalSpecifics.map((specific) => renderSpecificInput(specific))}
                </div>
              )}
              <h3 style={{ marginBottom: 0, marginTop: 32 }}>Custom item specifics</h3>
              <p style={{ margin: 0, color: "#555" }}>Add your own details when the category does not provide them.</p>
              <div style={{ marginTop: 12 }}>
                {customSpecificRows.length === 0 ? (
                  <p style={{ color: "#777" }}>No custom specifics yet.</p>
                ) : (
                  customSpecificRows.map((row) => renderCustomSpecificRow(row))
                )}
                <button
                  type="button"
                  onClick={handleAddCustomSpecificRow}
                  style={{
                    marginTop: customSpecificRows.length === 0 ? 0 : 12,
                    border: "1px dashed #3665f3",
                    color: "#3665f3",
                    background: "transparent",
                    borderRadius: "999px",
                    padding: "10px 18px",
                    fontWeight: 600,
                    cursor: "pointer"
                  }}
                >
                  + Add your own item specific
                </button>
              </div>
            </>
          )}
        </div>

        <hr />
        <div style={{ display: "none" }}>
          <InfoFilledIcon id="icon-information-filled-16" style={{ width: "16px", height: "16px" }} />
        </div>
        <div
          style={{
            marginBottom: "48px",
            display: "flex",
            justifyContent: "space-between",
            alignItems: "flex-start"
          }}
        >
          {/* Bên trái */}

          <div>
            <h3 style={{ marginBottom: "8px" }}>VARIATIONS</h3>

            {!selectedCategoryId && (
              <EbaySectionNotice status="information">
                <EbayNoticeContent>
                  Choose a category first to configure variations for this listing.
                </EbayNoticeContent>
              </EbaySectionNotice>
            )}

            {selectedCategoryId && priceFormat === "2" && !variationSummary && (
              <p style={{ marginTop: "16px", color: "#555", maxWidth: "420px" }}>
                Save time and money by listing multiple variations of your item in one multi-quantity, fixed price listing.
              </p>
            )}

            {selectedCategoryId && priceFormat === "2" && variationSummary && (
              <div
                style={{
                  marginTop: "20px",
                  maxWidth: "620px",
                  width: "100%",
                  display: "flex",
                  flexDirection: "column",
                  gap: "32px",
                  paddingTop: "20px"
                }}
              >
                <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                  <p style={{ margin: 0, fontWeight: 600, fontSize: "16px", color: "#111820" }}>Attributes</p>
                  {variationSummary.attributeRows.length > 0 ? (
                    <div style={{ display: "flex", flexDirection: "column", gap: "12px" }}>
                      {variationSummary.attributeRows.map((row, rowIndex) => (
                        <div
                          key={`attribute-row-${rowIndex}`}
                          style={{
                            display: "grid",
                            gridTemplateColumns: "repeat(2, minmax(0, 1fr))",
                            columnGap: "64px",
                            rowGap: "12px"
                          }}
                        >
                          {row.map((attribute) => (
                            <div
                              key={attribute.name}
                              style={{ display: "flex", flexDirection: "column", gap: "4px" }}
                            >
                              <span style={{ fontSize: "14px", color: "#6c707a" }}>{attribute.name}</span>
                              <span style={{ fontSize: "14px", color: "#111820" }}>
                                {attribute.values.map((value, index) => (
                                  <span
                                    key={`${attribute.name}-${value}`}
                                    style={{
                                      textDecoration: "underline",
                                      cursor: "pointer"
                                    }}
                                  >
                                    {value}
                                    {index < attribute.values.length - 1 ? ", " : ""}
                                  </span>
                                ))}
                              </span>
                            </div>
                          ))}
                          {row.length === 1 && <div />}
                        </div>
                      ))}
                    </div>
                  ) : (
                    <p style={{ margin: 0, color: "#6c707a", fontSize: "14px" }}>No attributes selected yet.</p>
                  )}
                </div>

                <div style={{ display: "flex", flexDirection: "column", gap: "16px" }}>
                  <p style={{ margin: 0, fontWeight: 600, fontSize: "16px", color: "#111820" }}>Pricing</p>
                  <div
                    style={{
                      display: "grid",
                      gridTemplateColumns: "repeat(2, minmax(0, 1fr))",
                      columnGap: "64px",
                      rowGap: "12px"
                    }}
                  >
                    <div style={{ display: "flex", flexDirection: "column", gap: "4px" }}>
                      <span style={{ fontSize: "14px", color: "#6c707a" }}>Price</span>
                      <span
                        style={{
                          fontSize: "14px",
                          color: variationSummary.priceRangeLabel ? "#111820" : "#6c707a",
                          textDecoration: variationSummary.priceRangeLabel ? "underline" : "none"
                        }}
                      >
                        {variationSummary.priceRangeLabel ?? "—"}
                      </span>
                    </div>

                    <div style={{ display: "flex", flexDirection: "column", gap: "4px" }}>
                      <span style={{ fontSize: "14px", color: "#6c707a" }}>Quantity</span>
                      <span
                        style={{
                          fontSize: "14px",
                          color: variationSummary.quantityLabel ? "#111820" : "#6c707a",
                          textDecoration: variationSummary.quantityLabel ? "underline" : "none"
                        }}
                      >
                        {variationSummary.quantityLabel ?? "—"}
                      </span>
                    </div>
                  </div>
                </div>
              </div>
            )}

            {selectedCategoryId && priceFormat === "2" && !variationSummary && (
              <p style={{ margin: "12px 0 0", color: "#555", fontSize: "14px" }}>
                No variations configured yet. Use Edit to set them up.
              </p>
            )}

            {selectedCategoryId && priceFormat !== "2" && (
              <>
                <EbaySectionNotice status="information">
                  <EbayNoticeContent>
                    Variations are unavailable for this listing. Eligibility is limited to qualifying item categories, Buy It Now pricing formats, and other criteria.
                  </EbayNoticeContent>
                  <EbaySectionNoticeCTA>
                    <EbayFakeLink onClick={() => { }}>
                      Learn more
                    </EbayFakeLink>
                  </EbaySectionNoticeCTA>
                </EbaySectionNotice>


              </>
            )}
          </div>

          {/* Bên phải */}
          {selectedCategoryId && priceFormat === "2" && (
            <div
              style={{
                display: "flex",
                flexDirection: "column",
                alignItems: "flex-end",
                gap: "12px"
              }}
            >
              <EbayButton
                borderless
                priority="tertiary"
                style={{ cursor: "pointer", display: "flex", alignItems: "center", gap: "4px" }}
                onClick={() =>
                  navigate("/variation-form", {
                    state: {
                      fromListing: true,
                      variationData,
                      categoryDetail,
                      selectedSpecifics: specificSelections,
                      listingDraft: buildListingDraft()
                    }
                  })
                }
              >
                <EditIcon style={{ width: "16px", height: "16px", marginRight: "4px" }} />
                <p>Edit</p>
              </EbayButton>

              {variationData && (
                <EbayButton
                  borderless
                  priority="tertiary"
                  style={{ cursor: "pointer" }}
                  onClick={handleClearVariations}
                >
                  Clear variations
                </EbayButton>
              )}
            </div>
          )}
        </div>

        <hr />

        <div style={{ marginBottom: "48px" }}>
          <h3>CONDITION</h3>
          {!selectedConditionId && (
            <EbaySectionNotice status="information">
              <EbayNoticeContent>
                Provide a condition for your item
              </EbayNoticeContent>
            </EbaySectionNotice>
          )}

          <h3 style={{ margin: 0, fontWeight: "normal" }}>Item condition</h3>
          {!selectedConditionId && conditions.length > 0 && (
            <MenuIcon
              onClick={() => setConditionModalOpen(true)}
              style={{ cursor: "pointer", width: "16px", height: "16px" }} />
          )}

          {selectedConditionId && (
            <EbayFakeLink
            >
              <h3
                onClick={() => setConditionModalOpen(true)}
                style={{ margin: 0, fontWeight: "normal" }}>
                {conditions.find(c => c.id === selectedConditionId)?.name}
              </h3>
            </EbayFakeLink>
          )}

          <ConditionModal
            isOpen={conditionModalOpen}
            onClose={() => setConditionModalOpen(false)}
            conditions={conditions}
            selectedConditionId={selectedConditionId}
            setSelectedConditionId={setSelectedConditionId}
          />

          {conditions.length === 0 && (
            <p style={{ color: "#777" }}>This category does not publish condition guidelines.</p>
          )}

          <EbayField layout="block">
            <EbayLabel stacked><h4 style={{ margin: 0, fontWeight: "normal" }}>Condition description</h4></EbayLabel>
            <EbayTextbox
              fluid
              multiline
              name="itemCondition"
              value={conditionDescription}
              onInputChange={(event, { value }) => setConditionDescription(value)}
              style={{ minHeight: "50px" }}
            />
          </EbayField>
        </div>

        <hr />

        <div style={{ marginBottom: "48px" }}>
          <h3>DESCRIPTION</h3>

          <EbayTextbox
            multiline
            name="itemDescription"
            fluid
            value={listingDescription}
            onInputChange={(event, { value }) => setListingDescription(value)}
            placeholder="Write a detailed description of your item"
          />
        </div>

        <hr />

        <div style={{ marginBottom: "48px" }}>
          <h3>PRICING</h3>
          <div className="date__select_listing">
            <EbayField layout="block">
              <EbayLabel stacked>Format</EbayLabel>
              {statusPermissions.allowFormatSelection ? (
                <EbaySelect
                  name="itemFormat"
                  value={priceFormat}
                  disabled={hasVariations}
                  onChange={(event) => {
                    if (hasVariations) {
                      return;
                    }
                    setPriceFormat(event.target.value);
                  }}
                  style={{ width: "400px" }}
                >
                  {priceFormats.map((option) => (
                    <EbaySelectOption key={option.value} value={option.value}>
                      {option.text}
                    </EbaySelectOption>
                  ))}
                </EbaySelect>
              ) : (
                <div style={{
                  border: "1px solid #c7c7c7",
                  borderRadius: "12px",
                  padding: "12px 16px",
                  minHeight: "48px",
                  display: "flex",
                  alignItems: "center",
                  width: "400px",
                  backgroundColor: "#f5f6f8",
                  color: "#6f7780",
                  fontSize: "14px"
                }}>
                  {priceFormats.find((option) => option.value === priceFormat)?.text ?? "Unknown format"}
                </div>
              )}
            </EbayField>
          </div>

          {priceFormat === "1" && (
            <div className="date__select_listing">
              <EbayField layout="block">
                <EbayLabel stacked>Auction duration</EbayLabel>
                <EbaySelect
                  name="itemAuctionDuration"
                  value={auctionDuration}
                  onChange={(event) => setAuctionDuration(event.target.value)}
                  style={{ width: "400px" }}
                >
                  {auctionDurations.map((option) => (
                    <EbaySelectOption key={option.value} value={option.value}>
                      {option.text}
                    </EbaySelectOption>
                  ))}
                </EbaySelect>
              </EbayField>

              <div style={{ display: "flex", gap: "32px", alignItems: "flex-start" }}>
                {/* Starting bid */}
                <div>
                  <EbayLabel stacked>Starting bid</EbayLabel>
                  <EbayTextbox
                    name="itemStartingBid"
                    value={startingBid}
                    onInputChange={(event, { value }) => {
                      if (/^(?:\d+)?(?:\.\d{0,2})?$/.test(value) || value === "") {
                        setStartingBid(value);
                      }
                    }}
                    style={{ width: "150px" }}
                  >
                    <EbayTextboxPrefixText id="prefix">$</EbayTextboxPrefixText>
                  </EbayTextbox>
                  <EbayFieldDescription>&nbsp;</EbayFieldDescription>
                </div>

                {/* Buy It Now (optional) */}
                <div>
                  <EbayLabel stacked>Buy It Now (optional)</EbayLabel>
                  <EbayTextbox
                    name="itemBuyItNow"
                    value={buyItNowPrice}
                    onInputChange={(event, { value }) => {
                      if (/^(?:\d+)?(?:\.\d{0,2})?$/.test(value) || value === "") {
                        setBuyItNowPrice(value);
                      }
                    }}
                    style={{ width: "150px" }}
                  >
                    <EbayTextboxPrefixText id="prefix">$</EbayTextboxPrefixText>
                  </EbayTextbox>
                  <EbayFieldDescription style={{ width: "150px" }}>
                    Minimum: 30% more than starting bid
                  </EbayFieldDescription>
                </div>
              </div>

              <EbayField layout="block">
                <EbayLabel stacked>Reserve price (optional)</EbayLabel>
                <EbayTextbox
                  name="itemReservePrice"
                  value={reservePrice}
                  onInputChange={(event, { value }) => {
                    if (/^(?:\d+)?(?:\.\d{0,2})?$/.test(value) || value === "") {
                      setReservePrice(value);
                    }
                  }}
                  style={{ width: "150px" }}
                >
                  <EbayTextboxPrefixText id="prefix">$</EbayTextboxPrefixText>
                </EbayTextbox>
              </EbayField>
            </div>
          )}

          {/* Nếu Buy It Now */}
          {priceFormat === "2" && !hasVariations && (
            <EbayField layout="block">
              <EbayLabel stacked>Item price</EbayLabel>
              <EbayTextbox
                name="itemPrice"
                value={fixedPrice}
                onInputChange={(event, { value }) => {
                  if (/^(?:\d+)?(?:\.\d{0,2})?$/.test(value) || value === "") {
                    setFixedPrice(value);
                  }
                }}
                style={{ width: "150px" }}
              >
                <EbayTextboxPrefixText id="prefix">$</EbayTextboxPrefixText>
              </EbayTextbox>
            </EbayField>
          )}

          {!hasVariations && (
            <EbayField layout="block">
              <EbayLabel stacked>Quantity</EbayLabel>
              <EbayTextbox
                name="itemQuantity"
                value={quantity}
                onInputChange={(e, { value }) => {
                  if (/^(?:[1-9]\d*|)$/.test(value)) {
                    setQuantity(value);
                  }
                }}
                style={{ width: "184px" }}
              />
            </EbayField>
          )}



          <div
            style={{
              border: "1px solid #ddd",
              borderRadius: "8px",
              padding: "16px",
              marginBottom: "20px"
            }}
          >
            <div
              style={{
                display: "flex",
                justifyContent: "space-between",
                marginBottom: "8px",
              }}
            >
              <div>
                <h3>Allow offers (optional)</h3>
                <p style={{ color: "#666" }}>
                  Interested buyers can send an offer for this item. You can accept, counter, or decline.
                </p>
              </div>
              <EbaySwitch checked={allowOfferEnabled} onChange={toggleAllowOfferEnabled} />
            </div>

            {allowOfferEnabled && (
              <div style={{ display: "flex", alignItems: "flex-end", gap: "12px" }}>
                <EbayField layout="block">
                  <EbayLabel stacked>Minimum offer</EbayLabel>
                  <EbayTextbox
                    name="itemMinimumOffer"
                    value={minimumOffer}
                    onInputChange={(event, { value }) => {
                      if (/^(?:\d+)?(?:\.\d{0,2})?$/.test(value) || value === "") {
                        setMinimumOffer(value);
                      }
                    }}
                    style={{ width: "150px" }}
                  >
                    <EbayTextboxPrefixText id="prefix">$</EbayTextboxPrefixText>
                  </EbayTextbox>
                </EbayField>

                <EbayField layout="block">
                  <EbayLabel stacked>Auto accept</EbayLabel>
                  <EbayTextbox
                    name="itemAutoAcceptOffer"
                    value={autoAcceptOffer}
                    onInputChange={(event, { value }) => {
                      if (/^(?:\d+)?(?:\.\d{0,2})?$/.test(value) || value === "") {
                        setAutoAcceptOffer(value);
                      }
                    }}
                    style={{ width: "150px" }}
                  >
                    <EbayTextboxPrefixText id="prefix">$</EbayTextboxPrefixText>
                  </EbayTextbox>
                </EbayField>


              </div>


            )}
          </div>

          {statusPermissions.showSchedule ? (
            <div
              style={{
                marginBottom: "40px",
                border: "1px solid #ddd",
                borderRadius: "8px",
                padding: "16px"
              }}
            >
              <div
                style={{
                  display: "flex",
                  justifyContent: "space-between",
                  marginBottom: "8px",
                }}
              >
                <div>
                  <h3>Schedule your listing</h3>
                  <p style={{ color: "#666" }}>
                    Your listing goes live immediately, unless you select a time and date you want it to start.
                  </p>
                </div>
                <EbaySwitch checked={scheduleEnabled} onChange={toggleScheduleEnabled} />
              </div>

              <div style={{ display: "none" }}><CalendarIcon id="icon-calendar-24" style={{ width: 24, height: 24 }} /></div>
              {scheduleEnabled && (
                <div className="date__select_listing" style={{ display: "flex", alignItems: "flex-end", gap: "12px" }}>
                  <EbayField layout="block" style={{ marginBottom: "4px" }}>
                    <EbayLabel stacked>Day</EbayLabel>
                    <input
                      type="date"
                      value={date}
                      onChange={(event) => setDate(event.target.value)}
                      style={{
                        width: "180px",
                        height: "40px",
                        borderRadius: "10px",
                        border: "1px solid #c7c7c7",
                        padding: "0 12px",
                        fontSize: "14px"
                      }}
                    />
                  </EbayField>

                  {/* Time */}
                  <EbayField layout="block">
                    <EbayLabel stacked>Time</EbayLabel>
                    <div style={{ display: "flex", gap: "8px", alignItems: "center" }}>
                      <EbaySelect
                        value={hour}
                        onChange={(event) => setHour(event.target.value)}
                        className="listing-select listing-select--no-icon"
                        style={{ width: "70px", paddingInlineEnd: "0" }}
                      >
                        {Array.from({ length: 12 }, (_, i) => (
                          <EbaySelectOption key={i} value={(i + 1).toString()}>
                            {i + 1}
                          </EbaySelectOption>
                        ))}
                      </EbaySelect>

                      <span>:</span>

                      <EbaySelect
                        value={minute}
                        onChange={(event) => setMinute(event.target.value)}
                        className="listing-select listing-select--no-icon"
                        style={{ width: "70px", paddingInlineEnd: "0" }}
                      >
                        {["00", "15", "30", "45"].map((m) => (
                          <EbaySelectOption key={m} value={m}>
                            {m}
                          </EbaySelectOption>
                        ))}
                      </EbaySelect>

                      <EbaySelect
                        value={ampm}
                        onChange={(event) => setAmpm(event.target.value)}
                        className="listing-select listing-select--no-icon"
                      >
                        {["AM", "PM"].map((a) => (
                          <EbaySelectOption key={a} value={a}>
                            {a}
                          </EbaySelectOption>
                        ))}
                      </EbaySelect>

                      {/* PDT cùng hàng */}
                      <span style={{ paddingLeft: "4px" }}>PDT</span>
                    </div>
                  </EbayField>
                </div>


              )}
            </div>
          ) : (
            isEditMode && (
              <div
                style={{
                  marginBottom: "40px",
                  border: "1px solid #f0f2f5",
                  borderRadius: "8px",
                  padding: "16px",
                  backgroundColor: "#f8fafc",
                  color: "#6f7780"
                }}
              >
                Scheduling is not available while the listing is {listingStatusLabel.toLowerCase()}.
              </div>
            )
          )}


          <hr />

          <div style={{ textAlign: "center", marginTop: "40px" }}>
            <h2>{isTemplateMode ? "Save your template." : "List it for free."}</h2>
            <p><EbayButton
              priority="primary"
              style={{ width: "343px", height: "45px" }}
              disabled={isTemplateMode ? isTemplateSaving : isSubmitting}
              onClick={() => handleSubmit(false)}
            >
              <div style={{ fontSize: "16px" }}>
                {isTemplateMode
                  ? templateContext.mode === "edit"
                    ? "Update template"
                    : "Save template"
                  : "List it"}
              </div>
            </EbayButton></p>

            {!isTemplateMode && (
              statusPermissions.allowSaveDraft ? (
                <p><EbayButton
                  priority="tertiary"
                  style={{ width: "343px", height: "45px" }}
                  disabled={isSubmitting}
                  onClick={() => handleSubmit(true)}
                >
                  <div style={{ fontSize: "16px" }}>Save for later</div>
                </EbayButton></p>
              ) : (
                <p style={{ fontSize: "13px", color: "#6f7780" }}>
                  Active listings can't be saved as drafts. Publish your changes to keep them live.
                </p>
              )
            )}
          </div>
        </div>
      </div >
    </div>
  );
};

export default ListingForm;
