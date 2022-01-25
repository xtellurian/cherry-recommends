import React from "react";
import { useAccessToken } from "./token";
import { fetchCustomersEventsAsync, fetchEventAsync } from "../api/eventsApi";

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
export const useCustomerEvents = ({ id }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token && id) {
      fetchCustomersEventsAsync({
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
