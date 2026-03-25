import React, { useEffect, useMemo, useRef, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import AuthService from "../../services/AuthService";
import UserService from "../../services/UserService";
import "./Header.scss";
import Dropdown from "../Common/CustomDropdown/CustomDropdown";
import logoEbay from "../../assets/images/logo_ebay.png";
import NavBar from "../Navigation/TopNav/NavBar";
import Notice from "../Common/CustomNotification";
import { clearAuthSession, getStoredUser, mapUserProfile, storeUserInfo } from "../../utils/auth";
import STORAGE, { getStorage } from "../../lib/storage";
import { useAuth } from "../../Context/AuthContext";
const { logout } = AuthService;
const NavHeader = ({ setCurrentSession }) => {
  const navigate = useNavigate();
  const { logoutUser } = useAuth();
  const location = useLocation();
  const [searchQuery, setSearchQuery] = useState('');
  const [userInfo, setUserInfo] = useState(() => getStoredUser());

  const showComingSoon = (feature) => {
    Notice({
      msg: `${feature} is coming soon.`,
      isSuccess: false
    });
  };

  const myEbayItems = [
    {
      content: (
        <button type="button" className="top-nav__link" onClick={() => showComingSoon("My eBay summary")}>
          Summary
        </button>
      )
    },
    {
      content: (
        <button type="button" className="top-nav__link" onClick={() => showComingSoon("My eBay messages")}>
          Messages
        </button>
      )
    },
    {
      content: (
        <button type="button" className="top-nav__link" onClick={() => showComingSoon("Purchase history")}>
          Purchase History
        </button>
      )
    }
  ];
  const [activeNav, setActiveNav] = useState("overview");
  const [promotionMenuOpen, setPromotionMenuOpen] = useState(false);
  const promotionMenuRef = useRef(null);
  useEffect(() => {
    const path = location.pathname || "";

    let nextActiveNav = "overview";

    if (path.startsWith("/order")) {
      nextActiveNav = "orders";
    } else if (path.startsWith("/listings")) {
      nextActiveNav = "listings";
    } else if (path.startsWith("/marketing")) {
      nextActiveNav = "marketing";
    } else if (path.startsWith("/feedback")) {
      nextActiveNav = "feedback";
    } else if (path.startsWith("/payments")) {
      nextActiveNav = "payments";
    } else if (path.startsWith("/research")) {
      nextActiveNav = "research";
    } else if (path.startsWith("/reports")) {
      nextActiveNav = "reports";
    } else if (path.startsWith("/store") || path.startsWith("/stores")) {
      nextActiveNav = "store";
    }

    setActiveNav((prev) => (prev === nextActiveNav ? prev : nextActiveNav));
  }, [location.pathname]);

  const goTo = (navId, target) => {
    if (typeof target === "string" || typeof target === "number") {
      navigate(target);
      return;
    }

    if (target && typeof target === "object") {
      navigate(target);
    }
  };
  const navItems = [
    {
      id: "overview",
      label: "Overview",
      active: activeNav === "overview",
      onClick: () => goTo("overview", "/")
    },
    {
      id: "orders",
      label: "Orders",
      active: activeNav === "orders",
      onClick: () => goTo("orders", { pathname: "/order/all", search: "?status=awaiting-shipment" }),
      items: [
        {
          id: "all-orders",
          label: "All orders",
          onClick: () => goTo("orders", { pathname: "/order/all", search: "?status=all" })
        },
        {
          id: "awaiting-payment",
          label: "Awaiting payment",
          onClick: () => goTo("orders", { pathname: "/order/all", search: "?status=awaiting-payment" })
        },
        {
          id: "awaiting-shipment",
          label: "Awaiting shipment",
          onClick: () => goTo("orders", { pathname: "/order/all", search: "?status=awaiting-shipment" })
        },
        {
          id: "paid-shipped",
          label: "Paid and shipped",
          onClick: () => goTo("orders", { pathname: "/order/all", search: "?status=paid-shipped" })
        },
        {
          id: "cancelled",
          label: "Cancelled",
          onClick: () => goTo("orders", { pathname: "/order/all", search: "?status=cancellations" })
        }
      ]
    },
    {
      id: "listings",
      label: "Listings",
      active: activeNav === "listings",
      items: [
        {
          id: "createlisting",
          label: "Create listing",
          onClick: () => {
            if (!userInfo?.isEmailVerified) {
              Notice({
                msg: "Xác minh email trước khi tạo tin đăng.",
                desc: "Vào Account settings để hoàn tất xác minh email.",
                isSuccess: false
              });
              navigate("/account/settings");
              return;
            }

            if (!userInfo?.isPaymentVerified) {
              Notice({
                msg: "Xác minh phương thức thanh toán trước khi tạo tin đăng.",
                desc: "Vào Account settings để xác minh thanh toán.",
                isSuccess: false
              });
              navigate("/account/settings");
              return;
            }

            navigate("/listing-form");
          }
        },
        {
          id: "active",
          label: "Active",
          onClick: () => goTo("listings", "/listings/active")
        },
        {
          id: "unsold",
          label: "Unsold",
          onClick: () => goTo("listings", "/listings/unsold")
        },
        {
          id: "drafts",
          label: "Drafts",
          onClick: () => goTo("listings", "/listings/drafts")
        },
        {
          id: "scheduled",
          label: "Scheduled",
          onClick: () => goTo("listings", "/listings/scheduled")
        },
        {
          id: "ended",
          label: "Ended",
          onClick: () => goTo("listings", "/listings/ended")
        },


      ]
    },
    {
      id: "marketing",
      label: "Marketing",
      active: activeNav === "marketing",
      items: [
        {
          id: "marketing-summary",
          label: "Summary",
          onClick: () => {
            setActiveNav("marketing");
            setPromotionMenuOpen(false);
            navigate("/marketing");
          }
        },
        {
          id: "marketing-promotions",
          label: "Promotions",
          onClick: () => {
            setActiveNav("marketing");
            setPromotionMenuOpen(false);
            navigate("/marketing");
          }
        },
        {
          id: "marketing-offers",
          label: (
            <span className="marketing-menu__label">
              Offers
              <span className="marketing-menu__badge">NEW</span>
            </span>
          ),
          onClick: () => {
            setActiveNav("marketing");
            setPromotionMenuOpen(false);
            navigate("/marketing/offers");
          }
        },
        {
          id: "marketing-bids",
          label: (
            <span className="marketing-menu__label">
              Bids
              <span className="marketing-menu__badge">NEW</span>
            </span>
          ),
          onClick: () => {
            setActiveNav("marketing");
            setPromotionMenuOpen(false);
            navigate("/marketing/bids");
          }
        },
        {
          id: "marketing-buyer-groups",
          label: "Buyer groups",
          onClick: () => {
            setActiveNav("marketing");
            showComingSoon("Buyer groups");
          }
        },
        {
          id: "marketing-social",
          label: "Social",
          onClick: () => {
            setActiveNav("marketing");
            showComingSoon("Social");
          }
        }
      ]
    },
    {
      id: "feedback",
      label: "Feedback",
      active: activeNav === "feedback",
      items: [
        {
          id: "summary",
          label: "Summary",
          onClick: () => goTo("feedback", "/feedback")
        }
      ]
    },
    {
      id: "store",
      label: "Store",
      active: activeNav === "store",
      items: [
        {
          id: "my-stores",
          label: "My Stores",
          onClick: () => goTo("store", "/stores")
        },
        {
          id: "settings",
          label: "Settings",
          onClick: () => goTo("store", "/store/settings")
        },
        {
          id: "policies",
          label: "Policies",
          onClick: () => goTo("store", "/store/policies")
        },
        {
          id: "subscription",
          label: "Subscription & Fees",
          onClick: () => goTo("store", "/store/subscription")
        }
      ]
    },
    {
      id: "performance",
      label: "Performance",
      active: activeNav === "performance",
      items: [
        {
          id: "performance-summary",
          label: "Summary",
          onClick: () => goTo("performance", "/performance/summary")
        },
        {
          id: "seller-level",
          label: "Seller level",
          onClick: () => console.log("Seller level")
        },
        {
          id: "performance-sales",
          label: "Sales",
          onClick: () => goTo("performance", "/performance/sales")
        },
        {
          id: "performance-traffic",
          label: "Traffic",
          onClick: () => goTo("performance", "/performance/traffic")
        },
        {
          id: "service-metrics",
          label: "Service metrics",
          onClick: () => console.log("Service metrics")
        },
        {
          id: "performance-stock",
          label: "Stock",
          onClick: () => goTo("performance", "/performance/stock")
        }
      ]
    },
    {
      id: "payments",
      label: "Payments",
      active: activeNav === "payments",
      items: [
        {
          id: "summary",
          label: "Summary",
          onClick: () => goTo("payments", "/payments/summary")
        },
        {
          id: "transactions",
          label: "All transactions",
          onClick: () => goTo("payments", "/payments/transactions")
        },
        {
          id: "payouts",
          label: "Payouts",
          onClick: () => goTo("payments", "/payments/payouts")
        },
        {
          id: "reports",
          label: "Reports",
          onClick: () => goTo("payments", "/payments/reports")
        },
        {
          id: "taxes",
          label: "Taxes",
          onClick: () => console.log("Taxes")
        },

      ]
    },
    {
      id: "research",
      label: "Research",
      active: activeNav === "research",
      items: [
        {
          id: "terapeak",
          label: "Product Research",
          onClick: () => goTo("research", "/research/product")
        },
        {
          id: "market-insights",
          label: "Sourcing insights",
          onClick: () => goTo("research", "/research/sourcing")
        }
      ]
    },
    {
      id: "reports",
      label: "Reports",
      active: activeNav === "reports",
      items: [
        {
          id: "uploads",
          label: "Uploads",
          onClick: () => goTo("reports", "/reports/uploads")
        },
        {
          id: "downloads",
          label: "Downloads",
          onClick: () => goTo("reports", "/reports/downloads")
        },
        {
          id: "schedule",
          label: "Schedule",
          onClick: () => goTo("reports", "/reports/schedule")
        }
      ]
    }
  ];
  const createPromotionOptions = [
    {
      id: "coupon",
      label: "Coupon",
      onClick: () => {
        setPromotionMenuOpen(false);
        navigate("/marketing/coupons/create");
      }
    },
    {
      id: "order-discount",
      label: "Order discount",
      onClick: () => {
        setPromotionMenuOpen(false);
        navigate("/marketing/order-discounts/create");
      }
    },
    {
      id: "sale-event",
      label: "Sale event",
      onClick: () => {
        setPromotionMenuOpen(false);
        navigate("/marketing/sale-events/create");
      }
    },
    {
      id: "shipping-discount",
      label: "Shipping discount",
      onClick: () => {
        setPromotionMenuOpen(false);
        navigate("/marketing/shipping-discounts/create");
      }
    },
    {
      id: "volume-pricing",
      label: "Volume pricing",
      onClick: () => {
        setPromotionMenuOpen(false);
        navigate("/marketing/volume-pricing/create");
      }
    }
  ];
  const notifications = [];

  useEffect(() => {
    setPromotionMenuOpen(false);
    if (location.pathname.startsWith("/orders")) {
      setActiveNav("orders");
    } else if (location.pathname.startsWith("/listings")) {
      setActiveNav("listings");
    } else if (location.pathname.startsWith("/marketing")) {
      setActiveNav("marketing");
    } else if (location.pathname.startsWith("/research")) {
      setActiveNav("research");
    } else if (location.pathname.startsWith("/performance")) {
      setActiveNav("performance");
    } else if (location.pathname.startsWith("/payments")) {
      setActiveNav("payments");
    } else {
      setActiveNav("overview");
    }
  }, [location.pathname]);

  useEffect(() => {
    if (!promotionMenuOpen) {
      return;
    }

    const handleClickOutside = (event) => {
      if (promotionMenuRef.current && !promotionMenuRef.current.contains(event.target)) {
        setPromotionMenuOpen(false);
      }
    };

    const handleEscape = (event) => {
      if (event.key === "Escape") {
        setPromotionMenuOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    document.addEventListener("keydown", handleEscape);

    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
      document.removeEventListener("keydown", handleEscape);
    };
  }, [promotionMenuOpen]);

  useEffect(() => {
    let isMounted = true;
    const stored = getStoredUser();
    setUserInfo(stored);

    const hydrateProfile = async () => {
      if (!stored?.id) {
        return;
      }

      try {
        const profile = await UserService.getById(stored.id);
        if (!isMounted || !profile) {
          return;
        }

        const normalized = mapUserProfile(profile, stored);
        if (normalized) {
          storeUserInfo(normalized, { notify: true });
          setUserInfo(normalized);
        }
      } catch (error) {
        // Unable to refresh profile silently
      }
    };

    hydrateProfile();

    const handleUserInfoUpdated = () => {
      setUserInfo(getStoredUser());
    };

    if (typeof window !== "undefined") {
      window.addEventListener("user-info-updated", handleUserInfoUpdated);
    }

    return () => {
      isMounted = false;
      if (typeof window !== "undefined") {
        window.removeEventListener("user-info-updated", handleUserInfoUpdated);
      }
    };
  }, []);

  const accountDisplayName = useMemo(() => {
    if (userInfo?.name) {
      return userInfo.name;
    }
    if (userInfo?.email) {
      return userInfo.email.split("@")[0];
    }
    return "there";
  }, [userInfo]);

  const userInitials = useMemo(() => {
    const source = userInfo?.name || userInfo?.email;
    if (!source) {
      return "?";
    }

    const words = source.trim().split(/\s+/).slice(0, 2);
    return words
      .map((word) => word.charAt(0).toUpperCase())
      .join("")
      .slice(0, 2);
  }, [userInfo]);

  const handleAccountSettings = () => {
    navigate("/account/settings");
  };

  const handleLogoutConfirm = async () => {
    const storedRefreshToken = getStorage(STORAGE.REFRESH_TOKEN);
    try {
      await logout(storedRefreshToken ?? null);
    } catch (error) {
      // Remote logout is best-effort; proceed with local cleanup.
    } finally {
      clearAuthSession();
      if (typeof logoutUser === "function") {
        logoutUser();
      }
      if (typeof setCurrentSession === "function") {
        setCurrentSession(null);
      }
      navigate("/login", { replace: true });
    }
  };

  const accountMenuItems = [
    {
      content: (
        <div className="account-menu__profile">
          <div className="account-menu__avatar" aria-hidden="true">
            {userInitials}
          </div>
          <div className="account-menu__details">
            <p className="account-menu__name">{userInfo?.name || "Authenticated user"}</p>
            {userInfo?.email && (
              <a className="account-menu__email" href={`mailto:${userInfo.email}`}>
                {userInfo.email}
              </a>
            )}
          </div>
        </div>
      ),
      className: "account-menu__profile-item",
      disableHover: true,
      keepOpen: true,
    },
    { divider: true },
    {
      content: <span className="account-menu__action">Account settings</span>,
      onClick: handleAccountSettings,
    },
    {
      content: <span className="account-menu__action">Sign out</span>,
      onClick: handleLogoutConfirm,
    },
  ];

  const handleLogoClick = () => {
    navigate("/");
    setCurrentSession(null);
  };
  return (
    <div className="seller-hub">
      {/* Top Navigation Bar */}
      <nav className="top-nav">
        <div className="top-nav__container">
          <div className="top-nav__group">
            <Dropdown
              label={
                <span className="top-nav__user-label">{`Hi ${accountDisplayName}!`}</span>
              }
              menuItems={accountMenuItems}
              width={260}
            />
            <button type="button" className="top-nav__link" onClick={() => showComingSoon("Daily Deals")}>
              Daily Deals
            </button>
            <button type="button" className="top-nav__link" onClick={() => showComingSoon("Brand Outlet")}>
              Brand Outlet
            </button>
            <button type="button" className="top-nav__link" onClick={() => showComingSoon("Gift Cards")}>
              Gift Cards
            </button>
            <button type="button" className="top-nav__link" onClick={() => showComingSoon("Help & Contact")}>
              Help & Contact
            </button>
          </div>

          <div className="top-nav__group">
            <div className="top-nav__ship-wrapper">
              <span className="top-nav__badge">
                <svg className="top-nav__badge-icon" fill="currentColor" viewBox="0 0 24 24">
                  <path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z" />
                </svg>
              </span>
              <button className="top-nav__button top-nav__button--dropdown">
                Ship to
                <svg className="top-nav__icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                </svg>
              </button>
            </div>
            <button type="button" className="top-nav__link" onClick={() => showComingSoon("Sell tools")}>
              Sell
            </button>
            <button className="top-nav__button top-nav__button--dropdown">
              Watchlist
              <svg className="top-nav__icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
              </svg>
            </button>
            <Dropdown label="My eBay" menuItems={myEbayItems} />

            <Dropdown
              label={
                <svg className="top-nav__icon top-nav__icon--large" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
                </svg>}


              menuItems={
                notifications.length > 0
                  ? notifications.map((notif) => ({
                    content: (
                      <div className="notification-item">
                        <p className="text-sm">{notif.text}</p>
                        <span className="text-xs text-gray-400">{notif.time}</span>
                      </div>
                    ),
                  }))
                  : [
                    {
                      content: (
                        <h2 className="px-4 py-6 text-center text-gray-400 font-bold">
                          There are no new notifications.
                        </h2>
                      ),
                    },
                  ]
              }
              icon={<span></span>}
              header={notifications.length > 0 ? "Thông báo mới" : null}
            />
            <button className="top-nav__button top-nav__button--icon">
              <svg className="top-nav__icon top-nav__icon--large" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
              </svg>
            </button>
          </div>
        </div>
      </nav>

      {/* Main Header */}
      <header className="header ">
        <div className="header__container">
          <div className="header__left">
            <img
              src={logoEbay}
              alt="Ebay Logo"
              className="header__logo"
              onClick={handleLogoClick}
            />

            <button className="category-dropdown">
              <span className="category-dropdown__text">Shop by category</span>
              <svg className="category-dropdown__icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
              </svg>
            </button>
          </div>

          {/* Search Bar */}
          <div className="search">
            <div className="search__wrapper">
              <input
                type="text"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                placeholder="Search for anything"
                className="search__input"
              />
              <div className="search__category">
                {/* <span className="search__category-text">All Categories</span>
                <svg className="search__category-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                </svg> */}
                <div className="dropdown dropdown-start">
                  <div tabIndex={0} role="button" className="search__category-text btn"><span className="search__category-text">All Categories </span>
                    <svg className="search__category-icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                    </svg>
                  </div>
                  <ul tabIndex={0} className="dropdown-content menu bg-base-100 rounded-box z-1 w-52 p-2 shadow-sm">
                    <li>
                      <button type="button" onClick={() => showComingSoon("Category filter")}>Item 1</button>
                    </li>
                    <li>
                      <button type="button" onClick={() => showComingSoon("Category filter")}>Item 2</button>
                    </li>
                  </ul>
                </div>
              </div>
            </div>

          </div>
          <button className="search__button">
            Search
          </button>
          <button type="button" className="header__advanced-link" onClick={() => showComingSoon("Advanced search")}>
            Advanced
          </button>
        </div>
      </header>

      {/* Seller Hub Section */}
      <section className="hub">
        <div className="hub__container">
          <div className="hub__info">
            <h1 className="hub__title">Seller Hub</h1>
            <button type="button" className="hub__username" onClick={() => showComingSoon("Seller profile")}>mnh20</button>
            <span className="hub__rating">( 0 )</span>
          </div>

          <button className="messages">
            <svg className="messages__icon" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z" />
            </svg>
            <span className="messages__text">Messages</span>
            <span className="messages__badge">3</span>
          </button>
        </div>
      </section>
      <NavBar items={navItems} />
      {activeNav === "marketing" && (
        <div className="marketing-toolbar">
          <button
            type="button"
            className="marketing-toolbar__feedback"
            onClick={() => Notice({ msg: "Thanks for the feedback!", isSuccess: true })}
          >
            Give us feedback
          </button>
          <div className="marketing-toolbar__actions" ref={promotionMenuRef}>
            <button
              type="button"
              className="marketing-toolbar__button"
              onClick={() => setPromotionMenuOpen((prev) => !prev)}
              aria-haspopup="menu"
              aria-expanded={promotionMenuOpen}
            >
              Create promotion
              <svg
                className={`marketing-toolbar__caret ${promotionMenuOpen ? "marketing-toolbar__caret--open" : ""}`}
                width="16"
                height="16"
                viewBox="0 0 16 16"
                fill="none"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path d="M4 6L8 10L12 6" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round" />
              </svg>
            </button>
            {promotionMenuOpen && (
              <ul className="marketing-toolbar__menu" role="menu">
                {createPromotionOptions.map((option) => (
                  <li key={option.id} role="none">
                    <button
                      type="button"
                      className="marketing-toolbar__menu-item"
                      onClick={option.onClick}
                      role="menuitem"
                    >
                      {option.label}
                    </button>
                  </li>
                ))}
              </ul>
            )}
          </div>
        </div>
      )}
    </div>

  );
};

export default NavHeader;
