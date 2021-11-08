import React from "react";
import {
  fetchParameterSetRecommendersAsync,
  fetchParameterSetRecommenderAsync,
  fetchParameterSetRecommendationsAsync,
  fetchLinkedRegisteredModelAsync,
  fetchInvokationLogsAsync,
  fetchRecommenderTrackedUserActionsAsync,
  fetchTargetVariablesAsync,
  fetchDestinationsAsync,
  fetchTriggerAsync,
} from "../api/parameterSetRecommendersApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { useEnvironment } from "./environmentsApi";

export const useParameterSetRecommenders = () => {
  const token = useAccessToken();
  const page = usePagination();
  const environment = useEnvironment()[0]; // fix setEnvironmnet is not used warning
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchParameterSetRecommendersAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, environment]);

  return result;
};

export const useParameterSetRecommender = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (id && token) {
      fetchParameterSetRecommenderAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const useParameterSetRecommendations = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchParameterSetRecommendationsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useInvokationLogs = ({ id }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchInvokationLogsAsync({
        token,
        id,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page]);

  return result;
};

export const useLinkedRegisteredModel = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchLinkedRegisteredModelAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useTargetVariables = ({ id, name }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchTargetVariablesAsync({
        token,
        id,
        name,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, name]);

  return state;
};

export const useRecommenderTrackedUserActions = ({ id, page, revenueOnly }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id && page) {
      fetchRecommenderTrackedUserActionsAsync({
        token,
        id,
        revenueOnly,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page, revenueOnly]);

  return state;
};

export const useDestinations = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchDestinationsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};

export const useTrigger = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTriggerAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};
