import React from "react";
import { useAccessToken } from "./token";
import {
  fetchEventSummary,
  fetchEventTimeline,
  fetchDashboard,
  fetchLatestActionsAsync,
} from "../api/dataSummaryApi";

export const useEventDataSummary = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token) {
      fetchEventSummary({
        success: setState,
        error: console.log,
        token,
      });
    }
  }, [token]);

  return { result };
};

export const useLatestActions = () => {
  const token = useAccessToken();
  const [state, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token) {
      setState({ loading: true });
      fetchLatestActionsAsync({
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token]);

  return state;
};

export const useEventTimeline = ({ kind, eventType }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token && kind && eventType) {
      setState({ loading: true });
      fetchEventTimeline({
        success: setState,
        error: console.log,
        token,
        kind,
        eventType,
      });
    }
  }, [token, kind, eventType]);

  return { result };
};

export const useDashboard = ({ scope }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchDashboard({
        success: setState,
        error: (e) =>
          setState({
            error: e,
          }),
        token,
        scope,
      });
    }
  }, [token, scope]);

  return result;
};
