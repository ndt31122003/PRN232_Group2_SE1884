import axios from "axios";
import STORAGE, { deleteStorage, getStorage, setStorage } from "../lib/storage";
import Notice from "../components/Common/CustomNotification";
import { forEach, isArray, isPlainObject } from "lodash";
import AuthService from "../services/AuthService";

// const BASE_URL = "https://prn232.mnhduc.site/api";
// const BASE_URL = "https://localhost:7046/api";
// const BASE_URL = "http://localhost:5149/api";
// const BASE_URL = "https://propval.io.vn/api";
const isDev = process.env.NODE_ENV !== "production";
const BASE_URL = isDev ? "http://localhost:5149/api" : "https://propval.io.vn/api";


const { refreshToken } = AuthService;

// Hàm chuẩn hóa dữ liệu: Trim các chuỗi trong request
const trimData = (data) => {
    if (!data) return data;
    const tempData = isArray(data) ? [] : {};
    forEach(data, (val, keyName) => {
        if (typeof val === "string") tempData[keyName] = val.trim();
        else if (typeof val === "object") tempData[keyName] = trimData(val);
        else tempData[keyName] = val;
    });
    return tempData;
};

// Xử lý lỗi chung
function parseError(messages) {
    return Promise.reject({
        messages: Array.isArray(messages) ? messages : [messages || "Lỗi hệ thống"],
    });
}

const limitJoin = (items = [], limit = 3) => {
    if (!Array.isArray(items) || items.length === 0) {
        return undefined;
    }

    const slice = items.slice(0, limit);
    const suffix = items.length > limit ? ` • +${items.length - limit} more` : "";
    return `${slice.join(" • ")}${suffix}`;
};

const collectErrorMessages = (rawErrors) => {
    if (!rawErrors) {
        return [];
    }

    if (Array.isArray(rawErrors)) {
        return rawErrors
            .map((item) => item?.description ?? item?.Description ?? item)
            .filter(Boolean)
            .map(String);
    }

    if (isPlainObject(rawErrors)) {
        return Object.values(rawErrors)
            .flat()
            .map((item) => item?.description ?? item?.Description ?? item)
            .filter(Boolean)
            .map(String);
    }

    return [String(rawErrors)];
};

const parseProblemDetails = (payload) => {
    if (!payload || typeof payload !== "object") {
        return null;
    }

    const title = payload.title ?? payload.Title ?? payload.error ?? payload.message;
    const detail = payload.detail ?? payload.Detail ?? payload.description ?? payload.Message;
    const status = payload.status ?? payload.Status ?? payload.statusCode ?? payload.StatusCode;
    const type = payload.type ?? payload.Type;

    const extensions = payload.extensions ?? payload.Extensions;
    const extErrors = extensions?.errors ?? extensions?.Errors;
    const rootErrors = payload.errors ?? payload.Errors;
    const errorMessages = collectErrorMessages(rootErrors ?? extErrors);

    return {
        title,
        detail,
        status,
        type,
        errorMessages
    };
};

const notifyProblemDetails = (payload, fallbackStatus) => {
    const problem = parseProblemDetails(payload);
    if (!problem) {
        return false;
    }

    const status = problem.status ?? fallbackStatus;
    const statusSuffix = status ? ` (SC${status})` : "";
    const message = problem.title ?? `Request failed${statusSuffix}`;
    const detail = problem.errorMessages.length > 0
        ? limitJoin(problem.errorMessages)
        : problem.detail ?? problem.type;

    Notice({
        msg: message,
        desc: detail,
        isSuccess: false
    });

    return true;
};

// Trạng thái refresh token
let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
    failedQueue.forEach((prom) => {
        if (token) {
            prom.resolve(token);
        } else {
            const reason = error instanceof Error ? error : new Error("Refresh token failed");
            prom.reject(reason);
        }
    });
    failedQueue = [];
};

const ensureHeaders = (config) => {
    if (!config.headers) {
        config.headers = {};
    }

    return config.headers;
};

const setAuthorizationHeader = (headers, token) => {
    if (!headers || !token) {
        return;
    }

    if (typeof headers.set === "function") {
        headers.set("Authorization", `Bearer ${token}`);
    } else {
        headers.Authorization = `Bearer ${token}`;
    }
};

const removeAuthorizationHeader = (headers) => {
    if (!headers) {
        return;
    }

    if (typeof headers.delete === "function") {
        headers.delete("Authorization");
    } else if (Object.prototype.hasOwnProperty.call(headers, "Authorization")) {
        delete headers.Authorization;
    }
};

const shouldSuppressNotice = (config) =>
    Boolean(config?.suppressErrorNotice);

// Hàm xử lý body response
async function parseBody(response) {
    const resData = response.data;
    const status = response?.status;

    // Nếu HTTP lỗi server
    if (status >= 500) {
        const problem = parseProblemDetails(resData);
        const notified = notifyProblemDetails(resData, status);

        if (!notified && !shouldSuppressNotice(response?.config)) {
            if (!shouldSuppressNotice(response?.config)) {
                Notice({ msg: "Hệ thống đang gián đoạn, vui lòng thử lại sau.", isSuccess: false });
            }
        }

        const messages = problem
            ? (problem.errorMessages.length > 0
                ? problem.errorMessages
                : [problem.detail, problem.title, problem.type].filter(Boolean))
            : ["Lỗi hệ thống"];

        return parseError(messages);
    }

    if (resData?.StatusCode === 401) {
        const unauthorizedError = new Error("Unauthorized");
        unauthorizedError.response = { ...response, status: 401 };
        unauthorizedError.config = response.config;
        return Promise.reject(unauthorizedError);
    }

    // Nếu API trả về mã EC -999 => Redirect login
    if (resData?.EC === -999) {
        redirectToLogin();
        return;
    }

    // Trả về dữ liệu bình thường
    return response;
}

// Tạo instance axios
const instance = axios.create({
    baseURL: BASE_URL,
    timeout: 60000,
    headers: { "Content-Type": "application/json" },
    withCredentials: true,
});

// Interceptor request: Thêm token vào header
instance.interceptors.request.use(
    (config) => {
        const requestConfig = config;

        requestConfig.params = { ...requestConfig.params, IsDomain: 1 };

        if (requestConfig.data && !(requestConfig.data instanceof FormData)) {
            requestConfig.data = trimData(requestConfig.data);
        }

        const headers = ensureHeaders(requestConfig);

        if (!requestConfig.skipAuthRefresh) {
            const token = getStorage(STORAGE.TOKEN);
            setAuthorizationHeader(headers, token);
        } else {
            removeAuthorizationHeader(headers);
        }

        return requestConfig;
    },
    (error) => Promise.reject(error)
);

// Interceptor response: Xử lý lỗi chung
const redirectToLogin = () => {
    deleteStorage(STORAGE.TOKEN);
    deleteStorage(STORAGE.REFRESH_TOKEN);
    deleteStorage(STORAGE.USER_INFO);

    const runtime = typeof window !== "undefined"
        ? window
        : typeof global !== "undefined"
            ? global
            : undefined;

    if (runtime?.location) {
        runtime.location.replace("/login");
    }
};

const extractAccessToken = (payload) => {
    if (!payload || typeof payload !== "object") {
        return undefined;
    }

    return (
        payload.Object ??
        payload.object ??
        payload.accessToken ??
        payload.token ??
        payload.data?.accessToken ??
        payload.data?.token
    );
};

const extractRefreshToken = (payload) => {
    if (!payload || typeof payload !== "object") {
        return undefined;
    }

    return (
        payload.refreshToken ??
        payload.RefreshToken ??
        payload.data?.refreshToken ??
        payload.data?.RefreshToken
    );
};

instance.interceptors.response.use(
    (response) => parseBody(response),
    async (error) => {
        const { response, code } = error;
        const originalRequest = error.config ?? {};

        if (response?.status === 401 && !originalRequest.skipAuthRefresh) {
            if (!error.config) {
                redirectToLogin();
                const reason = new Error("Unauthorized");
                return Promise.reject(reason);
            }
            const refreshTokenValue = getStorage(STORAGE.REFRESH_TOKEN);

            if (!refreshTokenValue) {
                // redirectToLogin();
                const reason = error instanceof Error ? error : new Error("Unauthorized");
                return Promise.reject(reason);
            }

            if (originalRequest._retry) {
                // redirectToLogin();
                const reason = error instanceof Error ? error : new Error("Unauthorized");
                return Promise.reject(reason);
            }

            if (isRefreshing) {
                return new Promise((resolve, reject) => {
                    failedQueue.push({ resolve, reject });
                })
                    .then((token) => {
                        if (!token) {
                            throw new Error("Refresh token not available");
                        }

                        const headers = ensureHeaders(originalRequest);
                        setAuthorizationHeader(headers, token);
                        return instance(originalRequest);
                    })
                    .catch((refreshErr) => {
                        const reason = refreshErr instanceof Error
                            ? refreshErr
                            : new Error("Failed to retry request after refresh");
                        return Promise.reject(reason);
                    });
            }

            originalRequest._retry = true;
            isRefreshing = true;

            return new Promise(async (resolve, reject) => {
                try {
                    const refreshResponse = await refreshToken(refreshTokenValue);
                    const refreshPayload = refreshResponse?.data ?? refreshResponse;
                    const newAccessToken = extractAccessToken(refreshPayload);
                    const newRefreshToken = extractRefreshToken(refreshPayload);

                    if (!newAccessToken) {
                        throw new Error("Không thể refresh token");
                    }

                    setStorage(STORAGE.TOKEN, newAccessToken);
                    if (newRefreshToken) {
                        setStorage(STORAGE.REFRESH_TOKEN, newRefreshToken);
                    }

                    if (instance.defaults.headers) {
                        setAuthorizationHeader(instance.defaults.headers, newAccessToken);

                        if (!instance.defaults.headers.common) {
                            instance.defaults.headers.common = {};
                        }

                        setAuthorizationHeader(instance.defaults.headers.common, newAccessToken);
                    }
                    processQueue(null, newAccessToken);

                    const headers = ensureHeaders(originalRequest);
                    setAuthorizationHeader(headers, newAccessToken);

                    resolve(instance(originalRequest));
                } catch (refreshErr) {
                    processQueue(refreshErr, null);
                    // redirectToLogin();
                    const reason = refreshErr instanceof Error
                        ? refreshErr
                        : new Error("Refresh token request failed");
                    reject(reason);
                } finally {
                    isRefreshing = false;
                }
            });
        }

        const suppressNotice = shouldSuppressNotice(originalRequest);

        // Axios throws ERR_CANCELED whenever AbortController cancels a request; skip the toast for this case
        if (code === "ERR_CANCELED" || originalRequest?.signal?.aborted) {
            const reason = error instanceof Error ? error : new Error("Request cancelled");
            return Promise.reject(reason);
        }

        if (code === "ECONNABORTED" || code === "ERR_NETWORK") {
            if (!suppressNotice) {
                Notice({
                    msg: "Hệ thống đang gián đoạn, vui lòng kiểm tra kết nối!",
                    isSuccess: false,
                });
            }
        } else if (response?.status >= 500) {
            if (!notifyProblemDetails(response?.data, response?.status) && !suppressNotice) {
                Notice({ msg: "Hệ thống gián đoạn", isSuccess: false });
            }
        } else if (response?.status && response.status !== 200) {
            if (!notifyProblemDetails(response?.data, response.status) && !suppressNotice) {
                Notice({
                    msg: `Lỗi hệ thống (SC${response.status}), vui lòng thử lại sau.`,
                    isSuccess: false,
                });
            }
        } else if (!suppressNotice) {
        }

        const reason = error instanceof Error ? error : new Error("Request failed");
        return Promise.reject(reason);
    }
);

export default instance;

// API tải file
export const httpGetFile = (path = "", optionalHeader = {}) =>
    instance.get(path, { headers: optionalHeader });