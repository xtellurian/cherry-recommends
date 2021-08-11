import React from "react";
import { useAccessToken } from "./token";
import { fetchUserEvents, fetchEventAsync } from "../api/eventsApi";

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
      fetchUserEvents({
        success: setState,
        error: (error) => setState({ error }),
        token,
        commonUserId,
      });
    }
  }, [token, commonUserId]);

  return result;
};

export const useLatestEvents = () => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token) {
      fetchUserEvents({
        success: setState,
        error: (error) => setState({ error }),
        token,
      });
    }
  }, [token]);

  return result;
};
