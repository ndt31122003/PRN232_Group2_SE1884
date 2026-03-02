export function trimPrefixIfExists(text, prefix) {
    if (text.toLowerCase().startsWith(prefix.toLowerCase())) {
        return text.toLowerCase().slice(prefix.length).trim(); // Cắt bỏ prefix và loại bỏ khoảng trắng thừa
    }
    return text;
}