import React, { useEffect, useState } from 'react';
import { getSignalRConnection, subscribeToEvent, stopSignalRConnection } from '../../utils/signalRConnection';
import { getStorage } from '../../lib/storage';
import STORAGE from '../../lib/storage';

const SignalRTest = () => {
  const [connectionState, setConnectionState] = useState('Disconnected');
  const [logs, setLogs] = useState([]);
  const [token, setToken] = useState('');

  const addLog = (message) => {
    setLogs(prev => [...prev, `[${new Date().toLocaleTimeString()}] ${message}`]);
    console.log(message);
  };

  useEffect(() => {
    const userToken = getStorage(STORAGE.USER_TOKEN);
    setToken(userToken ? 'Token exists' : 'No token found');
    addLog(`Token status: ${userToken ? 'Found' : 'Not found'}`);
  }, []);

  const handleConnect = async () => {
    try {
      addLog('Attempting to connect to SignalR...');
      const connection = await getSignalRConnection();
      
      if (connection) {
        setConnectionState(connection.state);
        addLog(`✅ Connected! State: ${connection.state}`);
        addLog(`Connection ID: ${connection.connectionId}`);
      } else {
        addLog('❌ Connection is null');
        setConnectionState('Failed');
      }
    } catch (error) {
      addLog(`❌ Connection error: ${error.message}`);
      setConnectionState('Error');
    }
  };

  const handleDisconnect = async () => {
    try {
      addLog('Disconnecting...');
      await stopSignalRConnection();
      setConnectionState('Disconnected');
      addLog('✅ Disconnected');
    } catch (error) {
      addLog(`❌ Disconnect error: ${error.message}`);
    }
  };

  const handleSubscribe = async () => {
    try {
      addLog('Subscribing to test events...');
      
      await subscribeToEvent('DisputeStatusChanged', (data) => {
        addLog(`📨 DisputeStatusChanged received: ${JSON.stringify(data)}`);
      });

      await subscribeToEvent('DisputeNewResponse', (data) => {
        addLog(`📨 DisputeNewResponse received: ${JSON.stringify(data)}`);
      });

      addLog('✅ Subscribed to events');
    } catch (error) {
      addLog(`❌ Subscribe error: ${error.message}`);
    }
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-6">SignalR Connection Test</h1>
      
      <div className="bg-white rounded-lg shadow p-6 mb-6">
        <div className="mb-4">
          <p className="text-sm text-gray-600">Connection State:</p>
          <p className="text-xl font-bold">{connectionState}</p>
        </div>
        
        <div className="mb-4">
          <p className="text-sm text-gray-600">Token Status:</p>
          <p className="text-sm font-mono">{token}</p>
        </div>

        <div className="flex gap-3">
          <button
            onClick={handleConnect}
            className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
          >
            Connect
          </button>
          
          <button
            onClick={handleDisconnect}
            className="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700"
          >
            Disconnect
          </button>
          
          <button
            onClick={handleSubscribe}
            className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700"
          >
            Subscribe to Events
          </button>
        </div>
      </div>

      <div className="bg-gray-900 text-green-400 rounded-lg p-4 font-mono text-sm">
        <div className="mb-2 text-white font-bold">Console Logs:</div>
        <div className="max-h-96 overflow-y-auto">
          {logs.map((log, index) => (
            <div key={index} className="mb-1">{log}</div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default SignalRTest;
