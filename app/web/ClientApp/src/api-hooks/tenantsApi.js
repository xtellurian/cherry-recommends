import React from "react";
import { useAccessToken } from "./token";
import {
  fetchCurrentTenantAsync,
  fetchMembershipsAsync,
  fetchCurrentTenantMembershipsAsync,
} from "../api/tenantsApi";

export const useCurrentTenant = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token) {
      setState({ loading: true });
      fetchCurrentTenantAsync({
        token,
      })
        .then((s) => {
          setState(s);
        })
        .catch((error) => setState({ error }));
    }
  }, [token]);
  return result;
};

export const useCurrentTenantMemberships = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token) {
      setState({ loading: true });
      fetchCurrentTenantMembershipsAsync({
        token,
      })
        .then((s) => {
          setState(s);
        })
        .catch((error) => setState({ error }));
    }
  }, [token]);
  return result;
};

export const useMemberships = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token) {
      setState({ loading: true });
      fetchMembershipsAsync({
        token,
      })
        .then((s) => {
          setState(s);
        })
        .catch((error) => setState({ error }));
    }
  }, [token]);
  return result;
};
