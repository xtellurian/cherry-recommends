import { useLocation } from "react-router-dom";

export function useQuery() {
  return new URLSearchParams(useLocation().search);
}

export function useTabs(defaultTab) {
  return useQuery().get("tab") || defaultTab;
}

export function usePagination() {
  const p = parseInt(useQuery().get("page")) || 1;
  return p;
}
