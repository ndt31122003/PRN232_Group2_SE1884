import { useCallback, useEffect, useRef } from "react";
import Notice from "./CustomNotification";
import { useSignalR } from "../../hooks/useSignalR";
import NotificationService from "../../services/NotificationService";

const SEEN_KEY = "notif_seen_ids"; // localStorage key

function getSeenIds() {
    try {
        return new Set(JSON.parse(localStorage.getItem(SEEN_KEY) || "[]"));
    } catch {
        return new Set();
    }
}

function addSeenId(id) {
    try {
        const ids = getSeenIds();
        ids.add(id);
        // Keep only latest 200 to avoid localStorage bloat
        const trimmed = [...ids].slice(-200);
        localStorage.setItem(SEEN_KEY, JSON.stringify(trimmed));
    } catch { /* silent */ }
}

/**
 * GlobalNotificationListener
 *
 * Strategy:
 * 1. SignalR → instant toast when server pushes "Notification" events.
 * 2. REST polling every 30 s as a fallback.
 * Seen IDs are persisted in localStorage so toasts don't spam on page refresh.
 */
const GlobalNotificationListener = () => {
    const showToast = useCallback((notification) => {
        const id = String(notification.id ?? notification.Id ?? "");
        if (id && getSeenIds().has(id)) return;
        if (id) addSeenId(id);

        const title   = notification.title  ?? notification.Title  ?? "Thông báo mới";
        const message = notification.message ?? notification.Message ?? "";
        const type    = (notification.type  ?? notification.Type   ?? "").toLowerCase();
        const isSuccess = type === "newbid" || type === "neworder";

        Notice({ msg: title, desc: message, isSuccess });
    }, []);

    // ── 1. SignalR real-time ───────────────────────────────────────────────
    const handleNotification = useCallback((payload) => {
        showToast(payload);
    }, [showToast]);

    useSignalR({ Notification: handleNotification });

    // ── 2. REST polling fallback (30s) ─────────────────────────────────────
    useEffect(() => {
        let isMounted = true;

        const poll = async () => {
            try {
                const notifications = await NotificationService.getNotifications(20);
                if (!isMounted) return;

                // Only show unread notifications not already seen
                notifications
                    .filter(n => !(n.isRead ?? n.IsRead))
                    .forEach(n => showToast(n));
            } catch {
                // silent
            }
        };

        // Delay first poll to avoid hammering the server on mount
        const timeout = setTimeout(poll, 3000);
        const id = setInterval(poll, 30_000);

        return () => {
            isMounted = false;
            clearTimeout(timeout);
            clearInterval(id);
        };
    }, [showToast]);

    return null;
};

export default GlobalNotificationListener;
