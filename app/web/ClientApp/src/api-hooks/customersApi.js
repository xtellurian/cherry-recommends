import React from "react";
import { usePagination } from "../utility/utility";
import { useAccessToken } from "./token";
import {
  fetchLatestRecommendationsAsync,
  fetchCustomersAsync,
  fetchCustomerAsync,
} from "../api/customersApi";
import { useEnvironment } from "./environmentsApi";

export const useCustomers = ({ searchTerm }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment, setEnvironment] = useEnvironment();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchCustomersAsync({
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

export const useCustomer = ({ id, useInternalId }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchCustomerAsync({
        token,
        id,
        useInternalId: useInternalId === undefined ? true : useInternalId, // default to true
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useLatestRecommendations = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchLatestRecommendationsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};
