import axios from "../../utils/axiosCustomize";

const resource = "research";

const getProductResearch = (params = {}) => {
  const searchParams = new URLSearchParams();

  if (params.keyword) {
    searchParams.append("keyword", params.keyword);
  }

  if (typeof params.days === "number" && !Number.isNaN(params.days)) {
    searchParams.append("days", params.days.toString());
  } else if (params.days) {
    searchParams.append("days", params.days);
  }

  if (params.format) {
    searchParams.append("format", params.format);
  }

  if (typeof params.minPrice === "number" && !Number.isNaN(params.minPrice)) {
    searchParams.append("minPrice", params.minPrice.toString());
  }

  if (typeof params.maxPrice === "number" && !Number.isNaN(params.maxPrice)) {
    searchParams.append("maxPrice", params.maxPrice.toString());
  }

  if (params.freeShippingOnly) {
    searchParams.append("freeShippingOnly", "true");
  }
  
  if (params.categoryId) {
    searchParams.append("categoryId", params.categoryId);
  }

  if (typeof params.page === "number" && params.page > 0) {
    searchParams.append("page", params.page.toString());
  }

  if (typeof params.pageSize === "number" && params.pageSize > 0) {
    searchParams.append("pageSize", params.pageSize.toString());
  }

  const query = searchParams.toString();
  const url = query ? `${resource}/product?${query}` : `${resource}/product`;

  return axios.get(url).then((response) => response?.data ?? null);
};

const normalizeSortKey = (sort) => {
  if (!sort || sort === "opportunity") {
    return undefined;
  }

  switch (sort) {
    case "searchVolume":
      return "searchvolume";
    case "activeListings":
      return "activelistings";
    case "sellThrough":
      return "sellthrough";
    case "alpha":
      return "alpha";
    default:
      return typeof sort === "string" ? sort.toLowerCase() : undefined;
  }
};

const getSourcingInsights = (params = {}) => {
  const searchParams = new URLSearchParams();

  const keyword = params.keyword?.trim();
  if (keyword) {
    searchParams.append("keyword", keyword);
  }

  const sortKey = normalizeSortKey(params.sort);
  if (sortKey) {
    searchParams.append("sort", sortKey);
  }

  if (typeof params.savedOnly === "boolean") {
    searchParams.append("savedOnly", params.savedOnly ? "true" : "false");
  }

  const page = typeof params.page === "number" && params.page > 0 ? params.page : 1;
  const pageSize = typeof params.pageSize === "number" && params.pageSize > 0 ? params.pageSize : 50;

  searchParams.append("page", page.toString());
  searchParams.append("pageSize", pageSize.toString());

  const query = searchParams.toString();
  const url = query ? `${resource}/sourcing?${query}` : `${resource}/sourcing`;

  return axios.get(url).then((response) => response?.data ?? null);
};

const saveSourcingCategory = (categoryId) => {
  if (!categoryId) {
    return Promise.reject(new Error("Category id is required"));
  }

  return axios
    .post(`${resource}/sourcing/saved/${encodeURIComponent(categoryId)}`)
    .then((response) => response?.data ?? null);
};

const removeSourcingCategory = (categoryId) => {
  if (!categoryId) {
    return Promise.reject(new Error("Category id is required"));
  }

  return axios
    .delete(`${resource}/sourcing/saved/${encodeURIComponent(categoryId)}`)
    .then((response) => response?.data ?? null);
};

const ResearchService = {
  getProductResearch,
  getSourcingInsights,
  saveSourcingCategory,
  removeSourcingCategory
};

export default ResearchService;
