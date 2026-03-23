import axios from "../../utils/axiosCustomize";

const resource = "sale-events";

export const SaleEventMode = {
  DiscountAndSaleEvent: 1,
  SaleEventOnly: 2
};

export const SaleEventDiscountType = {
  Percent: 1,
  Amount: 2
};

export const SaleEventStatusLabel = {
  1: "Draft",
  2: "Scheduled",
  3: "Active",
  4: "Ended",
  5: "Cancelled",
  Draft: "Draft",
  Scheduled: "Scheduled",
  Active: "Active",
  Ended: "Ended",
  Cancelled: "Cancelled",
  draft: "Draft",
  scheduled: "Scheduled",
  active: "Active",
  ended: "Ended",
  cancelled: "Cancelled"
};

export const SaleEventStatus = {
  Draft: 1,
  Scheduled: 2,
  Active: 3,
  Ended: 4,
  Cancelled: 5
};

const normalizePaging = (data, fallback = {}) => ({
  items: data?.items ?? data?.Items ?? [],
  totalCount: data?.totalCount ?? data?.TotalCount ?? 0,
  pageNumber: data?.pageNumber ?? data?.PageNumber ?? fallback.pageNumber ?? 1,
  pageSize: data?.pageSize ?? data?.PageSize ?? fallback.pageSize ?? 25
});

const getSaleEvents = async () => {
  const response = await axios.get(resource, { suppressErrorNotice: true });
  return response?.data ?? [];
};

const getSaleEventById = async (saleEventId) => {
  if (!saleEventId) {
    return null;
  }
  const response = await axios.get(`${resource}/${saleEventId}`, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const createSaleEvent = async (payload) => {
  const response = await axios.post(resource, payload, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const updateSaleEvent = async (saleEventId, payload) => {
  const response = await axios.put(`${resource}/${saleEventId}`, payload, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const updateSaleEventStatus = async (saleEventId, status) => {
  const response = await axios.patch(`${resource}/${saleEventId}/status`, { status }, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const deleteSaleEvent = async (saleEventId) => {
  await axios.delete(`${resource}/${saleEventId}`, { suppressErrorNotice: true });
};

const getEligibleListings = async (query = {}) => {
  const fallback = {
    pageNumber: query?.pageNumber ?? query?.PageNumber ?? 1,
    pageSize: query?.pageSize ?? query?.PageSize ?? 25
  };

  const params = Object.fromEntries(
    Object.entries({
      searchTerm: query?.searchTerm ?? query?.SearchTerm,
      categoryId: query?.categoryId ?? query?.CategoryId,
      minPrice: query?.minPrice ?? query?.MinPrice,
      maxPrice: query?.maxPrice ?? query?.MaxPrice,
      minDaysOnSite: query?.minDaysOnSite ?? query?.MinDaysOnSite,
      excludeAlreadyAssigned: query?.excludeAlreadyAssigned ?? query?.ExcludeAlreadyAssigned ?? true,
      pageNumber: fallback.pageNumber,
      pageSize: fallback.pageSize
    }).filter(([, value]) => value !== undefined && value !== null && value !== "")
  );

  const response = await axios.get(`${resource}/eligible-listings`, {
    params,
    suppressErrorNotice: true
  });

  return normalizePaging(response?.data, fallback);
};

const activateSaleEvent = async (saleEventId) => {
  const response = await axios.post(`${resource}/${saleEventId}/activate`, {}, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const deactivateSaleEvent = async (saleEventId) => {
  const response = await axios.post(`${resource}/${saleEventId}/deactivate`, {}, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const addTier = async (saleEventId, payload) => {
  const response = await axios.post(`${resource}/${saleEventId}/tiers`, payload, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const removeTier = async (saleEventId, tierId) => {
  await axios.delete(`${resource}/${saleEventId}/tiers/${tierId}`, { suppressErrorNotice: true });
};

const updateTierPriority = async (saleEventId, tierId, priority) => {
  const response = await axios.put(`${resource}/${saleEventId}/tiers/${tierId}/priority`, { priority }, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const assignListingsToTier = async (saleEventId, payload) => {
  const response = await axios.post(`${resource}/${saleEventId}/listings`, payload, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const removeListingAssignment = async (saleEventId, listingId) => {
  await axios.delete(`${resource}/${saleEventId}/listings/${listingId}`, { suppressErrorNotice: true });
};

const reassignListing = async (saleEventId, listingId, newTierId) => {
  const response = await axios.put(`${resource}/${saleEventId}/listings/${listingId}/reassign`, { newTierId }, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const getPerformanceMetrics = async (saleEventId, query = {}) => {
  const params = Object.fromEntries(
    Object.entries({
      startDate: query?.startDate ?? query?.StartDate,
      endDate: query?.endDate ?? query?.EndDate
    }).filter(([, value]) => value !== undefined && value !== null && value !== "")
  );

  const response = await axios.get(`${resource}/${saleEventId}/performance`, {
    params,
    suppressErrorNotice: true
  });

  return response?.data ?? null;
};

const SaleEventService = {
  getSaleEvents,
  getSaleEventById,
  getEligibleListings,
  createSaleEvent,
  updateSaleEvent,
  updateSaleEventStatus,
  deleteSaleEvent,
  activateSaleEvent,
  deactivateSaleEvent,
  addTier,
  removeTier,
  updateTierPriority,
  assignListingsToTier,
  removeListingAssignment,
  reassignListing,
  getPerformanceMetrics
};

export default SaleEventService;
