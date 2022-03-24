import React from "react";
import { usePagination } from "../utility/utility";
import { useAccessToken } from "./token";
import { fetchChannelsAsync } from "../api/channelsApi";
import { useEnvironmentReducer } from "./environmentsApi";

export const useChannels = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchChannelsAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, environment]);

  return result;
};
