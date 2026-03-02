import axios from "../../utils/axiosCustomize";

const resource = "performance";

const withParams = (params = {}, options = {}) => ({
    ...options,
    params: {
        ...(options.params ?? {}),
        ...params,
    },
});

/**
 * Lấy dữ liệu tổng quan cho tab Performance Summary
 */
const getPerformanceSummary = (period, options = {}) => {
    const params = typeof period === "string" ? { period } : (period ?? {});
    return axios.get(`/${resource}/summary`, withParams(params, options));
};

/**
 * Lấy dữ liệu biểu đồ doanh số
 */
const getPerformanceSales = (params, options = {}) => {
    const normalizedParams = typeof params === "string"
        ? { period: params }
        : (params ?? {});

    return axios.get(`/${resource}/sales`, withParams(normalizedParams, options));
};

/**
 * Lấy danh sách sản phẩm bán chạy
 */
const getTopSellingItems = (params, limitOrOptions, maybeOptions) => {
    if (typeof params === "string") {
        return axios.get(
            `/${resource}/top-selling`,
            withParams({ period: params, limit: limitOrOptions ?? 5 }, maybeOptions ?? {})
        );
    }

    const { period, limit = 5, ...rest } = params ?? {};
    const options = maybeOptions ?? limitOrOptions ?? {};

    return axios.get(
        `/${resource}/top-selling`,
        withParams({ period, limit, ...rest }, options)
    );
};

/**
 * Lấy dữ liệu traffic
 */
const getPerformanceTraffic = (params, options = {}) => {
    const normalizedParams = typeof params === "string"
        ? { period: params }
        : (params ?? {});

    return axios.get(`/${resource}/traffic`, withParams(normalizedParams, options));
};

/**
 * Lấy dữ liệu seller level
 */
const getPerformanceSellerLevel = (options = {}) => {
    return axios.get(`/${resource}/seller-level`, options);
};

/**
 * Lấy dữ liệu service metrics
 */
const getPerformanceServiceMetrics = (period, options = {}) => {
    const params = typeof period === "string"
        ? { period }
        : (period ?? {});

    return axios.get(`/${resource}/service-metrics`, withParams(params, options));
};

/**
 * Tải báo cáo doanh số (UC-33)
 */
const downloadSalesReport = (period, format, options = {}) => {
    const params = {
        ...(typeof period === "string" ? { period } : (period ?? {})),
        ...(typeof format === "string" ? { format } : {}),
    };

    const requestOptions = {
        responseType: "blob",
        ...options,
    };

    return axios.get(`/${resource}/export-sales`, withParams(params, requestOptions));
};

export const PerformanceService = {
    getPerformanceSummary,
    getPerformanceSales,
    getTopSellingItems,
    getPerformanceTraffic,
    getPerformanceSellerLevel,
    getPerformanceServiceMetrics,
    downloadSalesReport,
};