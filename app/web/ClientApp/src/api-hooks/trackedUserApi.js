import React from "react";
import { useAccessToken } from "./token";
import {
  fetchTrackedUsers,
  fetchSelectedTrackedUsers,
} from "../api/trackedUsersApi";

export const useTrackedUsers = () => {
  const token = useAccessToken();
  const [trackedUsers, setTrackedUsers] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchTrackedUsers({
        success: setTrackedUsers,
        error: console.log,
        token,
      });
    }
  }, [token]);

  return { trackedUsers };
};

export const useSelectedTrackedUsers = ({ ids }) => {
  const token = useAccessToken();
  const [trackedUsers, setTrackedUsers] = React.useState();
  React.useEffect(() => {
    if (token) {
      fetchSelectedTrackedUsers({
        success: setTrackedUsers,
        error: console.log,
        token,
        ids,
      });
    }
  }, [token]);

  return { trackedUsers };
};
