import React from "react";
import { useAccessToken } from "./token";
import {
  fetchEventSummaryAsync,
  fetchEventKindNamesAsync,
  fetchEventKindSummaryAsync,
  fetchEventTimelineAsync,
  fetchDashboardAsync,
  fetchLatestActionsAsync,
} from "../api/dataSummaryApi";
import { useEnvironmentReducer } from "./environmentsApi";

export const useEventKindNames = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token) {
      fetchEventKindNamesAsync({
        token,
      })
        .then((kinds) => setState({ kinds }))
        .catch((error) => setState({ error }));
    }
  }, [token]);

  return result;
};

export const useEventKindSummary = ({ kind }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token && kind) {
      fetchEventKindSummaryAsync({
        token,
        kind,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    } else if (token && !kind) {
      setState({});
    }
  }, [token, kind]);

  return result;
};

export const useEventDataSummary = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token) {
      fetchEventSummaryAsync({
        token,
      })
        .then(setState)
        .catch((error) => setState({ error }));
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
      fetchEventTimelineAsync({
        token,
        kind,
        eventType,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, kind, eventType]);

  return { result };
};

export const useDashboard = ({ scope }) => {
  const token = useAccessToken();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchDashboardAsync({
        token,
        scope,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, scope, environment]);

  return result;
};
