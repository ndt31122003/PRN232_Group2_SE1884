import React, { useEffect, useState } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import UserService from "../../services/UserService";
import Notice from "../../components/Common/CustomNotification";
import { LoadingScreen } from "../../components/LoadingScreen/LoadingScreen";
import { Star, MapPin, Calendar, Box, ThumbsUp, Medal, ShieldCheck } from "lucide-react";
import { getStoredUser } from "../../utils/auth";
import axios from "../../utils/axiosCustomize";

export default function SellerProfilePage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [seller, setSeller] = useState(null);
  const [listings, setListings] = useState([]);
  const [listingsLoading, setListingsLoading] = useState(false);
  const [loading, setLoading] = useState(true);
  const currentUser = getStoredUser();
  const isOwnProfile = currentUser?.id === id;

  useEffect(() => {
    const fetchSeller = async () => {
      try {
        setLoading(true);
        const data = await UserService.getById(id);
        if (data) {
          setSeller(data);
        } else {
          Notice({ msg: "Seller not found", isSuccess: false });
        }
      } catch (err) {
        Notice({ msg: "Could not load seller profile", isSuccess: false });
      } finally {
        setLoading(false);
      }
    };

    if (id) fetchSeller();
  }, [id]);

  // Load real listings: if own profile, call authenticated active listings endpoint
  useEffect(() => {
    if (!id) return;
    const fetchListings = async () => {
      setListingsLoading(true);
      try {
        if (isOwnProfile) {
          // Authenticated: fetch own active listings
          const res = await axios.get("listings/active", { params: { PageSize: 12 } });
          setListings(res?.data?.items ?? []);
        } else {
          // Public: use items already included in the user entity (if backend provides them)
          setListings([]);
        }
      } catch {
        setListings([]);
      } finally {
        setListingsLoading(false);
      }
    };
    fetchListings();
  }, [id, isOwnProfile]);

  if (loading) return <LoadingScreen />;

  if (!seller) {
    return (
      <div className="min-h-screen bg-gray-50 flex flex-col items-center justify-center p-4">
        <h1 className="text-2xl font-bold text-gray-800 mb-4">Seller not found</h1>
        <Link to="/" className="text-blue-600 hover:underline">Return to Home</Link>
      </div>
    );
  }

  // Derive display values safely
  const displayName = seller.businessName || seller.fullName || seller.username || "eBay Member";
  const initials = displayName.substring(0, 2).toUpperCase();
  const joinDate = seller.createdUtc ? new Date(seller.createdUtc).getFullYear() : "2024";
  const isTopRated = seller.performanceLevel === 2 || seller.performanceLevel === "TopRated";
  const location = seller.businessAddress?.country || "United States";

  // Real metrics from backend, fallback to 0 (not fake numbers)
  const feedbackScore = seller.feedbackScore ?? 0;
  const positiveFeedback = seller.positiveFeedbackPercentage ?? 0;
  const itemsSold = seller.itemsSoldCount ?? 0;

  return (
    <div className="bg-gray-100 min-h-screen pb-12">
      
      {/* 1. HERO BANNER COVER */}
      <div className="h-48 md:h-64 w-full bg-gradient-to-r from-blue-700 to-indigo-900 relative">
        <div className="absolute inset-0 opacity-10" style={{ backgroundImage: 'radial-gradient(circle, #ffffff 2px, transparent 2px)', backgroundSize: '30px 30px' }}></div>
      </div>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        
        {/* 2. PROFILE INFO CARD */}
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 -mt-20 relative z-10 px-6 py-6 md:px-10 flex flex-col md:flex-row items-start md:items-center gap-6">
            
          {/* Avatar */}
          <div className="flex-shrink-0 relative">
            <div className="w-32 h-32 md:w-40 md:h-40 bg-white rounded-full p-2 shadow-md border border-gray-100 -mt-16 md:-mt-24">
              <div className="w-full h-full rounded-full bg-gradient-to-br from-gray-200 to-gray-300 flex items-center justify-center text-gray-600 text-4xl font-bold overflow-hidden border border-gray-200 object-cover">
                 {seller.avatarUrl ? (
                    <img src={seller.avatarUrl} alt={displayName} className="w-full h-full object-cover" />
                 ) : (
                    <span>{initials}</span>
                 )}
              </div>
            </div>
            {isTopRated && (
              <div className="absolute bottom-2 right-2 bg-yellow-400 text-yellow-900 p-2 rounded-full shadow-lg border-2 border-white" title="Top Rated Seller">
                <Medal size={20} className="fill-current" />
              </div>
            )}
          </div>

          {/* Core Info */}
          <div className="flex-1">
            <h1 className="text-3xl font-bold text-gray-900 flex items-center gap-2">
              {displayName}
              {seller.isSellerVerified && <ShieldCheck size={24} className="text-blue-500" title="Verified Seller" />}
            </h1>
            <p className="text-gray-500 font-medium text-sm mt-1 mb-4 flex items-center gap-1">
                {seller.username} 
                <span className="mx-2">•</span> 
                {feedbackScore} <Star size={14} className="inline text-yellow-400 fill-current mb-1" />
            </p>

            <div className="flex flex-wrap gap-x-6 gap-y-3 mt-4 text-sm text-gray-700 bg-gray-50 inline-flex p-3 rounded-lg border border-gray-100">
              <div className="flex items-center gap-2">
                <ThumbsUp size={16} className="text-gray-400" />
                <span className="font-semibold text-gray-900">{positiveFeedback}%</span>
                <span className="text-gray-500">Positive feedback</span>
              </div>
              <div className="flex items-center gap-2">
                <Box size={16} className="text-gray-400" />
                <span className="font-semibold text-gray-900">{itemsSold.toLocaleString()}</span>
                <span className="text-gray-500">Items sold</span>
              </div>
              <div className="flex items-center gap-2">
                <MapPin size={16} className="text-gray-400" />
                <span className="text-gray-600">{location}</span>
              </div>
              <div className="flex items-center gap-2">
                <Calendar size={16} className="text-gray-400" />
                <span className="text-gray-600">Joined {joinDate}</span>
              </div>
            </div>
          </div>

          {/* Action Button — only show for visitors */}
          {!isOwnProfile && (
            <div className="w-full md:w-auto mt-4 md:mt-0 flex-shrink-0">
               <button className="w-full md:w-auto bg-white hover:bg-gray-50 border-2 border-blue-600 text-blue-700 font-semibold py-2.5 px-8 rounded-full shadow-sm transition-colors flex items-center justify-center gap-2">
                  <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" className="w-5 h-5">
                      <path d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
                  </svg>
                  Save Seller
               </button>
               <button className="w-full md:w-auto mt-3 bg-gray-100 hover:bg-gray-200 text-gray-800 font-medium py-2 px-8 rounded-full transition-colors text-sm">
                  Contact Seller
               </button>
            </div>
          )}

          {/* Edit profile button for own profile */}
          {isOwnProfile && (
            <div className="w-full md:w-auto mt-4 md:mt-0 flex-shrink-0">
               <button
                 onClick={() => navigate("/account/settings")}
                 className="w-full md:w-auto bg-blue-600 hover:bg-blue-700 text-white font-semibold py-2.5 px-8 rounded-full shadow-sm transition-colors">
                  Edit Profile
               </button>
            </div>
          )}
        </div>

        <div className="mt-8 flex flex-col lg:flex-row gap-8">
            
            {/* LEFT COLUMN */}
            <div className="w-full lg:w-1/4 space-y-6">

                {/* About the Seller */}
                <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
                    <h3 className="text-lg font-bold text-gray-900 border-b pb-3 mb-4">About the Seller</h3>

                    <div className="space-y-3 text-sm">
                        <div className="flex justify-between items-center">
                            <span className="text-gray-500">Full name</span>
                            <span className="font-medium text-gray-800">{seller.fullName || "—"}</span>
                        </div>
                        {seller.businessName && (
                            <div className="flex justify-between items-center">
                                <span className="text-gray-500">Business</span>
                                <span className="font-medium text-gray-800">{seller.businessName}</span>
                            </div>
                        )}
                        {seller.businessAddress?.city && (
                            <div className="flex justify-between items-center">
                                <span className="text-gray-500">Location</span>
                                <span className="font-medium text-gray-800">
                                    {[seller.businessAddress.city, seller.businessAddress.country].filter(Boolean).join(", ")}
                                </span>
                            </div>
                        )}
                        <div className="flex justify-between items-center">
                            <span className="text-gray-500">Joined</span>
                            <span className="font-medium text-gray-800">{joinDate}</span>
                        </div>
                    </div>

                    {/* Verification badges */}
                    <div className="mt-5 pt-4 border-t border-gray-100">
                        <p className="text-xs text-gray-400 uppercase tracking-wider font-semibold mb-3">Verifications</p>
                        <div className="flex flex-wrap gap-2">
                            <span className={`inline-flex items-center gap-1 text-xs font-medium px-2.5 py-1 rounded-full ${seller.isEmailVerified ? "bg-green-100 text-green-700" : "bg-gray-100 text-gray-500"}`}>
                                <span>{seller.isEmailVerified ? "✓" : "○"}</span> Email
                            </span>
                            <span className={`inline-flex items-center gap-1 text-xs font-medium px-2.5 py-1 rounded-full ${seller.isPhoneVerified ? "bg-green-100 text-green-700" : "bg-gray-100 text-gray-500"}`}>
                                <span>{seller.isPhoneVerified ? "✓" : "○"}</span> Phone
                            </span>
                            <span className={`inline-flex items-center gap-1 text-xs font-medium px-2.5 py-1 rounded-full ${seller.isPaymentVerified ? "bg-green-100 text-green-700" : "bg-gray-100 text-gray-500"}`}>
                                <span>{seller.isPaymentVerified ? "✓" : "○"}</span> Payment
                            </span>
                            <span className={`inline-flex items-center gap-1 text-xs font-medium px-2.5 py-1 rounded-full ${seller.isBusinessVerified ? "bg-blue-100 text-blue-700" : "bg-gray-100 text-gray-500"}`}>
                                <span>{seller.isBusinessVerified ? "✓" : "○"}</span> Business
                            </span>
                        </div>
                    </div>
                </div>

                {/* Selling Quota */}
                {(() => {
                    const limitPolicy = seller.limitPolicy;
                    const maxListings = limitPolicy?.maxActiveListings ?? 100;
                    const maxValue = limitPolicy?.maxTotalValue ?? 500000000;
                    const activeCount = listings.length;
                    const listingPct = Math.min((activeCount / maxListings) * 100, 100);

                    const perfLevelName = seller.performanceLevel?.name
                        ?? (typeof seller.performanceLevel === "string" ? seller.performanceLevel : "BelowStandard");

                    const levelColors = {
                        TopRated: { bar: "bg-yellow-400", badge: "bg-yellow-100 text-yellow-800" },
                        AboveStandard: { bar: "bg-blue-500", badge: "bg-blue-100 text-blue-800" },
                        BelowStandard: { bar: "bg-gray-400", badge: "bg-gray-100 text-gray-600" },
                    };
                    const colors = levelColors[perfLevelName] ?? levelColors.BelowStandard;
                    const fmtNum = (n) => n >= 1_000_000_000 ? `${(n / 1_000_000_000).toFixed(0)}B` : n >= 1_000_000 ? `${(n / 1_000_000).toFixed(0)}M` : n.toLocaleString();

                    return (
                        <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
                            <div className="flex items-center justify-between mb-1">
                                <h3 className="text-lg font-bold text-gray-900">Selling Quota</h3>
                                <span className={`text-xs font-bold px-2.5 py-1 rounded-full ${colors.badge}`}>
                                    {perfLevelName === "TopRated" ? "⭐ Top Rated" : perfLevelName === "AboveStandard" ? "Above Standard" : "Below Standard"}
                                </span>
                            </div>
                            <p className="text-xs text-gray-400 mb-5">Giới hạn đăng bán theo cấp độ seller</p>

                            <div className="mb-5">
                                <div className="flex justify-between text-sm mb-1.5">
                                    <span className="text-gray-600 font-medium">Active Listings</span>
                                    <span className="font-bold text-gray-900">{activeCount} <span className="text-gray-400 font-normal">/ {fmtNum(maxListings)}</span></span>
                                </div>
                                <div className="h-2.5 bg-gray-100 rounded-full overflow-hidden">
                                    <div className={`h-full rounded-full transition-all duration-500 ${colors.bar}`} style={{ width: `${listingPct}%` }} />
                                </div>
                                <p className="text-xs text-gray-400 mt-1 text-right">{listingPct.toFixed(1)}% used</p>
                            </div>

                            <div className="pt-4 border-t border-gray-100">
                                <div className="flex justify-between text-sm">
                                    <span className="text-gray-500">Max total value</span>
                                    <span className="font-semibold text-gray-800">${fmtNum(maxValue)}</span>
                                </div>
                                <div className="mt-3 flex justify-between text-sm">
                                    <span className="text-gray-500">Max listings</span>
                                    <span className="font-semibold text-gray-800">{fmtNum(maxListings)}</span>
                                </div>
                            </div>
                        </div>
                    );
                })()}
            </div>

            {/* RIGHT COLUMN: Real Listings */}
            <div className="w-full lg:w-3/4">
                <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
                    <div className="flex justify-between items-center mb-6">
                        <h2 className="text-xl font-bold text-gray-900">
                            Items for sale {listings.length > 0 && `(${listings.length})`}
                        </h2>
                        <div className="flex gap-2 text-sm">
                            <span className="text-gray-500 self-center">Sort:</span>
                            <select className="border border-gray-300 rounded px-2 py-1 text-gray-700 bg-white">
                                <option>Best Match</option>
                                <option>Time: ending soonest</option>
                                <option>Time: newly listed</option>
                                <option>Price: lowest first</option>
                            </select>
                        </div>
                    </div>

                    {listingsLoading ? (
                        <div className="flex justify-center py-12">
                            <div className="w-8 h-8 border-4 border-blue-600 border-t-transparent rounded-full animate-spin" />
                        </div>
                    ) : listings.length === 0 ? (
                        <div className="text-center py-12 text-gray-400">
                            <Box size={48} className="mx-auto mb-3 opacity-30" />
                            <p className="text-lg">{isOwnProfile ? "You have no active listings." : "No active listings at the moment."}</p>
                            {isOwnProfile && (
                                <Link to="/sell/listing/create" className="mt-4 inline-block text-blue-600 hover:underline text-sm">
                                    + Create your first listing
                                </Link>
                            )}
                        </div>
                    ) : (
                        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                            {listings.map(item => (
                                <Link to={`/listing/${item.id}`} key={item.id} className="group block h-full">
                                  <div className="bg-white rounded-lg border border-gray-200 overflow-hidden hover:shadow-md transition-shadow h-full flex flex-col">
                                    <div className="aspect-square w-full relative bg-gray-100">
                                       {item.thumbnail ? (
                                           <img src={item.thumbnail} alt={item.title} className="w-full h-full object-cover" />
                                       ) : (
                                           <div className="w-full h-full flex items-center justify-center text-gray-300">
                                               <Box size={40} />
                                           </div>
                                       )}
                                    </div>
                                    <div className="p-4 flex-1 flex flex-col">
                                        <h3 className="text-gray-900 font-medium text-sm line-clamp-2 leading-snug group-hover:text-blue-600 group-hover:underline mb-2">{item.title}</h3>
                                        <div className="mt-auto">
                                            <div className="text-lg font-bold text-gray-900">
                                                ${(item.currentPrice ?? item.price ?? 0).toFixed(2)}
                                            </div>
                                            <div className="text-xs text-gray-400 mt-1">
                                                {item.soldQuantity ?? 0} sold · {item.availableQuantity ?? 0} available
                                            </div>
                                        </div>
                                    </div>
                                  </div>
                                </Link>
                            ))}
                        </div>
                    )}
                </div>
            </div>
        </div>
      </div>
    </div>
  );
}
