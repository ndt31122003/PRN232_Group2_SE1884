import axios from "../../utils/axiosCustomize";

const resource = "stores";

const createStore = (body) => axios.post(`${resource}`, body);

const getStoreById = (storeId, signal) =>
    axios.get(`${resource}/${storeId}`, {
        signal,
    });

const getMyStores = (signal) =>
    axios.get(`${resource}/my-stores`, {
        signal,
    });

const updateStoreProfile = (storeId, body) =>
    axios.put(`${resource}/${storeId}`, body);

const StoreService = {
    createStore,
    getStoreById,
    getMyStores,
    updateStoreProfile
};

export default StoreService;

