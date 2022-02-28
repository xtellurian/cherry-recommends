import React from "react";
import { usePagination } from "../utility/utility";
import { useAccessToken } from "./token";
import {
  fetchBusinessesAsync,
} from "../api/businessesApi";
import { useEnvironmentReducer } from "./environmentsApi";

export const useBusinesses = ({ searchTerm }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchBusinessesAsync({
        token,
        page,
        searchTerm,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, searchTerm, environment]);

  return result;
};
