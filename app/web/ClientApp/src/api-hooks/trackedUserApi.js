import React from "react";
import { usePagination } from "../utility/utility";
import { useAccessToken } from "./token";
import {
  fetchTrackedUsersAsync,
  fetchTrackedUser,
  fetchUniqueTrackedUserActionGroupsAsync,
  fetchTrackedUserActionAsync,
} from "../api/trackedUsersApi";

export const useTrackedUsers = ({ searchTerm }) => {
  const token = useAccessToken();
  const page = usePagination();
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
  }, [token, page, searchTerm]);

  return result;
};

export const useTrackedUser = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTrackedUser({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        id,
      });
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
