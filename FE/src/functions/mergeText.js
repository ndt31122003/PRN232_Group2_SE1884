export function mergeCommonPart(text1, text2) {
    // Lưu vị trí dấu câu trong text1 trước khi xóa
    let punctuationMap = [];
    for (let i = 0; i < text1.length; i++) {
        if (/[.,!?]/.test(text1[i])) {
            punctuationMap.push({ index: i, char: text1[i] });
        }
    }

    // Loại bỏ dấu câu trong cả hai đoạn
    let cleanText1 = text1.replace(/[.,!?]/g, "");
    let cleanText2 = text2.replace(/[.,!?]/g, "");

    // Tách thành mảng từ
    let words1 = cleanText1.split(" ").filter(word => word.length > 0);
    let words2 = cleanText2.split(" ").filter(word => word.length > 0);

    // Tạo các biến phụ để kiểm tra nhiều trường hợp
    let foundOverlap = false;
    let overlapText1 = cleanText1;

    // Thử bỏ từ cuối cùng
    if (words1.length > 0) {
        let textWithoutLastWord = words1.slice(0, -1).join(" ");
        if (words2.join(" ").toLowerCase().startsWith(textWithoutLastWord.toLowerCase())) {
            overlapText1 = textWithoutLastWord;
            foundOverlap = true;
        }
    }

    // Nếu không tìm thấy, thử bỏ hai từ cuối
    if (!foundOverlap && words1.length > 1) {
        let textWithoutLastTwoWords = words1.slice(0, -2).join(" ");
        if (words2.join(" ").toLowerCase().startsWith(textWithoutLastTwoWords.toLowerCase())) {
            overlapText1 = textWithoutLastTwoWords;
            foundOverlap = true;
        }
    }

    // Nếu không tìm thấy sự phù hợp nào, tìm phần chồng lấp lớn nhất
    if (!foundOverlap) {
        let maxOverlap = 0;
        let bestOverlapLength = 0;

        // Tìm số từ chồng lấp tối đa ở cuối text1 và đầu text2
        for (let i = 1; i <= Math.min(words1.length, words2.length); i++) {
            const endOfText1 = words1.slice(words1.length - i).join(" ").toLowerCase();
            const startOfText2 = words2.slice(0, i).join(" ").toLowerCase();

            if (endOfText1 === startOfText2 && i > bestOverlapLength) {
                maxOverlap = i;
                bestOverlapLength = i;
            }
        }

        // Nếu tìm thấy sự chồng lấp
        if (maxOverlap > 0) {
            overlapText1 = words1.slice(0, words1.length - maxOverlap).join(" ");
            cleanText2 = words2.join(" ");
            foundOverlap = true;
        }
    }

    let mergedText;

    if (foundOverlap) {
        // Tìm vị trí chồng lấp giữa hai đoạn văn
        let minLength = Math.min(overlapText1.length, cleanText2.length);
        let commonIndex = 0;

        for (let i = 1; i <= minLength; i++) {
            if (overlapText1.toLowerCase().endsWith(cleanText2.substring(0, i).toLowerCase())) {
                commonIndex = i;
            }
        }

        // Gộp hai đoạn
        mergedText = overlapText1 + cleanText2.substring(commonIndex);
    } else {
        // Nếu không tìm thấy sự chồng lấp, đơn giản là nối hai đoạn với một khoảng trắng
        mergedText = cleanText1 + " " + cleanText2;
    }

    // Thêm lại dấu câu theo vị trí cũ
    let finalText = mergedText.split("");
    for (let { index, char } of punctuationMap) {
        if (index < finalText.length) {
            finalText.splice(index, 0, char);
        }
    }

    return finalText.join("").trim();
}