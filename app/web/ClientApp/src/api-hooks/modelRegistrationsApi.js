import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchModelRegistrationAsync,
  fetchModelRegistrationsAsync,
} from "../api/modelRegistrationsApi";

export const useModelRegistrations = (p) => {
  const { trigger } = p || {}; // ensure this works in the case of p === undefinfed
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchModelRegistrationsAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, trigger]);
  return result;
};

export const useModelRegistration = ({ id }) => {
  const token = useAccessToken();
  const [model, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchModelRegistrationAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return model;
};
