import axios from "../../utils/axiosCustomize";

const resource = "reviews";

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
 * Get reviews with filters
 * @param {Object} params - Filter parameters
 * @param {string} params.listingId - Filter by listing ID
 * @param {string} params.reviewerId - Filter by reviewer ID
 * @param {number} params.rating - Filter by rating (1-5)
 * @param {string} params.ratingType - Filter by rating type (Positive, Neutral, Negative)
 * @param {string} params.revisionStatus - Filter by revision status (None, Pending, Approved, Rejected)
 * @param {boolean} params.hasReply - Filter by whether review has reply
 * @param {string} params.fromDate - Filter from date (ISO string)
 * @param {string} params.toDate - Filter to date (ISO string)
 * @param {number} params.pageNumber - Page number (default: 1)
 * @param {number} params.pageSize - Page size (default: 20)
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} Normalized paging result
 */
const getReviews = (params = {}, signal) => {
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
 * Reply to a review
 * @param {string} reviewId - Review ID
 * @param {string} reply - Reply text
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const replyToReview = (reviewId, reply, signal) =>
    axios.post(`${resource}/${reviewId}/reply`, { reply }, {
        signal,
    }).then((response) => response?.data ?? response);

/**
 * Request review revision
 * @param {string} reviewId - Review ID
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const requestReviewRevision = (reviewId, signal) =>
    axios.post(`${resource}/${reviewId}/request-revision`, {}, {
        signal,
    }).then((response) => response?.data ?? response);

const ReviewService = {
    getReviews,
    replyToReview,
    requestReviewRevision,
};

export default ReviewService;



