import React from "react";
import { useNavigate } from "react-router-dom";
import "./MarketingSummary.scss";

const MarketingSummary = () => {
  const navigate = useNavigate();

  const handleNavigation = (path) => {
    navigate(path);
  };

  const marketingTools = [
    {
      id: "social",
      title: "Social sharing",
      desc: "Help to drive more visits to your listings from social media.",
      actionText: "Share to social",
      icon: (
        <svg viewBox="0 0 24 24" width="20" height="20" stroke="currentColor" strokeWidth="2" fill="none" strokeLinecap="round" strokeLinejoin="round">
          <circle cx="18" cy="5" r="3"></circle>
          <circle cx="6" cy="12" r="3"></circle>
          <circle cx="18" cy="19" r="3"></circle>
          <line x1="8.59" y1="13.51" x2="15.42" y2="17.49"></line>
          <line x1="15.41" y1="6.51" x2="8.59" y2="10.49"></line>
        </svg>
      ),
      path: "/marketing/social"
    },
    {
      id: "volume",
      title: "Volume pricing",
      desc: "Encourage buyers to purchase the same item in bulk.",
      actionText: "Create volume pricing",
      icon: (
        <svg viewBox="0 0 24 24" width="20" height="20" stroke="currentColor" strokeWidth="2" fill="none" strokeLinecap="round" strokeLinejoin="round">
          <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
          <path d="M7 11V7a5 5 0 0110 0v4"></path>
        </svg>
      ),
      path: "/marketing/volume-pricing"
    },
    {
      id: "sale",
      title: "Sale event",
      desc: "Lower prices for a limited time to help move inventory faster.",
      actionText: "Create sale event",
      icon: (
        <svg viewBox="0 0 24 24" width="20" height="20" stroke="currentColor" strokeWidth="2" fill="none" strokeLinecap="round" strokeLinejoin="round">
          <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
          <path d="M7 11V7a5 5 0 0110 0v4"></path>
        </svg>
      ),
      path: "/marketing/sale-events/create"
    },
    {
      id: "coupon",
      title: "Coupon",
      desc: "Give buyers a discount they can apply at checkout.",
      actionText: "Create coupon",
      icon: (
        <svg viewBox="0 0 24 24" width="20" height="20" stroke="currentColor" strokeWidth="2" fill="none" strokeLinecap="round" strokeLinejoin="round">
          <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
          <path d="M7 11V7a5 5 0 0110 0v4"></path>
        </svg>
      ),
      path: "/marketing/coupons/create"
    },
    {
      id: "automated",
      title: "Automated offer",
      desc: "Schedule and send offers to interested buyers to save time.",
      actionText: "Send automated offer",
      icon: (
        <svg viewBox="0 0 24 24" width="20" height="20" stroke="currentColor" strokeWidth="2" fill="none" strokeLinecap="round" strokeLinejoin="round">
          <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
          <path d="M7 11V7a5 5 0 0110 0v4"></path>
        </svg>
      ),
      path: "/marketing/offers"
    },
    {
      id: "shipping",
      title: "Shipping discount",
      desc: "Low cost or free shipping to help close the sale.",
      actionText: "Create shipping discount",
      icon: (
        <svg viewBox="0 0 24 24" width="20" height="20" stroke="currentColor" strokeWidth="2" fill="none" strokeLinecap="round" strokeLinejoin="round">
          <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
          <path d="M7 11V7a5 5 0 0110 0v4"></path>
        </svg>
      ),
      path: "/marketing/shipping-discounts"
    }
  ];

  return (
    <div className="marketing-summary">
      <h1 className="marketing-summary__title">Summary</h1>
      
      <div className="marketing-summary__banner">
        <div className="marketing-summary__banner-content">
          <h2>Get set to grow your business</h2>
          <p>Discover more ways to connect to buyers, market your business, and create promotions.</p>
          <div className="marketing-summary__banner-actions">
            <button className="btn-get-started" onClick={() => handleNavigation("/marketing")}>
              Get started
              <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" className="icon-caret">
                <polyline points="6 9 12 15 18 9"></polyline>
              </svg>
            </button>
            <button className="btn-learn-more">Learn more</button>
          </div>
        </div>
        <div className="marketing-summary__banner-image">
          {/* Simple representative geometric SVG mimicking the illustration */}
          <svg width="240" height="120" viewBox="0 0 240 120" fill="none" xmlns="http://www.w3.org/2000/svg">
            <rect x="140" y="20" width="50" height="40" fill="#fff" stroke="#ccc" strokeWidth="2" rx="4" />
            <rect x="150" y="30" width="30" height="20" fill="#a5f3e0" rx="2" />
            
            {/* Person 1 (left) */}
            <circle cx="100" cy="40" r="10" fill="#ff6b9a" />
            <rect x="90" y="55" width="20" height="65" fill="#00d890" rx="4" />
            <rect x="85" y="55" width="8" height="35" fill="#00d890" rx="4" />
            <rect x="107" y="55" width="8" height="30" fill="#00d890" rx="4" transform="rotate(-30 110 60)" />
            
            {/* Person 2 (right) */}
            <circle cx="210" cy="35" r="10" fill="#a4725e" />
            <rect x="200" y="50" width="20" height="35" fill="#003554" rx="4" />
            <rect x="204" y="85" width="12" height="35" fill="#003554" rx="2" />
            <rect x="195" y="55" width="8" height="25" fill="#a2dc48" rx="4" transform="rotate(20 195 55)" />
            <rect x="215" y="55" width="8" height="35" fill="#a2dc48" rx="4" />
          </svg>
        </div>
      </div>

      <div className="marketing-tools">
        <div className="marketing-tools__header">
          <h2>Marketing tools</h2>
          <p>Use a range of tactics to connect to buyers, grow visibility and sales.</p>
        </div>
        <div className="marketing-tools__grid">
          {marketingTools.map((tool) => (
            <div key={tool.id} className="tool-card" onClick={() => handleNavigation(tool.path)}>
              <div className="tool-card__header">
                <span className="tool-card__icon">{tool.icon}</span>
                <h3 className="tool-card__title">{tool.title}</h3>
              </div>
              <p className="tool-card__desc">{tool.desc}</p>
              <span className="tool-card__action">{tool.actionText}</span>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default MarketingSummary;
