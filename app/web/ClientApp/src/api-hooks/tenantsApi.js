import React from "react";
import { useAccessToken } from "./token";
import {
  fetchCurrentTenantAsync,
  fetchHostingAsync,
  fetchMembershipsAsync,
} from "../api/tenantsApi";

const addIsCanonicalRoot = (s) => {
  s.isCanonicalRoot = window.location.host === s.canonicalRootDomain;
  return s;
};
const addIsWww = (s) => {
  s.isWwwPage = window.location.host.startsWith("www");
  return s;
};
export const useHosting = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    if (token) {
      setState({ loading: true });
      fetchHostingAsync({
        token,
      })
        .then(addIsCanonicalRoot)
        .then(addIsWww)
        .then((s) => {
          setState(s);
        })
        .catch((error) => setState({ error }));
    }
  }, [token]);
  return result;
};

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
