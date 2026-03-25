import axios from "../../utils/axiosCustomize";

const resource = "identity";

const register = (payload) =>
  axios.post(`${resource}/register`, payload);

const requestOtp = (payload) =>
  axios
    .post(`${resource}/resend-otp`, payload)
    .then((response) => response?.data ?? null);

const verifyEmail = (payload) =>
  axios
    .post(`${resource}/verify-email`, payload)
    .then((response) => response?.data ?? null);

const forgotPassword = (payload) =>
  axios
    .post(`${resource}/forgot-password`, payload)
    .then((response) => response?.data ?? null);

const resetPassword = (payload) =>
  axios
    .post(`${resource}/reset-password`, payload)
    .then((response) => response?.data ?? null);

const setPhoneNumber = (payload) =>
  axios.post(`${resource}/set-phone`, payload);

const verifyPhone = (payload) =>
  axios.post(`${resource}/verify-phone`, payload);

const submitBusiness = (payload) =>
  axios.post(`${resource}/submit-business`, payload);

const getRegistrationStatus = () =>
  axios.get(`${resource}/registration-status`);

const verifyPayment = (payload) =>
  axios.post(`users/verify-payment`, payload);

const IdentityService = {
  register,
  requestOtp,
  verifyEmail,
  forgotPassword,
  resetPassword,
  setPhoneNumber,
  verifyPhone,
  submitBusiness,
  getRegistrationStatus,
  forgotPassword,
  resetPassword,
  verifyPayment,
};

export default IdentityService;

