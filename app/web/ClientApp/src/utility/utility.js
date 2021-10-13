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
