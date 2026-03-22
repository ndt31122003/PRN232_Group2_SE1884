import React, { useState, useMemo } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import Notice from "../../../components/Common/CustomNotification";
import ListingService from "../../../services/ListingService";
import "./BidsManagement.css";

const BidsManagementPage = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const listingId = searchParams.get('listingId');
    const [bids, setBids] = useState([]);
    const [loading, setLoading] = useState(true);

    React.useEffect(() => {
        const fetchBids = async () => {
            setLoading(true);
            try {
                const id = (listingId && listingId !== 'all') ? listingId : null;
                const data = await ListingService.getBids(id);
                
                const mapped = data.map(b => ({
                    id: b.id,
                    listingId: b.listingId,
                    title: b.listingTitle,
                    thumbnail: b.listingThumbnail || "https://images.unsplash.com/photo-1544731612-de7f96afe55f?q=80&w=300&auto=format&fit=crop",
                    bidder: b.bidderName,
                    amount: b.amount,
                    date: new Date(b.createdAt).toLocaleString(),
                    bidderId: b.bidderId
                }));
                
                setBids(mapped);
            } catch (err) {
                console.error("Failed to fetch bids:", err);
                Notice({
                    msg: "Fetch Failed",
                    desc: "Could not load real bids from the database.",
                    isSuccess: false
                });
            } finally {
                setLoading(false);
            }
        };

        fetchBids();
    }, [listingId]);

    // Group bids by listing for a better display if 'all' is selected
    const groupedBids = useMemo(() => {
        const groups = {};
        bids.forEach(bid => {
            if (!groups[bid.listingId]) {
                groups[bid.listingId] = {
                    title: bid.title,
                    thumbnail: bid.thumbnail,
                    listingId: bid.listingId,
                    bids: []
                };
            }
            groups[bid.listingId].bids.push(bid);
        });
        return Object.values(groups);
    }, [bids]);

    return (
        <div className="bids-mgmt">
            <header className="bids-mgmt__header">
                <div className="bids-mgmt__breadcrumb">
                    <span onClick={() => navigate("/overview")}>Seller Hub</span>
                    <span className="separator"> &gt; </span>
                    <span onClick={() => navigate("/listings/active")}>Listings</span>
                    <span className="separator"> &gt; </span>
                    <span className="current">Bids</span>
                </div>
                <h1>Auction Bids</h1>
                <p>Monitor your active auctions and bidder activity.</p>
            </header>

            <div className="bids-mgmt__content">
                {loading ? (
                    <div className="bids-mgmt__loading">Loading bids...</div>
                ) : bids.length === 0 ? (
                    <div className="bids-mgmt__empty">
                        <h3>No bids found</h3>
                        <p>When buyers place bids on your auctions, they will appear here.</p>
                    </div>
                ) : (
                    <div className="bids-mgmt__list">
                        {groupedBids.map(group => (
                            <div key={group.listingId} className="bid-group">
                                <div className="bid-group__listing-info">
                                    <div className="bid-card">
                                        <div className="bid-card__image">
                                            <img src={group.thumbnail} alt={group.title} />
                                        </div>
                                        <div className="bid-card__details">
                                            <h3 className="bid-card__title" onClick={() => navigate(`/p/${group.listingId}`)} style={{ cursor: "pointer", color: "#3665f3" }}>
                                                {group.title}
                                            </h3>
                                            <div className="bid-card__meta">
                                                <span>Total Bids: <strong>{group.bids.length}</strong></span>
                                                <span className="separator">•</span>
                                                <span>Current High Bid: <strong style={{color: "#008a00"}}>${Math.max(...group.bids.map(b => b.amount)).toFixed(2)}</strong></span>
                                            </div>
                                            
                                            <div className="bid-history">
                                                <table className="bid-history__table">
                                                    <thead>
                                                        <tr>
                                                            <th>Bidder</th>
                                                            <th>Amount</th>
                                                            <th>Date & Time</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        {group.bids.sort((a,b) => b.amount - a.amount).map(bid => (
                                                            <tr key={bid.id}>
                                                                <td><strong>{bid.bidder}</strong></td>
                                                                <td className="text-green-700 font-bold">${bid.amount.toFixed(2)}</td>
                                                                <td>{bid.date}</td>
                                                            </tr>
                                                        ))}
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default BidsManagementPage;
