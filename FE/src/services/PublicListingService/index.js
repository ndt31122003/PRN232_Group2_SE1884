import axios from "../../utils/axiosCustomize";

const PublicListingService = {
  getPublicListing: async (id) => {
    return axios.get(`/listings/${id}/public`);
  },

  createOffer: async (id, amount) => {
    return axios.post(`/listings/${id}/offers`, { amount });
  },

  placeBid: async (id, amount) => {
    return axios.post(`/listings/${id}/bids`, { amount });
  }
};

export default PublicListingService;
