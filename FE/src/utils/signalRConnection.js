import * as signalR from '@microsoft/signalr';
import { getStorage } from '../lib/storage';
import STORAGE from '../lib/storage';

let connection = null;
let connectionPromise = null;

/**
 * Get or create SignalR connection
 * @returns {Promise<signalR.HubConnection>}
 */
export const getSignalRConnection = async () => {
  if (connection && connection.state === signalR.HubConnectionState.Connected) {
    return connection;
  }

  if (connectionPromise) {
    return connectionPromise;
  }

  connectionPromise = createConnection();
  return connectionPromise;
};

/**
 * Create new SignalR connection
 * @returns {Promise<signalR.HubConnection>}
 */
const createConnection = async () => {
  try {
    const token = getStorage(STORAGE.TOKEN);
    
    if (!token) {
      console.warn('[SignalR] No auth token found, cannot connect');
      return null;
    }

    const apiBaseUrl = process.env.REACT_APP_API_BASE_URL || 'http://localhost:5149/api';
    const serverBaseUrl = apiBaseUrl.replace(/\/api\/?$/, '');
    const hubUrl = `${serverBaseUrl}/hub`;

    connection = new signalR.HubConnectionBuilder()
      .withUrl(hubUrl, {
        accessTokenFactory: () => token,
        skipNegotiation: false,
        transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.ServerSentEvents | signalR.HttpTransportType.LongPolling
      })
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          // Exponential backoff: 0s, 2s, 10s, 30s, then 30s
          if (retryContext.previousRetryCount === 0) return 0;
          if (retryContext.previousRetryCount === 1) return 2000;
          if (retryContext.previousRetryCount === 2) return 10000;
          return 30000;
        }
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    // Connection event handlers
    connection.onreconnecting((error) => {
      console.warn('[SignalR] Connection lost, reconnecting...', error);
    });

    connection.onreconnected((connectionId) => {
      console.log('[SignalR] Reconnected successfully', connectionId);
    });

    connection.onclose((error) => {
      console.error('[SignalR] Connection closed', error);
      connection = null;
      connectionPromise = null;
    });

    await connection.start();
    console.log('[SignalR] Connected successfully', connection.connectionId);

    connectionPromise = null;
    return connection;
  } catch (error) {
    console.error('[SignalR] Failed to connect:', error);
    connection = null;
    connectionPromise = null;
    throw error;
  }
};

/**
 * Stop SignalR connection
 */
export const stopSignalRConnection = async () => {
  if (connection) {
    try {
      await connection.stop();
      console.log('[SignalR] Connection stopped');
    } catch (error) {
      console.error('[SignalR] Error stopping connection:', error);
    } finally {
      connection = null;
      connectionPromise = null;
    }
  }
};

/**
 * Subscribe to a SignalR event
 * @param {string} eventName - Event name to listen for
 * @param {Function} callback - Callback function
 * @returns {Promise<Function>} Unsubscribe function
 */
export const subscribeToEvent = async (eventName, callback) => {
  try {
    const conn = await getSignalRConnection();
    if (!conn) {
      console.warn(`[SignalR] Cannot subscribe to ${eventName}, no connection`);
      return () => {};
    }

    conn.on(eventName, callback);
    console.log(`[SignalR] Subscribed to ${eventName}`);

    // Return unsubscribe function
    return () => {
      conn.off(eventName, callback);
      console.log(`[SignalR] Unsubscribed from ${eventName}`);
    };
  } catch (error) {
    console.error(`[SignalR] Error subscribing to ${eventName}:`, error);
    return () => {};
  }
};

export default {
  getSignalRConnection,
  stopSignalRConnection,
  subscribeToEvent
};
