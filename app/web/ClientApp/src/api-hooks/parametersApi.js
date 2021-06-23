import React from "react";
import { fetchParameters } from "../api/parametersApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";

export const useParameters = (p) => {
  const { trigger } = p || {}; // ensure this works in the case of p === undefinfed
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchParameters({
        success: setState,
        error: (error) => setState({ error }),
        token,
        page,
      });
    }
  }, [token, page, trigger]);

  return result;
};
