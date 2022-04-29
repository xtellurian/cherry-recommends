import React from "react";
import { useAccessToken } from "./token";
import { useTenantName } from "../components/tenants/PathTenantProvider";
import { useEnvironmentReducer } from "./environmentsApi";
import { fetchKlaviyoListsAsync } from "../api/klaviyoApi";

export const useKlaviyoLists = ({ id, trigger }) => {
  const token = useAccessToken();
  const { tenantName } = useTenantName();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });

  React.useEffect(() => {
    let mounted = true;
    if (token && id) {
      fetchKlaviyoListsAsync({
        token,
        tenant: tenantName,
        id,
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
  }, [token, tenantName, environment, id, trigger]);

  return result;
};
