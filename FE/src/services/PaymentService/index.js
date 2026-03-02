import axios from "../../utils/axiosCustomize";

const resource = "payments";

const getTransactions = (params = {}, signal) =>
    axios.get(`${resource}/transactions`, {
        params,
        signal,
    });

const getReport = (params = {}, signal) =>
    axios.get(`${resource}/reports`, {
        params,
        signal,
    });

const getPayouts = (params = {}, signal) =>
    axios.get(`${resource}/payouts`, {
        params,
        signal,
    });

const getPayoutDetail = (payoutId, signal) =>
    axios.get(`${resource}/payouts/${payoutId}`, {
        signal,
    });

const getSummary = (signal) =>
    axios.get(`${resource}/summary`, {
        signal,
    });

const PaymentService = {
    getTransactions,
    getReport,
    getPayouts,
    getPayoutDetail,
    getSummary,
};

export default PaymentService;
