import React from "react";
import {
  fetchProductRecommenders,
  fetchProductRecommender,
  fetchLinkedRegisteredModel,
  fetchProductRecommendations,
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
