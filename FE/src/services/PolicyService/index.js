import axiosInstance from "../../utils/axiosCustomize";

const PolicyService = {
    async createShippingPolicy(storeId, data) {
        return axiosInstance.post(`/Stores/${storeId}/policies/shipping`, data);
    },

    async getShippingPolicies(storeId) {
        return axiosInstance.get(`/Stores/${storeId}/policies/shipping`);
    },

    async updateShippingPolicy(storeId, policyId, data) {
        return axiosInstance.put(`/Stores/${storeId}/policies/shipping/${policyId}`, data);
    },

    async deleteShippingPolicy(storeId, policyId) {
        return axiosInstance.delete(`/Stores/${storeId}/policies/shipping/${policyId}`);
    },

    async createReturnPolicy(storeId, data) {
        return axiosInstance.post(`/Stores/${storeId}/policies/return`, data);
    },

    async getReturnPolicy(storeId) {
        return axiosInstance.get(`/Stores/${storeId}/policies/return`);
    },

    async updateReturnPolicy(storeId, data) {
        return axiosInstance.put(`/Stores/${storeId}/policies/return`, data);
    },

    async deleteReturnPolicy(storeId) {
        return axiosInstance.delete(`/Stores/${storeId}/policies/return`);
    }
};

export default PolicyService;
