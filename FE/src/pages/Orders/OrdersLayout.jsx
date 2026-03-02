import React from "react";
import LeftNav from "../../components/Navigation/LeftNav/LeftNav";
import { Outlet } from "react-router-dom";
import "./OrdersLayout.scss";

const OrdersPage = () => {
    const menuItems = [
        {
            id: "all-orders",
            label: "All orders",
            path: "/order/all",
            search: "?status=all",
            matchParams: { status: "all" }
        },
        {
            id: "awaiting-payment",
            label: "Awaiting payment",
            path: "/order/all",
            search: "?status=awaiting-payment",
            matchParams: { status: "awaiting-payment" }
        },
        {
            id: "awaiting-shipment",
            label: "Awaiting shipment",
            path: "/order/all",
            search: "?status=awaiting-shipment",
            matchParams: { status: "awaiting-shipment" }
        },
        {
            id: "paid-shipped",
            label: "Paid and shipped",
            path: "/order/all",
            search: "?status=paid-shipped",
            matchParams: { status: "paid-shipped" }
        },
        {
            id: "archived",
            label: "Archived",
            path: "/order/all",
            search: "?status=archived",
            matchParams: { status: "archived" }
        },
        // Divider
        { divider: true },
        {
            id: "cancellations",
            label: "Cancellations",
            path: "/order/cancellations",
        },
        {
            id: "returns",
            label: "Returns",
            path: "/order/returns"
        },
        {
            id: "requests",
            label: "Requests and disputes",
            path: "/order/requests"
        },
        {
            id: "shipping-labels",
            label: "Shipping labels",
            path: "/order/shipping-labels"
        },
        // Another divider
        { divider: true },
        {
            id: "shipping-preferences",
            label: "Shipping preferences",
            path: "/order/shipping-preferences"
        },
        {
            id: "automate-feedback",
            label: "Automate feedback",
            path: "/order/automate-feedback"
        },
        {
            id: "return-preferences",
            label: "Return preferences",
            path: "/order/return-preferences"
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

export default OrdersPage;
