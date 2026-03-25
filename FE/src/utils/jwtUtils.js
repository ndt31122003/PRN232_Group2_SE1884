import STORAGE, { getStorage } from "../lib/storage";

/**
 * Decodes a JWT token payload without verifying the signature.
 * @param {string} token - The raw JWT string
 * @returns {object|null} Decoded payload, or null if invalid
 */
export const decodeJwt = (token) => {
  if (!token || typeof token !== "string") return null;
  try {
    const parts = token.split(".");
    if (parts.length !== 3) return null;
    // Base64url → Base64
    const base64 = parts[1].replace(/-/g, "+").replace(/_/g, "/");
    const padded = base64.padEnd(base64.length + ((4 - (base64.length % 4)) % 4), "=");
    const json = decodeURIComponent(
      atob(padded)
        .split("")
        .map((c) => "%" + c.charCodeAt(0).toString(16).padStart(2, "0"))
        .join("")
    );
    return JSON.parse(json);
  } catch {
    return null;
  }
};

/**
 * Returns the current authenticated user's ID (as a GUID string)
 * from the stored JWT access token.
 *
 * The backend encodes the ID in the standard ClaimTypes.NameIdentifier claim,
 * which maps to the long URI key:
 *   "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
 *
 * @returns {string|null} GUID string or null if not authenticated
 */
export const getCurrentUserId = () => {
  const token = getStorage(STORAGE.TOKEN);
  if (!token) return null;
  const payload = decodeJwt(token);
  if (!payload) return null;
  // ClaimTypes.NameIdentifier (ASP.NET Core default mapping)
  return (
    payload["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] ??
    payload["sub"] ??
    payload["nameid"] ??
    null
  );
};
