import React, { useRef, useState, useEffect } from "react";
import { HiChevronRight, HiChevronLeft, HiMagnifyingGlass, HiXMark } from "react-icons/hi2";
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
                            <HiChevronLeft style={{ width: 24, height: 24 }} />
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
                            <div style={{ width: "95%", margin: 0, position: "relative", display: "flex", alignItems: "center" }}>
                                <HiMagnifyingGlass style={{ position: "absolute", left: 12, width: 16, height: 16, color: "#767676" }} />
                                <input
                                    type="text"
                                    placeholder="Enter a value"
                                    value={query}
                                    onChange={(event) => setQuery(event.target.value)}
                                    style={{
                                        width: "100%",
                                        padding: "10px 36px",
                                        border: "1px solid #c7c7c7",
                                        borderRadius: "8px",
                                        fontSize: "14px",
                                        outline: "none",
                                    }}
                                />
                                {query && (
                                    <button
                                        onClick={() => setQuery("")}
                                        style={{ position: "absolute", right: 12, border: "none", background: "none", cursor: "pointer", padding: 0 }}
                                        aria-label="Clear"
                                    >
                                        <HiXMark style={{ width: 16, height: 16, color: "#767676" }} />
                                    </button>
                                )}
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

                                                    <button
                                                        type="button"
                                                        onClick={() => !isLast && handleClickBreadcrumb(idx, true)}
                                                        style={{ background: "none", border: "none", color: isLast ? "#111" : "#0654ba", cursor: isLast ? "default" : "pointer", padding: 0, fontSize: "inherit", textDecoration: isLast ? "none" : "underline" }}
                                                    >
                                                        {p.name}
                                                    </button>
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
                                        {!cat.isLeaf && <HiChevronRight style={{ width: 16, height: 16 }} />}
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
