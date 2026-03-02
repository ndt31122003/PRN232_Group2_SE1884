// src/components/NavBar/NavBar.jsx
import React from "react";
import "./NavBar.scss";

const NavBar = ({ items = [], className = "" }) => {
    const renderNavItem = (item, index) => {
        const {
            id,
            label,
            active,
            href,
            onClick,
            items: dropdownItems,
            disabled
        } = item;

        // Render item with dropdown
        if (dropdownItems && dropdownItems.length > 0) {
            return (
                <div key={id || index} className="navbar__item-wrapper">
                    <div className="dropdown dropdown-hover">
                        <div
                            tabIndex={0}
                            role="button"
                            className={`navbar__item navbar__item--dropdown ${active ? "navbar__item--active" : ""
                                } ${disabled ? "navbar__item--disabled" : ""}`}
                            onClick={(event) => {
                                if (disabled) {
                                    return;
                                }
                                if (typeof onClick === "function") {
                                    onClick(event);
                                }
                            }}
                        >
                            <span className="navbar__label">{label}</span>
                            <svg
                                className="navbar__arrow"
                                width="12"
                                height="12"
                                viewBox="0 0 12 12"
                                fill="currentColor"
                            >
                                <path d="M6 8L2 4h8L6 8z" />
                            </svg>
                        </div>
                        <ul
                            tabIndex={0}
                            className="dropdown-content navbar__dropdown-menu"
                        >
                            {dropdownItems.map((dropItem, dropIndex) => (
                                <li key={dropItem.id || dropIndex}>
                                    <a
                                        href={dropItem.href || "#"}
                                        onClick={(e) => {
                                            if (dropItem.onClick) {
                                                e.preventDefault();
                                                dropItem.onClick();
                                            }
                                        }}
                                        className={`navbar__dropdown-item ${dropItem.active ? "navbar__dropdown-item--active" : ""
                                            }`}
                                    >
                                        {dropItem.label}
                                    </a>
                                </li>
                            ))}
                        </ul>
                    </div>
                </div>
            );
        }

        // Render regular item
        return (
            <div key={id || index} className="navbar__item-wrapper">
                <a
                    href={href || "#"}
                    onClick={(e) => {
                        if (onClick) {
                            e.preventDefault();
                            onClick();
                        }
                    }}
                    className={`navbar__item ${active ? "navbar__item--active" : ""
                        } ${disabled ? "navbar__item--disabled" : ""}`}
                >
                    <span className="navbar__label">{label}</span>
                </a>
            </div>
        );
    };

    return (
        <nav className={`navbar ${className}`}>
            <div className="navbar__container">
                {items.map((item, index) => renderNavItem(item, index))}
            </div>
        </nav>
    );
};

export default NavBar;