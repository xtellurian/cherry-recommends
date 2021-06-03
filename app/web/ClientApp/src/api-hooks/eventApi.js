import React from "react";
import { useAccessToken } from "./token";
import { fetchUserEvents } from "../api/eventsApi";

export const useUserEvents = ({ commonUserId }) => {
  const token = useAccessToken();
  const [result, setState] = React.useState({
    loading: true,
  });
  React.useEffect(() => {
    if (token && commonUserId) {
      fetchUserEvents({
        success: setState,
        error: console.log,
        token,
        commonUserId,
      });
    }
  }, [token, commonUserId]);

  return { result };
};
