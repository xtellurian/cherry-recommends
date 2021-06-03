import React from "react";
import { useAccessToken } from "./token";
import { fetchApiKeys } from "../api/apiKeyApi";
import { usePagination } from "../utility/utility";

export const useApiKeys = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchApiKeys({
        success: setState,
        error: console.log,
        token,
        page,
      });
    }
  }, [token, page]);

  return { result };
};
