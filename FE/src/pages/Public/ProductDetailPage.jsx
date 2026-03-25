import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import PublicListingService from "../../services/PublicListingService";
import Notice from "../../components/Common/CustomNotification";
import "./ProductDetailPage.css";

const ProductDetailPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const [listing, setListing] = useState(null);
    const [loading, setLoading] = useState(true);
    const [offerAmount, setOfferAmount] = useState("");
    const [bidAmount, setBidAmount] = useState("");
    const [showOfferModal, setShowOfferModal] = useState(false);

    const STATUS_LABELS = {
        1: "Draft",
        2: "Scheduled",
        3: "Active",
        4: "Ended"
    };

    useEffect(() => {
        const fetchListing = async () => {
            try {
                const response = await PublicListingService.getPublicListing(id);
                setListing(response.data);
            } catch (error) {
                console.error("Failed to fetch listing:", error);
                Notice({
                    msg: "Error",
                    desc: "Listing not found or not active.",
                    isSuccess: false
                });
            } finally {
                setLoading(false);
            }
        };

        fetchListing();
    }, [id]);

    const handleMakeOffer = async () => {
        try {
            await PublicListingService.createOffer(id, parseFloat(offerAmount));
            Notice({
                msg: "Offer Sent",
                desc: "Your offer has been submitted successfully!",
                isSuccess: true
            });
            setShowOfferModal(false);
            setOfferAmount("");
        } catch (error) {
            Notice({
                msg: "Failed to send offer",
                desc: error.response?.data?.Detail || error.response?.data?.detail || "Unauthorized. Please login as a buyer.",
                isSuccess: false
            });
        }
    };

    const handlePlaceBid = async () => {
        try {
            await PublicListingService.placeBid(id, parseFloat(bidAmount));
            Notice({
                msg: "Bid Placed",
                desc: "Your bid has been placed successfully!",
                isSuccess: true
            });
            setBidAmount("");
            // Refresh listing to see new bid count/price
            const response = await PublicListingService.getPublicListing(id);
            setListing(response.data);
        } catch (error) {
            Notice({
                msg: "Failed to place bid",
                desc: error.response?.data?.Detail || error.response?.data?.detail || "Your bid must be higher than current price.",
                isSuccess: false
            });
        }
    };

    const handleBuyItNow = async () => {
        try {
            await PublicListingService.buyItNow(id, 1);
            Notice({
                msg: "Success",
                desc: "Mua hàng thành công!",
                isSuccess: true
            });
            navigate("/order/all?status=all");
        } catch (error) {
            Notice({
                msg: "Mua hàng thất bại",
                desc: error.response?.data?.Detail || error.response?.data?.detail || "Đã xảy ra lỗi khi mua hàng.",
                isSuccess: false
            });
        }
    };

    if (loading) return <div className="product-loading">Loading product...</div>;
    if (!listing) return <div className="product-not-found">Listing not found.</div>;

    const getProp = (obj, ...keys) => {
        for (const key of keys) {
            if (obj && obj[key] !== undefined) return obj[key];
        }
        return undefined;
    };

    const format = getProp(listing, "format", "Format");
    const isAuction = format === 1; 
    const isFixedPrice = format === 2;
    const title = getProp(listing, "title", "Title");
    const description = getProp(listing, "listingDescription", "ListingDescription");
    const images = getProp(listing, "listingImages", "ListingImages") || [];
    const price = getProp(listing, "price", "Price");
    const startPrice = getProp(listing, "startPrice", "StartPrice");
    const status = getProp(listing, "status", "Status");
    const allowOffers = getProp(listing, "allowOffers", "AllowOffers", "bestOfferEnabled", "BestOfferEnabled");
    const buyItNowPrice = getProp(listing, "buyItNowPrice", "BuyItNowPrice");

    return (
        <div className="product-detail-container">
            <div className="product-header">
                <button className="back-button" onClick={() => navigate(-1)}>← Back</button>
                <h1>{title}</h1>
            </div>

            <div className="product-content">
                <div className="product-gallery">
                    {images.length > 0 ? (
                        <img src={images[0].url || images[0].Url} alt={title} />
                    ) : (
                        <div className="no-image">No Image</div>
                    )}
                </div>

                <div className="product-info">
                    <div className="price-section">
                        {isFixedPrice ? (
                            <div className="fixed-price">
                                <span className="label">Price:</span>
                                <span className="value">${(price || 0).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
                            </div>
                        ) : (
                            <div className="auction-price">
                                <span className="label">Starting Price:</span>
                                <span className="value">${(startPrice || 0).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</span>
                            </div>
                        )}
                    </div>

                    <div className="listing-status">
                        Status: <span className={`status-${String(status).toLowerCase()}`}>{STATUS_LABELS[status] || status}</span>
                    </div>

                    <div className="actions-section">
                        {isFixedPrice && (
                            <div className="fixed-price-actions">
                                <button className="buy-now-btn" onClick={handleBuyItNow}>Buy It Now</button>
                                {allowOffers && (
                                    <button 
                                        className="make-offer-btn"
                                        onClick={() => setShowOfferModal(true)}
                                    >
                                        Make Offer
                                    </button>
                                )}
                            </div>
                        )}

                        {isAuction && (
                            <div className="auction-actions" style={{ display: 'flex', flexDirection: 'column', gap: '15px' }}>
                                <div className="bid-input-group">
                                    <input 
                                        type="number" 
                                        placeholder={`Enter bid > $${startPrice}`}
                                        value={bidAmount}
                                        onChange={(e) => setBidAmount(e.target.value)}
                                    />
                                    <button className="place-bid-btn" onClick={handlePlaceBid}>Place Bid</button>
                                </div>
                                {buyItNowPrice > 0 && (
                                    <button className="buy-now-btn" onClick={handleBuyItNow}>
                                        Buy It Now for ${buyItNowPrice.toFixed(2)}
                                    </button>
                                )}
                            </div>
                        )}
                    </div>

                    <div className="product-description">
                        <h3>Description</h3>
                        <p>{description}</p>
                    </div>
                </div>
            </div>

            {showOfferModal && (
                <div className="offer-modal-overlay">
                    <div className="offer-modal">
                        <h2>Make an Offer</h2>
                        <p>Submit your best price for this item.</p>
                        <div className="offer-input-group">
                            <span>$</span>
                            <input 
                                type="number" 
                                value={offerAmount}
                                onChange={(e) => setOfferAmount(e.target.value)}
                                placeholder="Your offer"
                            />
                        </div>
                        <div className="modal-buttons">
                            <button className="cancel-btn" onClick={() => setShowOfferModal(false)}>Cancel</button>
                            <button className="submit-offer-btn" onClick={handleMakeOffer}>Send Offer</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ProductDetailPage;
