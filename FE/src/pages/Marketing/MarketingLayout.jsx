import React from "react";
import { Outlet, useNavigate, useLocation } from "react-router-dom";
import "./MarketingLayout.scss";

const MarketingLayout = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const sidebarItems = [
    {
      id: "summary",
      label: "Summary",
      path: "/marketing/summary",
      available: true
    },
    {
      id: "promotions",
      label: "Promotions",
      path: "/marketing",
      available: true
    },
    {
      id: "offers",
      label: "Offers",
      path: "/marketing/offers",
      available: false,
      badge: "NEW"
    },
    {
      id: "buyer-groups",
      label: "Buyer groups",
      path: "/marketing/buyer-groups",
      available: false
    },
    {
      id: "social",
      label: "Social",
      path: "/marketing/social",
      available: false
    }
  ];

  const isActive = (path) => {
    if (path === "/marketing") {
      return location.pathname === "/marketing";
    }
    return location.pathname.startsWith(path);
  };

  const handleNavClick = (item) => {
    if (item.available) {
      navigate(item.path);
    }
  };

  return (
    <div className="marketing-layout">
      <aside className="marketing-layout__sidebar">
        <nav className="marketing-sidebar">
          <ul className="marketing-sidebar__list">
            {sidebarItems.map((item) => (
              <li
                key={item.id}
                className={`marketing-sidebar__item ${
                  isActive(item.path) ? "marketing-sidebar__item--active" : ""
                } ${!item.available ? "marketing-sidebar__item--disabled" : ""}`}
                onClick={() => handleNavClick(item)}
              >
                <span className="marketing-sidebar__label">
                  {item.label}
                  {item.badge && (
                    <span className="marketing-sidebar__badge">{item.badge}</span>
                  )}
                </span>
              </li>
            ))}
          </ul>
        </nav>
      </aside>
      <main className="marketing-layout__content">
        <Outlet />
      </main>
    </div>
  );
};

export default MarketingLayout;
