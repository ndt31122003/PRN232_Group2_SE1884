import React from "react";
import { Outlet } from "react-router-dom";
import LeftNav from "../../components/Navigation/LeftNav/LeftNav";
import "./PaymentsLayout.scss";

const menuItems = [
    { id: "summary", label: "Summary", path: "/payments/summary" },
    { id: "transactions", label: "All transactions", path: "/payments/transactions" },
    { id: "payouts", label: "Payouts", path: "/payments/payouts" },
    { id: "reports", label: "Reports", path: "/payments/reports" },
    { id: "taxes", label: "Taxes", path: "/payments/taxes", disabled: true }
];

const PaymentsLayout = () => {
    return (
        <div style={{ display: "flex", height: "100vh" }} className="container">
            <LeftNav menuItems={menuItems} />
            <main style={{ flex: 1, overflowY: "auto" }}>
                <Outlet />
            </main>
        </div>
    );
};

export default PaymentsLayout;
