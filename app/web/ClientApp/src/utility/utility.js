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
