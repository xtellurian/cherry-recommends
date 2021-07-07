import React from "react";
import { usePagination } from "../utility/utility";
import { useAccessToken } from "./token";
import {
  fetchTrackedUsers,
  fetchTrackedUser,
  fetchUniqueTrackedUserActions,
  fetchTrackedUserAction,
} from "../api/trackedUsersApi";

export const useTrackedUsers = ({ searchTerm }) => {
  const token = useAccessToken();
  const page = usePagination();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token) {
      fetchTrackedUsers({
        success: setState,
        error: console.log,
        token,
        page,
        searchTerm,
      });
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

export const useTrackedUserUniqueActions = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchUniqueTrackedUserActions({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        id,
      });
    }
  }, [token, id]);

  return result;
};

export const useTrackedUserAction = ({ id, actionName }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({ loading: true });
  React.useEffect(() => {
    setState({ loading: true });
    if (token && id) {
      fetchTrackedUserAction({
        success: setState,
        error: (e) => setState({ error: e }),
        token,
        id,
        actionName,
      });
    }
  }, [token, id, actionName]);

  return result;
};
