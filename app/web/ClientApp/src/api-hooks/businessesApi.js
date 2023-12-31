import React from "react";
import { usePagination } from "../utility/utility";
import { useAccessToken } from "./token";
import {
  fetchBusinessesAsync,
  fetchBusinessAsync,
  fetchBusinessMembersAsync,
  fetchRecommendationsAsync,
} from "../api/businessesApi";
import { useEnvironmentReducer } from "./environmentsApi";

export const useBusinesses = ({ searchTerm } = {}) => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchBusinessesAsync({
        token,
        page,
        searchTerm,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, searchTerm, environment]);

  return result;
};

export const useBusiness = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchBusinessAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useBusinessMembers = ({ id, searchTerm, trigger }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchBusinessMembersAsync({
        token,
        id,
        page,
        searchTerm,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page, searchTerm, environment, trigger]);

  return result;
};

export const useRecommendations = ({ id }) => {
  const token = useAccessToken();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchRecommendationsAsync({
        token,
        id,
        page: 1,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, environment]);

  return result;
};
