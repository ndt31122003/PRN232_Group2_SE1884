import React, { useEffect, useState, useCallback, useRef } from 'react';
import { Link } from 'react-router-dom';
import SupportTicketService from '../../services/SupportTicketService';
import Notice from '../../components/Common/CustomNotification';
import { FaSpinner, FaLifeRing, FaSearch, FaChevronDown, FaChevronUp, FaPlus } from 'react-icons/fa';
import dayjs from 'dayjs';
import { motion } from 'framer-motion';

const MySupportTickets = () => {
  const [tickets, setTickets] = useState([]);
  const [loading, setLoading] = useState(false);
  const [expandedTickets, setExpandedTickets] = useState({});
  const [searchQuery, setSearchQuery] = useState('');
  const [statusFilter, setStatusFilter] = useState('');
  const [categoryFilter, setCategoryFilter] = useState('');
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
  });
  const abortControllerRef = useRef(null);

  const fetchTickets = useCallback(async () => {
    if (abortControllerRef.current) {
      try { abortControllerRef.current.abort(); } catch {}
    }
    abortControllerRef.current = new AbortController();

    setLoading(true);
    try {
      const filterParams = {
        pageNumber: pagination.pageNumber,
        pageSize: pagination.pageSize,
        status: statusFilter || undefined,
        category: categoryFilter || undefined,
      };

      const result = await SupportTicketService.getTickets(filterParams, abortControllerRef.current.signal);
      
      setTickets(result.items || []);
      setPagination(prev => ({ 
        ...prev, 
        totalCount: result.totalCount || 0 
      }));
    } catch (error) {
      const isAbort = error && (
        error.name === 'AbortError' || 
        error.name === 'CanceledError' ||
        error.message === 'The user aborted a request.' ||
        (error.code === 'ERR_CANCELED')
      );
      
      if (!isAbort) {
        console.error('Error fetching tickets:', error);
        Notice({ msg: 'Failed to load support tickets', isSuccess: false });
      }
    } finally {
      setLoading(false);
    }
  }, [statusFilter, categoryFilter, pagination.pageNumber, pagination.pageSize]);

  useEffect(() => {
    fetchTickets();
    return () => {
      if (abortControllerRef.current) {
        abortControllerRef.current.abort();
      }
    };
  }, [fetchTickets]);

  const toggleTicketExpansion = (ticketId) => {
    setExpandedTickets(prev => ({
      ...prev,
      [ticketId]: !prev[ticketId]
    }));
  };

  const handleCloseTicket = async (ticketId) => {
    if (!window.confirm('Are you sure you want to close this ticket? This action cannot be undone.')) return;
    
    try {
      setLoading(true);
      await SupportTicketService.closeTicket(ticketId);
      Notice({ msg: 'Ticket closed successfully', isSuccess: true });
      await fetchTickets();
    } catch (error) {
      console.error('Error closing ticket:', error);
      Notice({ msg: 'Failed to close ticket', isSuccess: false });
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateString) => {
    try {
      return dayjs(dateString).format('DD/MM/YYYY HH:mm');
    } catch (error) {
      return 'Invalid date';
    }
  };

  const getStatusBadgeColor = (status) => {
    const statusLower = (status || "").toLowerCase();
    switch (statusLower) {
      case 'open':
        return 'bg-blue-100 text-blue-800 border-blue-300';
      case 'pending':
        return 'bg-amber-100 text-amber-800 border-amber-300';
      case 'resolved':
        return 'bg-emerald-100 text-emerald-800 border-emerald-300';
      case 'closed':
        return 'bg-gray-100 text-gray-700 border-gray-300';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-300';
    }
  };

  const formatStatus = (status) => {
    if (!status) return "Unknown";
    return status.charAt(0).toUpperCase() + status.slice(1);
  };

  const filteredTickets = tickets.filter(ticket => {
    if (!searchQuery) return true;
    const subject = String(ticket.subject || ticket.Subject || '');
    const category = String(ticket.category || ticket.Category || '');
    const searchLower = searchQuery.toLowerCase();
    return subject.toLowerCase().includes(searchLower) || category.toLowerCase().includes(searchLower);
  });

  return (
    <div className="container mx-auto px-4 py-8">
      <motion.div 
        initial={{ opacity: 0, y: -10 }}
        animate={{ opacity: 1, y: 0 }}
        className="flex flex-col md:flex-row md:items-center md:justify-between mb-8 gap-4"
      >
        <div className="flex items-center gap-3">
          <div className="bg-blue-100 p-2.5 rounded-full">
            <FaLifeRing className="text-blue-600 text-xl" />
          </div>
          <h1 className="text-2xl md:text-3xl font-bold text-gray-800">My Support Tickets</h1>
        </div>
        
        <Link
          to="/support-tickets/create"
          className="bg-blue-600 text-white px-4 py-2.5 rounded-lg hover:bg-blue-700 transition-colors flex items-center gap-2 justify-center"
        >
          <FaPlus /> Create New Ticket
        </Link>
      </motion.div>

      <motion.div
        initial={{ opacity: 0 }}
        animate={{ opacity: 1 }}
        transition={{ delay: 0.1 }}
        className="bg-white rounded-xl shadow-sm border border-gray-200 p-4 mb-6"
      >
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          {/* Search */}
          <div className="relative">
            <input
              type="text"
              placeholder="Search tickets..."
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
              <option value="Pending">Pending</option>
              <option value="Resolved">Resolved</option>
              <option value="Closed">Closed</option>
            </select>
            <FaChevronDown className="absolute right-3.5 top-3.5 text-gray-400 pointer-events-none" />
          </div>

          {/* Category filter */}
          <div className="relative">
            <select
              value={categoryFilter}
              onChange={(e) => setCategoryFilter(e.target.value)}
              className="border border-gray-300 rounded-lg pl-4 pr-10 py-2.5 w-full appearance-none bg-white focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            >
              <option value="">All Categories</option>
              <option value="Account Issues">Account Issues</option>
              <option value="Payment Problems">Payment Problems</option>
              <option value="Technical Support">Technical Support</option>
              <option value="Listing Issues">Listing Issues</option>
              <option value="Shipping Questions">Shipping Questions</option>
              <option value="Policy Questions">Policy Questions</option>
              <option value="Other">Other</option>
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
          <p className="text-gray-600 font-medium">Loading your tickets...</p>
        </motion.div>
      ) : filteredTickets.length > 0 ? (
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 0.2 }}
          className="space-y-4"
        >
          {filteredTickets.map((ticket, index) => {
            const ticketId = ticket.id ?? ticket.Id;
            const status = ticket.status || ticket.Status || "";
            const statusLower = String(status).toLowerCase();
            const subject = String(ticket.subject || ticket.Subject || "");
            const category = String(ticket.category || ticket.Category || "");
            const message = String(ticket.message || ticket.Message || "");
            const createdAt = ticket.createdAt || ticket.CreatedAt;
            
            return (
              <motion.div 
                key={ticketId} 
                className="bg-white rounded-xl border border-gray-200 shadow-sm overflow-hidden"
                initial={{ opacity: 0, y: 20 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 0.1 * (index % 5) }}
              >
                <div className={`flex justify-between items-center p-5 ${expandedTickets[ticketId] ? 'bg-gray-50 border-b' : ''}`}>
                  <div className="flex flex-col gap-2 flex-1">
                    <div className="flex items-center gap-2 flex-wrap">
                      <span className={`px-3 py-1 rounded-full text-xs font-medium inline-flex items-center border ${getStatusBadgeColor(status)}`}>
                        {formatStatus(status)}
                      </span>
                      <span className="px-3 py-1 rounded-full text-xs font-medium bg-purple-100 text-purple-800 border border-purple-300">
                        {category}
                      </span>
                      <span className="text-gray-500 text-sm">
                        {formatDate(createdAt)}
                      </span>
                    </div>
                    
                    <h3 className="font-semibold text-gray-900 text-lg">
                      {subject}
                    </h3>
                  </div>
                  <div className="flex items-center gap-3">
                    {statusLower !== 'closed' && (
                      <button
                        onClick={() => handleCloseTicket(ticketId)}
                        className="text-red-600 hover:text-red-800 bg-red-50 hover:bg-red-100 transition-colors px-3 py-2 rounded-lg text-sm font-medium"
                      >
                        Close
                      </button>
                    )}
                    <button 
                      onClick={() => toggleTicketExpansion(ticketId)}
                      className="text-gray-700 hover:bg-gray-100 p-2 rounded-full transition-colors"
                    >
                      {expandedTickets[ticketId] ? (
                        <FaChevronUp className="text-gray-500" />
                      ) : (
                        <FaChevronDown className="text-gray-500" />
                      )}
                    </button>
                  </div>
                </div>
                
                {expandedTickets[ticketId] && (
                  <motion.div 
                    className="p-5"
                    initial={{ opacity: 0, height: 0 }}
                    animate={{ opacity: 1, height: 'auto' }}
                    exit={{ opacity: 0, height: 0 }}
                  >
                    <div className="mb-4">
                      <h4 className="text-sm font-medium text-gray-700 mb-2">Message:</h4>
                      <p className="bg-gray-50 p-4 rounded-lg text-gray-700 whitespace-pre-line">
                        {message}
                      </p>
                    </div>
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
            <FaLifeRing className="text-gray-400 text-3xl" />
          </div>
          <h2 className="text-2xl font-bold text-gray-800 mb-4">No Tickets Found</h2>
          <p className="text-gray-600 mb-6">
            {searchQuery || statusFilter || categoryFilter ? 
              'No tickets match your current filters. Try changing your search criteria.' :
              'You haven\'t created any support tickets yet.'}
          </p>
          <Link
            to="/support-tickets/create"
            className="inline-flex items-center gap-2 bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition-colors font-medium"
          >
            <FaPlus /> Create Your First Ticket
          </Link>
        </motion.div>
      )}
    </div>
  );
};

export default MySupportTickets;
