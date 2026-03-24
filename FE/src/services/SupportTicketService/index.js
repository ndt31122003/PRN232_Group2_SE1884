import axios from "../../utils/axiosCustomize";

const resource = "seller/tickets";

/**
 * Normalize paging response from backend
 */
const normalizePaging = (data, fallback = {}) => ({
    items: data?.items ?? data?.Items ?? [],
    totalCount: data?.totalCount ?? data?.TotalCount ?? 0,
    pageNumber: data?.pageNumber ?? data?.PageNumber ?? fallback.pageNumber ?? 1,
    pageSize: data?.pageSize ?? data?.PageSize ?? fallback.pageSize ?? 20
});

/**
 * Create a new support ticket
 * @param {FormData} formData - FormData containing Category, Subject, Message, and optional Attachments
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response with ticketId
 */
const createTicket = (formData, signal) =>
    axios.post(resource, formData, {
        signal,
        headers: {
            'Content-Type': 'multipart/form-data',
        },
    }).then((response) => response?.data ?? response);

/**
 * Get support tickets with filters
 * @param {Object} params - Filter parameters
 * @param {string} params.status - Filter by status (Open, Pending, Resolved, Closed)
 * @param {string} params.category - Filter by category
 * @param {string} params.keyword - Search keyword
 * @param {number} params.pageNumber - Page number (default: 1)
 * @param {number} params.pageSize - Page size (default: 20)
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} Normalized paging result
 */
const getTickets = (params = {}, signal) => {
    const fallback = {
        pageNumber: params?.pageNumber ?? 1,
        pageSize: params?.pageSize ?? 20
    };
    
    // Map pageNumber to Page for backend
    const queryParams = {
        Status: params.status,
        Category: params.category,
        Keyword: params.keyword,
        Page: params.pageNumber ?? 1,
        PageSize: params.pageSize ?? 20,
    };

    // Remove undefined/null values
    const cleanParams = Object.fromEntries(
        Object.entries(queryParams).filter(([, value]) => value !== undefined && value !== null && value !== '')
    );

    return axios
        .get(resource, { params: cleanParams, signal })
        .then((response) => normalizePaging(response?.data ?? response, fallback));
};

/**
 * Get support ticket by ID
 * @param {string} ticketId - Ticket ID
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const getTicketById = (ticketId, signal) =>
    axios.get(`${resource}/${ticketId}`, {
        signal,
    }).then((response) => response?.data ?? response);

/**
 * Close a support ticket
 * @param {string} ticketId - Ticket ID
 * @param {AbortSignal} signal - Abort signal for cancellation
 * @returns {Promise} API response
 */
const closeTicket = (ticketId, signal) =>
    axios.post(`${resource}/${ticketId}/close`, {}, {
        signal,
    }).then((response) => response?.data ?? response);

const SupportTicketService = {
    createTicket,
    getTickets,
    getTicketById,
    closeTicket,
};

export default SupportTicketService;
