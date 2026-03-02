import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import DisputeService from '../../services/DisputeService';
import Notice from '../../components/Common/CustomNotification';

export default function CreateDispute() {
  const [listingId, setListingId] = useState('');
  const [reason, setReason] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!listingId || !reason) {
      Notice({ msg: 'Listing ID and reason are required', isSuccess: false });
      return;
    }
    try {
      setSubmitting(true);
      // Convert listingId to Guid format if needed
      const payload = {
        listingId: listingId.trim(),
        reason: reason.trim()
      };
      await DisputeService.createDispute(payload);
      Notice({ msg: 'Dispute created successfully', isSuccess: true });
      navigate('/disputes');
    } catch (err) {
      console.error(err);
      const errorMessage = err?.response?.data?.title || err?.message || 'Failed to create dispute';
      Notice({ msg: errorMessage, isSuccess: false });
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-8 max-w-2xl">
      <h2 className="text-2xl font-semibold mb-4">Create Dispute</h2>
      <form onSubmit={handleSubmit} className="space-y-4 bg-white p-6 rounded-lg shadow-sm">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Listing ID *</label>
          <input 
            type="text"
            value={listingId} 
            onChange={(e) => setListingId(e.target.value)} 
            className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent" 
            placeholder="Enter listing ID (GUID)"
            required
          />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">Reason *</label>
          <textarea 
            value={reason} 
            onChange={(e) => setReason(e.target.value)} 
            className="mt-1 block w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent" 
            rows={5}
            placeholder="Describe the reason for this dispute..."
            required
          />
        </div>
        <div className="flex gap-2">
          <button type="submit" disabled={submitting} className="bg-blue-600 text-white px-4 py-2 rounded">
            {submitting ? 'Creating...' : 'Create Dispute'}
          </button>
        </div>
      </form>
    </div>
  );
}
