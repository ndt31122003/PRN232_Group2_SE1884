import axios from "../../utils/axiosCustomize";

const resource = "orders";

const getOrders = (params = {}, signal) =>
    axios.get(`${resource}/all-orders`, {
        params,
        signal,
    });

const getStatuses = (signal) =>
    axios.get(`${resource}/all-statuses`, {
        signal,
    });

const getOrderById = (orderId, signal) =>
    axios.get(resource, {
        params: { id: orderId },
        signal,
    });

const printShippingLabel = (orderId, payload, signal) =>
    axios.post(`${resource}/${orderId}/shipping-label`, payload, {
        signal,
    });

const previewShippingLabel = (orderId, payload, signal) =>
    axios.post(`${resource}/${orderId}/shipping-label/preview`, payload, {
        signal,
    });

const getShippingLabels = (params = {}, signal) =>
    axios.get(`${resource}/shipping-labels`, {
        params,
        signal,
    });

const voidShippingLabel = (orderId, labelId, payload, signal) =>
    axios.post(`${resource}/${orderId}/shipping-labels/${labelId}/void`, payload, {
        signal,
    });

const updateDeliveryStatus = (orderId, payload, signal) =>
    axios.post(`${resource}/${orderId}/delivery-status`, payload, {
        signal,
    });

const upsertShipments = (orderId, payload, signal) =>
    axios.post(`${resource}/${orderId}/shipments`, payload, {
        signal,
    });

const leaveFeedback = (orderId, payload, signal) =>
    axios.post(`${resource}/${orderId}/feedback`, payload, {
        signal,
    });

const getCancellationRequests = (params = {}, signal) =>
    axios.get(`${resource}/cancellations`, {
        params,
        signal,
    });

const getCancellationRequestById = (cancellationRequestId, signal) =>
    axios.get(`${resource}/cancellations/${cancellationRequestId}`, {
        signal,
    });

const getOrderByCancellationRequestId = (cancellationRequestId, signal) =>
    axios.get(`${resource}/cancellations/${cancellationRequestId}/order`, {
        signal,
    });

const getReturnRequests = (params = {}, signal) =>
    axios.get(`${resource}/returns`, {
        params,
        signal,
    });

const getReturnRequestById = (returnRequestId, signal) =>
    axios.get(`${resource}/returns/${returnRequestId}`, {
        signal,
    });

const getOrderByReturnRequestId = (returnRequestId, signal) =>
    axios.get(`${resource}/returns/${returnRequestId}/order`, {
        signal,
    });

const createCancellationRequest = (orderId, payload, signal) =>
    axios.post(`${resource}/${orderId}/cancellations`, payload, {
        signal,
    });

const approveCancellationRequest = (cancellationRequestId, payload, signal) =>
    axios.post(`${resource}/cancellations/${cancellationRequestId}/approve`, payload, {
        signal,
    });

const rejectCancellationRequest = (cancellationRequestId, payload, signal) =>
    axios.post(`${resource}/cancellations/${cancellationRequestId}/reject`, payload, {
        signal,
    });

const approveReturnRequest = (returnRequestId, payload, signal) =>
    axios.post(`${resource}/returns/${returnRequestId}/approve`, payload, {
        signal,
    });

const rejectReturnRequest = (returnRequestId, payload, signal) =>
    axios.post(`${resource}/returns/${returnRequestId}/reject`, payload, {
        signal,
    });

const OrderService = {
    getOrders,
    getStatuses,
    getOrderById,
    printShippingLabel,
    previewShippingLabel,
    getShippingLabels,
    voidShippingLabel,
    updateDeliveryStatus,
    getCancellationRequests,
    getCancellationRequestById,
    getOrderByCancellationRequestId,
    getReturnRequests,
    getReturnRequestById,
    getOrderByReturnRequestId,
    createCancellationRequest,
    approveCancellationRequest,
    rejectCancellationRequest,
    approveReturnRequest,
    rejectReturnRequest,
    upsertShipments,
    leaveFeedback,
};

export default OrderService;
