import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchIntegratedSystems,
  fetchIntegratedSystem,
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
      fetchIntegratedSystems({
        success: setState,
        error: (e) =>
          setState({
            error: e,
          }),
        token,
        page,
      });
    }
  }, [token, page]);

  return result;
};

export const useIntegratedSystem = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });

  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchIntegratedSystem({
        success: setState,
        error: (e) =>
          setState({
            error: e,
          }),
        token,
        id,
      });
    }
  }, [token, id]);

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
    if (token) {
      fetchWebhookReceivers({
        success: setState,
        error: (e) =>
          setState({
            error: e,
          }),
        token,
        page,
      });
    }
  }, [token, page]);

  return result;
};
