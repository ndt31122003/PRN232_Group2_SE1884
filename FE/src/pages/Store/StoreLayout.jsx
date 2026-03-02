import React from "react";
import LeftNav from "../../components/Navigation/LeftNav/LeftNav";
import { Outlet, useLocation } from "react-router-dom";
import "./StoreLayout.scss";

const StoreLayout = () => {
    const location = useLocation();
    
    // Extract storeId from current URL
    const searchParams = new URLSearchParams(location.search);
    const storeId = searchParams.get("storeId");
    
    // Build menu items with storeId preserved
    const menuItems = [
        {
            id: "settings",
            label: "Settings",
            path: "/store/settings",
            search: storeId ? `?storeId=${storeId}` : undefined
        },
        {
            id: "policies",
            label: "Policies",
            path: "/store/policies",
            search: storeId ? `?storeId=${storeId}` : undefined
        },
        {
            id: "subscription",
            label: "Subscription & Fees",
            path: "/store/subscription",
            search: storeId ? `?storeId=${storeId}` : undefined
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

export default StoreLayout;

