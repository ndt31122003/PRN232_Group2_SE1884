import axios from "../../utils/axiosCustomize";

const resource = "shipping-services";

const list = (signal) =>
    axios.get(resource, {
        signal,
    });

const ShippingServiceApi = {
    list,
};

export default ShippingServiceApi;
