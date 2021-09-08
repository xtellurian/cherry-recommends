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
export const useUserEvents = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token && id) {
      fetchTrackedUsersEventsAsync({
        token,
        id,
        useInternalId: true,
      })
        .then(setState)
        .catch((error) => setState({ error }));
    }
  }, [token, id]);

  return result;
};
