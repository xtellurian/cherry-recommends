import React from "react";
import { useAccessToken } from "./token";
import {
  fetchTrackedUsersEventsAsync,
  fetchEventAsync,
} from "../api/eventsApi";

export const useEvent = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token && id) {
      fetchEventAsync({ id, token })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};
export const useUserEvents = ({ commonUserId }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token && commonUserId) {
      fetchTrackedUsersEventsAsync({
        token,
        id: commonUserId,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, commonUserId]);

  return result;
};
