import React from "react";
import { useAccessToken } from "./token";
import { fetchReports } from "../api/reportsApi";

export const useReports = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchReports({
        success: setState,
        error: console.log,
        token,
      });
    }
  }, [token]);

  return { result };
};
