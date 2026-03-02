import axios from "../../utils/axiosCustomize";

const resource = "users";

const getById = (userId) =>
  axios
    .get(`${resource}/${userId}`)
    .then((response) => response?.data ?? null);

const verifyPayment = (payload) =>
  axios
    .post(`${resource}/verify-payment`, payload)
    .then((response) => response?.data ?? null);

const UserService = {
  getById,
  verifyPayment,
};

export default UserService;
