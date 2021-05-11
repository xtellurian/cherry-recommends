import React from "react";
import { useAccessToken } from "./token";
import { fetchApiKeys } from "../api/apiKeyApi";

export const useApiKeys = () => {
  const token = useAccessToken();
  const [apiKeys, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchApiKeys({
        success: setState,
        error: console.log,
        token,
      });
    }
  }, [token]);

  return { apiKeys };
};