import { useEffect, useRef } from "react";

/**
 * usePolling – executes `callback` immediately and then every `intervalMs` ms.
 *
 * @param {Function} callback  - async function to call on each tick
 * @param {number}   intervalMs - polling interval in milliseconds (default 30 000)
 * @param {boolean}  enabled   - set to false to pause polling (default true)
 *
 * Example usage inside a listing detail page:
 *
 *   usePolling(fetchListingData, 30_000);
 */
export function usePolling(callback, intervalMs = 30_000, enabled = true) {
    const savedCallback = useRef(callback);

    // Keep ref up-to-date without restarting the interval
    useEffect(() => {
        savedCallback.current = callback;
    }, [callback]);

    useEffect(() => {
        if (!enabled) return;

        // Fire immediately on mount
        savedCallback.current();

        const id = setInterval(() => {
            savedCallback.current();
        }, intervalMs);

        return () => clearInterval(id);
    }, [intervalMs, enabled]);
}
