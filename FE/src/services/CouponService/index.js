import axios from "../../utils/axiosCustomize";
import { COUPON_TYPE_CATALOG, getCouponTypeById } from "./catalog";

const resource = "coupons";

const listTypes = () => COUPON_TYPE_CATALOG.slice();

const getType = (id) => getCouponTypeById(id);

const createSellerCoupon = async (payload) => {
  const response = await axios.post(resource, payload, { suppressErrorNotice: true });
  return response?.data ?? null;
};

const getSellerCoupons = async () => {
  const response = await axios.get(resource, { suppressErrorNotice: true });
  return response?.data ?? [];
};

const updateCouponStatus = async (couponId, isActive) => {
  const payload = { isActive };
  await axios.patch(`${resource}/${couponId}/status`, payload, { suppressErrorNotice: true });
};

const deleteCoupon = async (couponId) => {
  await axios.delete(`${resource}/${couponId}`, { suppressErrorNotice: true });
};

const CouponService = {
  listTypes,
  getType,
  createSellerCoupon,
  getSellerCoupons,
  updateCouponStatus,
  deleteCoupon
};

export default CouponService;
