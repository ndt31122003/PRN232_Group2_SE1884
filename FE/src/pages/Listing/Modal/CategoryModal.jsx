import React, { useRef, useState, useEffect } from "react";

import { ReactComponent as ChevronRightIcon } from "@ebay/skin/dist/svg/icon/icon-chevron-right-16.svg";
import { ReactComponent as ChevronLeftIcon } from "@ebay/skin/dist/svg/icon/icon-chevron-left-24.svg";
import { ReactComponent as SearchIcon } from "@ebay/skin/dist/svg/icon/icon-search-16.svg";
import { ReactComponent as ClearIcon } from "@ebay/skin/dist/svg/icon/icon-clear-16.svg";

import { EbayTextbox, EbayTextboxPrefixIcon, EbayTextboxPostfixIcon } from '@ebay/ui-core-react/ebay-textbox';
import { EbayFakeLink } from '@ebay/ui-core-react/ebay-fake-link';
import CategoryService from '../../../services/CategoryService';

const CategoryModal = ({ isOpen, onClose, onSelect, initialSelected }) => {
    const [path, setPath] = useState([]); // stack of parents
    const [categories, setCategories] = useState([]);
    const [loading, setLoading] = useState(false);
    const [query, setQuery] = useState("");
    const [selected, setSelected] = useState(null); // optional if want to show chosen before Done
    const containerRef = useRef(null);

    const selectedPath = selected || [];

    // lock body scroll while modal open
    useEffect(() => {
        if (isOpen) {
            document.body.style.overflow = "hidden";

            // Mở modal thì luôn load root
            setPath([]);
            loadCategories(null);

            // Giữ selected để hiển thị "Selected" section
            if (initialSelected && initialSelected.length > 0) {
                setSelected(initialSelected);
            }
        } else {
            document.body.style.overflow = "auto";
        }
        return () => {
            document.body.style.overflow = "auto";
        };
    }, [isOpen, initialSelected]);


    const loadCategories = async (parentId = null) => {
        setLoading(true);
        try {
            const data = await CategoryService.getCategories(parentId);
            setCategories(Array.isArray(data) ? data : []);
        } finally {
            setLoading(false);
        }
    };

    const handleClickBreadcrumb = (idx, fromSelected = false) => {
        if (fromSelected) {
            // click breadcrumb ở phần Selected
            if (idx === null) {
                // click All categories
                setPath([]);
                setSelected(null);
                loadCategories(null);
            } else {
                const newPath = selectedPath.slice(0, idx + 1);
                setPath(newPath);
                loadCategories(newPath[newPath.length - 1]?.id || null); // safe
            }
        } else {
            // click breadcrumb trong path hiện tại
            if (idx === null) {
                setPath([]);
                loadCategories(null);
            } else {
                const newPath = path.slice(0, idx + 1);
                setPath(newPath);
                loadCategories(newPath[newPath.length - 1]?.id || null);
            }
        }

        setQuery("");
        if (containerRef.current) containerRef.current.scrollTop = 0;
    };

    const handleClickCategory = async (cat) => {
        if (cat.isLeaf) {
            setLoading(true);
            try {
                const fullPath = [...path, cat];
                setSelected(fullPath);
                const detail = await CategoryService.getCategoryDetail(cat.id);
                if (onSelect) {
                    onSelect({ id: cat.id, path: fullPath, detail });
                }
            } finally {
                setLoading(false);
            }

            setPath([]);
            setQuery("");
            if (containerRef.current) containerRef.current.scrollTop = 0;
        } else {
            setPath(prev => [...prev, cat]);
            setQuery("");
            loadCategories(cat.id);
            if (containerRef.current) containerRef.current.scrollTop = 0;
        }
    };



    const handleBack = () => {
        const nextPath = [...path];
        nextPath.pop();
        setPath(nextPath);
        const parentId = nextPath.length === 0 ? null : nextPath[nextPath.length - 1].id;
        setQuery("");
        loadCategories(parentId);
        if (containerRef.current) containerRef.current.scrollTop = 0;
    };


    if (!isOpen) return null;

    return (
        <div
            style={{
                position: "fixed",
                inset: 0,
                background: "rgba(0,0,0,0.5)",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                zIndex: 9999,
            }}
        >
            <div
                style={{
                    background: "#fff",
                    borderRadius: "16px",
                    width: "80%",          // chiếm 80% màn hình
                    maxWidth: "616px",     // không vượt quá 663px
                    height: "90%",         // chiếm 80% chiều cao
                    maxHeight: "646px",    // không vượt quá 646px
                    display: "flex",
                    flexDirection: "column",
                    boxShadow: "0 4px 12px rgba(0,0,0,0.15)",
                }}
            >
                {/* Header */}
                <div
                    style={{
                        position: "relative",
                        flex: "0 0 auto",
                        display: "flex",
                        justifyContent: "center",   // chữ ở giữa ngang modal
                        alignItems: "center",       // giữa dọc
                        padding: "12px 16px",
                        borderBottom: "1px solid #ddd",
                    }}
                >
                    {path.length > 0 && (
                        <button
                            onClick={handleBack}
                            aria-label="Back"
                            style={{
                                position: "absolute",
                                left: 12,
                                top: "50%",
                                transform: "translateY(-50%)",
                                border: "none",
                                background: "none",
                                cursor: "pointer",
                                fontSize: 20,
                                lineHeight: 1,
                                padding: 6,
                            }}
                        >
                            <ChevronLeftIcon style={{ width: 24, height: 24 }} />
                        </button>
                    )}

                    <h2
                        style={{
                            margin: 0
                        }}
                    >
                        {path.length > 0 ? path[path.length - 1].name : "Item category"}
                    </h2>

                    <button
                        onClick={onClose}
                        style={{
                            position: "absolute", // giữ nút luôn sát phải
                            right: "16px",
                            border: "none",
                            background: "none",
                            color: "#0654ba",
                            fontWeight: "500",
                            cursor: "pointer",
                            fontSize: "14px",
                            lineHeight: "1.5",
                        }}
                    >
                        Done
                    </button>
                </div>
                <div style={{
                    flex: "1 1 auto",
                    overflowY: "auto"    // cuộn toàn bộ cả search + body
                }}>
                    {/* Search bar */}
                    {path.length === 0 && (
                        <div
                            style={{
                                padding: "16px 16px",
                                flex: "0 0 auto",
                                display: "flex",
                                justifyContent: "center"
                            }}
                        >
                            <p style={{ width: "95%", margin: 0 }}>
                                <EbayTextbox
                                    fluid
                                    placeholder="Enter a value"
                                    value={query}
                                    onInput={(event) => setQuery(event.target.value)}
                                >
                                    <EbayTextboxPrefixIcon name="search16" />
                                    <EbayTextboxPostfixIcon name="clear16" buttonAriaLabel="Clear" />
                                </EbayTextbox>
                            </p>

                            <div style={{ display: "none" }}>
                                <SearchIcon id="icon-search-16" style={{ width: 16, height: 16 }} />
                            </div>

                            <div style={{ display: "none" }}>
                                <ClearIcon id="icon-clear-16" style={{ width: 16, height: 16 }} />
                            </div>
                        </div>
                    )}


                    {/* Body */}
                    <div
                        style={{
                            flex: 1,
                            padding: "32px",
                        }}
                    >

                        {selected && path.length === 0 && selectedPath.length > 0 && (
                            <>
                                <div style={{ marginBottom: "16px" }}>
                                    <h2 style={{ margin: "0 0 4px" }}>Selected</h2>
                                    <p style={{ fontSize: "14px", margin: 0 }}>
                                        {selectedPath.map((p, idx) => {
                                            const isLast = idx === selectedPath.length - 1;
                                            return (
                                                <span key={p.id}>

                                                    <EbayFakeLink
                                                        onClick={() => !isLast && handleClickBreadcrumb(idx, true)} >
                                                        {p.name}
                                                    </EbayFakeLink>
                                                    {!isLast && <>&nbsp;&gt;&nbsp;</>} {/* chỉ hiển thị dấu > nếu không phải cuối */}
                                                </span>
                                            );
                                        })}
                                    </p>
                                </div>
                                <div style={{ marginBottom: "32px" }}></div>
                            </>
                        )}


                        {path.length === 0 && (
                            <h2 style={{ marginTop: 0, marginBottom: 0 }}>
                                All categories
                            </h2>

                        )}

                        {path.length > 0 && (
                            <div style={{ color: "#777", fontSize: 13, paddingBottom: 24 }}>
                                <span
                                    style={{ cursor: "pointer" }}
                                    onClick={() => handleClickBreadcrumb(null)} // load root categories
                                >
                                    All categories
                                </span>
                                {path.map((p, idx) => {
                                    const isLast = idx === path.length - 1;
                                    return (
                                        <span key={p.id}>
                                            &nbsp;&gt;&nbsp;
                                            <span
                                                style={{
                                                    color: isLast ? "#000" : "#777",
                                                    cursor: isLast ? "default" : "pointer"
                                                }}
                                                onClick={() => !isLast && handleClickBreadcrumb(idx)}
                                            >
                                                {p.name}
                                            </span>
                                        </span>
                                    );
                                })}

                            </div>
                        )}


                        <ul style={{ listStyle: "none", margin: 0, padding: 0 }}>
                            {loading ? (
                                <li style={{ padding: "12px 0", color: "#666" }}>Loading...</li>
                            ) : categories.length === 0 ? (
                                <li style={{ padding: "12px 0", color: "#666" }}>No results</li>
                            ) : (
                                categories.map((cat) => (
                                    <li
                                        key={cat.id}
                                        onClick={() => handleClickCategory(cat)}
                                        style={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            padding: "16px 0",
                                            borderBottom: "1px solid #eee",
                                            cursor: "pointer",
                                        }}
                                        onMouseEnter={(e) =>
                                            (e.currentTarget.style.background = "#fafafa")
                                        }
                                        onMouseLeave={(e) =>
                                            (e.currentTarget.style.background = "transparent")
                                        }
                                        role="button"
                                        tabIndex={0}
                                        onKeyDown={(e) => {
                                            if (e.key === "Enter") handleClickCategory(cat);
                                        }}
                                    >
                                        <span style={{ color: "#222" }}>{cat.name}</span>
                                        {!cat.isLeaf && <ChevronRightIcon style={{ width: 16, height: 16 }} />}
                                    </li>
                                ))
                            )}
                        </ul>
                    </div>
                </div>

                {/* Footer */}
                <div
                    style={{
                        padding: "8px 16px",
                    }}
                >
                </div>

            </div>
        </div>
    );
};

export default CategoryModal;
