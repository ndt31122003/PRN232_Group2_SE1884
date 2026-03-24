import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import DisputeService from '../../services/DisputeService';
import { LoadingScreen } from '../../components/LoadingScreen/LoadingScreen';
import Notice from '../../components/Common/CustomNotification';

export default function DisputeDetail() {
  const { disputeId } = useParams();
  const [dispute, setDispute] = useState(null);
  const [loading, setLoading] = useState(true);
  const [responding, setResponding] = useState(false);
  const [responseMessage, setResponseMessage] = useState('');

  const loadDispute = async () => {
    try {
      setLoading(true);
      if (DisputeService.getDisputeById) {
        console.log('[DisputeDetail] Loading dispute:', disputeId);
        const res = await DisputeService.getDisputeById(disputeId);
        console.log('[DisputeDetail] Dispute loaded:', res);
        setDispute(res);
      } else {
        console.error('[DisputeDetail] getDisputeById not implemented');
        Notice({ msg: 'getDisputeById not implemented on backend', isSuccess: false });
      }
    } catch (err) {
      console.error('[DisputeDetail] Error loading dispute:', err);
      Notice({ msg: `Failed to load dispute: ${err.message}`, isSuccess: false });
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadDispute();
  }, [disputeId]);

  const handleRespond = async (e) => {
    e.preventDefault();
    if (!responseMessage.trim()) {
      Notice({ msg: 'Please enter a message', isSuccess: false });
      return;
    }

    try {
      setResponding(true);
      if (DisputeService.respondToDispute) {
        await DisputeService.respondToDispute(disputeId, { message: responseMessage });
        Notice({ msg: 'Response sent successfully!', isSuccess: true });
        setResponseMessage('');
        // Reload dispute to show new response
        await loadDispute();
      } else {
        Notice({ msg: 'respondToDispute not implemented on backend', isSuccess: false });
      }
    } catch (err) {
      console.error(err);
      Notice({ msg: 'Failed to send response', isSuccess: false });
    } finally {
      setResponding(false);
    }
  };

  if (loading) return <LoadingScreen />;

  if (!dispute) {
    return (
      <div className="container mx-auto px-4 py-8">
        <p>Dispute not found.</p>
        <Link to="/disputes" className="text-blue-600">Back to disputes</Link>
      </div>
    );
  }

  const responses = dispute.responses || dispute.Responses || [];
  const isClosed = dispute.status === 'Closed' || dispute.status === 'Resolved';

  return (
    <div className="container mx-auto px-4 py-8 max-w-4xl">
      <div className="mb-4">
        <Link to="/disputes" className="text-blue-600 hover:underline">&larr; Back to disputes</Link>
      </div>

      <div className="bg-white rounded-lg shadow-md p-6">
        <h2 className="text-2xl font-semibold mb-4">Dispute Details</h2>
        
        <div className="grid grid-cols-2 gap-4 mb-6">
          <div>
            <span className="text-gray-600">Status:</span>
            <span className={`ml-2 px-3 py-1 rounded-full text-sm ${
              dispute.status === 'Open' ? 'bg-yellow-100 text-yellow-800' :
              dispute.status === 'Resolved' ? 'bg-green-100 text-green-800' :
              dispute.status === 'Closed' ? 'bg-gray-100 text-gray-800' :
              'bg-blue-100 text-blue-800'
            }`}>
              {dispute.status || 'N/A'}
            </span>
          </div>
          <div>
            <span className="text-gray-600">Created:</span>
            <span className="ml-2">{new Date(dispute.createdAt).toLocaleString()}</span>
          </div>
        </div>

        <div className="mb-6">
          <h3 className="font-semibold text-lg mb-2">Reason:</h3>
          <div className="p-4 bg-gray-50 rounded">{dispute.reason || 'N/A'}</div>
        </div>

        <div className="mb-6">
          <h3 className="font-semibold text-lg mb-3">Conversation ({responses.length})</h3>
          <div className="space-y-3 max-h-96 overflow-y-auto">
            {responses.length === 0 ? (
              <p className="text-gray-500 italic">No responses yet</p>
            ) : (
              responses.map((response) => (
                <div key={response.id} className="p-4 bg-gray-50 rounded-lg">
                  <div className="flex justify-between items-start mb-2">
                    <span className="font-medium text-sm text-gray-700">
                      {response.responderUsername || `User ${response.responderId}`}
                    </span>
                    <span className="text-xs text-gray-500">
                      {new Date(response.createdAt).toLocaleString()}
                    </span>
                  </div>
                  <p className="text-gray-800">{response.message}</p>
                </div>
              ))
            )}
          </div>
        </div>

        {!isClosed && (
          <div className="border-t pt-6">
            <h3 className="font-semibold text-lg mb-3">Add Response</h3>
            <form onSubmit={handleRespond}>
              <textarea
                className="w-full p-3 border rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                rows="4"
                placeholder="Type your response here..."
                value={responseMessage}
                onChange={(e) => setResponseMessage(e.target.value)}
                disabled={responding}
              />
              <div className="mt-3 flex justify-end">
                <button
                  type="submit"
                  disabled={responding || !responseMessage.trim()}
                  className="px-6 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 disabled:bg-gray-400 disabled:cursor-not-allowed"
                >
                  {responding ? 'Sending...' : 'Send Response'}
                </button>
              </div>
            </form>
          </div>
        )}
      </div>
    </div>
  );
}
