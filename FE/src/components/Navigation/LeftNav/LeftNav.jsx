import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import "./LeftNav.scss";

const LeftNav = ({ menuItems, className = "", collapsible = true }) => {
    const [collapsed, setCollapsed] = useState(false);
    const [openGroups, setOpenGroups] = useState({});
    const location = useLocation();
    const navigate = useNavigate();

    const toggleGroup = (groupId) => {
        setOpenGroups((prev) => ({
            ...prev,
            [groupId]: !prev[groupId],
        }));
    };

    const renderMenuItem = (item, index) => {
        const {
            id,
            label,
            icon,
            items,
            divider,
            disabled,
            path,
            search,
            matchParams,
            onClick,
        } = item;

        // Divider
        if (divider) {
            return <div key={`divider-${index}`} className="left-nav__divider" />;
        }

        // Group
        if (items && items.length > 0) {
            return (
                <div key={id || index} className="left-nav__group">
                    <button
                        type="button"
                        className={`left-nav__item left-nav__item--group ${openGroups[id] ? "left-nav__item--open" : ""
                            }`}
                        onClick={() => toggleGroup(id)}
                        disabled={disabled}
                    >
                        {icon && <span className="left-nav__icon">{icon}</span>}
                        <span className="left-nav__label">{label}</span>
                        <svg
                            className="left-nav__arrow"
                            width="12"
                            height="12"
                            viewBox="0 0 12 12"
                            fill="currentColor"
                        >
                            <path d="M6 8L2 4h8L6 8z" />
                        </svg>
                    </button>
                    {openGroups[id] && (
                        <div className="left-nav__submenu">
                            {items.map((subItem, subIndex) =>
                                renderMenuItem(subItem, `${index}-${subIndex}`)
                            )}
                        </div>
                    )}
                </div>
            );
        }

        // Check if active (so sánh URL hiện tại với path)
        const isActive = (() => {
            if (!path) {
                return false;
            }

            const pathMatches =
                location.pathname === path || location.pathname.startsWith(`${path}/`);

            if (!pathMatches) {
                return false;
            }

            if (matchParams && Object.keys(matchParams).length > 0) {
                const currentParams = new URLSearchParams(location.search);
                return Object.entries(matchParams).every(([key, expected]) => {
                    const actualValue = currentParams.get(key);

                    if (typeof expected === "string") {
                        if (expected.toLowerCase() === "all" && (actualValue === null || actualValue === "")) {
                            return true;
                        }
                        return (actualValue ?? "").toLowerCase() === expected.toLowerCase();
                    }

                    if (Array.isArray(expected)) {
                        const normalizedActual = (actualValue ?? "").toLowerCase();
                        return expected
                            .map((value) => (typeof value === "string" ? value.toLowerCase() : String(value)))
                            .includes(normalizedActual);
                    }

                    return actualValue === expected;
                });
            }

            if (search) {
                const normalizedSearch = search.startsWith("?") ? search.slice(1) : search;
                const targetParams = new URLSearchParams(normalizedSearch);
                const currentParams = new URLSearchParams(location.search);

                return Array.from(targetParams.entries()).every(([key, value]) => currentParams.get(key) === value);
            }

            return true;
        })();

        return (
            <button
                type="button"
                key={id || index}
                className={`left-nav__item ${isActive ? "left-nav__item--active" : ""} ${disabled ? "left-nav__item--disabled" : ""
                    }`}
                onClick={() => {
                    if (disabled) return;
                    if (path) {
                        if (search) {
                            const normalizedSearch = search.startsWith("?") ? search : `?${search}`;
                            navigate({ pathname: path, search: normalizedSearch });
                        } else {
                            navigate(path);
                        }
                    }
                    if (onClick) onClick();
                }}
            >
                {icon && <span className="left-nav__icon">{icon}</span>}
                <span className="left-nav__label">{label}</span>
            </button>
        );
    };

    const isCollapsed = collapsible && collapsed;

    return (
        <aside
            className={`left-nav ${isCollapsed ? "left-nav--collapsed" : ""} ${className}`.trim()}
        >
            {collapsible && (
                <button
                    type="button"
                    className="left-nav__collapse-btn"
                    onClick={() => setCollapsed((prev) => !prev)}
                    title={collapsed ? "Expand" : "Collapse"}
                >
                    <svg width="16" height="16" viewBox="0 0 16 16" fill="currentColor">
                        <rect x="2" y="2" width="12" height="2" />
                        <rect x="2" y="7" width="12" height="2" />
                        <rect x="2" y="12" width="12" height="2" />
                    </svg>
                    <span className="left-nav__collapse-text">Collapse</span>
                </button>
            )}

            <nav className="left-nav__menu">
                {menuItems && menuItems.map((item, index) => renderMenuItem(item, index))}
            </nav>
        </aside>
    );
};

export default LeftNav;
