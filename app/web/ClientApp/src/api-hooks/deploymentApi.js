import React from "react";
import { useAccessToken } from "./token";
import { fetchDeploymentConfigurationAsync } from "../api/deploymentApi";

export const useDeploymentConfiguration = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ error: null, loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchDeploymentConfigurationAsync({
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token]);

  return result;
};
