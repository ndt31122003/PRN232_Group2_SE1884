import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import SupportTicketService from '../../services/SupportTicketService';
import Notice from '../../components/Common/CustomNotification';
import { FaLifeRing, FaArrowLeft, FaSpinner, FaPaperclip, FaTimes } from 'react-icons/fa';
import { motion } from 'framer-motion';

const CreateSupportTicket = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [formData, setFormData] = useState({
    category: '',
    subject: '',
    message: '',
  });
  const [files, setFiles] = useState([]);
  const [errors, setErrors] = useState({});

  const categories = [
    'Account Issues',
    'Payment Problems',
    'Technical Support',
    'Listing Issues',
    'Shipping Questions',
    'Policy Questions',
    'Other',
  ];

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
    // Clear error when user starts typing
    if (errors[name]) {
      setErrors(prev => ({
        ...prev,
        [name]: ''
      }));
    }
  };

  const handleFileChange = (e) => {
    const selectedFiles = Array.from(e.target.files);
    
    // Validate file size (max 10MB per file)
    const maxSize = 10 * 1024 * 1024; // 10MB
    const invalidFiles = selectedFiles.filter(file => file.size > maxSize);
    
    if (invalidFiles.length > 0) {
      Notice({ 
        msg: `Some files are too large. Maximum file size is 10MB.`, 
        isSuccess: false 
      });
      return;
    }

    // Add new files to existing files
    setFiles(prev => [...prev, ...selectedFiles]);
  };

  const removeFile = (index) => {
    setFiles(prev => prev.filter((_, i) => i !== index));
  };

  const validateForm = () => {
    const newErrors = {};

    if (!formData.category.trim()) {
      newErrors.category = 'Please select a category';
    }

    if (!formData.subject.trim()) {
      newErrors.subject = 'Subject is required';
    } else if (formData.subject.trim().length < 5) {
      newErrors.subject = 'Subject must be at least 5 characters';
    }

    if (!formData.message.trim()) {
      newErrors.message = 'Message is required';
    } else if (formData.message.trim().length < 20) {
      newErrors.message = 'Message must be at least 20 characters';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!validateForm()) {
      Notice({ msg: 'Please fix the errors in the form', isSuccess: false });
      return;
    }

    try {
      setLoading(true);

      // Create FormData
      const data = new FormData();
      data.append('Category', formData.category);
      data.append('Subject', formData.subject);
      data.append('Message', formData.message);

      // Append files
      files.forEach((file) => {
        data.append('Attachments', file);
      });

      const result = await SupportTicketService.createTicket(data);
      
      Notice({ 
        msg: 'Support ticket created successfully! Our team will review it soon.', 
        isSuccess: true 
      });

      // Navigate to tickets list
      setTimeout(() => {
        navigate('/support-tickets');
      }, 1500);

    } catch (error) {
      console.error('Error creating ticket:', error);
      const errorMsg = error?.response?.data?.detail 
        || error?.response?.data?.title
        || error?.response?.data?.message
        || error?.message 
        || 'Failed to create support ticket';
      Notice({ msg: errorMsg, isSuccess: false });
    } finally {
      setLoading(false);
    }
  };

  const formatFileSize = (bytes) => {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return Math.round(bytes / Math.pow(k, i) * 100) / 100 + ' ' + sizes[i];
  };

  return (
    <div className="container mx-auto px-4 py-8 max-w-3xl">
      <motion.div
        initial={{ opacity: 0, y: -10 }}
        animate={{ opacity: 1, y: 0 }}
      >
        <button
          onClick={() => navigate('/support-tickets')}
          className="flex items-center gap-2 text-gray-600 hover:text-gray-800 mb-6 transition-colors"
        >
          <FaArrowLeft /> Back to Tickets
        </button>

        <div className="flex items-center gap-3 mb-8">
          <div className="bg-blue-100 p-2.5 rounded-full">
            <FaLifeRing className="text-blue-600 text-xl" />
          </div>
          <h1 className="text-2xl md:text-3xl font-bold text-gray-800">Create Support Ticket</h1>
        </div>
      </motion.div>

      <motion.div
        initial={{ opacity: 0, y: 20 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ delay: 0.1 }}
        className="bg-white rounded-xl shadow-sm border border-gray-200 p-6"
      >
        <form onSubmit={handleSubmit} className="space-y-6">
          {/* Category */}
          <div>
            <label htmlFor="category" className="block text-sm font-medium text-gray-700 mb-2">
              Category <span className="text-red-500">*</span>
            </label>
            <select
              id="category"
              name="category"
              value={formData.category}
              onChange={handleInputChange}
              className={`w-full border ${errors.category ? 'border-red-500' : 'border-gray-300'} rounded-lg px-4 py-2.5 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent`}
            >
              <option value="">Select a category</option>
              {categories.map(cat => (
                <option key={cat} value={cat}>{cat}</option>
              ))}
            </select>
            {errors.category && (
              <p className="mt-1 text-sm text-red-500">{errors.category}</p>
            )}
          </div>

          {/* Subject */}
          <div>
            <label htmlFor="subject" className="block text-sm font-medium text-gray-700 mb-2">
              Subject <span className="text-red-500">*</span>
            </label>
            <input
              type="text"
              id="subject"
              name="subject"
              value={formData.subject}
              onChange={handleInputChange}
              placeholder="Brief description of your issue"
              maxLength={200}
              className={`w-full border ${errors.subject ? 'border-red-500' : 'border-gray-300'} rounded-lg px-4 py-2.5 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent`}
            />
            <div className="flex justify-between mt-1">
              {errors.subject ? (
                <p className="text-sm text-red-500">{errors.subject}</p>
              ) : (
                <p className="text-sm text-gray-500">Minimum 5 characters</p>
              )}
              <p className="text-sm text-gray-500">{formData.subject.length}/200</p>
            </div>
          </div>

          {/* Message */}
          <div>
            <label htmlFor="message" className="block text-sm font-medium text-gray-700 mb-2">
              Message <span className="text-red-500">*</span>
            </label>
            <textarea
              id="message"
              name="message"
              value={formData.message}
              onChange={handleInputChange}
              placeholder="Please describe your issue in detail..."
              rows={8}
              maxLength={2000}
              className={`w-full border ${errors.message ? 'border-red-500' : 'border-gray-300'} rounded-lg px-4 py-2.5 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-none`}
            />
            <div className="flex justify-between mt-1">
              {errors.message ? (
                <p className="text-sm text-red-500">{errors.message}</p>
              ) : (
                <p className="text-sm text-gray-500">Minimum 20 characters</p>
              )}
              <p className="text-sm text-gray-500">{formData.message.length}/2000</p>
            </div>
          </div>

          {/* File Attachments */}
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              Attachments (Optional)
            </label>
            <div className="border-2 border-dashed border-gray-300 rounded-lg p-6 text-center hover:border-blue-400 transition-colors">
              <input
                type="file"
                id="file-upload"
                multiple
                onChange={handleFileChange}
                className="hidden"
                accept="image/*,.pdf,.doc,.docx,.txt"
              />
              <label
                htmlFor="file-upload"
                className="cursor-pointer flex flex-col items-center"
              >
                <FaPaperclip className="text-3xl text-gray-400 mb-2" />
                <p className="text-sm text-gray-600 mb-1">
                  Click to upload or drag and drop
                </p>
                <p className="text-xs text-gray-500">
                  Images, PDF, DOC, TXT (Max 10MB per file)
                </p>
              </label>
            </div>

            {/* File List */}
            {files.length > 0 && (
              <div className="mt-4 space-y-2">
                {files.map((file, index) => (
                  <div
                    key={index}
                    className="flex items-center justify-between bg-gray-50 p-3 rounded-lg border border-gray-200"
                  >
                    <div className="flex items-center gap-3 flex-1 min-w-0">
                      <FaPaperclip className="text-gray-400 flex-shrink-0" />
                      <div className="flex-1 min-w-0">
                        <p className="text-sm font-medium text-gray-700 truncate">
                          {file.name}
                        </p>
                        <p className="text-xs text-gray-500">
                          {formatFileSize(file.size)}
                        </p>
                      </div>
                    </div>
                    <button
                      type="button"
                      onClick={() => removeFile(index)}
                      className="text-red-500 hover:text-red-700 p-1 flex-shrink-0"
                    >
                      <FaTimes />
                    </button>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* Info Box */}
          <div className="bg-blue-50 border border-blue-200 rounded-lg p-4">
            <p className="text-sm text-blue-800">
              <strong>💡 Tips:</strong> Please provide as much detail as possible to help us resolve your issue quickly. 
              Include screenshots or relevant files if applicable.
            </p>
          </div>

          {/* Submit Buttons */}
          <div className="flex gap-3 pt-4">
            <button
              type="submit"
              disabled={loading}
              className="flex-1 bg-blue-600 text-white px-6 py-3 rounded-lg hover:bg-blue-700 transition-colors font-medium disabled:bg-gray-400 disabled:cursor-not-allowed flex items-center justify-center gap-2"
            >
              {loading ? (
                <>
                  <FaSpinner className="animate-spin" />
                  Creating...
                </>
              ) : (
                'Create Ticket'
              )}
            </button>
            <button
              type="button"
              onClick={() => navigate('/support-tickets')}
              disabled={loading}
              className="px-6 py-3 border border-gray-300 text-gray-700 rounded-lg hover:bg-gray-50 transition-colors font-medium disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Cancel
            </button>
          </div>
        </form>
      </motion.div>
    </div>
  );
};

export default CreateSupportTicket;
