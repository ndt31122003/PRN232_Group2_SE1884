import React from "react";
import LeftNav from "../../components/Navigation/LeftNav/LeftNav";
import { Outlet } from "react-router-dom";

const ListingsPage = () => {
    const menuItems = [
        {
            id: "active-listings",
            label: "Active",
            path: "/listings/active"
        },
        {
            id: "unsold-listings",
            label: "Unsold",
            path: "/listings/unsold"
        },
        {
            id: "draft-listings",
            label: "Drafts",
            path: "/listings/drafts"
        },
        {
            id: "scheduled-listings",
            label: "Scheduled",
            path: "/listings/scheduled"
        },
        
        {
            id: "ended-listings",
            label: "Ended",
            path: "/listings/ended"
        },
        // Divider
        { divider: true },
        {
            id: "selling-preferences",
            label: "Selling preferences",
            path: "/listings/selling-preferences"
        },
        {
            id: "listing-templates",
            label: "Listing templates",
            path: "/listings/listing-templates"
        },
        {
            id: "business-policies",
            label: "Business policies",
            path: "/listings/business-policies"
        }
    ];

    return (
        <div style={{ display: "flex", height: "100vh" }} className="container">
            <LeftNav menuItems={menuItems} />
            {/* Main content */}
            <main style={{ flex: 1, overflowY: "auto" }}>
                <Outlet />
            </main>
        </div>
    );
};

export default ListingsPage;
