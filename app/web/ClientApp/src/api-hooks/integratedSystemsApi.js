import React from "react";
import { useAccessToken } from "./token";
import { fetchIntegratedSystems } from "../api/integratedSystemsApi";

export const useIntegratedSystems = () => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    integratedSystems: null,
    error: null,
    loading: true,
  });

  React.useEffect(() => {
    if (token) {
      fetchIntegratedSystems({
        success: (_) =>
          setState({
            integratedSystems: _,
            loading: false,
          }),
        error: (_) =>
          setState({
            error: _,
            loading: false,
          }),
        token,
      });
    }
  }, [token]);

  return state;
};
