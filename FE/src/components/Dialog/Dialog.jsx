import React, { useEffect } from "react";
import PropTypes from "prop-types";
import "./Dialog.scss";

const Dialog = ({
    isOpen,
    title,
    subtitle,
    onClose,
    children,
    footer,
}) => {
    useEffect(() => {
        if (!isOpen) {
            return undefined;
        }

        const handleKeyDown = (event) => {
            if (event.key === "Escape") {
                onClose?.();
            }
        };

        document.addEventListener("keydown", handleKeyDown);
        return () => {
            document.removeEventListener("keydown", handleKeyDown);
        };
    }, [isOpen, onClose]);

    if (!isOpen) {
        return null;
    }

    const handleBackdropMouseDown = (event) => {
        if (event.target === event.currentTarget) {
            onClose?.();
        }
    };

    return (
        <div
            className="dialog-backdrop"
            role="presentation"
            onMouseDown={handleBackdropMouseDown}
        >
            <div
                className="dialog"
                role="dialog"
                aria-modal="true"
                aria-labelledby="dialog-title"
                onMouseDown={(event) => event.stopPropagation()}
            >
                <header className="dialog__header">
                    <div>
                        {title && (
                            <h2 className="dialog__title" id="dialog-title">
                                {title}
                            </h2>
                        )}
                        {subtitle && <p className="dialog__subtitle">{subtitle}</p>}
                    </div>
                    <button
                        type="button"
                        className="dialog__close"
                        aria-label="Close dialog"
                        onClick={onClose}
                    >
                        <span aria-hidden="true">×</span>
                    </button>
                </header>
                <div className="dialog__body">{children}</div>
                {footer && <footer className="dialog__footer">{footer}</footer>}
            </div>
        </div>
    );
};

Dialog.propTypes = {
    isOpen: PropTypes.bool.isRequired,
    title: PropTypes.string,
    subtitle: PropTypes.string,
    onClose: PropTypes.func,
    children: PropTypes.node,
    footer: PropTypes.node,
};

Dialog.defaultProps = {
    title: "",
    subtitle: "",
    onClose: undefined,
    children: null,
    footer: null,
};

export default Dialog;
