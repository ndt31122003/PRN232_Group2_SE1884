import axios from "../../utils/axiosCustomize";

const resource = "payments";

const withParams = (params = {}, options = {}) => ({
    ...options,
    params: {
        ...(options.params ?? {}),
        ...params,
    },
});

const getTransactions = (params = {}, options = {}) => {
    return axios.get(`/${resource}/transactions`, withParams(params, options));
};

const getTransactionFilters = (options = {}) => {
    return axios.get(`/${resource}/filters`, withParams({}, options));
};

const getFinancialOverview = (params = {}, options = {}) => {
    return axios.get(`/${resource}/reports`, withParams(params, options));
};

const getFinancialDocuments = (params = {}, options = {}) => {
    return axios.get(`/${resource}/documents`, withParams(params, options));
};

const PaymentsService = {
    getTransactions,
    getTransactionFilters,
    getFinancialOverview,
    getFinancialDocuments,
};

export default PaymentsService;
