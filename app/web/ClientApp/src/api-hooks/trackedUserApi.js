import React from "react";
import { usePagination } from "../utility/utility";
import { useAccessToken } from "./token";
import {
  fetchTrackedUsersAsync,
  fetchTrackedUserAsync,
  fetchUniqueTrackedUserActionGroupsAsync,
  fetchTrackedUserActionAsync,
  fetchLatestRecommendationsAsync,
  fetchTrackedUsersActionsAsync,
} from "../api/trackedUsersApi";
import { useEnvironment } from "./environmentsApi";

export const useTrackedUsers = ({ searchTerm }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [environment, setEnvironment] = useEnvironment();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchTrackedUsersAsync({
        token,
        page,
        searchTerm,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, page, searchTerm, environment]);

  return result;
};

export const useTrackedUser = ({ id, useInternalId }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTrackedUserAsync({
        token,
        id,
        useInternalId: useInternalId === undefined ? true : useInternalId, // default to true
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useLatestRecommendations = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchLatestRecommendationsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useTrackedUserUniqueActionGroups = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchUniqueTrackedUserActionGroupsAsync({
        token,
        id,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};

export const useTrackedUserAction = ({ id, category, actionName }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTrackedUserActionAsync({
        token,
        id,
        category,
        actionName,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, category, actionName]);

  return result;
};

// pass through page here, because it's unlikely to be paginated via the query string
export const useTrackedUserRevenueActions = ({ id, page }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  if (!page) {
    page = 1;
  }
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTrackedUsersActionsAsync({
        token,
        id,
        page,
        revenueOnly: true,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id, page]);

  return result;
};
