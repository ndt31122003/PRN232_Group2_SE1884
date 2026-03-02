import { useState, useEffect } from "react";
import "./LoadingScreen.scss";
import CD from "../../assets/images/CD.png";

export const LoadingScreen = ({ isOverlay = false, isLoadingTable = false, sizeSmall = false }) => {
    const [show, setShow] = useState(false);

    useEffect(() => {
        // Giả lập lazy load: Delay 300ms trước khi hiển thị loading
        const timer = setTimeout(() => setShow(true), 300);
        return () => clearTimeout(timer);
    }, []);

    if (!show) return null; // Không hiển thị nếu chưa sẵn sàng

    return (
        <div className={`loading-screen ${isOverlay ? "overlay" : ""} ${isLoadingTable ? "loadingTable" : ""}`}>
            <div className={`spinner-container ${sizeSmall ? "sizeSmall" : ""}`}>

            </div>
        </div>
    );
};

