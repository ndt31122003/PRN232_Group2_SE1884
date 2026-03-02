import axios from "../../utils/axiosCustomize";

const resource = "disputes";

/**
 * Normalize paging response from backend
 * @param {Object} data - Response data from API
 * @param {Object} fallback - Fallback values
 * @returns {Object} Normalized paging data
 */
const normalizePaging = (data, fallback = {}) => ({
    items: data?.items ?? data?.Items ?? [],
    totalCount: data?.totalCount ?? data?.TotalCount ?? 0,
    pageNumber: data?.pageNumber ?? data?.PageNumber ?? fallback.pageNumber ?? 1,
    pageSize: data?.pageSize ?? data?.PageSize ?? fallback.pageSize ?? 20
});

/**
 * Get disputes with filters
 * @param {Object} params - Filter parameters
 * @param {string} params.listingId - Filter by listing ID
 * @param {string} params.raisedById - Filter by user ID who raised the dispute
 * @param {string} params.status - Filter by status (Open, UnderReview, Resolved, Closed)
 * @param {string} params.fromDate - Filter from date (ISO string)
 * @param {string} params.toDate - Filter to date (ISO string)
 * @param {number} params.pageNumber - Page number (default: 1)
 * @param {number} params.pageSize - Page size (default: 20)
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} Normalized paging result
 */
const getDisputes = (params = {}, signal) => {
    const fallback = {
        pageNumber: params?.pageNumber ?? params?.PageNumber ?? 1,
        pageSize: params?.pageSize ?? params?.PageSize ?? 20
    };
    
    const queryParams = Object.fromEntries(
        Object.entries(params).filter(([, value]) => value !== undefined && value !== null)
    );

    return axios
        .get(resource, { params: queryParams, signal })
        .then((response) => normalizePaging(response?.data ?? response, fallback));
};

/**
 * Create a new dispute
 * @param {Object} payload - Dispute data
 * @param {string} payload.listingId - Listing ID
 * @param {string} payload.reason - Reason for dispute
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const createDispute = (payload, signal) =>
    axios.post(resource, payload, {
        signal,
    }).then((response) => response?.data ?? response);

/**
 * Update dispute status
 * @param {string} disputeId - Dispute ID
 * @param {string} status - New status (Open, UnderReview, Resolved, Closed)
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const updateDisputeStatus = (disputeId, status, signal) =>
    axios.put(`${resource}/${disputeId}/status`, { status }, {
        signal,
    }).then((response) => response?.data ?? response);

/**
 * Resolve a dispute
 * @param {string} disputeId - Dispute ID
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const resolveDispute = (disputeId, signal) =>
    axios.post(`${resource}/${disputeId}/resolve`, {}, {
        signal,
    }).then((response) => response?.data ?? response);

/**
 * Close a dispute
 * @param {string} disputeId - Dispute ID
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const closeDispute = (disputeId, signal) =>
    axios.post(`${resource}/${disputeId}/close`, {}, {
        signal,
    }).then((response) => response?.data ?? response);

/**
 * Get dispute by ID
 * @param {string} disputeId - Dispute ID
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const getDisputeById = (disputeId, signal) =>
    axios.get(`${resource}/${disputeId}`, {
        signal,
    }).then((response) => response?.data ?? response);

/**
 * Respond to a dispute
 * @param {string} disputeId - Dispute ID
 * @param {Object} payload - Response data
 * @param {string} payload.message - Response message
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const respondToDispute = (disputeId, payload, signal) =>
    axios.post(`${resource}/${disputeId}/respond`, payload, {
        signal,
    }).then((response) => response?.data ?? response);

/**
 * Escalate a dispute
 * @param {string} disputeId - Dispute ID
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const escalateDispute = (disputeId, signal) =>
    axios.post(`${resource}/${disputeId}/escalate`, {}, {
        signal,
    }).then((response) => response?.data ?? response);

/**
 * Upload evidence files for a dispute
 * @param {string} disputeId - Dispute ID
 * @param {FormData} formData - FormData containing files
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response with file URLs
 */
const uploadEvidence = (disputeId, formData, signal) =>
    axios.post(`${resource}/${disputeId}/evidence`, formData, {
        signal,
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    }).then((response) => response?.data ?? response);

const DisputeService = {
    getDisputes,
    getDisputeById,
    createDispute,
    updateDisputeStatus,
    resolveDispute,
    closeDispute,
    respondToDispute,
    escalateDispute,
    uploadEvidence,
};

export default DisputeService;



