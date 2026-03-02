import React, { useEffect, useState, useCallback, useRef } from 'react';
import { Link } from 'react-router-dom';
import DisputeService from '../../services/DisputeService';
import Notice from '../../components/Common/CustomNotification';
import { getStorage } from '../../lib/storage';
import STORAGE from '../../lib/storage';
import { FaSpinner, FaExclamationCircle, FaClock, FaSearch, FaChevronDown, FaChevronUp, FaBan } from 'react-icons/fa';
import dayjs from 'dayjs';
import { motion } from 'framer-motion';

const MyDisputes = () => {
  const [disputes, setDisputes] = useState([]);
  const [loading, setLoading] = useState(false);
  const [expandedDisputes, setExpandedDisputes] = useState({});
  const [searchQuery, setSearchQuery] = useState('');
  const [statusFilter, setStatusFilter] = useState('');
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
  });
  const abortControllerRef = useRef(null);
  const uploadInputRefs = useRef({});
  const [showRespondForm, setShowRespondForm] = useState({});
  const [respondMessage, setRespondMessage] = useState({});

  // Get current user ID
  const currentUserId = React.useMemo(() => {
    const userInfo = getStorage(STORAGE.USER_INFO);
    if (userInfo) {
      try {
        const parsed = typeof userInfo === "string" ? JSON.parse(userInfo) : userInfo;
        return parsed?.id ?? parsed?.Id ?? parsed?.userId ?? parsed?.UserId;
      } catch {
        return null;
      }
    }
    return null;
  }, []);

  // Fetch disputes
  const fetchDisputes = useCallback(async () => {
    if (!currentUserId) return;

    if (abortControllerRef.current) {
      try { abortControllerRef.current.abort(); } catch {}
    }
    abortControllerRef.current = new AbortController();

    setLoading(true);
    try {
      const filterParams = {
        sellerId: currentUserId,
        pageNumber: pagination.pageNumber,
        pageSize: pagination.pageSize,
      };
      if (statusFilter) filterParams.status = statusFilter;

      const result = await DisputeService.getDisputes
        ? await DisputeService.getDisputes(filterParams, abortControllerRef.current.signal)
        : null;

      // support both shapes: { items: [], totalCount } or array []
      if (Array.isArray(result)) {
        setDisputes(result);
        setPagination(prev => ({ ...prev, totalCount: result.length || 0 }));
      } else if (result && typeof result === 'object') {
        setDisputes(result.items || result.data || []);
        setPagination(prev => ({ ...prev, totalCount: result.totalCount || result.total || (result.items ? result.items.length : 0) || 0 }));
      } else {
        setDisputes([]);
        setPagination(prev => ({ ...prev, totalCount: 0 }));
        if (!DisputeService.getDisputes) {
          Notice({ msg: 'Backend endpoint getDisputes not implemented', isSuccess: false });
        }
      }
    } catch (error) {
      const isAbort = error && (error.name === 'AbortError' || error.message === 'The user aborted a request.');
      if (!isAbort) {
        console.error('Error fetching disputes:', error);
        Notice({ msg: 'Failed to load disputes', isSuccess: false });
      }
    } finally {
      setLoading(false);
    }
  }, [currentUserId, statusFilter, pagination.pageNumber, pagination.pageSize]);

  useEffect(() => {
    fetchDisputes();
    return () => {
      if (abortControllerRef.current) {
        abortControllerRef.current.abort();
      }
    };
  }, [fetchDisputes]);

  const toggleDisputeExpansion = (disputeId) => {
    setExpandedDisputes(prev => ({
      ...prev,
      [disputeId]: !prev[disputeId]
    }));
  };

  const handleCancelDispute = async (disputeId) => {
    if (!window.confirm('Are you sure you want to close this dispute? This action cannot be undone.')) return;
    if (!DisputeService.closeDispute) {
      Notice({ msg: 'closeDispute endpoint not implemented', isSuccess: false });
      return;
    }
    try {
      setLoading(true);
      await DisputeService.closeDispute(disputeId);
      Notice({ msg: 'Dispute closed successfully', isSuccess: true });
      await fetchDisputes();
    } catch (error) {
      console.error('Error closing dispute:', error);
      Notice({ msg: 'Failed to close dispute', isSuccess: false });
    } finally {
      setLoading(false);
    }
  };

  // Helper: upload evidence file(s)
  const handleUploadEvidence = (disputeId) => {
    const input = uploadInputRefs.current[disputeId];
    if (!input) {
      Notice({ msg: 'Upload input not available', isSuccess: false });
      return;
    }
    input.click();
  };

  const onFilesSelected = async (disputeId, files) => {
    if (!files || files.length === 0) return;
    if (!DisputeService.uploadEvidence) {
      Notice({ msg: 'uploadEvidence endpoint not implemented', isSuccess: false });
      return;
    }
    try {
      setLoading(true);
      const formData = new FormData();
      Array.from(files).forEach((f) => formData.append('files', f));
      const result = await DisputeService.uploadEvidence(disputeId, formData);
      const fileCount = result?.fileUrls?.length || files.length;
      Notice({ 
        msg: `Successfully uploaded ${fileCount} evidence file(s)!`, 
        isSuccess: true 
      });
      await fetchDisputes();
    } catch (err) {
      console.error('Upload evidence error', err);
      const errorMsg = err?.response?.data?.detail || err?.message || 'Failed to upload evidence';
      Notice({ msg: errorMsg, isSuccess: false });
    } finally {
      setLoading(false);
    }
  };

  // Toggle respond form
  const toggleRespondForm = (disputeId) => {
    setShowRespondForm(prev => ({
      ...prev,
      [disputeId]: !prev[disputeId]
    }));
    if (!showRespondForm[disputeId]) {
      setRespondMessage(prev => ({
        ...prev,
        [disputeId]: ''
      }));
    }
  };

  // Respond to dispute (seller reply)
  const handleRespond = async (disputeId) => {
    const message = respondMessage[disputeId]?.trim() || '';
    if (!message) {
      Notice({ msg: 'Please enter your response message', isSuccess: false });
      return;
    }
    if (!DisputeService.respondToDispute) {
      Notice({ msg: 'respondToDispute endpoint not implemented', isSuccess: false });
      return;
    }
    try {
      setLoading(true);
      await DisputeService.respondToDispute(disputeId, { message });
      Notice({ msg: 'Response sent successfully!', isSuccess: true });
      setShowRespondForm(prev => ({ ...prev, [disputeId]: false }));
      setRespondMessage(prev => ({ ...prev, [disputeId]: '' }));
      await fetchDisputes();
    } catch (err) {
      console.error('Respond error', err);
      const errorMsg = err?.response?.data?.detail || err?.message || 'Failed to send response';
      Notice({ msg: errorMsg, isSuccess: false });
    } finally {
      setLoading(false);
    }
  };

  // Escalate to platform / mediation
  const handleEscalate = async (disputeId) => {
    if (!window.confirm('Are you sure you want to escalate this dispute to platform for review? This action will change the status to "Under Review".')) return;
    if (!DisputeService.escalateDispute) {
      Notice({ msg: 'escalateDispute endpoint not implemented', isSuccess: false });
      return;
    }
    try {
      setLoading(true);
      await DisputeService.escalateDispute(disputeId);
      Notice({ msg: 'Dispute escalated successfully!', isSuccess: true });
      await fetchDisputes();
    } catch (err) {
      console.error('Escalate error', err);
      const errorMsg = err?.response?.data?.detail || err?.message || 'Failed to escalate dispute';
      Notice({ msg: errorMsg, isSuccess: false });
    } finally {
      setLoading(false);
    }
  };

  // Format date helper function
  const formatDate = (dateString) => {
    try {
      return dayjs(dateString).format('DD/MM/YYYY HH:mm');
    } catch (error) {
      return 'Invalid date';
    }
  };

  // Get status badge color helper
  const getStatusBadgeColor = (status) => {
    const statusLower = (status || "").toLowerCase();
    switch (statusLower) {
      case 'open':
        return 'bg-blue-100 text-blue-800 border-blue-300';
      case 'underreview':
      case 'under_review':
        return 'bg-amber-100 text-amber-800 border-amber-300';
      case 'resolved':
        return 'bg-emerald-100 text-emerald-800 border-emerald-300';
      case 'closed':
        return 'bg-gray-100 text-gray-700 border-gray-300';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-300';
    }
  };

  // Format status display
  const formatStatus = (status) => {
    if (!status) return "Unknown";
    const statusLower = status.toLowerCase();
    if (statusLower === "underreview") return "Under Review";
    return status.charAt(0).toUpperCase() + status.slice(1).replace(/([A-Z])/g, ' $1');
  };

  // Filter disputes by search (coerce to string before lowercasing)
  const filteredDisputes = disputes.filter(dispute => {
    if (!searchQuery) return true;
    const reason = String(dispute.reason || dispute.Reason || '');
    const listingId = String(dispute.listingId || dispute.ListingId || '');
    const searchLower = searchQuery.toLowerCase();
    return reason.toLowerCase().includes(searchLower) || listingId.toLowerCase().includes(searchLower);
  });

  return (
    <div className="container mx-auto px-4 py-8">
      <motion.div 
        initial={{ opacity: 0, y: -10 }}
        animate={{ opacity: 1, y: 0 }}
        className="flex flex-col md:flex-row md:items-center md:justify-between mb-8 gap-4"
      >
        <div className="flex items-center gap-3">
          <div className="bg-red-100 p-2.5 rounded-full">
            <FaExclamationCircle className="text-red-600 text-xl" />
          </div>
          <h1 className="text-2xl md:text-3xl font-bold text-gray-800">My Disputes</h1>
        </div>
        
        <div className="flex flex-col sm:flex-row gap-3">
          {/* Search field */}
          <div className="relative">
            <input
              type="text"
              placeholder="Search disputes..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="border border-gray-300 rounded-lg pl-10 pr-4 py-2.5 w-full focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            />
            <FaSearch className="absolute left-3.5 top-3.5 text-gray-400" />
          </div>
          
          {/* Status filter */}
          <div className="relative">
            <select
              value={statusFilter}
              onChange={(e) => setStatusFilter(e.target.value)}
              className="border border-gray-300 rounded-lg pl-4 pr-10 py-2.5 w-full appearance-none bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
              <option value="">All Statuses</option>
              <option value="Open">Open</option>
              <option value="UnderReview">Under Review</option>
              <option value="Resolved">Resolved</option>
              <option value="Closed">Closed</option>
            </select>
            <FaChevronDown className="absolute right-3.5 top-3.5 text-gray-400 pointer-events-none" />
          </div>
        </div>
      </motion.div>

      {loading ? (
        <motion.div 
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          className="flex flex-col justify-center items-center py-20"
        >
          <FaSpinner className="animate-spin text-4xl text-blue-500 mb-4" />
          <p className="text-gray-600 font-medium">Loading your disputes...</p>
        </motion.div>
      ) : filteredDisputes.length > 0 ? (
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 0.2 }}
          className="space-y-4"
        >
          {filteredDisputes.map((dispute, index) => {
            const disputeId = dispute.id ?? dispute.Id ?? dispute._id ?? `idx-${index}`;
            const status = dispute.status || dispute.Status || "";
            const statusLower = String(status).toLowerCase();
            const reason = String(dispute.reason || dispute.Reason || "");
            const listingId = String(dispute.listingId || dispute.ListingId || "");
            const createdAt = dispute.createdAt || dispute.CreatedAt;
            
            return (
              <motion.div 
                key={disputeId} 
                className="bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden"
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 0.1 * (index % 5) }}
              >
                {/* Dispute Header */}
                <div className={`flex justify-between items-center p-5 ${expandedDisputes[disputeId] ? 'bg-gray-50 border-b' : ''}`}>
                  <div className="flex flex-col gap-2">
                    <div className="flex items-center gap-2">
                      <span className={`px-3 py-1 rounded-full text-xs font-medium inline-flex items-center border ${getStatusBadgeColor(status)}`}>
                        {statusLower === 'open' && <FaExclamationCircle className="mr-1" />}
                        {(statusLower === 'underreview' || statusLower === 'under_review') && <FaClock className="mr-1" />}
                        {(statusLower === 'resolved' || statusLower === 'closed') && <span className="mr-1">✓</span>}
                        {formatStatus(status)}
                      </span>
                      <span className="text-gray-500 text-sm">
                        {formatDate(createdAt)}
                      </span>
                    </div>
                    
                    <h3 className="font-medium text-gray-900">
                      Listing ID: {listingId}
                    </h3>
                  </div>
                  <div className="flex items-center gap-3">
                    {(statusLower === 'open' || statusLower === 'underreview' || statusLower === 'under_review') && (
                      <button
                        onClick={() => handleCancelDispute(disputeId)}
                        className="text-red-600 hover:text-red-800 bg-red-50 hover:bg-red-100 transition-colors px-3 py-2 rounded-lg text-sm font-medium flex items-center gap-1"
                      >
                        <FaBan className="text-sm" /> Close
                      </button>
                    )}
                    <button 
                      onClick={() => toggleDisputeExpansion(disputeId)}
                      className="text-gray-700 hover:bg-gray-100 p-2 rounded-full transition-colors"
                    >
                      {expandedDisputes[disputeId] ? (
                        <FaChevronUp className="text-gray-500" />
                      ) : (
                        <FaChevronDown className="text-gray-500" />
                      )}
                    </button>
                  </div>
                </div>
                
                {/* Expanded Content */}
                {expandedDisputes[disputeId] && (
                  <motion.div 
                    className="p-5"
                    initial={{ opacity: 0, height: 0 }}
                    animate={{ opacity: 1, height: 'auto' }}
                    exit={{ opacity: 0, height: 0 }}
                  >
                    <div className="mb-4">
                      <h4 className="text-sm font-medium text-gray-700 mb-2">Reason:</h4>
                      <p className="bg-gray-50 p-3 rounded-lg text-gray-700 whitespace-pre-line">
                        {reason}
                      </p>
                    </div>
                    
                    <div className="mb-4">
                      <h4 className="text-sm font-medium text-gray-700 mb-2">Listing ID:</h4>
                      <p className="bg-gray-50 p-3 rounded-lg text-gray-700">
                        {listingId}
                      </p>
                    </div>
                    
                    <div className="mt-4 space-y-3">
                      <Link
                        to={`/listings/${listingId}`}
                        className="text-blue-600 hover:text-blue-800 font-medium text-sm flex items-center gap-2 w-fit"
                      >
                        View Listing Details →
                      </Link>
                      
                      {/* Seller actions - Primary action: Respond */}
                      <div className="space-y-3">
                        {!showRespondForm[disputeId] ? (
                          <button
                            onClick={() => toggleRespondForm(disputeId)}
                            className="bg-indigo-600 text-white px-4 py-2 rounded-lg hover:bg-indigo-700 transition-colors text-sm font-medium"
                          >
                            💬 Respond
                          </button>
                        ) : (
                          <div className="bg-gray-50 border border-gray-200 rounded-lg p-4 space-y-3">
                            <div>
                              <label className="block text-sm font-medium text-gray-700 mb-2">
                                Enter your response:
                              </label>
                              <textarea
                                value={respondMessage[disputeId] || ''}
                                onChange={(e) => setRespondMessage(prev => ({
                                  ...prev,
                                  [disputeId]: e.target.value
                                }))}
                                placeholder="Enter your response to this dispute..."
                                rows={4}
                                className="w-full border border-gray-300 rounded-lg px-3 py-2 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:border-transparent resize-none"
                                maxLength={2000}
                              />
                              <div className="text-xs text-gray-500 mt-1">
                                {(respondMessage[disputeId]?.length || 0)} / 2000 characters
                              </div>
                            </div>
                            <div className="flex gap-2">
                              <button
                                onClick={() => handleRespond(disputeId)}
                                disabled={loading || !respondMessage[disputeId]?.trim()}
                                className="bg-indigo-600 text-white px-4 py-2 rounded-lg hover:bg-indigo-700 transition-colors text-sm font-medium disabled:bg-gray-400 disabled:cursor-not-allowed"
                              >
                                {loading ? 'Sending...' : 'Send Response'}
                              </button>
                              <button
                                onClick={() => toggleRespondForm(disputeId)}
                                disabled={loading}
                                className="bg-gray-200 text-gray-700 px-4 py-2 rounded-lg hover:bg-gray-300 transition-colors text-sm font-medium disabled:bg-gray-100 disabled:cursor-not-allowed"
                              >
                                Cancel
                              </button>
                            </div>
                          </div>
                        )}
                      </div>
                      
                      {/* Other actions */}
                      <div className="flex flex-wrap gap-2">
                        
                        {/* Secondary actions - only show for Open/UnderReview disputes */}
                        {(statusLower === 'open' || statusLower === 'underreview' || statusLower === 'under_review') && (
                          <>
                            <button
                              onClick={() => handleUploadEvidence(disputeId)}
                              className="bg-gray-100 text-gray-800 px-4 py-2 rounded-lg hover:bg-gray-200 transition-colors text-sm"
                              title="Upload evidence (product photos, tracking, invoices...)"
                            >
                              📎 Upload Evidence
                            </button>
                            
                            <button
                              onClick={() => handleEscalate(disputeId)}
                              className="bg-yellow-500 text-white px-4 py-2 rounded-lg hover:bg-yellow-600 transition-colors text-sm"
                              title="Escalate to platform when you think the buyer is wrong"
                            >
                              ⚠️ Escalate
                            </button>
                          </>
                        )}
                      </div>
                      
                      {/* Help text for seller */}
                      {(statusLower === 'open' || statusLower === 'underreview' || statusLower === 'under_review') && (
                        <div className="bg-blue-50 border border-blue-200 rounded-lg p-3 text-xs text-blue-800">
                          <strong>💡 Tips for Sellers:</strong>
                          <ul className="mt-1 ml-4 list-disc space-y-1">
                            <li><strong>Respond:</strong> Explain and communicate with the buyer about the issue</li>
                            <li><strong>Upload Evidence:</strong> Upload photos of shipped products, tracking numbers, shipping invoices to prove your case</li>
                            <li><strong>Escalate:</strong> When you think the buyer is wrong and need platform intervention</li>
                          </ul>
                        </div>
                      )}
                      
                      {/* show buyer info */}
                      <div className="text-sm text-gray-600 border-t pt-2">
                        <span className="font-medium">Raised by:</span> {dispute.buyerName || (dispute.buyer && dispute.buyer.name) || dispute.raisedByUsername || dispute.raisedByFullName || 'Unknown'} 
                        {dispute.orderId || dispute.OrderId ? ` — Order: ${dispute.orderId || dispute.OrderId}` : ''}
                      </div>
                    </div>
                    
                    {/* hidden file input */}
                    <input
                      ref={(el) => (uploadInputRefs.current[disputeId] = el)}
                      type="file"
                      multiple
                      onChange={(e) => onFilesSelected(disputeId, e.target.files)}
                      style={{ display: 'none' }}
                      accept="image/*,application/pdf"
                    />
                  </motion.div>
                )}
              </motion.div>
            );
          })}
        </motion.div>
      ) : (
        <motion.div 
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          className="bg-white border rounded-xl p-12 text-center shadow-sm max-w-lg mx-auto"
        >
          <div className="bg-gray-100 w-20 h-20 rounded-full flex items-center justify-center mx-auto mb-6">
            <FaExclamationCircle className="text-gray-400 text-3xl" />
          </div>
          <h2 className="text-2xl font-bold text-gray-800 mb-4">No Disputes Found</h2>
          <p className="text-gray-600 mb-6">
            {searchQuery || statusFilter ? 
              'No disputes match your current filters. Try changing your search criteria.' :
              'No disputes reported against your listings.'}
          </p>
          {/* intentionally removed order-history link for seller disputes */}
        </motion.div>
      )}
    </div>
  );
};

export default MyDisputes;



