import React from "react";
import {
  fetchItemsRecommendersAsync,
  fetchItemsRecommenderAsync,
  fetchLinkedRegisteredModelAsync,
  fetchInvokationLogsAsync,
  fetchItemsRecommendationsAsync,
  fetchTargetVariablesAsync,
  fetchItemsAsync,
  fetchRecommenderTrackedUserActionsAsync,
  fetchDestinationsAsync,
  getDefaultItemAsync,
  fetchTriggerAsync,
  fetchLearningFeaturesAsync,
} from "../api/itemsRecommendersApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { useEnvironment } from "./environmentsApi";

export const useItemsRecommenders = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment, setEnvironment] = useEnvironment();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchItemsRecommendersAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, environment]);
  return result;
};

export const useDefaultItem = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      getDefaultItemAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const useItemsRecommender = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchItemsRecommenderAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const useItemsRecommendations = ({ id }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchItemsRecommendationsAsync({
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

export const useInvokationLogs = ({ id }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
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

export const useItems = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchItemsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

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
    if (token) {
      fetchRecommenderTrackedUserActionsAsync({
        token,
        id,
        page,
        revenueOnly,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page]);

  return state;
};

export const useDestinations = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
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

// the trigger param is a different trigger
export const useTrigger = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
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

export const useLearningFeatures = ({ id, trigger }) => {
  const token = useAccessToken();
  const [state, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchLearningFeaturesAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return state;
};
