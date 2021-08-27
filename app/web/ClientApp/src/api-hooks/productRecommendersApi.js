import React from "react";
import {
  fetchProductRecommendersAsync,
  fetchProductRecommender,
  fetchLinkedRegisteredModel,
  fetchInvokationLogsAsync,
  fetchProductRecommendations,
  fetchTargetVariablesAsync,
  fetchRecommenderTrackedUserActionsAsync,
  getDefaultProductAsync,
} from "../api/productRecommendersApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";

export const useProductRecommenders = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchProductRecommendersAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page]);

  return result;
};

export const useDefaultProduct = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      getDefaultProductAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};

export const useProductRecommender = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchProductRecommender({
        success: setState,
        error: (error) => setState({ error }),
        token,
        id,
      });
    }
  }, [token, id]);

  return result;
};

export const useProductRecommendations = ({ id }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchProductRecommendations({
        success: setState,
        error: (error) => setState({ error }),
        token,
        id,
        page,
      });
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
      fetchLinkedRegisteredModel({
        success: setState,
        error: (error) => setState({ error }),
        token,
        id,
      });
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

export const useRecommenderTrackedUserActions = ({
  id,
  page,
  revenueOnly,
}) => {
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
