import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchIntegratedSystemsAsync,
  fetchIntegratedSystemAsync,
  fetchWebhookReceivers,
} from "../api/integratedSystemsApi";

export const useIntegratedSystems = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({
    loading: true,
  });

  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchIntegratedSystemsAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page]);

  return result;
};

export const useIntegratedSystem = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });

  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchIntegratedSystemAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const useWebhookReceivers = ({ id }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({
    loading: true,
  });

  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchWebhookReceivers({
        success: setState,
        error: (e) =>
          setState({
            error: e,
          }),
        token,
        page,
        id,
      });
    }
  }, [token, id, page]);

  return result;
};
