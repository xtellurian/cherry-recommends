import React from "react";
import { useAccessToken } from "./token";
import { usePagination } from "../utility/utility";
import { fetchActivityFeedEntitiesAsync } from "../api/activityFeedApi";
import { useEnvironmentReducer } from "./environmentsApi";

export const useActivityFeedEntities = () => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment] = useEnvironmentReducer();
  const [result, setState] = React.useState({ loading: true });

  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchActivityFeedEntitiesAsync({
        token,
        page,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, environment]);

  return result;
};
