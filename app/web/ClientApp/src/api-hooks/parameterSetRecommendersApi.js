import React from "react";
import {
  fetchParameterSetRecommenders,
  fetchParameterSetRecommender,
  fetchParameterSetRecommendationsAsync,
  fetchLinkedRegisteredModel,
  fetchTargetVariablesAsync,
} from "../api/parameterSetRecommendersApi";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";

export const useParameterSetRecommenders = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchParameterSetRecommenders({
        success: setState,
        error: (error) => setState({ error }),
        token,
        page,
      });
    }
  }, [token, page]);

  return result;
};

export const useParameterSetRecommender = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchParameterSetRecommender({
        success: setState,
        error: (error) => setState({ error }),
        token,
        id,
      });
    }
  }, [token, id]);

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
