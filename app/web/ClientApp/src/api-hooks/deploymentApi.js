import React from "react";
import { useAccessToken } from "./token";
import { fetchDeploymentConfiguration } from "../api/deploymentApi";

export const useDeploymentConfiguration = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ error: null, loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchDeploymentConfiguration({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
      });
    }
  }, [token]);

  return result;
};
