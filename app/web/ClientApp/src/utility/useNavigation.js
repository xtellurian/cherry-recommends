import { useCallback } from "react";
import { useHistory } from "react-router-dom";
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
  const history = useHistory();
  const hosting = useHosting();
  const { tenantName } = useTenantName();

  const appendCurrentURL = useCallback(
    (to) => {
      if (typeof to === "string") {
        return {
          ...history.location,
          pathname: to,
        };
      }

      if (typeof to === "object" && to !== null) {
        return {
          ...history.location,
          ...to,
        };
      }

      return "";
    },
    [history.location]
  );

  const ensureAbsolutePathsHaveTenantNamePrefixed = useCallback(
    (to) => {
      if (!hosting.multitenant) {
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

  const navigate = useCallback(
    (to) => {
      const newURL = appendCurrentURL(
        ensureAbsolutePathsHaveTenantNamePrefixed(to)
      );
      history.push(newURL);
    },
    [history, appendCurrentURL]
  );

  return {
    appendCurrentURL,
    ensureAbsolutePathsHaveTenantNamePrefixed,
    navigate,
  };
};
