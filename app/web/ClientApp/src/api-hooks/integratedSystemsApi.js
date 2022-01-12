import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchIntegratedSystemsAsync,
  fetchIntegratedSystemAsync,
  fetchWebhookReceiversAsync,
} from "../api/integratedSystemsApi";
import { useEnvironmentReducer } from "./environmentsApi";

export const useIntegratedSystems = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
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
  }, [token, page, environment]);

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
      fetchWebhookReceiversAsync({
        token,
        page,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page]);

  return result;
};
