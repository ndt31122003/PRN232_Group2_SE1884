import axios from "../../utils/axiosCustomize";

const resource = "seller-hub";

const getOverview = () =>
  axios.get(`${resource}/overview`).then((response) => response?.data ?? null);

const SellerHubService = {
  getOverview
};

export default SellerHubService;
