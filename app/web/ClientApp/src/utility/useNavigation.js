import { useCallback } from "react";
import { useNavigate, useLocation } from "react-router-dom";

import { useHosting } from "../components/tenants/HostingProvider";
import { useTenantName } from "../components/tenants/PathTenantProvider";

// we only need to prefix if the paths are absolute paths.
const prefixIfRequired = (path, prefix) => {
  let result = path;
  if (!prefix) {
    return path; // return fast if there's no prefix
  }
  if (path.startsWith("/")) {
    if (path.startsWith(`/${prefix}`)) {
      result = path;
    } else {
      result = `/${prefix}${path}`;
    }
  }
  return result;
};

export const useNavigation = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const hosting = useHosting();
  const { tenantName } = useTenantName();

  const appendCurrentURL = useCallback(
    (to, withQueryParams) => {
      if (typeof to === "string") {
        to = {
          ...location,
          pathname: to,
        };
      }

      const search = new URLSearchParams(location.search);
      const newSearch = new URLSearchParams();
      if (withQueryParams && Array.isArray(withQueryParams)) {
        for (const p of withQueryParams) {
          const val = search.get(p);
          if (val) {
            newSearch.set(p, val);
          }
        }
      }

      if (typeof to === "object" && to !== null) {
        return {
          ...location,
          search: newSearch.toString(),
          ...to,
        };
      }

      return "";
    },
    [location]
  );

  const ensureAbsolutePathsHaveTenantNamePrefixed = useCallback(
    (to) => {
      if (!hosting?.multitenant) {
        return to; // don't do it unless in multitenant mode
      }
      if (typeof to === "string") {
        return prefixIfRequired(to, tenantName);
      }
      if (typeof to === "object" && to !== null && to.pathname) {
        return {
          ...to,
          pathname: prefixIfRequired(to.pathname, tenantName),
        };
      }

      return to;
    },
    [tenantName, hosting]
  );

  const onNavigate = useCallback(
    (to) => {
      const newURL = appendCurrentURL(
        ensureAbsolutePathsHaveTenantNamePrefixed(to)
      );
      navigate(newURL);
    },
    [appendCurrentURL, ensureAbsolutePathsHaveTenantNamePrefixed, navigate]
  );

  return {
    appendCurrentURL,
    ensureAbsolutePathsHaveTenantNamePrefixed,
    navigate: onNavigate,
  };
};
