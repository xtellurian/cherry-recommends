import { useRef } from "react";
import { useLocation } from "react-router-dom";

export function useQuery() {
  return new URLSearchParams(useLocation().search);
}

export const useCurrentTab = () => {
  return useQuery().get("tab");
};

export function useTabs(defaultTab) {
  return useCurrentTab() || defaultTab;
}

export function usePagination() {
  const p = parseInt(useQuery().get("page")) || 1;
  return p;
}

export function toDate(date) {
  let dateValue = new Date();
  if (typeof date === "string") {
    dateValue = new Date(Date.parse(date));
  } else if (typeof date === "number") {
    dateValue = new Date(date);
  } else {
    return null;
  }

  return dateValue;
}

export function toShortDate(value) {
  var date = toDate(value);
  return date?.toLocaleDateString();
}

export function getFirstLastOfTheWeek(date) {
  // First day is the day of the month - the day of the week. +1 for Monday
  var first = date.getDate() - date.getDay() + 1;
  var last = first + 6; // last day is the first day + 6
  var firstday = new Date(date.setDate(first));
  var lastday = new Date(date.setDate(last));

  return {
    firstday,
    lastday,
  };
}

export function generateRandomHexColor() {
  const letters = "0123456789ABCDEF";
  let color = "#";
  for (var i = 0; i < 6; i++) {
    color += letters[Math.floor(Math.random() * 16)];
  }
  return color;
}

export function useCommonId() {
  const randomStr = useRef(
    Math.floor(Math.random() * 0x10000000000)
      .toString(16)
      .substring(0, 4)
  );

  const generateCommonId = (value) => {
    const maxNumOfWords = 3; // in value
    const maxNumOfChars = 10; // per word

    if (!value) {
      return "";
    }

    const formattedValue = value
      .split(/\s+/g) // split by whitespaces
      .filter((word) => word) // remove empty empty string
      .slice(0, maxNumOfWords) // get the first 3 words
      .map((word) =>
        word
          .replace(/[^a-zA-Z0-9 ]/g, "") // remove all special characters
          .substring(0, maxNumOfChars) // get only 10 characters from each word
          .toLowerCase()
      )
      .join("-");

    return formattedValue ? formattedValue + "-" + randomStr.current : "";
  };

  return {
    generateCommonId,
  };
}

export function saveBlob({ blob, name }) {
  var a = document.createElement("a");
  document.body.appendChild(a);
  a.style = "display: none";

  window.URL.createObjectURL(blob);
  a.download = name;
  const url = window.URL.createObjectURL(blob);
  a.href = url;
  a.click();
  window.URL.revokeObjectURL(url);
};
