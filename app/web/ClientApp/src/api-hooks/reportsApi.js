import React from "react";
import { useAccessToken } from "./token";
import { fetchReportsAsync } from "../api/reportsApi";

export const useReports = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchReportsAsync({
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token]);

  return { result };
};
