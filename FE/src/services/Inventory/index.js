import axios from "../../utils/axiosCustomize";

const resource = "inventories";

const getInventoryAlerts = (options = {}) => {
    return axios.get(`/${resource}/alerts`, options);
};

const updateInventoryAlert = (payload, options = {}) => {
    return axios.put(`/${resource}/alerts`, payload, options);
};

const restockInventory = (payload, options = {}) => {
    return axios.post(`/${resource}/restock`, payload, options);
};

const importInventoryRestockExcel = (formData, options = {}) => {
    return axios.post(`/${resource}/import-excel`, formData, {
        headers: {
            'Content-Type': 'multipart/form-data',
        },
        ...options,
    });
};

export const InventoryService = {
    getInventoryAlerts,
    updateInventoryAlert,
    restockInventory,
    importInventoryRestockExcel,
};