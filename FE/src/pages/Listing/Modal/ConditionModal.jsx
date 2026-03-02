import React, { useEffect } from "react";

import { ReactComponent as CheckedIcon } from "@ebay/skin/dist/svg/icon/icon-radio-checked-18.svg";
import { ReactComponent as UncheckedIcon } from "@ebay/skin/dist/svg/icon/icon-radio-unchecked-18.svg";

import { EbayRadio } from '@ebay/ui-core-react/ebay-radio';
import { EbayLabel } from '@ebay/ui-core-react/ebay-field';

const ConditionModal = ({ isOpen, onClose, conditions = [], selectedConditionId, setSelectedConditionId }) => {


    // lock body scroll while modal open
    useEffect(() => {
        if (isOpen) {
            document.body.style.overflow = "hidden";

        } else {
            document.body.style.overflow = "auto";
        }
        return () => {
            document.body.style.overflow = "auto";
        };
    }, [isOpen]);

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
                    <h2 style={{ margin: 0 }}>
                        Item condition
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

                <div style={{ padding: "28px", overflowY: "auto", flex: "1 1 auto" }}>
                    <div style={{ display: "none" }}>
                        <CheckedIcon id="icon-radio-checked-18" style={{ width: 18, height: 18 }} />
                    </div>
                    <div style={{ display: "none" }}>
                        <UncheckedIcon id="icon-radio-unchecked-18" style={{ width: 18, height: 18 }} />
                    </div>
                    {conditions.length === 0 && (
                        <p style={{ color: "#707070" }}>This category does not define condition options.</p>
                    )}
                    {conditions.map((condition) => (
                        <div key={condition.id} style={{ marginBottom: "24px" }}>
                            <EbayRadio
                                name="condition"
                                value={condition.id}
                                checked={selectedConditionId === condition.id}
                                onChange={(e) => {
                                    setSelectedConditionId(e.target.value);
                                }}
                            >
                                <EbayLabel style={{ marginLeft: 8 }}>{condition.name}</EbayLabel>
                            </EbayRadio>
                            <p
                                style={{
                                    color: "#707070",
                                    marginLeft: "28px",
                                    marginTop: 0,
                                    marginBottom: 0,
                                }}
                            >
                                {condition.description}
                            </p>
                        </div>
                    ))}

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

export default ConditionModal;
