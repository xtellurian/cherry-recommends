import { useLocation } from "react-router-dom";

export function useQuery() {
  return new URLSearchParams(useLocation().search);
}

export function usePagination() {
  const p = parseInt(useQuery().get("page")) || 1;
  return p;
}
