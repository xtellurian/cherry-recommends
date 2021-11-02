import React from "react";
import { fetchAuth0ConfigurationAsync } from "../api/reactConfigApi";

export const useAuth0Config = () => {
  const [config, setState] = React.useState();

  React.useEffect(() => {
    fetchAuth0ConfigurationAsync().then(setState).catch(console.log);
  }, []);

  return config;
};
