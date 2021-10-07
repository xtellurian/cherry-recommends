import React from "react";
import { fetchItemsAsync, fetchItemAsync } from "../api/recommendableItemsApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { useEnvironment } from "./environmentsApi";

export const useItems = (p) => {
  const { trigger } = p || {}; // ensure this works in the case of p === undefinfed
  const token = useAccessToken();
  const page = usePagination();
  const environment = useEnvironment()[0]; // fix setEnvironmnet is not used warning
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchItemsAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, trigger, environment]);

  return result;
};

export const useItem = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchItemAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};
