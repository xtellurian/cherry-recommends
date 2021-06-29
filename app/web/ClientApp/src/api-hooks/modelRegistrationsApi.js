import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import {
  fetchModelRegistration,
  fetchModelRegistrations,
} from "../api/modelRegistrationsApi";

export const useModelRegistrations = (p) => {
  const { trigger } = p || {}; // ensure this works in the case of p === undefinfed
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchModelRegistrations({
        success: setState,
        error: (error) => setState({ error }),
        token,
        page,
      });
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
      fetchModelRegistration({
        success: setState,
        error: console.log,
        token,
        id,
      });
    }
  }, [token, id]);

  return model;
};
