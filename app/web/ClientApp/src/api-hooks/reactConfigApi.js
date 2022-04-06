import React from "react";
import {
  fetchAuth0ConfigurationAsync,
  fetchHostingAsync,
} from "../api/reactConfigApi";

export const managementSubdomain = "manage";

export const useAuth0Config = () => {
  const [config, setState] = React.useState();

  React.useEffect(() => {
    fetchAuth0ConfigurationAsync().then(setState).catch(console.error);
  }, []);

  return config;
};

const addIsCanonicalRoot = (s) => {
  s.isCanonicalRoot = window.location.host === s.canonicalRootDomain;
  return s;
};
const addIsWww = (s) => {
  s.isWwwPage = window.location.host.startsWith("www");
  return s;
};
const addIsManagementSubdomain = (s) => {
  s.isManagementSubdomain =
    window.location.host.startsWith(managementSubdomain);
  return s;
};

export const useHosting = () => {
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    fetchHostingAsync()
      .then(addIsCanonicalRoot)
      .then(addIsWww)
      .then(addIsManagementSubdomain)
      .then((s) => {
        setState(s);
      })
      .catch((error) => setState({ error }));
  }, []);
  return result;
};
