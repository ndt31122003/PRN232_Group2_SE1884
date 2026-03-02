import React from "react";
import { Outlet } from "react-router-dom";
import LeftNav from "../../components/Navigation/LeftNav/LeftNav";

const ReportLayout = () => {
  const menuItems = [
    {
      id: "uploads",
      label: "Uploads",
      path: "/reports/uploads"
    },
    {
      id: "downloads",
      label: "Downloads",
      path: "/reports/downloads"
    },
    {
      id: "schedule",
      label: "Schedule",
      path: "/reports/schedule"
    }
  ];

  return (
    <div style={{ display: "flex", height: "100vh" }} className="container">
      <LeftNav menuItems={menuItems}/>
      <main style={{ flex: 1, overflowY: "auto" }}>
        <Outlet />
      </main>
    </div>
  );
};

export default ReportLayout;
