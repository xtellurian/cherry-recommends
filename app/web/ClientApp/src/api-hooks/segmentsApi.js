import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchSegmentsAsync,
  fetchSegmentAsync,
  fetchSegmentCustomersAsync,
  fetchSegmentEnrolmentRulesAsync,
} from "../api/segmentsApi";

export const useSegments = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token) {
      fetchSegmentsAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page]);

  return result;
};

export const useSegment = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token) {
      fetchSegmentAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useSegmentCustomers = (p) => {
  const { id, trigger, searchTerm, weeksAgo } = p || {}; // ensure this works in the case of p === undefinfed
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchSegmentCustomersAsync({
        token,
        page,
        id,
        searchTerm,
        weeksAgo,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page, trigger, searchTerm]);

  return result;
};

export const useSegmentEnrolmentRules = ({ id, trigger }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token) {
      fetchSegmentEnrolmentRulesAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, trigger]);

  return result;
};
