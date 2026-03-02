import React, { useEffect, useState } from 'react';
import { useParams, Link } from 'react-router-dom';
import DisputeService from '../../services/DisputeService';
import { LoadingScreen } from '../../components/LoadingScreen/LoadingScreen';
import Notice from '../../components/Common/CustomNotification';

export default function DisputeDetail() {
  const { disputeId } = useParams();
  const [dispute, setDispute] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let mounted = true;
    const load = async () => {
      try {
        if (DisputeService.getDisputeById) {
          const res = await DisputeService.getDisputeById(disputeId);
          if (mounted) setDispute(res);
        } else {
          Notice({ msg: 'getDisputeById not implemented on backend', isSuccess: false });
        }
      } catch (err) {
        console.error(err);
        Notice({ msg: 'Failed to load dispute', isSuccess: false });
      } finally {
        if (mounted) setLoading(false);
      }
    };
    load();
    return () => { mounted = false; };
  }, [disputeId]);

  if (loading) return <LoadingScreen />;

  if (!dispute) {
    return (
      <div className="container mx-auto px-4 py-8">
        <p>Dispute not found.</p>
        <Link to="/disputes" className="text-blue-600">Back to disputes</Link>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8 max-w-3xl">
      <h2 className="text-2xl font-semibold mb-4">Dispute #{disputeId}</h2>
      <div className="bg-white p-6 rounded shadow-sm space-y-3">
        <div><strong>Status:</strong> {dispute.status || dispute.Status || 'N/A'}</div>
        <div><strong>Listing:</strong> {dispute.listingId || dispute.ListingId || 'N/A'}</div>
        <div><strong>Order:</strong> {dispute.orderId || dispute.OrderId || 'N/A'}</div>
        <div><strong>Reason:</strong>
          <div className="mt-2 p-3 bg-gray-50 rounded">{dispute.reason || dispute.Reason || 'N/A'}</div>
        </div>
        <div>
          <strong>Messages / Threads:</strong>
          <pre className="mt-2 bg-gray-50 p-3 rounded max-h-64 overflow-auto">{JSON.stringify(dispute.messages || dispute.threads || [], null, 2)}</pre>
        </div>
        <div className="pt-4">
          <Link to="/disputes" className="text-blue-600">Back to disputes</Link>
        </div>
      </div>
    </div>
  );
}
