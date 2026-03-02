import axios from "../../utils/axiosCustomize";

const resource = "selling-preferences";

const toBoolean = (value, fallback = false) => {
  if (typeof value === "boolean") {
    return value;
  }

  if (value === 1) {
    return true;
  }

  if (value === 0) {
    return false;
  }

  return fallback;
};

const toInteger = (value, fallback = undefined) => {
  if (typeof value === "number" && Number.isFinite(value)) {
    return value;
  }

  if (typeof value === "string" && value.trim().length > 0) {
    const parsed = Number.parseInt(value.trim(), 10);
    if (!Number.isNaN(parsed)) {
      return parsed;
    }
  }

  return fallback;
};

const normalizeList = (items) => {
  if (!Array.isArray(items)) {
    return [];
  }

  return items
    .map((item) => (typeof item === "string" ? item.trim() : ""))
    .filter((item) => item.length > 0);
};

const normalizeInvoice = (raw = {}) => {
  const format = toInteger(raw.format ?? raw.Format, 1);
  const sendEmailCopy = toBoolean(raw.sendEmailCopy ?? raw.SendEmailCopy, true);
  const applyCreditsAutomatically = toBoolean(raw.applyCreditsAutomatically ?? raw.ApplyCreditsAutomatically, true);
  const formatName = raw.formatName ?? raw.FormatName ?? (typeof raw.format === "string" ? raw.format : undefined);

  return {
    format,
    formatName: formatName || (format === 0 ? "Summary" : "Detailed"),
    sendEmailCopy,
    applyCreditsAutomatically
  };
};

const normalizeBuyerManagement = (raw = {}) => {
  const blockSettings = raw.blockSettings ?? raw.BlockSettings ?? {};
  const rules = raw.rules ?? raw.Rules ?? {};

  return {
    blockSettings: {
      blockUnpaidItemStrikes: toBoolean(blockSettings.blockUnpaidItemStrikes ?? blockSettings.BlockUnpaidItemStrikes),
      unpaidItemStrikesCount: toInteger(blockSettings.unpaidItemStrikesCount ?? blockSettings.UnpaidItemStrikesCount, 2),
      unpaidItemStrikesPeriodInMonths: toInteger(
        blockSettings.unpaidItemStrikesPeriodInMonths ?? blockSettings.UnpaidItemStrikesPeriodInMonths,
        1
      ),
      blockPrimaryAddressOutsideShippingLocation: toBoolean(
        blockSettings.blockPrimaryAddressOutsideShippingLocation
          ?? blockSettings.BlockPrimaryAddressOutsideShippingLocation,
        true
      ),
      blockMaxItemsInLastTenDays: toBoolean(
        blockSettings.blockMaxItemsInLastTenDays ?? blockSettings.BlockMaxItemsInLastTenDays
      ),
      maxItemsInLastTenDays: toInteger(blockSettings.maxItemsInLastTenDays ?? blockSettings.MaxItemsInLastTenDays),
      applyFeedbackScoreThreshold: toBoolean(
        blockSettings.applyFeedbackScoreThreshold ?? blockSettings.ApplyFeedbackScoreThreshold
      ),
      feedbackScoreThreshold: toInteger(
        blockSettings.feedbackScoreThreshold ?? blockSettings.FeedbackScoreThreshold
      ),
      updateBlockSettingsForActiveListings: toBoolean(
        blockSettings.updateBlockSettingsForActiveListings ?? blockSettings.UpdateBlockSettingsForActiveListings
      )
    },
    rules: {
      requirePaymentMethodBeforeBid: toBoolean(rules.requirePaymentMethodBeforeBid ?? rules.RequirePaymentMethodBeforeBid, true),
      requirePaymentMethodBeforeOffer: toBoolean(
        rules.requirePaymentMethodBeforeOffer ?? rules.RequirePaymentMethodBeforeOffer,
        true
      ),
      preventBlockedBuyersFromContacting: toBoolean(
        rules.preventBlockedBuyersFromContacting ?? rules.PreventBlockedBuyersFromContacting,
        true
      )
    }
  };
};

const normalizeSellerPreference = (raw = {}) => {
  const multiQuantity = raw.multiQuantity ?? raw.MultiQuantity ?? {};

  return {
    multiQuantity: {
      listingsStayActiveWhenOutOfStock: toBoolean(
        multiQuantity.listingsStayActiveWhenOutOfStock ?? multiQuantity.ListingsStayActiveWhenOutOfStock
      ),
      showExactQuantityAvailable: toBoolean(
        multiQuantity.showExactQuantityAvailable ?? multiQuantity.ShowExactQuantityAvailable,
        true
      )
    },
    invoice: normalizeInvoice(raw.invoice ?? raw.Invoice ?? {}),
    buyerManagement: normalizeBuyerManagement(raw.buyerManagement ?? raw.BuyerManagement ?? {}),
    blockedBuyerCount: toInteger(raw.blockedBuyerCount ?? raw.BlockedBuyerCount, 0),
    exemptBuyerCount: toInteger(raw.exemptBuyerCount ?? raw.ExemptBuyerCount, 0),
    buyersCanSeeVatNumber: toBoolean(raw.buyersCanSeeVatNumber ?? raw.BuyersCanSeeVatNumber),
    vatNumber: typeof raw.vatNumber === "string" ? raw.vatNumber : raw.VatNumber ?? ""
  };
};

const normalizeBuyerList = (raw = {}) => {
  const items = normalizeList(raw.items ?? raw.Items);
  const lastUpdatedAtUtc = raw.lastUpdatedAtUtc ?? raw.LastUpdatedAtUtc ?? null;

  return {
    items,
    lastUpdatedAtUtc
  };
};

const sanitizeOverviewPayload = (payload = {}) => {
  const body = {};

  if (typeof payload.listingsStayActiveWhenOutOfStock === "boolean") {
    body.listingsStayActiveWhenOutOfStock = payload.listingsStayActiveWhenOutOfStock;
  }

  if (typeof payload.showExactQuantityAvailable === "boolean") {
    body.showExactQuantityAvailable = payload.showExactQuantityAvailable;
  }

  if (payload.invoice && typeof payload.invoice === "object") {
    const invoice = {};
    const format = toInteger(payload.invoice.format ?? payload.invoice.Format);
    if (typeof format === "number") {
      invoice.format = format;
    }

    if (typeof payload.invoice.sendEmailCopy === "boolean") {
      invoice.sendEmailCopy = payload.invoice.sendEmailCopy;
    }

    if (typeof payload.invoice.applyCreditsAutomatically === "boolean") {
      invoice.applyCreditsAutomatically = payload.invoice.applyCreditsAutomatically;
    }

    if (Object.keys(invoice).length > 0) {
      body.invoice = invoice;
    }
  }

  if (typeof payload.buyersCanSeeVatNumber === "boolean") {
    body.buyersCanSeeVatNumber = payload.buyersCanSeeVatNumber;
  }

  if (typeof payload.vatNumber === "string") {
    body.vatNumber = payload.vatNumber.trim();
  }

  return body;
};

const sanitizeBuyerManagementPayload = (payload = {}) => {
  const body = {
    blockUnpaidItemStrikes: toBoolean(payload.blockUnpaidItemStrikes, false),
    unpaidItemStrikesCount: toInteger(payload.unpaidItemStrikesCount, 2),
    unpaidItemStrikesPeriodInMonths: toInteger(payload.unpaidItemStrikesPeriodInMonths, 1),
    blockPrimaryAddressOutsideShippingLocation: toBoolean(
      payload.blockPrimaryAddressOutsideShippingLocation,
      true
    ),
    blockMaxItemsInLastTenDays: toBoolean(payload.blockMaxItemsInLastTenDays, false),
    maxItemsInLastTenDays: toInteger(payload.maxItemsInLastTenDays),
    applyFeedbackScoreThreshold: toBoolean(payload.applyFeedbackScoreThreshold, false),
    feedbackScoreThreshold: toInteger(payload.feedbackScoreThreshold),
    updateBlockSettingsForActiveListings: toBoolean(payload.updateBlockSettingsForActiveListings, false),
    requirePaymentMethodBeforeBid: toBoolean(payload.requirePaymentMethodBeforeBid, true),
    requirePaymentMethodBeforeOffer: toBoolean(payload.requirePaymentMethodBeforeOffer, true),
    preventBlockedBuyersFromContacting: toBoolean(payload.preventBlockedBuyersFromContacting, true)
  };

  if (!body.blockMaxItemsInLastTenDays) {
    delete body.maxItemsInLastTenDays;
  }

  if (!body.applyFeedbackScoreThreshold) {
    delete body.feedbackScoreThreshold;
  }

  return body;
};

const sanitizeBuyerListPayload = (items) => ({
  items: normalizeList(items)
});

const getOverview = (signal) => {
  const options = {};
  if (signal) {
    options.signal = signal;
  }

  return axios.get(resource, options).then((response) => normalizeSellerPreference(response?.data ?? {}));
};

const updateOverview = (payload = {}) =>
  axios.put(resource, sanitizeOverviewPayload(payload)).then((response) => normalizeSellerPreference(response?.data ?? {}));

const getBuyerManagement = (signal) => {
  const options = {};
  if (signal) {
    options.signal = signal;
  }

  return axios
    .get(`${resource}/buyer-management`, options)
    .then((response) => normalizeBuyerManagement(response?.data ?? {}));
};

const updateBuyerManagement = (payload = {}) =>
  axios
    .put(`${resource}/buyer-management`, sanitizeBuyerManagementPayload(payload))
    .then((response) => normalizeBuyerManagement(response?.data ?? {}));

const getBlockedBuyers = (signal) => {
  const options = {};
  if (signal) {
    options.signal = signal;
  }

  return axios
    .get(`${resource}/blocked-buyers`, options)
    .then((response) => normalizeBuyerList(response?.data ?? {}));
};

const updateBlockedBuyers = (items = []) =>
  axios
    .put(`${resource}/blocked-buyers`, sanitizeBuyerListPayload(items))
    .then((response) => normalizeBuyerList(response?.data ?? {}));

const getExemptBuyers = (signal) => {
  const options = {};
  if (signal) {
    options.signal = signal;
  }

  return axios
    .get(`${resource}/exempt-buyers`, options)
    .then((response) => normalizeBuyerList(response?.data ?? {}));
};

const updateExemptBuyers = (items = []) =>
  axios
    .put(`${resource}/exempt-buyers`, sanitizeBuyerListPayload(items))
    .then((response) => normalizeBuyerList(response?.data ?? {}));

const SellingPreferenceService = {
  getOverview,
  updateOverview,
  getBuyerManagement,
  updateBuyerManagement,
  getBlockedBuyers,
  updateBlockedBuyers,
  getExemptBuyers,
  updateExemptBuyers,
  normalizeSellerPreference,
  normalizeBuyerManagement,
  normalizeBuyerList
};

export default SellingPreferenceService;
