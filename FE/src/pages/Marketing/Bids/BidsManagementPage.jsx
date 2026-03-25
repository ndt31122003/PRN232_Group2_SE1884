import React, { useState, useCallback, useEffect, useRef } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import * as signalR from "@microsoft/signalr";
import Notice from "../../../components/Common/CustomNotification";
import ListingService from "../../../services/ListingService";
import STORAGE, { getStorage } from "../../../lib/storage";
import "./BidsManagement.css";

const isDev = process.env.NODE_ENV === "development";
const HUB_URL = isDev ? "http://localhost:5149/hub" : "https://propval.io.vn/hub";

/* ------------------------------------------------------------------ */
/* Countdown timer component                                            */
/* ------------------------------------------------------------------ */
function Countdown({ endDate }) {
    const calc = () => {
        const diff = Math.max(0, Math.floor((new Date(endDate) - Date.now()) / 1000));
        const h = Math.floor(diff / 3600);
        const m = Math.floor((diff % 3600) / 60);
        const s = diff % 60;
        return { h, m, s, expired: diff === 0 };
    };

    const [time, setTime] = useState(calc);

    useEffect(() => {
        if (!endDate) return;
        const id = setInterval(() => setTime(calc()), 1000);
        return () => clearInterval(id);
    }, [endDate]);

    if (!endDate) return null;
    if (time.expired) return <span className="countdown expired">Auction ended</span>;

    return (
        <span className="countdown">
            ⏱ {String(time.h).padStart(2, "0")}:{String(time.m).padStart(2, "0")}:{String(time.s).padStart(2, "0")}
        </span>
    );
}

/* ------------------------------------------------------------------ */
/* Main page                                                            */
/* ------------------------------------------------------------------ */
const BidsManagementPage = () => {
    const [searchParams] = useSearchParams();
    const navigate = useNavigate();
    const listingIdParam = searchParams.get("listingId");

    const [groups, setGroups]           = useState([]);
    const [loading, setLoading]         = useState(true);
    const [flashedBids, setFlashedBids] = useState(new Set());

    const connectionRef   = useRef(null);
    const joinedGroupsRef = useRef(new Set());

    /* ---- Load bids from API ---- */
    const buildGroups = useCallback((data) => {
        const map = {};
        (data || []).forEach((b) => {
            const lid = b.listingId ?? b.ListingId;
            if (!lid) return;
            if (!map[lid]) {
                map[lid] = {
                    listingId: lid,
                    title: b.listingTitle ?? b.ListingTitle ?? "Untitled",
                    thumbnail: b.listingThumbnail ?? b.ListingThumbnail ?? "https://via.placeholder.com/100",
                    endDate: b.endDate ?? b.EndDate ?? null,
                    bids: [],
                };
            }
            map[lid].bids.push({
                id: b.id ?? b.Id,
                bidder: b.bidderName ?? b.BidderName ?? "Unknown",
                amount: b.amount ?? b.Amount ?? 0,
                date: new Date(b.createdAt ?? b.CreatedAt).toLocaleString(),
            });
        });
        const groupList = Object.values(map);
        setGroups(groupList);
        return groupList;
    }, []);

    const fetchBids = useCallback(async () => {
        try {
            const id = listingIdParam && listingIdParam !== "all" ? listingIdParam : null;
            const data = await ListingService.getBids(id);
            const groupList = buildGroups(data);

            // After loading, join listing groups not yet subscribed
            const conn = connectionRef.current;
            if (conn && conn.state === signalR.HubConnectionState.Connected) {
                groupList.forEach((g) => {
                    if (!joinedGroupsRef.current.has(g.listingId)) {
                        conn.invoke("JoinListingGroup", String(g.listingId))
                            .then(() => joinedGroupsRef.current.add(g.listingId))
                            .catch(() => {});
                    }
                });
            }
        } catch (err) {
            console.error("[BidsPage] fetchBids error:", err);
        } finally {
            setLoading(false);
        }
    }, [listingIdParam, buildGroups]);

    useEffect(() => {
        fetchBids();
        const id = setInterval(fetchBids, 30_000); // 30s polling fallback
        return () => clearInterval(id);
    }, [fetchBids]);

    /* ---- SignalR: own connection so we can join MULTIPLE listing groups ---- */
    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(HUB_URL, {
                accessTokenFactory: () => getStorage(STORAGE.TOKEN) ?? "",
                transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling,
            })
            .withAutomaticReconnect([0, 2000, 5000, 10000])
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.on("NewBid", (payload) => {
            const listingId = payload.listingId ?? payload.ListingId;
            const bidId     = payload.bidId     ?? payload.BidId     ?? String(Date.now());
            const bidder    = payload.bidderName ?? payload.BidderName ?? "Unknown";
            const amount    = payload.amount    ?? payload.Amount    ?? 0;
            const placedAt  = payload.placedAt  ?? payload.PlacedAt  ?? new Date().toISOString();

            setGroups((prev) =>
                prev.map((g) => {
                    if (g.listingId !== listingId) return g;
                    if (g.bids.some(b => b.id === bidId)) return g; // deduplicate
                    return {
                        ...g,
                        bids: [
                            { id: bidId, bidder, amount, date: new Date(placedAt).toLocaleString() },
                            ...g.bids,
                        ],
                    };
                })
            );

            setFlashedBids((prev) => new Set([...prev, bidId]));
            setTimeout(() => setFlashedBids((prev) => {
                const n = new Set(prev); n.delete(bidId); return n;
            }), 3000);

            Notice({ msg: "New bid placed!", desc: `${bidder} bid $${Number(amount).toFixed(2)}`, isSuccess: true });
        });

        connectionRef.current = connection;

        connection
            .start()
            .then(() => {
                console.log("[BidsPage SignalR] ✅ Connected");
                // Join all currently-loaded listing groups
                setGroups((currentGroups) => {
                    currentGroups.forEach((g) => {
                        if (!joinedGroupsRef.current.has(g.listingId)) {
                            connection.invoke("JoinListingGroup", String(g.listingId))
                                .then(() => joinedGroupsRef.current.add(g.listingId))
                                .catch(() => {});
                        }
                    });
                    return currentGroups;
                });
            })
            .catch((err) => console.error("[BidsPage SignalR] ❌ Failed:", err?.message));

        connection.onreconnected(() => {
            console.log("[BidsPage SignalR] ✅ Reconnected – re-joining groups");
            joinedGroupsRef.current.clear();
            setGroups((currentGroups) => {
                currentGroups.forEach((g) => {
                    connection.invoke("JoinListingGroup", String(g.listingId))
                        .then(() => joinedGroupsRef.current.add(g.listingId))
                        .catch(() => {});
                });
                return currentGroups;
            });
        });

        return () => {
            connection.stop().catch(() => {});
            joinedGroupsRef.current.clear();
        };
    }, []); // mount once

    /* ---------------------------------------------------------------- */
    return (
        <div className="bids-mgmt">
            <header className="bids-mgmt__header">
                <div className="bids-mgmt__breadcrumb">
                    <span onClick={() => navigate("/")}>Seller Hub</span>
                    <span className="separator"> › </span>
                    <span onClick={() => navigate("/listings/active")}>Listings</span>
                    <span className="separator"> › </span>
                    <span className="current">Bids</span>
                </div>
                <h1>Auction Bids</h1>
                <p>Monitor your active auctions and bidder activity — updates in real-time.</p>
            </header>

            <div className="bids-mgmt__content">
                {loading ? (
                    <p className="bids-mgmt__loading">Loading bids…</p>
                ) : groups.length === 0 ? (
                    <div className="bids-mgmt__empty">
                        <h3>No bids yet</h3>
                        <p>When buyers place bids on your auctions they will appear here instantly.</p>
                    </div>
                ) : (
                    groups.map((group) => (
                        <div key={group.listingId} className="bid-group">
                            <div className="bid-card">
                                <div className="bid-card__image">
                                    <img src={group.thumbnail} alt={group.title} />
                                </div>
                                <div className="bid-card__details">
                                    <h3
                                        className="bid-card__title"
                                        onClick={() => navigate(`/p/${group.listingId}`)}
                                        style={{ cursor: "pointer" }}
                                    >
                                        {group.title}
                                    </h3>
                                    <div className="bid-card__meta">
                                        <span>Total bids: <strong>{group.bids.length}</strong></span>
                                        {group.bids.length > 0 && (
                                            <>
                                                <span className="separator">•</span>
                                                <span>
                                                    High bid:{" "}
                                                    <strong style={{ color: "#008a00" }}>
                                                        ${Math.max(...group.bids.map((b) => b.amount)).toFixed(2)}
                                                    </strong>
                                                </span>
                                            </>
                                        )}
                                        <span className="separator">•</span>
                                        <Countdown endDate={group.endDate} />
                                    </div>

                                    <div className="bid-history">
                                        <table className="bid-history__table">
                                            <thead>
                                                <tr>
                                                    <th>Bidder</th>
                                                    <th>Amount</th>
                                                    <th>Date &amp; Time</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {[...group.bids]
                                                    .sort((a, b) => b.amount - a.amount)
                                                    .map((bid) => (
                                                        <tr
                                                            key={bid.id}
                                                            className={flashedBids.has(bid.id) ? "bid-row--flash" : ""}
                                                        >
                                                            <td><strong>{bid.bidder}</strong></td>
                                                            <td style={{ color: "#008a00", fontWeight: 700 }}>
                                                                ${Number(bid.amount).toFixed(2)}
                                                            </td>
                                                            <td>{bid.date}</td>
                                                        </tr>
                                                    ))}
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    ))
                )}
            </div>
        </div>
    );
};

export default BidsManagementPage;
