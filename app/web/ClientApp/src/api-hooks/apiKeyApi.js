import React from "react";
import { useAccessToken } from "./token";
import { fetchApiKeysAsync } from "../api/apiKeyApi";
import { usePagination } from "../utility/utility";

export const useApiKeys = (props) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchApiKeysAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, props.trigger]);

  return { result };
};
