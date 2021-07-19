import React from "react";
import {
  fetchProductRecommenders,
  fetchProductRecommender,
  fetchLinkedRegisteredModel,
  fetchInvokationLogsAsync,
  fetchProductRecommendations,
  fetchTargetVariablesAsync,
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
      fetchProductRecommenders({
        success: setState,
        error: (error) => setState({ error }),
        token,
        page,
      });
    }
  }, [token, page]);

  return result;
};

export const useProductRecommender = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
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
