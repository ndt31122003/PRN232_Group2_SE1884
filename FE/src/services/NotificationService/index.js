import axios from "../../utils/axiosCustomize";

const NotificationService = {
    getNotifications: (pageSize = 20) =>
        axios.get("notifications", { params: { pageSize } }).then(r => r?.data ?? []),

    getUnreadCount: () =>
        axios.get("notifications/unread-count").then(r => r?.data?.count ?? 0),

    markAllRead: () =>
        axios.post("notifications/mark-all-read"),
};

export default NotificationService;
