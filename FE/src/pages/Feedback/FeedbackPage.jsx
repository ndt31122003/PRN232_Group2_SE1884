import React, { useMemo, useState, useEffect, useCallback, useRef } from "react";
import dayjs from "dayjs";
import ReviewService from "../../services/ReviewService";
import Notice from "../../components/Common/CustomNotification";
import { getStorage } from "../../lib/storage";
import STORAGE from "../../lib/storage";
import "./FeedbackPage.scss";

const FEEDBACK_TABS = [
  { id: "received", label: "All received Feedback" },
  { id: "buyer", label: "Received as buyer" },
  { id: "seller", label: "Received as seller" },
];

const PERIOD_OPTIONS = [
  { value: "all", label: "All" },
  { value: "1m", label: "1 month" },
  { value: "6m", label: "6 months" },
  { value: "12m", label: "12 months" },
];

const SORT_OPTIONS = [
  { value: "recent", label: "Most recent" },
  { value: "oldest", label: "Oldest" },
];

const FeedbackPage = () => {
  const [activeTab, setActiveTab] = useState("buyer");
  const [period, setPeriod] = useState("all");
  const [sortBy, setSortBy] = useState("recent");
  const [searchTerm, setSearchTerm] = useState("");
  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(false);
  const [stats, setStats] = useState({
    positive: { "1m": 0, "6m": 0, "12m": 0 },
    neutral: { "1m": 0, "6m": 0, "12m": 0 },
    negative: { "1m": 0, "6m": 0, "12m": 0 },
  });
  const [replyText, setReplyText] = useState({});
  const [replyingId, setReplyingId] = useState(null);
  const [showRevisionDialog, setShowRevisionDialog] = useState(null);
  const [requestingRevisionId, setRequestingRevisionId] = useState(null);
  const [pagination, setPagination] = useState({
    pageNumber: 1,
    pageSize: 20,
    totalCount: 0,
  });
  const abortControllerRef = useRef(null);

  const currentUserInfo = useMemo(() => {
    const userInfo = getStorage(STORAGE.USER_INFO);
    if (!userInfo) return {};
    try {
      const parsed = typeof userInfo === "string" ? JSON.parse(userInfo) : userInfo;
      const username = parsed?.username || parsed?.Username || parsed?.userName || parsed?.UserName || parsed?.email || parsed?.Email || "Member";
      const memberSince = parsed?.createdAt || parsed?.CreatedAt || parsed?.memberSince || parsed?.MemberSince || null;
      const country = parsed?.country || parsed?.Country || parsed?.region || parsed?.Region || "";
      const feedbackScore = parsed?.feedbackScore ?? parsed?.FeedbackScore ?? 0;
      return { username, memberSince, country, feedbackScore };
    } catch {
      return {};
    }
  }, []);

  const tabDescription = useMemo(() => {
    switch (activeTab) {
      case "buyer":
        return "Feedback buyers have left for this seller.";
      case "seller":
        return "Feedback this seller has received for selling.";
      default:
        return "All feedback received by this member.";
    }
  }, [activeTab]);

  // Get current user ID
  const currentUserId = useMemo(() => {
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

  // Calculate date range from period
  const getDateRange = useCallback((periodValue) => {
    if (periodValue === "all") return { fromDate: null, toDate: null };
    
    const now = dayjs();
    const months = periodValue === "1m" ? 1 : periodValue === "6m" ? 6 : 12;
    const fromDate = now.subtract(months, "month").startOf("day");
    
    return { fromDate: fromDate.toISOString(), toDate: now.toISOString() };
  }, []);

  // Fetch reviews
  const fetchReviews = useCallback(async () => {
    if (!currentUserId) return;

    // Cancel previous request
    if (abortControllerRef.current) {
      abortControllerRef.current.abort();
    }
    abortControllerRef.current = new AbortController();

    setLoading(true);
    try {
      const { fromDate, toDate } = getDateRange(period);
      
      const filterParams = {
        pageNumber: pagination.pageNumber,
        pageSize: pagination.pageSize,
      };

      // Optional: accept listingId from querystring (?listingId=<id>)
      try {
        const params = new URLSearchParams(window.location.search);
        const qsListingId = params.get("listingId");
        if (qsListingId) {
          filterParams.listingId = qsListingId;
        }
      } catch {}

      // Filter by tab
      if (activeTab === "buyer") {
        filterParams.reviewerId = currentUserId;
      } else if (activeTab === "seller") {
        // Only the seller tab scopes to listings created by current user
        filterParams.sellerId = currentUserId;
      }

      if (fromDate) filterParams.fromDate = fromDate;
      if (toDate) filterParams.toDate = toDate;

      const result = await ReviewService.getReviews(filterParams, abortControllerRef.current.signal);
      
      setReviews(result.items || []);
      setPagination(prev => ({
        ...prev,
        totalCount: result.totalCount || 0,
      }));

      // Calculate stats
      const allReviews = result.items || [];
      const calculateStats = (months) => {
        const cutoffDate = dayjs().subtract(months, "month").startOf("day").toDate();
        const filtered = allReviews.filter(r => {
          const reviewDate = new Date(r.createdAt || r.CreatedAt);
          return reviewDate >= cutoffDate;
        });
        
        return {
          positive: filtered.filter(r => {
            const ratingType = r.ratingType || r.RatingType || "";
            return ratingType === "Positive" || (r.rating || r.Rating) >= 4;
          }).length,
          neutral: filtered.filter(r => {
            const ratingType = r.ratingType || r.RatingType || "";
            return ratingType === "Neutral" || (r.rating || r.Rating) === 3;
          }).length,
          negative: filtered.filter(r => {
            const ratingType = r.ratingType || r.RatingType || "";
            return ratingType === "Negative" || (r.rating || r.Rating) <= 2;
          }).length,
        };
      };

      setStats({
        positive: { "1m": calculateStats(1).positive, "6m": calculateStats(6).positive, "12m": calculateStats(12).positive },
        neutral: { "1m": calculateStats(1).neutral, "6m": calculateStats(6).neutral, "12m": calculateStats(12).neutral },
        negative: { "1m": calculateStats(1).negative, "6m": calculateStats(6).negative, "12m": calculateStats(12).negative },
      });
    } catch (error) {
      if (error.name !== "AbortError") {
        console.error("Error fetching reviews:", error);
      }
    } finally {
      setLoading(false);
    }
  }, [currentUserId, activeTab, period, pagination.pageNumber, pagination.pageSize, getDateRange]);

  useEffect(() => {
    fetchReviews();
    return () => {
      if (abortControllerRef.current) {
        abortControllerRef.current.abort();
      }
    };
  }, [fetchReviews]);

  // Filter and sort reviews
  const filteredReviews = useMemo(() => {
    let filtered = [...reviews];

    // Search filter
    if (searchTerm) {
      const searchLower = searchTerm.toLowerCase();
      filtered = filtered.filter(review => {
        const comment = (review.comment || review.Comment || "").toLowerCase();
        const reviewerName = (review.reviewerUsername || review.ReviewerUsername || "").toLowerCase();
        return comment.includes(searchLower) || reviewerName.includes(searchLower);
      });
    }

    // Sort
    filtered.sort((a, b) => {
      const dateA = new Date(a.createdAt || a.CreatedAt);
      const dateB = new Date(b.createdAt || b.CreatedAt);
      return sortBy === "recent" ? dateB - dateA : dateA - dateB;
    });

    return filtered;
  }, [reviews, searchTerm, sortBy]);

  const handleReplyChange = (id, value) => {
    setReplyText((prev) => ({ ...prev, [id]: value }));
  };

  const handleReplySubmit = async (reviewId) => {
    const reply = replyText[reviewId]?.trim();
    if (!reply) {
      Notice({ msg: "Please enter a reply", isSuccess: false });
      return;
    }

    setReplyingId(reviewId);
    try {
      await ReviewService.replyToReview(reviewId, reply);
      Notice({ msg: "Reply submitted successfully", isSuccess: true });
      setReplyText((prev) => ({ ...prev, [reviewId]: "" }));
      await fetchReviews();
    } catch (error) {
      console.error("Error replying to review:", error);
      Notice({ msg: "Failed to submit reply", isSuccess: false });
    } finally {
      setReplyingId(null);
    }
  };

  const handleRequestRevision = (id) => {
    setShowRevisionDialog(id);
  };

  const confirmRequestRevision = async (reviewId) => {
    setRequestingRevisionId(reviewId);
    try {
      await ReviewService.requestReviewRevision(reviewId);
      Notice({ msg: "Revision request sent successfully", isSuccess: true });
      setShowRevisionDialog(null);
      await fetchReviews();
    } catch (error) {
      console.error("Error requesting revision:", error);
      Notice({ msg: "Failed to send revision request", isSuccess: false });
    } finally {
      setRequestingRevisionId(null);
    }
  };

  return (
    <div className="feedback-page">
      {/* --- HEADER --- */}
      <header className="feedback-page__header">
        <div className="feedback-page__profile">
          <div className="feedback-page__avatar" aria-hidden="true">
            <span>🙂</span>
          </div>
          <div>
            <button type="button" className="feedback-page__username">
              {currentUserInfo.username || "Member"} ({currentUserInfo.feedbackScore ?? 0})
            </button>
            {currentUserInfo.memberSince && (
              <p className="feedback-page__meta">
                Member since: {dayjs(currentUserInfo.memberSince).format("MMM-DD-YY")} {currentUserInfo.country ? `in ${currentUserInfo.country}` : ""}
              </p>
            )}
          </div>
        </div>
      </header>

      {/* --- TABS --- */}
      <section className="feedback-page__tabs" aria-label="Feedback filters">
        <div className="feedback-page__tabs-list" role="tablist">
          {FEEDBACK_TABS.map((tab) => (
            <button
              key={tab.id}
              type="button"
              className={`feedback-page__tab ${activeTab === tab.id ? "is-active" : ""}`}
              onClick={() => setActiveTab(tab.id)}
              role="tab"
              aria-selected={activeTab === tab.id}
            >
              {tab.label}
            </button>
          ))}
        </div>
        <div className="feedback-page__tab-description">{tabDescription}</div>
      </section>

      {/* --- FEEDBACK RATINGS --- */}
      <section className="feedback-page__ratings">
        <div className="feedback-page__card">
          <h3>Feedback ratings</h3>
          <table>
            <thead>
              <tr>
                <th></th>
                <th>1 month</th>
                <th>6 months</th>
                <th>12 months</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td className="is-positive">Positive</td>
                <td>{stats.positive["1m"]}</td>
                <td>{stats.positive["6m"]}</td>
                <td>{stats.positive["12m"]}</td>
              </tr>
              <tr>
                <td className="is-neutral">Neutral</td>
                <td>{stats.neutral["1m"]}</td>
                <td>{stats.neutral["6m"]}</td>
                <td>{stats.neutral["12m"]}</td>
              </tr>
              <tr>
                <td className="is-negative">Negative</td>
                <td>{stats.negative["1m"]}</td>
                <td>{stats.negative["6m"]}</td>
                <td>{stats.negative["12m"]}</td>
              </tr>
            </tbody>
          </table>
        </div>

        <div className="feedback-page__card">
          <h3>Detailed seller ratings</h3>
          {(() => {
            // Count seller ratings (only from seller tab or all reviews if seller tab is active)
            const sellerRatings = activeTab === "seller" 
              ? reviews.filter(r => {
                  // Only count reviews where current user is the seller
                  const sellerId = r.sellerId || r.SellerId;
                  return sellerId === currentUserId;
                })
              : activeTab === "received"
              ? reviews.filter(r => {
                  // Count all reviews received as seller
                  const sellerId = r.sellerId || r.SellerId;
                  return sellerId === currentUserId;
                })
              : [];
            
            const sellerRatingsCount = sellerRatings.length;
            const requiredCount = 10;
            const remaining = Math.max(0, requiredCount - sellerRatingsCount);

            if (sellerRatingsCount >= requiredCount) {
              // Show detailed ratings when available
              return (
                <div>
                  <p className="feedback-page__info">
                    Based on {sellerRatingsCount} detailed seller ratings
                  </p>
                  {/* TODO: Add detailed rating breakdown here when backend provides this data */}
                  <p className="feedback-page__empty" style={{ fontStyle: 'italic', color: '#666' }}>
                    Detailed rating breakdown will be displayed here.
                  </p>
                </div>
              );
            } else {
              return (
                <p className="feedback-page__empty">
                  This information will be available when this member receives at
                  least {requiredCount} detailed seller ratings. 
                  {sellerRatingsCount > 0 && (
                    <span style={{ display: 'block', marginTop: '8px', fontSize: '0.9em', color: '#666' }}>
                      ({sellerRatingsCount} of {requiredCount} received{remaining > 0 ? `, ${remaining} more needed` : ''})
                    </span>
                  )}
                </p>
              );
            }
          })()}
        </div>
      </section>

      {/* --- FILTERS --- */}
      <section className="feedback-page__filters" aria-label="Feedback filters">
        <div className="feedback-page__search">
          <input
            type="text"
            placeholder="Search feedback..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>

        <div className="feedback-page__select">
          <label>Period</label>
          <select value={period} onChange={(e) => setPeriod(e.target.value)}>
            {PERIOD_OPTIONS.map((opt) => (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ))}
          </select>
        </div>

        <div className="feedback-page__select">
          <label>Sort by</label>
          <select value={sortBy} onChange={(e) => setSortBy(e.target.value)}>
            {SORT_OPTIONS.map((opt) => (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ))}
          </select>
        </div>

        <div className="feedback-page__action">
          <a
            href="/disputes"
            className="feedback-page__support-link"
            style={{
              display: 'inline-flex',
              alignItems: 'center',
              gap: '0.5rem',
              padding: '0.625rem 1rem',
              backgroundColor: '#dc2626',
              color: 'white',
              borderRadius: '0.5rem',
              textDecoration: 'none',
              fontSize: '0.875rem',
              fontWeight: '500',
              transition: 'background-color 0.2s',
              whiteSpace: 'nowrap',
              marginRight: '0.5rem'
            }}
            onMouseEnter={(e) => e.target.style.backgroundColor = '#b91c1c'}
            onMouseLeave={(e) => e.target.style.backgroundColor = '#dc2626'}
          >
            <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
              <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10h8v-2h-8c-4.41 0-8-3.59-8-8s3.59-8 8-8 8 3.59 8 8v1h2v-1c0-5.52-4.48-10-10-10zm1 5h-2v6h6v-2h-4z"/>
            </svg>
            Seller Disputes
          </a>
          <a 
            href="/support-tickets" 
            className="feedback-page__support-link"
            style={{
              display: 'inline-flex',
              alignItems: 'center',
              gap: '0.5rem',
              padding: '0.625rem 1rem',
              backgroundColor: '#2563eb',
              color: 'white',
              borderRadius: '0.5rem',
              textDecoration: 'none',
              fontSize: '0.875rem',
              fontWeight: '500',
              transition: 'background-color 0.2s',
              whiteSpace: 'nowrap'
            }}
            onMouseEnter={(e) => e.target.style.backgroundColor = '#1d4ed8'}
            onMouseLeave={(e) => e.target.style.backgroundColor = '#2563eb'}
          >
            <svg width="16" height="16" viewBox="0 0 24 24" fill="currentColor">
              <path d="M12 2C6.48 2 2 6.48 2 12s4.48 10 10 10 10-4.48 10-10S17.52 2 12 2zm1 15h-2v-2h2v2zm0-4h-2V7h2v6z"/>
            </svg>
            Support Tickets
          </a>
        </div>
      </section>

      {/* --- FEEDBACK LIST --- */}
      <section className="feedback-page__results">
        <header>
          <h2>{tabDescription}</h2>
        </header>

        {loading ? (
          <p className="feedback-page__empty">Loading reviews...</p>
        ) : filteredReviews.length === 0 ? (
          <p className="feedback-page__empty">No reviews available.</p>
        ) : (
          <table className="feedback-page__table">
            <thead>
              <tr>
                <th>Rating</th>
                <th>Feedback</th>
                <th>From</th>
                <th>When</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {filteredReviews.map((review) => {
                const reviewId = review.id || review.Id;
                const rating = review.rating || review.Rating || 0;
                const comment = review.comment || review.Comment || "";
                const reply = review.reply || review.Reply;
                const repliedAt = review.repliedAt || review.RepliedAt;
                const createdAt = review.createdAt || review.CreatedAt;
                const reviewerName = review.reviewerUsername || review.ReviewerUsername || review.reviewerId || review.ReviewerId || "Unknown";
                const revisionStatus = review.revisionStatus || review.RevisionStatus || "None";
                
                return (
                  <tr key={reviewId}>
                    <td>
                      {"⭐".repeat(rating)}
                      <span style={{ color: "#ccc" }}>
                        {"☆".repeat(5 - rating)}
                      </span>
                    </td>
                    <td>
                      <p>{comment}</p>
                      {reply && (
                        <div className="feedback-reply">
                          <strong>Seller reply:</strong> {reply}
                          {repliedAt && (
                            <span className="feedback-reply-date">
                              {" "}({dayjs(repliedAt).format("MMM DD, YYYY")})
                            </span>
                          )}
                        </div>
                      )}

                      {!reply && activeTab === "seller" && (
                        <div className="feedback-reply-form">
                          <textarea
                            placeholder="Write a reply..."
                            value={replyText[reviewId] || ""}
                            onChange={(e) =>
                              handleReplyChange(reviewId, e.target.value)
                            }
                            disabled={replyingId === reviewId}
                          />
                          <button
                            type="button"
                            onClick={() => handleReplySubmit(reviewId)}
                            disabled={replyingId === reviewId}
                          >
                            {replyingId === reviewId ? "Submitting..." : "Reply"}
                          </button>
                        </div>
                      )}
                      
                      {revisionStatus !== "None" && (
                        <div className="feedback-revision-status">
                          <span>Revision Status: {revisionStatus}</span>
                        </div>
                      )}
                    </td>
                    <td>{reviewerName}</td>
                    <td>{createdAt ? dayjs(createdAt).format("MMM DD, YYYY") : "N/A"}</td>
                    <td>
                      {activeTab === "seller" && revisionStatus === "None" && (
                        <button
                          type="button"
                          onClick={() => handleRequestRevision(reviewId)}
                          disabled={requestingRevisionId === reviewId}
                        >
                          {requestingRevisionId === reviewId ? "Requesting..." : "Request Revision"}
                        </button>
                      )}
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        )}
      </section>

      {/* --- REVISION MODAL --- */}
      {showRevisionDialog && (
        <div className="feedback-revision-modal">
          <div className="modal-content">
            <h3>Request Feedback Revision</h3>
            <p>
              Are you sure you want to send a request for the buyer to revise
              this feedback?
            </p>
            <div className="modal-actions">
              <button 
                onClick={() => confirmRequestRevision(showRevisionDialog)}
                disabled={requestingRevisionId === showRevisionDialog}
              >
                {requestingRevisionId === showRevisionDialog ? "Sending..." : "Confirm"}
              </button>
              <button 
                onClick={() => setShowRevisionDialog(null)}
                disabled={requestingRevisionId === showRevisionDialog}
              >
                Cancel
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default FeedbackPage;
