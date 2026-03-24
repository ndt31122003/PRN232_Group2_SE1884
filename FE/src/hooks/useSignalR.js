import { useEffect, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import STORAGE, { getStorage } from "../lib/storage";

const HUB_URL = "http://localhost:5149/hub";

/**
 * useSignalR – stable SignalR hook.
 *
 * Pass handlers as a stable object (use useRef or top-level const in the component to avoid
 * creating a new object on every render, which would close/reopen the connection).
 *
 * @param {Object} handlers – { Notification: fn, NewBid: fn, ... }
 * @param {string|null} listingId – join the listing SignalR group when provided
 */
export function useSignalR(handlers = {}, listingId = null) {
    // Keep the latest handlers in a ref so we never recreate the connection when they change
    const handlersRef = useRef(handlers);
    handlersRef.current = handlers;

    const connectionRef = useRef(null);

    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(HUB_URL, {
                accessTokenFactory: () => getStorage(STORAGE.TOKEN) ?? "",
                // Try WebSockets first, fall back to LongPolling
                transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
            })
            .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
            .configureLogging(signalR.LogLevel.Information)
            .build();

        // Delegate every incoming call to the current handler in the ref
        // so we bind once but always call the latest version
        const subscribedMethods = Object.keys(handlersRef.current);
        subscribedMethods.forEach((method) => {
            connection.on(method, (...args) => {
                handlersRef.current[method]?.(...args);
            });
        });

        connectionRef.current = connection;

        connection
            .start()
            .then(() => {
                console.log("[SignalR] ✅ Connected to hub");
                if (listingId) {
                    return connection
                        .invoke("JoinListingGroup", String(listingId))
                        .then(() => console.log(`[SignalR] Joined listing group: ${listingId}`))
                        .catch((e) => console.warn("[SignalR] JoinListingGroup error:", e));
                }
            })
            .catch((err) => {
                console.error("[SignalR] ❌ Connection failed:", err?.message ?? err);
            });

        connection.onreconnected(() => {
            console.log("[SignalR] ✅ Reconnected");
            if (listingId) {
                connection.invoke("JoinListingGroup", String(listingId)).catch(() => {});
            }
        });

        return () => {
            if (listingId && connection.state === signalR.HubConnectionState.Connected) {
                connection.invoke("LeaveListingGroup", String(listingId)).catch(() => {});
            }
            connection.stop().catch(() => {});
        };
        // Only reconnect when listingId changes, NOT on every handler change
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [listingId]);

    return connectionRef;
}

