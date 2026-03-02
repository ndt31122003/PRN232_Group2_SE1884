
import { words } from "lodash";

const ROOT_ID = "custom-notice-container";

const ensureContainer = () => {
    if (typeof document === "undefined") {
        return null;
    }

    let container = document.getElementById(ROOT_ID);
    if (container) {
        return container;
    }

    container = document.createElement("div");
    container.id = ROOT_ID;
    container.style.position = "fixed";
    container.style.top = "24px";
    container.style.right = "24px";
    container.style.display = "flex";
    container.style.flexDirection = "column";
    container.style.gap = "12px";
    container.style.zIndex = "9999";
    document.body.appendChild(container);
    return container;
};

const createToast = ({ msg, desc, isSuccess }) => {
    const toast = document.createElement("div");
    toast.style.minWidth = "260px";
    toast.style.maxWidth = "360px";
    toast.style.padding = "14px 16px";
    toast.style.borderRadius = "10px";
    toast.style.boxShadow = "0 10px 24px rgba(17, 17, 17, 0.15)";
    toast.style.backgroundColor = isSuccess ? "#e6f4ea" : "#fdecea";
    toast.style.border = `1px solid ${isSuccess ? "#96d7a5" : "#f5b5b0"}`;
    toast.style.color = "#111";
    toast.style.fontSize = "14px";
    toast.style.lineHeight = "1.4";
    toast.style.opacity = "0";
    toast.style.transform = "translateY(-8px)";
    toast.style.transition = "opacity 0.2s ease, transform 0.2s ease";

    const title = document.createElement("div");
    title.textContent = msg || (isSuccess ? "Success" : "Something went wrong");
    title.style.fontWeight = "600";
    toast.appendChild(title);

    if (desc) {
        const body = document.createElement("div");
        body.textContent = desc;
        body.style.marginTop = "6px";
        body.style.fontSize = "13px";
        body.style.color = "#333";
        toast.appendChild(body);
    }

    requestAnimationFrame(() => {
        toast.style.opacity = "1";
        toast.style.transform = "translateY(0)";
    });

    return toast;
};

const scheduleRemoval = (container, toast, timeout) => {
    window.setTimeout(() => {
        toast.style.opacity = "0";
        toast.style.transform = "translateY(-8px)";
        window.setTimeout(() => {
            if (container.contains(toast)) {
                container.removeChild(toast);
            }
            if (!container.childElementCount) {
                container.remove();
            }
        }, 220);
    }, timeout);
};

export default function Notice({
    msg,
    desc,
    isSuccess = true,
    duration
} = {}) {
    if (typeof document === "undefined") {
        return;
    }

    const container = ensureContainer();
    if (!container) {
        return;
    }

    const estimated = words(msg || "").length;
    const timeout = duration
        ? duration
        : estimated > 20
            ? 4500
            : 3000;

    const toast = createToast({ msg, desc, isSuccess });
    container.appendChild(toast);
    scheduleRemoval(container, toast, timeout);
}
