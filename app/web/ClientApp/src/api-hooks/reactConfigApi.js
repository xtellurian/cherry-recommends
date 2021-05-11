import React from "react";
import { fetchAuth0Configuration } from "../api/reactConfigApi";

export const useAuth0Config = () => {
  const [config, setState] = React.useState();

  React.useEffect(() => {
    fetchAuth0Configuration()
      .then(setState)
      .catch(console.log);
  }, []);

  return config;
};
