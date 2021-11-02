import React from "react";
import { fetchParametersAsync } from "../api/parametersApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { useEnvironment } from "./environmentsApi";

export const useParameters = (p) => {
  const { trigger } = p || {}; // ensure this works in the case of p === undefinfed
  const token = useAccessToken();
  const page = usePagination();
  const environment = useEnvironment()[0]; // fix warning that setEnvironment is never used
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchParametersAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, trigger, environment]);

  return result;
};
