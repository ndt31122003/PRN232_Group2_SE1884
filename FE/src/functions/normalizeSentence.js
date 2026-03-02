export function normalizeText(text) {
    if (!text || typeof text !== "string") return "";

    return text
        .trim() // Xóa khoảng trắng thừa đầu/cuối
        .replace(/\s+/g, " ") // Chuyển nhiều khoảng trắng thành một
        .replace(/^[,.!?;:]+/, "") // Loại bỏ dấu câu ở đầu câu
        .replace(/\s*([,.!?;:])\s*/g, "$1 ") // Chuẩn hóa khoảng trắng quanh dấu câu
        .replace(/(^|\.\s+)([a-z])/g, (match, p1, p2) => p1 + p2.toUpperCase()); // Viết hoa chữ cái đầu câu
}
