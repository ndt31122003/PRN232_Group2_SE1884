import React from 'react';
import { useParams, Link } from 'react-router-dom';

export default function DisputeDetailTest() {
  const { disputeId } = useParams();
  
  console.log('[TEST] DisputeDetailTest rendered, disputeId:', disputeId);
  
  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-2xl font-bold mb-4">TEST: Dispute Detail</h1>
      <p>Dispute ID: {disputeId}</p>
      <Link to="/disputes" className="text-blue-600 hover:underline mt-4 inline-block">
        &larr; Back to disputes
      </Link>
    </div>
  );
}
