import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchIntegratedSystemsAsync,
  fetchIntegratedSystemAsync,
  fetchWebhookReceiversAsync,
} from "../api/integratedSystemsApi";
import { useEnvironmentReducer } from "./environmentsApi";

export const useIntegratedSystems = ({ systemType } = {}) => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({
    loading: true,
  });

  React.useEffect(() => {
    let mounted = true;
    if (token) {
      fetchIntegratedSystemsAsync({
        token,
        page,
        systemType,
      })
        .then((value) => {
          if (mounted) {
            setState(value);
          }
        })
        .catch((error) => {
          if (mounted) {
            setState({ error });
          }
        });
    }
    return () => (mounted = false);
  }, [token, page, systemType, environment]);

  return result;
};

export const useIntegratedSystem = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });

  React.useEffect(() => {
    let mounted = true;
    if (token && id) {
      fetchIntegratedSystemAsync({
        token,
        id,
      })
        .then((value) => {
          if (mounted) {
            setState(value);
          }
        })
        .catch((error) => {
          if (mounted) {
            setState({ error });
          }
        });
    }
    return () => (mounted = false);
  }, [token, id, trigger]);

  return result;
};

export const useWebhookReceivers = ({ id, trigger }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({
    loading: true,
  });

  React.useEffect(() => {
    let mounted = true;
    if (token && id) {
      fetchWebhookReceiversAsync({
        token,
        page,
        id,
      })
        .then((value) => {
          if (mounted) {
            setState(value);
          }
        })
        .catch((error) => {
          if (mounted) {
            setState({ error });
          }
        });
    }
    return () => (mounted = false);
  }, [token, id, page, trigger]);

  return result;
};
