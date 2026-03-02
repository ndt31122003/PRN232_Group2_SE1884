import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import DisputeService from '../../services/DisputeService';
import Notice from '../../components/Common/CustomNotification';

export default function DisputeAction() {
  const { disputeId, mode } = useParams();
  const [message, setMessage] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleAction = async () => {
    try {
      setLoading(true);
      if (mode === 'respond') {
        if (DisputeService.respondToDispute) {
          await DisputeService.respondToDispute(disputeId, { message });
          Notice({ msg: 'Response sent', isSuccess: true });
        } else {
          Notice({ msg: 'respondToDispute not implemented on backend', isSuccess: false });
        }
      } else if (mode === 'escalate') {
        if (DisputeService.escalateDispute) {
          await DisputeService.escalateDispute(disputeId);
          Notice({ msg: 'Dispute escalated', isSuccess: true });
        } else {
          Notice({ msg: 'escalateDispute not implemented on backend', isSuccess: false });
        }
      } else if (mode === 'close') {
        if (DisputeService.closeDispute) {
          await DisputeService.closeDispute(disputeId);
          Notice({ msg: 'Dispute closed', isSuccess: true });
        } else {
          Notice({ msg: 'closeDispute not implemented on backend', isSuccess: false });
        }
      } else {
        Notice({ msg: 'Unknown action', isSuccess: false });
      }
      navigate('/disputes');
    } catch (err) {
      console.error(err);
      Notice({ msg: 'Action failed', isSuccess: false });
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-8 max-w-2xl">
      <h2 className="text-2xl font-semibold mb-4">Action: {mode} — Dispute #{disputeId}</h2>
      {mode === 'respond' && (
        <div className="mb-4">
          <textarea value={message} onChange={(e) => setMessage(e.target.value)} rows={6} className="w-full border rounded p-2" />
        </div>
      )}
      <div className="flex gap-2">
        <button onClick={handleAction} disabled={loading} className="bg-blue-600 text-white px-4 py-2 rounded">
          {loading ? 'Processing...' : 'Confirm'}
        </button>
      </div>
    </div>
  );
}
