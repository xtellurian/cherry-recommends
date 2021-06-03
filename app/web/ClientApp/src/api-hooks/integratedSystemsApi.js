import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { fetchIntegratedSystems } from "../api/integratedSystemsApi";

export const useIntegratedSystems = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({
    error: null,
    loading: true,
  });

  React.useEffect(() => {
    if (token) {
      fetchIntegratedSystems({
        success: setState,
        error: (_) =>
          setState({
            error: _,
            loading: false,
          }),
        token,
        page,
      });
    }
  }, [token, page]);

  return { result };
};
