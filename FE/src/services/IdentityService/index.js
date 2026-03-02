import axios from "../../utils/axiosCustomize";

const resource = "identity";

const requestOtp = (payload) =>
  axios
    .post(`${resource}/resend-otp`, payload)
    .then((response) => response?.data ?? null);

const verifyEmail = (payload) =>
  axios
    .post(`${resource}/verify-email`, payload)
    .then((response) => response?.data ?? null);

const IdentityService = {
  requestOtp,
  verifyEmail,
};

export default IdentityService;
