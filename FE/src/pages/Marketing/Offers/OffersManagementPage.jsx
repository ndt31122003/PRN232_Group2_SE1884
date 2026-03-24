import React, { useState, useMemo } from "react";
import { useNavigate, useLocation, useSearchParams } from "react-router-dom";
import Notice from "../../../components/Common/CustomNotification";
import ListingService from "../../../services/ListingService";
import "./OffersManagement.css";

const STATUS_MAP = {
    0: "Pending",
    1: "Accepted",
    2: "Declined",
    3: "Countered",
    4: "Expired"
};

const OffersManagementPage = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const listingId = searchParams.get('listingId');
    const [offers, setOffers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [filter, setFilter] = useState('All');

    React.useEffect(() => {
        const fetchOffers = async () => {
            setLoading(true);
            try {
                // Pass null if 'all' or no listingId
                const id = (listingId && listingId !== 'all') ? listingId : null;
                const data = await ListingService.getOffers(id);
                
                // Map backend DTO to UI fields
                const mapped = data.map(o => ({
                    id: o.id,
                    listingId: o.listingId,
                    title: o.listingTitle,
                    thumbnail: o.listingThumbnail || "https://images.unsplash.com/photo-1544731612-de7f96afe55f?q=80&w=300&auto=format&fit=crop",
                    buyer: o.buyerName,
                    amount: o.amount,
                    currentPrice: o.currentPrice,
                    status: STATUS_MAP[o.status] || "Unknown",
                    timeLeft: "Active", // Backend could provide real time left later
                    date: new Date(o.createdAt).toLocaleDateString(),
                    buyerId: o.buyerId
                }));
                
                setOffers(mapped);
            } catch (err) {
                console.error("Failed to fetch offers:", err);
                Notice({
                    msg: "Fetch Failed",
                    desc: "Could not load real offers from the database.",
                    isSuccess: false
                });
            } finally {
                setLoading(false);
            }
        };

        fetchOffers();
    }, [listingId]);

    const filteredOffers = useMemo(() => {
        let result = offers;
        if (filter !== 'All') {
            result = result.filter(o => o.status === filter);
        }
        return result;
    }, [offers, filter]);

    const handleAction = async (offerId, action) => {
        const offer = offers.find(o => o.id === offerId);
        if (!offer) return;

        if (action === 'accept') {
            try {
                await ListingService.acceptOffer(offer.listingId, offer.amount, offer.buyerId);

                setOffers(prev => prev.map(o =>
                    o.id === offerId ? { ...o, status: 'Accepted' } : o
                ));

                Notice({
                    msg: "Offer Accepted",
                    desc: `A real Order has been created for ${offer.buyer}. check the 'Manage Orders' section!`,
                    isSuccess: true
                });
            } catch (err) {
                console.error("Failed to accept offer:", err);
                Notice({
                    msg: "Action Failed",
                    desc: err.response?.data?.detail || "Could not create order in the database.",
                    isSuccess: false
                });
            }
        } else {
            // Logic for decline would go here (e.g., ListingService.declineOffer)
            // For now, we just update local state to reflect the UI
            setOffers(prev => prev.map(o =>
                o.id === offerId ? { ...o, status: 'Declined' } : o
            ));
            Notice({
                msg: "Offer Declined",
                desc: "The offer status has been updated.",
                isSuccess: false
            });
        }
    };

    return (
        <div className="offers-mgmt">
            <header className="offers-mgmt__header">
                <div className="offers-mgmt__breadcrumb">
                    <span onClick={() => navigate("/overview")}>Seller Hub</span>
                    <span className="separator"> &gt; </span>
                    <span onClick={() => navigate("/listings/active")}>Listings</span>
                    <span className="separator"> &gt; </span>
                    <span className="current">Offers</span>
                </div>
                <h1>Review Offers</h1>
                <p>Manage price negotiations with interested buyers.</p>
            </header>

            <div className="offers-mgmt__tabs">
                <button className="offers-mgmt__tab active">Active ({filteredOffers.filter(o => o.status === "Pending").length})</button>
                <button className="offers-mgmt__tab">History</button>
            </div>

            <div className="offers-mgmt__content">
                {filteredOffers.length === 0 ? (
                    <div className="offers-mgmt__empty">
                        <h3>No offers found</h3>
                        <p>When buyers send you offers, they will appear here.</p>
                    </div>
                ) : (
                    <div className="offers-mgmt__list">
                        {filteredOffers.map(offer => (
                            <div key={offer.id} className={`offer-card ${offer.status.toLowerCase()}`}>
                                <div className="offer-card__image">
                                    <img src={offer.thumbnail} alt={offer.title} />
                                </div>
                                <div className="offer-card__details">
                                    <h3 className="offer-card__title" onClick={() => navigate(`/p/${offer.listingId}`)} style={{ cursor: "pointer", color: "#3665f3" }}>{offer.title}</h3>
                                    <div className="offer-card__meta">
                                        <span>Buyer: <strong className="text-blue-600">{offer.buyer}</strong></span>
                                        <span className="separator">•</span>
                                        <span>Sent: {offer.date}</span>
                                    </div>
                                    <div className="offer-card__pricing">
                                        <div className="price-box">
                                            <label>Offer Amount</label>
                                            <span className="amount highlight">${offer.amount.toFixed(2)}</span>
                                        </div>
                                        <div className="price-box">
                                            <label>Current Price</label>
                                            <span className="amount">${offer.currentPrice.toFixed(2)}</span>
                                        </div>
                                        <div className="price-box">
                                            <label>Status</label>
                                            <span className={`status-badge ${offer.status.toLowerCase()}`}>{offer.status}</span>
                                        </div>
                                    </div>
                                </div>
                                <div className="offer-card__actions">
                                    {offer.status === "Pending" ? (
                                        <>
                                            <button
                                                className="btn-accept"
                                                onClick={() => handleAction(offer.id, "accept")}
                                            >
                                                Accept Offer
                                            </button>
                                            <button
                                                className="btn-decline"
                                                onClick={() => handleAction(offer.id, "decline")}
                                            >
                                                Decline
                                            </button>
                                            <div className="time-left">Time left: {offer.timeLeft}</div>
                                        </>
                                    ) : (
                                        <div className="action-completed">
                                            {offer.status === "Accepted" ? "Order pending payment" : "Offer closed"}
                                        </div>
                                    )}
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default OffersManagementPage;
