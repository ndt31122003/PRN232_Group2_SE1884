import axios from "../../utils/axiosCustomize";

const resource = "inventories";

const getInventoryAlerts = (options = {}) => {
    return axios.get(`/${resource}/alerts`, options);
};

const updateInventoryAlert = (payload, options = {}) => {
    return axios.put(`/${resource}/alerts`, payload, options);
};

export const InventoryService = {
    getInventoryAlerts,
    updateInventoryAlert,
};