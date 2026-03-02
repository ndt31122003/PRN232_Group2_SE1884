import React, { useEffect, useRef, useState } from "react";
import "./CustomDropdown.scss";

const Dropdown = ({ label, icon, menuItems, width, header }) => {
    const [isOpen, setIsOpen] = useState(false);
    const [position, setPosition] = useState("left");
    const [menuStyles, setMenuStyles] = useState({});
    const buttonRef = useRef();

    useEffect(() => {
        if (!isOpen) {
            return;
        }

        const menuWidth = width || 180;

        const updatePosition = () => {
            if (!buttonRef.current) {
                return;
            }

            const rect = buttonRef.current.getBoundingClientRect();
            const alignRight = window.innerWidth - rect.right < menuWidth;
            setPosition(alignRight ? "right" : "left");

            const calculatedLeft = alignRight
                ? rect.right + window.scrollX - menuWidth
                : rect.left + window.scrollX;

            setMenuStyles({
                top: rect.bottom + window.scrollY + 8,
                left: calculatedLeft,
                minWidth: menuWidth,
            });
        };

        updatePosition();

        window.addEventListener("resize", updatePosition);
        window.addEventListener("scroll", updatePosition, true);

        return () => {
            window.removeEventListener("resize", updatePosition);
            window.removeEventListener("scroll", updatePosition, true);
        };
    }, [isOpen, width]);

    const handleItemClick = (item) => {
        if (typeof item?.onClick === "function") {
            item.onClick();
        }

        if (!item?.keepOpen) {
            setIsOpen(false);
        }
    };

    return (
        <div
            className="dropdown relative inline-block"
            onMouseEnter={() => setIsOpen(true)}
            onMouseLeave={() => setIsOpen(false)}
        >
            {/* Button */}
            <button ref={buttonRef} className="top-nav__button top-nav__button--dropdown flex items-center">
                {label}
                {icon ?? (
                    <svg
                        className="top-nav__icon ml-1"
                        fill="none"
                        stroke="currentColor"
                        viewBox="0 0 24 24"
                    >
                        <path
                            strokeLinecap="round"
                            strokeLinejoin="round"
                            strokeWidth={2}
                            d="M19 9l-7 7-7-7"
                        />
                    </svg>
                )}
            </button>

            {/* Dropdown Menu */}
            {isOpen && (
                <ul
                    className={`dropdown__menu bg-white border rounded-xl shadow-md mt-1 z-50 dropdown__menu--${position}`}
                    style={menuStyles}
                >
                    {header && <li className="px-4 py-2 font-bold border-b">{header}</li>}
                    {menuItems.map((item, index) => (
                        item?.divider ? (
                            <li key={`divider-${index}`} className="dropdown__divider" />
                        ) : (
                            (() => {
                                const isInteractive = Boolean(item?.onClick);
                                return (
                            <li
                                key={index}
                                onClick={isInteractive ? () => handleItemClick(item) : undefined}
                                onKeyDown={
                                    isInteractive
                                        ? (event) => {
                                              if (event.key === "Enter" || event.key === " ") {
                                                  event.preventDefault();
                                                  handleItemClick(item);
                                              }
                                          }
                                        : undefined
                                }
                                className={`dropdown__item px-4 py-2 rounded-xl${
                                    item?.onClick && !item?.disableHover ? " dropdown__item--action" : ""
                                }${item?.className ? ` ${item.className}` : ""}`}
                                tabIndex={isInteractive ? 0 : undefined}
                                role={isInteractive ? "menuitem" : undefined}
                            >
                                {item.content}
                            </li>
                                );
                            })()
                        )
                    ))}
                </ul>
            )}
        </div>
    );
};

export default Dropdown;
