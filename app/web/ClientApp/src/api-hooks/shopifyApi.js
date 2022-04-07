import React from "react";
import { useAccessToken } from "./token";
import {
  fetchShopifyAppInformationAsync,
  fetchShopInformationAsync,
} from "../api/shopifyApi";
import { useTenantName } from "../components/tenants/PathTenantProvider";

export const useShopifyAppInformation = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });

  React.useEffect(() => {
    let mounted = true;
    if (token) {
      setState({ loading: true });
      fetchShopifyAppInformationAsync({
        token,
      })
        .then((value) => {
          if (mounted) {
            setState(value);
          }
        })
        .catch((error) => {
          if (mounted) {
            setState({ error });
          }
        });
    }
    return () => (mounted = false);
  }, [token]);

  return result;
};

export const useShopInformation = ({ id, trigger }) => {
  const token = useAccessToken();
  const tenant = useTenantName();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    let mounted = true;
    if (token && tenant && id) {
      fetchShopInformationAsync({
        id,
        token,
        tenant: tenant?.tenantName,
      })
        .then((value) => {
          if (mounted) {
            setState(value);
          }
        })
        .catch((error) => {
          if (mounted) {
            setState({ error });
          }
        });
    }
    return () => (mounted = false);
  }, [token, tenant, id, trigger]);

  return result;
};
