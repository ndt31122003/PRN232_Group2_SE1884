import dayjs from "dayjs";
export function checkDateStatus(dateStr, t) {
  try {
    const inputDate = dayjs(dateStr, "YYYY-MM-DD", true);
    if (!inputDate.isValid()) {
      throw new Error("Invalid date format. Please use YYYY-MM-DD.");
    }

    const today = dayjs();
    const yesterday = today.subtract(1, "day");

    if (inputDate.isSame(today, "day")) {
      return t("today"); // Dịch "Today"
    } else if (inputDate.isSame(yesterday, "day")) {
      return t("yesterday"); // Dịch "Yesterday"
    } else {
      return "";
    }
  } catch (error) {
    throw new Error(error.message);
  }
}

const tailwindColors = [
  "bg-red-100",
  "bg-blue-100",
  "bg-green-100",
  "bg-yellow-100",
  "bg-purple-100",
  "bg-pink-100",
  "bg-indigo-100",
  "bg-teal-100",
  "bg-orange-100",
  "bg-gray-100",
];

const speakerColorMap = {};

export const getTailwindBgColorForSpeaker = (speakerName) => {
  if (!speakerName) return "bg-gray-300"; // Default for undefined names

  // Check if the speaker already has a color assigned
  if (speakerName in speakerColorMap) {
    return speakerColorMap[speakerName];
  }

  // Hash the speaker name to determine a unique index
  const hash = [...speakerName].reduce(
    (acc, char) => acc + char.charCodeAt(0),
    0
  );
  const colorIndex = hash % tailwindColors.length;

  // Ensure uniqueness by checking if the color is already assigned
  let assignedColor = tailwindColors[colorIndex];
  while (Object.values(speakerColorMap).includes(assignedColor)) {
    // Increment index and wrap around if necessary
    colorIndex = (colorIndex + 1) % tailwindColors.length;
    assignedColor = tailwindColors[colorIndex];
  }

  // Assign the color to the speaker
  speakerColorMap[speakerName] = assignedColor;

  return assignedColor;
};
