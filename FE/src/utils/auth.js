import STORAGE, { deleteStorage, getStorage, setStorage } from "../lib/storage";

const normalizeBase64 = (segment) => {
  if (!segment) {
    return "";
  }

  let base64 = segment.replace(/-/g, "+").replace(/_/g, "/");
  const requiredPadding = (4 - (base64.length % 4)) % 4;
  if (requiredPadding) {
    base64 = base64.padEnd(base64.length + requiredPadding, "=");
  }

  return base64;
};

export const decodeJwt = (token) => {
  if (typeof token !== "string" || token.split(".").length < 2) {
    return null;
  }

  try {
    const payloadSegment = token.split(".")[1];
    const normalized = normalizeBase64(payloadSegment);
    const json = atob(normalized);
    return JSON.parse(json);
  } catch (error) {
    console.warn("Failed to decode JWT payload", error);
    return null;
  }
};

const getClaimValue = (payload, keys) => {
  if (!payload) {
    return undefined;
  }

  for (const key of keys) {
    if (payload[key]) {
      return payload[key];
    }
  }

  return undefined;
};

const unwrapUserId = (candidate) => {
  if (candidate == null) {
    return null;
  }

  if (typeof candidate === "object") {
    const valueLike = candidate?.value ?? candidate?.Value ?? null;
    if (valueLike != null) {
      return valueLike;
    }
    return null;
  }

  return typeof candidate === "string" ? candidate : String(candidate);
};

export const extractUserFromToken = (token) => {
  const payload = decodeJwt(token);
  if (!payload) {
    return null;
  }

  const id = getClaimValue(payload, [
    "sub",
    "nameid",
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
  ]);

  const name = getClaimValue(payload, [
    "name",
    "unique_name",
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
  ]);

  const email = getClaimValue(payload, [
    "email",
    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress",
  ]);

  const isEmailVerified = payload["is_email_verified"] === "true";
  const isPhoneVerified = payload["is_phone_verified"] === "true";
  const isBusinessVerified = payload["is_business_verified"] === "true";
  const isSellerVerified = payload["is_seller_verified"] === "true";

  const role = getClaimValue(payload, [
    "role",
    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
  ]);

  return {
    id: id ?? null,
    name: name ?? null,
    email: email ?? null,
    role: role ?? null,
    isEmailVerified,
    isPhoneVerified,
    isBusinessVerified,
    isSellerVerified,
  };
};

export const mapUserProfile = (source = {}, fallback = {}) => {
  if (!source && !fallback) {
    return null;
  }

  const normalizedId = unwrapUserId(source?.id ?? source?.Id)
    ?? unwrapUserId(fallback?.id ?? fallback?.Id);

  const normalizedEmail = source?.email?.value
    ?? source?.email?.Value
    ?? (typeof source?.email === "string" ? source.email : null)
    ?? fallback?.email
    ?? null;

  return {
    id: normalizedId,
    name: source?.fullName ?? source?.FullName ?? fallback?.name ?? null,
    email: normalizedEmail,
    role: source?.role ?? source?.Role ?? fallback?.role ?? null,
    isSellerVerified: Boolean(source?.isSellerVerified ?? source?.IsSellerVerified ?? fallback?.isSellerVerified),
    isEmailVerified: Boolean(source?.isEmailVerified ?? source?.IsEmailVerified ?? fallback?.isEmailVerified),
    isPaymentVerified: Boolean(source?.isPaymentVerified ?? source?.IsPaymentVerified ?? fallback?.isPaymentVerified),
    isPhoneVerified: Boolean(source?.isPhoneVerified ?? source?.IsPhoneVerified ?? fallback?.isPhoneVerified),
    isBusinessVerified: Boolean(source?.isBusinessVerified ?? source?.IsBusinessVerified ?? fallback?.isBusinessVerified)
  };
};

export const storeUserInfo = (user, { notify = false } = {}) => {
  if (!user) {
    deleteStorage(STORAGE.USER_INFO);
    if (notify && typeof window !== "undefined") {
      window.dispatchEvent(new Event("user-info-updated"));
    }
    return;
  }

  const payload = JSON.stringify({
    id: unwrapUserId(user?.id),
    name: user.name ?? null,
    email: user.email ?? null,
    role: user.role ?? null,
    isSellerVerified: Boolean(user.isSellerVerified),
    isEmailVerified: Boolean(user.isEmailVerified),
    isPaymentVerified: Boolean(user.isPaymentVerified),
    isPhoneVerified: Boolean(user.isPhoneVerified),
    isBusinessVerified: Boolean(user.isBusinessVerified)
  });

  setStorage(STORAGE.USER_INFO, payload);

  if (notify && typeof window !== "undefined") {
    window.dispatchEvent(new Event("user-info-updated"));
  }
};

export const persistAuthSession = (accessToken, refreshToken) => {
  if (accessToken) {
    setStorage(STORAGE.TOKEN, accessToken);
  }

  if (refreshToken) {
    setStorage(STORAGE.REFRESH_TOKEN, refreshToken);
  }

  const user = extractUserFromToken(accessToken);
  if (user) {
    storeUserInfo(user);
  }
};

export const clearAuthSession = () => {
  deleteStorage(STORAGE.TOKEN);
  deleteStorage(STORAGE.REFRESH_TOKEN);
  deleteStorage(STORAGE.USER_INFO);
};

export const isAuthenticated = () => {
  const token = getStorage(STORAGE.TOKEN);
  return Boolean(token);
};

export const getStoredUser = () => {
  const user = getStorage(STORAGE.USER_INFO);
  if (!user) {
    return null;
  }

  if (typeof user === "string") {
    try {
      return JSON.parse(user);
    } catch (error) {
      return null;
    }
  }

  return user;
};

export const isSellerVerified = () => {
  const user = getStoredUser();
  return Boolean(user?.isSellerVerified);
};
