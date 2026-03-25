import axios from "axios";

// const BASE_URL = "https://prn232.mnhduc.site/api/";
// const BASE_URL = "http://localhost:8080/api/";
const BASE_URL = "https://propval.io.vn/api/";

// Hàm hiển thị thông báo lỗi (có thể thay bằng Toast Notification)
const showError = (message) => {
    console.error(message);
    alert(message); // TODO: Thay bằng Toast Notification như react-toastify
};

// Hàm hỗ trợ quản lý token
const getToken = () => localStorage.getItem("TOKEN");
const getRefreshToken = () => localStorage.getItem("REFRESH_TOKEN");
const setToken = (token) => localStorage.setItem("TOKEN", token);
const setRefreshToken = (token) => localStorage.setItem("REFRESH_TOKEN", token);
const clearAuth = () => {
    localStorage.removeItem("TOKEN");
    localStorage.removeItem("REFRESH_TOKEN");
    window.location.replace("/login");
};

// Tạo instance Axios
const instance = axios.create({
    baseURL: BASE_URL,
    timeout: 60000,
    headers: {
        "Content-Type": "application/json",
    },
    withCredentials: true,
});

// Logic refresh token
let isRefreshing = false;
let failedQueue = [];

const processQueue = (error, token = null) => {
    failedQueue.forEach((prom) => {
        if (error) prom.reject(error);
        else prom.resolve(token);
    });
    failedQueue = [];
};

const refreshAccessToken = async () => {
    const refreshToken = getRefreshToken();
    if (!refreshToken) {
        clearAuth();
        throw new Error("No refresh token available");
    }

    try {
        const res = await axios.post(`${BASE_URL}/identity/refresh-token`, { refreshToken });
        const { accessToken, refreshToken: newRefreshToken } = res.data;
        setToken(accessToken);
        setRefreshToken(newRefreshToken);
        return accessToken;
    } catch (err) {
        clearAuth();
        throw new Error("Failed to refresh token");
    }
};

// Request interceptor
instance.interceptors.request.use(
    (config) => {
        const token = getToken();
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Response interceptor
instance.interceptors.response.use(
    (response) => response.data,
    async (error) => {
        const originalRequest = error.config;

        // Xử lý lỗi 401 và refresh token
        if (error.response?.status === 401 && !originalRequest._retry) {
            if (isRefreshing) {
                // Đưa yêu cầu vào hàng đợi nếu token đang được làm mới
                return new Promise((resolve, reject) => {
                    failedQueue.push({
                        resolve: (token) => {
                            originalRequest.headers.Authorization = `Bearer ${token}`;
                            resolve(instance(originalRequest));
                        },
                        reject,
                    });
                });
            }

            originalRequest._retry = true;
            isRefreshing = true;

            try {
                const newToken = await refreshAccessToken();
                processQueue(null, newToken);
                originalRequest.headers.Authorization = `Bearer ${newToken}`;
                return instance(originalRequest);
            } catch (err) {
                processQueue(err, null);
                showError("Phiên đăng nhập đã hết hạn!");
                return Promise.reject(err);
            } finally {
                isRefreshing = false;
            }
        }

        // Xử lý các lỗi khác
        if (error.code === "ECONNABORTED") {
            showError("Hệ thống đang gián đoạn, vui lòng thử lại sau!");
        } else if (error.code === "ERR_NETWORK") {
            showError("Lỗi mạng, vui lòng kiểm tra kết nối Internet!");
        } else if (error.response) {
            const status = error.response.status;
            const message = error.response.data?.message || error.response.data?.error || "Có lỗi xảy ra";

            if (status >= 500) {
                showError("Lỗi máy chủ, vui lòng thử lại sau!");
            } else {
                showError(`${message} (SC${status})`);
            }
        } else {
            showError("Hệ thống đang gián đoạn, vui lòng thử lại sau!");
        }

        return Promise.reject(error);
    }
);

// Hàm hỗ trợ GET file
export const httpGetFile = (path = "", optionalHeader = {}) =>
    instance({
        method: "GET",
        url: path,
        headers: { ...optionalHeader },
        responseType: "blob",
    });

export default instance;