import axios from "../../utils/axiosCustomize";

const resource = "reports";
const DEFAULT_PAGE_NUMBER = 1;
const DEFAULT_PAGE_SIZE = 25;

const toInteger = (value) => {
  if (value === undefined || value === null || value === "") {
    return undefined;
  }

  const parsed = Number.parseInt(value, 10);
  return Number.isNaN(parsed) ? undefined : parsed;
};

const toPositiveInt = (value) => {
  const parsed = toInteger(value);
  return typeof parsed === "number" && parsed > 0 ? parsed : undefined;
};

const toBoolean = (value) => {
  if (typeof value === "boolean") {
    return value;
  }

  if (typeof value === "string") {
    const normalized = value.trim().toLowerCase();
    if (normalized === "true") {
      return true;
    }

    if (normalized === "false") {
      return false;
    }
  }

  return undefined;
};

const trimString = (value) => {
  if (typeof value !== "string") {
    return undefined;
  }

  const trimmed = value.trim();
  return trimmed.length > 0 ? trimmed : undefined;
};

const normalizePaging = (payload = {}, fallback = {}) => {
  const rawItems = Array.isArray(payload.items)
    ? payload.items
    : Array.isArray(payload.Items)
      ? payload.Items
      : [];

  const rawTotalCount = payload.totalCount ?? payload.TotalCount;
  const totalCount = typeof rawTotalCount === "number" && !Number.isNaN(rawTotalCount)
    ? rawTotalCount
    : toInteger(rawTotalCount) ?? fallback.totalCount ?? 0;

  const rawPageNumber = payload.pageNumber ?? payload.PageNumber;
  const pageNumber = toPositiveInt(rawPageNumber)
    ?? toPositiveInt(fallback.pageNumber)
    ?? DEFAULT_PAGE_NUMBER;

  const rawPageSize = payload.pageSize ?? payload.PageSize;
  const pageSize = toPositiveInt(rawPageSize)
    ?? toPositiveInt(fallback.pageSize)
    ?? DEFAULT_PAGE_SIZE;

  return {
    items: rawItems,
    totalCount,
    pageNumber,
    pageSize
  };
};

const sanitizeQuery = (query = {}) => {
  const sanitized = {};

  const pageNumber = toPositiveInt(query.pageNumber ?? query.PageNumber);
  if (pageNumber) {
    sanitized.pageNumber = pageNumber;
  }

  const pageSize = toPositiveInt(query.pageSize ?? query.PageSize);
  if (pageSize) {
    sanitized.pageSize = pageSize;
  }

  const source = trimString(query.source ?? query.Source);
  if (source) {
    sanitized.source = source;
  }

  const status = query.status ?? query.Status;
  if (typeof status === "number" && Number.isFinite(status)) {
    sanitized.status = status;
  } else if (typeof status === "string") {
    const trimmedStatus = status.trim();
    if (trimmedStatus.length > 0) {
      sanitized.status = trimmedStatus;
    }
  }

  const fromUtc = query.fromUtc ?? query.FromUtc;
  if (fromUtc) {
    sanitized.fromUtc = fromUtc;
  }

  const toUtc = query.toUtc ?? query.ToUtc;
  if (toUtc) {
    sanitized.toUtc = toUtc;
  }

  const onlyActive = toBoolean(query.onlyActive ?? query.OnlyActive);
  if (typeof onlyActive === "boolean") {
    sanitized.onlyActive = onlyActive;
  }

  const type = trimString(query.type ?? query.Type);
  if (type) {
    sanitized.type = type;
  }

  const frequency = toInteger(query.frequency ?? query.Frequency);
  if (typeof frequency === "number") {
    sanitized.frequency = frequency;
  }

  return sanitized;
};

const sanitizeDownloadPayload = (payload = {}) => {
  const source = trimString(payload.source ?? payload.Source);
  const type = trimString(payload.type ?? payload.Type);
  const rangeStartUtc = payload.rangeStartUtc ?? payload.RangeStartUtc;
  const rangeEndUtc = payload.rangeEndUtc ?? payload.RangeEndUtc;
  const timeZone = trimString(payload.timeZone ?? payload.TimeZone);

  const body = {};

  if (source) {
    body.source = source;
  }

  if (type) {
    body.type = type;
  }

  if (rangeStartUtc) {
    body.rangeStartUtc = rangeStartUtc;
  }

  if (rangeEndUtc) {
    body.rangeEndUtc = rangeEndUtc;
  }

  if (timeZone) {
    body.timeZone = timeZone;
  }

  return body;
};

const sanitizeSchedulePayload = (payload = {}) => {
  const source = trimString(payload.source ?? payload.Source);
  const type = trimString(payload.type ?? payload.Type);
  const frequency = toInteger(payload.frequency ?? payload.Frequency);
  const endDateUtc = payload.endDateUtc ?? payload.EndDateUtc;
  const deliveryEmail = trimString(payload.deliveryEmail ?? payload.DeliveryEmail);

  const body = {};

  if (source) {
    body.source = source;
  }

  if (type) {
    body.type = type;
  }

  if (typeof frequency === "number") {
    body.frequency = frequency;
  }

  if (endDateUtc) {
    body.endDateUtc = endDateUtc;
  }

  if (deliveryEmail) {
    body.deliveryEmail = deliveryEmail;
  }

  return body;
};

const getReportDownloads = (query = {}, signal) => {
  const params = sanitizeQuery(query);
  const fallback = {
    pageNumber: params.pageNumber ?? DEFAULT_PAGE_NUMBER,
    pageSize: params.pageSize ?? DEFAULT_PAGE_SIZE
  };

  const options = { params };
  if (signal) {
    options.signal = signal;
  }

  return axios
    .get(`${resource}/downloads`, options)
    .then((response) => normalizePaging(response?.data ?? {}, fallback));
};

const getReportSchedules = (query = {}, signal) => {
  const params = sanitizeQuery(query);
  const fallback = {
    pageNumber: params.pageNumber ?? DEFAULT_PAGE_NUMBER,
    pageSize: params.pageSize ?? DEFAULT_PAGE_SIZE
  };

  const options = { params };
  if (signal) {
    options.signal = signal;
  }

  return axios
    .get(`${resource}/schedules`, options)
    .then((response) => normalizePaging(response?.data ?? {}, fallback));
};

const createReportDownload = (payload = {}) => {
  const body = sanitizeDownloadPayload(payload);

  if (!body.source || !body.type) {
    return Promise.reject(new Error("Source and type are required."));
  }

  return axios.post(`${resource}/downloads`, body).then((response) => response?.data ?? null);
};

const createReportSchedule = (payload = {}) => {
  const body = sanitizeSchedulePayload(payload);

  if (!body.source || !body.type || typeof body.frequency !== "number") {
    return Promise.reject(new Error("Source, type, and frequency are required."));
  }

  return axios.post(`${resource}/schedules`, body).then((response) => response?.data ?? null);
};

const ReportService = {
  getReportDownloads,
  createReportDownload,
  getReportSchedules,
  createReportSchedule
};

export default ReportService;
