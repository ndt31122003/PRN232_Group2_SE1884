import React from "react";
import { Outlet } from "react-router-dom";
import LeftNav from "../../components/Navigation/LeftNav/LeftNav";
import "./ResearchLayout.scss";

const ResearchLayout = () => {
  const menuItems = [
    {
      id: "product-research",
      label: "Product research",
      path: "/research/product"
    },
    {
      id: "sourcing-insights",
      label: "Sourcing insights",
      path: "/research/sourcing"
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

export default ResearchLayout;
