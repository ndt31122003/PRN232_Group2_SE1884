import React, { useState, useMemo } from "react";
import { useNavigate, useLocation, useSearchParams } from "react-router-dom";
import Notice from "../../../components/Common/CustomNotification";
import ListingService from "../../../services/ListingService";
import "./OffersManagement.css";

const MOCK_OFFERS = [
    {
        id: "offer-1",
        listingId: "14845ea2-0f3c-4838-8501-0091d26cdf3a", // Real Laptop ID from DB
        title: "Bán laptop siêu cấp vip pro",
        thumbnail: "https://res.cloudinary.com/djmftornv/image/upload/v1774108440/ebay-clone/laptop-1_9aea42c8-98b2-4664-b0d8-5b88e664755f.jpg",
        buyer: "buyer_premium_99",
        amount: 855.00,
        currentPrice: 900.00,
        status: "Pending",
        timeLeft: "23h 45m",
        date: "Mar 21, 2026"
    },
    {
        id: "offer-2",
        listingId: "14845ea2-0f3c-4838-8501-0091d26cdf3a",
        title: "Bán laptop siêu cấp vip pro",
        thumbnail: "https://images.unsplash.com/photo-1517336713481-48c938f70105?q=80&w=300&auto=format&fit=crop",
        buyer: "tech_enthusiast",
        amount: 870.00,
        currentPrice: 900.00,
        status: "Pending",
        timeLeft: "1d 2h",
        date: "Mar 21, 2026"
    },
    {
        id: "offer-3",
        listingId: "71000000-0000-0000-0000-000000000002",
        title: "iPhone 15 Pro Max - Silver - 256GB",
        thumbnail: "https://images.unsplash.com/photo-1696446701796-da61225697cc?q=80&w=300&auto=format&fit=crop",
        buyer: "johndoe_82",
        amount: 1050.00,
        currentPrice: 1199.00,
        status: "Declined",
        timeLeft: "Expired",
        date: "Mar 20, 2026"
    }
];

const OffersManagementPage = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const listingId = searchParams.get('listingId');
    const [offers, setOffers] = useState(MOCK_OFFERS);
    const [filter, setFilter] = useState('All');

    const filteredOffers = useMemo(() => {
        let result = offers;
        if (listingId && listingId !== 'all') {
            // Filter by the ID in URL or match our mock ID for the laptop
            result = result.filter(o => o.listingId === listingId || (listingId === '14845ea2-0f3c-4838-8501-0091d26cdf3a' && o.listingId === '14845ea2-0f3c-4838-8501-0091d26cdf3a'));
        }
        if (filter !== 'All') {
            result = result.filter(o => o.status === filter);
        }
        return result;
    }, [offers, listingId, filter]);

    const handleAction = async (offerId, action) => {
        if (action === 'accept') {
            const offer = offers.find(o => o.id === offerId);
            try {
                // Bridge to real backend: Create an Order
                // For demo, we use the real listing ID provided by you
                const lid = (listingId && listingId !== 'all' && listingId !== '14845ea2-0f3c-4838-8501-0091d26cdf3a')
                    ? listingId
                    : "14845ea2-0f3c-4838-8501-0091d26cdf3a"; // Laptop ID

                const demoBuyerId = "70000000-0000-0000-0000-000000000003"; // Demo Buyer (Cecilia)

                await ListingService.acceptOffer(lid, offer.amount, demoBuyerId);

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
            setOffers(prev => prev.map(o =>
                o.id === offerId ? { ...o, status: 'Declined' } : o
            ));
            Notice({
                msg: "Offer Declined",
                desc: "The offer has been removed from your active list.",
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
                                    <h3 className="offer-card__title">{offer.title}</h3>
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
